import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      // minden /api-vel kezdődő hívást irányítson át a backend-re
      '/api': {
        target: 'https://localhost:7107',
        changeOrigin: true,
        secure: false,
        // pathRewrite nem kell, mert az útvonalak megegyeznek
      }
    }
  }
})
