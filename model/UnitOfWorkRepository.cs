namespace WebApplication1.Models
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IThief _thiefRepository;
        private IFIR _firRepository;

        public UnitOfWorkRepository(AppDbContext context)
        {
            _context = context;
        }

        public IThief Thief
        {
            get
            {
                return _thiefRepository = _thiefRepository ?? new ThiefRepository(_context);
            }
        }

        public IFIR FIR
        {
            get
            {
                return _firRepository = _firRepository ?? new FIRRepository(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}
