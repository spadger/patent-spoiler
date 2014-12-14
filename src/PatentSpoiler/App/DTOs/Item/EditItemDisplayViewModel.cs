using System.Collections.Generic;
using AutoMapper;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EditItemDisplayViewModelMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.DTOs.Item
{
    public class EditItemDisplayViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public HashSet<string> Categories { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
    }


    static class EditItemDisplayViewModelMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<PatentableEntity, EditItemDisplayViewModel>();
        }
    }
}