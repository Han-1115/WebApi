using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectOtherBudgetHistoryMapConfig : EntityMappingConfiguration<ProjectOtherBudgetHistory>
    {
        public override void Map(EntityTypeBuilder<ProjectOtherBudgetHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

