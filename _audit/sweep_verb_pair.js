// #291 SWEEP: tim cac MAN co CAP DONG TU GAN NGHIA cung ton tai o WS (Cancel/Delete, Reject/Remove...).
// Sinh tu bai hoc #290 (C0-quadringentesimusoctogesimusnonus): co ca hai => LAM HAI VIEC KHAC NHAU.
const fs = require('fs');
const names = fs.readFileSync(process.argv[2], 'utf8').split(/\r?\n/).map(s => s.trim()).filter(Boolean);

// cac nhom dong tu gan nghia de bi gop nham khi port
const GROUPS = [
    ['Cancel', 'Delete'],
    ['Cancel', 'Remove'],
    ['Reject', 'Delete'],
    ['Reject', 'Cancel'],
    ['Close', 'Delete'],
    ['Close', 'Finish'],
    ['Inactive', 'Delete'],
    ['UnApprove', 'Reject'],
    ['Approve', 'Confirm'],
];

// tach ten ham thanh <goc><dongtu><hau to ban>  — bo hau to _New2023.../_Old/_WH de gom ve cung MAN
function stem(n) {
    return n.replace(/_(New|Old)\d*[A-Za-z0-9_]*$/i, '')
            .replace(/_WH$/i, '')
            .replace(/\d+$/, '');
}

const byStem = new Map();
for (const n of names) {
    const s = stem(n);
    if (!byStem.has(s)) byStem.set(s, []);
    byStem.get(s).push(n);
}

// gom theo GOC (bo phan dong tu cuoi) de tim cap
const byRoot = new Map();
for (const s of byStem.keys()) {
    for (const g of GROUPS) for (const verb of g) {
        const re = new RegExp('_?' + verb + '$', 'i');
        if (re.test(s)) {
            const root = s.replace(re, '');
            const key = root.toLowerCase();
            if (!byRoot.has(key)) byRoot.set(key, new Map());
            byRoot.get(key).set(verb.toLowerCase(), s);
        }
    }
}

let found = 0;
for (const g of GROUPS) {
    const [a, b] = g.map(v => v.toLowerCase());
    for (const [root, verbs] of byRoot) {
        if (verbs.has(a) && verbs.has(b)) {
            console.log('🔴 ' + root + '  ->  ' + verbs.get(a) + '  +  ' + verbs.get(b));
            found++;
        }
    }
}
console.log('\nTONG: ' + names.length + ' ham WS song, ' + byRoot.size + ' goc co dong tu, ' + found + ' CAP gan nghia');
