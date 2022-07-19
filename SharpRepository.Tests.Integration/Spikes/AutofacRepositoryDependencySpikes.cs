﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using System;
using System.Reflection;
using Autofac;
using SharpRepository.Ioc.Autofac;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class RepositoryDependencySpikes
    {
        protected IContainer container;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                 .UseInMemoryDatabase("integration test")
                 .Options;

            var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json")
             .Build();

            var sectionName = "sharpRepository";

            IConfigurationSection sharpRepoSection = config.GetSection(sectionName);

            if (sharpRepoSection == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            var sharpRepoConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(sharpRepoSection);
            sharpRepoConfig.DefaultRepository = "efCoreRepos";
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var dbContext = new TestObjectContextCore(options);

            //// autofac            
            var builder = new ContainerBuilder();
            builder.Register<DbContext>(c => dbContext);
            builder.Register<TestObjectContextCore>(c => dbContext);
            builder.Register<IMemoryCache>(c => memoryCache);
            builder.RegisterSharpRepository(sharpRepoConfig);
            container = builder.Build();

            RepositoryDependencyResolver.SetDependencyResolver(new AutofacProvider(container));
        }

        class AutofacProvider : IServiceProvider
        {
           protected IContainer container;

           public AutofacProvider(IContainer container)
           {
               this.container = container;
           }

           public object GetService(Type serviceType)
           {
               return container.Resolve(serviceType);
           }
        }


        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Not_Require_ConnectionString()
        {
            var config = new EfCoreRepositoryConfiguration("TestConfig");
            var factory = new EfCoreConfigRepositoryFactory(config);

            factory.GetInstance<Contact, string>();
        }

        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Return_TestObjectEntites_Without_DbContextType_Defined()
        {
            var config = new EfCoreRepositoryConfiguration("TestConfig");
            var factory = new EfCoreConfigRepositoryFactory(config);

            var repos = factory.GetInstance<Contact, string>();

            var propInfo = repos.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext = (TestObjectContextCore)propInfo.GetValue(repos, null);
            dbContext.ShouldBeOfType<TestObjectContextCore>();
        }

        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Share_DbContext()
        {
            var config = new EfCoreRepositoryConfiguration("TestConfig", "tmp");
            var factory = new EfCoreConfigRepositoryFactory(config);

            var repos1 = factory.GetInstance<Contact, string>();
            var repos2 = factory.GetInstance<Contact, string>();

            // use reflecton to get the protected Context property
            var propInfo1 = repos1.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext1 = (TestObjectContextCore)propInfo1.GetValue(repos1, null);
            var propInfo2 = repos2.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext2 = (TestObjectContextCore)propInfo2.GetValue(repos2, null);

            dbContext1.ShouldBe(dbContext2);
        }
        
        [Test]
        public void Ioc_For_IRepository_T_TKey_Should_Be_EfRepository_T_TKey()
        {
            var repos = container.Resolve<IRepository<ContactType, int>>();
            repos.ShouldBeOfType<EfCoreRepository<ContactType, int>>();
        }

        [Test]
        public void Ioc_For_ICompoundKeyRepository_T_TKey_TKey2_Should_Be_EfRepository_T_TKey_TKey2()
        {
            var repos = container.Resolve<ICompoundKeyRepository<ContactType, int, string>>();
            repos.ShouldBeOfType<EfCoreRepository<ContactType, int, string>>();
        }

        [Test]
        public void Ioc_For_ICompoundRepository_T_TKey_TKey2_TKey3_Should_Be_EfRepository_T_TKey_TKey2_TKey3()
        {
            var repos = container.Resolve<ICompoundKeyRepository<ContactType, int, string, string>>();
            repos.ShouldBeOfType<EfCoreRepository<ContactType, int,string,string>>();
        }

        [Test]
        public void Ioc_For_ICompoundRepository_T_Should_Be_EfCompoundRepository_T()
        {
            var repos = container.Resolve<ICompoundKeyRepository<ContactType>>();
            repos.ShouldBeOfType<EfCoreCompoundKeyRepository<ContactType>>();
        }
    }
}