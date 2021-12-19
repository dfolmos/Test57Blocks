using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test57Blocks.DTO;
using Test57Blocks.Controllers;
using Microsoft.Extensions.Configuration;
using Test57Blocks.Models;

namespace Test57BlocksTest.UnitTest
{
    [TestClass]
    public class UsersControllerTest
    {
        public UsersController BuildController(string dbName, User user=null)
        {
            var context = TestDatabase.BuildDbContext(dbName);
            if (user != null)
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            var config = new Dictionary<string, string>
            {
                { "JWTKey","ALSFNIGIDFBVIUBFVDNFGIURTGBMASDNDCFOIEURHG34958U0FSDNGF9384Y8723Y34354TWEFSDFSF&%$%(325EWFSDDFHFGJFGXCVSDFJSUDFVSUJCFA" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            return new UsersController(configuration, context);
        }
        [TestMethod]
        public async Task RegisterUserSuccess()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            RegisterUserDTO user = new RegisterUserDTO();
            user.Name = "David Olmos";
            user.UserEmail = "dfolmos@gmail.com";
            user.Password = "124343dsfrgdsfgsdfTF%DTY";

            var controller = BuildController(dataBaseName);
            var answer = await controller.Register(user);
            TransactionResultDTO result = answer.Value;

            Assert.AreEqual("User created succesfully", result.Message);
        }
        [TestMethod]
        public async Task RegisterExistingUser()
        {
            string dataBaseName = Guid.NewGuid().ToString();
            User user = new User();
            user.UserName = "David Olmos";
            user.UserEmail = "dfolmos@gmail.com";
            user.UserPassword = "124343dsfrgdsfgsdfTF%DTY";
           

            RegisterUserDTO userNew = new RegisterUserDTO();
            userNew.Name = "David Olmos";
            userNew.UserEmail = "dfolmos@gmail.com";
            userNew.Password = "124343dsfrgdsfgsdfTF%DTY";

            var controller = BuildController(dataBaseName, user);
            var answer = await controller.Register(userNew);
            TransactionResultDTO result = answer.Value;

            Assert.AreEqual("The email already exits", result.Errors[0]);
        }
    }
}
