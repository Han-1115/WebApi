using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.MappingConfiguration.Contract
{
    public class ContractHistoryMapConfig : EntityMappingConfiguration<ContractHistory>
    {
        public override void Map(EntityTypeBuilder<ContractHistory>
   builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
