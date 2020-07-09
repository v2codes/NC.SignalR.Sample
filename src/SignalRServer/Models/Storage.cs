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
        public static ConcurrentBag<ClientSummary> TeacherList { get; private set; }

        /// <summary>
        /// 学生机缓存列表
        /// </summary>
        public static ConcurrentDictionary<string, List<ClientSummary>> StudentList { get; private set; }

        static Storage()
        {
            TeacherList = new ConcurrentBag<ClientSummary>();
            StudentList = new ConcurrentDictionary<string, List<ClientSummary>>();
        }

        /// <summary>
        /// 持久化教师机信息
        /// </summary>
        /// <param name="clientSummary"></param>
        public static void AddTeacher(ClientSummary clientSummary)
        {
            TeacherList.Add(clientSummary);
        }

        /// <summary>
        /// 持久化学生机信息
        /// </summary>
        /// <param name="clientSummary"></param>
        public static void AddStudent(ClientSummary clientSummary)
        {
            if (StudentList.ContainsKey(clientSummary.TeacherCode))
            {
                StudentList[clientSummary.TeacherCode].Add(clientSummary);
            }
            else
            {
                var list = new List<ClientSummary>() { clientSummary };
                StudentList.TryAdd(clientSummary.TeacherCode, list);
            }
        }

        /// <summary>
        /// 客户端连接、断开状态更新
        /// </summary>
        /// <param name="connectionId"></param>
        public static void UpdateConnectedState(string connectionId, bool IsConnected)
        {
            var teacher = TeacherList.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (teacher != null)
            {
                //TeacherList.TryTake(out teacher);
                teacher.IsConnected = false;
                // TODO 通知所有学生机
                //...
            }
            else
            {
                foreach (var students in StudentList)
                {
                    var student = students.Value.FirstOrDefault(p => p.ConnectionId == connectionId);
                    if (student != null)
                    {
                        //students.Value.Remove(student);
                        student.IsConnected = false;
                        // TODO 通知教师机
                        //...
                        break;
                    }
                }
            }
        }
    }
}
