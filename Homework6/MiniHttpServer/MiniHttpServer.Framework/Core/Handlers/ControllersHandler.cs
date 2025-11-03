using System.Net;
using System.Reflection;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;
using MiniHttpServer.Framework.Utils;

namespace MiniHttpServer.Framework.Core.Handlers;

public class ControllersHandler: Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        if (true)
        {
            var request = context.Request;
            var endpointPath = request.Url?.AbsolutePath;
            
            // TODO: сделать так чтобы слеши по краям не влияли ни на что
            if (!GlobalContext.Endpoints.TryGetValue((endpointPath, request.HttpMethod), out var controllerAndEndpoint))
            {
                GlobalContext.Server.Send404Response(context, endpointPath);
                return;
            }
            
            if (controllerAndEndpoint.Item1 is not null && controllerAndEndpoint.Item2 is not null)
            {
                var controllerInstance = Activator.CreateInstance(controllerAndEndpoint.Item1);
                (controllerInstance as BaseController)?.SetContext(context);
                var endpointMethod = controllerAndEndpoint.Item2;
                
                var result = endpointMethod.Invoke(controllerInstance, null);

                var resultString = (result as IResponseResult)?.Execute(context);
                var resultStatusCode = (result as IResponseResult).StatusCode;

                if (result is PageResult)
                {
                    GlobalContext.Server.SendPageResponse(context, resultStatusCode, resultString);
                }
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