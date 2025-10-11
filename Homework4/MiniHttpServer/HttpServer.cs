using System;
using System.Net;
using System.Text;
using System.Text.Json;
using MiniHttpServer.Core.Handlers;

namespace MiniHttpServer;

public class HttpServer
{
    private readonly HttpListener _listener = new ();
    private readonly ServerLogger _logger = new ();
    private readonly SettingsModel _settings = SettingsManager.Instance.Settings;

    private static readonly Dictionary<string, string> HeaderByExtension = new()
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
        
        Handler h1 = new StaticFilesHandler();
        Handler h2 = new FindMethodsHandler();
        h1.Successor = h2;
        h1.HandleRequest(2);
        
        var response = context.Response;
        var request = context.Request;

        if (request.HttpMethod.Equals("get", StringComparison.InvariantCultureIgnoreCase))
        {
                    
            var pathToFile = _settings.StaticFilesPath + context.Request.Url?.LocalPath;
        
            // Написать три варианта отправки файла
            if (pathToFile[^1] == '/')
                pathToFile += "index.html";
        
            var fileInfo = new FileInfo(pathToFile);

            try
            {
                var buffer = await File.ReadAllBytesAsync(pathToFile);
                response.ContentLength64 = buffer.Length;
                response.ContentType = HeaderByExtension.GetValueOrDefault(fileInfo.Extension, "text/html");
        
                await using Stream output = response.OutputStream;

                if (buffer.Length > 0) await output.WriteAsync(buffer);
                await output.FlushAsync();

                _logger.LogMessage("Запрос обработан");

                Receive();
            }
            catch (FileNotFoundException e)
            {
                response.StatusCode = 404;
                await using Stream output = response.OutputStream;
            
                await output.FlushAsync();

                _logger.LogMessage("Запрос обработан, 404");

                Receive();
            }
        } else if (context.Request.HttpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase))
        {
            switch (request.Url.AbsolutePath)
            {
                case "/sendEmail":
                    break;
                default:
                    break;
            }
        }
    }
}
