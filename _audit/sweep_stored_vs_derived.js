// #313 SWEEP "LUU hay DAN XUAT": tim cot MiniHTC luu lam COT THAT nhung nguon chi TINH luc doc.
// Sinh tu su co #312 (C0-quingentesimusquinquagesimus): TotalQuantityIn / TotalQuantityInExchangeRate
// duoc them lam cot LUU + cho ghi qua DTO, trong khi nguon SUM tu phieu nhap moi lan doc
// => client gui gi cung thanh "da nhap bay nhieu". Build xanh, §12 du dau vet, NGU NGHIA SAI.
//
// Cach lam:
//   1) Quet NGUON: moi cot duoc GHI that su -> `Rows[0]["X"]` / `newRow["X"]` / lstMapFN "X"
//      => tap W (writable).
//   2) Quet NGUON: moi cot xuat hien nhu ALIAS ket qua (`) X` / `end X` / `as X`) hoac trong
//      `into #tbl...` => tap D (derived-looking).
//   3) Quet MiniHTC Entities.cs: moi property.
//   4) Bao cot nao: co trong D, KHONG co trong W, va MiniHTC dang LUU => nghi la dan xuat.
//
// Ket qua chi la GOI Y (luat C0-...vicesimussecundus: sweep neu vi tri, khong neu tham quyen):
// phai mo ham Create/Update cua nguon xac nhan truoc khi sua.
// ===== #381 HAI DIEU PHAI BIET TRUOC KHI TIN KET QUA SWEEP NAY =====
//
// 1) DIEM MU LON: gia dinh "nguon khong ghi cot X" chi dung khi lenh ghi CO liet ke cot.
//    Do that tren 2 cay nguon: 6061 lenh SaveData, trong do 859 (14%) goi dang
//    `SaveData(table, dt)` — KHONG truyen danh sach cot => ghi MOI cot cua DataTable ma
//    CLIENT gui len. Vi du `Ser_Mst_Part` (Inventory.Report.cs:8001-8006): alEffectiveColumn
//    duoc tao RONG roi truyen thang, con dt_Ser_MST_Part_Input lay nguyen tu ds client.
//    => Voi nhung bang kieu nay, "0 hit ghi" KHONG chung minh duoc gi.
//
// 2) DO CHINH XAC THUC DO (#381, kiem tay 8/13 nghi van cua lan chay #380):
//    - THAT:  ServicePart.InventoryQuantity  (da va o #380)
//    - GIA:   AmountTotal x3 (MiniHTC tu tinh tu dong, khong nhan DTO; con tro cua sweep con
//             tro nham vao mot danh sach ten bang)
//    - GIA:   ExchangeRate, CreateDateTime, TotalValVAT, EffectiveDate, CustomerName
//             (nguon CO ghi that: 8 / 64 / 18 / 46 / 23 lan)
//    - GIA:   TotalPrice x2 (MiniHTC tu tinh = CostInCheck + CostOutCheck)
//    => 1 that / 8 da kiem. Ket qua sweep la GOI Y RAT THO; luon kiem tay ca hai chieu:
//       (a) nguon co ghi that khong, (b) MiniHTC co nhan tu DTO khong.
//
// Cac cot da kiem va LOAI dưới day se khong bao lai nua.
const CLEARED = new Set([
    'amounttotal',      // #381: MiniHTC tu tinh tu dong (h.AmountTotal = total)
    // 🔴 #382 RUT LAI 5 muc duoi day khoi CLEARED: chung duoc "loai" o #381 bang phep dem
    //   SO LAN XUAT HIEN cua ten cot, ma moi lan xuat hien deu la DOC (vd ExchangeRate: 8/8 hit
    //   la drPartItem["ExchangeRate"] dung lam GIA TRI). Phai dung _audit/detect_column_write.js
    //   (5 dang ghi) roi moi ket luan. Da kiem lai bang bo do do:
    //     ExchangeRate  -> CO ghi, nhung chi vao MASTER TST_Mst_Exchange_Unit; tren DONG DON HANG
    //                      la DAN XUAT => THAT SU LA LOI, da va o #382.
    //     UnitStockIn   -> KHONG co cho ghi nao => cung dan xuat, da va o #382.
    //   Bon muc con lai chua kiem lai bang bo do dung => KHONG dam loai, tra ve danh sach nghi.
    'totalprice',       // #381: MiniHTC tu tinh (CostInCheck + CostOutCheck)
    'inventoryquantity',// #380: DA VA — bo nhan tu DTO, them endpoint tinh tu PartStock
]);

const fs = require('fs'), path = require('path');

function readText(p) {
    const b = fs.readFileSync(p);
    if (b.length > 1 && b[0] === 0xFF && b[1] === 0xFE) return b.toString('utf16le');
    return b.toString('utf8');
}

// 🔴 #381b BAT BUOC CHAY MOT LAN TREN TOAN BO FILE.
//   Su co that: goi bang `find ... -print0 | xargs -0 node sweep.js` thi xargs CHIA NHO danh sach
//   thanh nhieu lo, moi lo chay sweep MOT LAN voi tap nguon CAT KHUC. Tap W (cot co ghi) cua
//   moi lo thieu => dương tinh gia bung len (do duoc 60, 78 nghi van o cac lo, trong khi chay
//   day du chi con vai cai). Lan chay #380 bi dinh dung loi nay: con so "13 property" thuc ra
//   la ket qua cua LO CUOI, khong phai cua toan cay.
//   => Dung `--list <file>` : moi dong la mot duong dan. Vi du:
//      find A B -name "*.cs" > /tmp/files.txt
//      node sweep.js Models/Entities.cs --list /tmp/files.txt
const rawArgs = process.argv.slice(2);
const args = [];
for (let i = 0; i < rawArgs.length; i++) {
    if (rawArgs[i] === '--list') {
        const lf = rawArgs[++i];
        for (const ln of fs.readFileSync(lf, 'utf8').split(/\r?\n/)) {
            const p = ln.trim();
            if (p) args.push(p);
        }
    } else args.push(rawArgs[i]);
}
const entFile = args.find(a => /Entities\.cs$/i.test(a));
const srcFiles = args.filter(a => a !== entFile);
if (!entFile) { console.error('Thieu duong dan Models/Entities.cs'); process.exit(1); }

// 🔴 #314c BAT BUOC QUET **MOI** CAY NGUON LIEN QUAN.
//   MiniHTC port tu ~20 he nguon. Tap W (cot co GHI) dung lai o cay nao duoc truyen vao; cot cua he
//   KHONG duoc quet se trong nhu "chua bao gio duoc ghi" => bao dan xuat NHAM.
//   Bang chung: TotalValVAT bi bao la dan xuat khi chi quet TCMotor; no la cot LUU that cua 2010.HTC
//   (Invoice_Invoice, doc qua Rows[0]["TotalValVAT"]). Them cay 2010.HTC vao => tu bien mat.
const trees = new Set(srcFiles.map(f => (f.match(/idocNet[\\/]([^\\/]+)/) || [])[1]).filter(Boolean));
if (trees.size < 2) {
    console.log("⚠️  CANH BAO: chi thay " + trees.size + " cay nguon (" + [...trees].join(", ") + ").");
    console.log("    Ket qua se co FALSE POSITIVE cho moi cot den tu he nguon KHONG duoc quet.");
    console.log("    Truyen them cac cay khac (2010.HTC, 2021.1.TCMotor, ...) roi chay lai.\n");
}

const W = new Set();   // cot NGUON co ghi
const D = new Map();   // ten dan xuat -> 'file:line' dau tien (de kiem tay)
for (const f of srcFiles) {
    const raw = readText(f).split(/\r?\n/);
    // #314: NGUON con GHI bang SQL `update <alias> set <alias>.<Cot> = ...` — sweep #313 chi bat kieu
    //   DataTable (Rows[0]["X"]=) nen goi nham cac cot do la "dan xuat".
    //   Bang chung: TotalValOrderBeforeDc/AfterDc/AfterVAT (A.02.OrderPart.cs:1068) duoc UPDATE that su,
    //   nhung #313 van liet ke chung => FALSE POSITIVE.
    let inUpdateSet = false;
    for (const line0 of raw) {
        if (line0.trim().startsWith('//')) continue;          // C# comment (bai hoc #308)
        const line = line0.replace(/--.*$/, '');              // SQL comment (bai hoc #305)

        // #314 nhan dien khoi `update ... set`
        if (/\bupdate\s+[A-Za-z0-9_\[\]\.]+/i.test(line)) inUpdateSet = false;
        if (/^\s*set\b/i.test(line) || /\bupdate\b[\s\S]*\bset\b/i.test(line)) inUpdateSet = true;
        if (/^\s*(from|where|;)\b/i.test(line)) inUpdateSet = false;
        if (inUpdateSet) {
            // `t.Cot = f.Cot` hoac `Cot = ...` trong menh de SET => day la GHI
            const mu = /^\s*,?\s*(?:[A-Za-z0-9_]+\.)?([A-Za-z0-9_]+)\s*=/.exec(line);
            if (mu) W.add(mu[1].toLowerCase());
        }

        // (1) GHI: Rows[0]["X"] = ... | newRow["X"] = ... | strFN = "X"
        let m;
        // #314b: TEN BIEN ghi la TUY Y — nguon dung ca `Rows[0]["X"]`, `newRow["X"]`, `aRow["X"]`, `dr["X"]`.
        //   #313 hardcode "newRow" nen goi nham AverageCost (ghi qua aRow["AverageCost"], Stock.cs:3972)
        //   la "dan xuat" => FALSE POSITIVE thu hai. Nay bat MOI `<bien>["X"] =` (gan, khong phai so sanh).
        const reW = /[A-Za-z_][A-Za-z0-9_.\[\]]*\[\s*"([A-Za-z0-9_]+)"\s*\]\s*=(?!=)/g;
        while ((m = reW.exec(line))) W.add(m[1].toLowerCase());
        const reMap = /strFN\s*=\s*"([A-Za-z0-9_]+)"/g;
        while ((m = reMap.exec(line))) W.add(m[1].toLowerCase());
        const reEff = /alColumnEffective\.Add\("([A-Za-z0-9_]+)"\)|alEffectiveColumn\.Add\("([A-Za-z0-9_]+)"\)/g;
        while ((m = reEff.exec(line))) W.add((m[1] || m[2]).toLowerCase());

        // (2) DAN XUAT: ') X' sau subquery, 'end X'/'end as X' sau case, 'sum(...) X'
        const reD1 = /\)\s+([A-Za-z][A-Za-z0-9_]{2,})\s*$/;             // ) TotalQuantityIn
        const reD2 = /\bend\s+(?:as\s+)?([A-Za-z][A-Za-z0-9_]{2,})\s*$/i;
        const reD3 = /\bsum\s*\([^)]*\)\s+(?:as\s+)?([A-Za-z][A-Za-z0-9_]{2,})/i;
        for (const re of [reD1, reD2, reD3]) {
            const r = re.exec(line.trim());
            if (r) { const k = r[1].toLowerCase(); if (!D.has(k)) D.set(k, path.basename(f) + ':' + (raw.indexOf(line0) + 1)); }
        }
    }
}

// (3) property cua MiniHTC, kem ten class dang chua
const ent = readText(entFile).split(/\r?\n/);
const rows = [];
let cls = '';
for (const l of ent) {
    const mc = /^public sealed class ([A-Za-z0-9_]+)/.exec(l);
    if (mc) { cls = mc[1]; continue; }
    const mp = /^\s*public\s+[A-Za-z0-9_<>?\[\]]+\s+([A-Za-z0-9_]+)\s*\{\s*get;\s*set;/.exec(l);
    if (mp) rows.push({ cls, prop: mp[1] });
}

// #313b: ten CHUNG CHUNG bi loai — chung la alias o mot truy van nao do nhung van la cot LUU that o bang
//   khac. Sweep doi chieu theo TEN TRAN (khong theo BANG) nen khong tu phan biet duoc; day la gioi han
//   da biet, khong phai bo sot. Chi giu ten DAC TRUNG (>= 10 ky tu, khong nam trong stoplist).
const generic = new Set(['totalamount', 'amount', 'unitprice', 'itemcode', 'phoneno', 'customercode',
    'transactiondate', 'rostatus', 'qtyremain', 'quantity', 'price', 'status', 'total', 'vat']);
const skip = new Set(['id', 'orgid', 'createdat', 'updatedat']);
const hits = [];
for (const r of rows) {
    const k = r.prop.toLowerCase();
    if (skip.has(k)) continue;
    if (generic.has(k)) continue;              // #313b: ten chung chung, khong du tin hieu
    if (r.prop.length < 10) continue;          // #313b: ten ngan de trung nghia giua cac bang
    if (!D.has(k)) continue;        // khong trong nhu dan xuat -> bo qua
    if (W.has(k)) continue;         // nguon CO ghi -> la cot LUU that
    hits.push(r);
}

const byCls = new Map();
for (const h of hits) {
    if (!byCls.has(h.cls)) byCls.set(h.cls, []);
    byCls.get(h.cls).push(h.prop);
}
// #381: bo cac cot da kiem tay va loai.
let nCleared = 0;
for (const [c, ps] of [...byCls.entries()]) {
    const keep = ps.filter(p => !CLEARED.has(p.toLowerCase()));
    nCleared += ps.length - keep.length;
    if (keep.length) byCls.set(c, keep); else byCls.delete(c);
}

for (const [c, ps] of [...byCls.entries()].sort((a, b) => b[1].length - a[1].length)) {
    console.log('🟠 ' + c + ':\n     ' + ps.map(p => p + '   [dan xuat tai ' + (D.get(p.toLowerCase()) || '?') + ']').join('\n     '));
}
console.log('\nTONG: nguon co ' + W.size + ' cot GHI, ' + D.size + ' ten trong nhu DAN XUAT; '
    + (hits.length - nCleared) + ' property MiniHTC CON nghi la DAN XUAT (tren ' + byCls.size + ' entity).');
console.log('   (' + nCleared + ' cot da kiem tay o #381 va LOAI — xem danh sach CLEARED dau file)');
console.log('⚠️  Day chi la GOI Y — phai mo ham Create/Update cua nguon xac nhan truoc khi sua.');
console.log('⚠️  DIEM MU: 859/6061 lenh SaveData KHONG liet ke cot (ghi moi cot client gui)');
console.log('    => voi nhung bang do, \"0 hit ghi\" KHONG chung minh duoc gi.');
