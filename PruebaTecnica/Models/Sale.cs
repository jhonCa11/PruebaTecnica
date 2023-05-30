
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Models
{
    [Table("Sales")]
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<SaleItem> Items { get; set; }
    }
}