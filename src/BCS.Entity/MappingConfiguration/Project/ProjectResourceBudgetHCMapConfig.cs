using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectResourceBudgetHCMapConfig : EntityMappingConfiguration<ProjectResourceBudgetHC>
    {
        public override void Map(EntityTypeBuilder<ProjectResourceBudgetHC>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

