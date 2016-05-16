using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            ICacheManager cacheManager = new CacheManager();
            var person = new Person()
            {
                Name = "Cuong"
            };
            cacheManager.Set("person", person);
            do
            {
                var a = Console.ReadLine();
                if (a == "a")
                {
                    if (cacheManager.IsExisted("person"))
                    {
                        Console.WriteLine(cacheManager.Get<Person>("person").Name);
                    }
                }
                else
                {
                    break;
                }
            } while (true);
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public interface ICacheManager
    {
        void Remove(string key);
        T Get<T>(string key);
        List<T> GetList<T>(string key);
        void Set<T>(string key, List<T> items);
        void Set<T>(string key, T item);
        void Clear();
        bool IsExisted(string key);
    }

    public class CacheManager : ICacheManager
    {
        private MemoryCache cache;
        private CacheItemPolicy cacheItemPolicy;
        public CacheManager()
        {
            cache = new MemoryCache("CachingProvider");
            cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(5000)
            };
        }
        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public T Get<T>(string key)
        {
            return (T)cache[key];
        }

        public List<T> GetList<T>(string key)
        {
            return (List<T>)cache.Get(key);
        }

        public void Set<T>(string key, List<T> items)
        {
            cache.Set(key, items, cacheItemPolicy);
        }

        public void Set<T>(string key, T item)
        {
            cache.Set(key, item, cacheItemPolicy);
        }

        public bool IsExisted(string key)
        {
            return cache.Any(c => c.Key == key);
        }

        public void Clear()
        {
            var keys = cache.Select(c => c.Key).ToList();
            keys.ForEach(k => cache.Remove(k));
        }
    }
}
