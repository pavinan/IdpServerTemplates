using IdpServer.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer
{
    public class ODataHelpers
    {
        public static IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<ApplicationUser>("ApplicationUser");

            return odataBuilder.GetEdmModel();
        }
    }
}
