using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;

namespace MyShop.WebUI.UnitTest.Mocks
{
    public class MockContext<Generic> : IRepo<Generic> where Generic: BaseEntity
    {
        private List<Generic> items;
        private string className;

        public MockContext()
        {
            items = new List<Generic>();
        }

        public void Commit()
        {
            return;
        }

        public void Insert(Generic g)
        {
            items.Add(g);
        }

        public void Update(Generic g)
        {
            Generic gToUpdate = items.Find(i => i.Id == g.Id);

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
