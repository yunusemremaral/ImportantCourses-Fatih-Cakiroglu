using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        private static int connectedClientCount = 0;
        public async Task BroadcastMessageToAllClient(string message) // js den cagırdıgımız metodumuz 
        {
            Clients.All.ReceiveMessageForAllClient(message);

           // await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        }

        public async Task BroadcastMessageToCallerClient(string message)  
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);

        }

        public override async Task OnConnectedAsync()
        {
            connectedClientCount++;
            await Clients.All.ReceiveConnectedClientCount(connectedClientCount);
           await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            connectedClientCount--;
            await Clients.All.ReceiveConnectedClientCount(connectedClientCount);
           await base.OnDisconnectedAsync(exception);
        }

    }
}
