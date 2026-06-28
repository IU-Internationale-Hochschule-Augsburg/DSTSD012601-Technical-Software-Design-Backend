using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Subscription_Control_Backend.Api.OpenApi;

/// <summary>
/// Fügt dem OpenAPI-Dokument das JWT-Bearer-Security-Scheme hinzu, sofern die Bearer-Authentifizierung
/// registriert ist – damit Swagger-UI einen "Authorize"-Button bietet.
/// </summary>
public class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider schemeProvider)
    {
        _schemeProvider = schemeProvider;
    }

    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var schemes = await _schemeProvider.GetAllSchemesAsync();
        if (schemes.All(s => s.Name != "Bearer"))
        {
            return;
        }

        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT-Access-Token aus dem Login eingeben (nur das Token, ohne 'Bearer '-Präfix)."
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = scheme;

        var reference = new OpenApiSecuritySchemeReference("Bearer", document);
        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [reference] = new List<string>()
        });
    }
}