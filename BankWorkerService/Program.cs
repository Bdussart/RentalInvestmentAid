using BankWorkerService;


System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(option =>
{
    option.ServiceName = "BankWorkerService";
});
builder.Services.AddHostedService<BankInformationWorkerService>();

var host = builder.Build();
host.Run();
