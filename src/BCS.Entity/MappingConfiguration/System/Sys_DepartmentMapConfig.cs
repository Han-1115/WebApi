using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_DepartmentMapConfig : EntityMappingConfiguration<Sys_Department>
    {
        public override void Map(EntityTypeBuilder<Sys_Department>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

