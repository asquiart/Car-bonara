using System.ComponentModel.DataAnnotations;

namespace CarbonaraWebAPI.Model.DAO
{
    public class Bill
    {
        public Bill(DateTime billDate, User user, float amount, string notes)
        {
            BillDate = billDate;
            User = user;
            Amount = amount;
            Notes = notes;
        }

        public Bill()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime BillDate { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public float Amount { get; set; }
        public string Notes { get; set; }

        public List<BillPosition> Positions { get; set; }


    }


    public class BillPosition
    {
        [Key]
        public int Id { get; set; }
        public string Item { get; set; }
        public double Amount { get; set; }
        public double PricePerAmount { get; set; }

        public BillPosition()
        {

        }
        public BillPosition(string item, double amount, double pricePerAmount)
        {
            Item = item;
            Amount = amount;
            PricePerAmount = pricePerAmount;
        }
    }



}
