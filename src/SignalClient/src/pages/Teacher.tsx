import React, { useState, useEffect, useCallback } from 'react';
import { Input, Button, Divider } from 'antd';
import { Link } from 'umi';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@microsoft/signalr';
import getSignalRClient, { SignalRClientBuilder, CommandType } from '@/signalr';
import styles from './index.less';

const TeacherCode = 'Teacher001';

export default () => {

  // const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();
  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);

  useEffect(() => {
    (async () => {
      // 初始化客户端
      const client = await getSignalRClient();

      // 注册成功回调通知
      client.onRegisted(onRegisterSuccess);

      // 接收消息回调
      client.onReceived(onReceive);
      
      // 注册客户端 - 教师机
      await client.registerTeacher(TeacherCode, `教师 ${TeacherCode} 注册客户端` );

      // save state
      // setSignalRClient(client);
    })();
  }, []);

  // 收到消息
  const onReceive = useCallback(
    (commandType: CommandType, data: string) => {
      receiveMessages.push({
        eventCommand: commandType,
        data: JSON.stringify(data),
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  // 注册成功回调通知
  const onRegisterSuccess = useCallback(
    (commandType, regInfo) => {
      receiveMessages.push({
        eventCommand: commandType,
        data: JSON.stringify(regInfo),
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  // 发送开始考试按钮事件
  const onStartClick = async (): Promise<void> => {
    await window.SignalRClient?.sendMessage(CommandType.Start, TeacherCode);
  };

  // 发送结束考试按钮事件
  const onEndClick = async (): Promise<void> => {
    await window.SignalRClient?.sendMessage(CommandType.Stop, TeacherCode);
  };

  return (
    <div>
      <h1 className={window.SignalRClient?.isConnected() ? styles.title : styles.titleWarn}>Index</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <div>Identity:Teacher, Code:{TeacherCode}</div>
        <div style={{ textAlign: 'center', margin: '20px' }}>
          <Button
            type="primary"
            shape="round"
            // style={{ width: '140px' }}
            onClick={() => {
              onStartClick();
            }}
          >
            发送 - 开始考试
          </Button>

          <Button
            type="primary"
            shape="round"
            // style={{ width: '140px' }}
            onClick={() => {
              onEndClick();
            }}
          >
            发送 - 结束考试
          </Button>
        </div>
        <div>
          <h3>收到消息：</h3>
          {receiveMessages &&
            receiveMessages.length > 0 &&
            receiveMessages.map((m, idx) => {
              return (
                <div key={idx}>
                  {idx + 1}. EventCommand:{m.eventCommand} | Data:{m.data}
                  <br />
                </div>
              );
            })}
        </div>
      </div>
    </div>
  );
};
