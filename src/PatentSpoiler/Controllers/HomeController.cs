using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PatentSpoiler.App.Data;
using PatentSpoiler.App.Data.Indexes;
using PatentSpoiler.App.Data.Queries;
using PatentSpoiler.App.Domain;
using PatentSpoiler.App.DTOs;
using Raven.Abstractions.Util;
using Raven.Client;

namespace PatentSpoiler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentStore documentStore;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;
        private readonly ISearchForClassificationQuery searchForClassificationQuery;

        private readonly Singleton singleton1;
        private readonly Singleton singleton2;

        private readonly Web web1;
        private readonly Web web2;

        private readonly Transient transient1;
        private readonly Transient transient2;

        public HomeController(IPatentStoreHierrachy patentStoreHierrachy, IDocumentStore documentStore, ISearchForClassificationQuery searchForClassificationQuery, Singleton singleton1, Singleton singleton2, Web web1, Web web2, Transient transient1, Transient transient2)
        {
            this.patentStoreHierrachy = patentStoreHierrachy;
            this.documentStore = documentStore;
            this.searchForClassificationQuery = searchForClassificationQuery;
            this.singleton1 = singleton1;
            this.singleton2 = singleton2;
            this.web1 = web1;
            this.web2 = web2;
            this.transient1 = transient1;
            this.transient2 = transient2;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SearchForTerm(string term)
        {
            var result = searchForClassificationQuery.Execute(term, 0, 10);

            return Json(result.Select(SearchResultTree.From), JsonRequestBehavior.AllowGet);
        }
    }
}
