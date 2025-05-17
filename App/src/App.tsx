import React from 'react';
import { AuthProvider } from './context/AuthContext';
import LoginForm from './components/LoginForm';
import AdminDashboard from './components/AdminDashboard';
import SpecialistDashboard from './components/SpecialistDashboard';
import SpecialistsList from './components/SpecialistsList';
import PatientsList from './components/PatientsList';
import { useAuth } from './context/AuthContext';

function DashboardRouter() {
  const { user } = useAuth();
  const [view, setView] = React.useState<'dashboard' | 'list'>('dashboard');

  if (!user) {
    return <LoginForm />;
  }

  if (view === 'list') {
    return user.role === 'admin' ? (
      <SpecialistsList onBackToDashboard={() => setView('dashboard')} />
    ) : (
      <PatientsList onBackToDashboard={() => setView('dashboard')} />
    );
  }

  return user.role === 'admin' ? (
    <AdminDashboard onViewList={() => setView('list')} />
  ) : (
    <SpecialistDashboard onViewList={() => setView('list')} />
  );
}

function App() {
  return (
    <AuthProvider>
      <DashboardRouter />
    </AuthProvider>
  );
}

export default App;