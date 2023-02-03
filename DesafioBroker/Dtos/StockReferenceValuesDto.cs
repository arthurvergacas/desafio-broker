namespace DesafioBroker.Dtos;

public class StockReferenceValuesDto
{
    public decimal SaleReferenceValue { get; set; }
    public decimal PurchaseReferenceValue { get; set; }

    public StockReferenceValuesDto(decimal purchaseReferenceValue, decimal saleReferenceValue)
    {
        if (saleReferenceValue <= purchaseReferenceValue)
        {
            throw new ArgumentException("Sale reference value cannot be lower than purchase reference value");
        }

        this.PurchaseReferenceValue = purchaseReferenceValue;
        this.SaleReferenceValue = saleReferenceValue;
    }
}
