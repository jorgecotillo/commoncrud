using com.jc.services.Integration.Interfaces.ADO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Implementation.ADO
{
    public class MySQLContext :
        BaseADOContext, IADOContext
    {
        private string _connectionString = string.Empty;
        public MySQLContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public override DbConnection GetADOConnection()
        {
            // now create the conection
            MySqlConnection connection = new MySqlConnection(_connectionString);

            // return the connection
            return connection;
        }

        public override DbCommand CreateCommand(string storeProcedureName, DbConnection adoConnection)
        {
            MySqlCommand command = new MySqlCommand(storeProcedureName, adoConnection as MySqlConnection);
            return command;
        }

        public override void AddCommandParameters(List<KeyValuePair<string, object>> parameters, DbCommand adoCommand, bool addDefaultRefCursor = false)
        {
            // configure the parameters
            foreach (KeyValuePair<string, object> keyValue in parameters)
            {
                MySqlCommand command = adoCommand as MySqlCommand;

                if (command == null)
                    throw new ArgumentException("Invalid command received");

                command.Parameters.AddWithValue(keyValue.Key, keyValue.Value);
            }
        }
    }
}
