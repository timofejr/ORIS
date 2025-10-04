using System;
using System.ComponentModel.DataAnnotations;

namespace MiniHttpServer;

public class ServerSettings
{
    public string StaticFilesPath { get; init; }

    public string Domain { get; init; }

    public string Port { get; init; }

    public ServerSettings(string staticFilesPath, string domain, string port)
    {
        StaticFilesPath = staticFilesPath;
        Domain = domain;
        Port = port;
    }
}
