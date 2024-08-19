using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_CalendarMapConfig : EntityMappingConfiguration<Sys_Calendar>
    {
        public override void Map(EntityTypeBuilder<Sys_Calendar>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

