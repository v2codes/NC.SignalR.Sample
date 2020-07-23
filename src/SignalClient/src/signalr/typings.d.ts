/**
 * SignalR 客户端配置选项
 */
export interface ISignalRClientOptions {
    /**
     * signalR 服务器地址
     */
    serverAddress?: string,

    // /**
    //  * 超时时间，默认5000毫秒
    //  */
    // timeout?: number,

    /**
     * 开启日志
     */
    logging?: boolean,
}

/*************************** 接受消息方法参数定义 *********************************/
/**
 * 注册成功后，客户端链接信息
 */
export interface IClientSummary {
    messageId: string, // 当前消息ID
    connectionId: string,    // 连接ID
    isConnected: boolean, // 是否连接中
    remoteIpAddress: string, // 客户端IP
    Identity: string, // 角色类型：Teacher、Student
    teacherCode: string, // 教师机Code
    code: string, // 当前用户 Code
    data: any, // 其他数据
    systemTime: number, // Proxy系统时间（Unix时间戳-毫秒）
}

/**
 * 注册成功后，客户端链接信息
 * TODO 数据待完善
 */
export interface IReceiveData {
    messageId: string, // 当前消息ID
    [key: string]: any, // 其他数据
}


/**
 * 服务端异常消息
 */
export interface IReceiveError {
    messageId: string, // 当前消息ID
    message: any, // 异常消息
}
