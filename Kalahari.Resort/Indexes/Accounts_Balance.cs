using System.Linq;
using Kalahari.Resort.Models;
using Raven.Client.Indexes;

namespace Kalahari.Resort.Indexes
{
	public class Accounts_Balance : AbstractIndexCreationTask<AccountHistory, Accounts_Balance.ReduceResult>
	{
		public class ReduceResult
		{
			public string AccountId { get; set; }
			public decimal Amount { get; set; }
		}

		public Accounts_Balance()
		{
			Map = histories =>
			      from history in histories
			      from tx in history.Transactions
			      select new
			      {
			      	history.AccountId,
			      	tx.Amount
			      };

			Reduce = results =>
			         from result in results
			         group result by result.AccountId
			         into g
			         select new
			         {
						 AccountId = g.Key,
						 Amount = g.Sum(x=>x.Amount)
			         };

		}		 
	}
}