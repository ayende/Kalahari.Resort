using System.Linq;
using Kalahari.Resort.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class Meaningless : AbstractMultiMapIndexCreationTask<Meaningless.Result>
	{
		public class Result
		{
			public string Title { get; set; }
		}
		public Meaningless()
		{
			AddMap<Album>(albums =>
			              from album in albums
			              select new
			              {
			              	album.Title
			              });
			AddMap<ReadingList>(lists =>
			                    from readingList in lists
			                    from book in readingList.Books
			                    select new
			                    {
			                    	book.Title
			                    });

			Index(x=>x.Title, FieldIndexing.Analyzed);
		}
	}
}