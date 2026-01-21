import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../api/reservations";

export default function RegisterPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  function validatePhone(phone: string) {
    return /^\+?[0-9]{9,15}$/.test(phone);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!username || !password || !phoneNumber) {
      setError("Wypełnij wszystkie pola.");
      return;
    }

    if (!validatePhone(phoneNumber)) {
      setError("Nieprawidłowy numer telefonu.");
      return;
    }

    try {
      await register(username, password, phoneNumber);
      navigate("/login");
    } catch (err: any) {
      setError(err.response?.data?.message || "Błąd rejestracji");
    }
  }

  return (
    <div className="p-4">
      <h1>Rejestracja</h1>

      <form onSubmit={handleSubmit}>
        <input
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          placeholder="Username"
        />

        <input
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
          type="password"
        />

        <input
          value={phoneNumber}
          onChange={(e) => setPhoneNumber(e.target.value)}
          placeholder="+48123456789"
        />

        <button type="submit">Zarejestruj</button>
      </form>

      {error && <div style={{ color: "red" }}>{error}</div>}
    </div>
  );
}
