import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ISignalRClientOptions } from './ISignalRClientOptions';
import { IdentityType, EventType, CommandType } from './commands';

/**
 * SignalR 客户端工具类
 */
export class SignalRClientBuilder {

    private client: HubConnection;// 当前客户端
    private serverAddress: string;// signalR 服务器地址
    private timeout?: number;// 超时时间
    public readonly connectionId?: string;// 当前连接ID

    // 注册成功回调
    private onRegistedSuccessCallback?: (...args: any[]) => void;

    // 接受消息回调
    private onReceivedCallback?: (commandType: CommandType, ...args: any[]) => void;

    // 重新连接成功回调
    private onReconnectedCallback?: (connectionId: string | undefined) => void;

    // 连接关闭回调
    private onCloseCallback?: (error?: Error | undefined) => void;

    // 服务端异常消息回调
    private onErrorCallback?: (...args: []) => void;


    constructor(options?: ISignalRClientOptions) {
        this.serverAddress = options && options.serverAddress || 'http://10.17.9.30:5000/signalr';
        this.timeout = options && options.timeout || 3000;

        // 初始化 signalR 客户端 && 注册成功通知消息处理 && 接收消息处理
        this.client = this.initClient();
    }

    /**
     * 初始化 signalR 客户端
     */
    private initClient = (): HubConnection => {
        // 配置连接
        // 重连机制
        //    启用重连机制，指掉线的瞬间马上重连，默认重连4次，分别时间间隔为：0、2、10和30秒
        //    建立连接后，断网后，进行消息传递会进入 onreconnecting 回调
        //    第一次，立即进入重连 0；第二次，2s后重连；第三次，10s后重连；第四次，30s后重连
        //    网络恢复后的一次重连会是失败的，需要到下一次重连才可以重连成功。如果一直断网，4次重连全部失败，则会进入onclose回调。
        const builder = new HubConnectionBuilder()
            .withUrl(this.serverAddress) // , HttpTransportType.WebSockets)
            .withAutomaticReconnect() // 启用重连机制
            .build();

        // 客户端如果在30s内未收到服务器端发送的消息，客户端会断开连接，进入onclose事件
        builder.serverTimeoutInMilliseconds = 30000;
        // 如果客户端在15s内没有发送任何消息，则15s的时候客户端会自动ping一下服务器端，从而告诉服务器端，我在线
        builder.keepAliveIntervalInMilliseconds = 15000;

        // 注册成功通知消息处理 //TODO 返回参数？？？ onRegistedSuccess
        builder.on(EventType.RegisterSuccess, (...args: any[]) => {
            if (this.onRegistedSuccessCallback) {
                this.onRegistedSuccessCallback('RegisterSuccess', ...args);
            }
        });

        // 接收消息处理
        builder.on(EventType.ReceiveMessage, (commandType: CommandType, ...args: any[]): void => {
            if (this.onReceivedCallback) {
                this.onReceivedCallback(commandType, ...args);
            }
        });

        // 服务端异常消息处理
        builder.on(EventType.ServerErrorMessage, (...args: []) => {
            if (this.onErrorCallback) {
                this.onErrorCallback(...args);
            }
        });

        /* 重连机制 */
        // 重连机制开始前出发，仅执行一次
        builder.onreconnecting(error => {
            console.log('onreconnecting', error)
        });

        // 重连成功，任何一次重连成功后执行
        builder.onreconnected(connectionId => {
            console.log('onreconnected', connectionId);
            if (this.onReconnectedCallback) {
                this.onReconnectedCallback(connectionId);
            }
        });

        // 连接关闭事件，默认4次重连失败后，调用关闭连接
        builder.onclose((error) => {
            console.log('onclose', error);
            if (this.onCloseCallback) {
                this.onCloseCallback(error);
            }
        });

        return builder;
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
     * 当前SignalR客户端 是否连接中
     */
    public isConnected = (): boolean => {
        if (this.client && this.client.state === 'Connected') {
            return true;
        }
        return false;
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
        this.onRegistedSuccessCallback = callback;
    }

    /**
     * 注册消息接收回调
     * @param callback 
     */
    public onReceived = (callback: (commandType: CommandType, ...args: any[]) => void): void => {
        this.onReceivedCallback = callback;
    }

    /**
     * 服务端异常消息回调
     * @param callback 
     */
    public onError = (callback: (...args: any[]) => void): void => {
        this.onErrorCallback = callback;
    }

    /**
     * 重新连接成功回调
     * @param callback 
     */
    public onReconnected = (callback: (connectionId?: string) => void): void => {
        this.onReconnectedCallback = callback;
    }

    /**
     * 连接关闭回调
     * @param callback 
     */
    public onClose = (callback: (error?: Error) => void): void => {
        this.onCloseCallback = callback;
    }

    /**
     * 发送 SendMessage 类型消息
     * @param args 
     */
    public sendMessage = async (commandType: CommandType, ...args: any[]): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(commandType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
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
        // 全局保存客户端对象
        window.SignalRClient = client;
        return client;
    }
}


