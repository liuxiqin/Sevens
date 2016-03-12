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
    public interface IPersistence<TEntity> where TEntity : EntityBase
    {
        void Save(TEntity entity);

        TEntity GetById(string aggregateRootId);

        TEntity Get(ISpecification<TEntity> specification);

        TEntity Get(string aggregateRootId, int version);
    }


    public class EventEntity
    {
        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// 此处采用json格式存储
        /// </summary>
        public byte[] Datas { get; set; }
    }
}
