using System.Web.Mvc;

namespace UzmanCrm.CrmService.WebUI.Filters
{
    public class JsonResultFilter : IResultFilter
    {
        public int? MaxJsonLength { get; set; }

        public int? RecursionLimit { get; set; }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is JsonResult jsonResult)
            {
                // override properties only if they're not set
                jsonResult.MaxJsonLength = jsonResult.MaxJsonLength ?? MaxJsonLength;
                jsonResult.RecursionLimit = jsonResult.RecursionLimit ?? RecursionLimit;
            }
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
