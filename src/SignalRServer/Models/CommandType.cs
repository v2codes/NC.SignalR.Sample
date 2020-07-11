using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 消息类型 -- 前端接收消息方法名
    /// </summary>
    public class CommandType
    {
        /// <summary>
        /// 注册教师机、学生机
        /// </summary>
        public const string Register = "Register";

        /// <summary>
        /// 学生机连接成功
        /// </summary>
        public const string StudentConnected = "StudentConnected";

        /// <summary>
        /// 开始考试
        /// </summary>
        public const string Start = "Start";

        /// <summary>
        /// 暂停考试
        /// </summary>
        public const string Pause = "Pause";

        /// <summary>
        /// 结束考试
        /// </summary>
        public const string Stop = "Stop";

        /// <summary>
        /// 上报状态
        /// </summary>
        public const string Status = "Status";

    }

    /// <summary>
    /// 角色类型
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
    }
}
