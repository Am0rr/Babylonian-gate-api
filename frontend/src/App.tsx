import { useState, useEffect } from 'react';
import { Sidebar } from './components/Sidebar';
import { StatsBanner } from './components/StatsBanner';
import { SearchBar } from './components/SearchBar';
import { InventoryTable } from './components/InventoryTable';
import type { DashboardStats } from './types/types';

const initialStats: DashboardStats = {
  storage: { total: 0, readyPercentage: 0 },
  operational: { deployed: 0, repair: 0, missing: 0 },
  ammo: { totalRounds: 0, crates: 0 },
  personnel: { armed: 0, totalStaff: 0 }
};

function App() {
  const [stats, setStats] = useState<DashboardStats>(initialStats);
  const [isLoading, setIsLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    const timer = setTimeout(() => {
      setStats({
        storage: { total: 154, readyPercentage: 84 },
        operational: { deployed: 70, repair: 2, missing: 3 },
        ammo: { totalRounds: 42300, crates: 15 },
        personnel: { armed: 34, totalStaff: 120 }
      });
      setIsLoading(false);
    }, 1000); 
    return () => clearTimeout(timer);
  }, []);

  const handleSearch = (query: string) => {
    setSearchQuery(query);
    console.log('Searching:', query);
  };

  return (
    <div className="flex h-screen bg-[#050505] text-white font-mono overflow-hidden">
      
      <Sidebar />

      
      <div className="flex-1 flex flex-col relative h-full">
        
        <main className="flex-1 flex flex-col p-8 min-h-0">
          <div className="shrink-0">
            <StatsBanner 
              stats={stats} 
              isLoading={isLoading} 
              onFilterClick={(filter) => console.log('Filter:', filter)}
            />
            
            <SearchBar 
              value={searchQuery} 
              onChange={handleSearch} 
            />
          </div>

            <div className="flex-1 min-h-0 pb-4">
                <InventoryTable />
            </div>
        </main>
      </div>
    </div>
  )
}

export default App;