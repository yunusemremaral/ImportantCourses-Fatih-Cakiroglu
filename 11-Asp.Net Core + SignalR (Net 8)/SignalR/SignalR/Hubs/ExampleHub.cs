using Microsoft.AspNetCore.SignalR;

namespace SignalR.Hubs
{
    public class ExampleHub:Hub
    {
        public  async Task BroadcastMessageToAllClient(string message) // js den cagırdıgımız metodumuz 
        {
            await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        }
    }
}
