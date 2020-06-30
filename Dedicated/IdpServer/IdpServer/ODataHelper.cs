using IdpServer.Application.Users.Queries.GetUsers;
using IdpServer.Common.ODataConventions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Routing.Conventions;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer
{
    public class ODataHelper
    {
        public static void GetEdmModel(IServiceProvider sp, IContainerBuilder containerBuilder)
        {

            containerBuilder.AddService(ServiceLifetime.Singleton,
                        _ =>
                        {
                            var list = ODataRoutingConventions.CreateDefaultWithAttributeRouting("api", sp);

                            list.Add(new MeForwardingConvention());

                            return list.AsEnumerable();
                        });


            var builder = new ODataConventionModelBuilder(sp);
            var usersType = builder.EntitySet<ApplicationUserModel>("Users");
            var meType = builder.Singleton<ApplicationUserModel>("Me");

            builder.EnableLowerCamelCase();

            var edmModel = builder.GetEdmModel();

            containerBuilder.AddService(ServiceLifetime.Singleton, (_) => edmModel);
        }
    }
}
