using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubcontractingContractPaymentPlanMapConfig : EntityMappingConfiguration<SubcontractingContractPaymentPlan>
    {
        public override void Map(EntityTypeBuilder<SubcontractingContractPaymentPlan>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

