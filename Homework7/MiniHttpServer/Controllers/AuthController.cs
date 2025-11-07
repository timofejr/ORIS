using System.Net;
using System.Text.Json;
using MiniHttpServer.Framework.Core.Abstracts;
using MiniHttpServer.Framework.Core.Attributes;
using MiniHttpServer.Framework.Core.HttpResponse;
using MiniHttpServer.DTOs;
using MiniHttpServer.Framework.Utils;

namespace MiniHttpServer.Controllers;

[Controller]
public class AuthController: BaseController
{
    [HttpGet("/")]
    public IResponseResult MainPage()
    {
        return Page("/auth/index.html", HttpContext);
    }

    [HttpPost("/sendEmail/")]
    public IResponseResult SendEmail()
    {
        if (!HttpContext.Request.HasEntityBody)
            return Json(string.Empty, HttpStatusCode.BadRequest);

        using var reader = new StreamReader(HttpContext.Request.InputStream, HttpContext.Request.ContentEncoding);
        var body = reader.ReadToEnd();

        if (string.IsNullOrEmpty(body))
            return Json(string.Empty, HttpStatusCode.BadRequest);
        
        var emailData = JsonSerializer.Deserialize<SendEmailDto> (body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        var message = $"Ваш email: {emailData.Email}, пароль: {emailData.Password}";
        
        EmailService.SendEmail(emailData.Email, "Данные от хтппсервера", message);
        
        return Json(string.Empty);
    }
}