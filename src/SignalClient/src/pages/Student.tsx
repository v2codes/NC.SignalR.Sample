import React, { useState, useEffect, useCallback } from 'react';
import { Button } from 'antd';
// import { Link, useParams } from 'umi';
import { v4 as uuidv4 } from 'uuid';
import { getSignalRClient, CommandType } from '@/signalr';
import styles from './index.less';

const TeacherCode = 'Teacher001';
const StudentCode = uuidv4();
export default () => {
  // const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();

  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);
  // const { code } = useParams();|| 'S001'
  useEffect(() => {
    (async () => {
      // 初始化客户端
      const client = getSignalRClient();

      // 注册成功回调通知
      client.onRegisted(onRegisterSuccess);

      // 接收消息回调
      client.onReceived(onReceive);

      // 消息发出后，超时未收到反馈通知
      client.onTimeout(onTimeout);

      // 连接关闭
      client.onClose(onClose);

      client
        .connect()
        .then(() => {
          // 注册客户端 - 学生机
          client.registerStudent(TeacherCode, `学生机 ${StudentCode} 注册`);
        })
        .catch(e => {
          alert(e);
        });
    })();
  }, []);

  // 注册成功回调通知
  const onRegisterSuccess = useCallback(
    clientInfo => {
      receiveMessages.push({
        eventCommand: 'RegisterSuccess',
        data: JSON.stringify(clientInfo),
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

  // 消息发出后，超时未收到反馈通知
  const onTimeout = useCallback(
    (messageSummary: any) => {
      receiveMessages.push({
        eventCommand: 'Timeout',
        data: JSON.parse(messageSummary),
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  // 连接关闭
  const onClose = useCallback((error?: Error) => {
    receiveMessages.push({
      eventCommand: 'onClose',
      data: JSON.stringify(error),
    });
    setReceiveMessages([...receiveMessages]);
  }, []);

  // 发送按钮事件
  const onBtnSendClick = async (): Promise<void> => {
    await window.SignalRClient?.sendMessage(CommandType.Status, { TeacherCode });
  };

  return (
    <div>
      <h1 className={window.SignalRClient?.isConnected() ? styles.title : styles.titleWarn}>Index</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <div>Identity:Student, Code:{StudentCode}</div>
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
