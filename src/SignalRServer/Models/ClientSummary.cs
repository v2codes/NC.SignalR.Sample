using SignalRServer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Models
{
    /// <summary>
    /// 客户端基本信息
    /// </summary>
    public abstract class ClientSummary : ICmdParams
    {
        /// <summary>
        /// 角色类型：Teacher、Student
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 是否连接中
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string RemoteIpAddress { get; set; }

        /// <summary>
        /// 开始连接时间
        /// </summary>
        public DateTimeOffset ConnectedTime { get; set; }

        /// <summary>
        /// 断开连接时间
        /// </summary>
        public DateTimeOffset DisconnectedTime { get; set; }

        /// <summary>
        /// 消息ID
        /// TODO：？？？
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// Proxy系统时间（Unix时间戳-毫秒）
        /// TODO：？？？
        /// </summary>
        public long SystemTime { get { return DateTimeOffset.Now.ToUnixTimeMilliseconds(); } }
    }

    /// <summary>
    /// 教师机客户端信息
    /// </summary>
    public class TeacherClientSummary : ClientSummary
    {
        public TeacherClientSummary()
        {
            ConnectedTime = DateTimeOffset.Now;
            StudentClients = new List<StudentClientSummary>();
        }
        /// <summary>
        /// 教师机Code
        /// </summary>
        public string TeacherCode { get; set; }

        /// <summary>
        /// 学生机列表
        /// </summary>
        public List<StudentClientSummary> StudentClients { get; set; }
    }

    /// <summary>
    /// 学生机机客户端信息
    /// </summary>
    public class StudentClientSummary : ClientSummary
    {
        public StudentClientSummary()
        {
            ConnectedTime = DateTimeOffset.Now;
        }
        /// <summary>
        /// 教师机Code
        /// </summary>
        public string TeacherCode { get; set; }
    }
}
