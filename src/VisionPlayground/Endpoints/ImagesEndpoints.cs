using OperationResults.AspNetCore.Http;
using VisionPlayground.BusinessLayer.Services.Interfaces;
using VisionPlayground.Shared.Models;

namespace VisionPlayground.Endpoints;

public class ImagesEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var imageApi = endpoints.MapGroup("/api/images");

        imageApi.MapPost(string.Empty, UploadImageAsync)
            .WithName("UploadImage")
            .Produces<ImageAnalyzeResponse>()
            .ProducesValidationProblem()
            .DisableAntiforgery()
            .WithOpenApi();
    }

    public static async Task<IResult> UploadImageAsync(IFormFile file, IImageService imageService, HttpContext httpContext)
    {
        var result = await imageService.AnalyzeAsnyc(file.OpenReadStream(), file.ContentType);

        var response = httpContext.CreateResponse(result);
        return response;
    }
}