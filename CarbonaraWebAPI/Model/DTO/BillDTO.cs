using CarbonaraWebAPI.Model.DAO;

namespace CarbonaraWebAPI.Model.DTO
{
    public class BillDTO
    {
        public BillDTO(Bill bill)
        {
            Id = bill.Id;
            BillDate = new DateTimeOffset(bill.BillDate, TimeSpan.Zero).ToUnixTimeMilliseconds();
            User = new UserDTO( bill.User , true);
            Amount = bill.Amount;
            Notes = bill.Notes;
            Positions = new BillPositionDTO[bill.Positions.Count];
            for (int i = 0; i < bill.Positions.Count; i++)
                Positions[i] = new BillPositionDTO(bill.Positions[i]);
        }

        public BillDTO()
        {

        }
        public int Id { get; set; }
        public long BillDate { get; set; }
        public UserDTO User { get; set; }
        public float Amount { get; set; }
        public string Notes { get; set; }
        public BillPositionDTO[] Positions { get; set; }
    }


    public struct BillPositionDTO
    {
        public string Item { get; set; }
        public double Amount { get; set; }
        public double PricePerAmount { get; set; }

        public BillPositionDTO(BillPosition position)
        {
            Item = position.Item;
            Amount = position.Amount;
            PricePerAmount = position.PricePerAmount;
        }
    }
}

