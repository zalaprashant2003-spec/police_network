using System.Collections.Generic;

namespace WebApplication1.Models
{
    public interface IFIR
    {
        List<FIR> GetAll();
        FIR GetById(string id);
        void Insert(FIR fir);
        void Update(FIR fir);
        void Delete(FIR fir);
    }

}
