using System;

namespace MiniHttpServer;

public class ServerLogger
{
    public void LogMessage(string message)
    {
        Console.WriteLine($"Log: {message}");
    }

    public void ServerStarted()
    {
        Console.WriteLine("Log: Сервер запустился");
    }

    public void ServerWaiting()
    {
        Console.WriteLine("Log: Сервер ожидает");
    }

    public void ServerStopped()
    {
        Console.WriteLine("Сервер завершил работу");
    }
}
