using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectBudgetSummarytHistoryMapConfig : EntityMappingConfiguration<ProjectBudgetSummaryHistory>
    {
        public override void Map(EntityTypeBuilder<ProjectBudgetSummaryHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

