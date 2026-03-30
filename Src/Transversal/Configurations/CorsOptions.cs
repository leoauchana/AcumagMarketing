namespace Transversal.Configurations;

public class CorsOptions
{
    public const string Section = "Cors";
    public string AllowedOrigins { get; set; } = string.Empty;
}