using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test57Blocks.Tools
{
    public static class HttpContextExtensions
    {
        public async static Task InsertValueinHeader<T>(this HttpContext httpContext, IQueryable<T> query)
        {
            if(httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double numberOfRecord=await query.CountAsync();
            httpContext.Response.Headers.Add("numberOfRecord", numberOfRecord.ToString());
        }
    }
}
