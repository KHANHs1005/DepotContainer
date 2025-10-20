export const environment = {
  production: true,
  appName: 'DepotContainer',
  // Base API URL for production
  apiBaseUrl: 'http://localhost:5011',
  // Feature toggles
  features: {
    enableTelemetry: true,
    enableDebugToolbar: false
  },
  // Authentication settings
  auth: {
    tokenStorageKey: 'depot_auth_token',
    refreshTokenStorageKey: 'depot_refresh_token'
  },
  // Logging configuration
  logging: {
    level: 'error' // possible values: 'debug' | 'info' | 'warn' | 'error'
  }
};
