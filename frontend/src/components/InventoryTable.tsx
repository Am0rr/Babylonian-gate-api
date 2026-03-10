import { AlertCircle, Wrench, ShieldCheck, Crosshair, MoreHorizontal } from 'lucide-react';

export const WeaponStatus = {
    InStorage: 0,
    Deployed: 1,
    Maintenance: 3,
    Missing: 4
} as const;

export type WeaponStatus = typeof WeaponStatus[keyof typeof WeaponStatus];

export const WeaponType = {
    Rifle: 0,
    Pistol: 1,
    Sniper: 2,
    Launcher: 3
} as const;

export type WeaponType = typeof WeaponType[keyof typeof WeaponType];

interface Weapon {
    id: string;
    codeName: string;
    serialNumber: string;
    type: WeaponType;
    status: WeaponStatus;
    condition: number;
    caliber: string;
}

const MOCK_DATA: Weapon[] = [
    { id: '1', serialNumber: 'BG-773-A', codeName: 'M4A1 Carbine', type: WeaponType.Rifle, status: WeaponStatus.InStorage, condition: 98, caliber: '5.56x45mm' },
    { id: '2', serialNumber: 'BG-774-B', codeName: 'M249 SAW', type: WeaponType.Rifle, status: WeaponStatus.Deployed, condition: 85, caliber: '5.56x45mm' },
    { id: '3', serialNumber: 'BG-812-C', codeName: 'Glock 19', type: WeaponType.Pistol, status: WeaponStatus.InStorage, condition: 100, caliber: '9x19mm' },
    { id: '4', serialNumber: 'BG-404-X', codeName: 'M2010 ESR', type: WeaponType.Sniper, status: WeaponStatus.Maintenance, condition: 45, caliber: '.300 Win Mag' },
    { id: '5', serialNumber: 'BG-999-Z', codeName: 'FGM-148 Javelin', type: WeaponType.Launcher, status: WeaponStatus.Missing, condition: 0, caliber: '127mm' },
    { id: '6', serialNumber: 'BG-773-A', codeName: 'M4A1 Carbine', type: WeaponType.Rifle, status: WeaponStatus.InStorage, condition: 98, caliber: '5.56x45mm' },
    { id: '7', serialNumber: 'BG-774-B', codeName: 'M249 SAW', type: WeaponType.Rifle, status: WeaponStatus.Deployed, condition: 85, caliber: '5.56x45mm' },
    { id: '8', serialNumber: 'BG-812-C', codeName: 'Glock 19', type: WeaponType.Pistol, status: WeaponStatus.InStorage, condition: 100, caliber: '9x19mm' },
    { id: '9', serialNumber: 'BG-404-X', codeName: 'M2010 ESR', type: WeaponType.Sniper, status: WeaponStatus.Maintenance, condition: 45, caliber: '.300 Win Mag' },
    { id: '10', serialNumber: 'BG-999-Z', codeName: 'FGM-148 Javelin', type: WeaponType.Launcher, status: WeaponStatus.Missing, condition: 0, caliber: '127mm' },
    { id: '11', serialNumber: 'BG-812-C', codeName: 'Glock 19', type: WeaponType.Pistol, status: WeaponStatus.InStorage, condition: 100, caliber: '9x19mm' },
    { id: '12', serialNumber: 'BG-404-X', codeName: 'M2010 ESR', type: WeaponType.Sniper, status: WeaponStatus.Maintenance, condition: 45, caliber: '.300 Win Mag' },
    { id: '13', serialNumber: 'BG-999-Z', codeName: 'FGM-148 Javelin', type: WeaponType.Launcher, status: WeaponStatus.Missing, condition: 0, caliber: '127mm' },
    { id: '14', serialNumber: 'BG-812-C', codeName: 'Glock 19', type: WeaponType.Pistol, status: WeaponStatus.InStorage, condition: 100, caliber: '9x19mm' },
    { id: '15', serialNumber: 'BG-404-X', codeName: 'M2010 ESR', type: WeaponType.Sniper, status: WeaponStatus.Maintenance, condition: 45, caliber: '.300 Win Mag' },
    { id: '16', serialNumber: 'BG-999-Z', codeName: 'FGM-148 Javelin', type: WeaponType.Launcher, status: WeaponStatus.Missing, condition: 0, caliber: '127mm' },
];

const getTypeLabel = (type: WeaponType) => {
    switch(type) {
        case WeaponType.Rifle: return 'Rifle';
        case WeaponType.Pistol: return 'Pistol';
        case WeaponType.Sniper: return 'Sniper';
        case WeaponType.Launcher: return 'Launcher';
        default: return 'UNKNOWN';
    }
}

const getStatusConfig = (status: WeaponStatus) => {
    switch(status) {
        case WeaponStatus.InStorage: 
            return {label: 'IN STORAGE', color: 'text-emerald-400', bg: 'bg-emerald-500/10', border: 'border-emerald-500/20', icon: ShieldCheck};
        case WeaponStatus.Deployed: 
            return {label: 'DEPLOYED', color: 'text-amber-400', bg: 'bg-amber-500/10', border: 'border-amber-500/20', icon: Crosshair};
        case WeaponStatus.Maintenance: 
            return {label: 'REPAIRING', color: 'text-blue-400', bg: 'bg-blue-500/10', border: 'border-blue-500/20', icon: Wrench};
        case WeaponStatus.Missing: 
            return {label: 'MISSING', color: 'text-rose-400', bg: 'bg-rose-500/10', border: 'border-rose-500/20', icon: AlertCircle};
        default: 
            return { label: 'UNKNOWN', color: 'text-gray-400', bg: 'bg-gray-500/10', border: 'border-gray-500/20', icon: AlertCircle };;
    }
};

export const InventoryTable = () => {
    return (
        <div className="w-full h-full bg-[#050505] border border-white/10 rounded-lg flex flex-col overflow-hidden">
            <div className="flex-1 overflow-y-auto custom-scrollbar">
                <table className="w-full text-left border-collapse">
                    <thead className="bg-[#050505] shadow-md">
                    <tr className="border-b border-white/20 bg-white/[0.02] text-[16px] text-white uppercase tracking-widest font-mono">
                            <th className="px-6 py-5 font-semibold border-r border-white/10 whitespace-nowrap">Serial Number</th>
                            <th className="px-6 py-5 font-semibold border-r border-white/10 whitespace-nowrap">Code Name</th>
                            <th className="px-6 py-5 font-semibold border-r border-white/10 whitespace-nowrap">Caliber</th>
                            <th className="px-6 py-5 font-semibold border-r border-white/10 whitespace-nowrap">Category</th>
                            <th className="px-6 py-5 font-semibold border-r border-white/10 w-px whitespace-nowrap">Condition</th>
                            <th className="px-6 py-5 font-semibold border-r border-white/10 whitespace-nowrap text-center">Status</th>
                            <th className="px-6 py-5 font-semibold w-px whitespace-nowrap text-center">Action</th>
                        </tr>
                    </thead>

                    <tbody>
                        {MOCK_DATA.map((weapon) => {
                            const statusConfig = getStatusConfig(weapon.status);
                            const StatusIcon = statusConfig.icon;
                            const conditionColor = weapon.condition > 80 ? 'bg-emerald-500' : weapon.condition > 40 ? 'bg-amber-500' : 'bg-rose-500';

                            return (
                                <tr
                                    key={weapon.id}
                                    className="border-b border-white/10 hover:bg-white/[0.03] transition-colors group cursor-default"
                                >
                                    {/* Serial Number */}
                                    <td className="text-[16px] px-6 py-4 text-bold font-mono border-r border-white/10">
                                        {weapon.serialNumber}
                                    </td>

                                    {/* Code Name */}
                                    <td className="text-[16px] px-6 py-4 text-bold font-mono border-r border-white/10">
                                        {weapon.codeName}
                                    </td>

                                    {/* Caliber */}
                                    <td className="text-[16px] px-6 py-4 text-bold font-mono text-white border-r border-white/10">
                                        {weapon.caliber}
                                    </td>

                                    {/* Category */}
                                    <td className="px-6 py-4 border-r border-white/10">
                                        <span className="text-[16px] text-bold font-mono text-white tracking-wide">
                                            {getTypeLabel(weapon.type)}
                                        </span>
                                    </td>

                                    {/* Condition */}
                                    <td className="px-6 py-4 border-r border-white/10">
                                        <div className="flex items-center gap-4">
                                            <div className="w-36 h-2 bg-gray-800 rounded-full overflow-hidden">
                                                <div
                                                    className={`h-full ${conditionColor}`}
                                                    style={{ width: `${weapon.condition}%` }}
                                                ></div>
                                            </div>
                                            <span className="text-bold font-mono text-[16px] text-gray-300 w-10">
                                                {weapon.condition}%
                                            </span>
                                        </div>
                                    </td>

                                    {/* Status */}
                                    <td className="px-1 py-4 border-r border-white/10 whitespace-nowrap text-center">
                                        <div className={`inline-flex items-center gap-2 px-3 py-1.5 rounded border ${statusConfig.bg} ${statusConfig.border} ${statusConfig.color}`}>
                                            <StatusIcon size={16} strokeWidth={2.5} />
                                            <span className="text-[12px] text-bold font-mono tracking-wider uppercase">
                                                {statusConfig.label}
                                            </span>
                                        </div>
                                    </td>

                                    {/* Action */}
                                    <td className="px-6 py-4 w-px whitespace-nowrap text-center">
                                        <button className="p-2 text-gray-500 rounded hover:text-white hover:bg-white/10 active:scale-95 transition-all">
                                            <MoreHorizontal size={20} strokeWidth={2.5} />
                                        </button>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
};