using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_DepartmentSettingMapConfig : EntityMappingConfiguration<Sys_DepartmentSetting>
    {
        public override void Map(EntityTypeBuilder<Sys_DepartmentSetting>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

