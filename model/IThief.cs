using System.Collections.Generic;

namespace WebApplication1.Models
{
    public interface IThief
    {
        List<Thief> GetAll();
        Thief GetById(string id);
        void Insert(Thief thief);
        void Update(Thief thief);
        void Delete(Thief thief);
    }

}
