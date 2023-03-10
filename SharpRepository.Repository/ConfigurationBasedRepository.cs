using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;
using SharpRepository.Repository.Configuration;
using Microsoft.Extensions.Options;

namespace SharpRepository.Repository
{
    public class ConfigurationBasedRepository<T> : ConfigurationBasedRepository<T, int> where T : class, new()
    {
        public ConfigurationBasedRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null) : base(configuration, repositoryName)
        {
        }
    }

    /// <summary>
    /// Inherit from this when you want to create a custom Repository and have the specific type based on the configuration file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ConfigurationBasedRepository<T, TKey> : IRepository<T, TKey> where T : class, new()
    {
        protected readonly IRepository<T, TKey> Repository;
        
        public ConfigurationBasedRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            Repository = RepositoryFactory.GetInstance<T, TKey>(configuration, repositoryName);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        public IQueryable<T> AsQueryable()
        {
            return Repository.AsQueryable();
        }

        public IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector,
                                                                    Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class
        {
            return Repository.Join(innerRepository, outerKeySelector, innerKeySelector, resultSelector);
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(fetchStrategy);
        }

        public IEnumerable<T> GetAll(params string[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(queryOptions);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(queryOptions, fetchStrategy);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.GetAll(selector);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(selector, queryOptions);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, queryOptions, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, queryOptions);
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, selector, queryOptions);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return TryFind(predicate, out T entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
        {
            return Repository.TryFind(predicate, out entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(predicate, queryOptions, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, queryOptions, out entity);
        }

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, queryOptions);
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, selector, queryOptions);
        }

        public bool Exists(ISpecification<T> criteria)
        {
            return Repository.Exists(criteria);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return Repository.TryFind(criteria, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(criteria, queryOptions, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, queryOptions,  out entity);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, selector, queryOptions);
        }

        public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, selector, queryOptions);
        }

        public IRepositoryConventions Conventions
        {
            get { return Repository.Conventions; }
            set { Repository.Conventions = value;  }
        }

        public Type EntityType
        {
            get { return Repository.EntityType; }
        }

        public Type KeyType
        {
            get { return Repository.KeyType; }
        }

        public T Get(TKey key, params string[] includePathes)
        {
            return Repository.Get(key, includePathes);
        }

        public T Get(TKey key, params Expression<Func<T, object>>[] includePathes)
        {
            return Repository.Get(key, includePathes);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePathes)
        {
            return Repository.Get(key, selector, includePathes);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params string[] includePathes)
        {
            return Repository.Get(key, selector, includePathes);
        }

        public T Get(TKey key)
        {
            return Repository.Get(key);
        }

        public T Get(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.Get(key, fetchStrategy);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            return Repository.Get(key, selector);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.Get(key, selector, fetchStrategy);
        }

        public IEnumerable<T> GetMany(params TKey[] keys)
        {
            return Repository.GetMany(keys);
        }

        public IEnumerable<T> GetMany(IEnumerable<TKey> keys)
        {
            return Repository.GetMany(keys);
        }

        public IEnumerable<T> GetMany(IEnumerable<TKey> keys, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetMany(keys, fetchStrategy);
        }

        public IEnumerable<TResult> GetMany<TResult>(Expression<Func<T, TResult>> selector, params TKey[] keys)
        {
            return Repository.GetMany(selector, keys);
        }

        public IEnumerable<TResult> GetMany<TResult>(IEnumerable<TKey> keys, Expression<Func<T, TResult>> selector)
        {
            return Repository.GetMany(keys, selector);
        }

        public IDictionary<TKey, T> GetManyAsDictionary(params TKey[] keys)
        {
            return Repository.GetManyAsDictionary(keys);
        }

        public IDictionary<TKey, T> GetManyAsDictionary(IEnumerable<TKey> keys)
        {
            return Repository.GetManyAsDictionary(keys);
        }

        public IDictionary<TKey, T> GetManyAsDictionary(IEnumerable<TKey> keys, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetManyAsDictionary(keys, fetchStrategy);
        }

        public bool Exists(TKey key)
        {
            return TryGet(key, out T entity);
        }

        public bool TryGet(TKey key, out T entity)
        {
            return Repository.TryGet(key, out entity);
        }

        public bool TryGet<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryGet(key, selector, out entity);
        }

        public void Add(T entity)
        {
            Repository.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            Repository.Add(entities);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Repository.Update(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Repository.Delete(entities);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Repository.Delete(predicate);
        }

        public void Delete(ISpecification<T> criteria)
        {
            Repository.Delete(criteria);
        }

        public void Delete(TKey key)
        {
            Repository.Delete(key);
        }

        public void Delete(IEnumerable<TKey> keys)
        {
            Repository.Delete(keys);
        }

        public void Delete(params TKey[] keys)
        {
            Repository.Delete(keys);
        }

        public IBatch<T> BeginBatch()
        {
            return Repository.BeginBatch();
        }

        public IDisabledCache DisableCaching()
        {
            return Repository.DisableCaching();
        }

        public void ClearCache()
        {
            Repository.ClearCache();
        }

        public string TraceInfo
        {
            get { return Repository.TraceInfo; }
        }

        public bool GenerateKeyOnAdd
        {
            get { return Repository.GenerateKeyOnAdd; }
            set { Repository.GenerateKeyOnAdd = value; }
        }

        public TKey GetPrimaryKey(T entity)
        {
            return Repository.GetPrimaryKey(entity);
        }

        public ICachingStrategy<T, TKey> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }

        public bool CachingEnabled
        {
            get { return Repository.CachingEnabled; }
            set { Repository.CachingEnabled = value; }
        }

        public bool CacheUsed
        {
            get { return Repository.CacheUsed; }
        }

        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return Repository.GroupBy(keySelector, resultSelector);
        }

        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return Repository.GroupBy(criteria, keySelector, resultSelector);
        }

        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return Repository.GroupBy(predicate, keySelector, resultSelector);
        }

        public int Count()
        {
            return Repository.Count();
        }

        public int Count(ISpecification<T> criteria)
        {
            return Repository.Count(criteria);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Repository.Count(predicate);
        }

        public long LongCount()
        {
            return Repository.LongCount();
        }

        public long LongCount(ISpecification<T> criteria)
        {
            return Repository.LongCount(criteria);
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return Repository.LongCount(predicate);
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return Repository.Sum(selector);
        }

        public int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public int? Sum(Expression<Func<T, int?>> selector)
        {
            return Repository.Sum(selector);
        }

        public int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public long Sum(Expression<Func<T, long>> selector)
        {
            return Repository.Sum(selector);
        }

        public long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public long? Sum(Expression<Func<T, long?>> selector)
        {
            return Repository.Sum(selector);
        }

        public long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return Repository.Sum(selector);
        }

        public decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public decimal? Sum(Expression<Func<T, decimal?>> selector)
        {
            return Repository.Sum(selector);
        }

        public decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public double Sum(Expression<Func<T, double>> selector)
        {
            return Repository.Sum(selector);
        }

        public double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public double Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public double? Sum(Expression<Func<T, double?>> selector)
        {
            return Repository.Sum(selector);
        }

        public double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public double? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public float Sum(Expression<Func<T, float>> selector)
        {
            return Repository.Sum(selector);
        }

        public float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public float? Sum(Expression<Func<T, float?>> selector)
        {
            return Repository.Sum(selector);
        }

        public float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return Repository.Sum(criteria, selector);
        }

        public float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Repository.Sum(predicate, selector);
        }

        public double Average(Expression<Func<T, int>> selector)
        {
            return Repository.Average(selector);
        }

        public double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public double? Average(Expression<Func<T, int?>> selector)
        {
            return Repository.Average(selector);
        }

        public double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public double Average(Expression<Func<T, long>> selector)
        {
            return Repository.Average(selector);
        }

        public double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public double? Average(Expression<Func<T, long?>> selector)
        {
            return Repository.Average(selector);
        }

        public double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public decimal Average(Expression<Func<T, decimal>> selector)
        {
            return Repository.Average(selector);
        }

        public decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public decimal Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public decimal? Average(Expression<Func<T, decimal?>> selector)
        {
            return Repository.Average(selector);
        }

        public decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public decimal? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public double Average(Expression<Func<T, double>> selector)
        {
            return Repository.Average(selector);
        }

        public double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public double? Average(Expression<Func<T, double?>> selector)
        {
            return Repository.Average(selector);
        }

        public double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public float Average(Expression<Func<T, float>> selector)
        {
            return Repository.Average(selector);
        }

        public float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public float Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public float? Average(Expression<Func<T, float?>> selector)
        {
            return Repository.Average(selector);
        }

        public float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return Repository.Average(criteria, selector);
        }

        public float? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Repository.Average(predicate, selector);
        }

        public TResult Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.Min(selector);
        }

        public TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            return Repository.Min(criteria, selector);
        }

        public TResult Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Repository.Min(predicate, selector);
        }

        public TResult Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.Max(selector);
        }

        public TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            return Repository.Max(criteria, selector);
        }

        public TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Repository.Max(predicate, selector);
        }

        public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupCount(selector);
        }

        public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupCount(criteria, selector);
        }

        public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupCount(predicate, selector);
        }

        public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupLongCount(selector);
        }

        public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupLongCount(criteria, selector);
        }

        public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector)
        {
            return Repository.GroupLongCount(predicate, selector);
        }
    }
}
