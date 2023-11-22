start 2 server instances
``` dotnet run --urls="http://localhost:5002" ```
navigate to Extragrpc project and run it several times: ``` dotnet run ```
you should see the response served from both instances using round robin
