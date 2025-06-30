import React from 'react';
import ReservationList from '../components/ReservationList';
import BookingForm from '../components/BookingForm';

const Dashboard = () => {
  const role = localStorage.getItem("role");

  return (
    <div>
      <h1>Dashboard</h1>
      {role === "admin" ? <ReservationList /> : <BookingForm />}
    </div>
  );
};

export default Dashboard;
