/**
 * The unified configuration object.
 * Values are populated from build-time VITE_* envs, but can be overridden at runtime via config.json.
 */
export const ENV = {
  VIETMAP_TILE_KEY: import.meta.env.VITE_VIETMAP_TILE_KEY,
  VIETMAP_API_KEY: import.meta.env.VITE_VIETMAP_API_KEY,
  VIETMAP_SERVICE_KEY: import.meta.env.VITE_VIETMAP_SERVICE_KEY,
  API_BASE_URL: import.meta.env.VITE_API_BASE_URL,
};

/**
 * Loads configuration from a runtime config.json file if it exists.
 * This allows for "Build once, run anywhere" (CDN/Docker friendly).
 */
export const loadRuntimeConfig = async () => {
  try {
    const response = await fetch('/config.json');
    if (response.ok) {
      const runtimeConfig = await response.json();
      Object.assign(ENV, runtimeConfig);
      console.log('[Env] Runtime configuration loaded and applied.');
    }
  } catch (error) {
    console.warn('[Env] No runtime config.json found, using build-time defaults.');
  }
};

/**
 * Validates the environment variables.
 */
export const validateEnv = () => {
  const criticalKeys: (keyof typeof ENV)[] = ['API_BASE_URL'];
  const missingKeys = criticalKeys.filter((key) => !ENV[key]);

  if (missingKeys.length > 0) {
    const errorMsg = `[Env Validation] Missing required environment variables: ${missingKeys.join(', ')}`;
    console.error(errorMsg);
    
    // Don't throw for now to prevent blank page, just log it.
    // if (import.meta.env.PROD) {
    //   throw new Error('Critical environment variables are missing. Application cannot start in production.');
    // }
  }
};
