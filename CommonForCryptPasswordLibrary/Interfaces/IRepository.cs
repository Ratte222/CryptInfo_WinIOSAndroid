using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(Predicate<T> predicate);
        //T Get(string name);
        void Add(T item);
        void Edit(T item);
        void Delete(T item);
    }
}
