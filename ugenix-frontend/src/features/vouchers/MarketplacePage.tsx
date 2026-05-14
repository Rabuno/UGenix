import React, { useState } from 'react';
import { Search, Filter, Tag, Clock, ArrowRight } from 'lucide-react';
import { motion } from 'framer-motion';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Card, CardContent } from '../../components/ui/Card';

export default function MarketplacePage() {
  const [searchQuery, setSearchQuery] = useState('');

  const container = {
    hidden: { opacity: 0 },
    show: {
      opacity: 1,
      transition: { staggerChildren: 0.05 }
    }
  };

  const item = {
    hidden: { opacity: 0, y: 10 },
    show: { opacity: 1, y: 0 }
  };

  return (
    <motion.div 
      initial="hidden"
      animate="show"
      variants={container}
      className="space-y-8 max-w-5xl mx-auto pt-8 pb-20"
    >
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold text-slate-50">
          Voucher Marketplace
        </h1>
      </div>

      <div className="space-y-8">
        <div className="flex flex-col md:flex-row gap-4 items-center">
          <div className="relative group w-full md:flex-1">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-slate-500 group-focus-within:text-violet-400 transition-colors z-10" />
            <Input 
              type="text" 
              placeholder="Search premium vouchers..." 
              className="pl-12"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div className="flex gap-3 w-full md:w-auto">
            <Button variant="secondary" className="flex-1 md:w-auto flex items-center gap-2 h-12">
              <Filter className="w-4 h-4" /> Category
            </Button>
            <Button variant="secondary" className="flex-1 md:w-auto flex items-center gap-2 h-12">
              <Clock className="w-4 h-4" /> Expiring Soon
            </Button>
          </div>
        </div>

        <div className="space-y-6 pt-2">
          <div className="flex items-center justify-between px-1">
            <h3 className="text-sm font-bold uppercase tracking-widest text-violet-400">Featured Deals</h3>
            <span className="text-xs text-slate-500 font-medium cursor-pointer hover:text-slate-300">View all</span>
          </div>
          
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {[1, 2, 3, 4, 5, 6].map((i) => (
              <motion.div variants={item} key={i}>
                <Card className="h-full flex flex-col group cursor-pointer">
                  <CardContent className="p-6 flex flex-col justify-between h-full space-y-6">
                    <div>
                      <div className="flex justify-between items-start mb-5">
                        <div className="w-14 h-14 bg-violet-500/10 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform duration-300">
                          <Tag className="w-6 h-6 text-violet-500" />
                        </div>
                        <span className="text-[10px] font-bold bg-green-500/10 text-green-400 px-3 py-1.5 rounded-full uppercase tracking-wider">Active</span>
                      </div>
                      <h4 className="font-bold text-xl mb-2 text-slate-50 group-hover:text-violet-400 transition-colors">Premium Dining {i * 10}% Off</h4>
                      <p className="text-slate-400 text-sm leading-relaxed line-clamp-2">Valid at all participating high-end restaurants for an exclusive dining experience.</p>
                    </div>
                    
                    <div className="flex items-center justify-between pt-5 border-t border-slate-800">
                      <div className="flex flex-col">
                        <span className="text-xs text-slate-500 line-through">500,000đ</span>
                        <span className="text-lg font-bold text-slate-50">Free</span>
                      </div>
                      <span className="text-sm font-bold text-violet-400 flex items-center gap-1.5 group-hover:gap-2.5 transition-all">
                        Claim Now <ArrowRight className="w-4 h-4" />
                      </span>
                    </div>
                  </CardContent>
                </Card>
              </motion.div>
            ))}
          </div>
        </div>
      </div>
    </motion.div>
  );
}
