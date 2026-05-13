import React from 'react';
import { User, Settings, Shield, Bell, LogOut, ChevronRight, CreditCard } from 'lucide-react';

export default function ProfilePage() {
  return (
    <div className="space-y-8 animate-fade-in max-w-3xl mx-auto pt-8">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-white to-gray-500">
          Your Profile
        </h1>
      </div>

      <div className="space-y-8">
        <div className="flex items-center gap-6 p-8 glass-card border-indigo-500/20 bg-indigo-500/5">
          <div className="w-24 h-24 bg-indigo-600 rounded-full flex items-center justify-center text-4xl font-bold shadow-xl shadow-indigo-500/20">JD</div>
          <div>
            <h3 className="text-3xl font-bold mb-2">John Doe</h3>
            <p className="text-md text-indigo-400 font-medium flex items-center gap-2">
              <Shield className="w-4 h-4" /> Premium Member
            </p>
          </div>
        </div>

        <div className="space-y-4">
          <h4 className="text-xs font-bold uppercase tracking-widest text-gray-500 px-1 mb-4">Account Settings</h4>
          <div className="grid gap-4 md:grid-cols-2">
            <ProfileItem icon={<User className="w-5 h-5" />} label="Personal Information" />
            <ProfileItem icon={<CreditCard className="w-5 h-5" />} label="Payment Methods" />
            <ProfileItem icon={<Bell className="w-5 h-5" />} label="Notifications" />
            <ProfileItem icon={<Shield className="w-5 h-5" />} label="Security & Privacy" />
          </div>
        </div>

        <div className="pt-8">
          <button className="w-full py-4 glass-card flex items-center justify-center gap-2 text-red-400 hover:bg-red-400/10 hover:border-red-400/50 transition-all font-bold text-lg">
            <LogOut className="w-5 h-5" /> Sign Out
          </button>
        </div>
      </div>
    </div>
  );
}

function ProfileItem({ icon, label }: { icon: React.ReactNode; label: string }) {
  return (
    <button className="w-full p-6 glass-card flex items-center justify-between hover:bg-white/5 hover:border-indigo-500/50 transition-all group">
      <div className="flex items-center gap-4">
        <div className="text-gray-400 group-hover:text-indigo-400 transition-colors">{icon}</div>
        <span className="text-md font-medium text-gray-300 group-hover:text-white transition-colors">{label}</span>
      </div>
      <ChevronRight className="w-5 h-5 text-gray-600 group-hover:text-white transition-all" />
    </button>
  );
}
