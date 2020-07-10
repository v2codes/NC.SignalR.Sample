import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';

/**
 * SignalR 客户端配置选项
 */
export interface ISignalRClientOptions {
    /**
     * signalR 服务器地址
     */
    serverAddress?: string,

    /**
     * 超时时间，默认5000毫秒
     */
    timeout?: number,

    /**
     * 开启日志
     */
    logging?: boolean,
}


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

    /**
     * 开始考试消息类型
     */
    Start = 'Start'
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

    /**
     * 开始考试
     */
    Start = 'Start',

    /**
     * 结束考试
     */
    Stop = 'Stop'
}

/**
 * SignalR 客户端工具类
 */
export class SignalRClientBuilder {

    private client: HubConnection;// 当前客户端
    private serverAddress: string;// signalR 服务器地址
    private timeout?: number;// 超时时间
    public readonly connectionId?: string;// 当前连接ID

    // 注册成功回调
    private onRegistedSuccess?: (...args: any[]) => void;

    // 接受消息回调
    private onReceivedMessage?: (commandType: CommandType, ...args: any[]) => void;

    // 接受消息回调
    private onStart?: (commandType: CommandType, ...args: any[]) => void;

    // 服务端异常消息回调
    private onServerError?: (...args: []) => void;


    constructor(options?: ISignalRClientOptions) {
        this.serverAddress = options && options.serverAddress || 'http://10.17.9.30:5000/signalr';
        this.timeout = options && options.timeout || 3000;

        // 初始化 signalR 客户端 && 注册成功通知消息处理 && 接收消息处理
        this.client = this.initSignalRClient();
    }

    /**
     * 初始化 signalR 客户端
     * @param serverUrl 服务器
     */
    private initSignalRClient = (): HubConnection => {
        // 配置连接
        const builder = new HubConnectionBuilder()
            .withUrl(this.serverAddress) // , HttpTransportType.WebSockets)
            .build();

        // 注册成功通知消息处理 //TODO 返回参数？？？ onRegistedSuccess
        builder.on(EventType.RegisterSuccess, (...args: any[]) => {
            if (this.onRegistedSuccess) {
                this.onRegistedSuccess('RegisterSuccess', ...args);
            }
        });

        // 接收消息处理
        builder.on(EventType.ReceiveMessage, (commandType: CommandType, ...args: any[]): void => {
            if (this.onReceivedMessage) {
                this.onReceivedMessage(commandType, ...args);
            }
        });

        // 服务端异常消息处理
        builder.on(EventType.ServerErrorMessage, (...args: []) => {
            if (this.onServerError) {
                this.onServerError(...args);
            }
        });

        // 连接关闭事件
        builder.onclose((error)=>{
            console.log('onclose', error)
        });
        
        // 断开重连
        builder.onreconnecting(error=>{
            console.log('onreconnecting', error)
        });

        // 断开重连
        builder.onreconnected(connectionId=>{
            console.log('onreconnected', connectionId)
        });

        // 开始考试
        builder.on(EventType.Start, (commandType: CommandType, ...args: any[]): void => {
            if (this.onStart) {
                this.onStart(commandType, ...args);
            }
        });



        return builder;
    }

    /**
     * 注册客户端（教师机）
     * @param identityType 
     * @param data 
     */
    public registerTeacher = async (code: string, data: any): Promise<void> => {
        const regInfo = {
            identity: IdentityType.Teacher,
            code,
            data
        }
        await this.send(EventType.Register, regInfo);
    }

    /**
     * 注册客户端（学生机）
     * @param code 
     * @param data - 包含 teacherCode 等信息在内 
     */
    public registerStudent = async (code: string, teacherCode: string, data: any): Promise<void> => {
        const regInfo = {
            identity: IdentityType.Student,
            teacherCode: teacherCode,
            code,
            data
        }
        await this.send(EventType.Register, regInfo);
    }

    /**
     * 注册成功回调
     * @param callback 
     */
    public onRegisted = (callback: (...args: any[]) => void): void => {
        this.onRegistedSuccess = callback;
    }

    /**
     * 注册消息接收回调
     * @param callback 
     */
    public onReceived = (callback: (commandType: CommandType, ...args: any[]) => void): void => {
        this.onReceivedMessage = callback;
    }

    /**
     * 注册服务端异常消息回调
     * @param callback 
     */
    public onError = (callback: (...args: any[]) => void): void => {
        this.onServerError = callback;
    }

    // // 接收消息处理
    // public on = (methodName: string, callback: (commandType: CommandType, ...args: any[]) => void) => {
    //     this.client.on(methodName, callback);
    // }

    // public onStarted = (callback: (commandType: CommandType, ...args: any[]) => void): void => {
    //     this.onStart = callback;
    // }


    /**
     * 发送 SendMessage 类型消息
     * @param args 
     */
    public sendMessage = async (commandType: CommandType, ...args: any[]): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(commandType, ...args);
            // await this.client.send(EventType.SendMessage, commandType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 打开连接
     */
    public connect = async (): Promise<void> => {
        if (this.client) {
            try {
                await this.client.start();
            } catch (error) {
                throw error;
            }
        } else {
            throw "signalr init failed.";
        }
    }

    /**
     * 发送消息
     * @param eventType 消息类型
     * @param args 参数
     */
    private send = async (eventType: EventType, ...args: any[]): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(eventType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    //#region 打开连接/接收消息回调/重新连接回调/连接关闭回调
    /**
     * 重新连接回调
     */
    public onReconnecting = (callback: (error?: Error) => void): void => {
        this.client.onreconnecting(callback);
    }

    /**
     * 重新连接成功
     */
    public onReconnected = (callback: (connectionId?: string) => void): void => {
        this.client.onreconnected(callback);
    }

    /**
     * 连接关闭回调
     */
    public onClose = (callback: (error?: Error) => void): void => {
        this.client.onclose(callback)
    }

    /**
     * 当前SignalR客户端 是否连接中
     */
    public isConnected = (): boolean => {
        if (this.client && this.client.state === 'Connected') {
            return true;
        }
        return false;
    }
    //#endregion
}

/**
 * 创建 SignalR 客户端
 * 同时打开连接
 */
export function getSignalRClient(options?: ISignalRClientOptions): SignalRClientBuilder {
    if (window.SignalRClient) {
        return window.SignalRClient;
    } else {
        const client = new SignalRClientBuilder(options);
        // 打开连接
        // client.start().then(() => {
        //     console.log('signalr: server connected!')
        // }).catch(err => {
        //     throw err;
        // });

        // 全局保存客户端对象
        window.SignalRClient = client;
        return client;
    }
}


