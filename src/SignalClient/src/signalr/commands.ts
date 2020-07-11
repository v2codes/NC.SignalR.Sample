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
 * 可注册消息类型，对应后端接收消息方法名
 */
export enum EventType {
    /**
     * 注册教师机 & 学生机
     */
    Register = 'Register',

    /**
     * 注册成功通知消息类型
     */
    RegisterSuccess = 'RegisterSuccess',

    /**
     * 接收消息类型
     */
    ReceiveMessage = 'ReceiveMessage',

    /**
     * 发送消息类型
     */
    SendMessage = 'SendMessage',

    /**
     * 服务端异常消息通知
     */
    ServerErrorMessage = 'ServerErrorMessage',
}

/**
 * 指令类型，教师机&学生机交互
 */
export enum CommandType {

    /**
     * 注册成功
     */
    RegisterSuccess = 'RegisterSuccess',

    /**
     * 学生机加入
     */
    StudentConnected = 'StudentConnected',
}
