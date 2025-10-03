using AutoMapper;
using System.Linq;

namespace WebApplication1.Models
{
    public class Helper : Profile
    {
        public Helper()
        {
            CreateMap<Thief, ThiefViewModel>().ReverseMap();
            CreateMap<CreateThiefViewModel, Thief>();
            CreateMap<FIR, FIRViewModel>();
        }
    }

}
