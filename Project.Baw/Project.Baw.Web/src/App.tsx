import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import ServicesPage from "./pages/ServicesPage";
import MyAppointmentsPage from "./pages/MyAppointmentsPage";
import CreateAppointmentPage from "./pages/CreateAppointmentPage";
import AuthGuard from "./components/AuthGuard";
import RegisterPage from "./pages/RegisterPage.tsx";
import Layout from "./components/Layout.tsx";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route
          path="/services"
          element={
            <AuthGuard>
              <Layout>
                <ServicesPage />
              </Layout>
            </AuthGuard>
          }
        />

        <Route
          path="/my"
          element={
            <AuthGuard>
              <Layout>
                <MyAppointmentsPage />
              </Layout>
            </AuthGuard>
          }
        />

        <Route
          path="/create"
          element={
            <AuthGuard>
              <Layout>
                <CreateAppointmentPage />
              </Layout>
            </AuthGuard>
          }
        />

        <Route path="*" element={<Navigate to="/services" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
