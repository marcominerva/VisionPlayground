using System.Net.Http.Headers;
using System.Text.Json;
using VisionPlayground.BusinessLayer.Services.Interfaces;
using VisionPlayground.Shared.Models;
using OperationResults;

namespace VisionPlayground.BusinessLayer.Services;

public class ImageService : IImageService
{
    private readonly HttpClient httpClient;

    public ImageService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Result<ImageAnalyzeResponse>> AnalyzeAsnyc(Stream stream, string contentType)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "computervision/imageanalysis:analyze?features=caption,denseCaptions&model-version=latest&api-version=2023-02-01-preview")
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

        var text = document.RootElement.GetProperty("captionResult").GetProperty("text").GetString();
        var confidence = document.RootElement.GetProperty("captionResult").GetProperty("confidence").GetSingle();

        var result = new ImageAnalyzeResponse(text, confidence);
        return result;
    }
}
