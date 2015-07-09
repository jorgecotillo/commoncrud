using com.jc.providers.Interfaces.RelationalDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.jc.providers.BaseDomain;
using System.Data.Common;
using System.Data;

namespace com.jc.providers.ADOWrappers
{
    public abstract class BaseADOWrapper : IADOContext
    {
        public abstract DbConnection GetADOConnection();

        public abstract DbCommand CreateCommand(
            string storeProcedureName, 
            DbConnection adoConnection);

        public abstract void AddCommandParameters(
            List<KeyValuePair<string, object>> parameters, 
            DbCommand adoCommand);

        public async Task<IList<TEntity>> GetMultipleEntities<TEntity>(string storeProcedureName, List<KeyValuePair<string, object>> parameters) where TEntity : BaseEntity, new()
        {
            return await GetMultipleEntities<TEntity>(storeProcedureName, parameters, CommandBehavior.Default);
        }

        public async Task<TEntity> GetSingleEntity<TEntity>(string storeProcedureName, List<KeyValuePair<string, object>> parameters) where TEntity : BaseEntity, new()
        {
            IList<TEntity> listOfEntities =
               await GetMultipleEntities<TEntity>(storeProcedureName, parameters, CommandBehavior.SingleRow);
            return listOfEntities.FirstOrDefault();
        }

        private async Task<IList<TEntity>> GetMultipleEntities<TEntity>(
            string storeProcedureName,
            List<KeyValuePair<string, object>> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
            where TEntity : BaseEntity, new()
        {
            IList<TEntity> listOfEntities = new List<TEntity>();
            try
            {
                // get a connection - safely do the cast because it is coming from the above method
                using (DbConnection adoConnection = GetADOConnection())
                {
                    // create the command object
                    DbCommand command = CreateCommand(storeProcedureName, adoConnection);

                    // set the command type
                    command.CommandType = CommandType.StoredProcedure;

                    // configure the parameters
                    AddCommandParameters(parameters, command);

                    // open the connection
                    adoConnection.Open();

                    //Create placeholder of entity so we can call FromReader method. This is used in order to avoid reflection at the 
                    //moment of creating the entity and assigning the proper values.
                    TEntity entity = new TEntity();
                    // execute the proc
                    using (DbDataReader reader = await command.ExecuteReaderAsync(behavior))
                    {
                        while (reader.Read())
                            listOfEntities.Add(entity.FromReader(reader) as TEntity);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listOfEntities;
        }

        public async Task<TValueType> GetSingleValue<TValueType>(string storeProcedureName, List<KeyValuePair<string, object>> parameters)
        {
            try
            {
                // get a connection - safely do the cast because it is coming from the above method
                using (DbConnection adoConnection = GetADOConnection())
                {
                    // create the command object
                    DbCommand command = CreateCommand(storeProcedureName, adoConnection);

                    // set the command type
                    command.CommandType = CommandType.StoredProcedure;

                    // configure the parameters
                    AddCommandParameters(parameters, command);

                    // open the connection
                    adoConnection.Open();

                    // execute the proc
                    object obj = await command.ExecuteScalarAsync();

                    if (obj == null)
                        return default(TValueType);
                    else
                        return (TValueType)Convert.ChangeType(obj, typeof(TValueType));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task PersistEntity<TEntity>(
            string storeProcedureName, 
            List<KeyValuePair<string, object>> parameters)
        {
            try
            {
                // get a connection - safely do the cast because it is coming from the above method
                using (DbConnection adoConnection = GetADOConnection())
                {
                    // create the command object
                    DbCommand command = CreateCommand(storeProcedureName, adoConnection);

                    // set the command type
                    command.CommandType = CommandType.StoredProcedure;

                    // configure the parameters
                    AddCommandParameters(parameters, command);

                    // open the connection
                    adoConnection.Open();

                    // execute the proc
                    int affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows <= 0)
                        throw new Exception("No afftected records");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
