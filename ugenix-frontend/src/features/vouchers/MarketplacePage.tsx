import React, { useState } from 'react';
import { Search, Filter, Tag, Clock, ArrowRight } from 'lucide-react';

export default function MarketplacePage() {
  const [searchQuery, setSearchQuery] = useState('');

  return (
    <div className="space-y-8 animate-fade-in max-w-4xl mx-auto pt-8">
      <div className="flex items-center justify-between">
        <h1 className="text-3xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-white to-gray-500">
          Voucher Marketplace
        </h1>
      </div>

      <div className="space-y-6">
        <div className="flex gap-4 items-center">
          <div className="relative group flex-1">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-500 group-focus-within:text-indigo-400 transition-colors" />
            <input 
              type="text" 
              placeholder="Search premium vouchers..." 
              className="input-field pl-12 w-full"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div className="flex gap-2 w-1/3">
            <button className="flex-1 py-3 glass-card text-xs font-bold uppercase tracking-wider text-gray-400 hover:text-white hover:border-indigo-500/50 transition-all flex items-center justify-center gap-2">
              <Filter className="w-4 h-4" /> Category
            </button>
            <button className="flex-1 py-3 glass-card text-xs font-bold uppercase tracking-wider text-gray-400 hover:text-white hover:border-indigo-500/50 transition-all flex items-center justify-center gap-2">
              <Clock className="w-4 h-4" /> Expiring Soon
            </button>
          </div>
        </div>

        <div className="space-y-6 pt-4">
          <h3 className="text-sm font-bold uppercase tracking-widest text-indigo-400 px-1">Featured Deals</h3>
          <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
            {[1, 2, 3].map((i) => (
              <div key={i} className="glass-card p-6 hover:border-indigo-500/50 transition-all cursor-pointer group flex flex-col justify-between h-full">
                <div>
                  <div className="flex justify-between items-start mb-4">
                    <div className="w-14 h-14 bg-indigo-500/10 rounded-xl flex items-center justify-center">
                      <Tag className="w-7 h-7 text-indigo-500" />
                    </div>
                    <span className="text-[10px] font-bold bg-green-500/10 text-green-400 px-3 py-1 rounded-full uppercase">Active</span>
                  </div>
                  <h4 className="font-bold text-xl mb-2 group-hover:text-indigo-400 transition-colors">Premium Dining {i * 10}% Off</h4>
                  <p className="text-gray-400 text-sm mb-6">Valid at all participating high-end restaurants for an exclusive dining experience.</p>
                </div>
                <div className="flex items-center justify-between pt-4 border-t border-white/5">
                  <span className="text-lg font-bold text-white">Free</span>
                  <button className="text-sm font-bold text-indigo-400 flex items-center gap-2 hover:gap-3 transition-all">
                    Claim Now <ArrowRight className="w-4 h-4" />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
