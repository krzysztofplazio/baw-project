import { useEffect, useState } from "react";
import { getServices, createAppointment } from "../api/reservations";
import type { Service } from "../types";
import { useNavigate } from "react-router-dom";

export default function CreateAppointmentPage() {
  const [services, setServices] = useState<Service[]>([]);
  const [serviceId, setServiceId] = useState("");
  const [date, setDate] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    getServices().then(setServices);
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    await createAppointment(date, serviceId);
    navigate("/my");
  }

  return (
    <div className="p-4">
      <h1>Create Appointment</h1>
      <form onSubmit={handleSubmit}>
        <input type="datetime-local" value={date} onChange={(e) => setDate(e.target.value)} />
        <select value={serviceId} onChange={(e) => setServiceId(e.target.value)}>
          <option value="">Select service</option>
          {services.map((s) => (
            <option value={s.id} key={s.id}>
              {s.name}
            </option>
          ))}
        </select>
        <button type="submit">Create</button>
      </form>
    </div>
  );
}
