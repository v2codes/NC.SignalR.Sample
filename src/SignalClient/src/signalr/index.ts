import { HubConnection, HubConnectionBuilder, HttpTransportType, dataType } from '@microsoft/signalr';

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
 * SignalR 客户端配置选项
 */
export interface ISignalRClientOptions {
    /**
     * signalR 服务器地址
     */
    serverAddress?: string,

    /**
     * 注册信息（教师|学生）
     */
    identityType: IdentityType,

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
 * 可注册消息类型，对应后端接收消息方法名
 */
export enum EventType {
    /**
     * 注册教师机
     */
    RegisterTeacher = 'RegisterTeacher',

    /**
     * 注册学生机
     */
    RegisterStudent = 'RegisterStudent',

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
    SendMessage = 'SendMessage'
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
    StudentRegister = 'StudentRegister',

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
 * SignalR 客户端工具
 * 1. 创建Builder对象
 * 2. 初始化SignalR客户端
 * 3. 注册客户端时，打开连接
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


    constructor(options?: ISignalRClientOptions) {
        this.serverAddress = options && options.serverAddress || 'http://localhost:5000/hubs/chatHub';
        this.timeout = options && options.timeout || 3000;

        // 初始化 signalR 客户端 && 注册成功通知消息处理 && 接收消息处理
        this.client = this.initSignalRClient();

        // // 打开连接
        // this.start().then(() => {
        //     console.log('signalr: server connected!')
        // }).catch(err => {
        //     throw err;
        // });
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
            console.log('this.onReceivedMessage', this.onReceivedMessage)
            if (this.onReceivedMessage) {
                this.onReceivedMessage(commandType, ...args);
            }
        });

        return builder;
    }

    /**
     * 注册客户端（教师机|学生机）
     * @param code 
     * @param data 
     */
    public registerTeacher = async (...args: any[]): Promise<void> => {
        await this.send(EventType.RegisterTeacher, ...args);
    }

    /**
     * 注册客户端（教师机|学生机）
     * @param code 
     * @param data - 包含 teacherCode 等信息在内 
     */
    public registerStudent = async (data: any): Promise<void> => {
        const teacherCode = data && data.teacherCode || 'Teacher001';
        await this.send(EventType.RegisterStudent, { ...data, teacherCode });
    }

    /**
     * 接收消息回调
     * @param callback 
     */
    public onRegisted = (callback: (...args: any[]) => void): void => {
        this.onRegistedSuccess = callback;
    }

    /**
     * 接收消息回调
     * @param callback 
     */
    public onReceived = (callback: (commandType: CommandType, ...args: any[]) => void): void => {
        this.onReceivedMessage = callback;
    }

    /**
     * 发送 SendMessage 类型消息
     * @param args 
     */
    public sendMessage = async (commandType: CommandType, ...args: any[]): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(EventType.SendMessage, commandType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 打开连接
     */
    public start = async (): Promise<void> => {
        if (this.client) {
            await this.client.start();
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
        if (this.client) {
            this.client.onreconnecting(callback);
        } else {
            console.error('signalr init failed.');
        }
    }

    /**
     * 重新连接成功
     */
    public onReconnected = (callback: (connectionId?: string) => void): void => {
        if (this.client) {
            this.client.onreconnected(callback);
        } else {
            console.error('signalr init failed.');
        }
    }

    /**
     * 连接关闭回调
     */
    public onClose = (callback: (error?: Error) => void): void => {
        if (this.client) {
            this.client.onclose(callback)
        } else {
            console.error('signalr init failed.');
        }
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
 */
const getSignalRClient = async (options?: ISignalRClientOptions): Promise<SignalRClientBuilder> => {
    if (window.SignalRClient) {
        return window.SignalRClient;
    } else {
        const client = new SignalRClientBuilder(options);
        // 打开连接
        await client.start().then(() => {
            console.log('signalr: server connected!')
        }).catch(err => {
            throw err;
        });

        // 全局保存客户端对象
        window.SignalRClient = client;
        return client;
    }
}
export default getSignalRClient;