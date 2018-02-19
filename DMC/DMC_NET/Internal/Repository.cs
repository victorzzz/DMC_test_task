using DMC_NET.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DMC_NET.Internal
{
    internal class Repository<TEntity> : IRepository<TEntity>
    {
        private readonly ConcurrentDictionary<int, TEntity> _storage;
        private volatile int _lastId = -1;

        public Repository(int estimatedCuncurencyLevel, int estimatedNumberOfObjects)
        {
            _storage = new ConcurrentDictionary<int, TEntity>(estimatedCuncurencyLevel, estimatedNumberOfObjects);
        }

        TEntity IRepository<TEntity>.ReadById(int id)
        {
            if(!_storage.TryGetValue(id, out var entity))
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        IReadOnlyCollection<KeyValuePair<int, TEntity>> IRepository<TEntity>.ReadAll()
        {
            return _storage;
        }

        int IRepository<TEntity>.Update(TEntity entity, int? id)
        {
            if (id.HasValue)
            {
                UpdateEntity(id.Value, entity);
                return id.Value;
            }
            else
            {
                return AddEntity(entity);
            }
        }

        #region private methods

        private int AddEntity(TEntity entity)
        {
            var idToAdd = Interlocked.Increment(ref _lastId);
            try
            {
                if (!_storage.TryAdd(idToAdd, entity))
                {
                    throw new UnexpectedErrorException($"Unexpected internal error on an entity adding. Type '{typeof(TEntity)}'");
                }
                return idToAdd;
            }
            catch(OverflowException overflowException)
            {
                throw new UnexpectedErrorException($"Repository for '{typeof(TEntity)}' contains maximum namber of entities", overflowException);
            }
        }

        private void UpdateEntity(int id, TEntity entity)
        {
            if(_storage.ContainsKey(id))
            {
                _storage.AddOrUpdate(id, default(TEntity), (entityId, existingEntity) => entity);
            }
            else
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }
        }

        #endregion
    }
}
