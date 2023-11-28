using System.Collections.Generic;

namespace AnhQuoc_WPF_C4_B1
{
    interface IRepositoryBase<T>
    {
        int Length();

        List<T> Gets();
        T GetByIndex(int index);
        void Add(T entity);

        void BulkInsert(List<T> entities);
        void Insert(T refEntity, T entity);
        void Remove(T entity);
    }
}
