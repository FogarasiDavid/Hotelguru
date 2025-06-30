import axios from 'axios'

const api = axios.create({
  baseURL: '/api',      // innentől a Vite proxy-olja a hívásokat
})

// token interceptor marad ugyanúgy
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

export default api
