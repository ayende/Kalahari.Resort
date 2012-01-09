using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kalahari.Resort.Models;

namespace Kalahari.Resort.Controllers
{
	public class BooksController : RavenController
	{
		public ActionResult Create()
		{
			Session.Store(new ReadingList
			{
				Id = "ReadingLists/Ayende/1",
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
			return Json(new {ok = 1});
		}
	}
}