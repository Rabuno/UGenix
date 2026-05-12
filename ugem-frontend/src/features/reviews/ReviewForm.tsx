import React, { useState } from 'react';
import { Star, Send, ShieldCheck } from 'lucide-react';
import { reviewApi } from './review.api';
import { clsx } from 'clsx';

interface ReviewFormProps {
  restaurantId: string;
  onSuccess?: () => void;
}

export default function ReviewForm({ restaurantId, onSuccess }: ReviewFormProps) {
  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);
  const [comment, setComment] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (rating === 0) return;

    setIsSubmitting(true);
    try {
      await reviewApi.submit({ restaurantId, rating, comment });
      setRating(0);
      setComment('');
      onSuccess?.();
    } catch (error) {
      console.error('Failed to submit review', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="glass-card p-6 space-y-6">
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-indigo-500/10 rounded-xl flex items-center justify-center">
          <Star className="w-5 h-5 text-indigo-500" />
        </div>
        <div>
          <h3 className="font-bold text-lg">Leave a Review</h3>
          <p className="text-xs text-gray-500 flex items-center gap-1">
            <ShieldCheck className="w-3 h-3" /> Anti-fraud protection active
          </p>
        </div>
      </div>

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="flex items-center gap-2">
          {[1, 2, 3, 4, 5].map((star) => (
            <button
              key={star}
              type="button"
              className="focus:outline-none transition-transform hover:scale-125"
              onMouseEnter={() => setHover(star)}
              onMouseLeave={() => setHover(0)}
              onClick={() => setRating(star)}
            >
              <Star
                className={clsx(
                  "w-8 h-8 transition-colors",
                  (hover || rating) >= star ? "text-yellow-500 fill-yellow-500" : "text-gray-700"
                )}
              />
            </button>
          ))}
          <span className="ml-4 text-sm font-medium text-gray-400">
            {rating > 0 ? `${rating} / 5` : 'Select your rating'}
          </span>
        </div>

        <textarea
          value={comment}
          onChange={(e) => setComment(e.target.value)}
          placeholder="Share your experience (optional)..."
          className="w-full h-32 bg-gray-950/50 border border-gray-800 rounded-xl p-4 focus:ring-2 focus:ring-indigo-500/50 outline-none transition-all placeholder:text-gray-700"
        />

        <button
          type="submit"
          disabled={isSubmitting || rating === 0}
          className="w-full btn-primary flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {isSubmitting ? (
            <div className="w-5 h-5 border-2 border-white/20 border-t-white rounded-full animate-spin" />
          ) : (
            <>
              <Send className="w-4 h-4" />
              Submit Review
            </>
          )}
        </button>
      </form>
    </div>
  );
}
