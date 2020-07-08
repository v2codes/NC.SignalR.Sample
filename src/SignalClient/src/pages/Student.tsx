import React, { useState, useEffect, useCallback } from 'react';
import { Button } from 'antd';
// import { Link, useParams } from 'umi';
import { v4 as uuidv4 } from 'uuid';
import getSignalRClient, { SignalRClientBuilder, CommandType } from '@/signalr';
import styles from './index.less';

const TeacherCode = 'Teacher001';

export default () => {
  // const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();

  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);
  // const { code } = useParams();|| 'S001'
  useEffect(() => {
    (async () => {
      // 初始化客户端
      const client = await getSignalRClient();

      // 注册成功回调通知
      client.onRegisted(onRegisterSuccess);

      // 接收消息回调
      client.onReceived(onReceive);

      // 注册客户端
      var studentCode = uuidv4();
      await client.registerStudent({ code: studentCode, TeacherCode, data: `学生机 ${studentCode} 注册` });

      // save state
      // setSignalRClient(client);
    })();
  }, []);

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

  // 收到消息
  const onReceive = useCallback(
    (commandType: CommandType, data: any) => {
      receiveMessages.push({
        eventCommand: commandType,
        data: JSON.stringify(data),
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  // 发送按钮事件
  const onBtnSendClick = async (): Promise<void> => {
    await window.SignalRClient?.sendMessage('Student', 'Teacher001');
  };

  return (
    <div>
      <h1 className={window.SignalRClient?.isConnected() ? styles.title : styles.titleWarn}>Index</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <div>User:Student, Message:001</div>
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
