using System.Collections.Generic;

namespace WebApplication1.Models
{
    public interface IPoliceRepository
    {
        Police GetPoliceById(int id);
        IEnumerable<Police> GetAllPolices();
        Police Add(Police police);
        Police Update(Police policeChanges);
        Police Delete(int id);
        bool isPolice(string name); 
    }

}
