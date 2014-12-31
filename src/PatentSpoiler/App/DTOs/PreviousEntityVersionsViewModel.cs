using System;
using AutoMapper;
using PatentSpoiler.App.Domain.Patents;
using PatentSpoiler.App.DTOs;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PreviousVersionViewModelMappings), "ConfigureMappings")]
namespace PatentSpoiler.App.DTOs
{
    public class PreviousEntityVersionsViewModel
    {
        public int Id { get; set; }
        public string Changes { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    static class PreviousVersionViewModelMappings
    {
        static void ConfigureMappings()
        {
            Mapper.CreateMap<PatentableEntity, PreviousEntityVersionsViewModel>();
        }
    }
}