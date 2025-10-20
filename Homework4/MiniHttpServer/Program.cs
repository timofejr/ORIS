using System.Reflection;
using MiniHttpServer;
using MiniHttpServer.Utils;
using MiniHttpServer.Context;
using MiniHttpServer.Settings;

try
{   
    EndpointsRegistry.LoadEndpoints(Assembly.GetExecutingAssembly());
    
    GlobalContext.Server = new HttpServer();
    GlobalContext.Logger = Logger.Instance;
    GlobalContext.SettingsManager = SettingsManager.Instance;
    GlobalContext.Endpoints = EndpointsRegistry.LoadEndpoints(Assembly.GetExecutingAssembly());

    GlobalContext.Server.Start();

    while (true)
    {
        var input = await Task.Run(Console.ReadLine);
        if (input.ToLower().Equals("/stop"))
            break;
    }

    GlobalContext.Server.Stop();
}
catch (Exception ex)
{
    GlobalContext.Logger.LogMessage(ex.Message);
}   
finally
{
    GlobalContext.Logger.ServerStopped();
}