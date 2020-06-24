import { defineConfig } from 'umi';

export default defineConfig({
  nodeModulesTransform: {
    type: 'none',
  },
  layout: {},
  routes: [
    { path: '/', component: '@/pages/index' },
    { path: '/step1', component: '@/pages/Steps/step1' },
    { path: '/step2', component: '@/pages/Steps/step2' },
    { path: '/step3', component: '@/pages/Steps/step3' },
  ],
  // proxy: {
  //   '/api': {
  //     'target': 'ws://localhost:5000',
  //     'changeOrigin': true,
  //     // 'secure': false,
  //     'ws': true,
  //     'pathRewrite': { '^/api': '' },
  //   },
  // }
});
