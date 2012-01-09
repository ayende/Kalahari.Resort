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

			return Json(new
			{
				new RoomTypePricing
				{
					RoomTypeId = "RoomType/1",
					Month = 1,
					Year = 2012,
					Pricing = Enumerable.Range(1, 31)
			            	.ToDictionary(x => x, x => (decimal)rand.Next(200, 300))
				}.Id
			}, JsonRequestBehavior.AllowGet);
		}
	}
}