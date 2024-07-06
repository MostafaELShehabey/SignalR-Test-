using Microsoft.AspNetCore.SignalR;

namespace SignalR_test
{
    public sealed class ChatHub : Hub<IChatClient>
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.ReceiveMessage( $"this message will send , connection id =  {Context.ConnectionId}");
            
        }



        public async Task SendMessage(string name ,string massage)
        {
            // save databse 
            
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} : {massage}");
            
           // Clients.Others //send to all people without this user 
        }

       


    }
}
