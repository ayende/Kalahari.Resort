using System;
using System.Collections.Generic;

namespace Kalahari.Resort.Models
{
	public class ReadingList
	{
		public string Id { get; set; }
		public string UserId { get; set; }

		public List<ReadBook> Books { get; set; }

		public class ReadBook
		{
			public string Title { get; set; }
			public DateTime ReadAt { get; set; }
		}
	}
}