using MiniHttpServer;

// 0. Добавить логирование работы сервера(writeline)
// 1. В папке static искать index.html. Если его нет не запускать сервервер и выводить сообщение в консоль.
// 2. В файл settings вытащить настройки сервера, а именно: путь к статическим файлам (StaticDirectoryPath),
// Domain, Port
// 3. Брать настройки из файла settings.json. Если файла нет или ошибка в настройках, выводить сообщение и не запускать сервер.
// 4. Обрабатывать более одного запроса
// 5. Необходимо остановить сервер  при написанной команде "/stop"

var logger = new ServerLogger();

try
{
    var httpServer = new HttpServer();

    httpServer.Start();

    while (true)
    {
        var input = await Task.Run(Console.ReadLine);
        if (input.ToLower().Equals("/stop"))
            break;
    }

    httpServer.Stop();
}
catch (Exception ex)
{
    logger.LogMessage(ex.Message);
}   
finally
{
    logger.ServerStopped();
}
