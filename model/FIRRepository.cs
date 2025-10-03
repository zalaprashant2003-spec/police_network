using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Models
{
    public class FIRRepository : IFIR
    {
        private readonly AppDbContext _context;

        public FIRRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<FIR> GetAll()
        {
            return _context.FIRs.ToList();
        }

        public FIR GetById(string id)
        {
            return _context.FIRs
        .Include(f => f.FIRThieves)       // include the join table
            .ThenInclude(ft => ft.Thief)  // include the related Thief entity
        .FirstOrDefault(x => x.Id == id);
        }

        public void Insert(FIR fir)
        {
            fir.Id = Guid.NewGuid().ToString();
            _context.FIRs.Add(fir);
        }

        public void Update(FIR fir)
        {
            _context.FIRs.Update(fir);
        }

        public void Delete(FIR fir)
        {
            _context.FIRs.Remove(fir);
        }
    }

}
