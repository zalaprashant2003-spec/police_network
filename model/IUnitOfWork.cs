namespace WebApplication1.Models
{
    public interface IUnitOfWork
    {
        IThief Thief { get; }
        IFIR FIR { get; }
        void Save();
    }

}
