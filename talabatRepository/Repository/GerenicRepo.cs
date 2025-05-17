using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.core.model;
using Talabat.core.Repository;
using Talabat.core.Specifications;
using talabatRepository.Data;
using talabatRepository.Repository;

namespace TalabatRepository.Repository
{
    public class GenericRepo<T> : IgerenicRepo<T> where T : BaseEntity
    {
        private readonly TalabatDbContext _context;
        public GenericRepo(TalabatDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            
            return await _context.Set<T>().ToListAsync();
        }

      

        public async Task<IReadOnlyList<T>> GetAllAsyncspec(ISpecification<T> spec)
        {
            return await ApplySpec(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

     

        public async Task<T> GetByIdAsyncspec(ISpecification<T> spec)
        {
            return await ApplySpec(spec).FirstOrDefaultAsync();
        }

        public async Task<int> Productcount(ISpecification<T> spec)
        {
           return await ApplySpec(spec).CountAsync();
        }

        private IQueryable<T> ApplySpec(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }
    }
}