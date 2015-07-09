using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.providers.CustomAttributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class DatabaseMappingAttribute : Attribute
    {
        readonly string _tableName;

        // Positional argument
        public DatabaseMappingAttribute(string tableName)
        {
            this._tableName = tableName;
        }

        public string TableName
        {
            get { return _tableName; }
        }

        // Named argument
        public string SelectSP { get; set; }
        public string InsertSP { get; set; }
        public string UpdateSP { get; set; }
        public string DeleteSP { get; set; }
    }
}
