import React, { useEffect, useState } from 'react';
import { getReservations } from '../api/api';

const ReservationList = () => {
  const [reservations, setReservations] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem("token");
      const data = await getReservations(token);
      setReservations(data);
    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>All Reservations</h2>
      <ul>
        {reservations.map((r, idx) => (
          <li key={idx}>{r.user} - Room {r.room} on {r.date}</li>
        ))}
      </ul>
    </div>
  );
};

export default ReservationList;
