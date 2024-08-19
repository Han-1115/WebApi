using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Framework.Entity.MappingConfiguration
{
    public class Sys_LogMapConfig : EntityMappingConfiguration<Sys_Log>
    {
        public override void Map(EntityTypeBuilder<Sys_Log> builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}


