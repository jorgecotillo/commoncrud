﻿using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Interfaces.ADO
{
    public interface IADOContext
    {
        Task<TValueType> GetSingleValue<TValueType>(
           string storeProcedureName,
           List<KeyValuePair<string, object>> parameters);
        Task<TEntity> GetSingleEntity<TEntity>(
            string storeProcedureName,
            List<KeyValuePair<string, object>> parameters)
            where TEntity : BaseEntity, new();
        Task<IList<TEntity>> GetMultipleEntities<TEntity>(
            string storeProcedureName,
            List<KeyValuePair<string, object>> parameters)
            where TEntity : BaseEntity, new();
        Task PersistEntity<TEntity>(
            string storeProcedureName,
            List<KeyValuePair<string, object>> parameters);
    }
}
