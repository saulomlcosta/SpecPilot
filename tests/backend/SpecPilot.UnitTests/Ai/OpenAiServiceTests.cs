using System.Net;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using SpecPilot.Application.Ai.Models;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.UnitTests.Ai;

public class OpenAiServiceTests
{
    [Fact]
    public async Task Should_parse_refinement_questions_from_openai_response()
    {
        var service = CreateService("""
            {
              "id": "chatcmpl_test",
              "model": "gpt-4.1-mini",
              "choices": [
                {
                  "finish_reason": "stop",
                  "message": {
                    "content": "{\"questions\":[\"Pergunta 1?\",\"Pergunta 2?\",\"Pergunta 3?\",\"Pergunta 4?\",\"Pergunta 5?\"]}"
                  }
                }
              ],
              "usage": {
                "prompt_tokens": 10,
                "completion_tokens": 20,
                "total_tokens": 30
              }
            }
            """);

        var response = await service.GenerateRefinementQuestionsAsync(new GenerateRefinementQuestionsRequest
        {
            ProjectName = "SpecPilot",
            InitialDescription = "Descricao",
            Goal = "Objetivo",
            TargetAudience = "Analistas"
        });

        response.Questions.Should().HaveCount(5);
        response.Metadata.Should().NotBeNull();
        response.Metadata!.Provider.Should().Be("OpenAI");
        response.Metadata.Model.Should().Be("gpt-4.1-mini");
        response.Metadata.RawResponse.Should().Contain("chatcmpl_test");
    }

    [Fact]
    public async Task Should_parse_project_document_from_openai_response()
    {
        var service = CreateService("""
            {
              "id": "chatcmpl_test",
              "model": "gpt-4.1-mini",
              "choices": [
                {
                  "finish_reason": "stop",
                  "message": {
                    "content": "{\"overview\":\"Resumo\",\"functionalRequirements\":[\"RF1\"],\"nonFunctionalRequirements\":[\"RNF1\"],\"useCases\":[\"UC1\"],\"risks\":[\"R1\"]}"
                  }
                }
              ]
            }
            """);

        var response = await service.GenerateProjectDocumentAsync(new GenerateProjectDocumentRequest
        {
            ProjectName = "SpecPilot",
            InitialDescription = "Descricao",
            Goal = "Objetivo",
            TargetAudience = "Analistas",
            Answers =
            [
                new RefinementAnswerInput
                {
                    Question = "Pergunta",
                    Answer = "Resposta"
                }
            ]
        });

        response.Overview.Should().Be("Resumo");
        response.FunctionalRequirements.Should().ContainSingle("RF1");
        response.NonFunctionalRequirements.Should().ContainSingle("RNF1");
        response.UseCases.Should().ContainSingle("UC1");
        response.Risks.Should().ContainSingle("R1");
    }

    [Fact]
    public async Task Should_throw_controlled_exception_when_http_status_is_not_success()
    {
        var service = CreateService("{\"error\":\"invalid\"}", HttpStatusCode.BadGateway);

        var act = () => service.GenerateRefinementQuestionsAsync(new GenerateRefinementQuestionsRequest
        {
            ProjectName = "SpecPilot",
            InitialDescription = "Descricao",
            Goal = "Objetivo",
            TargetAudience = "Analistas"
        });

        await act.Should().ThrowAsync<AiProviderException>()
            .WithMessage("*OpenAI*");
    }

    [Fact]
    public async Task Should_throw_controlled_exception_when_json_is_invalid()
    {
        var service = CreateService("""
            {
              "id": "chatcmpl_test",
              "model": "gpt-4.1-mini",
              "choices": [
                {
                  "finish_reason": "stop",
                  "message": {
                    "content": "nao-e-json"
                  }
                }
              ]
            }
            """);

        var act = () => service.GenerateRefinementQuestionsAsync(new GenerateRefinementQuestionsRequest
        {
            ProjectName = "SpecPilot",
            InitialDescription = "Descricao",
            Goal = "Objetivo",
            TargetAudience = "Analistas"
        });

        await act.Should().ThrowAsync<AiProviderException>()
            .WithMessage("*JSON*");
    }

    private static OpenAiService CreateService(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var promptsRoot = CreatePromptsRoot();
        var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
        });

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.openai.com/")
        };

        return new OpenAiService(
            httpClient,
            Options.Create(new AiOptions
            {
                Provider = "OpenAI",
                PromptsRoot = promptsRoot,
                OpenAi = new OpenAiOptions
                {
                    ApiKey = "test-key",
                    Model = "gpt-4.1-mini"
                }
            }),
            new OpenAiPromptRenderer(Options.Create(new AiOptions
            {
                PromptsRoot = promptsRoot
            })));
    }

    private static string CreatePromptsRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "specpilot-prompts", Guid.NewGuid().ToString("N"));
        var runtime = Path.Combine(root, "runtime");
        Directory.CreateDirectory(runtime);

        File.WriteAllText(Path.Combine(runtime, "generate-refinement-questions.costar.md"), "Projeto: {{ProjectName}}");
        File.WriteAllText(Path.Combine(runtime, "generate-project-document.costar.md"), "Projeto: {{ProjectName}} {{RefinementAnswers}}");

        return root;
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(request));
        }
    }
}
