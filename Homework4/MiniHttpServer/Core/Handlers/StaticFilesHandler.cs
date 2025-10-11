using System.Net;

namespace MiniHttpServer.Core.Handlers;

public class StaticFilesHandler: Handler
{
    
    public override void HandleRequest(HttpListenerRequest request)
    {
        bool isGetMethod = request.HttpMethod.Equals("get", StringComparison.InvariantCultureIgnoreCase);
        bool isRequiredFile = request.Url.IsFile;
         
        if (isGetMethod)
        {
            var buffer = await File.ReadAllBytesAsync(pathToFile);
            response.ContentLength64 = buffer.Length;
            response.ContentType = HeaderByExtension.GetValueOrDefault(fileInfo.Extension, "text/html");
        
            await using Stream output = response.OutputStream;

            if (buffer.Length > 0) await output.WriteAsync(buffer);
            await output.FlushAsync();        }
        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(condition);
        }
    }
}