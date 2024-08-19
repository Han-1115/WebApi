using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubContractFlowAttachmentMapConfig : EntityMappingConfiguration<SubContractFlowAttachment>
    {
        public override void Map(EntityTypeBuilder<SubContractFlowAttachment>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

