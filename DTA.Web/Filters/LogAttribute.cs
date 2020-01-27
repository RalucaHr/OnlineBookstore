
using System;
using System.Web.Mvc;
using System.IO;


namespace DTA.Web.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogInfo");
                Directory.CreateDirectory(path);
                path += "\\Log.txt";



                using (TextWriter tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(DateTime.Now + Environment.NewLine + filterContext.Exception.Message + Environment.NewLine + filterContext.Exception.StackTrace);
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}