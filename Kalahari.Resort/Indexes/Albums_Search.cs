using System.Linq;
using Kalahari.Resort.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class Albums_Search : AbstractIndexCreationTask<Album, Albums_Search.Result>
	{
		public class Result
		{
			public string Query { get; set; }
		}

		public Albums_Search()
		{
			Map = albums => from album in albums
							select new
							{
								Query = new[]
								{
									album.Title, 
									album.Artist.Name,
									album.Genre.Name,
								}
							};

			Index(x=>x.Query, FieldIndexing.Analyzed);
		}
	}
}