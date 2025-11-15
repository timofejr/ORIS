using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;

namespace MiniHttpServer.Controllers;

[Controller]
public class MigrationsController: BaseController
{
    [HttpGet("/migrate/create")]
    public IResponseResult CreateMigration()
    {
        var migrationLibrary = new MigrationLibrary.MigrationLibrary(Environment.GetEnvironmentVariable("ConnectionString"));
        migrationLibrary.CreateMigration();
        return Json("фыв");
    }
    
    [HttpGet("/migrate/apply")]
    public IResponseResult ApplyMigration()
    {
        var migrationLibrary = new MigrationLibrary.MigrationLibrary(Environment.GetEnvironmentVariable("ConnectionString"));
        migrationLibrary.ApplyMigration();
        return Json("фыв");
    }
    
    [HttpGet("/migrate/rollback")]
    public IResponseResult RollbackMigration()
    {
        return Json("фыв");
    }
    
    [HttpGet("/migrate/status")]
    public IResponseResult MigrationStatus()
    {
        return Json("фыв");
    }
    
    [HttpGet("/migrate/log ")]
    public IResponseResult LogMigration()
    {
        return Json("фыв");
    }
}