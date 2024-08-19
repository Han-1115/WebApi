using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_SalaryMapMapConfig : EntityMappingConfiguration<Sys_SalaryMap>
    {
        public override void Map(EntityTypeBuilder<Sys_SalaryMap>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

