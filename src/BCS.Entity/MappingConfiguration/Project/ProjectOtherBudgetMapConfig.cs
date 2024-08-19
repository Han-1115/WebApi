using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectOtherBudgetMapConfig : EntityMappingConfiguration<ProjectOtherBudget>
    {
        public override void Map(EntityTypeBuilder<ProjectOtherBudget>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

