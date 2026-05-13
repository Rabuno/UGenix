import React from 'react';
import { Link } from 'react-router-dom';
import { Shield, Map as MapIcon, Zap, ChevronRight, Sparkles } from 'lucide-react';

export default function HomePage() {
  return (
    <div className="space-y-24 pb-20">
      {/* Hero Section */}
      <section className="text-center space-y-8 pt-16">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-indigo-500/10 border border-indigo-500/20 text-indigo-400 text-xs font-bold tracking-wider uppercase animate-fade-in">
          <Sparkles className="w-3 h-3" />
          <span>v1.0.0 Alpha - Developer Preview</span>
        </div>
        
        <div className="space-y-4">
          <h1 className="text-6xl md:text-8xl font-bold tracking-tight bg-clip-text text-transparent bg-gradient-to-b from-white to-gray-500">
            UGenix Platform
          </h1>
          <p className="text-xl md:text-2xl text-gray-400 max-w-2xl mx-auto font-light leading-relaxed">
            The next generation of discovery. 
            <span className="text-indigo-400"> Premium</span>, 
            <span className="text-purple-400"> Scalable</span>, and 
            <span className="text-pink-400"> Secure</span>.
          </p>
        </div>

        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 pt-4">
          <Link to="/discovery" className="btn-primary flex items-center gap-2 px-8 py-3">
            <span>Explore Discovery</span>
            <ChevronRight className="w-4 h-4" />
          </Link>
          <button className="px-8 py-3 glass-card hover:bg-white/5 transition-all text-sm font-semibold">
            Documentation
          </button>
        </div>
      </section>

      {/* Feature Grid */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <FeatureCard 
          icon={<Shield className="w-6 h-6 text-indigo-500" />}
          title="Zero-Trust Security" 
          desc="Enterprise-grade Argon2id and Refresh Token Rotation logic for maximum security." 
        />
        <FeatureCard 
          icon={<MapIcon className="w-6 h-6 text-purple-500" />}
          title="Spatial Intelligence" 
          desc="Advanced PostGIS integration for high-performance spatial discovery and analytics." 
        />
        <FeatureCard 
          icon={<Zap className="w-6 h-6 text-pink-500" />}
          title="Micro-Modular" 
          desc="Clean Architecture with strict domain isolation and governance for scalability." 
        />
      </section>

      {/* Social Proof / Stats */}
      <section className="glass-card p-12 text-center space-y-8">
        <div className="max-w-3xl mx-auto space-y-4">
          <h2 className="text-3xl font-bold">Built for Performance</h2>
          <p className="text-gray-400">
            Our architecture is designed to handle millions of requests with sub-millisecond latency, 
            leveraging Redis caching and efficient PostgreSQL indexing.
          </p>
        </div>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
          <StatCard label="Response Time" value="< 50ms" />
          <StatCard label="Availability" value="99.9%" />
          <StatCard label="Security" value="Argon2" />
          <StatCard label="Tech" value="React 18" />
        </div>
      </section>
    </div>
  );
}

function FeatureCard({ icon, title, desc }: { icon: React.ReactNode; title: string; desc: string }) {
  return (
    <div className="glass-card p-8 text-left hover:border-indigo-500/50 transition-colors group">
      <div className="w-12 h-12 bg-white/5 rounded-xl flex items-center justify-center mb-6 group-hover:scale-110 transition-transform">
        {icon}
      </div>
      <h3 className="text-xl font-semibold mb-2">{title}</h3>
      <p className="text-gray-400 leading-relaxed text-sm">
        {desc}
      </p>
    </div>
  );
}

function StatCard({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-1">
      <p className="text-2xl font-bold text-white">{value}</p>
      <p className="text-xs text-gray-500 font-medium uppercase tracking-wider">{label}</p>
    </div>
  );
}
