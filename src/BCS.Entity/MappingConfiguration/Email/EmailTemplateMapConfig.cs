using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class EmailTemplateMapConfig : EntityMappingConfiguration<EmailTemplate>
    {
        public override void Map(EntityTypeBuilder<EmailTemplate>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

