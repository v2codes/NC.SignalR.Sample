using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using SignalRServer.Models;
using System.Threading.Tasks;

namespace SignalRServer.Storage
{
    /// <summary>
    /// 客户端信息存储
    /// @Created by Leo 2020/7/13 18:16:58
    /// </summary>
    public class ClientStorage
    {
        /// <summary>
        /// 教师机&学生机缓存列表
        /// </summary>
        public ConcurrentBag<TeacherClientSummary> TeacherClients { get; private set; }

        private readonly ILogger<ClientStorage> _logger;

        public ClientStorage(ILogger<ClientStorage> logger)
        {
            TeacherClients = new ConcurrentBag<TeacherClientSummary>();
            _logger = logger;
        }

        /// <summary>
        /// 持久化教师机信息
        /// </summary>
        /// <param name="client"></param>
        public void AddTeacherClient(TeacherClientSummary client)
        {
            TeacherClients.Add(client);
        }

        /// <summary>
        /// 持久化学生机信息
        /// </summary>
        /// <param name="client"></param>
        public void AddStudent(StudentClientSummary client)
        {
            var teacherClient = TeacherClients.FirstOrDefault(p => p.TeacherCode == client.TeacherCode);
            if (teacherClient.StudentClients == null)
            {
                teacherClient.StudentClients = new List<StudentClientSummary>();
            }

            teacherClient.StudentClients.Add(client);
        }

        /// <summary>
        /// 根据学生机 ConnectionId 获取教师机信息
        /// </summary>
        /// <param name="studentConnId"></param>
        /// <returns></returns>
        public TeacherClientSummary GetTeacherClient(string studentConnId)
        {
            TeacherClientSummary teacherClient = null;
            foreach (var tclient in TeacherClients)
            {
                var exists = tclient.StudentClients.Any(p => p.ConnectionId == studentConnId);
                if (exists)
                {
                    teacherClient = tclient;
                    break;
                }
            }
            return teacherClient;
        }

        /// <summary>
        /// 客户端连接、断开状态更新
        /// </summary>
        /// <param name="connectionId"></param>
        public void UpdateConnectedState(string connectionId, bool isConnected)
        {
            var teacher = TeacherClients.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (teacher != null)
            {
                teacher.IsConnected = isConnected;
                if (!isConnected)
                {
                    teacher.DisconnectedTime = DateTimeOffset.Now;
                }
                // TODO 通知所有学生机
                // ...
            }
            else
            {
                foreach (var tclient in TeacherClients)
                {
                    var student = tclient.StudentClients.FirstOrDefault(p => p.ConnectionId == connectionId);
                    if (student != null)
                    {
                        student.IsConnected = isConnected;
                        if (!isConnected)
                        {
                            student.DisconnectedTime = DateTimeOffset.Now;
                        }
                        // TODO 通知教师机
                        //...
                        break;
                    }
                }
            }
        }

        #region  XXXXXXXXXXXXXXX
        ///// <summary>
        ///// 教师机缓存列表
        ///// </summary>
        //public ConcurrentBag<ClientSummary> TeacherList { get; private set; }

        ///// <summary>
        ///// 学生机缓存列表 
        ///// TODO，存储学生机和教师机关联方式
        ///// </summary>
        //public ConcurrentDictionary<string, List<ClientSummary>> StudentList { get; private set; }

        //private readonly ILogger<ClientStorage> _logger;

        //public ClientStorage(ILogger<ClientStorage> logger)
        //{
        //    TeacherList = new ConcurrentBag<ClientSummary>();
        //    StudentList = new ConcurrentDictionary<string, List<ClientSummary>>();
        //    _logger = logger;
        //}

        ///// <summary>
        ///// 持久化教师机信息
        ///// </summary>
        ///// <param name="clientSummary"></param>
        //public void AddTeacher(ClientSummary clientSummary)
        //{
        //    TeacherList.Add(clientSummary);
        //}

        ///// <summary>
        ///// 持久化学生机信息
        ///// </summary>
        ///// <param name="clientSummary"></param>
        //public void AddStudent(ClientSummary clientSummary)
        //{
        //    if (StudentList.ContainsKey(clientSummary.TeacherCode))
        //    {
        //        StudentList[clientSummary.TeacherCode].Add(clientSummary);
        //    }
        //    else
        //    {
        //        var list = new List<ClientSummary>() { clientSummary };
        //        StudentList.TryAdd(clientSummary.TeacherCode, list);
        //    }
        //}

        ///// <summary>
        ///// 根据学生机 ConnectionId 获取教师机信息
        ///// </summary>
        ///// <param name="studentConnId"></param>
        ///// <returns></returns>
        //public ClientSummary GetTeacher(string studentConnId)
        //{
        //    ClientSummary teacher = null;
        //    StudentList.ForEach(kv =>
        //    {
        //        var student = kv.Value.FirstOrDefault(p => p.ConnectionId == studentConnId);
        //        if (student != null)
        //        {
        //            teacher = TeacherList.FirstOrDefault(p => p.TeacherCode == student.TeacherCode);
        //        }
        //    });

        //    return teacher;
        //}

        ///// <summary>
        ///// 客户端连接、断开状态更新
        ///// </summary>
        ///// <param name="connectionId"></param>
        //public void UpdateConnectedState(string connectionId, bool isConnected)
        //{
        //    var teacher = TeacherList.FirstOrDefault(p => p.ConnectionId == connectionId);
        //    if (teacher != null)
        //    {
        //        //TeacherList.TryTake(out teacher);
        //        teacher.IsConnected = isConnected;
        //        // TODO 通知所有学生机
        //        // ...
        //    }
        //    else
        //    {
        //        foreach (var students in StudentList)
        //        {
        //            var student = students.Value.FirstOrDefault(p => p.ConnectionId == connectionId);
        //            if (student != null)
        //            {
        //                //students.Value.Remove(student);
        //                student.IsConnected = isConnected;
        //                // TODO 通知教师机
        //                //...
        //                break;
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
