namespace DesafioBroker.Dtos;

public class StockReferenceValuesDto
{
    public decimal SaleReferenceValue { get; set; }
    public decimal PurchaseReferenceValue { get; set; }

    public StockReferenceValuesDto(decimal saleReferenceValue, decimal purchaseReferenceValue)
    {
        if (saleReferenceValue <= purchaseReferenceValue)
        {
            throw new ArgumentException("Sale reference value cannot be lower than purchase reference value");
        }

        this.SaleReferenceValue = saleReferenceValue;
        this.PurchaseReferenceValue = purchaseReferenceValue;
    }
}
