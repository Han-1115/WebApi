using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ContractAttachmentsHistoryMapConfig : EntityMappingConfiguration<ContractAttachmentsHistory>
    {
        public override void Map(EntityTypeBuilder<ContractAttachmentsHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

