using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectPlanInfoMapConfig : EntityMappingConfiguration<ProjectPlanInfo>
    {
        public override void Map(EntityTypeBuilder<ProjectPlanInfo>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

