using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kalahari.Resort.Models;
using Raven.Client;
using Raven.Client.Document;

namespace Kalahari.Resort.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			using (var store = new DocumentStore
			{
				Url = "http://localhost:8080"
			}.Initialize())
			{
				return UseSession(store);
			}
		}

















		private ActionResult UseSession(IDocumentStore store)
		{
			using (var session = store.OpenSession())
			{
				var rand = new Random();
				var roomTypePricing = new RoomTypePricing
				{
					RoomTypeId = "RoomType/1",
					Month = 1,
					Year = 2012,
					Pricing = Enumerable.Range(1,31)
						.ToDictionary(x=>x, x=> (decimal)rand.Next(200,300))
				};
				session.Store(roomTypePricing);

				session.SaveChanges();

				return Json(new
				{
					roomTypePricing.Id
				}, JsonRequestBehavior.AllowGet);
			}
		}
	}
}