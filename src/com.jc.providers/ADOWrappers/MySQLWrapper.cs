using com.jc.providers.Interfaces.RelationalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.jc.providers.BaseDomain;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace com.jc.providers.ADOWrappers
{
    public class MySQLWrapper : 
        BaseADOWrapper, IADOContext
    {
        private string _connectionString = string.Empty;
        public MySQLWrapper(string connectionString)
        {
            connectionString = _connectionString;
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

        public override void AddCommandParameters(List<KeyValuePair<string, object>> parameters, DbCommand adoCommand)
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
