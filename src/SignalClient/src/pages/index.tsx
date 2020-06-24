import React, { useState, useEffect,useRef } from 'react';
import { Input, Button, Divider } from 'antd';
import { Link } from 'umi';
import Links from './Steps/Links';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import styles from './index.less';

export default () => {
  // const [signalRConnection, setSignalRConnection] = useState<HubConnection>();

  const connected = window.SignalRConnection && window.SignalRConnection.state === 'Connected';

  const [state, setState] = useState({
    connected,
    user: 'Teacher',
    message: 'Index',
    receiveMessage: '',
  });
  const stateRef = useRef<any>();
  stateRef.current=state;

  useEffect(() => {
    // 配置连接
    if (!state.connected) {
      initConnection();
    }
  }, []);

  useEffect(() => {
    // 打开连接、注册客户端
    if (!state.connected) {
      startConnect();
    }
  }, [window.SignalRConnection]);

  // 配置连接
  const initConnection = () => {
    // 配置连接
    const conn = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/chatHub') // , HttpTransportType.WebSockets)
      .build();

    // 注册成功通知消息处理
    conn.on('AcceptConnection', onAcceptConnection);

    // 接收消息处理
    conn.on('ReceiveMessage', onReceiveMessage);

    // 连接关闭回调
    conn.onclose(onConnectionClose);

    // 重新连接中回调
    conn.onreconnecting(onReconnecting);

    // 重连成功回调
    conn.onreconnected(onReconnected);

    window.SignalRConnection = conn;
    // setSignalRConnection(conn);
  };

  // 打开连接、注册客户端
  const startConnect = async (): Promise<void> => {
    if (window.SignalRConnection) {
      // 开始会话
      try {
        await window.SignalRConnection.start();
        setState({
          ...state,
          connected: true,
        });
      } catch (err) {
        console.error('start error：', err);
        return;
      }
      console.log(`Connected!`);
      // 注册客户端
      await registerClient();

      // // 开始会话
      // signalRConnection
      //   .start()
      //   .then(
      //     async (): Promise<void> => {
      //       console.log(`Connected!`);
      //       // 注册客户端
      //       await registerClient();
      //     },
      //   )
      //   .catch(err => {
      //     console.error('connect error：', err);
      //   });
    }
  };

  // 注册客户端
  const registerClient = async (): Promise<void> => {
    await sendMessage('Register', 'Teacher', '001');
  };

  // 连接关闭回调
  const onConnectionClose = (error: Error | undefined): void => {
    setState({
      ...stateRef.current,
      connected: false,
    });
    console.log('Closed！', error);
  };

  // 重新连接回调
  const onReconnecting = (error: Error | undefined): void => {
    console.log('Reconnecting...', error);
  };

  // 重新连接成功回调
  const onReconnected = (connectionId: string | undefined): void => {
    console.log('Reconnected success：', connectionId);
  };

  // 注册成功通知消息处理
  const onAcceptConnection = (identity: string, message: string) => {
    console.log(`Register success：${identity} ${message}`);
  };

  // 接收消息处理
  const onReceiveMessage = (identity: string, message: string): void => {
    console.log(`Receive success：${identity} ${message}`);
    setState({
      ...stateRef.current,
      receiveMessage: `identity=${identity} message=${message}`,
    });
  };

  // 发送消息
  const sendMessage = async (command: string, identity: string, message: string): Promise<void> => {
    if (!window.SignalRConnection || window.SignalRConnection.state !== 'Connected') {
      // || !signalRConnection.connectionStarted
      console.log('signalR is not connected');
      return;
    }

    try {
      await window.SignalRConnection.invoke(command, identity, message);
      console.log('send success：', { command, identity, message });
    } catch (error) {
      console.log('sendMessage error：', error);
    }
  };

  // 发送按钮事件
  const onBtnSendClick = async (): Promise<void> => {
    const { user, message } = state;
    await sendMessage('SendMessage', user, message);
  };

  return (
    <div>
      <h1 className={state.connected ? styles.title : styles.titleWarn}>Index</h1>
      <div style={{ width: '300px', marginTop: '100px', margin: '0 auto' }}>
        <div>
          User：
          <Input value={state.user} onChange={e => setState({ ...state, user: e.target.value })} />
          Message:
          <Input value={state.message} onChange={e => setState({ ...state, message: e.target.value })} />
        </div>
        <div style={{ textAlign: 'center', margin: '20px' }}>
          <Button
            type="primary"
            shape="round"
            style={{ width: '120px' }}
            onClick={() => {
              onBtnSendClick();
            }}
          >
            发送
          </Button>
        </div>
        {state.receiveMessage && <div>收到消息：{state.receiveMessage}</div>}

        <Links />
      </div>
    </div>
  );
};
