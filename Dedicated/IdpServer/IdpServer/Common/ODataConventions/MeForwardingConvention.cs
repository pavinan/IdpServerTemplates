using IdpServer.Application.Services;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer.Common.ODataConventions
{
    public class MeForwardingConvention : IODataRoutingConvention
    {

        public IEnumerable<ControllerActionDescriptor> SelectAction(RouteContext routeContext)
        {
            var odata = routeContext.HttpContext.ODataFeature();

            if (odata.Path.PathTemplate == "~/singleton" && odata.Path.NavigationSource.Name.Equals("Me", StringComparison.OrdinalIgnoreCase))
            {
                var actionCollectionProvider = routeContext.HttpContext.RequestServices.GetRequiredService<IActionDescriptorCollectionProvider>();

                var actions = actionCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>();

                var actionName = "Get";

                if(routeContext.HttpContext.Request.Method == "PUT")
                {
                    actionName = "Put";
                }

                var getActions = actions.Where(x => x.ControllerName == "Users" && x.ActionName == actionName && x.Parameters.Any(x => x.Name == "key"));
                routeContext.RouteData.Values["key"] = "me";
                return getActions;
            }

            return null;
        }
    }
}
