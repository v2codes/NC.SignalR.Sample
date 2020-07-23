import React, { useState, useEffect, useCallback } from 'react';
import { Input, Select, Button, Divider } from 'antd';
import { Link } from 'umi';
import { v4 as uuidv4 } from 'uuid';
import { getSignalRClient, CommandType, IReceiveData } from '@/signalr';
import { IAllowLoginCmdParams } from '@/signalr';
import styles from './index.less';

const TeacherCode = 'Teacher001';
const taskId = uuidv4();
const { Option } = Select;
const { TextArea } = Input;

export default () => {
  // const [signalRClient, setSignalRClient] = useState<SignalRClientBuilder>();
  const [receiveMessages, setReceiveMessages] = useState<Array<any>>([]);

  useEffect(() => {
    // 初始化客户端
    const client = getSignalRClient({ serverAddress: '' });

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
        // 注册客户端 - 教师机
        client.registerTeacher(TeacherCode, `教师 ${TeacherCode} 注册客户端`);
      })
      .catch(e => {
        alert(e);
      });

    client.onClose(err => {
      console.log('err', err);
    });
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

  // 消息发出后，超时未收到反馈通知
  const onTimeout = useCallback(
    (message: any) => {
      receiveMessages.push({
        eventCommand: 'Timeout',
        data: JSON.stringify(message),
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

  // 发送允许登录
  const onAllowLoginClick = async (): Promise<void> => {
    var data: IAllowLoginCmdParams = {
      taskId, // 任务ID
      description: '测试任务XXXXXXXXXXX', // 任务描述
      paperPolicy: 'ET_1', // 试卷发放策略
    };
    await window.SignalRClient?.sendMessage(CommandType.allowLogin, data);
  };

  // 发送结束考试按钮事件
  const onEndClick = async (): Promise<void> => {
    // await window.SignalRClient?.sendStop({ TeacherCode });
  };

  // 获取所有命令
  const getCommandOptions = useCallback((): Array<JSX.Element> => {
    let options: Array<JSX.Element> = [];
    Object.keys(CommandType).forEach((k: string, idx) => {
      options.push(<Option value={CommandType[k as keyof typeof CommandType]}>{k}</Option>);
    });
    return options;
  }, []);

  return (
    <div>
      <h1 className={window.SignalRClient?.isConnected() ? styles.title : styles.titleWarn}>Teacher</h1>
      <div style={{ textAlign: 'center', marginTop: '100px', margin: '0 auto' }}>
        <Link to="/teacher2">Teacher2</Link> | <Link to="/student">Student</Link> 
        <div>Identity:Teacher, Code:{TeacherCode}</div>
        <div style={{ textAlign: 'center', margin: '20px' }}>
          {/* <div>
            <Select defaultValue={CommandType.allowLogin} style={{ width: 150 }} dropdownMatchSelectWidth={false}>
              {getCommandOptions()}
            </Select>
            <TextArea
              // value={value}
              // onChange={this.onChange}
              style={{ width: 500 }}
              placeholder="命令参数..."
              autoSize={{ minRows: 6 }}
            />
          </div> */}
          <Button
            type="primary"
            shape="round"
            // style={{ width: '140px' }}
            onClick={() => {
              onAllowLoginClick();
            }}
          >
            允许登录[t.allowLogin]
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
                  {idx + 1}. command：{m.eventCommand} | data：{m.data}
                  <br />
                </div>
              );
            })}
        </div>
      </div>
    </div>
  );
};
