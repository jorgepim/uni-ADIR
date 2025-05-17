export interface User {
  id: string;
  name: string;
  email: string;
  role: 'admin' | 'specialist';
  active: boolean;
  password: string;
}

export interface Patient {
  id: string;
  name: string;
  dateOfBirth: string;
  specialistId: string;
  createdAt: string;
  updatedAt: string;
}

export interface Assessment {
  id: string;
  patientId: string;
  specialistId: string;
  type: 'ADOS-2' | 'ADI-R';
  date: string;
  score: number;
  observations: string;
  responses: Record<string, number | string>;
}