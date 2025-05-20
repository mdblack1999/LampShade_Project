namespace _01_LampShadeQuery.Contracts.Inventory
{
    public interface IInventorQuery
    {
        StockStatus CheckStock(IsInStock command);
    }
}
