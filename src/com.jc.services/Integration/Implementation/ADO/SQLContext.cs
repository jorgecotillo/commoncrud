using com.jc.services.Integration.Interfaces.ADO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Implementation.ADO
{
    public class SQLContext :
        BaseADOContext, IADOContext
    {
        private string _connectionString = string.Empty;
        public SQLContext(string connectionString)
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

        public override void AddCommandParameters(List<KeyValuePair<string, object>> parameters, DbCommand adoCommand, bool addDefaultRefCursor = false)
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
