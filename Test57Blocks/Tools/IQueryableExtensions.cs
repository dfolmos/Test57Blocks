using System.Linq;
using Test57Blocks.DTO;

namespace Test57Blocks.Tools
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, PageDTO pageDTO)
        {
            return queryable
                .Skip((pageDTO.Page - 1) * pageDTO.RecordsByPage)
                .Take(pageDTO.RecordsByPage);
        }
    }
}
