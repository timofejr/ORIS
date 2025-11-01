using System.Net;
using System.Text.Json;
using MiniHttpServer.Context;
using MiniHttpServer.Core.Attributes;
using MiniHttpServer.DTOs;
using MiniHttpServer.Utils;

namespace MiniHttpServer.Controllers;

[Controller]
public class AuthController
{
    [HttpGet("/")]
    public void MainPage(HttpListenerContext context)
    {
        GlobalContext.Server.SendStaticResponse(context,  HttpStatusCode.OK, GlobalContext.SettingsManager.Settings.StaticFilesPath + "/auth/index.html");
    }

    [HttpPost("/sendEmail/")]   
    public void SendEmail(HttpListenerContext context)
    {
        if (!context.Request.HasEntityBody)
        {
            GlobalContext.Server.SendJsonResponse(context, HttpStatusCode.BadRequest);
            return;
        }

        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        var body =  reader.ReadToEnd();

        if (string.IsNullOrEmpty(body))
        {
            GlobalContext.Server.SendJsonResponse(context, HttpStatusCode.BadRequest);
            return;
        }
        
        var emailData = JsonSerializer.Deserialize<SendEmailDto> (body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        var message = $"Ваш email: {emailData.Email}, пароль: {emailData.Password}";
        
        EmailService.SendEmail(emailData.Email, "Данные от хтппсервера", message);
        
        GlobalContext.Server.SendJsonResponse(context, HttpStatusCode.OK);
    }
}