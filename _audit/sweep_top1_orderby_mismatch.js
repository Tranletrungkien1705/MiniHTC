// #308 SWEEP "top 1 lech truc": moi 'select top 1 <cot A> ... order by <cot B>' ma A != B.
// Sinh tu bai hoc C0-quingentesimustricesimusseptimus (#307): 'StockInDateLastest' sap theo
// pf.CreatedDate nhung lay pf.StockInDate => KHONG phai max(StockInDate).
//
// Vi sao dang doc: 'top 1' luon phai hoi "theo thu tu NAO?". Neu cot SELECT khac cot ORDER thi
// ten cot ket qua ("Ngay nhap cuoi") de danh lua nguoi port thanh mot phep MAX.
//
// Phan loai:
//   🔴 LECH   : cot select va cot order khac nhau  -> phai doc ky, ghi lai
//   🟡 KHONG ORDER : co 'top 1' ma KHONG co 'order by' -> ket qua KHONG TAT DINH (bat ky dong nao)
//   (bo qua) : select va order cung cot -> dung la "ban ghi lon nhat theo cot do"
const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}
const bare = c => (c || '').split('.').pop().toLowerCase();

let nMismatch = 0, nNoOrder = 0, nOk = 0, nGuard = 0;
for (const f of process.argv.slice(2)) {
    const raw = readText(f).split(/\r?\n/);
    // bo phan sau '--' (bai hoc #305: dong comment la luat DA CHET)
    // #308b: PHAI bo CA HAI kieu comment.
    //   `--` = comment SQL (bai hoc #305).
    //   `//` = khoi SQL bi comment bang C# — 7/9 hit "lech truc" dau tien deu nam trong khoi nhu vay,
    //         tuc luat DA CHET. Chi bo khi dong BAT DAU bang // (tranh cat nham `--//[mylock]`).
    const lines = raw.map(l => (l.trim().startsWith("//") ? "" : l).replace(/--.*$/, ""));

    for (let i = 0; i < lines.length; i++) {
        if (!/\bselect\s+top\s+1\b/i.test(lines[i])) continue;

        // cot duoc SELECT: dong nay sau 'top 1', hoac dong ke tiep neu trong
        let sel = (/\bselect\s+top\s+1\s+(.+)$/i.exec(lines[i]) || [])[1] || '';
        let j = i;
        while (!sel.trim() && j + 1 < lines.length && j - i < 3) { j++; sel = lines[j]; }
        sel = sel.trim().replace(/,.*$/, '');
        const selCol = (/([A-Za-z0-9_]+\.[A-Za-z0-9_]+|[A-Za-z0-9_]+)/.exec(sel) || [])[1];
        if (!selCol) continue;

        // tim 'order by' trong pham vi khoi (toi da 25 dong, dung khi gap ')' ket khoi con)
        let ordCol = null, depth = 0, closed = false;
        for (let k = i; k < Math.min(i + 25, lines.length); k++) {
            // #310c: `order by` rat hay xuong dong — cot nam o DONG KE TIEP:
            //     order by
            //         f.StockInDate desc
            //   Regex cu doi cot CUNG DONG => bao nham 12 subquery trong Inventory.Report.cs la
            //   "khong co order by". Nay: bat `order by` truoc, thieu cot thi lay dong ke tiep.
            if (/\border\s+by\b/i.test(lines[k])) {
                let m = /\border\s+by\s+([A-Za-z0-9_]+\.[A-Za-z0-9_]+|[A-Za-z0-9_]+)/i.exec(lines[k]);
                if (!m) {
                    for (let z = k + 1; z < Math.min(k + 4, lines.length); z++) {
                        const t2 = lines[z].trim();
                        if (!t2) continue;
                        m = /^([A-Za-z0-9_]+\.[A-Za-z0-9_]+|[A-Za-z0-9_]+)/.exec(t2);
                        break;
                    }
                }
                if (m) { ordCol = m[1]; break; }
            }
            const m = null;
            if (m) { ordCol = m[1]; break; }
            depth += (lines[k].match(/\(/g) || []).length - (lines[k].match(/\)/g) || []).length;
            if (k > i && depth < 0) { closed = true; break; }
        }

        // #310 (bai hoc C0-quingentesimusquadragesimussecundus): PHAN LOAI theo NGU CANH DUNG ket qua.
        //   'top 1' khong order by chi NGUY HIEM khi co doc noi dung dong; neu chi hoi 'co dong nao khong'
        //   thi vo hai. #309 do 7/7 hit trong AssignmentOfWork.cs deu la guard => khong phai no.
        //   Doc 40 dong C# sau khoi SQL: co Rows[0][...] / .Rows[0]. => DOC NOI DUNG (that su khong tat dinh)
        //                                chi co Rows.Count / Any()  => GUARD (bo qua)
        let usesRow = false, checksCount = false;
        for (let k = i; k < Math.min(i + 40, lines.length); k++) {
            const c = raw[k];
            // #310b: doc Rows[0][...] BEN TRONG nhanh BAO LOI van la GUARD — no chi lay du lieu de ghi
            //   vao thong bao (vd "Check.ROID"), khong dung lam du lieu nghiep vu.
            //   Nhan dien: quanh do (±6 dong) co AddRange/throw/CMyException/TError.
            if (/Rows\s*\[\s*0\s*\]\s*\[/.test(c)) {
                const lo = Math.max(0, k - 6), hi = Math.min(raw.length, k + 7);
                const ctx = raw.slice(lo, hi).join(" ");
                if (!/AddRange|throw |CMyException|\.Raise\(|TError\./.test(ctx)) usesRow = true;
            }
            if (/Rows\.Count|\.Any\(\)|Rows\.Count\s*>\s*0/.test(c)) checksCount = true;
        }
        const kind = usesRow ? 'DOC-DONG' : (checksCount ? 'GUARD' : 'CHUA-RO');

        const where = path.basename(f) + ':' + (i + 1);
        if (!ordCol) {
            if (!closed) continue;
            if (kind === 'GUARD') { nGuard++; continue; }          // vo hai, khong dem la no
            nNoOrder++;
            console.log((kind === 'DOC-DONG' ? '🟠 KHONG ORDER BY + DOC DONG  ' : '🟡 KHONG ORDER BY (chua ro)  ')
                + where + '   select top 1 ' + selCol);
            continue;
        }
        if (bare(selCol) === bare(ordCol)) { nOk++; continue; }
        nMismatch++;
        console.log('🔴 LECH TRUC  ' + where + '   select ' + selCol + '   |   order by ' + ordCol);
    }
}
console.log('\nTONG: ' + nMismatch + ' LECH, ' + nNoOrder + ' khong-order-by DANG LO, '
    + nGuard + ' guard (vo hai, da loai), ' + nOk + ' khop truc');
