using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectBudgetKeyItemMapConfig : EntityMappingConfiguration<ProjectBudgetKeyItem>
    {
        public override void Map(EntityTypeBuilder<ProjectBudgetKeyItem>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

