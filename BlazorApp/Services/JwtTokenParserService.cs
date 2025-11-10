using System.Text;
using System.Text.Json;

namespace BlazorApp.Services;

public class JwtTokenParserService
{
    /// <summary>
    /// Extrae el valor de una claim específica del payload de un JWT.
    /// </summary>
    /// <param name="token">El token JWT (cadena completa)</param>
    /// <param name="claimName">Nombre de la claim a extraer (ej: "sub", "user_id")</param>
    /// <returns>Valor de la claim como string, o null si no se encuentra</returns>
    public string? GetClaimValue(string token, string claimName)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
                return null;

            var payload = parts[1];
            payload = AddBase64Padding(payload);
            var jsonBytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(jsonBytes);

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty(claimName, out var element))
            {
                return element.GetString();
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Intenta extraer un valor entero de una claim.
    /// </summary>
    public int? GetClaimAsInt(string token, string claimName)
    {
        var value = GetClaimValue(token, claimName);
        return int.TryParse(value, out int result) ? result : null;
    }

    private static string AddBase64Padding(string input)
    {
        var padding = (4 - input.Length % 4) % 4;
        return input.PadRight(input.Length + padding, '=');
    }
}