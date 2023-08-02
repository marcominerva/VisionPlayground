using System.Net.Http.Headers;
using System.Text.Json;
using OperationResults;
using VisionPlayground.BusinessLayer.Services.Interfaces;
using VisionPlayground.Shared.Models;

namespace VisionPlayground.BusinessLayer.Services;

public class ImageService : IImageService
{
    private readonly HttpClient httpClient;
    private readonly ITranslatorService translatorService;

    public ImageService(HttpClient httpClient, ITranslatorService translatorService)
    {
        this.httpClient = httpClient;
        this.translatorService = translatorService;
    }

    public async Task<Result<ImageAnalyzeResponse>> AnalyzeAsnyc(Stream stream, string contentType)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "computervision/imageanalysis:analyze?features=caption&model-version=latest&api-version=2023-02-01-preview")
        {
            Content = new StreamContent(stream)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/octet-stream")
                }
            }
        };

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var document = JsonDocument.Parse(contentStream);

        var caption = document.RootElement.GetProperty("captionResult").GetProperty("text").GetString();
        var confidence = document.RootElement.GetProperty("captionResult").GetProperty("confidence").GetSingle();

        var language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
        if (language != "en")
        {
            var translationResponse = await translatorService.TranslatorAsync(caption, language);
            caption = translationResponse.Content.Text;
        }

        var result = new ImageAnalyzeResponse(caption, confidence);
        return result;
    }
}
