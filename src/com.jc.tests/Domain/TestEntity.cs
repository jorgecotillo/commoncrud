using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using com.jc.providers.CustomAttributes;
using System.Reflection;

namespace com.jc.tests.Domain
{
    [DatabaseMapping("TestTable", SelectSP = "dbo.SelectTestTable")]
    public class TestEntity : BaseEntity
    {
        [ColumnMapping("Description")]
        public string Description { get; set; }

        //public override BaseEntity FromReader(System.Data.Common.DbDataReader reader)
        //{
        //    TestEntity entity = new TestEntity();
        //    entity.Id = reader.GetFieldValue<int>(reader.GetOrdinal("Id"));
        //    entity.Description = reader.GetFieldValue<string>(reader.GetOrdinal("Description"));
        //    return entity;
        //}

        //public override TEntity ToEntity<TEntity>(System.Data.IDataReader reader)
        //{
        //    TestEntity entity = new TestEntity();
        //    entity.Id = reader.GetInt32(reader.GetOrdinal("Id"));
        //    entity.Description = reader.GetString(reader.GetOrdinal("Description"));
        //    return entity as TEntity;
        //}
    }
}
