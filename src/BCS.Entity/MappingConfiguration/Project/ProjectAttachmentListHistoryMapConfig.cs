using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class ProjectAttachmentListHistoryMapConfig : EntityMappingConfiguration<ProjectAttachmentListHistory>
    {
        public override void Map(EntityTypeBuilder<ProjectAttachmentListHistory>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

