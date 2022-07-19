using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepo<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet;

        public SQLRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var g = Find(Id);
            if (context.Entry(g).State == EntityState.Detached)
                dbSet.Attach(g);

            dbSet.Remove(g);
        }

        public T Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(T g)
        {
            dbSet.Add(g);
        }

        public void Update(T g)
        {
            dbSet.Attach(g);
            context.Entry(g).State = EntityState.Modified;
        }
    }
}
