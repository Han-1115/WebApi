using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class SubContractFlowPaymentPlanMapConfig : EntityMappingConfiguration<SubContractFlowPaymentPlan>
    {
        public override void Map(EntityTypeBuilder<SubContractFlowPaymentPlan>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

