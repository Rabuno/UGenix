import React from 'react';
import { Ticket, Zap, Clock, ShoppingCart } from 'lucide-react';
import { Voucher } from './voucher.api';

interface VoucherCardProps {
  voucher: Voucher;
  onPurchase: (id: string) => void;
}

export default function VoucherCard({ voucher, onPurchase }: VoucherCardProps) {
  return (
    <div className="glass-card p-5 relative group overflow-hidden border-indigo-500/20 hover:border-indigo-500/50 transition-all">
      {/* Decorative Background Icon */}
      <Ticket className="absolute -right-4 -bottom-4 w-24 h-24 text-indigo-500/5 rotate-12" />

      <div className="flex justify-between items-start relative z-10">
        <div className="space-y-1">
          <div className="flex items-center gap-2 text-indigo-400 font-bold text-xs uppercase tracking-widest">
            <Zap className="w-3 h-3" />
            Limited Deal
          </div>
          <h4 className="text-2xl font-bold text-white">{voucher.code}</h4>
        </div>
        <div className="text-right">
          <div className="text-sm text-gray-500 line-through">
            {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(voucher.price + voucher.discountValue)}
          </div>
          <div className="text-xl font-bold text-indigo-400">
            {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(voucher.price)}
          </div>
        </div>
      </div>

      <div className="mt-6 flex items-center justify-between relative z-10">
        <div className="flex flex-col gap-1">
          <div className="flex items-center gap-1.5 text-xs text-gray-400">
            <Clock className="w-3 h-3" />
            Ends {new Date(voucher.expiresAt).toLocaleDateString()}
          </div>
          <div className="text-[10px] text-gray-600 font-medium">
            {voucher.remainingStock} vouchers left
          </div>
        </div>

        <button 
          onClick={() => onPurchase(voucher.id)}
          className="btn-primary py-2 px-4 flex items-center gap-2 text-sm"
        >
          <ShoppingCart className="w-4 h-4" />
          Buy Now
        </button>
      </div>
    </div>
  );
}
