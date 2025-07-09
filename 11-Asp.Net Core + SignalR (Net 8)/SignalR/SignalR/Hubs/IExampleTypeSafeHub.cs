namespace SignalR.Hubs
{
    public interface IExampleTypeSafeHub
    {
        Task ReceiveMessageForAllClient(string message);
        Task ReceiveConnectedClientCount(int clientCount);
        Task ReceiveMessageForCallerClient(string message);

    }
}
