using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface ICRUDOperations<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> PostAsync(T obj);
        Task<T> UpdateAsync(int id, T obj);
        Task<T> DeleteAsync(int id);

    }
}
