using System;
using System.Collections.Generic;
using System.Text;

namespace DMC_NET
{
    public interface IRepository<TEntity>
    {
        int Update(TEntity entity, int? id = null);

        TEntity ReadById(int id);

        IReadOnlyCollection<KeyValuePair<int, TEntity>> ReadAll();
    }
}
