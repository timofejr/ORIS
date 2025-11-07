using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;

namespace MiniHttpServer.Controllers;

[Controller]
public class BonxController: BaseController
{
    [HttpGet("/bonx/")]
    public IResponseResult MainPage()
    {
        return Page("/bonx/index.html", HttpContext);
    }
}