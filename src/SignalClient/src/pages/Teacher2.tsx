import React, { useState, useEffect, useCallback } from 'react';
import { Input, Button, Divider } from 'antd';
import { Link } from 'umi';
import { getSignalRClient, CommandType, IReceiveData } from '@/signalr';
import styles from './index.less';

const TeacherCode = 'Teacher001';

export default () => {
  // const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();
  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);

  useEffect(() => {
    (async () => {
      // 初始化客户端
      const client = getSignalRClient().onReceived(onReceive);
    })();
  }, []);

  // 收到消息
  const onReceive = useCallback(
    (commandType: CommandType, data: IReceiveData) => {
      receiveMessages.push({
        eventCommand: commandType,
        data: JSON.stringify(data),
      });
      setReceiveMessages([...receiveMessages]);
    },
    [receiveMessages],
  );

  return (
    <div>
      <h1 className={window.SignalRClient?.isConnected() ? styles.title : styles.titleWarn}>Teacher2</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <div>Identity:Teacher, Code:{TeacherCode}</div>
        <div style={{ textAlign: 'center', margin: '20px' }}></div>
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
