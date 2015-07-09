using com.jc.providers.Interfaces.RelationalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using com.jc.providers.BaseDomain;
using com.jc.providers.Helpers;

namespace com.jc.providers.ADOWrappers
{
    public class SQLWrapper :
        BaseADOWrapper, IADOContext
    {
        private string _connectionString = string.Empty;
        public SQLWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override DbConnection GetADOConnection()
        {
            // now create the conection
            SqlConnection connection = new SqlConnection(_connectionString);

            // return the connection
            return connection;
        }

        public override void AddCommandParameters(List<KeyValuePair<string, object>> parameters, DbCommand adoCommand)
        {
            // configure the parameters
            foreach (KeyValuePair<string, object> keyValue in parameters)
            {
                SqlCommand command = adoCommand as SqlCommand;

                if (command == null)
                    throw new ArgumentException("Invalid command received");

                command.Parameters.AddWithValue(keyValue.Key, keyValue.Value);
            }
        }

        public override DbCommand CreateCommand(string storeProcedureName, DbConnection adoConnection)
        {
            // create the command object
            SqlCommand command = new SqlCommand(storeProcedureName, adoConnection as SqlConnection);
            return command;
        }
    }
}
