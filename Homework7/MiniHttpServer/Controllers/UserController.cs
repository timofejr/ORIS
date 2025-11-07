using System.Net;
using System.Text.Json;
using MiniHttpServer.Framework.Context;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;
using MiniHttpServer.Models;
using MyORMLibrary;

namespace MiniHttpServer.Controllers;

[Controller]
public class UserController: BaseController
{
    
    [HttpGet("/users/")]
    public IResponseResult GetUsers()
    {
        var orm = new ORMContext(GlobalContext.SettingsManager.Settings.ConnectionString);

        var data = new
        {
            Users = new
            {
                Items = orm.ReadByAll<Users>()
            }
        };
        
        return Page("/users/index.html", data);
    }
}