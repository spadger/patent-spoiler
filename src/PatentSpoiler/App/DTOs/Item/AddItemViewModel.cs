﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs.Item;
using PatentSpoiler.ValidationAttributes;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UpdateItemRequestViewModelMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.DTOs.Item
{
    public class AddItemDisplayViewModel
    {
        public string Category { get; set; }
    }

    public abstract class ItemRequestViewModel
    {
        [Required, StringLength(250, MinimumLength = 5)]
        public string Name { get; set; }
        [Required, MinLength(50)]
        public string Description { get; set; }
        public IEnumerable<string> Claims { get; set; }

        [CannotBeEmpty]
        public HashSet<string> Categories { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }
    }

    public class AddItemRequestViewModel : ItemRequestViewModel
    { }

    public class UpdateItemRequestViewModel : ItemRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Changes { get; set; }
    }


    static class UpdateItemRequestViewModelMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<AddItemRequestViewModel, PatentableEntity>();
            Mapper.CreateMap<UpdateItemRequestViewModel, PatentableEntity>()
                .ForMember(x=>x.Id, opt=>opt.Ignore())
                .ForMember(x=>x.Attachments, opt=>opt.Ignore());
        }
    }
}