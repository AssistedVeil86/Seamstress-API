using System;
using System.Reflection;

namespace Seamstress.VSA.Infrastructure.Extensions;

public static class EmbeddedResourceProvider
{
    public static byte[] GetLogoBytes() => _logoBytes.Value;
    private static readonly Lazy<byte[]> _logoBytes =
        new(() => ReadResource("Seamstress.VSA.Resources.logoConta.png"));
        
    private static byte[] ReadResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException(
                $"Recurso embebido '{resourceName}' no encontrado. " +
                $"Verifica que el archivo esté marcado como EmbeddedResource en el .csproj. " +
                $"Recursos disponibles: {string.Join(", ", assembly.GetManifestResourceNames())}");

        using var memorySteam = new MemoryStream();
        stream.CopyTo(memorySteam);
        return memorySteam.ToArray();
    }

}
