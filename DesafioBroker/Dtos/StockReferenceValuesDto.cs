namespace DesafioBroker.Dtos;

public class StockReferenceValuesDto
{
    public decimal SaleReferenceValue { get; set; }
    public decimal PurchaseReferenceValue { get; set; }

    public StockReferenceValuesDto(decimal saleReferenceValue, decimal purchaseReferenceValue)
    {
        this.SaleReferenceValue = saleReferenceValue;
        this.PurchaseReferenceValue = purchaseReferenceValue;
    }
}
