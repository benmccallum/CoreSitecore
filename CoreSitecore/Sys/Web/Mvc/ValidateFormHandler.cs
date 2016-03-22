using System.Reflection;
using System.Web.Mvc;

namespace CoreSitecore.Sys.Web.Mvc
{
    /// <summary>
    /// Validates whether a form POST should be handled by an Action method given the presence of two hidden fields in the form.
    /// This is used for getting around Sitecore MVC which doesn't behave like standard ASP.NET.
    /// </summary>
    /// <remarks>
    /// Source: http://stackoverflow.com/a/23086364/725626
    /// </remarks>
    /// <example>
    /// Attribute your HttpPost action method with: [ValidateFormHandler]
    /// Place two hidden fields in your forms like:
    ///     &lt;input type="hidden" name="fhController" value="TestController" /&gt;
    ///     &lt;input type="hidden" name="fhAction" value="Index" /&gt;
    /// </example>
    public class ValidateFormHandler : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var controller = controllerContext.HttpContext.Request.Form["fhController"];
            var action = controllerContext.HttpContext.Request.Form["fhAction"];

            return !string.IsNullOrWhiteSpace(controller)
                    && !string.IsNullOrWhiteSpace(action)
                    && controller == controllerContext.Controller.GetType().Name
                    && methodInfo.Name == action;
        }
    }
}
