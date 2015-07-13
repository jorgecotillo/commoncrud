using com.jc.services.Domain;
using com.jc.services.Integration.Interfaces.ADO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.jc.services.Domain.Extensions;

namespace com.jc.services.Integration.Implementation.ADO
{
    public abstract class BaseADOContext : IADOContext
    {
        /// <summary>
        /// Abstract method that will be implemented by any Custom ADO Context Implementation.
        /// This method returns a ADO Connection (All ADO's Implementations returns a Sub Classing of DbConnection).
        /// </summary>
        /// <returns>DbConnection</returns>
        public abstract DbConnection GetADOConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="adoConnection"></param>
        /// <returns></returns>
        public abstract DbCommand CreateCommand(
            string storeProcedureName,
            DbConnection adoConnection);

        public abstract void AddCommandParameters(
            List<KeyValuePair<string, object>> parameters,
            DbCommand adoCommand);

        public async Task<IList<TEntity>> GetMultipleEntities<TEntity>(
            string storeProcedureName, 
            List<KeyValuePair<string, object>> parameters) 
            where TEntity : BaseEntity, new()
        {
            return await GetMultipleEntities<TEntity>(storeProcedureName, parameters, CommandBehavior.Default);
        }

        public async Task<TEntity> GetSingleEntity<TEntity>(
            string storeProcedureName, 
            List<KeyValuePair<string, object>> parameters) 
            where TEntity : BaseEntity, new()
        {
            IList<TEntity> listOfEntities =
               await GetMultipleEntities<TEntity>(storeProcedureName, parameters, CommandBehavior.SingleRow);
            return listOfEntities.FirstOrDefault();
        }

        private async Task<IList<TEntity>> GetMultipleEntities<TEntity>(
            string storeProcedureName,
            List<KeyValuePair<string, object>> parameters,
            CommandBehavior behavior)
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
                            //listOfEntities.Add(entity.ToEntity<TEntity>(reader));
                            listOfEntities.Add(entity.GetMapper(entity).Map(reader));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return listOfEntities;
        }

        public async Task<TValueType> GetSingleValue<TValueType>(
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
