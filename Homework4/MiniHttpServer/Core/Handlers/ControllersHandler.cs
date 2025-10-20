using System.Net;
using System.Reflection;
using MiniHttpServer.Context;
using MiniHttpServer.Core.Abstracts;
using MiniHttpServer.Core.Attributes;
using MiniHttpServer.Utils;

namespace MiniHttpServer.Core.Handlers;

public class ControllersHandler: Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        if (true)
        {
            var request = context.Request;
            var endpointPath = request.Url?.AbsolutePath;
            
            
            // TODO: сделать так чтобы слеши по краям не влияли ни на что
            if (!GlobalContext.Endpoints.TryGetValue((endpointPath, request.HttpMethod), out var endpoint))
            {
                GlobalContext.Server.Send404Response(context, endpointPath);
                return;
            }

            if (endpoint.Item1 is not null)
            {
                endpoint.Item2.Invoke(Activator.CreateInstance(endpoint.Item1), [context]);
            }
            else
                GlobalContext.Server.Send404Response(context, endpointPath);
            
        }

        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}