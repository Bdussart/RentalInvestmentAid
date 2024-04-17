using FetchAnnoucementWorkerService;

System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "FetchAnnoucementInformation";
});
builder.Services.AddHostedService<AnnouncementWorker>();

var host = builder.Build();
host.Run();
