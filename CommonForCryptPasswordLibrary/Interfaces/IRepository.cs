using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll_Enumerable();
        IQueryable<T> GetAll_Queryable();
        T Get(Predicate<T> predicate);
        //T Get(string name);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}
