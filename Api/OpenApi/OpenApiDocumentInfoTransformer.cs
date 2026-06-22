using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Subscription_Control_Backend.Api.OpenApi;

/// <summary>
/// Reichert das generierte OpenAPI-Dokument um beschreibende Metadaten an
/// (Titel, Version, Beschreibung, Kontakt).
/// </summary>
public class OpenApiDocumentInfoTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Subscription Control API",
            Version = "v1",
            Description =
                "REST-API zur Verwaltung von Abonnements, Kategorien, Abrechnungszyklen, "
                + "Benachrichtigungen und Benutzern. Alle Antworten sind in ein einheitliches "
                + "ApiResponse-Objekt (success, data, message, errorMessage) gehüllt.",
            Contact = new OpenApiContact
            {
                Name = "IU Internationale Hochschule Augsburg",
                Url = new Uri("https://github.com/IU-Internationale-Hochschule-Augsburg")
            }
        };

        return Task.CompletedTask;
    }
}