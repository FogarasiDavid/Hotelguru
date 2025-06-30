import React, { useState } from 'react';
import { createReservation } from '../api/api';

const BookingForm = () => {
  const [room, setRoom] = useState('');
  const [date, setDate] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
    const result = await createReservation({ room, date }, token);
    alert(result.message || "Reservation created");
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Book a Room</h2>
      <input value={room} onChange={e => setRoom(e.target.value)} placeholder="Room number" />
      <input type="date" value={date} onChange={e => setDate(e.target.value)} />
      <button type="submit">Reserve</button>
    </form>
  );
};

export default BookingForm;
