using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubContractFlowStaffMapConfig : EntityMappingConfiguration<SubContractFlowStaff>
    {
        public override void Map(EntityTypeBuilder<SubContractFlowStaff>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

