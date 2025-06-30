import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { AuthContext } from '../context/AuthContext';
import './ReceptionHome.css';

export default function ReceptionHome() {
  const [bookings, setBookings] = useState([]);
  const [erkezes, setErkezes] = useState('');
  const [tavozas, setTavozas] = useState('');
  const [selectedGuest, setSelectedGuest] = useState(null);
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    fetchFoglalasok();
  }, []);

  const fetchFoglalasok = () => {
    api.get('/foglalas')
      .then(res => setBookings(res.data))
      .catch(err => console.error('Foglalások lekérése hiba:', err));
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const handleConfirm = async (id) => {
    try {
      await api.put(`/foglalas/visszaigazolas/${id}`);
      fetchFoglalasok();
    } catch {
      alert('Hiba a visszaigazolásnál.');
    }
  };

  const handleBooking = async (szobaId) => {
    if (!erkezes || !tavozas) return alert('Adj meg érkezés és távozás dátumokat!');
    if (new Date(erkezes) >= new Date(tavozas)) return alert('Az érkezésnek korábbinak kell lennie a távozásnál!');

    try {
      const szabad = await api.get('/foglalas/szobaszabad', {
        params: { szobaId, erkezes, tavozas }
      });
      if (!szabad.data) return alert('A szoba nem elérhető ebben az időszakban.');

      await api.post('/foglalas', {
        vendegId: user.id,
        szobaId,
        erkezesDatum: erkezes,
        tavozasDatum: tavozas
      });

      alert('Sikeres foglalás!');
      fetchFoglalasok();
    } catch (err) {
      alert('Foglalási hiba.');
    }
  };

  const showGuestDetails = async (vendegId) => {
    try {
      const res = await api.get(`/vendeg/${vendegId}`);
      setSelectedGuest(res.data);
    } catch {
      alert('Nem sikerült betölteni a vendég adatokat.');
    }
  };

  return (
    <div className="reception-container">
      <header className="reception-header">
        <h1>Recepciós felület — Üdv, {user.username}!</h1>
        <button className="logout-btn" onClick={handleLogout}>Kijelentkezés</button>
      </header>

      <section className="date-picker">
        <label>Érkezés: <input type="date" value={erkezes} onChange={e => setErkezes(e.target.value)} /></label>
        <label>Távozás: <input type="date" value={tavozas} onChange={e => setTavozas(e.target.value)} /></label>
      </section>

      <div className="summary-box">
        <p>Összes foglalás: {bookings.length}</p>
        <p>Visszaigazolt: {bookings.filter(b => b.visszaigazolva).length}</p>
        <p>Várakozó: {bookings.filter(b => !b.visszaigazolva).length}</p>
      </div>

      <table className="reception-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Szoba</th>
            <th>Érkezés</th>
            <th>Távozás</th>
            <th>Visszaigazolva</th>
            <th>Műveletek</th>
          </tr>
        </thead>
        <tbody>
          {bookings.map(f => (
            <tr key={f.id}>
              <td>{f.id}</td>
              <td>{f.szobaId}</td>
              <td>{new Date(f.erkezesDatum).toLocaleDateString()}</td>
              <td>{new Date(f.tavozasDatum).toLocaleDateString()}</td>
              <td>{f.visszaigazolva ? '✅' : '⏳'}</td>
              <td>
                {!f.visszaigazolva && <button onClick={() => handleConfirm(f.id)}>✔ Visszaigazolás</button>}
                <button onClick={() => handleBooking(f.szobaId)}>🛏 Foglalás</button>
                <button onClick={() => showGuestDetails(f.vendegId)}>👤 Vendég</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {selectedGuest && (
        <div className="guest-modal">
          <div className="guest-modal-content">
            <h3>Vendég adatok</h3>
            <p>Név: {selectedGuest.teljesNev}</p>
            <p>Email: {selectedGuest.email}</p>
            <p>Telefonszám: {selectedGuest.telefonszam}</p>
            <p>Lakcím: {selectedGuest.lakcim}</p>
            <button onClick={() => setSelectedGuest(null)}>Bezárás</button>
          </div>
        </div>
      )}
    </div>
  );
}
