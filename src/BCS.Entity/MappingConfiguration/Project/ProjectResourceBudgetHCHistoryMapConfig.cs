using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectResourceBudgetHCHistoryMapConfig : EntityMappingConfiguration<ProjectResourceBudgetHCHistory>
    {
        public override void Map(EntityTypeBuilder<ProjectResourceBudgetHCHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

