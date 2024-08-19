using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_DepartmentMappingMapConfig : EntityMappingConfiguration<Sys_DepartmentMapping>
    {
        public override void Map(EntityTypeBuilder<Sys_DepartmentMapping>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

