using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdpServer.BuildingBlocks.AutoMapper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(Assembly assembly)
        {
            LoadStandardMappings(assembly);
            LoadCustomMappings(assembly);
            LoadConverters(assembly);
        }

        private void LoadConverters(Assembly assembly)
        {

        }

        private void LoadStandardMappings(Assembly assembly)
        {
            var mapsFrom = MapperProfileHelper.LoadStandardMappings(assembly);

            foreach (var map in mapsFrom)
            {
                CreateMap(map.Source, map.Destination, MemberList.None);
                CreateMap(map.Destination, map.Source, MemberList.None);
            }
        }

        private void LoadCustomMappings(Assembly assembly)
        {
            var mapsFrom = MapperProfileHelper.LoadCustomMappings(assembly);

            foreach (var map in mapsFrom)
            {
                map.CreateMappings(this);
            }
        }
    }
}
