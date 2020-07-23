using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Dtos
{
    /// <summary>
    /// AllowLogin 参数
    /// @Created by Leo 2020/7/21 17:49:58
    /// </summary>
    public class AllowLoginCmdParams : ICmdParams
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string PaperPolicy { get; set; }

        /// <summary>
        /// 试卷发放策略
        /// </summary>
        public string Description { get; set; }
    }
}
