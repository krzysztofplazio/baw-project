import { useState } from "react";
import {Link, useNavigate } from "react-router-dom";
import { login } from "../api/reservations";
import { useAuthStore } from "../store/authStore";

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const setToken = useAuthStore((s) => s.setToken);
  const navigate = useNavigate();

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const data = await login(username, password);
      setToken(data.access_token);
      navigate("/services");
    } catch {
      setError("Błędne dane logowania");
    }
  }

  return (
    <div className="p-4">
      <h1>Login</h1>
      <form onSubmit={handleSubmit}>
        <input value={username} onChange={(e) => setUsername(e.target.value)} placeholder="Username" />
        <input value={password} onChange={(e) => setPassword(e.target.value)} placeholder="Password" type="password" />
        <button type="submit">Zaloguj</button>   <Link to="/register">Zarejestruj się</Link>
      </form>
      {error && <div>{error}</div>}
    </div>
  );
}
