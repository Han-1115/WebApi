using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.MappingConfiguration
{
    public class ContractMapConfig : EntityMappingConfiguration<BCS.Entity.DomainModels.Contract>
    {
        public override void Map(EntityTypeBuilder<BCS.Entity.DomainModels.Contract>
   builderTable)
        {
            //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}
