using RentWorkerService;

var builder = Host.CreateApplicationBuilder(args);
System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "RentCalculWorkerService";
});
builder.Services.AddHostedService<RentCalculWorkerService>();
var host = builder.Build();
host.Run();


