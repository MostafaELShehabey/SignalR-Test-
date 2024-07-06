using Microsoft.AspNetCore.Identity;

namespace SignalR_test.Model
{
    public class User:IdentityUser
    {
        public string ConnectioId { get; set; }
    }
}
