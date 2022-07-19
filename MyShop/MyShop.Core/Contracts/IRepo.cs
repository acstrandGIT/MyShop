using System;
using System.Linq;
using MyShop.Core.Models;



namespace MyShop.Core.Contracts
{
    public interface IRepo<Generic> where Generic : BaseEntity
    {
        void Commit();
        void Insert(Generic g);
        void Update(Generic g);
        Generic Find(string Id);
        IQueryable<Generic> Collection();
        void Delete(String Id);
    }
}