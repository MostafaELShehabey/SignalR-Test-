namespace SignalR_test
{
    public interface IChatClient
    {
        Task ReceiveMessage(string message);
        Task SendMessage(string name , string message);
        Task NewMassage(string name , string message );
    }
}
