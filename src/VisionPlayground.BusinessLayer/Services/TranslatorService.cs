using System.Net.Http.Json;
using System.Text.Json;
using OperationResults;
using VisionPlayground.BusinessLayer.Services.Interfaces;
using VisionPlayground.Shared.Models;

namespace VisionPlayground.BusinessLayer.Services;

public class TranslatorService(HttpClient httpClient) : ITranslatorService
{
    public async Task<Result<TranslationResponse>> TranslatorAsync(string sourceText, string targetLanguage)
    {
        var requestUri = $"translate?api-version=3.0&from=en&to={targetLanguage}";
        using var response = await httpClient.PostAsJsonAsync(requestUri, new[] { new { Text = sourceText } });
        response.EnsureSuccessStatusCode();

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var document = JsonDocument.Parse(contentStream);

        var translation = document.RootElement[0].GetProperty("translations")[0].GetProperty("text").GetString();

        var result = new TranslationResponse(translation);
        return result;
    }
}
