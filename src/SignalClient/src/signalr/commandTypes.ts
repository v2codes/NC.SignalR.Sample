/**
 * 角色类型
 */
export enum IdentityType {
    /**
     * 教师机
     */
    Teacher = 'Teacher',

    /**
     * 学生机
     */
    Student = 'Student'
}

/**
 * 发送消息类型
 * 对应Server端hub方法名
 */
export enum ServerEventType {
    /**
    * 注册教师机 & 学生机
    */
    RegisterTeacher = 'RegisterTeacher',

    /**
    * 注册教师机 & 学生机
    */
    RegisterStudent = 'RegisterStudent',

    /**
     * 发送普通指令消息
     */
    SendMessage = 'SendMessage',

    /**
     * 发送消息反馈
     */
    Feedback = 'Feedback',

    /**
     * Start
     */
    Start = 'Start',

    /**
     * Stop
     */
    Stop = 'Stop',
}

/**
 * 接收消息类型
 * 对应后端发送消息类型， 对应 ISignalrClient 定义
 */
export enum ClientEventType {
    /**
     * 注册成功通知消息类型
     */
    RegisterSuccess = 'RegisterSuccess',

    /**
     * 接收消息类型
     */
    ReceiveMessage = 'ReceiveMessage',

    /**
     * 服务端异常消息通知
     */
    ReceiveServerError = 'ReceiveServerError',

    /**
     * 消息发出后，超时未收到反馈通知
     * 发出指令消息后，服务端在规定时间内（默认3s）未收到接收方发送"Feedback"时，通知该指令发送方
     */
    Timeout = 'Timeout',

    /**
     * 断开连接消息类型
     */
    CloseConnection = 'CloseConnection',
}

/**
 * 教师机&学生机交互指令类型
 */
export enum CommandType {

    /************************* 教师机收到发出指令 *****************************/
    /**
     * 学生机加入
     */
    studentConnected = 's.connected',

    /**
     * 学生机断开链接
     */
    studentDisonnected = 's.disconnected',
    
    /**
     * 学生机上报状态
     */
    Status = 'Status',

    /************************* 教师机发出指令 *****************************/

     /**
     * (教师机) 允许登录
     * open
     */
    allowLogin = 't.allowLogin',

    /**
     *  (教师机) 登录成功
     */
    loginAllow = 's.loginAllow',

    /**
     * (教师机) 拒绝登录
     */
    loginDenied = 's.loginDenied',

    /**
     * (教师机) 修改学生机座位号
     * modify:number
     */
    modifySeatNumber = 't.modifySeatNumber',

    /**
     * (教师机) 获取学生机状态信息
     * student:getstatus
     */
    studentGetStatus = 't.studentGetStatus',

    /**
     * (教师机) 接收到学生端请求指令(commandStatus)，返回学生当前的操作状态
     * commandstatus:return
     */
    commandStatusReturn = 't.commandStatusReturn',

    /**
     * (教师机) 响应学生机主动请求开始考试
     * applystart:return
     */
    applyStartReturn = 't.applyStartReturn',

    /**
     * (教师机) 开始考试
     * start:manual
     */
    startManual = 't.startManual',

    /**
     * (教师机) 结束考试
     * stop:manual
     */
    stopManual = 't.stopManual',

    /**
     * (教师机)回收试卷
     * recycle
     */
    recycle = 't.recycle',

    /**
     * (教师机)关闭学生机
     * exit
     */
    exit = 't.exit',

    /**
     * (教师机)清屏操作
     * clean
     */
    clean = 't.clean',

    /**
     * (教师机) 设置考试失败教师机置该学生机失败
     * manualFaile
     */
    manualFaile = 't.manualFaile',

    /**
     * (教师机) 任务被XX教师终止
     * taskStop
     */
    taskStop = 't.taskStop',

    /**
     * (教师机) 收到结束考试指令后
     * beforeProcess
     */
    beforeProcess = 't.beforeProcess',

    /**
     * (教师机) 获取未练习试卷列表，任务中的试卷列表
     * paperused:return
     */
    paperUsedReturn = 't.paperUsedReturn',

    /************************* 学生机发出指令 *****************************/

    /**
     * (学生机) 登录
     * login
     */
    Login = 's.login',

    /**
     * (学生机) 登录成功后发送身份确认指令
     * verification
     */
    verification = 's.verification',

    /**
     * (学生机) 询问教师机现在是在哪一步
     * 连接成功后问教师机现在是在哪一步，只有在考中才进行如下处理
     * commandstatus
     */
    commandStatus = 's.commandStatus',

    /**
     * (学生机) 获取未练习试卷列表，任务中的试卷列表
     * paperused
     */
    paperUsed = 's.paperUsed',

    /**
     * (学生机) 报告教师机最新状态
     * student:status
     */
    studentStatus = 's.studentStatus',

    /**
     * (学生机) 更新学生状态 monitoringStatus = MS_1
     * watchStatus
     */
    watchStatus = 's.watchStatus',

    /**
     * (学生机) 更新学生考试状态
     * student:examstatus
     * 学生端耳机掉落：examstatus: 'MS_12'
     */
    setExamStatus = 's.setExamStatus',

    /**
     * (学生机) 试卷包下载状态更新
     * paper:down
     */
    paperDown = 's.paperDown',

    /**
     * (学生机) 录音测试
     * check:wavein
     */
    checkWavein = 's.checkWavein',

    /**
     * (学生机) 放音测试
     * check:waveout
     */
    checkWaveout = 's.checkWaveout',

    /**
     * (学生机) 学生机主动请求开始考试
     * applystart
     */
    applyStart = 's.applyStart',

    /**
     * (学生机) 学生考试过程信息推送
     * progress
     */
    progress = 's.progress',

    /**
     * (学生机) 学生答题完成
     */
    complete = 's.complete',

    /**
     * (学生机) 回收答卷包
     * recycle:reply
     */
    recycleReply = 's.recycleReply',

    /**
     * (学生机) 举手
     * help
     */
    help = 's.help',
}

/**
 * 允许登录
 */
export interface IAllowLoginCmdParams {
    taskId?: string,  // 任务ID
    description?: string,  // 任务描述
    paperPolicy?: string,  // 试卷发放策略
}

/**
 * 登录结果
 * login:deniedm/login:allow
 */
interface ILoginResultCmdParams {
    id: string, // 考号
    studentId: string, // 学生ID
    studentName: string, // 姓名
    seatNumber: string, // 座位号
    classId: string, // 班级ID
    taskId: string, // 任务ID
    taskType: string, // 考试类型
    ipAddr: string, // 学生机IP
    error: any, // 异常信息
}

/**
 * 教师机修改学生机座位号
 */
interface IModifySeatNumberCmdParams {
    seatNumber: number, // 座位号
}

/**
 * 接收到学生端请求指令返回学生当前的操作状态
 */
interface ICommandStatusReturnCmdParams {
    commandOperationFlag: string, // "01":允许登录 、"02":开始考试、"03":开始练习、"04":结束考试、"05":结束练习
    taskId: string, // 任务ID
    description: string, // 任务描述
    paperPolicy?: string,  // 试卷发放策略
}

/**
 * 学生机主动请求开始考试
 */
interface IApplyStartCmdParams {
    applyStatus: boolean | string
}

/**
 * 任务中的试卷列表
 */
interface IPaperUsedReturnCmdParams {
    // that.props.taskPaperIdList;
}

/*********************************************************************************************/

/**
 * (学生机) 登录
 */
interface ILoginCmdParams {
    id: string, // 考号
    seatNumber: number, // 座位号
}

/**
 * 发送身份确认指令
 */
interface IVerificationCmdParams {
    id: string,      // 考号
    seatNumber: string, // 座位号
    studentName: string, // 姓名
    ipAddr: string, // 学生机IP
    verification: string, // 确认：'1'，不确认：'0'
}

/**
 * 回收试卷反馈json
 */
interface IRespondentsObject {
    respondentsStatus: string, // 答卷包状态:'RS_1'、'RS_4'
    duration: number, // "用时"
    fileCount: number, // "答卷包文件数"
    needFileCount: number, // "待打包文件数"
    respondentsMd5: string, // "0字节文件数"
    paperName: string, // "答卷包名称"
    zeroCount: number, // "0字节文件数"
    upLoadStatus: number, // 上传状态
    fullMark: number, // 总分
    paperTime: number, // 时长
    questionPointCount: number, // 小题数
    score: number, // 得分
    responseQuestionCount: number, // 答题数
}

/**
 * (学生机) 报告教师机最新状态
 */
interface IStudentStatusCmdParams {
    monitorStatus: string, // 监考状态，MS_6:等待考试、MS_7-考试异常(下载异常)、MS_8:正在考试、MS_9:答题完成、MS_11:上传答案包、MS_12:考试失败、MS_13:考试异常(上传异常)、MS_14:考试完成
    answerProcess: string, // 答题进度
    answerNum: number, // TODO ??
    instanceList: number, // TODO 试卷结构类型 paperInstance
    answerCount: number, // TODO ??
    duration: number, // 用时
    respondentsObject: Array<IRespondentsObject>// paper list
}

/**
 * 设置考试状态
 * 学生端耳机掉落：MS_12
 */
interface ISetExamStatusCmdParams {
    examstatus: string
}

/**
 * (学生机)试卷下载状态变化
 */
interface IPaperDownCmdParams {
    taskId: string, // 任务ID
    ipAddr: string, // 学生机IP，可选
    paperId: string, // 试卷ID snapshotId
    paperName: string, // 试卷名称
    answerCount: string, //paperInstance.length, // 题目数量
    instanceList: string[], // paperData.paperInstance[i].type
    status: boolean | string,// 下载成功:true， 下载试卷失败:MS_7
}

/**
 * 放音&录音测试
 */
interface ICheckWaveInOutCmdParams {
    result: number, // number waveinStatus 1、2
    playVolume: number,// 耳机音量
    recordVolume: number // mic音量
    checkResult: any, // 检测结果
    recordMax: number, // 录音音量峰值
    recordAvg: number, // 录音音量平均值
}

/**
 * (学生机) 学生考试过程信息推送
 */
interface IProcessCmdParams {
    paperId: string, // snapshotId paperId???
    paperName: string,
    answerNum: number, // answerProcess
    description: string, // tipName
    instanceList: any[] // number totalQuestionNum
}

/**
 * (学生机) 答题完成
 */
interface ICompleteCmdParams {
    paperId: string, // snapshotId paperId???
    paperName: string,
    duration: number // elapsedTime
}

/**
 * (学生机)更新学生状态 monitoringStatus = MS_1
 */
interface IWatchStatusCmdParams {
    status: string, // MS_1，更新 monitoringStatus
}

/**
 * (学生机) 回收答案包
 */
interface IRecycleReplyCmdParams {
    paperId: string, // 试卷ID snapshotId? paperId?
    result: number,// 状态，1:回收答案包成功考试成功、2:打包失败、3:上传答案包失败
    respondentsObject: any// this.state.respondentsObject
}