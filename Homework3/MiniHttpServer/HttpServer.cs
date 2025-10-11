using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MiniHttpServer;

public class HttpServer
{
    private readonly HttpListener _listener = new ();
    private readonly ServerLogger _logger = new ();
    private readonly SettingsModel _settings = SettingsManager.Instance.Settings;

    private static readonly Dictionary<string, string> headerByExtension = new()
    {
        { ".html", "text/html" },
        { ".css", "text/css" },
        { ".js", "application/javascript" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".webp", "image/webp" },
        { ".ico", "image/x-icon" },
        { ".svg", "image/svg+xml" },
        { ".json", "application/json" }
    };

    public HttpServer()
    {
        
    }

    public void Start()
    {
        _listener.Prefixes.Add($"{_settings.Domain}:{_settings.Port}/");
        _listener.Start();
        _logger.ServerStarted();
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        _logger.ServerWaiting();
    }

    private async void ListenerCallback(IAsyncResult result)
    {

        if (!_listener.IsListening) return;
        var context = _listener.EndGetContext(result);

        var response = context.Response;
        var pathToFile = _settings.StaticFilesPath + context.Request.Url?.LocalPath;
        
        // Написать три варианта отправки файла
        if (pathToFile[^1] == '/')
            pathToFile += "index.html";
        
        var fileInfo = new FileInfo(pathToFile);
        var buffer = await File.ReadAllBytesAsync(pathToFile);
        response.ContentLength64 = buffer.Length;
        response.ContentType = headerByExtension.TryGetValue(fileInfo.Extension, out var ext) ? ext : "text/html";
        
        await using Stream output = response.OutputStream;

        if (buffer.Length > 0) await output.WriteAsync(buffer);
        await output.FlushAsync();

        _logger.LogMessage("Запрос обработан");

        Receive();
    }
}
