import { render, screen } from '@testing-library/react';
import { Card, CardHeader, CardContent, CardFooter } from '../Card';
import { describe, it, expect } from 'vitest';

describe('Card', () => {
  it('renders correctly with children', () => {
    render(
      <Card>
        <CardHeader>Header</CardHeader>
        <CardContent>Content</CardContent>
        <CardFooter>Footer</CardFooter>
      </Card>
    );
    expect(screen.getByText('Header')).toBeInTheDocument();
    expect(screen.getByText('Content')).toBeInTheDocument();
    expect(screen.getByText('Footer')).toBeInTheDocument();
  });

  it('applies custom className', () => {
    const { container } = render(<Card className="custom-class">Content</Card>);
    expect(container.firstChild).toHaveClass('custom-class');
    expect(container.firstChild).toHaveClass('bg-surface');
  });

  it('renders all subcomponents with correct padding classes', () => {
    render(
      <Card>
        <CardHeader data-testid="header">Header</CardHeader>
        <CardContent data-testid="content">Content</CardContent>
        <CardFooter data-testid="footer">Footer</CardFooter>
      </Card>
    );
    
    expect(screen.getByTestId('header')).toHaveClass('p-6');
    expect(screen.getByTestId('content')).toHaveClass('p-6 pt-0');
    expect(screen.getByTestId('footer')).toHaveClass('p-6 pt-0');
  });
});
