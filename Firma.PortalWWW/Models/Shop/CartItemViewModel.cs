﻿namespace Firma.PortalWWW.Models.Shop
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? ProductPhotoUrl { get; set; }
        // Można dodać inne właściwości, np. link do produktu
    }
}