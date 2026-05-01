namespace SpecPilot.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "SpecPilot";
    public string Audience { get; set; } = "SpecPilot";
    public string SecretKey { get; set; } = "ChangeThisSecretKeyInFutureStages123!";
    public int ExpirationMinutes { get; set; } = 60;
}
