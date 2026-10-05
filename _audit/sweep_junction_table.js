// #295 SWEEP BANG NOI (junction): lop Tbl* co >= 2 cot khoa "<X>ID" tro ve HAI thuc the KHAC NHAU.
// Sinh tu bai hoc #294 (C0-quingentesimusprimus): quan he nhieu-nhieu duoc luu o BANG NOI RIENG,
// khong co khoa ngoai tren hai bang chinh => port de bo sot hoan toan.
const fs = require('fs');
const src = fs.readFileSync(process.argv[2], 'utf8');

// tach cac lop hang: public class Tbl<Name> { ... }
const classes = [];
const re = /public\s+class\s+(Tbl[A-Za-z0-9_]*)\s*\{([\s\S]*?)\n\s*\}/g;
let m;
while ((m = re.exec(src))) {
    const consts = [...m[2].matchAll(/public\s+const\s+string\s+([A-Za-z0-9_]+)\s*=/g)].map(x => x[1]);
    classes.push({ name: m[1], consts });
}

const known = new Set(classes.map(c => c.name.replace(/^Tbl/, '').toLowerCase()));

let found = 0;
for (const c of classes) {
    // cot ket thuc bang ID (bo cot khoa chinh cua chinh no)
    const ids = c.consts.filter(x => /ID$/i.test(x) && x.length > 2);
    if (ids.length < 2) continue;

    // ten thuc the suy tu cot: bo hau to ID
    const ents = [...new Set(ids.map(x => x.replace(/ID$/i, '').toLowerCase()))].filter(e => e.length > 2);
    // loai cot khoa chinh cua chinh lop (trung ten lop)
    const self = c.name.replace(/^Tbl/, '').toLowerCase();
    const others = ents.filter(e => !self.startsWith(e) && !e.startsWith(self));
    if (others.length < 2) continue;

    // dau hieu MANH: co ca cap <X>No di kem
    const nos = c.consts.filter(x => /No$/.test(x));
    const strong = others.filter(e => nos.some(n => n.toLowerCase().startsWith(e)));

    console.log((strong.length >= 2 ? '🔴 MANH ' : '   nghi ') + c.name
        + '  | khoa: ' + others.join(', ')
        + (strong.length >= 2 ? '  | co ca so: ' + strong.join(', ') : ''));
    found++;
}
console.log('\nTONG: ' + classes.length + ' lop Tbl*, ' + found + ' lop nghi la BANG NOI');
