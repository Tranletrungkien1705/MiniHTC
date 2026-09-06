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
        // #300b: cho phep MA AM (vd "when -1 then N'That bai'"). Regex cu nuot dau tru nen chu ky
        //   khoi SendMail.cs:5497 mat cap -1 => hai khoi khac nghia bi coi la gan giong nhau.
        // 'when <x> = 'v'' hoac 'when 'v''  +  then N'...'
        const mWhen = /\bwhen\s+(?:([A-Za-z0-9_]+\.[A-Za-z0-9_]+)\s*(?:=|is)\s*)?'?(-?[A-Za-z0-9_]*)'?\s*(?:is\s+null)?\s*then\s+N'([^']*)'/i.exec(l);
        if (mWhen && cur) {
            if (mWhen[1]) cur.col = mWhen[1].toLowerCase();
            const isNull = /is\s+null/i.test(l);
            cur.pairs.push([isNull ? '(null)' : mWhen[2], mWhen[3]]);
            continue;
        }
        // #300 (bài học C0-quingentesimusquintusdecimus): PHẢI bắt nhánh ELSE.
        //   Có ELSE  = BLACKLIST một phía -> NULL/mã lạ hiển thị NHƯ dữ liệu bình thường (nguy hiểm).
        //   Không ELSE = WHITELIST        -> mã lạ cho ra NULL, người dùng thấy ô trống (an toàn hơn).
        //   Chính sweep này ở #289 đã BỎ SÓT nhánh đó: khối Customer.cs:2213 hiện ra như chỉ có 1 nhãn,
        //   thực tế còn "else N'Không hoạt động'" -> kết luận phân kỳ bị hiểu sai một nửa.
        const mElse = /\belse\s+N?'([^']*)'/i.exec(l);
        if (cur && mElse && !/\bwhen\b/i.test(l)) { cur.els = mElse[1]; }
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
        // #300: ELSE là MỘT PHẦN của chữ ký — cùng bộ when mà khác else = KHÁC NGHĨA.
        const sigE = sig + (b.els ? " | else=" + b.els : " | (KHONG else)");
        if (!sigs.has(sigE)) sigs.set(sigE, []);
        sigs.get(sigE).push(b.file + ':' + b.line);
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
