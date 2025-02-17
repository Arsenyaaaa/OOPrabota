using System;

namespace NorthwindApp
{
    public class Products
    {
        // Поля
        private int productID;
        private string productName;
        private int categoryID;
        private decimal unitPrice;

        // Конструктор
        public Products(int productID, string productName, int categoryID, decimal unitPrice)
        {
            this.ProductID = productID;
            this.ProductName = productName;
            this.CategoryID = categoryID;
            this.UnitPrice = unitPrice;
        }

        // Свойства
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

