using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models.DatabaseModel
{
    public class CardModel : BaseModel
    {
        [Key]
        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}