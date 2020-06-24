import { HubConnection } from "@microsoft/signalr"
interface Window {
    SignalRConnection: HubConnection | undefined
}
