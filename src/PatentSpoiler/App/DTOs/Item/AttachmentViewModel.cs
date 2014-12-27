using System;
using AutoMapper;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AttachmentViewModelMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.DTOs.Item
{
    public class AttachmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }
    }

    static class AttachmentViewModelMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<Attachment, AttachmentViewModel>();

            Mapper.CreateMap<AttachmentViewModel, Attachment>()
                .ForMember(x=>x.DateCreated, opt=> opt.UseValue(DateTime.Now));
        }
    }
}