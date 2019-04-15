using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MovieCritiqueAPI
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			//Enable CORS
			//config.EnableCors(new System.Web.Http.Cors.EnableCorsAttribute("http://localhost:4200", headers: "*", methods: "*"));

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.Filters.Add(new AuthorizeAttribute());
		}
	}
}
