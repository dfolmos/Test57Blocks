using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test57Blocks.Models;


namespace Test57BlocksTest
{
    public static class TestDatabase
    {

        public static _57BlocksContext BuildDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<_57BlocksContext>()
                .UseInMemoryDatabase(name).Options;
            var dbContext = new _57BlocksContext(options);
            return dbContext;
        }
        public static ControllerContext BuildControllerContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                      new Claim("idUser", "1")
                }));
            return new ControllerContext()
            {
                HttpContext=new DefaultHttpContext() { User= user}
            };
        }
    }
}
