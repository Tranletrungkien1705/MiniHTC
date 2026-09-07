// sweep_dead_column.js — #343
//
// TÌM "CỘT CHẾT": thuộc tính có trong entity (và thường cả Seeder) nhưng KHÔNG đường nào GHI.
//
// Vì sao cần: #337 phát hiện `RoPartItem.ExpenseType` (thêm ở #280) không có đường ghi ⇒ mọi
// chỉ tiêu lọc theo nó LUÔN bằng 0 mà build vẫn xanh. #342 phát hiện `RoServiceItem.ExpenseType`
// **cũng vậy** — cùng đợt thêm cột, sót ở bảng song sinh. Hai lần đều tìm ra bằng tay; sweep này
// làm việc đó một cách hệ thống.
//
// Cách nhận "có ghi" (bám 4 dạng ghi thật trong Program.cs):
//   1. khởi tạo object:      `new Entity { … Col = … }`   → bắt `Col = ` trong khối `new Entity {`
//   2. gán trực tiếp:        `x.Col = …`  /  `ex.Col = …`
//   3. gán qua EF property:  `entry.Property("Col")`
//   4. cập nhật hàng loạt:   `SetProperty(e => e.Col, …)`
//
// CHỈ báo cột **có thể ghi được về mặt nghiệp vụ**. Bỏ qua:
//   - khoá và cột hệ thống (Id/OrgId/CreatedAt/UpdatedAt…) — do hạ tầng đặt;
//   - cột chỉ-đọc theo thiết kế: sweep KHÔNG tự biết, nên phần "đọc mà không ghi" được tách riêng
//     để người đọc quyết định, thay vì gộp chung thành "lỗi".
'use strict';
const fs = require('fs');
const path = require('path');

const ROOT = process.argv[2] ? path.resolve(process.argv[2]) : path.resolve(__dirname, '..');
const ent = fs.readFileSync(path.join(ROOT, 'Models/Entities.cs'), 'utf8');
const prog = fs.readFileSync(path.join(ROOT, 'Program.cs'), 'utf8');

// cột do hạ tầng đặt, không tính là "chết"
const SYSTEM = new Set(['Id', 'OrgId', 'CreatedAt', 'UpdatedAt']);

// ---- LOAI TRU CO CHU DICH ----
// Cot da TRA NGUON va ket luan "khong co duong ghi" la DUNG. Ghi vao day de cac luot sau
// khong phai dieu tra lai tu dau. Moi muc PHAI co ly do + so hieu luot da ket luan.
const EXPECTED = {
    // #312: nguon DAN XUAT hai cot nay bang cach cong dong nhap kho Status='3',
    //   khong luu san. Da CO Y go duong ghi; cot chi con de doi chieu du lieu cu.
    'OrderPartLine.TotalQuantityIn': '#312 dan xuat, khong luu',
    'OrderPartLine.TotalQuantityInExchangeRate': '#312 dan xuat, khong luu',
    // #346: moi cau GHI trong TERP.BizCarSv deu bi comment; he TST ghi thang vao CSDL.
    'OrderPart.TSTID': '#346 he ngoai ghi, tang biz chi TRA',
    'OrderPartLine.TSTID': '#346 he ngoai ghi, tang biz chi TRA',
    // #346: nguon KHONG co cot nay — objValDiscount la TEN THAM SO, gan vao DiscountRate.
    'OrderPart.ValDiscount': '#346 cot ma, nguon dung DiscountRate',
    // #350: cung ho TSTID — he TST/ngoai ghi thang, tang biz chi DOC (ghi = 0 trong ca 2 cay canonical).
    'OrderComplain.TSTOrderComplainNo': '#350 he TST ghi, biz chi DOC',
    'OrderComplain.TSTEmployeeCode': '#350 he TST ghi, biz chi DOC',
    'ReqPartPrice.TSTReqPartPriceID': '#350 he TST ghi, biz chi DOC',
    'ReqPartPrice.IsUpdatePrice': '#350 nguon khong ghi (ghi = 0)',
    // #350: hai cot nay o nguon CHI duoc SAO CHEP lai tu ban ghi cu trong luong dong bo DataWH
    //   (dr["X"] = dt_CheckOnDB.Rows[i]["X"]), khong phai dong dau duyet do nghiep vu sinh ra.
    'TranspDlvConfirm.FApprovedDate': '#350 chi sao chep trong luong dong bo, khong phai dong dau',
    'TranspDlvConfirm.FApprovedBy': '#350 chi sao chep trong luong dong bo, khong phai dong dau',
    // #343 da tra nguon: grep ra 0 cho ghi vao Ser_RO => nghi la cot do port tu them.
    'RepairOrder.FlagIsDLQuery': '#343 nguon khong ghi vao Ser_RO',
    'RepairOrder.PointVoucher': '#343 nguon khong ghi vao Ser_RO',
};

// ---- 1. gom entity → danh sách thuộc tính ----
const entities = [];
const reClass = /public sealed class ([A-Za-z0-9_]+)\s*\n?\s*\{/g;
let m;
while ((m = reClass.exec(ent)) !== null) {
    const name = m[1];
    // cắt tới dấu } ở cột 0 gần nhất
    const rest = ent.slice(m.index);
    const end = rest.indexOf('\n}');
    const body = end < 0 ? rest : rest.slice(0, end);
    const props = [];
    const reProp = /public\s+[A-Za-z0-9_<>?.\[\]]+\s+([A-Za-z0-9_]+)\s*\{\s*get;\s*set;/g;
    let p;
    while ((p = reProp.exec(body)) !== null) props.push(p[1]);
    if (props.length) entities.push({ name, props: [...new Set(props)] });
}

// ---- 2. với mỗi entity, gom các cột ĐƯỢC GHI ----
function writtenColumns(entityName) {
    const written = new Set();

    // (1) new Entity { … }  — quét mọi khối khởi tạo của entity này
    const reNew = new RegExp('new\\s+' + entityName + '\\s*(?:\\(\\s*\\))?\\s*\\{', 'g');
    let n;
    while ((n = reNew.exec(prog)) !== null) {
        // cân ngoặc để lấy trọn khối
        let i = prog.indexOf('{', n.index), depth = 0, j = i;
        for (; j < prog.length; j++) {
            if (prog[j] === '{') depth++;
            else if (prog[j] === '}') { depth--; if (depth === 0) break; }
        }
        const block = prog.slice(i, j + 1);
        // 🔴 #349 PHÉP GÁN KÉP: `X += …` cũng là GHI. Mẫu cũ chỉ bắt `X =` nên bỏ sót
        //   `+= -= *= /= ??=` ⇒ báo NHẦM cột đã có đường ghi là "chết" (ca `SupplierDebit`).
        const reAssign = /([A-Za-z0-9_]+)\s*(?:\+|-|\*|\/|\?\?)?=(?!=)/g;
        let a;
        while ((a = reAssign.exec(block)) !== null) written.add(a[1]);
    }

    // (2)(3)(4) gán qua biến / EF — không gắn được chắc chắn với entity nào,
    //     nên chỉ dùng để LOẠI nghi ngờ, đúng tinh thần "thà bỏ sót còn hơn báo bừa".
    return written;
}

// tập mọi tên cột được gán qua biến ở BẤT KỲ đâu (dùng để giảm dương tính giả)
const anyVarAssign = new Set();
{
    // #349: cũng phải chấp nhận phép gán kép (`h.PaidAmount += …`) và hậu tố `!` (`row!.X = …`).
    const re = /(?:^|[^A-Za-z0-9_])[a-z][A-Za-z0-9_]*!?\.([A-Za-z0-9_]+)\s*(?:\+|-|\*|\/|\?\?)?=(?!=)/g;
    let a;
    while ((a = re.exec(prog)) !== null) anyVarAssign.add(a[1]);
    const re2 = /SetProperty\(\s*[A-Za-z0-9_]+\s*=>\s*[A-Za-z0-9_]+\.([A-Za-z0-9_]+)/g;
    while ((a = re2.exec(prog)) !== null) anyVarAssign.add(a[1]);
    const re3 = /Property\(\s*"([A-Za-z0-9_]+)"\s*\)/g;
    while ((a = re3.exec(prog)) !== null) anyVarAssign.add(a[1]);
}

// ---- 3. báo cáo ----
const dead = [];
for (const e of entities) {
    // entity không hề được khởi tạo trong Program.cs ⇒ bỏ qua (có thể chỉ đọc / seed)
    if (!new RegExp('new\\s+' + e.name + '\\b').test(prog)) continue;
    const w = writtenColumns(e.name);
    const miss = e.props.filter(p => !SYSTEM.has(p) && !w.has(p) && !anyVarAssign.has(p)
        && !EXPECTED[e.name + '.' + p]);
    if (miss.length) dead.push({ entity: e.name, total: e.props.length, miss });
}

dead.sort((a, b) => b.miss.length - a.miss.length);
let totalMiss = 0;
for (const d of dead) totalMiss += d.miss.length;

console.log('=== CỘT CHẾT: có trong entity, KHÔNG đường nào ghi ===');
console.log('entity có cột chết: ' + dead.length + ' — tổng cột chết: ' + totalMiss);
console.log('(đã loại cột hệ thống ' + [...SYSTEM].join('/') + ' và mọi tên cột được gán qua biến ở bất kỳ đâu)');
console.log('(đã loại ' + Object.keys(EXPECTED).length + ' cột LOẠI TRỪ CÓ CHỦ ĐÍCH — xem bảng EXPECTED đầu file)');
console.log('');
for (const d of dead)
    console.log(d.entity + '  (' + d.miss.length + '/' + d.total + '):  ' + d.miss.join(' '));

console.log('');
console.log('⚠️ Đây là DANH SÁCH NGHI VẤN, không phải kết luận. Mỗi mục phải kiểm tay:');
console.log('   - cột chỉ-đọc theo thiết kế (do job/seed đặt) là HỢP LỆ;');
console.log('   - cột nguồn CÓ ghi mà MiniHTC không ⇒ GAP thật (xem #337/#342).');
console.log('   Ưu tiên kiểm cột mà báo cáo/bộ lọc có dùng — sai ở đó ra số THIẾU, không ra lỗi.');
