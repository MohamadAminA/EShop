﻿using System.Collections.Generic;

namespace EShop.Models
{
	public class CartViewModel
	{
		public List<CartItem> CartItems { get; set; }
		public long OrderTotaL { get; set; }
	}
}