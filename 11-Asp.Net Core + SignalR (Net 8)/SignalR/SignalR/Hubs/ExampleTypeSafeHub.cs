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

        public async Task BroadcastMessageToOtherClient(string message)
        {
            await Clients.Others.ReceiveMessageForOtherClient(message);

        }
        public async Task BroadcastMessageToSpesificClient(string connectionid ,string message)
        {
            await Clients.Client(connectionid).ReceiveMessageForSpesificClient(message);

        }

        public async Task BroadcastMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroup(message);
        }

        // ✅ Gruba katıl
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        // ✅ Gruptan çık
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
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
