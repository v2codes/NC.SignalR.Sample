using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class EventType
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        public const string ReceiveMessage = "ReceiveMessage";

        /// <summary>
        /// 注册成功
        /// </summary>
        public const string RegisterSuccess = "RegisterSuccess";

        /// <summary>
        /// 开始考试
        /// </summary>
        public const string Start = "Start";
    }

    /// <summary>
    /// 消息类型 -- 前端接收消息方法名
    /// </summary>
    public class CommandType
    {
        /// <summary>
        /// 注册教师机
        /// </summary>
        public const string RegisterTeacher = "RegisterTeacher";

        /// <summary>
        /// 注册学生机
        /// </summary>
        public const string RegisterStudent = "RegisterStudent";

        /// <summary>
        /// 学生机接入
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
