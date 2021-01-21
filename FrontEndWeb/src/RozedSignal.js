import {HubConnectionBuilder} from '@microsoft/signalr'

export default class RozedSignal {
    static connection = new HubConnectionBuilder().withUrl("/hub").build()
    
}
