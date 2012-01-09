using System.Collections.Generic;

namespace Kalahari.Resort.Models
{
	public class AccountHistory
	{
		public string AccountId { get; set; }

		public List<Transaction> Transactions { get; set; }

		public class Transaction
		{
			public decimal Amount { get; set; }
			public string Note { get; set; }
		}
	}
}