using System.Collections.Generic;
using AutoMapper;
using PatentSpoiler.App.Data.ElasticSearch;
using PatentSpoiler.App.Domain.Patents;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PatentableEntityIndexItemMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.Data.ElasticSearch
{
    public class PatentableEntityIndexItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Claims { get; set; }
        public string Description { get; set; }
    }

    static class PatentableEntityIndexItemMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<PatentableEntity, PatentableEntityIndexItem>();
        }
    }
}