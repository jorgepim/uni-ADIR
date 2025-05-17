import React from 'react';
import { Users, UserPlus, Activity, LogOut, ArrowLeft } from 'lucide-react';
import { useAuth } from '../context/AuthContext';
import { User } from '../types';
import AddSpecialistForm from './AddSpecialistForm';

interface SpecialistsListProps {
  onBackToDashboard: () => void;
}

export default function SpecialistsList({ onBackToDashboard }: SpecialistsListProps) {
  const { logout } = useAuth();
  const [specialists, setSpecialists] = React.useState<User[]>([]);
  const [showAddSpecialist, setShowAddSpecialist] = React.useState(false);

  React.useEffect(() => {
    const storedSpecialists = localStorage.getItem('specialists');
    if (storedSpecialists) {
      setSpecialists(JSON.parse(storedSpecialists));
    }
  }, []);

  const handleAddSpecialist = (specialist: User) => {
    const updatedSpecialists = [...specialists, specialist];
    setSpecialists(updatedSpecialists);
    localStorage.setItem('specialists', JSON.stringify(updatedSpecialists));
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
                <span className="ml-2 text-xl font-semibold">Admin Dashboard</span>
              </div>
            </div>
            <div className="flex items-center space-x-4">
              <button
                onClick={() => setShowAddSpecialist(true)}
                className="flex items-center px-4 py-2 bg-indigo-600 text-white rounded-md hover:bg-indigo-700"
              >
                <UserPlus className="h-5 w-5 mr-2" />
                Agregar Especialista
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
        <h1 className="text-2xl font-semibold text-gray-900 mb-6">Todos los Especialistas</h1>
        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {specialists.map((specialist) => (
              <li key={specialist.id} className="px-6 py-4 hover:bg-gray-50">
                <div className="flex items-center justify-between">
                  <div>
                    <h3 className="text-lg font-medium text-gray-900">{specialist.name}</h3>
                    <p className="text-sm text-gray-500">{specialist.email}</p>
                  </div>
                  <span className="px-3 py-1 text-xs font-medium text-green-800 bg-green-100 rounded-full">
                    Activo
                  </span>
                </div>
              </li>
            ))}
          </ul>
        </div>
      </div>

      {showAddSpecialist && (
        <AddSpecialistForm
          onClose={() => setShowAddSpecialist(false)}
          onAdd={handleAddSpecialist}
        />
      )}
    </div>
  );
}