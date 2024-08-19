using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class Sys_WorkFlowStepMapConfig : EntityMappingConfiguration<Sys_WorkFlowStep>
    {
        public override void Map(EntityTypeBuilder<Sys_WorkFlowStep>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

