using System.Web.Mvc;
using Kalahari.Resort.Models;
using Raven.Client.Document;

namespace Kalahari.Resort.Controllers
{
	public class HomeController : Controller
	{
		public  ActionResult Index()
		{
			using(var store = new DocumentStore
			{
				Url = "http://localhost:8080"
			}.Initialize())
			{
				using(var session = store.OpenSession())
				{
					session.Store(new Room
					{
						Number = "2556",
						Rating = 4,
						RoomType = "Double Bed",
						Smoking = false
					});

					session.SaveChanges();
				}
			}
			return Json(new {ok = true}, JsonRequestBehavior.AllowGet);
		}
	}
}