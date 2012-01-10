using System;
using System.Collections.Generic;

namespace Kalahari.Resort.Models
{
	public class Design
	{
		public string Name { get; set; }
		public string Architect { get; set; }
		public List<Component> Components { get; set; }

		public string Id { get; set; }


		public class Component
		{
			public string Id { get; set; }
			public int Qty { get; set; }
		}
	}

	public class Architect
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Citation> Citations { get; set; }

		public class Citation
		{
			public string For { get; set; }
			public DateTime At { get; set; }
		}
	}
}