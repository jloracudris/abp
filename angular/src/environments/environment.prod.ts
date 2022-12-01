import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'Demo',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44336/',
    redirectUri: baseUrl,
    clientId: 'Demo_App',
    responseType: 'code',
    scope: 'offline_access Demo',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44336',
      rootNamespace: 'Americasa.Demo',
    },
  },
} as Environment;
