import { HubConnection, HubConnectionBuilder, HttpTransportType, MessageType } from '@microsoft/signalr';

/**
 * SignalR 客户端配置选项
 */
export interface ISignalRClientBuilderOptions {
    /**
     * signalR 服务器地址
     */
    serverAddress?: string,
    /**
     * 超时时间，默认5000毫秒
     */
    timeout?: number,
}

/**
 * 可注册消息类型
 */
export enum CommandType {
    /**
     * 注册消息类型
     */
    Register = 'Register',
    /**
     * 注册成功通知消息类型
     */
    AcceptConnection = 'AcceptConnection',
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
 * SignalR 客户端工具
 */
export default class SignalRClientBuilder {

    // 当前连接ID
    public readonly connectionId?: string;

    // 当前连接
    private client?: HubConnection;

    // 当前连接
    public isConnected?: boolean;

    // signalR 服务器地址
    private serverAddress: string;

    // 超时时间
    private timeout: number;

    // 接受消息回调
    private receiveCallback?: (commandType: CommandType, ...args: any[]) => void;

    constructor(options?: ISignalRClientBuilderOptions) {
        this.serverAddress = options && options.serverAddress || 'http://localhost:5000/hubs/chatHub';
        this.timeout = options && options.timeout || 5000;
        this.isConnected = false;

        // 初始化 signalR 客户端 
        this.initClientBuilder();

        // 绑定消息处理程序
        this.bindMessageHandler();

        // 打开连接
        // (async () => {
        //     await this.connect();
        //     this.isConnected = true;
        // })();
    }

    /**
     * 初始化 signalR 客户端
     * @param serverUrl 服务器
     */
    private initClientBuilder = (): void => {
        // 配置连接
        this.client = new HubConnectionBuilder()
            .withUrl(this.serverAddress) // , HttpTransportType.WebSockets)
            .build();
    }

    /**
     * 绑定消息处理程序
     */
    private bindMessageHandler = (): void => {
        if (this.client) {
            // 注册成功通知消息处理
            this.client.on('AcceptConnection', this.onAcceptConnection);

            // 接收消息处理
            this.client.on('ReceiveMessage', this.onReceiveMessage);
        } else {
            console.error('signalr init failed.');
        }
    }

    /**
     * 打开连接
     */
    private connect = async (): Promise<void> => {
        if (this.client) {
            try {
                await this.client.start();
                this.isConnected = true;
            } catch (err) {
                console.error('signalr connect error：', err);
                throw err;
            }
        } else {
            console.error('signalr init failed.');
        }
    }

    // 注册成功通知消息处理
    private onAcceptConnection = (...args: any[]) => {
        console.log(`onAcceptConnection：${args[0]} ${args[1]}`);
        if (this.receiveCallback) {
            this.receiveCallback(CommandType.AcceptConnection, args[0], args[1]);
        }
    };

    /**
     * 接收消息处理
     */
    private onReceiveMessage = (...args: any[]): void => {
        console.log(`onReceiveMessage：${args[0]} ${args[1]}`);
        if (this.receiveCallback) {
            this.receiveCallback(CommandType.ReceiveMessage, args[0], args[1]);
        }
    };

    /**
     * 注册客户端
     */
    public register = async (commandType: CommandType, identity: string, message: string): Promise<void> => {
        await this.connect();
        await this.send(commandType, identity, message);
    }

    /**
     * 接收消息回调
     */
    public onReceive = (callback: (commandType: CommandType, ...args: any[]) => void): void => {
        this.receiveCallback = callback;
    }

    /**
     * 发送消息
     * @param commandType 消息类型
     * @param args 参数
     */
    public send = async (commandType: CommandType, ...args: any[]): Promise<void> => {
        if (this.client && this.client.state === 'Connected') {
            await this.client.send(commandType, ...args);
        } else {
            console.log('signalr init failed or disconnected.');
        }
    }

    //#region 接收消息回调/重新连接回调/连接关闭回调
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
    //#endregion
}