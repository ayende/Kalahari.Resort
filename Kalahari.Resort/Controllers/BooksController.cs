using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kalahari.Resort.Indexes;
using Kalahari.Resort.Models;
using Raven.Client.Linq;
using System.Linq;

namespace Kalahari.Resort.Controllers
{
	public class BooksController : RavenController
	{
		public ActionResult Habits(string name)
		{
			var q = Session.Query<User,Users_ByName>()
				.Search(x=>x.Name, name);
			var user = q.FirstOrDefault();

			if(user == null)
			{
				var suggestionQueryResult = q.Suggest();
				if(suggestionQueryResult.Suggestions.Length == 1)
				{
					return RedirectToActionPermanent("Habits",
						new {name = suggestionQueryResult.Suggestions[0]});
				}
				return Json(new
				{
					DidYouMean = suggestionQueryResult.Suggestions
				});
			}

			var results = Session.Query<ReadingHabits_ByDayOfWeek.Result, ReadingHabits_ByDayOfWeek>()
				.Where(x=>x.UserId == user.Id)
				.ToList();

			return Json(results);
		}

		public ActionResult Create()
		{
			Session.Store(new ReadingList
			{
				UserId = "users/2",
				Books = new List<ReadingList.ReadBook>
				{
					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-7),
						Title = "Can't recall?"
					},

					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-6),
						Title = "Heavy is the Head"
					},
					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-4),
						Title = "Torch of Freedom"
					},

				}
			});

			Session.Store(new ReadingList
			{
				UserId = "users/2",
				Books = new List<ReadingList.ReadBook>
				{
					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-7),
						Title = "The magic of?"
					},

					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-6),
						Title = "Crown of Swords"
					},
					new ReadingList.ReadBook
					{
						ReadAt = DateTime.Today.AddDays(-4),
						Title = "Freedom? where?"
					},

				}
			});
			return Json(new {ok = 1});
		}
	}
}