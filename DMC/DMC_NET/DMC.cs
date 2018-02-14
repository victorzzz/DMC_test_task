using DMC_NET.Exceptions;
using DMC_NET.Internal;
using System;
using System.Collections.Concurrent;

namespace DMC_NET
{
    public class DMC
    {
        private readonly ConcurrentDictionary<Type, object> _repositories;

        public DMC(int estimatedNumberOfTypes = 4, int estimatedCuncurencyLevel = 8)
        {
            _repositories = new ConcurrentDictionary<Type, object>(estimatedCuncurencyLevel, estimatedNumberOfTypes);
        }

        public IRepository<TEntity> Register<TEntity>(int estimatedCuncurencyLevel = 8, int estimatedNumberOfObjects = 16)
        {
            try
            {
                return (IRepository<TEntity>)_repositories
                    .GetOrAdd(typeof(TEntity),
                          (type) => new Repository<TEntity>(estimatedCuncurencyLevel, estimatedNumberOfObjects));
            }
            catch(OverflowException overflowException)
            {
                throw new UnexpectedErrorException($"DMC contains maximum namber of registered types", overflowException);

            }
        }
    }
}
