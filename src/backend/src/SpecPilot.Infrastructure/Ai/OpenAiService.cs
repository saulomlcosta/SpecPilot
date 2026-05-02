using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Application.Ai.Models;

namespace SpecPilot.Infrastructure.Ai;

public class OpenAiService : IAiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly AiOptions _options;
    private readonly OpenAiPromptRenderer _promptRenderer;

    public OpenAiService(
        HttpClient httpClient,
        IOptions<AiOptions> options,
        OpenAiPromptRenderer promptRenderer)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _promptRenderer = promptRenderer;
    }

    public async Task<GenerateRefinementQuestionsResponse> GenerateRefinementQuestionsAsync(
        GenerateRefinementQuestionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var prompt = await _promptRenderer.RenderRefinementQuestionsPromptAsync(
            request.ProjectName,
            request.InitialDescription,
            request.Goal,
            request.TargetAudience,
            cancellationToken);

        var response = await SendAsync(prompt, cancellationToken);
        var content = ExtractContent(response);

        OpenAiRefinementQuestionsPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<OpenAiRefinementQuestionsPayload>(content, JsonOptions);
        }
        catch (JsonException exception)
        {
            throw new AiProviderException("A resposta da OpenAI nao retornou um JSON valido para perguntas.", exception);
        }

        if (payload?.Questions is null ||
            payload.Questions.Count is < 5 or > 8 ||
            payload.Questions.Any(string.IsNullOrWhiteSpace))
        {
            throw new AiProviderException("A resposta da OpenAI nao atendeu ao formato esperado para perguntas.");
        }

        return new GenerateRefinementQuestionsResponse
        {
            Questions = payload.Questions,
            Metadata = CreateMetadata(response, prompt)
        };
    }

    public async Task<GenerateProjectDocumentResponse> GenerateProjectDocumentAsync(
        GenerateProjectDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        var prompt = await _promptRenderer.RenderProjectDocumentPromptAsync(
            request.ProjectName,
            request.InitialDescription,
            request.Goal,
            request.TargetAudience,
            request.Answers.Select(x => (x.Question, x.Answer)).ToList(),
            cancellationToken);

        var response = await SendAsync(prompt, cancellationToken);
        var content = ExtractContent(response);

        OpenAiProjectDocumentPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<OpenAiProjectDocumentPayload>(content, JsonOptions);
        }
        catch (JsonException exception)
        {
            throw new AiProviderException("A resposta da OpenAI nao retornou um JSON valido para documento.", exception);
        }

        if (payload is null ||
            string.IsNullOrWhiteSpace(payload.Overview) ||
            payload.FunctionalRequirements.Count == 0 ||
            payload.NonFunctionalRequirements.Count == 0 ||
            payload.UseCases.Count == 0 ||
            payload.Risks.Count == 0)
        {
            throw new AiProviderException("A resposta da OpenAI nao atendeu ao formato esperado para documento.");
        }

        return new GenerateProjectDocumentResponse
        {
            Overview = payload.Overview,
            FunctionalRequirements = payload.FunctionalRequirements,
            NonFunctionalRequirements = payload.NonFunctionalRequirements,
            UseCases = payload.UseCases,
            Risks = payload.Risks,
            Metadata = CreateMetadata(response, prompt)
        };
    }

    private async Task<OpenAiChatCompletionResponse> SendAsync(string prompt, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.OpenAi.ApiKey))
        {
            throw new AiProviderException("A configuracao da OpenAI exige uma API key valida.");
        }

        var request = new OpenAiChatCompletionRequest
        {
            Model = _options.OpenAi.Model,
            ResponseFormat = new OpenAiResponseFormat
            {
                Type = "json_object"
            },
            Messages =
            [
                new OpenAiChatMessage
                {
                    Role = "system",
                    Content = "Responda apenas com JSON valido."
                },
                new OpenAiChatMessage
                {
                    Role = "user",
                    Content = prompt
                }
            ]
        };

        using var message = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.OpenAi.ApiKey);

        HttpResponseMessage responseMessage;
        try
        {
            responseMessage = await _httpClient.SendAsync(message, cancellationToken);
        }
        catch (TaskCanceledException exception)
        {
            throw new AiProviderException("A chamada para a OpenAI excedeu o tempo limite configurado.", exception);
        }
        catch (HttpRequestException exception)
        {
            throw new AiProviderException("Falha de rede ao chamar a OpenAI.", exception);
        }

        var rawResponse = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

        if (responseMessage.StatusCode < HttpStatusCode.OK || responseMessage.StatusCode >= HttpStatusCode.MultipleChoices)
        {
            throw new AiProviderException($"A OpenAI retornou HTTP {(int)responseMessage.StatusCode}. Resposta: {rawResponse}");
        }

        OpenAiChatCompletionResponse? response;
        try
        {
            response = JsonSerializer.Deserialize<OpenAiChatCompletionResponse>(rawResponse, JsonOptions);
        }
        catch (JsonException exception)
        {
            throw new AiProviderException("A resposta HTTP da OpenAI nao pode ser desserializada.", exception);
        }

        if (response is null)
        {
            throw new AiProviderException("A OpenAI retornou uma resposta vazia.");
        }

        response.RawJson = rawResponse;
        response.RenderedPrompt = prompt;
        return response;
    }

    private static string ExtractContent(OpenAiChatCompletionResponse response)
    {
        var content = response.Choices.FirstOrDefault()?.Message?.Content;

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new AiProviderException("A OpenAI nao retornou conteudo utilizavel.");
        }

        return content;
    }

    private AiResponseMetadata CreateMetadata(OpenAiChatCompletionResponse response, string prompt)
    {
        var firstChoice = response.Choices.FirstOrDefault();

        return new AiResponseMetadata
        {
            Provider = "OpenAI",
            Model = response.Model ?? _options.OpenAi.Model,
            RenderedPrompt = prompt,
            RawResponse = response.RawJson,
            FinishReason = firstChoice?.FinishReason,
            AdditionalData = new Dictionary<string, string>
            {
                ["promptTokens"] = response.Usage?.PromptTokens?.ToString() ?? string.Empty,
                ["completionTokens"] = response.Usage?.CompletionTokens?.ToString() ?? string.Empty,
                ["totalTokens"] = response.Usage?.TotalTokens?.ToString() ?? string.Empty
            }
        };
    }

    private sealed class OpenAiChatCompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; init; } = string.Empty;
        [JsonPropertyName("messages")]
        public IReadOnlyList<OpenAiChatMessage> Messages { get; init; } = [];
        [JsonPropertyName("response_format")]
        public OpenAiResponseFormat ResponseFormat { get; init; } = new();
    }

    private sealed class OpenAiChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; init; } = string.Empty;
        [JsonPropertyName("content")]
        public string Content { get; init; } = string.Empty;
    }

    private sealed class OpenAiResponseFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; init; } = "json_object";
    }

    private sealed class OpenAiChatCompletionResponse
    {
        [JsonPropertyName("model")]
        public string? Model { get; init; }
        [JsonPropertyName("choices")]
        public List<OpenAiChoice> Choices { get; init; } = [];
        [JsonPropertyName("usage")]
        public OpenAiUsage? Usage { get; init; }
        public string RawJson { get; set; } = string.Empty;
        public string RenderedPrompt { get; set; } = string.Empty;
    }

    private sealed class OpenAiChoice
    {
        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; init; }
        [JsonPropertyName("message")]
        public OpenAiMessage? Message { get; init; }
    }

    private sealed class OpenAiMessage
    {
        [JsonPropertyName("content")]
        public string? Content { get; init; }
    }

    private sealed class OpenAiUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int? PromptTokens { get; init; }
        [JsonPropertyName("completion_tokens")]
        public int? CompletionTokens { get; init; }
        [JsonPropertyName("total_tokens")]
        public int? TotalTokens { get; init; }
    }

    private sealed class OpenAiRefinementQuestionsPayload
    {
        [JsonPropertyName("questions")]
        public List<string> Questions { get; init; } = [];
    }

    private sealed class OpenAiProjectDocumentPayload
    {
        [JsonPropertyName("overview")]
        public string Overview { get; init; } = string.Empty;
        [JsonPropertyName("functionalRequirements")]
        public List<string> FunctionalRequirements { get; init; } = [];
        [JsonPropertyName("nonFunctionalRequirements")]
        public List<string> NonFunctionalRequirements { get; init; } = [];
        [JsonPropertyName("useCases")]
        public List<string> UseCases { get; init; } = [];
        [JsonPropertyName("risks")]
        public List<string> Risks { get; init; } = [];
    }
}
