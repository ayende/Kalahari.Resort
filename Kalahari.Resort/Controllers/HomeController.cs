using System;
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
			new ReadingHabits_ByDayOfWeek_MultiMap().Execute(DocumentStore);
			var names = new[]
					{
						"Oren Eini",
						"Ayende Rahien",
						"David Boike",
						"Bryan Mihok",
						"Brandon Sharp",
						"Corey Perkins",
						"Cody Myers",
						"Gabriel Perez",
						"David Nolf",
						"Everett Muniz"
					};
			foreach (var name in names)
			{
				Session.Store(new User
				{
					Name = name
				});
			}

			return Json(1);
		}

		public ActionResult SaveRoomAndType()
		{

			using(Session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
			{
				
			}

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