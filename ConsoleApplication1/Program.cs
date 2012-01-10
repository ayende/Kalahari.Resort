using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using ClassLibrary1;
using Raven.Database;
using Raven.Database.Config;
using Raven.Database.Server;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			var ravenConfiguration = new RavenConfiguration
			{
				Catalog =
					{
						Catalogs =
							{
								new TypeCatalog(typeof (DocAsXml))
							}
					}
			};

			var documentDatabase = new DocumentDatabase(ravenConfiguration);
			documentDatabase.SpinBackgroundWorkers();
			var httpServer = new HttpServer(ravenConfiguration, documentDatabase);
			httpServer.StartListening();
			Console.WriteLine(ravenConfiguration.ServerUrl);

			Console.WriteLine("Listening...");

			Console.ReadLine();
		}
	}
}
