﻿using System.Linq;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using System.Collections.Generic;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;

namespace SharpRepository.Tests.Integration.Spikes
{        
    [TestFixture]
    public class EfCoreLazyLoadSpike
    {
        private TestObjectContextCore dbContext;

        private Func<string, bool> filterSelects = q => q.StartsWith("Executing DbCommand") && q.Contains("SELECT");

        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {            
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("627a7ed1-b2c9-408a-a341-c01fc197a606")
                .Build();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeleted();
        }


        [SetUp]
        public void SetupRepository()
        {
            var configurationRoot = GetIConfigurationRoot(TestContext.CurrentContext.TestDirectory);

            var connectionString = configurationRoot.GetConnectionString("EfCoreConnectionString");

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseLazyLoadingProxies()
                .UseSqlServer(connectionString)
                .Options;
            
            // Create the schema in the database
            dbContext = new TestObjectContextCore(options);
            
            dbContext.Database.EnsureCreated();
            const int totalItems = 5;

            for (int i = 1; i <= totalItems; i++)
            {
                dbContext.Contacts.Add(
                    new Contact
                    {
                        ContactId = i.ToString(),
                        Name = "Test User " + i.ToString(),
                        EmailAddresses = new List<EmailAddress>()
                    });
            }

            dbContext.SaveChanges();

            foreach (var contact in dbContext.Contacts)
            {
                contact.EmailAddresses.Add(
                    new EmailAddress
                    {
                        Email = "test.addr." + contact.ContactId + "@email.com",
                        Label = "test.addr." + contact.ContactId
                    });
            }

            dbContext.SaveChanges();

            // reistantiate in order to lose caches
            dbContext = new TestObjectContextCore(options);
        }
        
        [Test]
        public void EfCore_GetAll_Without_Includes_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var contact = repository.GetAll().First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2); //may be that dbcontext is disposed and the successive queries are not logged, quieries does not contains email so query was made in a lazy way but after.
        }

        [Test]
        public void EfCore_GetAll_With_Includes_In_Strategy_EagerLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);
            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
        }

        [Test]
        public void EfCore_GetAll_With_Includes_In_Strategy_String_EagerLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);
            
            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
        }

        [Test]
        public void EfCore_GetAll_With_Text_Include_EagerLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var contact = repository.GetAll("EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
        }

        [Test]
        public void EfCore_GetAll_With_Text_Include_And_Pagination_EagerLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var pagination = new PagingOptions<Contact>(1, 4, "ContactId");

            var contact = repository.GetAll(pagination, "EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2); // first query is count for total records
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
        }

        [Test]
        public void EfCore_FindAll_With_Include_And_Predicate_In_Specs_EagerLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var findAllBySpec = new Specification<Contact>(obj => obj.ContactId == "1")
                    .And(obj => obj.EmailAddresses.Any(m => m.Email == "test.addr.1@email.com"));

            var specification = new Specification<Contact>(obj => obj.Name == "Test User 1");

            findAllBySpec.FetchStrategy = new GenericFetchStrategy<Contact>();
            findAllBySpec.FetchStrategy
                .Include(obj => obj.EmailAddresses);

            // NOTE: This line will erase my FetchStrategy from above
            if (null != specification)
            {
                findAllBySpec = findAllBySpec.And(specification);
            }

            var contact = repository.FindAll(findAllBySpec).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1); // seems SqlServer does not makes statistic queries
            contact.EmailAddresses.First().Email.ShouldBe("test.addr.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);

            repository.FindAll(findAllBySpec).Count().ShouldBe(1);
        }
    }
}