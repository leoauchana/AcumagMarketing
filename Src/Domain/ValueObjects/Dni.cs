using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public class Dni
{
    public string Value { get; }
    private Dni(string value)
    {
        Value = value;
    }
    public static Dni Create(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
            throw new ArgumentException("Dni no puede ser vacío.");
        
        if (!Regex.IsMatch(dni, @"^\d{7,8}$"))
            throw new ArgumentException("Dni con formato inválido.");

        return new Dni(dni);
    }
}