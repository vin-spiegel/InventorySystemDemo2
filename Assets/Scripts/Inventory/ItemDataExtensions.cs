namespace Inventory
{
    public static class ItemDataExtensions
    {
        public static void SetShape(this ItemData itemData, int row, int column, int value)
        {
            itemData.shape[row, column] = value;
            itemData.OnItemDataChanged();
        }

        public static void SetShape(this ItemData itemData, int[,] shape)
        {
            itemData.shape = shape;
            itemData.OnItemDataChanged();
        }
    }
}