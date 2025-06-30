import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { AuthContext } from '../context/AuthContext';
import './GuestHome.css';

export default function GuestHome() {
  const [rooms, setRooms] = useState([]);
  const [selectedRoom, setSelectedRoom] = useState(null);
  const [arrive, setArrive] = useState('');
  const [leave, setLeave]   = useState('');
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    api.get('/szoba')
       .then(res => setRooms(res.data))
       .catch(err => console.error('Szoba lekérés hiba:', err));
  }, []);

  const handleBook = async () => {
    if (!selectedRoom) {
      return alert('Először válassz ki egy szobát!');
    }
    if (!selectedRoom.foglalhato) {
      return alert('Ez a szoba nem foglalható!');
    }
    if (!arrive || !leave) {
      return alert('Kérlek, add meg az érkezés- és távozás dátumát!');
    }

    await api.post('/foglalas', {
      VendegId:     user.id,
      SzobaId:      selectedRoom.id,
      ErkezesDatum: arrive,
      TavozasDatum: leave
    });

    alert(`Sikeresen lefoglaltad a(z) "${selectedRoom.szobaszam}" szobát!`);

    // Frissítjük a rooms tömböt: a kiválasztott szoba foglalható = false
    setRooms(prev =>
      prev.map(r =>
        r.id === selectedRoom.id
          ? { ...r, foglalhato: false }
          : r
      )
    );

    // Reset form
    setSelectedRoom(null);
    setArrive('');
    setLeave('');
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="guest-container">
      <header className="guest-header">
        <h1>Üdv, {user.username}!</h1>
        <button className="logout-btn" onClick={handleLogout}>
          Kijelentkezés
        </button>
      </header>

      <h2>Szobák</h2>
      <table className="rooms-table">
        <thead>
          <tr>
            <th>Szoba</th>
            <th>Férőhely</th>
            <th>Felszereltség</th>
            <th>Foglalható</th>
            <th>Művelet</th>
          </tr>
        </thead>
        <tbody>
          {rooms.map(r => (
            <tr
              key={r.id}
              className={selectedRoom?.id === r.id ? 'selected' : ''}
            >
              <td>{r.szobaszam} ({r.tipus})</td>
              <td>{r.ferohelyekSzama} fő</td>
              <td>{r.felszereltseg}</td>
              <td>{r.foglalhato ? 'Igen' : 'Nem'}</td>
              <td>
                <button
                  onClick={() => setSelectedRoom(r)}
                  disabled={!r.foglalhato}
                >
                  {selectedRoom?.id === r.id ? 'Kiválasztva' : 'Kiválaszt'}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="booking-form">
        <h2>Új foglalás</h2>
        <p>
          <strong>Szoba:</strong>{' '}
          {selectedRoom
            ? `${selectedRoom.szobaszam} (${selectedRoom.tipus})`
            : <em>nincs kiválasztva</em>
          }
        </p>
        <label>
          Érkezés:
          <input
            type="date"
            value={arrive}
            onChange={e => setArrive(e.target.value)}
          />
        </label>
        <label>
          Távozás:
          <input
            type="date"
            value={leave}
            onChange={e => setLeave(e.target.value)}
          />
        </label>
        <button onClick={handleBook}>Foglalás megerősítése</button>
      </div>
    </div>
  );
}
