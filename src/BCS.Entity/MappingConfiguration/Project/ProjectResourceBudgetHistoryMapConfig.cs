using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectResourceBudgetHistoryMapConfig : EntityMappingConfiguration<ProjectResourceBudgetHistory>
    {
        public override void Map(EntityTypeBuilder<ProjectResourceBudgetHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

