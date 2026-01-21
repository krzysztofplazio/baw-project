export interface Service {
  id: string;
  name: string;
  price?: number;
}

export interface Appointment {
  id: string;
  date: string;
  service: string;
}
