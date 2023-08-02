using OperationResults;
using VisionPlayground.Shared.Models;

namespace VisionPlayground.BusinessLayer.Services.Interfaces;

public interface ITranslatorService
{
    Task<Result<TranslationResponse>> TranslatorAsync(string sourceText, string targetLanguage);
}