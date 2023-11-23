using Grpc.Core;
using Grpc.Net.Client;
using Server;

#region interceptor
//using var channel = GrpcChannel.ForAddress("https://localhost:5000");
//var invoker = channel.Intercept(new ErrorHandlerInterceptor());
//var client = new Greeter.GreeterClient(invoker);
#endregion

GrpcChannel channel =  GrpcChannel.ForAddress("http://localhost:5000");
var client = new Greeter.GreeterClient(channel);
var cts = new CancellationTokenSource();

Request request = new Request() { ContentValue = "Hello Update Conference!" };

Console.WriteLine($"sending: {request.ContentValue}");

//options
var response = client.SayHello(request, options: new CallOptions() { });

var metadata = new Metadata();
metadata.Add(new Metadata.Entry("first-key", "first-key-value"));
metadata.Add(new Metadata.Entry("secondkey", "second-key-value"));

//var response = await client.SayHelloAsync(
//        request,
//        headers: metadata,
//        deadline: DateTime.UtcNow.AddMilliseconds(1),
//        cancellationToken: cts.Token);


Console.WriteLine(response.Message);