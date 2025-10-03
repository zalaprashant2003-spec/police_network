using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public class SQLPoliceRepository : IPoliceRepository
    {
        private readonly AppDbContext _context;

        public SQLPoliceRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public Police Add(Police police)
        {
            _context.Polices.Add(police);
            _context.SaveChanges();
            return police;
        }

        // Delete by Id
        public Police Delete(int id)
        {
            var police = _context.Polices.Find(id);
            if (police != null)
            {
                _context.Polices.Remove(police);
                _context.SaveChanges();
            }
            return police;
        }

        // Get all polices
        public IEnumerable<Police> GetAllPolices()
        {
            return _context.Polices;
        }

        // Get police by Id
        public Police GetPoliceById(int id)
        {
            return _context.Polices.Find(id);
        }

        // Update
        public Police Update(Police policeChanges)
        {
            var police = _context.Polices.Attach(policeChanges);
            police.State = EntityState.Modified;
            _context.SaveChanges();
            return policeChanges;
        }

        public bool isPolice(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Check if any police has this email
            return _context.Polices.Any(p => p.Name == email);
        }

    }
}
