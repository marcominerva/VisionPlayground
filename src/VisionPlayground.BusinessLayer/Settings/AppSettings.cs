namespace VisionPlayground.BusinessLayer.Settings;

public class AppSettings
{
    public string ApplicationName { get; init; } = "ChatGPT Playground";

    public string ApplicationDescription { get; init; } = "Chat using AI";

    public int MaxUploadSize { get; init; }

    public string VisionEndpoint { get; init; }

    public string VisionApiKey { get; init; }
}
