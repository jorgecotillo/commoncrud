using com.jc.providers.Interfaces.RelationalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.jc.providers.BaseDomain;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace com.jc.providers.ADOWrappers
{
    public class ODPWrapper : 
        BaseADOWrapper, IADOContext
    {
        private string _connectionString = string.Empty;
        public ODPWrapper(string connectionString)
        {
            connectionString = _connectionString;
        }

        public override DbConnection GetADOConnection()
        {
            // now create the conection
            OracleConnection connection = new OracleConnection(_connectionString);

            // return the connection
            return connection;
        }

        public override void AddCommandParameters(List<KeyValuePair<string, object>> parameters, DbCommand adoCommand)
        {
            // configure the parameters
            foreach (KeyValuePair<string, object> keyValue in parameters)
            {
                OracleCommand command = adoCommand as OracleCommand;

                if (command == null)
                    throw new ArgumentException("Invalid command received");

                command.Parameters.Add(keyValue.Key, keyValue.Value);
            }
        }

        public override DbCommand CreateCommand(string storeProcedureName, DbConnection adoConnection)
        {
            // create the command object
            OracleCommand command = new OracleCommand(storeProcedureName, adoConnection as OracleConnection);
            return command;
        }
    }
}
