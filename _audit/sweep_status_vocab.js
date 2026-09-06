// Sweep #238b: LOC ra guard chet cam THAT SU.
// Nguyen tac: cot trang thai co DEFAULT la MA NGAN (<=2 ky tu) trong Entities.cs
// ma code lai so sanh voi LITERAL DAI => tu vung da doi, guard con sot => CHET CAM.
const fs = require('fs');
const B = 'd:/idocNet/_labs/MiniHTC/';
// argv[2] = duong dan Program.cs can quet (de self-test tren ban cu)
const PROG_PATH = process.argv[2] || (B + 'Program.cs');
const prog = fs.readFileSync(PROG_PATH, 'utf8').split(/\r?\n/);
const ents = fs.readFileSync(B + 'Models/Entities.cs', 'utf8');

// 1) Thu thap default cua moi thuoc tinh *Status* trong Entities.cs
//    vd:  public string OrderPartStatus { get; set; } = "P";
const defs = new Map();          // ten cot -> tap gia tri default
const RE_DEF = /public\s+string\??\s+([A-Za-z_][A-Za-z0-9_]*)\s*\{\s*get;\s*set;\s*\}\s*=\s*"([^"]*)"/g;
let m;
while ((m = RE_DEF.exec(ents)) !== null) {
    const col = m[1], val = m[2];
    if (!/Status|Stage/.test(col)) continue;
    if (!defs.has(col)) defs.set(col, new Set());
    defs.get(col).add(val);
}

// 2) Quet so sanh trong Program.cs
const CMP = /([A-Za-z_][A-Za-z0-9_]*)\s*(==|!=)\s*"([A-Za-z][A-Za-z0-9_]*)"/g;
const cmp = new Map();           // ten cot -> [{ln,op,val}]
prog.forEach((line, i) => {
    const tr = line.trim();
    if (tr.startsWith('//') || tr.startsWith('///')) return;
    CMP.lastIndex = 0;
    let x;
    while ((x = CMP.exec(line)) !== null) {
        const col = x[1], op = x[2], val = x[3];
        if (!/Status|Stage/.test(col)) continue;
        if (!cmp.has(col)) cmp.set(col, []);
        cmp.get(col).push({ ln: i + 1, op, val, text: tr.slice(0, 130) });
    }
});

console.log('Cot *Status* co default trong Entities.cs:', defs.size);
console.log('Cot *Status* duoc so sanh trong Program.cs:', cmp.size);
console.log('');

// ma ngan kieu "P"/"A"/"F"/"15"/"31". Chuoi RONG khong tinh la ma ngan
// (default "" = "chua co trang thai", khong noi len tu vung) -> tranh false positive VerifyStatus.
const SHORT = v => v.length > 0 && v.length <= 2;
let flagged = 0;

for (const [col, list] of cmp) {
    const d = defs.get(col);
    if (!d) continue;                              // khong biet default => bo qua (bao rieng)
    const defShort = [...d].every(SHORT);
    if (!defShort) continue;                       // entity von dung tu vung dai => hop le
    const bad = list.filter(h => !SHORT(h.val));
    if (bad.length === 0) continue;
    flagged++;
    console.log(`🔴 ${col}  default=[${[...d].join('|')}] (MA NGAN) nhung co ${bad.length} so sanh LITERAL DAI:`);
    for (const h of bad) console.log(`      ${h.ln}: [${h.op}] "${h.val}"   ${h.text}`);
    console.log('');
}
if (flagged === 0) console.log('=> KHONG con cot nao vua co default ma ngan vua so sanh literal dai.');

// 3) Bao cac cot duoc so sanh nhung KHONG tim thay default (khong ket luan duoc)
const noDef = [...cmp.keys()].filter(c => !defs.has(c));
console.log('');
console.log('Cot so sanh nhung KHONG co default trong Entities.cs (phai audit tay):', noDef.length);
console.log('  ' + noDef.join(', '));
