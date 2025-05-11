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

            // Apply includes
            query = spec.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            return query;
        }
    }
}