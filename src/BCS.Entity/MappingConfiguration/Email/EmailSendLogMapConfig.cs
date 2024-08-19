using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class EmailSendLogMapConfig : EntityMappingConfiguration<EmailSendLog>
    {
        public override void Map(EntityTypeBuilder<EmailSendLog>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

