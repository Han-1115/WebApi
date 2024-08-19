using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class FormCollectionObjectMapConfig : EntityMappingConfiguration<FormCollectionObject>
    {
        public override void Map(EntityTypeBuilder<FormCollectionObject>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

