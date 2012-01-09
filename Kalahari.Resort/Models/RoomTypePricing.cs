using System;
using System.Collections.Generic;

namespace Kalahari.Resort.Models
{
	public class RoomTypePricing
	{
		public string Id { get; set; }
		public string RoomTypeId { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public Dictionary<int, decimal> Pricing { get; set; }
	}
}