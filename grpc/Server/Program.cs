using Grpc.Net.Compression;
using Server.Interceptors.Server;
using Server.Services;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
//builder.Services.AddGrpc();

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    // options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    //   options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
    options.ResponseCompressionLevel = CompressionLevel.Optimal;
    options.ResponseCompressionAlgorithm = "gzip";
    options.CompressionProviders = new List<ICompressionProvider>()
    {
        //brotli or your own
    };

    options.Interceptors.Add<ServerLoggingInterceptor>();
    // options.Interceptors
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
