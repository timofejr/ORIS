using System.Net;
using MiniHttpServer.Context;
using MiniHttpServer.Core.Attributes;

namespace MiniHttpServer.Controllers;

[Controller]
public class BonxController
{
    [HttpGet("/bonx/")]
    public void MainPage(HttpListenerContext context)
    {
        GlobalContext.Server.SendStaticResponse(context,  HttpStatusCode.OK, GlobalContext.SettingsManager.Settings.StaticFilesPath + "/bonx/index.html");
    }
}