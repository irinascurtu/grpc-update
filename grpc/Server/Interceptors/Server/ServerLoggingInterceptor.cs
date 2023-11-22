using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Server.Interceptors.Server
{

    public class ServerLoggingInterceptor : Interceptor
    {
        private readonly ILogger _logger;

        public ServerLoggingInterceptor(ILogger<ServerLoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation($"Starting receiving call. Type: {MethodType.Unary}. " +
          $"Method: {context.Method}.");


            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error thrown by {context.Method}.");
                throw;
            }
        }
    }
}
