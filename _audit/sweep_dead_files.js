// #267 SWEEP DEAD FILE: .cs nam tren dia nhung KHONG co trong .csproj => DEAD, cam doc lam nguon su that.
// Sinh ra tu bai hoc #266 (C0-quadringentesimusvicesimussextus).
const fs = require('fs'), path = require('path');

const PROJ = process.argv.slice(2);
if (!PROJ.length) { console.log('usage: node sweep267.js <csproj> [...]'); process.exit(1); }

function readText(p) {
    const b = fs.readFileSync(p);
    // .csproj co the la UTF-8 hoac UTF-16LE (bay E1)
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}
function walk(dir, out) {
    for (const e of fs.readdirSync(dir, { withFileTypes: true })) {
        if (e.isDirectory()) {
            if (['bin', 'obj', '.svn', '.git'].includes(e.name)) continue;
            walk(path.join(dir, e.name), out);
        } else if (e.name.toLowerCase().endsWith('.cs')) out.push(path.join(dir, e.name));
    }
    return out;
}

let totalDead = 0, totalOk = 0;
for (const proj of PROJ) {
    const root = path.dirname(proj);
    const txt = readText(proj);
    // <Compile Include="Views\Services\FrmInvoice.cs" />
    const inc = new Set();
    const re = /<Compile\s+Include\s*=\s*"([^"]+\.cs)"/gi;
    let m; while ((m = re.exec(txt))) inc.add(m[1].split('\\').join('/').toLowerCase());

    const disk = walk(root, []);
    const dead = [];
    for (const f of disk) {
        const rel = path.relative(root, f).split('\\').join('/').toLowerCase();
        if (inc.has(rel)) totalOk++; else dead.push(rel);
    }
    if (dead.length) {
        console.log('\n=== ' + path.basename(proj) + '  (' + inc.size + ' compile, ' + dead.length + ' DEAD) ===');
        for (const d of dead.sort()) console.log('  DEAD  ' + d);
        totalDead += dead.length;
    } else {
        console.log('\n=== ' + path.basename(proj) + '  (' + inc.size + ' compile) — sach');
    }
}
console.log('\nTONG: ' + totalOk + ' file song, ' + totalDead + ' file CHET');
