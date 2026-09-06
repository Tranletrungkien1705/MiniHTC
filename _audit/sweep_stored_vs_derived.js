// #313 SWEEP "LUU hay DAN XUAT": tim cot MiniHTC luu lam COT THAT nhung nguon chi TINH luc doc.
// Sinh tu su co #312 (C0-quingentesimusquinquagesimus): TotalQuantityIn / TotalQuantityInExchangeRate
// duoc them lam cot LUU + cho ghi qua DTO, trong khi nguon SUM tu phieu nhap moi lan doc
// => client gui gi cung thanh "da nhap bay nhieu". Build xanh, §12 du dau vet, NGU NGHIA SAI.
//
// Cach lam:
//   1) Quet NGUON: moi cot duoc GHI that su -> `Rows[0]["X"]` / `newRow["X"]` / lstMapFN "X"
//      => tap W (writable).
//   2) Quet NGUON: moi cot xuat hien nhu ALIAS ket qua (`) X` / `end X` / `as X`) hoac trong
//      `into #tbl...` => tap D (derived-looking).
//   3) Quet MiniHTC Entities.cs: moi property.
//   4) Bao cot nao: co trong D, KHONG co trong W, va MiniHTC dang LUU => nghi la dan xuat.
//
// Ket qua chi la GOI Y (luat C0-...vicesimussecundus: sweep neu vi tri, khong neu tham quyen):
// phai mo ham Create/Update cua nguon xac nhan truoc khi sua.
const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

const args = process.argv.slice(2);
const entFile = args.find(a => /Entities\.cs$/i.test(a));
const srcFiles = args.filter(a => a !== entFile);
if (!entFile) { console.error('Thieu duong dan Models/Entities.cs'); process.exit(1); }

const W = new Set();   // cot NGUON co ghi
const D = new Map();   // ten dan xuat -> 'file:line' dau tien (de kiem tay)
for (const f of srcFiles) {
    const raw = readText(f).split(/\r?\n/);
    for (const line0 of raw) {
        if (line0.trim().startsWith('//')) continue;          // C# comment (bai hoc #308)
        const line = line0.replace(/--.*$/, '');              // SQL comment (bai hoc #305)

        // (1) GHI: Rows[0]["X"] = ... | newRow["X"] = ... | strFN = "X"
        let m;
        const reW = /(?:Rows\s*\[\s*0\s*\]|newRow)\s*\[\s*"([A-Za-z0-9_]+)"\s*\]\s*=/g;
        while ((m = reW.exec(line))) W.add(m[1].toLowerCase());
        const reMap = /strFN\s*=\s*"([A-Za-z0-9_]+)"/g;
        while ((m = reMap.exec(line))) W.add(m[1].toLowerCase());
        const reEff = /alColumnEffective\.Add\("([A-Za-z0-9_]+)"\)|alEffectiveColumn\.Add\("([A-Za-z0-9_]+)"\)/g;
        while ((m = reEff.exec(line))) W.add((m[1] || m[2]).toLowerCase());

        // (2) DAN XUAT: ') X' sau subquery, 'end X'/'end as X' sau case, 'sum(...) X'
        const reD1 = /\)\s+([A-Za-z][A-Za-z0-9_]{2,})\s*$/;             // ) TotalQuantityIn
        const reD2 = /\bend\s+(?:as\s+)?([A-Za-z][A-Za-z0-9_]{2,})\s*$/i;
        const reD3 = /\bsum\s*\([^)]*\)\s+(?:as\s+)?([A-Za-z][A-Za-z0-9_]{2,})/i;
        for (const re of [reD1, reD2, reD3]) {
            const r = re.exec(line.trim());
            if (r) { const k = r[1].toLowerCase(); if (!D.has(k)) D.set(k, path.basename(f) + ':' + (raw.indexOf(line0) + 1)); }
        }
    }
}

// (3) property cua MiniHTC, kem ten class dang chua
const ent = readText(entFile).split(/\r?\n/);
const rows = [];
let cls = '';
for (const l of ent) {
    const mc = /^public sealed class ([A-Za-z0-9_]+)/.exec(l);
    if (mc) { cls = mc[1]; continue; }
    const mp = /^\s*public\s+[A-Za-z0-9_<>?\[\]]+\s+([A-Za-z0-9_]+)\s*\{\s*get;\s*set;/.exec(l);
    if (mp) rows.push({ cls, prop: mp[1] });
}

// #313b: ten CHUNG CHUNG bi loai — chung la alias o mot truy van nao do nhung van la cot LUU that o bang
//   khac. Sweep doi chieu theo TEN TRAN (khong theo BANG) nen khong tu phan biet duoc; day la gioi han
//   da biet, khong phai bo sot. Chi giu ten DAC TRUNG (>= 10 ky tu, khong nam trong stoplist).
const generic = new Set(['totalamount', 'amount', 'unitprice', 'itemcode', 'phoneno', 'customercode',
    'transactiondate', 'rostatus', 'qtyremain', 'quantity', 'price', 'status', 'total', 'vat']);
const skip = new Set(['id', 'orgid', 'createdat', 'updatedat']);
const hits = [];
for (const r of rows) {
    const k = r.prop.toLowerCase();
    if (skip.has(k)) continue;
    if (generic.has(k)) continue;              // #313b: ten chung chung, khong du tin hieu
    if (r.prop.length < 10) continue;          // #313b: ten ngan de trung nghia giua cac bang
    if (!D.has(k)) continue;        // khong trong nhu dan xuat -> bo qua
    if (W.has(k)) continue;         // nguon CO ghi -> la cot LUU that
    hits.push(r);
}

const byCls = new Map();
for (const h of hits) {
    if (!byCls.has(h.cls)) byCls.set(h.cls, []);
    byCls.get(h.cls).push(h.prop);
}
for (const [c, ps] of [...byCls.entries()].sort((a, b) => b[1].length - a[1].length)) {
    console.log('🟠 ' + c + ':\n     ' + ps.map(p => p + '   [dan xuat tai ' + (D.get(p.toLowerCase()) || '?') + ']').join('\n     '));
}
console.log('\nTONG: nguon co ' + W.size + ' cot GHI, ' + D.size + ' ten trong nhu DAN XUAT; '
    + hits.length + ' property MiniHTC nghi la DAN XUAT (tren ' + byCls.size + ' entity).');
console.log('⚠️  Day chi la GOI Y — phai mo ham Create/Update cua nguon xac nhan truoc khi sua.');
