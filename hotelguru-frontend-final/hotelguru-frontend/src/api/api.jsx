const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000/api";

export const login = async (credentials) => {
  const res = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(credentials)
  });
  return res.json();
};

export const register = async (data) => {
  const res = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });
  return res.json();
};

export const getReservations = async (token) => {
  const res = await fetch(`${API_URL}/reservations`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return res.json();
};

export const createReservation = async (data, token) => {
  const res = await fetch(`${API_URL}/reserve`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(data)
  });
  return res.json();
};
