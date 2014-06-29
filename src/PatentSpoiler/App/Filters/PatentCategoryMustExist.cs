using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using PatentSpoiler.App.Data;

namespace PatentSpoiler.App.Filters
{
    public class PatentCategoryMustExistAttribute : FilterAttribute
    {
        public PatentCategoryMustExistAttribute(string categoryPath)
        {
            CategoryPath = categoryPath;
        }

        public string CategoryPath { get; private set; }
    }

    public class PatentCategoryMustExistFilter : IActionFilter
    {
        private IPatentStoreHierrachy patentStoreHierrachy;

        public PatentCategoryMustExistFilter(string categoryPath, IPatentStoreHierrachy patentStoreHierrachy)
        {
            this.patentStoreHierrachy = patentStoreHierrachy;
            categoryPathSegments = categoryPath.Split(new[] { '.' });
        }

        string[] categoryPathSegments { get; set; }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object categoryObject = null;
            if (categoryPathSegments.Length == 1 && !filterContext.RouteData.Values.TryGetValue(categoryPathSegments[0], out categoryObject))
            {
                categoryObject = filterContext.HttpContext.Request.Params[categoryPathSegments[0]];
            }

            if (categoryObject == null)
            {
                var request = filterContext.RequestContext.HttpContext.Request;
                var possibleJson = new UTF8Encoding().GetString(request.BinaryRead(request.ContentLength));
                categoryObject = RecurseDownPropertyTree(JsonConvert.DeserializeObject(possibleJson), categoryPathSegments);
                request.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            }

            if (categoryObject == null)
            {
                throw new InvalidOperationException("Could not find the category id.");
            }


            if (!patentStoreHierrachy.ContainsCategory(categoryObject.ToString()))
            {
                filterContext.Result = new HttpNotFoundResult("Category not found");
            }
        }

        private string RecurseDownPropertyTree(dynamic tree, string[] pathElements)
        {
            for (var i = 0; i < pathElements.Length - 1; i++)
            {
                tree = tree[pathElements[i]];
            }

            return tree[pathElements[pathElements.Length - 1]];
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {}
    }


}