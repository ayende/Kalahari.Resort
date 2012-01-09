using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kalahari.Resort.Indexes;
using Kalahari.Resort.Models;
using Raven.Client.Linq;

namespace Kalahari.Resort.Controllers
{
	public class HomeController : RavenController
	{
		public ActionResult Index(string q)
		{
			var accountHistory = new[]
			{
				new AccountHistory
				{
					AccountId = "accounts/2",
					Transactions = new List<AccountHistory.Transaction>
					{
						new AccountHistory.Transaction
						{
							Amount = 50m,
							Note = "Initial Deposit"
						},
						new AccountHistory.Transaction
						{
							Amount = -15m,
							Note = "Bad Doggie"
						}
					}
				},

				new AccountHistory
				{
					AccountId = "accounts/2",
					Transactions = new List<AccountHistory.Transaction>
					{
						new AccountHistory.Transaction
						{
							Amount = -34.5m,
							Note = "Applebeess"
						},
						new AccountHistory.Transaction
						{
							Amount = -5m,
							Note = "Damn you, doggie!"
						}
					}
				}
				,

				new AccountHistory
				{
					AccountId = "accounts/1",
					Transactions = new List<AccountHistory.Transaction>
					{
						new AccountHistory.Transaction
						{
							Amount = 150m,
							Note = "Initial Deposit"
						},
						new AccountHistory.Transaction
						{
							Amount = -15m,
							Note = "Bad Doggie"
						}
					}
				},

				new AccountHistory
				{
					AccountId = "accounts/1",
					Transactions = new List<AccountHistory.Transaction>
					{
						new AccountHistory.Transaction
						{
							Amount = -14.5m,
							Note = "Applebeess"
						},
						new AccountHistory.Transaction
						{
							Amount = -5m,
							Note = "Dropped it!"
						}
					}
				}
			};

			var b = from history in accountHistory
					from tx in history.Transactions
			        group tx by history.AccountId
			        into g
			        select new
			        {
			        	AccountId = g.Key, 
						Balance = g.Sum(x=>x.Amount)
			        };

			foreach (var history in accountHistory)
			{
				Session.Store(history);
			}

			return Json(b);
		}

		public ActionResult SaveRoomAndType()
		{
			RoomType newType = new RoomType
			{
				Name = "King Suite"
			};

			Session.Store(newType);

			Session.Store(new Room
			{
				Number = "5000",
				Rating = 5,
				RoomType = newType.Id,
				Smoking = false
			});

			return Json(new { ok = true });
		}

		public ActionResult LoadRoomAndType()
		{
			var room = Session
				.Include<Room>(r => r.RoomType)
				.Load<Room>("rooms/1");
			var type = Session.Load<RoomType>(room.RoomType);

			return Json(new { room, Type = type });
		}


	}
}