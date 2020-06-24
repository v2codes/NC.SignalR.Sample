import React, { useState, useEffect,useRef } from 'react';
import { Input, Button, Divider } from 'antd';
import { Link } from 'umi';
import Links from './Links';
import styles from '../index.less';

export default () => {
  const connected = window.SignalRConnection && window.SignalRConnection.state === 'Connected';

  const [state, setState] = useState({
    connected,
    user: 'Teacher',
    message: 'step2',
    receiveMessage: '',
  });
  const stateRef = useRef<any>();
  stateRef.current=state;

  useEffect(() => {
    console.log('step2  signalRConnection:', window.SignalRConnection);
    // 接收消息处理
    window.SignalRConnection.on('ReceiveMessage', onReceiveMessage);
  }, []);

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
      <h1 className={state.connected ? styles.title : styles.titleWarn}>Step 2</h1>
      <div style={{ width: '300px', marginTop: '100px', margin: '0 auto' }}>
        <div>
          User：
          <Input value={state.user} onChange={e => setState({ ...state, user: e.target.value })} />
          Message:
          <Input value={state.message} onChange={e => setState({ ...state, message: e.target.value })} />
        </div>
        <div style={{ textAlign: 'center', margin: '20px' }}>
          <Button type="primary" shape="round" style={{ width: '120px' }} onClick={() => onBtnSendClick()}>
            发送
          </Button>
        </div>
        {state.receiveMessage && <div>收到消息：{state.receiveMessage}</div>}

        <Links />
      </div>
    </div>
  );
};
