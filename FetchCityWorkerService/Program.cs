using FetchCityWorkerService;

System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "FetchCityInformation";
});
builder.Services.AddHostedService<FetchCityInformationWorkerService>();

var host = builder.Build();
host.Run();
