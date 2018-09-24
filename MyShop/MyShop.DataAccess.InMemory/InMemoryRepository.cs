using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    // this is to ccreate a generic repository 
    // <T> is a placeholder and can be called whatever you want as it will be replaced at RunTime
    // Where T : BaseEntity - this means that T will always call the Base Entity class to get an ID 
    public class InMemoryRepository<T> where T : BaseEntity 
    {
        // This creates the memory cahe 
        ObjectCache cache = MemoryCache.Default;
        // this creates a generic list with the place holder as a name 
        List<T> items;
        // this is a seperate varabile so we can name the list 
        string ClassName;

        public InMemoryRepository()
        {
            // we set the class name using the typeof method which will get its name from the (T) which will be passed
            ClassName = typeof(T).Name;
            // next we set the cache to the list 
            items = cache[ClassName] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[ClassName] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(ClassName + " not Found!");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(ClassName + " not found!");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T toDelete = items.Find(i => i.Id == Id);

            if (toDelete!=null)
            {
                items.Remove(toDelete);
            }
            else
            {
                throw new Exception(ClassName + " Not found!");
            }


        }


    }
}
