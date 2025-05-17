using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model;
using Talabat.core.Specifications;

namespace Talabat.core.Repository
{
    public interface IgerenicRepo <T> where T : BaseEntity 
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        public Task<IReadOnlyList<T>> GetAllAsyncspec( ISpecification<T> spec);
        public Task<T> GetByIdAsyncspec( ISpecification<T> spec);
        Task<int> Productcount(ISpecification<T> spec);



    }
}
