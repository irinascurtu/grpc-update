using Grpc.Core;
using Grpc.Net.Client;
using Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var channel =  GrpcChannel.ForAddress("http://localhost:5000");
            var client = new Server.Greeter.GreeterClient(channel);


            var cts = new CancellationTokenSource();
            using var streamingCall = client.ServerStream(new Request(), cancellationToken: cts.Token);

            //var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            //using var streamingCall = client.ServerStream(new Server.Request(),
            //                                            deadline: DateTime.UtcNow.AddMilliseconds(1),
            //                                            cancellationToken: cts.Token);

            try
            {
                await foreach (Response response in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    Console.WriteLine($"{response.Message}");
                }

                var trailers = streamingCall.GetTrailers();
                var myValue = trailers.GetValue("my-fake-header");
                Console.WriteLine($"found some trailer values in the gRPC response:{myValue}");
                

            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
            {
                Console.WriteLine("Your greetings timeout'd :)).");
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }

    }
}
