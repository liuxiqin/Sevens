using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Infrastructure.Snapshoting;

namespace Seven.Infrastructure.Persistence
{
    public interface IPersistence
    {
        void Save<TEntity>(TEntity entity) where TEntity : EntityBase;

        TEntity GetById<TEntity>(string aggregateRootId) where TEntity : EntityBase;

        TEntity Get<TEntity>(ISpecification<TEntity> specification) where TEntity : EntityBase;
    }


    public class EventEntity
    {
        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// 此处采用json格式存储
        /// </summary>
        public string Datas { get; set; }
    }
}
