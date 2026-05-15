import { Button } from "../../components/ui/Button";
import { Input } from "../../components/ui/Input";
import { Card, CardHeader, CardContent, CardFooter } from "../../components/ui/Card";
import { Search, MapPin, Star, Zap } from "lucide-react";

export default function DemoPage() {
  return (
    <div className="space-y-12">
      {/* Header Section */}
      <header className="max-w-4xl mx-auto text-center space-y-4">
        <h1 className="text-4xl font-bold bg-amethyst-gradient bg-clip-text text-transparent">
          UGenix Elegant Amethyst
        </h1>
        <p className="text-text-secondary">
          Demonstrating optimized UI components synchronized with design.md
        </p>
      </header>

      <main className="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-8">
        {/* Buttons Showcase */}
        <section className="space-y-6">
          <h2 className="text-xl font-semibold border-b border-slate-800 pb-2">Buttons</h2>
          <div className="flex flex-wrap gap-4">
            <Button variant="primary">Primary Action</Button>
            <Button variant="secondary">Secondary</Button>
            <Button variant="outline">Outline</Button>
            <Button variant="ghost">Ghost Action</Button>
            <Button variant="primary" isLoading>Loading</Button>
          </div>
          <div className="flex flex-wrap gap-4">
            <Button size="sm">Small</Button>
            <Button size="md">Medium</Button>
            <Button size="lg">Large</Button>
          </div>
        </section>

        {/* Form Showcase */}
        <section className="space-y-6">
          <h2 className="text-xl font-semibold border-b border-slate-800 pb-2">Inputs</h2>
          <div className="space-y-4">
            <Input 
              label="Location" 
              placeholder="Enter your city..." 
              defaultValue="Ho Chi Minh City"
            />
            <Input 
              label="Search Vouchers" 
              placeholder="Pizza, Spa, Cinema..." 
              error="Please enter a search term"
            />
          </div>
        </section>

        {/* Card Showcase */}
        <section className="md:col-span-2 space-y-6">
          <h2 className="text-xl font-semibold border-b border-slate-800 pb-2">Cards & Elevation</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {[1, 2, 3].map((i) => (
              <Card key={i}>
                <div className="aspect-video bg-slate-800 rounded-t-card flex items-center justify-center relative overflow-hidden">
                  <div className="absolute top-3 right-3 bg-violet-500/20 text-violet-400 text-xs font-bold px-2 py-1 rounded-full backdrop-blur-md border border-violet-500/30 flex items-center gap-1">
                    <MapPin className="w-3 h-3" /> 1.2 km
                  </div>
                  <Zap className="w-12 h-12 text-violet-500/20" />
                </div>
                <CardHeader>
                  <div className="flex justify-between items-start">
                    <h3 className="font-bold text-lg">Amethyst Lounge #{i}</h3>
                    <div className="flex items-center text-yellow-500 gap-1 text-sm">
                      <Star className="w-4 h-4 fill-current" /> 4.9
                    </div>
                  </div>
                </CardHeader>
                <CardContent>
                  <p className="text-sm text-text-secondary line-clamp-2">
                    Premium spatial discovery experience with deep violet accents and high-performance design.
                  </p>
                </CardContent>
                <CardFooter>
                  <Button variant="primary" className="w-full">View Details</Button>
                </CardFooter>
              </Card>
            ))}
          </div>
        </section>
      </main>
    </div>
  );
}
