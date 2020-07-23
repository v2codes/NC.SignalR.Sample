using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Dtos
{
    /// <summary>
    /// 注册信息
    /// @Created by Leo 2020/7/13 17:47:58
    /// </summary>
    public class RegisterCmdParams
    {
        /// <summary>
        /// 角色类型：Teacher、Student
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 教师机Code
        /// </summary>
        public string TeacherCode { get; set; }
    }
}
