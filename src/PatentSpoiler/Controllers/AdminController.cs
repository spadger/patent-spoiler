﻿using System.Web.Mvc;
using PatentSpoiler.App.Data;

namespace PatentSpoiler.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPatentDatabaseLoader patentDatabaseLoader;
        private readonly IPatentStoreHierrachy patentStoreHierrachy;

        public AdminController(IPatentDatabaseLoader patentDatabaseLoader, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.patentDatabaseLoader = patentDatabaseLoader;
            this.patentStoreHierrachy = patentStoreHierrachy;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportData()
        {
            var root = patentStoreHierrachy.Root;
            
            patentDatabaseLoader.StoreNodes(root);
            return Content("Done!");
        }
    }
}