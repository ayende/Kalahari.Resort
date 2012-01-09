using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kalahari.Resort.Models;
using Raven.Client;
using Raven.Client.Document;

namespace Kalahari.Resort.Controllers
{
	public class HomeController : RavenController
	{
		public ActionResult Index()
		{
			var rand = new Random();
			Session.Store(new RoomTypePricing
			{
				RoomTypeId = "RoomType/1",
				Month = 1,
				Year = 2012,
				Pricing = Enumerable.Range(1, 31)
			              	.ToDictionary(x => x, x => (decimal)rand.Next(200, 300))
			});

			return JsonGet(new
			{
				new RoomTypePricing
				{
					RoomTypeId = "RoomType/1",
					Month = 1,
					Year = 2012,
					Pricing = Enumerable.Range(1, 31)
			            	.ToDictionary(x => x, x => (decimal)rand.Next(200, 300))
				}.Id
			});
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

			return JsonGet(new { ok = true });
		}

		public ActionResult LoadRoomAndType()
		{
			var room = Session
				.Include<Room>(r => r.RoomType)
				.Load<Room>("rooms/1");
			var type = Session.Load<RoomType>(room.RoomType);

			return JsonGet(new { room, Type = type });
		}


	}
}