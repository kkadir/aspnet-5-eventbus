namespace Bussy.Server.Models.Order
{
    public abstract class OrderModelForManipulation
    {
        public string Account { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }
        public int Size { get; set; }
    }
}