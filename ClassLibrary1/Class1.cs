using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Raven.Database.Server.Abstractions;
using Raven.Database.Server.Responders;
using Raven.Database.Extensions;

namespace ClassLibrary1
{
	public class DocAsXml : RequestResponder
	{
		public override void Respond(IHttpContext context)
		{
			var match = urlMatcher.Match(context.GetRequestUrl());
			var docId = match.Groups[1].Value;

			var jsonDocument = Database.Get(docId, GetRequestTransaction(context));
			if(jsonDocument == null)
			{
				context.SetStatusToNotFound();
				return;
			}

			var deserializeXNode = JsonConvert.DeserializeXNode(jsonDocument.DataAsJson.ToString(Formatting.None), "doc");
			context.Response.ContentType = "text/xml";
			context.Write(deserializeXNode.ToString(SaveOptions.None));
		}

		public override string UrlPattern
		{
			get { return "^/xml/(.*)$"; }
		}

		public override string[] SupportedVerbs
		{
			get { return new[]{"GET"}; }
		}
	}
}
