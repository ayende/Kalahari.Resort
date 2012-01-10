using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Shard;
using Raven.Client.Shard.ShardStrategy;
using Raven.Client.Shard.ShardStrategy.ShardAccess;
using Raven.Client.Shard.ShardStrategy.ShardResolution;
using Raven.Client.Shard.ShardStrategy.ShardSelection;
using Sharding.Model;

namespace Sharding
{
	class Program
	{
		static void Main(string[] args)
		{
			var shard1 = new DocumentStore
			{
				Identifier = "Shard1",
				Url = "http://localhost:8080"
			}.Initialize();

			var shard2 = new DocumentStore
			{
				Identifier = "Shard2",
				Url = "http://localhost:8081"
			}.Initialize();

			var shards = new Shards
			{
				shard1,
				shard2
			};
			var shardStrategy = new ShardStrategy
			{
				ShardAccessStrategy = new SequentialShardAccessStrategy(),
				ShardResolutionStrategy = new MyShardsResolutionStrategy(shards),
				ShardSelectionStrategy = new MyShardSelectionStrategy(shards)
			};
		
			var documentStore = new ShardedDocumentStore(shardStrategy, shards).Initialize();


			for (int i = 0; i < 3; i++)
			{
				using (var s = documentStore.OpenSession())
				{
					var user = new User
					{
						Name = "Ayende"
					};
					s.Store(user);
					s.Store(new ReadingList
					{
						UserId = user.Id,
						Books = new List<ReadingList.ReadBook>()
					});
					s.SaveChanges();
				}

			}
			using (var session = documentStore.OpenSession())
			{
				var load = session.Load<User>("users/Shard2/33");
			}

			using (var session = documentStore.OpenSession())
			{
				var load = session.Advanced.LuceneQuery<ReadingList>().ToList();
			}
		}
	}

	public class MyShardSelectionStrategy : IShardSelectionStrategy
	{
		private readonly Shards shards;

		private int current;

		public MyShardSelectionStrategy(Shards shards)
		{
			this.shards = shards;
		}

		public string ShardIdForNewObject(dynamic obj)
		{
			if(obj is ReadingList)
			{
				var first = shards.First(x=>x.Identifier==obj.UserId.Split('/')[1]);
				return SetNewShardIdForNewObject(obj, first);
			}
			var tmp = Interlocked.Increment(ref current);
			var documentStore = shards[tmp%shards.Count];
			return SetNewShardIdForNewObject(obj, documentStore);
		}

		private static string SetNewShardIdForNewObject(dynamic obj, IDocumentStore documentStore)
		{
			var shardIdForNewObject = documentStore.Identifier;

			string documentKey = documentStore.Conventions.GenerateDocumentKey(obj);

			var parts = new List<string>(documentKey.Split('/'));
			parts.Insert(1, shardIdForNewObject);
			documentKey = string.Join("/", parts);
			obj.Id = documentKey;
			return shardIdForNewObject;
		}

		public string ShardIdForExistingObject(dynamic obj)
		{
			string docId = obj.Id;

			return docId.Split('/')[1];
		}
	}

	public class MyShardsResolutionStrategy : IShardResolutionStrategy
	{
		private readonly Shards shards;

		public MyShardsResolutionStrategy(Shards shards)
		{
			this.shards = shards;
		}

		public IList<string> SelectShardIds(ShardResolutionStrategyData srsd)
		{
			if (srsd.Key != null)
				return new[] { srsd.Key.Split('/')[1] };
			return null;
		}
	}
}
