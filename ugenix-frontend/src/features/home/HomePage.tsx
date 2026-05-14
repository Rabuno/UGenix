import React from 'react';
import { motion } from 'framer-motion';
import { Link } from 'react-router-dom';
import { Shield, Map as MapIcon, Zap, ChevronRight, Sparkles } from 'lucide-react';
import { Button } from '../../components/ui/Button';
import { Card, CardContent } from '../../components/ui/Card';

export default function HomePage() {
  const container = {
    hidden: { opacity: 0 },
    show: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1
      }
    }
  };

  const item = {
    hidden: { opacity: 0, y: 20 },
    show: { opacity: 1, y: 0 }
  };

  return (
    <motion.div 
      initial="hidden"
      animate="show"
      variants={container}
      className="space-y-24 pb-20"
    >
      {/* Hero Section */}
      <motion.section variants={item} className="text-center space-y-8 pt-16">
        <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-violet-500/10 border border-violet-500/20 text-violet-400 text-xs font-bold tracking-wider uppercase">
          <Sparkles className="w-3.5 h-3.5" />
          <span>v1.0.0 Alpha - Developer Preview</span>
        </div>
        
        <div className="space-y-6">
          <h1 className="text-5xl md:text-7xl font-extrabold tracking-tight text-slate-50">
            UGenix <span className="bg-amethyst-gradient bg-clip-text text-transparent">Platform</span>
          </h1>
          <p className="text-xl text-slate-400 max-w-2xl mx-auto font-light leading-relaxed">
            The next generation of spatial discovery. 
            <span className="text-violet-400 font-medium"> Premium</span>, 
            <span className="text-violet-400 font-medium"> Scalable</span>, and 
            <span className="text-violet-400 font-medium"> Secure</span>.
          </p>
        </div>

        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 pt-4">
          <Link to="/discovery">
            <Button size="lg" className="flex items-center gap-2">
              <span>Explore Discovery</span>
              <ChevronRight className="w-4 h-4" />
            </Button>
          </Link>
          <Button variant="secondary" size="lg">
            Documentation
          </Button>
        </div>
      </motion.section>

      {/* Feature Grid */}
      <motion.section variants={item} className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <FeatureCard 
          icon={<Shield className="w-6 h-6 text-violet-500" />}
          title="Zero-Trust Security" 
          desc="Enterprise-grade Argon2id and Refresh Token Rotation logic for maximum security." 
        />
        <FeatureCard 
          icon={<MapIcon className="w-6 h-6 text-violet-500" />}
          title="Spatial Intelligence" 
          desc="Advanced PostGIS integration for high-performance spatial discovery and analytics." 
        />
        <FeatureCard 
          icon={<Zap className="w-6 h-6 text-violet-500" />}
          title="Micro-Modular" 
          desc="Clean Architecture with strict domain isolation and governance for scalability." 
        />
      </motion.section>

      {/* Social Proof / Stats */}
      <motion.section variants={item}>
        <Card className="bg-surface/50 border-violet-900/20 backdrop-blur-sm">
          <CardContent className="p-12 text-center space-y-12">
            <div className="max-w-3xl mx-auto space-y-4">
              <h2 className="text-3xl font-bold text-slate-50">Built for Extreme Performance</h2>
              <p className="text-slate-400 text-lg">
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
          </CardContent>
        </Card>
      </motion.section>
    </motion.div>
  );
}

function FeatureCard({ icon, title, desc }: { icon: React.ReactNode; title: string; desc: string }) {
  return (
    <Card className="group h-full">
      <CardContent className="p-8 text-left">
        <div className="w-12 h-12 bg-violet-500/10 rounded-xl flex items-center justify-center mb-6 group-hover:scale-110 group-hover:bg-violet-500/20 transition-all duration-300">
          {icon}
        </div>
        <h3 className="text-xl font-semibold mb-3 text-slate-50">{title}</h3>
        <p className="text-slate-400 leading-relaxed text-sm">
          {desc}
        </p>
      </CardContent>
    </Card>
  );
}

function StatCard({ label, value }: { label: string; value: string }) {
  return (
    <div className="space-y-2">
      <p className="text-3xl font-bold text-slate-50">{value}</p>
      <p className="text-xs text-violet-400 font-semibold uppercase tracking-wider">{label}</p>
    </div>
  );
}
