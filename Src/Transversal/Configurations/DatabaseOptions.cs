namespace Transversal.Configurations;

public class DatabaseOptions
{
    public const string Section = "DatabaseConnection";
    public string SqlConnectionString { get; set; } = string.Empty;
}