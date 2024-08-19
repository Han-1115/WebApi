using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubcontractingContractMapConfig : EntityMappingConfiguration<SubcontractingContract>
    {
        public override void Map(EntityTypeBuilder<SubcontractingContract>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

