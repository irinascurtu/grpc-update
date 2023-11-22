
using Grpc.Core;
using Grpc.Net.Client;
using Server;

var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = new Greeter.GreeterClient(channel);

try
{
    using var call = client.ClientStream();

    for (var i = 0; i < 10000; i++)
    {

        await call.RequestStream.WriteAsync(new Request { ContentValue = i.ToString() });
    }

    await call.RequestStream.CompleteAsync();
    Response response = await call;

    Console.WriteLine($"{response.Message} is the last value server received");
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}
