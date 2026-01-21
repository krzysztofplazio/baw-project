import { useEffect, useState } from "react";
import { getServices } from "../api/reservations";
import type { Service } from "../types";
import { Link } from "react-router-dom";

export default function ServicesPage() {
  const [services, setServices] = useState<Service[]>([]);
  useEffect(() => {
    getServices().then(setServices);
  }, []);

  return (
    <div className="p-4">
      <h1>Services</h1>
      <Link to="/create">Create Appointment</Link>
      <ul>
        {services.map((s) => (
          <li key={s.id}>
            {s.name} <span>&ensp;{s.price} PLN</span>
          </li>
        ))}
      </ul>
    </div>
  );
}
