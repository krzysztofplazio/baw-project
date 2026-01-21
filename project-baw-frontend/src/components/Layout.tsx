import { NavLink, useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/authStore";
import type {ReactNode} from "react";

export default function Layout({ children }: { children: ReactNode }) {
  const logout = useAuthStore((s) => s.logout);
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login");
  }

  const linkStyle = ({ isActive }: { isActive: boolean }) => ({
    marginRight: 12,
    textDecoration: "none",
    fontWeight: isActive ? "bold" : "normal",
  });

  return (
    <div>
      <header
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          padding: 16,
          borderBottom: "1px solid #ccc",
        }}
      >
        <h2>Reservations</h2>

        <nav>
          <NavLink to="/services" style={linkStyle}>
            Services
          </NavLink>
          <NavLink to="/create" style={linkStyle}>
            New appointment
          </NavLink>
          <NavLink to="/my" style={linkStyle}>
            My appointments
          </NavLink>
        </nav>

        <button onClick={handleLogout}>Wyloguj</button>
      </header>

      <main style={{ padding: 16 }}>{children}</main>
    </div>
  );
}
