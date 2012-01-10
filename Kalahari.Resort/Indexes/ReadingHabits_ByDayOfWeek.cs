using System;
using System.Collections.Generic;
using System.Linq;
using Kalahari.Resort.Models;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class ReadingHabits_ByDayOfWeek
		 : AbstractIndexCreationTask<ReadingList, ReadingHabits_ByDayOfWeek.Result>
	{
		public class Result
		{
			public DayOfWeek DayOfWeek { get; set; }
			public string UserId { get; set; }
			public int Count { get; set; }
		}
		public ReadingHabits_ByDayOfWeek()
		{
			Map = lists =>
				  from list in lists
				  from book in list.Books
				  select new
				  {
					  list.UserId,
					  book.ReadAt.DayOfWeek,
					  Count = 1
				  };

			Reduce = results =>
					 from result in results
					 group result by new { result.DayOfWeek, result.UserId }
						 into g
						 select new
						 {
							 g.Key.DayOfWeek,
							 g.Key.UserId,
							 Count = g.Sum(x => x.Count)
						 };
		}
	}

	public class ReadingHabits_ByDayOfWeek_MultiMap
		 : AbstractMultiMapIndexCreationTask<ReadingHabits_ByDayOfWeek_MultiMap.Result>
	{
		public class Result
		{
			public string UserId { get; set; }
			public CountPerDay[] CountsPerDay { get; set; }
			public string Name { get; set; }

			public class CountPerDay
			{
				public DayOfWeek DayOfWeek { get; set; }
				public int Count { get; set; }
			}
		}
		public ReadingHabits_ByDayOfWeek_MultiMap()
		{
			AddMap<ReadingList>(lists =>
				  from list in lists
				  select new
				  {
					  list.UserId,
					  Name = (string)null,
					  CountsPerDay = from b in list.Books
									 group b by b.ReadAt.DayOfWeek into g
									 select new
									 {
									 	DayOfWeek = g.Key,
										Count = g.Count()
									 }
				  });

			AddMap<User>(users =>
						 from user in users
						 from day in Enumerable.Range(0,6)
						 select new
						 {
							 UserId = user.Id,
							 CountsPerDay = new object[0],
							 user.Name,
						 }
			);

			Reduce = results =>
					 from result in results
					 group result by result.UserId
						 into g
						 select new
						 {
							 UserId = g.Key,
							 Name = g.Select(x => x.Name).FirstOrDefault(x => x != null),
							 CountsPerDay = from cpd in g.SelectMany(x=>x.CountsPerDay)
											group cpd by cpd.DayOfWeek into gi
											select new
											{
												DayOfWeek = gi.Key,
												Count = gi.Sum(x=>x.Count)
											}
						 };
		}
	}
}