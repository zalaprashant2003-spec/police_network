//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using WebApplication1.Data;

//namespace WebApplication1.Models
//{
//    public class SQLMsgRepository : IMsgRepository
//    {
//        //private readonly ApplicationDbContext context;
//        private readonly AppDbContext _context;


//        public SQLMsgRepository(ApplicationDbContext context)
//        {
//            this.context = context;
//        }

//        // Create
//        public Msg Add(Msg msg)
//        {
//            context.Messages.Add(msg);
//            context.SaveChanges();
//            return msg;
//        }

//        // Delete by Id
//        public Msg Delete(int id)
//        {
//            Msg msg = context.Messages.Find(id);
//            if (msg != null)
//            {
//                context.Messages.Remove(msg);
//                context.SaveChanges();
//            }
//            return msg;
//        }

//        // Get all messages
//        public IEnumerable<Msg> GetAllMsgs()
//        {
//            return context.Messages;
//        }

//        // Get message by Id
//        public Msg GetMsgById(int id)
//        {
//            return context.Messages.Find(id);
//        }

//        // Update
//        public Msg Update(Msg msgChanges)
//        {
//            var msg = context.Messages.Attach(msgChanges);
//            msg.State = EntityState.Modified;
//            context.SaveChanges();
//            return msgChanges;
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication1.Models; // for Msg

namespace WebApplication1.Models
{
    public class SQLMsgRepository : IMsgRepository
    {
        private readonly AppDbContext _context;

        // Constructor injection of AppDbContext
        public SQLMsgRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public Msg Add(Msg msg)
        {
            _context.Messages.Add(msg);
            _context.SaveChanges();
            return msg;
        }

        // Delete by Id
        public Msg Delete(int id)
        {
            Msg msg = _context.Messages.Find(id);
            if (msg != null)
            {
                _context.Messages.Remove(msg);
                _context.SaveChanges();
            }
            return msg;
        }

        // Get all messages
        public IEnumerable<Msg> GetAllMsgs()
        {
            return _context.Messages;
        }

        // Get message by Id
        public Msg GetMsgById(int id)
        {
            return _context.Messages.Find(id);
        }

        // Update
        public Msg Update(Msg msgChanges)
        {
            var msg = _context.Messages.Attach(msgChanges);
            msg.State = EntityState.Modified;
            _context.SaveChanges();
            return msgChanges;
        }
    }
}
