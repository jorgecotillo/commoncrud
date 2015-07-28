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
                (new ODPContext("Data Source=localhost:1521/XE;User ID=FLCAPP;Password=flcapp"));

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

        [TestMethod]
        public void EF_Oracle_Test_Select()
        {
            //Setup a mock of repository
            IRepository<TestEntity, IQueryable<TestEntity>> mockEfSqlRepository =
                new EFRepository<TestEntity>
                (new EFContext("Data Source=localhost:1521/XE;User ID=FLCAPP;Password=flcapp"));

            ITestEntityService service = new EFTestEntityService(mockEfSqlRepository);
            var x = service.GetById(1).Result;
            Console.WriteLine(x.Id);
            Console.WriteLine(x.Description);
        }

        [TestMethod]
        public void EF_Oracle_Test_MultipleStatements()
        {
            //Setup a mock of repository
            IRepository<TestEntity, IQueryable<TestEntity>> mockEfSqlRepository =
                new EFRepository<TestEntity>
                (new EFContext("OracleHR"));

            ITestEntityService service = new EFTestEntityService(mockEfSqlRepository);
            TestEntity x = service.GetById(1).Result;

            if (x != null)
            {
                x.Description = "Description modified";
                Console.WriteLine(x.Id);
                Console.WriteLine(x.Description);
            }

            List<TestEntity> lst = new List<TestEntity>();
            for (int i = 1; i < 11; i++)
            {
                TestEntity newEntity = new TestEntity() { Description = "I am a new entity " + i.ToString() };
                lst.Add(newEntity);
            }

            foreach (var item in lst)
            {
                service.Save(item);
            }

            var lst2 = service.GetAll(page: 0, pageSize: 5).Result;

            foreach (var item2 in lst2)
            {
                item2.Description = "Modified at : " + DateTime.Now.ToLongTimeString();
            }

            service.Commit();
            

            foreach (var newEntity in lst)
            {
                Console.WriteLine(newEntity.Id);
                Console.WriteLine(newEntity.Description);
            }
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
