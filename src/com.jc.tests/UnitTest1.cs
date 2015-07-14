using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using com.jc.services.Integration.Interfaces.ADO;
using com.jc.services.Domain;
using com.jc.services.Integration.Implementation.ADO;
using com.jc.providers.CustomAttributes;
using com.jc.services.Business.Implementations;
using com.jc.services.Business.Interfaces;
using com.jc.services.Integration.Interfaces.EF;
using com.jc.services.Integration.Implementation.EF;
using com.jc.tests.Domain;
using System.Linq;
using com.jc.services.Integration.Interfaces;

namespace com.jc.tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SQL_Test_Select()
        {
            //Setup a mock of repository
            IRepository<TestEntity, IADOContext> mockAdoSqlRepository =
               new ADORepository<TestEntity>
               (new SQLContext(@"Data Source=.\sqlexpress;Initial Catalog=ShowcaseDB;Integrated Security=False;User ID=nop_user_dev;Password=C0ll@borativ3;Persist Security Info=False;MultipleActiveResultSets=True"));

            ITestEntityService service = new ADOTestEntityService(mockAdoSqlRepository);
            var x = service.GetById(1).Result;
            Console.WriteLine(x.Id);
            Console.WriteLine(x.Description);
        }

        [TestMethod]
        public void Oracle_Test_Select()
         {
            //Setup a mock of repository
             IRepository<TestEntity, IADOContext> mockAdoSqlRepository = 
                new ADORepository<TestEntity>
                (new ODPContext("User Id=flcapp;Password=flcapp;Data Source=xe"));

            ITestEntityService service = new ADOTestEntityService(mockAdoSqlRepository);
            var x = service.GetById(1).Result;
            Console.WriteLine(x.Id);
            Console.WriteLine(x.Description);
        }

        [TestMethod]
        public void EF_Test_Select()
        {
            //Setup a mock of repository
            IRepository<TestEntity, IQueryable<TestEntity>> mockEfSqlRepository =
                new EFRepository<TestEntity>
                (new EFContext(@"Data Source=.\sqlexpress;Initial Catalog=ShowcaseDB;Integrated Security=False;User ID=nop_user_dev;Password=C0ll@borativ3;Persist Security Info=False;MultipleActiveResultSets=True"));

            ITestEntityService service = new EFTestEntityService(mockEfSqlRepository);
            var x = service.GetById(1).Result;
            Console.WriteLine(x.Id);
            Console.WriteLine(x.Description);
        }
        
        private interface ITestEntityService : ICRUDCommonService<TestEntity>
        {

        }

        private class ADOTestEntityService : CRUDCommonService<TestEntity, IADOContext>, ITestEntityService
        {
            readonly IRepository<TestEntity, IADOContext> _testEntityRepository;
            public ADOTestEntityService(IRepository<TestEntity, IADOContext> testEntityRepository)
                : base(testEntityRepository)
            {
                _testEntityRepository = testEntityRepository;
            }
        }

        private class EFTestEntityService : CRUDCommonService<TestEntity, IQueryable<TestEntity>>, ITestEntityService
        {
            readonly IRepository<TestEntity, IQueryable<TestEntity>> _testEntityRepository;
            public EFTestEntityService(IRepository<TestEntity, IQueryable<TestEntity>> testEntityRepository)
                : base(testEntityRepository)
            {
                _testEntityRepository = testEntityRepository;
            }
        }
    }
}
