// #320 SWEEP "TWIN GHI LECH COT": voi moi cum ham cung goc (X, X_New20230417, X_WH, ...),
// so TAP COT DUOC GHI cua tung ban va bao cum nao co ban ghi NHIEU cot hon han cac ban khac.
//
// Sinh tu bai hoc C0-quingentesimussexagesimustertius (#319): so tap cot `["X"] =` giua hai ban cua
// CUNG mot ham la cach nhanh nhat de thay "ban nao ghi thieu cot nao" — nhanh hon diff vi bo qua
// khac biet dinh dang/log/thu tu.
//
// Vi sao quan trong: neu port doc nham ban (twin CHET), MiniHTC se thieu dung nhung cot ma ban LIVE
// moi ghi. Build van xanh, §12 van du dau vet, nhung DU LIEU THIEU.
//
// Ket qua chi la GOI Y: phai trace WS xem ban nao LIVE (luat C0-...vicesimussecundus) roi moi ket luan.
const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

// Ten goc: bo hau to _New<8 so>, _WH, _Old, _xxx, _ForTab, _Dealer...
function rootName(n) {
    return n.replace(/_New\d{6,8}[A-Za-z]*$/i, '')
            .replace(/_(WH|Old|xxx|Dealer|ForTab|ForWeb)$/i, '')
            .replace(/_New\d{6,8}$/i, '');
}

const funcs = [];   // {file, name, start, end}
for (const f of process.argv.slice(2)) {
    const raw = readText(f).split(/\r?\n/);
    const marks = [];
    for (let i = 0; i < raw.length; i++) {
        if (raw[i].trim().startsWith('//')) continue;
        const m = /^\s*(?:public|private|protected)\s+[A-Za-z0-9_<>.\[\]]+\s+([A-Za-z0-9_]+)\s*\(/.exec(raw[i]);
        if (m) marks.push({ name: m[1], line: i });
    }
    for (let k = 0; k < marks.length; k++) {
        const start = marks[k].line;
        const end = k + 1 < marks.length ? marks[k + 1].line : raw.length;
        const cols = new Set();
        for (let i = start; i < end; i++) {
            const l0 = raw[i];
            if (l0.trim().startsWith('//')) continue;          // C# comment (#308)
            const l = l0.replace(/--.*$/, '');                 // SQL comment (#305)
            let m;
            // moi dang GHI da biet (bai hoc C0-...quinquagesimusquintus, #314)
            const re = /[A-Za-z_][A-Za-z0-9_.\[\]]*\[\s*"([A-Za-z0-9_]+)"\s*\]\s*=(?!=)/g;
            while ((m = re.exec(l))) cols.add(m[1].toLowerCase());
            const re2 = /strFN\s*=\s*"([A-Za-z0-9_]+)"/g;
            while ((m = re2.exec(l))) cols.add(m[1].toLowerCase());
            const re3 = /al(?:Column|)Effective(?:Column|)\.Add\("([A-Za-z0-9_]+)"\)/g;
            while ((m = re3.exec(l))) cols.add(m[1].toLowerCase());
        }
        // #324 (bai hoc C0-quingentesimusseptuagesimusprimus): ham UY QUYEN ghi cho HELPER bi dem hut.
        //   Vi du: Ser_App_Update_New20201230 chi ghi 5 cot bang con, con 16 cot HEADER do helper
        //   Function_UtilsSerApp ghi (tra ve qua `out dt_...` + `out alEffectiveColumn`).
        //   Sweep khong lan duoc qua helper => danh dau de nguoi doc biet SO COT LA HUT.
        let delegates = false;
        for (let i = start; i < end; i++) {
            if (raw[i].trim().startsWith("//")) continue;
            if (/\bout\s+(ArrayList\s+)?alEffectiveColumn|\bout\s+dt_[A-Za-z0-9_]+/.test(raw[i])) { delegates = true; break; }
        }
        if (cols.size > 0) funcs.push({ file: path.basename(f), name: marks[k].name, line: start + 1, cols, delegates });
    }
}

// nhom theo ten goc
const byRoot = new Map();
for (const fn of funcs) {
    const r = rootName(fn.name);
    if (!byRoot.has(r)) byRoot.set(r, []);
    byRoot.get(r).push(fn);
}

let flagged = 0;
for (const [root, list] of [...byRoot.entries()].sort()) {
    if (list.length < 2) continue;                 // khong co twin
    const max = Math.max(...list.map(x => x.cols.size));
    const min = Math.min(...list.map(x => x.cols.size));
    if (max - min < 3) continue;                   // lech nho, bo qua
    flagged++;
    console.log('\n🟠 ' + root + '  (chenh ' + (max - min) + ' cot giua cac ban)');
    for (const fn of list.sort((a, b) => b.cols.size - a.cols.size)) {
        const missing = [...list.find(x => x.cols.size === max).cols].filter(c => !fn.cols.has(c));
        console.log('     ' + String(fn.cols.size).padStart(3) + ' cot' + (fn.delegates ? '⚠️' : '  ') + ' ' + fn.file + ':' + fn.line + '  ' + fn.name
            + (missing.length && fn.cols.size !== max ? '   [thieu: ' + missing.slice(0, 6).join(', ')
                + (missing.length > 6 ? ' +' + (missing.length - 6) : '') + ']' : ''));
    }
}
console.log('\nTONG: ' + funcs.length + ' ham co ghi cot, ' + byRoot.size + ' cum, ' + flagged + ' cum LECH >= 3 cot.');
console.log('⚠️  GOI Y thoi — phai trace WS xem ban nao LIVE roi moi ket luan (C0-...vicesimussecundus).');
console.log('⚠️  Dau \u26a0\ufe0f sau so cot = ham UY QUYEN ghi cho helper (out dt_/out alEffectiveColumn)');
console.log('    => SO COT DO LA HUT, phai doc cot trong helper (C0-...septuagesimusprimus).');
