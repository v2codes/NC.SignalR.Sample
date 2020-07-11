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