using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_QuartzOptionsMapConfig : EntityMappingConfiguration<Sys_QuartzOptions>
    {
        public override void Map(EntityTypeBuilder<Sys_QuartzOptions>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

