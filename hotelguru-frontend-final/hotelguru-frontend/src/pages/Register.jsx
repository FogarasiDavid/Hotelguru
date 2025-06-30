import React, { useState, useContext } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import './Login.css';

export default function Register() {
  const [username, setUsername] = useState('');
  const [fullName, setFullName] = useState('');
  const [password, setPassword] = useState('');
  const { register } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await register(username, password, fullName);
      alert('Sikeres regisztráció! Jelentkezz be!');
      navigate('/login');
    } catch {
      alert('Hiba a regisztráció során!');
    }
  };

  return (
    <div className="auth-container">
      <form className="auth-form" onSubmit={handleSubmit}>
        <h2>Regisztráció</h2>
        <input placeholder="Felhasználónév" value={username} onChange={e => setUsername(e.target.value)} />
        <input placeholder="Teljes név" value={fullName} onChange={e => setFullName(e.target.value)} />
        <input type="password" placeholder="Jelszó" value={password} onChange={e => setPassword(e.target.value)} />
        <button type="submit">Regisztráció</button>
        <p>Már van fiókod? <Link to="/login">Bejelentkezés</Link></p>
      </form>
    </div>
  );
}
