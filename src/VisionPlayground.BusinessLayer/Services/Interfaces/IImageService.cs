using VisionPlayground.Shared.Models;
using OperationResults;

namespace VisionPlayground.BusinessLayer.Services.Interfaces;

public interface IImageService
{
    Task<Result<ImageAnalyzeResponse>> AnalyzeAsnyc(Stream stream, string contentType);
}