using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        // first we create an internal varible to hold the DataContext 
        internal DataContext context;
        // then we set the dbset to a type of DbSet which is the table that we are going to pass 
        internal DbSet<T> dbset;

        // Next we create a contructor 
        // this passes in the a dataContext and then sets the database table to the table passed in T
        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbset = context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return dbset;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            // set a variable to the record 
            var t = Find(Id);
            if (context.Entry(t).State == EntityState.Detached)
                dbset.Attach(t);
            dbset.Remove(t);
        }

        public T Find(string Id)
        {
            return dbset.Find(Id);
        }

        public void Insert(T t)
        {
            dbset.Add(t);
        }

        public void Update(T t)
        {
            // first we have to attach the entity we want to update 
            dbset.Attach(t);
            // set the entry to a state of modifed  
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
