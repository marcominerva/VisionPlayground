using VisionPlayground.BusinessLayer.Services.Interfaces;
using VisionPlayground.Shared.Models;
using MinimalHelpers.Routing;
using OperationResults.AspNetCore.Http;

namespace VisionPlayground.Endpoints;

public class ImagesEndpoints : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var imageApi = endpoints.MapGroup("/api/images");

        imageApi.MapPost(string.Empty, UploadImageAsync)
            .WithName("UploadImage")
            .Produces<ImageAnalyzeResponse>()
            .ProducesValidationProblem()
            .WithOpenApi();
    }

    public async Task<IResult> UploadImageAsync(IFormFile file, IImageService imageService, HttpContext httpContext)
    {
        var result = await imageService.AnalyzeAsnyc(file.OpenReadStream(), file.ContentType);

        var response = httpContext.CreateResponse(result);
        return response;
    }
}