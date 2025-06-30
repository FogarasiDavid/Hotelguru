import React, { createContext, useState, useEffect } from 'react';
import api from '../api/axios';
import { jwtDecode } from 'jwt-decode';

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);

  // Ha már van token a localStorage-ban, dekódoljuk és beállítjuk a user-t
  useEffect(() => {
    const token = localStorage.getItem('token');
    if (token) {
      const payload = jwtDecode(token);
      setUser({
        username: payload.sub,
        role:     payload.role,
        exp:      payload.exp,
        id:       payload.id
      });
    }
  }, []);

  // Bejelentkezés: lekérjük a tokent, elmentjük, beállítjuk a user-t, és visszaadjuk a payload-ot
  const login = async (username, password) => {
  const { data } = await api.post('/auth/login', {
  FelhasznaloNev: username,
  Jelszo:         password
});


    localStorage.setItem('token', data.token);

    const payload = jwtDecode(data.token);
    const role =
      payload.role
      || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    setUser({
      username: payload.sub, role,
      
      exp:      payload.exp,
      id:       payload.id
    });

    

    return { ...payload, role };
  };

  // Regisztráció: csak a kérést küldjük, nincs navigáció itt
  const register = async (username, password, fullName) => {
    await api.post('/auth/register', {
      FelhasznaloNev: username,
      TeljesNev:      fullName,
      Jelszo:         password
    });
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
