namespace MiniHttpServer.Settings;

public class SettingsModel
{
    public string StaticFilesPath { get; init; }
    public string Domain { get; init; }
    public string Port { get; init; }
    public string EmailAddressFrom { get; init; }
    public string EmailNameFrom { get; init; }
    public int SmtpPort { get; init; }
    public string SmtpHost { get; init; }
    public string SmtpPassword { get; init; }
}