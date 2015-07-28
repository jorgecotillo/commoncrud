using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.tests.Domain.Mappings
{
    public class TestEntityMapping : EntityTypeConfiguration<TestEntity>
    {
        public TestEntityMapping()
        {
            this.ToTable("TESTTABLE");
            this.HasKey(testTable => testTable.Id)
                .Property(t => t.Id)
                .HasColumnName("ID");
            this.Property(testTable => testTable.Description)
                .HasMaxLength(50)
                .HasColumnName("DESCRIPTION");
            this.Property(t => t.Active)
                .HasColumnName("ACTIVE");
            //this.Ignore(testTable => testTable.Active);
            this.Ignore(testTable => testTable.CreatedOn);
            this.Ignore(testTable => testTable.LastUpdated);
        }
    }
}
