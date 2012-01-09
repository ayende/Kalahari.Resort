using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;

namespace Kalahari.Resort.Controllers
{
	public class RavenController : Controller
	{
		static IDocumentStore documentStore;

		public static IDocumentStore DocumentStore
		{
			get
			{
				if (documentStore != null)
					return documentStore;
				lock (typeof(RavenController))
				{
					if (documentStore != null)
						return documentStore;
	
					documentStore = new DocumentStore
					{
						ConnectionStringName = "RavenDB"
					}.Initialize();
				}
				return documentStore;
			}
		}

		public new IDocumentSession Session { get; set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			Session = DocumentStore.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			using(Session)
			{
				if (Session == null || filterContext.Exception != null)
					return;

				Session.SaveChanges();
			}
		}

	}
}