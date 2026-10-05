// #288 SWEEP cap bang TEN DAO CHU / hoan vi tu (bay "suyt gop nham" cua #287).
// Sinh tu bai hoc C0-quadringentesimusseptuagesimusnonus: Ser_Order_Part vs Ser_Part_Order.
const fs = require('fs');
const names = fs.readFileSync(process.argv[2], 'utf8').split(/\r?\n/).map(s => s.trim()).filter(Boolean);

// khoa = tap cac TU (tach theo '_') da sap xep -> hai ten cung khoa = hoan vi cua nhau
const byKey = new Map();
for (const n of names) {
    const parts = n.split('_').filter(Boolean);
    if (parts.length < 2) continue;
    const key = parts.slice().sort().join('|');
    if (!byKey.has(key)) byKey.set(key, []);
    byKey.get(key).push(n);
}
let found = 0;
for (const [key, list] of byKey) {
    const uniq = [...new Set(list)];
    if (uniq.length > 1) { console.log('🔴 HOAN VI: ' + uniq.join('  <->  ')); found++; }
}
console.log('--- cap hoan vi: ' + found);

// them: cap khac nhau DUY NHAT 1 ky tu o cuoi (so it/so nhieu, hau to)
const near = [];
for (let i = 0; i < names.length; i++)
    for (let j = i + 1; j < names.length; j++) {
        const a = names[i], b = names[j];
        if (Math.abs(a.length - b.length) !== 1) continue;
        const [s, l] = a.length < b.length ? [a, b] : [b, a];
        if (l.startsWith(s)) near.push(s + '  <->  ' + l);
    }
console.log('--- cap lech DUNG 1 ky tu cuoi: ' + near.length);
near.slice(0, 20).forEach(x => console.log('  ' + x));
