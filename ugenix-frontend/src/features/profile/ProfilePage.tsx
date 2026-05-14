import React from 'react';
import { User, Shield, Bell, LogOut, ChevronRight, CreditCard } from 'lucide-react';
import { motion } from 'framer-motion';
import { Card, CardContent } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';

export default function ProfilePage() {
  const container = {
    hidden: { opacity: 0 },
    show: {
      opacity: 1,
      transition: { staggerChildren: 0.1 }
    }
  };

  const item = {
    hidden: { opacity: 0, x: -20 },
    show: { opacity: 1, x: 0 }
  };

  return (
    <motion.div 
      initial="hidden"
      animate="show"
      variants={container}
      className="space-y-8 max-w-3xl mx-auto pt-8 pb-20"
    >
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-slate-50">
          Your Profile
        </h1>
      </div>

      <div className="space-y-8">
        <motion.div variants={item}>
          <Card className="border-violet-500/20 bg-violet-900/10 shadow-lg shadow-violet-900/10">
            <CardContent className="flex items-center gap-6 p-8">
              <div className="w-24 h-24 bg-amethyst-gradient rounded-full flex items-center justify-center text-4xl font-bold shadow-xl shadow-violet-500/20 text-white shrink-0">JD</div>
              <div>
                <h3 className="text-3xl font-bold mb-2 text-slate-50">John Doe</h3>
                <p className="text-md text-violet-400 font-medium flex items-center gap-2">
                  <Shield className="w-4 h-4" /> Premium Member
                </p>
              </div>
            </CardContent>
          </Card>
        </motion.div>

        <div className="space-y-4">
          <motion.h4 variants={item} className="text-xs font-bold uppercase tracking-widest text-slate-500 px-1 mb-4">Account Settings</motion.h4>
          <div className="grid gap-4 md:grid-cols-2">
            {[
              { icon: <User className="w-5 h-5" />, label: "Personal Information" },
              { icon: <CreditCard className="w-5 h-5" />, label: "Payment Methods" },
              { icon: <Bell className="w-5 h-5" />, label: "Notifications" },
              { icon: <Shield className="w-5 h-5" />, label: "Security & Privacy" }
            ].map((setting, idx) => (
              <motion.div variants={item} key={idx}>
                <Card className="group cursor-pointer hover:border-violet-500/50">
                  <CardContent className="p-6 flex items-center justify-between">
                    <div className="flex items-center gap-4">
                      <div className="text-slate-400 group-hover:text-violet-400 transition-colors">{setting.icon}</div>
                      <span className="text-md font-medium text-slate-300 group-hover:text-slate-50 transition-colors">{setting.label}</span>
                    </div>
                    <ChevronRight className="w-5 h-5 text-slate-600 group-hover:text-violet-400 transition-all" />
                  </CardContent>
                </Card>
              </motion.div>
            ))}
          </div>
        </div>

        <motion.div variants={item} className="pt-8">
          <Button variant="secondary" className="w-full py-4 text-red-400 border-red-500/20 hover:bg-red-500/10 hover:text-red-300 gap-2 h-14">
            <LogOut className="w-5 h-5" /> Sign Out
          </Button>
        </motion.div>
      </div>
    </motion.div>
  );
}
