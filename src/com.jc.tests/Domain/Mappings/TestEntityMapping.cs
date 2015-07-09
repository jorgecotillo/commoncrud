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
            this.ToTable("TestTable");
            this.HasKey(testTable => testTable.Id);
            this.Property(testTable => testTable.Description)
                .HasMaxLength(50);
            //this.Ignore(testTable => testTable.Active);
            this.Ignore(testTable => testTable.CreatedOn);
            this.Ignore(testTable => testTable.LastUpdated);
        }
    }
}
