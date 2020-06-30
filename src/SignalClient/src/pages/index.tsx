import React, { useState, useEffect, useCallback } from 'react';
import { Input, Button, Divider } from 'antd';
import { Link } from 'umi';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import SignalRClientBuilder, { CommandType } from '@/signalr';
import styles from './index.less';

export default () => {
  const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();

  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);

  useEffect(() => {
    (async () => {
      const client = new SignalRClientBuilder();
      client.onReceive(onReceive);
      await client.register(CommandType.Register, 'Teacher', '001');
      setSignalRClient(client);
    })();
  }, []);

  // 收到消息
  const onReceive = useCallback(
    (commandType: CommandType, identity: string, message: string) => {
      receiveMessages.push({
        commandType,
        identity,
        message,
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  // 发送按钮事件
  const onBtnSendClick = async (): Promise<void> => {
    await signalRClient?.send(CommandType.SendMessage, 'Teacher', '001');
  };

  return (
    <div>
      <h1 className={signalRClient?.isConnected ? styles.title : styles.titleWarn}>Index</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <div>User:Teacher, Message:001</div>
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
        <div>
          <h3>收到消息：</h3>
          {receiveMessages &&
            receiveMessages.length > 0 &&
            receiveMessages.map((m, idx) => {
              return (
                <div key={idx}>
                  {idx + 1}. CommandType:{m.commandType} | identity:{m.identity} | message:{m.message}
                  <br />
                </div>
              );
            })}
        </div>
      </div>
    </div>
  );
};
