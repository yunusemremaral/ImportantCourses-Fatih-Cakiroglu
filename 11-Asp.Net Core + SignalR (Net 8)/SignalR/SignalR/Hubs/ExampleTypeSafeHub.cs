using Microsoft.AspNetCore.SignalR;
using SignalR.Models;

namespace SignalR.Hubs
{
    public class ExampleTypeSafeHub:Hub<IExampleTypeSafeHub>
    {
        private static int connectedClientCount = 0;
        public async Task BroadcastMessageToAllClient(string message) // js den cagırdıgımız metodumuz 
        {
            await Clients.All.ReceiveMessageForAllClient(message);

           // await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        }
        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {


            await foreach (var name in nameAsChunks)
            {

                await Task.Delay(1000);
                await Clients.All.ReceiveMessageAsStreamForAllClient(name);
            }

        }

        public async Task BroadcastStreamProductToAllClient(IAsyncEnumerable<Product> productAsChunks)
        {


            await foreach (var product in productAsChunks)
            {

                await Task.Delay(1000);
                await Clients.All.ReceiveProductAsStreamForAllClient(product);
            }

        }

        public async IAsyncEnumerable<string> BroadCastFromHubToClient(int count)
        {

            foreach (var item in Enumerable.Range(1, count).ToList())
            {
                await Task.Delay(1000);
                yield return $"{item}. data";
            }




        }
        public async Task BroadcastTypedMessageToAllClient(Product product) // js den cagırdıgımız metodumuz 
        {
            Clients.All.ReceiveTypedMessageForAllClient(product);

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
