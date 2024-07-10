using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SignalR_test.Model
{
    public class Connection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  string  Id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public User User { get; set; }
    }
}
