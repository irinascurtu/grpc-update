// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Server;

var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 2,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.Internal }
    },

    //HedgingPolicy = new HedgingPolicy
    //{
    //    HedgingDelay = TimeSpan.FromSeconds(30),
    //    MaxAttempts = 4,
    //    NonFatalStatusCodes = { StatusCode.Unavailable, }
    //}
};

Request request = new Request() { ContentValue = "Update-Conference-Prague" };

#region ClientSide Load Balancing
var factory = new StaticResolverFactory(addr => new[]
{
    new BalancerAddress("localhost", 5000),
    new BalancerAddress("localhost",5002)
});

var services = new ServiceCollection();
services.AddSingleton<ResolverFactory>(factory);

var channel = GrpcChannel.ForAddress(
    "static://localhost",
    new GrpcChannelOptions
    {
        Credentials = ChannelCredentials.Insecure,
        ServiceConfig = new ServiceConfig
        {
            LoadBalancingConfigs = { new RoundRobinConfig() }
        },
        ServiceProvider = services.BuildServiceProvider()
    });


var client = new Greeter.GreeterClient(channel);
var response = client.SayHello(request);
Console.WriteLine($"The reply is: {response.Message}");


#endregion

#region retry

//var retryPolicy = new MethodConfig
//{
//    Names = { MethodName.Default },
//    RetryPolicy = new RetryPolicy
//    {
//        MaxAttempts = 5,
//        InitialBackoff = TimeSpan.FromSeconds(1),
//        MaxBackoff = TimeSpan.FromSeconds(0.5),
//        BackoffMultiplier = 1,
//        RetryableStatusCodes = { StatusCode.Internal }
//    }
//};


//var options = new GrpcChannelOptions()
//{
//    ServiceConfig = new ServiceConfig()
//    {
//        MethodConfigs = { retryPolicy }
//    }

//};


//var cts = new CancellationTokenSource();
//using var channelWithRetry = GrpcChannel.ForAddress("http://localhost:5000", options);
//var clientWithRetry = new Greeter.GreeterClient(channelWithRetry);
//var responseWithRetry = clientWithRetry.SayHello(request);
//Console.WriteLine(responseWithRetry);

#endregion
Console.ReadLine();