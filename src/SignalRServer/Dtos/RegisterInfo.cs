using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Dtos
{

    /// <summary>
    /// 注册信息
    /// </summary>
    public class RegisterInfo
    {
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
