using Grpc.Core.Interceptors;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.Core.Interceptors.Interceptor;

namespace Unary
{
    public class ErrorHandlerInterceptor : Interceptor
    {


        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
           TRequest request,
           ClientInterceptorContext<TRequest, TResponse> context,
           AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleResponse(call.ResponseAsync),
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }



        private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
        {
            try
            {
                var response = await inner;
                 return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Uuupssss an error happened");
                throw;
            }
           // try
            //{
            //    return await inner;
            //}

            //catch (Exception ex)
            //{
            //    Console.WriteLine("Uuupssss an error happened");
            //    throw;
            //    // throw new InvalidOperationException("Uuupssss an error happened", ex);
            //}
        }
    }
}
