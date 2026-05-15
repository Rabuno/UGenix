import { useEffect } from 'react';
import { Routes, Route, useSearchParams } from 'react-router-dom';
import Navbar from './components/layout/Navbar';
import DiscoveryPage from './features/discovery/DiscoveryPage';
import HomePage from './features/home/HomePage';
import DemoPage from './features/home/DemoPage';
import AuthModal from './features/auth/AuthModal';
import MarketplacePage from './features/vouchers/MarketplacePage';
import ProfilePage from './features/profile/ProfilePage';
import { useUIStore, ActiveOverlay } from './store/ui.store';
import ProtectedRoute from './components/layout/ProtectedRoute';

function App() {
  const [searchParams, setSearchParams] = useSearchParams();
  const { activeOverlay, setOverlay } = useUIStore();

  // 1. Sync URL -> State (Initial & URL changes)
  useEffect(() => {
    const overlay = searchParams.get('overlay') as ActiveOverlay;
    if (overlay !== activeOverlay) {
      setOverlay(overlay);
    }
  }, [searchParams, activeOverlay, setOverlay]);

  // 2. Sync State -> URL
  const handleCloseOverlay = () => {
    const newParams = new URLSearchParams(searchParams);
    newParams.delete('overlay');
    setSearchParams(newParams);
  };

  const handleOpenOverlay = (type: ActiveOverlay) => {
    const newParams = new URLSearchParams(searchParams);
    if (type) {
      newParams.set('overlay', type);
    } else {
      newParams.delete('overlay');
    }
    setSearchParams(newParams);
  };

  return (
    <div className="min-h-screen flex flex-col relative overflow-hidden bg-background text-white">
      <Navbar onOpenOverlay={handleOpenOverlay} />
      
      {/* Dynamic Background Elements */}
      <div className="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] bg-indigo-600/20 blur-[120px] rounded-full pointer-events-none" />
      <div className="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-purple-600/20 blur-[120px] rounded-full pointer-events-none" />
      
      <main className="relative z-10 w-full max-w-7xl mx-auto px-6 py-10 flex-1">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/demo" element={<DemoPage />} />
          <Route 
            path="/discovery" 
            element={
              <ProtectedRoute>
                <DiscoveryPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/marketplace" 
            element={
              <ProtectedRoute>
                <MarketplacePage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/profile" 
            element={
              <ProtectedRoute>
                <ProfilePage />
              </ProtectedRoute>
            } 
          />
        </Routes>
      </main>

      {/* Global Overlays */}
      <AuthModal isOpen={activeOverlay === 'auth'} onClose={handleCloseOverlay} />
    </div>
  );
}

export default App;
