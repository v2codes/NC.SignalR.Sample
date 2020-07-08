using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 可用命令配置
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
        /// 开始考试
        /// </summary>
        public const string Start = "Start";

        /// <summary>
        /// 结束考试
        /// </summary>
        public const string Stop = "Stop";

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
