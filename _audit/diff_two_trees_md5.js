// #269 So khop md5 TUNG FILE giua 2 cay DMSCarSv (laptop V20.2023.Release.V2 vs 150 V20.2023.Release).
// Sinh tu bai hoc #268 (C0-quadringentesimustricesimus): mot file khop KHONG chung minh ca cay khop.
const fs = require('fs');
function load(f) {
    const m = new Map();
    for (const l of fs.readFileSync(f, 'utf8').split(/\r?\n/)) {
        const x = /^([0-9a-f]{32})\s+\*?(.+)$/.exec(l.trim());
        if (x) m.set(x[2].split('\\').join('/').toLowerCase(), x[1]);
    }
    return m;
}
const A = load(process.argv[2]), B = load(process.argv[3]);
const onlyA = [], onlyB = [], diff = [];
for (const [k, v] of A) { if (!B.has(k)) onlyA.push(k); else if (B.get(k) !== v) diff.push(k); }
for (const k of B.keys()) if (!A.has(k)) onlyB.push(k);
console.log('laptop=' + A.size + '  150=' + B.size);
console.log('CHI CO LAPTOP: ' + onlyA.length + '   CHI CO 150: ' + onlyB.length + '   LECH NOI DUNG: ' + diff.length);
const noise = /designer|\.resx|assemblyinfo|reference\.cs|\/refs\/|\/properties\//i;
const f = x => x.filter(p => !noise.test(p));
const show = (t, arr, n) => { const g = f(arr); console.log('\n--- ' + t + ': ' + g.length + ' (loc designer/refs)'); g.slice(0, n).forEach(p => console.log('  ' + p)); if (g.length > n) console.log('  … con ' + (g.length - n)); };
show('LECH NOI DUNG', diff, 60);
show('CHI CO 150', onlyB, 40);
show('CHI CO LAPTOP', onlyA, 40);
