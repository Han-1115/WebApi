using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_WorkFlowTableMapConfig : EntityMappingConfiguration<Sys_WorkFlowTable>
    {
        public override void Map(EntityTypeBuilder<Sys_WorkFlowTable>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

