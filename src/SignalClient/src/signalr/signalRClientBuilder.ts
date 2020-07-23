import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ISignalRClientOptions, IClientSummary, IReceiveData, IReceiveError } from './typings';
import { IdentityType, ServerEventType, ClientEventType, CommandType } from './commandTypes';

/**
 * SignalR 客户端工具类
 */
export class SignalRClientBuilder {

    private client: HubConnection;// 当前客户端
    private serverAddress: string;// signalR 服务器地址
    // private timeout?: number;// 超时时间
    public readonly connectionId?: string;// 当前连接ID

    // 注册成功回调
    private onRegistedSuccessCallback?: (clientSummary: IClientSummary) => void;

    // 接受消息回调
    private onReceivedCallback?: (commandType: CommandType, data: IReceiveData) => void;

    // 服务端异常消息回调
    private onErrorCallback?: (error: IReceiveError | Error | undefined) => void;

    // 消息发出后，超时未收到反馈通知
    private onTimeoutCallback?: (...args: any[]) => void;

    // 重新连接成功回调
    private onReconnectedCallback?: (connectionId: string | undefined) => void;

    // 连接关闭回调
    private onCloseCallback?: (error?: Error | undefined) => void;

    constructor(options?: ISignalRClientOptions) {
        this.serverAddress = options && options.serverAddress || 'http://localhost:5000/signalr';
        // this.serverAddress = options && options.serverAddress || 'http://10.17.9.30:1443/signalr';
        // this.timeout = options && options.timeout || 3000;

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
        // builder.serverTimeoutInMilliseconds = 30000;
        // 如果客户端在15s内没有发送任何消息，则15s的时候客户端会自动ping一下服务器端，从而告诉服务器端，我在线
        // builder.keepAliveIntervalInMilliseconds = 15000;

        // 注册成功通知消息处理
        builder.on(ClientEventType.RegisterSuccess, async (clientSummary: IClientSummary): Promise<void> => {

            await this.feedback(clientSummary.messageId);

            if (this.onRegistedSuccessCallback) {
                this.onRegistedSuccessCallback(clientSummary);
            }
        });

        // 接收消息处理
        builder.on(ClientEventType.ReceiveMessage, async (commandType: CommandType, data: IReceiveData): Promise<void> => {

            await this.feedback(data.messageId);

            if (this.onReceivedCallback) {
                this.onReceivedCallback(commandType, data);
            }
        });

        // 消息发出后，超时未收到反馈通知
        builder.on(ClientEventType.Timeout, (...args: any[]): void => {
            if (this.onTimeoutCallback) {
                this.onTimeoutCallback(...args);
            }
        });

        // 服务端异常消息处理
        builder.on(ClientEventType.ReceiveServerError, async (data: IReceiveError): Promise<void> => {

            await this.feedback(data.messageId);

            if (this.onErrorCallback) {
                this.onErrorCallback(data.message);
            }
        });

        // 服务端异常消息处理
        builder.on(ClientEventType.CloseConnection, async () => {
            console.log('CloseConnection');
            await this.client.stop();
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
    public registerTeacher = async (teacherCode: string): Promise<void> => {
        const regInfo = {
            identity: IdentityType.Teacher,
            teacherCode
        }
        await this.send(ServerEventType.RegisterTeacher, regInfo);
    }

    /**
     * 注册客户端（学生机）
     * @param code 
     * @param data - 包含 teacherCode 等信息 
     */
    public registerStudent = async (teacherCode: string): Promise<void> => {
        const regInfo = {
            identity: IdentityType.Student,
            teacherCode: teacherCode
        }
        await this.send(ServerEventType.RegisterStudent, regInfo);
    }

    /**
     * 注册成功回调
     * @param callback 
     */
    public onRegisted = (callback: (clientSummary: IClientSummary) => void): void => {
        this.onRegistedSuccessCallback = callback;
    }

    /**
     * 注册消息接收回调
     * @param callback 
     */
    public onReceived = (callback: (commandType: CommandType, data: IReceiveData) => void): void => {
        this.onReceivedCallback = callback;
    }


    /**
     * 消息发出后，超时未收到反馈通知
     */
    public onTimeout = (callback: (...args: any[]) => void): void => {
        this.onTimeoutCallback = callback;
    }

    /**
     * 服务端异常消息回调
     * @param callback 
     */
    public onError = (callback: (error: IReceiveError | Error | undefined) => void): void => {
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
    * 发送消息
    * @param eventType 消息类型
    * @param args 参数
    */
    private send = async (eventType: ServerEventType, ...args: any[]): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(eventType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 发送 SendMessage 类型消息
     * @param args 
     */
    public sendMessage = async (commandType: CommandType, data?: object): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(ServerEventType.SendMessage, commandType, data ? JSON.stringify(data) : null);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 发送 Feedback 类型消息
     */
    private feedback = async (messageId: string): Promise<void> => {
        if (this.isConnected()) {
            await this.client.send(ServerEventType.Feedback, messageId);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 发送 Start 类型消息
     */
    private sendStart = async (data: object): Promise<void> => {
        debugger;
        if (this.isConnected()) {
            await this.client.send(ServerEventType.Start, JSON.stringify(data));
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    /**
     * 发送 Start 类型消息
     */
    private sendStop = async (data: object): Promise<void> => {
        debugger;
        if (this.isConnected()) {
            await this.client.send(ServerEventType.Stop, JSON.stringify(data));
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


