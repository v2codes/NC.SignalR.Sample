import { HubConnection } from '@microsoft/signalr';

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
    SignalRConnection: HubConnection | undefined;
  }
}
export { }