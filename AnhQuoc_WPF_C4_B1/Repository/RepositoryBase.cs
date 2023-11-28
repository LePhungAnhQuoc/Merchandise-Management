using System;
using System.Collections.Generic;
using System.Linq;

namespace AnhQuoc_WPF_C4_B1
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public List<T> Items { get; set; }

        public RepositoryBase()
        {
            Items = new List<T>();
        }

        public RepositoryBase(List<T> entities)
        {
            Items = entities;
        }

        public int Length()
        {
            return Items.Count();
        }

        public List<T> Gets()
        {
            return Items;
        }

        public T GetByIndex(int index)
        {
            return Items[index];
        }

        public void Add(T entity)
        {
            Items.Add(entity);
        }

        public void BulkInsert(List<T> entities)
        {
            Items.AddRange(entities);
        }

        public void Insert(T refEntity, T entity)
        {
            Items.Insert(Items.IndexOf(refEntity), entity);
        }

        public void Remove(T entity)
        {
            Items.Remove(entity);
        }
    }
}
