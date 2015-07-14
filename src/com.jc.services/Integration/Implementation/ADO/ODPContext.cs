using com.jc.services.Integration.Interfaces.ADO;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Implementation.ADO
{
    public class ODPContext :
        BaseADOContext, IADOContext
    {
        private string _connectionString = string.Empty;
        public ODPContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override DbConnection GetADOConnection()
        {
            // now create the conection
            OracleConnection connection = new OracleConnection(_connectionString);
        
            // return the connection
            return connection;
        }

        public override void AddCommandParameters(
            List<KeyValuePair<string, object>> parameters,
            DbCommand adoCommand, 
            bool addDefaultRefCursor = false)
        {
            OracleCommand command = adoCommand as OracleCommand;

            if (command == null)
                throw new ArgumentException("Invalid command received");

            // configure the parameters
            foreach (KeyValuePair<string, object> keyValue in parameters)
            {
                command.Parameters.Add(keyValue.Key, keyValue.Value);
            }

            if (addDefaultRefCursor)
            {
                OracleParameter par = new OracleParameter("cur", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);
                command.Parameters.Add(par);
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
