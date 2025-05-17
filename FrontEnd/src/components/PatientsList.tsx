import React from 'react';
import { Users, UserPlus, Activity, LogOut, ArrowLeft } from 'lucide-react';
import { Patient } from '../types';
import { useAuth } from '../context/AuthContext';
import AddPatientForm from './AddPatientForm';

interface PatientsListProps {
  onBackToDashboard: () => void;
}

export default function PatientsList({ onBackToDashboard }: PatientsListProps) {
  const { logout, user } = useAuth();
  const [patients, setPatients] = React.useState<Patient[]>([]);
  const [showAddPatient, setShowAddPatient] = React.useState(false);

  React.useEffect(() => {
    const storedPatients = localStorage.getItem('patients');
    if (storedPatients) {
      const allPatients = JSON.parse(storedPatients);
      const specialistPatients = allPatients.filter(
        (patient: Patient) => patient.specialistId === user?.id
      );
      setPatients(specialistPatients);
    }
  }, [user?.id]);

  const handleAddPatient = (patient: Patient) => {
    const allPatients = JSON.parse(localStorage.getItem('patients') || '[]');
    const updatedPatients = [...allPatients, patient];
    localStorage.setItem('patients', JSON.stringify(updatedPatients));
    setPatients(prev => [...prev, patient]);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center space-x-4">
              <button
                onClick={onBackToDashboard}
                className="flex items-center text-gray-600 hover:text-gray-800"
              >
                <ArrowLeft className="h-5 w-5 mr-2" />
                
              </button>
              <div className="flex items-center">
                <Activity className="h-8 w-8 text-indigo-600" />
                <span className="ml-2 text-xl font-semibold">Especialista Dashboard </span>
              </div>
            </div>
            <div className="flex items-center space-x-4">
              <button
                onClick={() => setShowAddPatient(true)}
                className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
              >
                <UserPlus className="h-5 w-5 mr-2" />
                Agregar Paciente
              </button>
              <button
                onClick={logout}
                className="flex items-center px-4 text-gray-600 hover:text-gray-800"
              >
                <LogOut className="h-5 w-5 mr-2" />
                Cerrar Sesión
              </button>
            </div>
          </div>
        </div>
      </nav>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-2xl font-semibold text-gray-900 mb-6">Mis Pacientes</h1>
        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {patients.map((patient) => (
              <li key={patient.id} className="px-6 py-4 hover:bg-gray-50">
                <div className="flex items-center justify-between">
                  <div>
                    <h3 className="text-lg font-medium text-gray-900">{patient.name}</h3>
                    <p className="text-sm text-gray-500">Fecha de Nacimiento: {new Date(patient.dateOfBirth).toLocaleDateString()}</p>
                  </div>
                  <span className="text-sm text-gray-500">
                    Registro: {new Date(patient.createdAt).toLocaleDateString()}
                  </span>
                </div>
              </li>
            ))}
          </ul>
        </div>
      </div>

      {showAddPatient && (
        <AddPatientForm
          onClose={() => setShowAddPatient(false)}
          onAdd={handleAddPatient}
          specialistId={user?.id || ''}
        />
      )}
    </div>
  );
}