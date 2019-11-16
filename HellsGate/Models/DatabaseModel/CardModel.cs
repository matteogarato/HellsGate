using System;
using System.ComponentModel.DataAnnotations;

namespace HellsGate.Models
{
    public class CardModel
    {
        [Key]
        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}