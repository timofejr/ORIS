using System.Net;
using System.Text;
using MiniHttpServer.Context;
using MiniHttpServer.Core.Abstracts;
using MiniHttpServer.Settings;
using MiniHttpServer.Utils;

namespace MiniHttpServer.Core.Handlers;

public class StaticFilesHandler : Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var isGetMethod = request.HttpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase);
        var isStaticFile = request.Url.AbsolutePath.Split('/').Any(x=> x.Contains("."));
        
        if (isGetMethod && isStaticFile)
        {
            var response = context.Response;
            
            string path = request.Url.AbsolutePath.Trim('/');
            
            GlobalContext.Server.SendStaticResponse(context, HttpStatusCode.OK, path);

        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}