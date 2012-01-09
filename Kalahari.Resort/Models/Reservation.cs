using System;

namespace Kalahari.Resort.Models
{
	public class Reservation
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public string RoomType { get; set; }
		public int Occupancy { get; set; }
		public CustomerInfo Customer { get; set; }

		public class CustomerInfo
		{
			public string Name { get; set; }
			public string Phone { get; set; }
		}
	}

}