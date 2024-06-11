using TreatAnnouncementWorkerService;

System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "TreatAnnoucementInformation";
});
builder.Services.AddHostedService<TreatAnnouncementWoker>();

var host = builder.Build();
host.Run();
