using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_WorkFlowTableStepMapConfig : EntityMappingConfiguration<Sys_WorkFlowTableStep>
    {
        public override void Map(EntityTypeBuilder<Sys_WorkFlowTableStep>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

