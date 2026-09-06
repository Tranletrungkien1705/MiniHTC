// #302 SWEEP BAN CHUP / DU PHONG: tim moi cot duoc doc bang isnull(<chungtu>.X, <master>.X[, ...]).
// Sinh tu bai hoc C0-quingentesimusvicesimus (#301): chung tu giu BAN CHUP luc phat sinh,
// bang master chi la DU PHONG. Port doc thang cot chung tu => mat du phong;
// port doc thang master => mat ban chup. Ca hai deu sai.
//
// Bao cao theo ALIAS CHUNG TU (ve trai cua isnull), vi do la thuc the co ban chup.
// Danh dau rieng:
//   - chuoi 3 CAP (isnull long nhau)  -> de port sot cap thu 3
//   - cot chi 2 CAP trong khi anh em cung nhom co 3 -> so cap KHONG deu, phai chep dung
const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

// isnull(a.X , isnull(b.Y , c.Z))  hoac  isnull(a.X , b.Y)
const RE3 = /isnull\(\s*([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\s*,\s*isnull\(\s*([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\s*,\s*([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\s*\)\s*\)/gi;
const RE2 = /isnull\(\s*([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\s*,\s*([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)\s*\)/gi;

// #304 (bai hoc C0-quingentesimusvicesimusquintus): loai THANH NGU FULL OUTER JOIN.
//   'select ISNULL(a.ROWID,b.ROWID) ROWID, ISNULL(a.RONo,b.RONo) RONo ...' la cach DUY NHAT lay khoa
//   sau full outer join (ben nao cung co the NULL) — KHONG phai luat ban chup/du phong.
//   Dau hieu: CUNG TEN COT ca hai ve. Sweep #302 da bao nham 'svp' la chung tu giu ban chup.
const sameColName = h => h.chain.split(' -> ')
    .map(x => x.split('.')[1]).every((c, _, a) => c.toLowerCase() === a[0].toLowerCase());

// Ve phai la HANG SO (isnull(t.Price, 0.0)) cung khong phai master.
const RE_CONST = /isnull\(\s*[A-Za-z0-9_]+\.[A-Za-z0-9_]+\s*,\s*[-0-9.]+\s*\)/i;

const hits = [];   // {file, line, doc, col, levels, chain}
for (const f of process.argv.slice(2)) {
    const lines = readText(f).split(/\r?\n/);
    for (let i = 0; i < lines.length; i++) {
        const l = lines[i];
        let m;
        RE3.lastIndex = 0;
        const seen3 = [];
        while ((m = RE3.exec(l))) {
            seen3.push(m[0]);
            hits.push({
                file: path.basename(f), line: i + 1, doc: m[1].toLowerCase(), col: m[2],
                levels: 3, chain: m[1] + '.' + m[2] + ' -> ' + m[3] + '.' + m[4] + ' -> ' + m[5] + '.' + m[6],
            });
        }
        RE2.lastIndex = 0;
        while ((m = RE2.exec(l))) {
            // bo qua neu nam trong mot khop 3-cap da bat o tren
            if (seen3.some(s => s.includes(m[0]))) continue;
            hits.push({
                file: path.basename(f), line: i + 1, doc: m[1].toLowerCase(), col: m[2],
                levels: 2, chain: m[1] + '.' + m[2] + ' -> ' + m[3] + '.' + m[4],
            });
        }
    }
}

// nhom theo alias chung tu
const byDoc = new Map();
for (const h of hits) {
    if (!byDoc.has(h.doc)) byDoc.set(h.doc, []);
    byDoc.get(h.doc).push(h);
}

let flagged = 0;
for (const [doc, hs] of [...byDoc.entries()].sort((a, b) => b[1].length - a[1].length)) {
    // chi quan tam alias co >= 2 cot chup (1 cot le thuong la tien ich, khong phai luat ban chup)
    const cols = new Set(hs.map(h => h.col));
    if (cols.size < 2) continue;
    flagged++;
    // #304: tach hit thanh ngu join khoi hit ban chup THAT
    const idiom = hs.filter(sameColName);
    const real = hs.filter(h => !sameColName(h));
    const realCols = new Set(real.map(h => h.col));
    const files = [...new Set(hs.map(h => h.file + ':' + h.line))].slice(0, 3);
    const l3 = real.filter(h => h.levels === 3), l2 = real.filter(h => h.levels === 2);
    if (realCols.size < 2) {
        console.log('\n⚪ BO QUA alias "' + doc + '": ' + idiom.length + ' hit deu la THANH NGU JOIN'
            + ' (cung ten cot hai ve) — khong phai luat ban chup.');
        console.log('   ' + files.join(', '));
        continue;
    }
    console.log('\n🔴 CHUNG TU alias "' + doc + '" giu BAN CHUP: ' + realCols.size + ' cot THAT / ' + real.length + ' cho doc'
        + (idiom.length ? '   [da loai ' + idiom.length + ' hit thanh ngu join]' : ''));
    console.log('   ' + files.join(', '));
    if (l3.length && l2.length)
        console.log('   ⚠️ SO CAP KHONG DEU: ' + l3.length + ' cho 3-cap, ' + l2.length + ' cho 2-cap'
            + ' -> cot 2-cap de bi port them cap thu 3 cho "deu"');
    const uniq = new Map();
    for (const h of real) if (!uniq.has(h.chain)) uniq.set(h.chain, h.levels);
    for (const [c, lv] of [...uniq.entries()].slice(0, 14)) console.log('     [' + lv + '] ' + c);
    if (uniq.size > 14) console.log('     ... +' + (uniq.size - 14) + ' chuoi nua');
}
// #304 (bai hoc C0-quingentesimusvicesimustertius): du phong TU THAN — hai ve CUNG ALIAS.
//   'isnull(ro.CreatedDate, ro.CheckInDate)' khong phai master fallback: port thanh bieu thuc,
//   DUNG them cot moi.
const selfFb = hits.filter(h => {
    const p = h.chain.split(' -> ').map(x => x.split('.')[0].toLowerCase());
    return p.length === 2 && p[0] === p[1];
});
if (selfFb.length) {
    console.log('\n🟡 DU PHONG TU THAN (hai ve cung alias) — port thanh BIEU THUC, khong them cot:');
    const u = new Set(selfFb.map(h => h.chain));
    for (const c of u) console.log('     ' + c);
}
console.log('\nTONG: ' + hits.length + ' cho doc qua isnull, ' + byDoc.size + ' alias, ' + flagged + ' chung tu CO BAN CHUP');
