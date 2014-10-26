using AutoMapper;
using PatentSpoiler.App.Domain.Patents;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(PatentSpoiler.EntityMappings), "Start")]
namespace PatentSpoiler
{
    public static class EntityMappings
    {
        public static void Start()
        {
            Mapper.CreateMap<PatentableEntity, PatentableEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            Mapper.CreateMap<Attachment, Attachment>();
        }
    }
}