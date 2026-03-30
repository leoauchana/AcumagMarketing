using System.ComponentModel.DataAnnotations;

namespace Transversal.Configurations;

public class JwtOptions
{
    public const string Section = "Jwt";
    [Required]
    public string Issuer { get; set; } = string.Empty;
    [Required]
    public string Audience { get; set; }  = string.Empty;
    [Required]
    public string ExpiresMinutes { get; set; } =  string.Empty;
    [Required]
    public string Key { get; set; }  = string.Empty;
}