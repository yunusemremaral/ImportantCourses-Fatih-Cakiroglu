namespace SignalR.Hubs
{
    public interface IExampleTypeSafeHub
    {
        Task ReceiveMessageForAllClient(string message);
        Task ReceiveConnectedClientCount(int clientCount);
        Task ReceiveMessageForCallerClient(string message);
        Task ReceiveMessageForOtherClient(string message);
        Task ReceiveMessageForSpesificClient(string message);
        Task ReceiveMessageForGroup(string message); // ✅ Grup mesajı


    }
}
