// #268 SWEEP SQL CHET: bien chuoi SQL DUNG XONG ROI VUT (khong bao gio toi Exec*/SaveData/InsertHuge).
// Sinh tu bai hoc #267 (C0-quadringentesimusvicesimusseptimus): strSql_SaveOnDB_01 chua DELETE khong WHERE
// duoc dung day du roi bo di -> port nham = viet API xoa sach bang.
//
// ⚠️ Ban dau tinh THEO DONG => bao oan hang loat, vi loi goi that thuong xuong dong:
//        DataSet ds = _dbMain.ExecQuery(
//               strSqlGetROID
//               );
//    Nen phai xet TOAN VAN BAN, cua so sau moi tu khoa thuc thi cho toi dau cham phay.
// Self-test bat buoc: strSqlGetROID (PartOrder:1719) phai SONG, strSql_SaveOnDB_01 phai CHET.
const fs = require('fs'), path = require('path');

const EXEC_KW = '(?:ExecQuery|ExecNonQuery|ExecScalar|SaveData|InsertHuge|UpdateHuge|ExecuteSql|Fill)';
// ⚠️ Bay thu HAI: ham DUNG SQL roi `return strSql;` cho ham khac chay (BuildGetDebAmountSql,
//    SqlTemplate_*). Khong dem `return` thi bao oan ca ho ham template.

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

let total = 0, checked = 0;
for (const f of process.argv.slice(2)) {
    const raw = readText(f);
    const lines = raw.split(/\r?\n/);

    const declared = new Map();
    const reDecl = /\bstring\s+(str[A-Za-z0-9_]*[Ss]ql[A-Za-z0-9_]*)\s*=/;
    lines.forEach((l, i) => { const m = reDecl.exec(l); if (m && !declared.has(m[1])) declared.set(m[1], i + 1); });
    checked += declared.size;

    const live = new Set();
    for (const n of declared.keys()) {
        // ten xuat hien trong danh sach tham so cua 1 loi goi thuc thi (co the xuong dong)
        const re = new RegExp(EXEC_KW + '\\s*\\([^;]{0,4000}\\b' + n + '\\b');
        const reRet = new RegExp('\\breturn\\s+' + n + '\\s*;');
        // ⚠️ Bay thu BA: nhieu chuoi con duoc string.Format GHEP vao mot strSql khac roi moi chay
        //    (BizHTC.Report.cs). Khong the truy dataflow bang regex => dung tieu chi CHAT NHAT,
        //    khong the bao oan: ten chi xuat hien DUNG 1 LAN trong ca file (chinh dong khai bao)
        //    => khong ai dung no, du duoi hinh thuc nao.
        const uses = (raw.match(new RegExp('\\b' + n + '\\b', 'g')) || []).length;
        if (re.test(raw) || reRet.test(raw) || uses > 1) live.add(n);
    }
    // gan gian tiep: a = b;
    for (let pass = 0; pass < 3; pass++)
        for (const l of lines) {
            const m = /^\s*([A-Za-z0-9_]+)\s*=\s*([A-Za-z0-9_]+)\s*;/.exec(l);
            if (!m) continue;
            if (live.has(m[1]) && declared.has(m[2])) live.add(m[2]);
            if (live.has(m[2]) && declared.has(m[1])) live.add(m[1]);
        }

    const dead = [...declared.entries()].filter(([n]) => !live.has(n));
    if (!dead.length) continue;
    console.log('\n=== ' + path.basename(f) + ' (' + declared.size + ' bien SQL, ' + dead.length + ' CHET)');
    for (const [n, ln] of dead) {
        const body = lines.slice(ln - 1, ln + 120).join('\n');
        const danger = [];
        if (/\bdelete\s+from\s+[\w\[\]\.]+\s*;/i.test(body)) danger.push('DELETE-KHONG-WHERE');
        if (/\btruncate\s+table\b/i.test(body)) danger.push('TRUNCATE');
        if (/\bdrop\s+table\s+(?!#)/i.test(body)) danger.push('DROP-TABLE-THAT');
        console.log('  DEAD  ' + n + '  (:' + ln + ')' + (danger.length ? '   ⚠️ ' + danger.join(',') : ''));
        total++;
    }
}
console.log('\nTONG: ' + checked + ' bien SQL, ' + total + ' CHET');
