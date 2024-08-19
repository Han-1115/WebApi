using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubcontractingContractAttachmentMapConfig : EntityMappingConfiguration<SubcontractingContractAttachment>
    {
        public override void Map(EntityTypeBuilder<SubcontractingContractAttachment>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

