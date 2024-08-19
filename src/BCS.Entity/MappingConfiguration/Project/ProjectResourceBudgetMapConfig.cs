using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectResourceBudgetMapConfig : EntityMappingConfiguration<ProjectResourceBudget>
    {
        public override void Map(EntityTypeBuilder<ProjectResourceBudget>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

