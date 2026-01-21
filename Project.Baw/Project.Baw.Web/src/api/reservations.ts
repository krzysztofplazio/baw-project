import apiClient from "./apiClient";
import type {Appointment, Service} from "../types.ts";

export async function login(username: string, password: string) {
  const data = new URLSearchParams();
  data.append("grant_type", "password");
  data.append("username", username);
  data.append("password", password);

  const res = await apiClient.post("/connect/token", data);
  return res.data;
}

export async function getServices(): Promise<Service[]> {
  const res = await apiClient.get("/api/services");
  return res.data;
}

export async function getMyAppointments(): Promise<Appointment[]> {
  const res = await apiClient.get("/api/appointments/me");
  return res.data;
}

export async function createAppointment(date: string, serviceId: string) {
  const selectedDate = new Date(date);

  const tomorrow = new Date();
  tomorrow.setDate(tomorrow.getDate() + 1);
  tomorrow.setHours(0, 0, 0, 0);

  if (selectedDate < tomorrow) {
    alert("Nie można zarejestrować wizyty wcześniej niż od jutra.");
    return;
  }

  return apiClient.post("/api/appointments", { date, serviceId });
}


export async function deleteAppointment(id: string) {
  return apiClient.delete(`/api/appointments/${id}`);
}

export async function register(username: string, password: string, phone: string) {
  const res = await apiClient.post("/connect/register", {
    username,
    password,
    phone
  });
  return res.data;
}
