﻿using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Framework.Entity.MappingConfiguration
{
    public class vSys_DictionaryMapConfig : EntityMappingConfiguration<vSys_Dictionary>
    {
        public override void Map(EntityTypeBuilder<vSys_Dictionary> builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
    }
}


