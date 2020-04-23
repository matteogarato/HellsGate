using Microsoft.EntityFrameworkCore;

namespace HellsGate.Models.DatabaseModel
{
    [Owned]
    public class SafeAuthModel : BaseModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AutId { get; set; }
        public string Control { get; set; }
    }
}