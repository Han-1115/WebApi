using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.MappingConfiguration.Contract
{
    public class ContractProjecMapConfig : EntityMappingConfiguration<ContractProject>
    {
        public override void Map(EntityTypeBuilder<ContractProject> builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
