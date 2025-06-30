import React, { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import './Login.css';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      const payload = await login(username, password);
      const role =
        payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (role === 'Admin') navigate('/admin');
      else if (role === 'Recepcios') navigate('/reception');
      else navigate('/guest');
    } catch (err) {
      if (err.response) {
        const { status, data } = err.response;
        if (status === 401) alert(data);
        else if (status === 500 && data?.detail) alert(`Szerver hiba: ${data.detail}`);
        else alert(`Hiba ${status}: ${JSON.stringify(data)}`);
      } else {
        alert(`Hiba: ${err.message}`);
      }
    }
  };

  return (
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Bejelentkezés</h2>
        <input placeholder="Felhasználónév" value={username} onChange={e => setUsername(e.target.value)} />
        <input type="password" placeholder="Jelszó" value={password} onChange={e => setPassword(e.target.value)} />
        <button type="submit">Bejelentkezés</button>
        <p>Nincs még fiókod? <Link to="/register">Regisztráció</Link></p>
      </form>
    </div>
  );
}
