import React from 'react';
import { Divider } from 'antd';
import { Link } from 'umi';

export default () => {
  return (
    <div style={{ textAlign: 'center', margin: '20px' }}>
      <Link to="/">Index</Link>
      <Divider
        type="vertical"
        style={{ backgroundColor: '#bfbfbf', width: '3px' }}
      />
      <Link to="/step1">Step1</Link>
      <Divider
        type="vertical"
        style={{ backgroundColor: '#bfbfbf', width: '3px' }}
      />
      <Link to="/step2">Step2</Link>
      <Divider
        type="vertical"
        style={{ backgroundColor: '#bfbfbf', width: '3px' }}
      />
      <Link to="/step3">Step3</Link>
    </div>
  );
};
