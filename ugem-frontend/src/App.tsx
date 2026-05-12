import { Routes, Route } from 'react-router-dom';
import Navbar from './components/layout/Navbar';
import DiscoveryPage from './features/discovery/DiscoveryPage';

function App() {
  return (
    <div className="min-h-screen flex flex-col relative overflow-hidden bg-background">
      <Navbar />
      
      {/* Dynamic Background Elements */}
      <div className="absolute top-[-10%] left-[-10%] w-[40%] h-[40%] bg-indigo-600/20 blur-[120px] rounded-full pointer-events-none" />
      <div className="absolute bottom-[-10%] right-[-10%] w-[40%] h-[40%] bg-purple-600/20 blur-[120px] rounded-full pointer-events-none" />
      
      <main className="relative z-10 w-full max-w-7xl mx-auto px-6 py-10 flex-1">
        <Routes>
          <Route path="/" element={<Landing />} />
          <Route path="/discovery" element={<DiscoveryPage />} />
          <Route path="/vouchers" element={<div className="text-center py-20 text-gray-500">Marketplace Coming Soon (UI Baseline Ready)</div>} />
          <Route path="/login" element={<div className="text-center">Login Page Placeholder</div>} />
        </Routes>
      </main>
    </div>
  );
}

function Landing() {
  return (
    <div className="text-center space-y-8 py-20">
      <div className="space-y-4">
        <h1 className="text-6xl md:text-8xl font-bold tracking-tight bg-clip-text text-transparent bg-gradient-to-b from-white to-gray-500">
          UGem Platform
        </h1>
        <p className="text-xl md:text-2xl text-gray-400 max-w-2xl mx-auto font-light leading-relaxed">
          The next generation of discovery. 
          <span className="text-indigo-400"> Premium</span>, 
          <span className="text-purple-400"> Scalable</span>, and 
          <span className="text-pink-400"> Secure</span>.
        </p>
      </div>

      <div className="flex flex-col sm:flex-row items-center justify-center gap-4">
        <button className="btn-primary">
          Explore Discovery
        </button>
        <button className="px-6 py-2.5 glass-card hover:bg-white/5 transition-all">
          Learn More
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 pt-20">
        <FeatureCard 
          title="Zero-Trust Security" 
          desc="Enterprise-grade Argon2id and Refresh Token Rotation logic." 
        />
        <FeatureCard 
          title="Spatial Intelligence" 
          desc="Advanced PostGIS integration for high-performance discovery." 
        />
        <FeatureCard 
          title="Micro-Modular" 
          desc="Clean Architecture with strict domain isolation and governance." 
        />
      </div>
    </div>
  );
}

function FeatureCard({ title, desc }: { title: string; desc: string }) {
  return (
    <div className="glass-card p-8 text-left hover:border-indigo-500/50 transition-colors group">
      <div className="w-12 h-12 bg-indigo-500/10 rounded-xl flex items-center justify-center mb-6 group-hover:scale-110 transition-transform">
        <div className="w-2 h-2 bg-indigo-500 rounded-full" />
      </div>
      <h3 className="text-xl font-semibold mb-2">{title}</h3>
      <p className="text-gray-400 leading-relaxed text-sm">
        {desc}
      </p>
    </div>
  );
}

export default App;
