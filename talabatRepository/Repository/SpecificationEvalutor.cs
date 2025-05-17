using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Talabat.core.model;
using Talabat.core.Specifications;

namespace talabatRepository.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
        {
            // Apply criteria if it exists
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if(spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            // Apply includes
            query = spec.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            return query;
        }
    }
}