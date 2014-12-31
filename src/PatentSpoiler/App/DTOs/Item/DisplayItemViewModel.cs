using System;
using System.Collections.Generic;
using AutoMapper;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DisplayItemViewModelMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.DTOs.Item
{
    public class DisplayItemViewModel
    {
        public int? LatestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public Guid SetId { get; set; }
        public HashSet<string> Categories { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    static class DisplayItemViewModelMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<PatentableEntity, DisplayItemViewModel>();
        }
    }
}