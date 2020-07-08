using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{

    public class TeacherRegisterInfo
    {
        public string Code { get; set; }
        public string Data { get; set; }
    }

    public class StudentRegisterInfo
    {
        public string Code { get; set; }
        public string TeacherCode { get; set; }
        public string Data { get; set; }
    }
}
