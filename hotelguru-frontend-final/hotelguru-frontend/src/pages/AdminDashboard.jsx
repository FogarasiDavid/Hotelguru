import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { AuthContext } from '../context/AuthContext';
import './AdminDashboard.css';

export default function AdminDashboard() {
  const [bookings, setBookings] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [newRoom, setNewRoom] = useState({
    szobaszam: '',
    tipus: '',
    ferohelyekSzama: '',
    felszereltseg: '',
    foglalhato: true
  });

  const [adminBookingDates, setAdminBookingDates] = useState({});
  const { user, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = () => {
    api.get('/foglalas')
      .then(res => setBookings(res.data))
      .catch(err => console.error('Foglalások hiba:', err));

    api.get('/szoba')
      .then(res => setRooms(res.data))
      .catch(err => console.error('Szoba hiba:', err));
  };

  const confirmBooking = async (id) => {
    try {
      await api.put(`/foglalas/visszaigazolas/${id}`);
      fetchData();
    } catch {
      alert('Hiba a visszaigazolásnál!');
    }
  };

  const handleCreateRoom = async () => {
    try {
      const payload = {
        szobaszam: newRoom.szobaszam,
        tipus: newRoom.tipus,
        ferohelyekSzama: parseInt(newRoom.ferohelyekSzama),
        felszereltseg: newRoom.felszereltseg,
        foglalhato: newRoom.foglalhato
      };
      await api.post('/szoba', payload);
      alert('Szoba sikeresen létrehozva!');
      setNewRoom({ szobaszam: '', tipus: '', ferohelyekSzama: '', felszereltseg: '', foglalhato: true });
      fetchData();
    } catch {
      alert('Hiba a szoba létrehozásakor!');
    }
  };

  const handleRoomBooking = async (szobaId) => {
    const dates = adminBookingDates[szobaId];
    if (!dates?.erkezes || !dates?.tavozas) {
      alert('Adj meg érkezési és távozási dátumot!');
      return;
    }

    try {
      await api.post('/foglalas', {
        vendegId: user.id,
        szobaId,
        erkezesDatum: dates.erkezes,
        tavozasDatum: dates.tavozas
      });
      alert('Foglalás sikeres!');
      fetchData();
    } catch (err) {
      alert('Hiba történt a foglalás során!');
    }
  };

  const handleDateChange = (szobaId, field, value) => {
    setAdminBookingDates(prev => ({
      ...prev,
      [szobaId]: {
        ...prev[szobaId],
        [field]: value
      }
    }));
  };

  const roomCount = {};
  bookings.forEach(b => {
    roomCount[b.szobaId] = (roomCount[b.szobaId] || 0) + 1;
  });
  const mostBookedRoomId = Object.entries(roomCount).sort((a, b) => b[1] - a[1])[0]?.[0];

  return (
    <div className="admin-container">
      <header className="admin-header">
        <h1>Admin Dashboard — Üdv, {user.username}!</h1>
        <button className="logout-btn" onClick={() => { logout(); navigate('/login'); }}>
          Kijelentkezés
        </button>
      </header>

      <section className="admin-summary">
        <div>Foglalások száma: <strong>{bookings.length}</strong></div>
        <div>Szobák száma: <strong>{rooms.length}</strong></div>
        {mostBookedRoomId && (
          <div>Legnépszerűbb szoba: <strong>{mostBookedRoomId}</strong></div>
        )}
      </section>

      <h2>Foglalások</h2>
      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Vendég ID</th>
            <th>Szoba ID</th>
            <th>Érkezés</th>
            <th>Távozás</th>
            <th>Visszaigazolva</th>
            <th>Művelet</th>
          </tr>
        </thead>
        <tbody>
          {bookings.map(f => (
            <tr key={f.id}>
              <td>{f.id}</td>
              <td>{f.vendegId}</td>
              <td>{f.szobaId}</td>
              <td>{new Date(f.erkezesDatum).toLocaleDateString()}</td>
              <td>{new Date(f.tavozasDatum).toLocaleDateString()}</td>
              <td><span className={`status ${f.visszaigazolva ? 'igen' : 'nem'}`}>{f.visszaigazolva ? 'Igen' : 'Nem'}</span></td>
              <td>{!f.visszaigazolva && <button onClick={() => confirmBooking(f.id)}>Visszaigazolás</button>}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2>Összes szoba</h2>
      <table className="admin-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Szobaszám</th>
            <th>Típus</th>
            <th>Férőhely</th>
            <th>Felszereltség</th>
            <th>Foglalható</th>
            <th>Foglalás</th>
          </tr>
        </thead>
        <tbody>
          {rooms.map(sz => (
            <tr key={sz.id}>
              <td>{sz.id}</td>
              <td>{sz.szobaszam}</td>
              <td>{sz.tipus}</td>
              <td>{sz.ferohelyekSzama} fő</td>
              <td>{sz.felszereltseg}</td>
              <td><span className={`status ${sz.foglalhato ? 'igen' : 'nem'}`}>{sz.foglalhato ? 'Igen' : 'Nem'}</span></td>
              <td>
                {sz.foglalhato ? (
                  <>
                    <input
                      type="date"
                      value={adminBookingDates[sz.id]?.erkezes || ''}
                      onChange={e => handleDateChange(sz.id, 'erkezes', e.target.value)}
                    />
                    <input
                      type="date"
                      value={adminBookingDates[sz.id]?.tavozas || ''}
                      onChange={e => handleDateChange(sz.id, 'tavozas', e.target.value)}
                    />
                    <button onClick={() => handleRoomBooking(sz.id)}>Foglalás</button>
                  </>
                ) : 'N/A'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2>Új szoba létrehozása</h2>
      <div className="room-form">
        <input placeholder="Szobaszám" value={newRoom.szobaszam} onChange={e => setNewRoom({ ...newRoom, szobaszam: e.target.value })} />
        <input placeholder="Típus" value={newRoom.tipus} onChange={e => setNewRoom({ ...newRoom, tipus: e.target.value })} />
        <input type="number" placeholder="Férőhely" value={newRoom.ferohelyekSzama} onChange={e => setNewRoom({ ...newRoom, ferohelyekSzama: e.target.value })} />
        <input placeholder="Felszereltség" value={newRoom.felszereltseg} onChange={e => setNewRoom({ ...newRoom, felszereltseg: e.target.value })} />
        <label>
          <input type="checkbox" checked={newRoom.foglalhato} onChange={e => setNewRoom({ ...newRoom, foglalhato: e.target.checked })} />
          Foglalható
        </label>
        <button onClick={handleCreateRoom}>Szoba hozzáadása</button>
      </div>
    </div>
  );
}
