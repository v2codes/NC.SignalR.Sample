using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Models
{
    /// <summary>
    /// 消息指令类型
    /// @Created by Leo 2020/7/13 17:50:07
    /// </summary>
    public class CommandType
    {
        #region 教师机接收通知
        /// <summary>
        /// 学生机连接成功
        /// </summary>
        public const string StudentConnected = "s.connected";

        /// <summary>
        /// 学生机断开
        /// </summary>
        public const string StudentDisconnected = "s.disconnected";
        #endregion

        #region 教师机发出指令
        /// <summary>
        /// (教师机) 允许登录
        /// open
        /// </summary>
        public const string AllowLogin = "t.allowLogin";

        /// <summary>
        /// 登录成功
        /// </summary>
        public const string LoginSuccess = "s.LoginSuccess";

        /// <summary>
        /// 拒绝登录
        /// </summary>
        public const string LoginDenied = "s.loginDenied";

        /// <summary>
        /// (教师机) 修改学生机座位号
        /// modify:number
        /// </summary>
        public const string ModifySeatNumber = "t.modifySeatNumber";

        /// <summary>
        /// (教师机) 获取学生机状态信息
        /// student:getstatus                    0
        /// </summary>
        public const string StudentGetStatus = "t.studentGetStatus";

        /// <summary>
        /// (教师机) 接收到学生端请求指令(commandStatus)，返回学生当前的操作状态
        /// commandstatus:return
        /// </summary>
        public const string CommandStatusReturn = "t.commandStatusReturn";

        /// <summary>
        /// (教师机) 响应学生机主动请求开始考试
        /// applystart:return
        /// </summary>
        public const string ApplyStartReturn = "t.applyStartReturn";

        /// <summary>
        /// (教师机) 开始考试
        /// start:manual
        /// </summary>
        public const string StartManual = "t.startManual";

        /// <summary>
        /// (教师机) 结束考试
        /// stop:manual
        /// </summary>
        public const string StopManual = "t.stopManual";

        /// <summary>
        /// (教师机)回收试卷
        /// recycle
        /// </summary>
        public const string Recycle = "t.recycle";

        /// <summary>
        /// (教师机)关闭学生机
        /// exit
        /// </summary>
        public const string Exit = "t.exit";

        /// <summary>
        /// (教师机)清屏操作
        /// clean
        /// </summary>
        public const string Clean = "t.clean";

        /// <summary>
        /// (教师机) 设置考试失败教师机置该学生机失败
        /// manualFaile
        /// </summary>
        public const string ManualFaile = "t.manualFaile";
        
        /// <summary>
        /// (教师机) 任务被XX教师终止
        /// taskStop
        /// </summary>
        public const string TaskStop = "t.taskStop";

        /// <summary>
        /// (教师机) 收到结束考试指令后
        /// beforeProcess
        /// </summary>
        public const string BeforeProcess = "t.beforeProcess";

        /// <summary>
        /// (教师机) 获取未练习试卷列表，任务中的试卷列表
        /// paperused:return
        /// </summary>
        public const string PaperUsedReturn = "t.paperUsedReturn";

        #endregion

        #region 学生机发出指令

        /// <summary>
        /// (学生机) 登录
        /// login
        /// </summary>
        public const string Login = "s.login";

        /// <summary>
        /// (学生机) 登录成功后发送身份确认指令
        /// verification
        /// </summary>
        public const string  Verification = "s.verification";

        /// <summary>
        /// (学生机) 询问教师机现在是在哪一步
        /// 连接成功后问教师机现在是在哪一步，只有在考中才进行如下处理
        /// commandstatus
        /// </summary>
        public const string CommandStatus = "s.commandStatus";

        /// <summary>
        /// (学生机) 获取未练习试卷列表，任务中的试卷列表
        /// paperused
        /// </summary>
        public const string PaperUsed = "s.paperUsed";

        /// <summary>
        /// (学生机) 报告教师机最新状态       
        /// student:status
        /// </summary>
        public const string StudentStatus = "s.studentStatus";

        /// <summary>
        /// (学生机) 更新学生状态 monitoringStatus = MS_1
        /// watchStatus
        /// </summary>
        public const string WatchStatus = "s.watchStatus";

        /// <summary>
        /// (学生机) 更新学生考试状态
        /// student:examstatus
        /// 学生端耳机掉落：examstatus: "MS_12"
        /// </summary>
        public const string SetExamStatus = "s.setExamStatus";

        /// <summary>
        /// (学生机) 试卷包下载状态更新
        /// paper:down
        /// </summary>
        public const string PaperDown = "s.paperDown";

        /// <summary>
        /// (学生机) 录音测试
        /// check:wavein
        /// </summary>
        public const string CheckWavein = "s.checkWavein";

        /// <summary>
        /// (学生机) 放音测试
        /// check:waveout
        /// </summary>
        public const string CheckWaveout = "s.checkWaveout";

        /// <summary>
        /// (学生机) 学生机主动请求开始考试
        /// applystart
        /// </summary>
        public const string ApplyStart = "s.applyStart";

        /// <summary>
        /// (学生机) 学生考试过程信息推送
        /// progress
        /// </summary>
        public const string Progress = "s.progress";

        /// <summary>
        /// (学生机) 学生答题完成
        /// complete
        /// </summary>
        public const string Complete = "s.complete";

        /// <summary>
        /// (学生机) 回收答卷包
        /// recycle:reply
        /// </summary>
        public const string RecycleReply = "s.recycleReply";

        /// <summary>
        /// (学生机) 举手
        /// help
        /// </summary>
        public const string Help = "s.help";
        #endregion

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
}
