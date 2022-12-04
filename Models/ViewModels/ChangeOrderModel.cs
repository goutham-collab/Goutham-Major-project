namespace eMobileShop.Models.ViewModels;

public class ChangeOrderModel
{
    public int OrderId { get; set; }
    public OrderChangeType OrderChangeType { get; set; }
}

public enum OrderChangeType
{ 
    Complete = 1,
    Cancel = 2
}
