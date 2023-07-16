﻿namespace PointOfSale.Dto.Request
{
    public class AddItem
    {
        public int ItemId { get; set; }
        public string ExternalProductId { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
    }
}
