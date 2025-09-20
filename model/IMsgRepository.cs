using System.Collections.Generic;

namespace WebApplication1.Models
{
    public interface IMsgRepository
    {
        Msg GetMsgById(int id);
        IEnumerable<Msg> GetAllMsgs();
        Msg Add(Msg msg);
        Msg Update(Msg msgChanges);
        Msg Delete(int id);
    }
}
