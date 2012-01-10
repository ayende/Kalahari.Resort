using System.Linq;
using Kalahari.Resort.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class Users_ByName : AbstractIndexCreationTask<User, User>
	{
		public Users_ByName()
		{
			Map = users => from user in users
						   select new { user.Name };

			Index(x => x.Name, FieldIndexing.Analyzed);
		}
	}
}