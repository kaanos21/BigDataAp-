namespace BigDataApı.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public double UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CountryOfOrigin { get; set; }
        public string ProductImageUrl { get; set; }
        public Category Category { get; set; }

    }
}
