import React, { useContext } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, AuthContext } from './context/AuthContext';
import Login from './pages/Login';
import Register from './pages/Register';
import GuestHome from './pages/GuestHome';
import AdminDashboard from './pages/AdminDashboard';
import ReceptionHome from './pages/ReceptionHome';  // új import

function PrivateRoute({ children, roles }) {
  const { user } = useContext(AuthContext);
  if (!user) return <Navigate to="/login" replace />;
  if (roles && !roles.includes(user.role)) return <Navigate to="/login" replace />;
  return children;
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login"    element={<Login />} />
          <Route path="/register" element={<Register />} />

          <Route path="/guest" element={
            <PrivateRoute roles={['Vendeg']}>
              <GuestHome />
            </PrivateRoute>
          }/>

          <Route path="/admin" element={
            <PrivateRoute roles={['Admin']}>
              <AdminDashboard />
            </PrivateRoute>
          }/>

          <Route path="/reception" element={
            <PrivateRoute roles={['Recepcios']}>
              <ReceptionHome />
            </PrivateRoute>
          }/>

          <Route path="*" element={<Navigate to="/login" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
