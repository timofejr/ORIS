using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MiniHttpServer;

public class HttpServer
{
    private readonly HttpListener _listener = new ();
    private readonly ServerLogger _logger = new ();
    private ServerSettings _serverSettings;

    public HttpServer()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        try
        {
            var settingsFile = File.ReadAllText("settings.json");
            _serverSettings = JsonSerializer.Deserialize<ServerSettings>(settingsFile)
                ?? throw new InvalidOperationException("Десериализация провалилась");
            
            if (string.IsNullOrEmpty(_serverSettings.StaticFilesPath))
                throw new InvalidOperationException("Поле 'StaticFilesPath' не было заполнено из settings.json");
        
            if (string.IsNullOrEmpty(_serverSettings.Domain))
                throw new InvalidOperationException("Поле 'Domain' не было заполнено из settings.json");
        
            if (string.IsNullOrEmpty(_serverSettings.Port))
                throw new InvalidOperationException("Поле 'Port' не было заполнено из settings.json");
        
            _logger.LogMessage("Настройки упешно загружены");
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException("Файл settings.json не был найден");
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new DirectoryNotFoundException("Директория с файлом settings.json не была найдена");
        }
    }

    public void Start()
    {
        _listener.Prefixes.Add($"{_serverSettings.Domain}:{_serverSettings.Port}/");
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
        var responseText = await File.ReadAllTextAsync("static/index.html");

        if (!_listener.IsListening) return;
        var context = _listener.EndGetContext(result);

        var response = context.Response;

        var buffer = Encoding.UTF8.GetBytes(responseText);

        response.ContentLength64 = buffer.Length;
        await using Stream output = response.OutputStream;

        await output.WriteAsync(buffer);
        await output.FlushAsync();

        _logger.LogMessage("Запрос обработан");

        Receive();
    }
}
