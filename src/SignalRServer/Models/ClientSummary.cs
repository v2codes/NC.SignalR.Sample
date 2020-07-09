using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientSummary
    {
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
        /// 角色类型：Teacher、Student
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 教师机Code
        /// </summary>
        public string TeacherCode { get; set; }

        /// <summary>
        /// 当前用户 Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 其他数据
        /// </summary>
        public object Data { get; set; }
    }
}
