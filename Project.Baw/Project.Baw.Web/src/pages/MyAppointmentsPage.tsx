import { useEffect, useState } from "react";
import { getMyAppointments, deleteAppointment } from "../api/reservations";
import type { Appointment } from "../types";

export default function MyAppointmentsPage() {
  const [appointments, setAppointments] = useState<Appointment[]>([]);

  useEffect(() => {
    getMyAppointments().then(setAppointments);
  }, []);

  async function handleDelete(id: string) {
    await deleteAppointment(id);
    setAppointments((prev) => prev.filter((a) => a.id !== id));
  }

  return (
    <div className="p-4">
      <h1>My Appointments</h1>
      <ul>
        {appointments.map((a) => (
          <li key={a.id}>
            {a.date} - {a.service}
            <button onClick={() => handleDelete(a.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  );
}
