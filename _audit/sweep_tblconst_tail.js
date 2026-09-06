// Sweep #261: tim cac lop Tbl* trong DbDefine.cs co HANG NAM SAU DONG TRONG (khoi phu).
// Day la lop bi bo sot o #237 (InStockQuantity/LocationCode/LocationName cua
// TblSer_SupplierPaymentDtl nam tach rieng duoi khoi chinh) -> #260 moi phat hien.
const fs = require('fs');
const SRC = process.argv[2]
  || 'd:/idocNet/2021.1.TCMotor/Dev/DMSCarSv/V20.2023.Release.V2/TERP.HTCServiceClient/Common/DbDefine.cs';
const lines = fs.readFileSync(SRC, 'utf8').split(/\r?\n/);

const RE_CLASS = /^\s*public\s+class\s+(Tbl[A-Za-z0-9_]*)/;
const RE_CONST = /^\s*public\s+const\s+string\s+([A-Za-z0-9_]+)\s*=/;

let cur = null, out = [];
for (let i = 0; i < lines.length; i++) {
    const l = lines[i];
    const mc = l.match(RE_CLASS);
    if (mc) { cur = { name: mc[1], ln: i + 1, groups: [[]], sawConstInGroup: false }; out.push(cur); continue; }
    if (!cur) continue;
    if (/^\s*\}/.test(l)) { cur = null; continue; }          // dong lop
    const mk = l.match(RE_CONST);
    if (mk) { cur.groups[cur.groups.length - 1].push({ name: mk[1], ln: i + 1 }); cur.sawConstInGroup = true; continue; }
    if (/^\s*$/.test(l) && cur.sawConstInGroup) {            // dong trong SAU khi da co hang -> mo nhom moi
        cur.groups.push([]); cur.sawConstInGroup = false;
    }
}

const risky = out.filter(c => c.groups.filter(g => g.length > 0).length > 1);
console.log('Tong so lop Tbl*:', out.length);
console.log('Lop co HANG SAU DONG TRONG (nguy co doc thieu):', risky.length);
console.log('');
for (const c of risky) {
    const gs = c.groups.filter(g => g.length > 0);
    const tail = gs.slice(1).flat();
    console.log(`🔴 ${c.name}  (dong ${c.ln})  — khoi chinh ${gs[0].length} hang, con ${tail.length} hang o khoi PHU:`);
    console.log('     ' + tail.map(x => `${x.name}(:${x.ln})`).join(' , '));
}
