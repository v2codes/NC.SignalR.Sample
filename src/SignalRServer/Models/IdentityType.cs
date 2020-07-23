using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Models
{
    /// <summary>
    /// 角色类型
    /// @Created by Leo 2020/7/13 17:49:20
    /// </summary>
    public class IdentityType
    {
        /// <summary>
        /// 教师机
        /// </summary>
        public static readonly string Teacher = "Teacher";

        /// <summary>
        /// 学生机
        /// </summary>
        public static readonly string Student = "Student";

        /// <summary>
        /// Proxy
        /// </summary>
        public static readonly string Proxy = "Proxy";

        /// <summary>
        /// 教师机守护进程
        /// </summary>
        public static readonly string DaemonAgent = "DaemonAgent";
    }
}
