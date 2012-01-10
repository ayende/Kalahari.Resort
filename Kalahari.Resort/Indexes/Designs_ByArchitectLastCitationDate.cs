using System;
using System.Linq;
using Kalahari.Resort.Models;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class Designs_ByArchitectLastCitationDate
		 : AbstractMultiMapIndexCreationTask<Designs_ByArchitectLastCitationDate.ReduceResult>
	{
		public class ReduceResult
		{
			public DateTime? LastCitation { get; set; }
			public string Name { get; set; }
			public string ArchitectId { get; set; }
			public string[] ComponentIds { get; set; }
			public string[] Designs { get; set; }
		}

		public Designs_ByArchitectLastCitationDate()
		{
			AddMap<Architect>(architects =>
							  from architect in architects
							  select new
							 {
								 LastCitation = (from citation in architect.Citations
														 select citation.At)
												 .LastOrDefault(),
								 architect.Name,
								 ArchitectId = architect.Id,
								 ComponentIds = new string[0],
								 Designs = new string[0]
							 });

			AddMap<Design>(designs =>
						   from design in designs
						   select new
						   {
							   Designs = new[] { design.Id },
							   LastCitation = (string)null,
							   ArchitectId = design.Architect,
							   Name = (string)null,
							   ComponentIds = design.Components.Select(x => x.Id)
						   });

			Reduce = results => from r in results
			                    group r by r.ArchitectId
			                    into g
			                    select new
			                    {
			                    	ArchitectId = g.Key,
									Designs = g.SelectMany(x=>x.Designs),
									Name = g.Select(x=>x.Name).FirstOrDefault(x=>x!=null),
									ComponentIds = g.SelectMany(x => x.ComponentIds),
									LastCitation = g.Select(x=>x.LastCitation).FirstOrDefault(x=>x!=null)
			                    };
		}
	}
}