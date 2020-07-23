using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Dtos
{
    /// <summary>
    /// 注册信息
    /// @Created by Leo 2020/7/13 17:47:58
    /// </summary>
    public class StatusCmdParams : ICmdParams
    {
        /// <summary>
        /// 教师机Code
        /// </summary>
        public string TeacherCode { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }
    }
}
