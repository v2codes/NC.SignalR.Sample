using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Models
{
    /// <summary>
    /// 依赖注入别名
    /// @Created by Leo 2020/7/21 15:07:30
    /// </summary>
    public class InjectNamedAttribute : Attribute
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Named { get; set; }

        public InjectNamedAttribute(string named)
        {
            this.Named = named;
        }
    }
}
