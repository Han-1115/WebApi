using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ContractAttachmentsMapConfig : EntityMappingConfiguration<ContractAttachments>
    {
        public override void Map(EntityTypeBuilder<ContractAttachments>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

