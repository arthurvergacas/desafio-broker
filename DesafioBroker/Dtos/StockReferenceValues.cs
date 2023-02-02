namespace DesafioBroker.Dtos;

public class StockReferenceValues
{
    public decimal SaleReferenceValue { get; set; }
    public decimal PurchaseReferenceValue { get; set; }

    public StockReferenceValues(decimal saleReferenceValue, decimal purchaseReferenceValue)
    {
        this.SaleReferenceValue = saleReferenceValue;
        this.PurchaseReferenceValue = purchaseReferenceValue;
    }
}
