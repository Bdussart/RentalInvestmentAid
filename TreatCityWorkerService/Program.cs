using TreatCityWorkerService;


System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "TreatCityInformationWorkerService";
});
builder.Services.AddHostedService<TreatCityInformationWorkerService>();

var host = builder.Build();
host.Run();
