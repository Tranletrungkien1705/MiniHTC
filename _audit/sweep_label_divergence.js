// #289 SWEEP: gom MOI khoi CASE nhan trong SQL nguon, nhom theo COT duoc switch,
// bao cot nao co NHIEU BO NHAN khac nhau (=> nhan THEO MAN, luat #286/#288).
// Bien 993 hit roi rac cua hang doi #285 thanh danh sach co dich.
const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

// mot "khoi nhan" = day cac dong 'when ... then N'...'' lien tiep
const blocks = [];   // {col, file, line, pairs:[[code,label]]}
for (const f of process.argv.slice(2)) {
    const lines = readText(f).split(/\r?\n/);
    let cur = null;
    for (let i = 0; i < lines.length; i++) {
        const l = lines[i];
        // 'case <col>' hoac 'case' tran
        const mCase = /\bcase\s+([A-Za-z0-9_]+\.[A-Za-z0-9_]+|[A-Za-z0-9_]+)?\s*$/i.exec(l.trim());
        if (mCase) { cur = { col: (mCase[1] || '?').toLowerCase(), file: path.basename(f), line: i + 1, pairs: [] }; continue; }
        // 'when <x> = 'v'' hoac 'when 'v''  +  then N'...'
        const mWhen = /\bwhen\s+(?:([A-Za-z0-9_]+\.[A-Za-z0-9_]+)\s*(?:=|is)\s*)?'?([A-Za-z0-9_]*)'?\s*(?:is\s+null)?\s*then\s+N'([^']*)'/i.exec(l);
        if (mWhen && cur) {
            if (mWhen[1]) cur.col = mWhen[1].toLowerCase();
            const isNull = /is\s+null/i.test(l);
            cur.pairs.push([isNull ? '(null)' : mWhen[2], mWhen[3]]);
            continue;
        }
        if (cur && cur.pairs.length && /\bend\b/i.test(l)) { blocks.push(cur); cur = null; }
    }
}

// nhom theo cot
const byCol = new Map();
for (const b of blocks) {
    if (!b.pairs.length) continue;
    const col = b.col.replace(/^[a-z0-9]+\./, '');           // bo alias
    if (!byCol.has(col)) byCol.set(col, []);
    byCol.get(col).push(b);
}

let diverge = 0;
for (const [col, bs] of [...byCol.entries()].sort()) {
    const sigs = new Map();
    for (const b of bs) {
        const sig = b.pairs.map(p => p[0] + '=' + p[1]).sort().join(' | ');
        if (!sigs.has(sig)) sigs.set(sig, []);
        sigs.get(sig).push(b.file + ':' + b.line);
    }
    if (sigs.size > 1) {
        diverge++;
        console.log('\n🔴 COT "' + col + '" co ' + sigs.size + ' BO NHAN khac nhau (' + bs.length + ' khoi):');
        let k = 0;
        for (const [sig, where] of sigs) {
            console.log('   [' + (++k) + '] ' + where.slice(0, 3).join(', ') + (where.length > 3 ? ' (+' + (where.length - 3) + ')' : ''));
            console.log('       ' + sig.slice(0, 200));
        }
    }
}
console.log('\nTONG: ' + blocks.length + ' khoi nhan, ' + byCol.size + ' cot, ' + diverge + ' cot CO PHAN KY');
