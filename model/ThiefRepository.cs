using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public class ThiefRepository : IThief
    {
        private readonly AppDbContext _context;

        public ThiefRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Thief> GetAll()
        {
            return _context.Thieves.ToList();
        }

        public Thief GetById(string id)
        {
            return _context.Thieves.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(Thief thief)
        {
            thief.Id = Guid.NewGuid().ToString();
            _context.Thieves.Add(thief);
        }

        public void Update(Thief thief)
        {
            _context.Thieves.Update(thief);
        }

        public void Delete(Thief thief)
        {
            _context.Thieves.Remove(thief);
        }
    }

}
