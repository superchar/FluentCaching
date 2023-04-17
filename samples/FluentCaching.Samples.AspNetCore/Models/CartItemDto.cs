﻿// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace FluentCaching.Samples.AspNetCore.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class CartItemDto
{
    public Guid CardId { get; set; }
    
    public string ProductName { get; set; }

    public int Quantity { get; set; }
}