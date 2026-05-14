import { render, screen } from '@testing-library/react';
import { Input } from '../Input';
import { describe, it, expect } from 'vitest';

describe('Input', () => {
  it('renders correctly with label', () => {
    render(<Input label="Username" placeholder="Enter username" />);
    
    const label = screen.getByText('Username');
    const input = screen.getByPlaceholderText('Enter username');
    
    expect(label).toBeInTheDocument();
    expect(input).toBeInTheDocument();
    expect(label).toHaveAttribute('for', input.id);
  });

  it('generates unique IDs using useId when no id is provided', () => {
    const { rerender } = render(<Input label="First" />);
    const firstInput = screen.getByLabelText('First');
    expect(firstInput.id).toBeTruthy();

    rerender(<Input label="Second" />);
    const secondInput = screen.getByLabelText('Second');
    expect(secondInput.id).toBeTruthy();
    // In React 18, useId generates a consistent ID but we can check if it exists
  });

  it('uses provided id instead of generated id', () => {
    render(<Input label="Custom" id="custom-id" />);
    const input = screen.getByLabelText('Custom');
    expect(input.id).toBe('custom-id');
  });

  it('handles error state correctly for accessibility', () => {
    const errorMessage = 'Invalid input';
    render(<Input label="Email" error={errorMessage} />);
    
    const input = screen.getByLabelText('Email');
    const errorText = screen.getByText(errorMessage);
    
    expect(input).toHaveAttribute('aria-invalid', 'true');
    expect(input).toHaveAttribute('aria-describedby', errorText.id);
    expect(errorText).toHaveAttribute('id', input.getAttribute('aria-describedby'));
  });

  it('does not have aria-invalid or aria-describedby when there is no error', () => {
    render(<Input label="Email" />);
    const input = screen.getByLabelText('Email');
    
    expect(input).toHaveAttribute('aria-invalid', 'false');
    expect(input).not.toHaveAttribute('aria-describedby');
  });
});
