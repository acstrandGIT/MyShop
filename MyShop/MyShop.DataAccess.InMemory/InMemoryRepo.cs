using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepo<Generic> where Generic : BaseEntity
    {
        private ObjectCache cache = MemoryCache.Default;
        private List<Generic> items;
        private string className;

        public InMemoryRepo()
        {
            className = typeof(Generic).Name;
            items = cache[className] as List<Generic>;
            if (items == null)
            {
                items = new List<Generic>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(Generic g)
        {
            items.Add(g);
        }

        public void Update(Generic g)
        {
            Generic gToUpdate = items.Find(i=>i.Id == g.Id);

            if (gToUpdate != null)
            {
                gToUpdate = g;
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }

        public Generic Find(string Id)
        {
            Generic g = items.Find(i => i.Id == Id);
            if (g != null)
            {
                return g;
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }

        public IQueryable<Generic> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(String Id)
        {
            Generic gToDelete = items.Find(i => i.Id == Id);

            if (gToDelete != null)
            {
                items.Remove(gToDelete);
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }


    }
}
