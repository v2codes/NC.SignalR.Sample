using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    public class Storage
    {
        /// <summary>
        /// 教师机缓存列表
        /// </summary>
        public static ConcurrentBag<ClientSummary> TeacherList { get; set; }

        /// <summary>
        /// 学生机缓存列表
        /// </summary>
        public static ConcurrentDictionary<string, List<ClientSummary>> StudentList { get; set; }

        static Storage()
        {
            TeacherList = new ConcurrentBag<ClientSummary>();
            StudentList = new ConcurrentDictionary<string, List<ClientSummary>>();
        }
    }
}
