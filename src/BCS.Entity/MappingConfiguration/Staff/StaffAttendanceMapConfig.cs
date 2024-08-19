using BCS.Entity.MappingConfiguration;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BCS.Entity.MappingConfiguration
{
    public class StaffAttendanceMapConfig : EntityMappingConfiguration<StaffAttendance>
    {
        public override void Map(EntityTypeBuilder<StaffAttendance>
        builderTable)
        {
          //b.Property(x => x.StorageName).HasMaxLength(45);
        }
     }
}

