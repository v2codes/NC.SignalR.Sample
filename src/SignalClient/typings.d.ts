import { HubConnection } from '@microsoft/signalr';
import { SignalRClientBuilder } from '@/signalr';

declare module '*.less';
declare module '*.css';
declare module "*.png";
declare module '*.svg' {
  export function ReactComponent(props: React.SVGProps<SVGSVGElement>): React.ReactElement
  const url: string
  export default url
}

declare global {
  interface Window {
    signalRServerUrl: string | undefined;
    SignalRConnection: HubConnection | undefined;

    /**
     * 全局 SignalR 客户端对象
     */
    SignalRClient: SignalRClientBuilder | undefined;
  }
}
export { }