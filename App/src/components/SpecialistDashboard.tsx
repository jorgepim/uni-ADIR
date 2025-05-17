import React, { useState } from 'react';
import { Users, ClipboardList, Activity, LogOut, UserPlus } from 'lucide-react';
import { useAuth } from '../context/AuthContext';
import AddPatientForm from './AddPatientForm';
import { Patient } from '../types';

interface SpecialistDashboardProps {
  onViewList: () => void;
}

export default function SpecialistDashboard({ onViewList }: SpecialistDashboardProps) {
  const { logout, user } = useAuth();
  const [showAddPatient, setShowAddPatient] = useState(false);
  const [patientsCount, setPatientsCount] = useState(0);

  React.useEffect(() => {
    const patients = JSON.parse(localStorage.getItem('patients') || '[]');
    const specialistPatients = patients.filter(
      (patient: Patient) => patient.specialistId === user?.id
    );
    setPatientsCount(specialistPatients.length);
  }, [user?.id]);

  const handleAddPatient = (patient: Patient) => {
    const patients = JSON.parse(localStorage.getItem('patients') || '[]');
    patients.push(patient);
    localStorage.setItem('patients', JSON.stringify(patients));
    setPatientsCount((prev) => prev + 1);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between h-16">
            <div className="flex items-center">
              <Activity className="h-8 w-8 text-indigo-600" />
              <span className="ml-2 text-xl font-semibold">Especialista Dashboard</span>
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

      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          <div className="grid grid-cols-1 gap-6 sm:grid-cols-2">
            <div className="bg-white overflow-hidden shadow rounded-lg">
              <div className="p-5">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <Users className="h-6 w-6 text-gray-400" />
                  </div>
                  <div className="ml-5 w-0 flex-1">
                    <dl>
                      <dt className="text-sm font-medium text-gray-500 truncate">
                        Mis Pacientes
                      </dt>
                      <dd className="text-3xl font-semibold text-gray-900">{patientsCount}</dd>
                    </dl>
                  </div>
                </div>
              </div>
              <div className="bg-gray-50 px-5 py-3">
                <div className="text-sm">
                  <button
                    onClick={onViewList}
                    className="font-medium text-indigo-600 hover:text-indigo-900"
                  >
                    Ver todos mis pacientes
                  </button>
                </div>
              </div>
            </div>

            <div className="bg-white overflow-hidden shadow rounded-lg">
              <div className="p-5">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <ClipboardList className="h-6 w-6 text-gray-400" />
                  </div>
                  <div className="ml-5 w-0 flex-1">
                    <dl>
                      <dt className="text-sm font-medium text-gray-500 truncate">
                        Evaluaciones Pendientes
                      </dt>
                      <dd className="text-3xl font-semibold text-gray-900">2</dd>
                    </dl>
                  </div>
                </div>
              </div>
              <div className="bg-gray-50 px-5 py-3">
                <div className="text-sm">
                  <a href="#" className="font-medium text-indigo-600 hover:text-indigo-900">
                    Ver Evaluaciones
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>

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