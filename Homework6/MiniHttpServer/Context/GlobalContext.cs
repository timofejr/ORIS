using System.Reflection;
using MiniHttpServer.Settings;
using MiniHttpServer.Utils;

namespace MiniHttpServer.Context;

public class GlobalContext
{
    public static HttpServer Server { get; set; }
    public static Logger Logger { get; set; }
    public static SettingsManager SettingsManager { get; set; }
    public static Dictionary<(string, string), (Type, MethodInfo)> Endpoints { get; set; }
}