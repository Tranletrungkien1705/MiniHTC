namespace MiniHTC.Models;
public sealed class Org { public Guid Id { get; set; } = Guid.NewGuid(); public string Name { get; set; } = ""; public string ApiKey { get; set; } = ""; public DateTime CreatedAt { get; set; } = DateTime.Now; }

/// <summary>Khu vực (Mst_Area) — port 1:1 FrmArea (2010.HTC/TERP.HTCClient/Admin/Dealer).</summary>
public sealed class Area
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AreaCode { get; set; } = "";
    public string AreaName { get; set; } = "";
    public string? AreaRootCode { get; set; }   // mã khu vực cha (cây phân cấp) — port 1:1 FrmArea, audit 2026-09-03 phát hiện thiếu
    public int Level { get; set; } = 1;         // cấp bậc, tự tính = Level(cha)+1; root=1
    public string Status { get; set; } = "1";   // 1=hiệu lực, 0=ngừng (cờ 2010.HTC)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đại lý (Mst_Dealer) — port 1:1 FrmDealer (2010.HTC/Admin/Dealer).</summary>
/// <summary>🔴 #375 §12 TIÊU ĐỀ BÁO CÁO theo đại lý (`Mst_ReportHeader`) — phần đầu thư in trên mọi
/// biểu mẫu. **BA nơi trong nguồn đọc bảng này với BA hành vi khác nhau**, xem `/api/reportheaders`.</summary>
public sealed class ReportHeader
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string? DealerName { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? Website { get; set; }
    /// <summary>🔴 Tên cột nói 'phòng trưng bày' nhưng nguồn trả ra dưới nhãn **Tel**.</summary>
    public string? Showroom1 { get; set; }
    /// <summary>🔴 Trả ra dưới nhãn **Fax**.</summary>
    public string? Showroom2 { get; set; }
    /// <summary>🔴 Trả ra dưới nhãn **Mobile**.</summary>
    public string? Showroom3 { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class Dealer
{
    /// <summary>🔴 #395 §12 ĐỊA CHỈ WEB SERVICE CỦA ĐẠI LÝ (`WSUrlAddr` trong `CmCt_Mst_Network`).
    /// HTC duyệt đề nghị bảo hành xong **gọi thẳng web service của đại lý**
    /// (`Ser_ROWarrantyReport_HTCApproved_ForDealer`); thiếu địa chỉ này thì nguồn **ném lỗi**
    /// `Ser_ROWarrantyReport_WSUrlAddr_NotFound` và **KHÔNG duyệt**. Xem
    /// `POST /api/warrantyclaims/{id}/action`.</summary>
    public string? WsUrlAddr { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string DealerName { get; set; } = "";
    public string? BUCode { get; set; }
    public string? ProvinceCode { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // audit 2026-09-03 — 29 field còn thiếu so với FrmDealer gốc (39 field), bổ sung đủ:
    public string? DealerType { get; set; }
    public string? BuPattern { get; set; }
    public string? FlagDirect { get; set; }
    public string? FlagActive { get; set; }
    public string? DealerScale { get; set; }
    /// <summary>Mã vùng thị trường marketing (`Mst_Dealer_UpdateMRKAMCode`,
    /// 2010.HTC `BizHTC.Marketing.cs:12035`) — trỏ sang <see cref="MrkMstAreaMarket"/>.</summary>
    public string? MRKAMCode { get; set; }
    public string? DealerPhoneNo { get; set; }
    public string? DealerFaxNo { get; set; }
    public string? CompanyName { get; set; }
    public string? CompanyAddress { get; set; }
    public string? ShowroomAddress { get; set; }
    public string? GarageAddress { get; set; }
    public string? GarageManagerPhoneNo { get; set; }
    public string? GarageFaxNo { get; set; }
    public string? DirectorName { get; set; }
    public string? DirectorPhoneNo { get; set; }
    public string? DirectorEmail { get; set; }
    public string? SalesManagerName { get; set; }
    public string? SalesManagerPhoneNo { get; set; }
    public string? SalesManagerEmail { get; set; }
    public string? GarageManagerName { get; set; }
    public string? GarageManagerEmail { get; set; }
    public string? ContactName { get; set; }
    public string? Signer { get; set; }
    public string? SignerPosition { get; set; }
    public string? CtrNoSigner { get; set; }
    public string? CtrNoSignerPosition { get; set; }
    public string? Remark { get; set; }
    public string? HTCStaffInCharge { get; set; }
    public string? DealerAddress01 { get; set; }
    public string? DealerAddress02 { get; set; }
    public string? DealerAddress03 { get; set; }
    public string? DealerAddress04 { get; set; }
    public string? DealerAddress05 { get; set; }
    public string? FlagTCG { get; set; }
    public string? FlagOrdTCG { get; set; }
    public string? FlagAutoLXX { get; set; }
    public string? FlagAutoMapVIN { get; set; }
    public string? FlagAutoSOAppr { get; set; }

    // ===== 🔴 #269: hai mã ĐĂNG KÝ VỚI HỆ HCC (`Mst_Dealer.OrgHCCID` / `NetworkHCCID`) =====
    /// <summary>`OrgHCCID` — mã tổ chức bên HCC. **Cổng chặn của cả job NoShow**: nguồn lọc
    /// `and md.OrgHCCID is not null` ở CẢ HAI câu (chọn đại lý và ghép dữ liệu) ⇒ đại lý chưa đăng ký
    /// HCC thì không bao giờ được đẩy.</summary>
    /// <summary>🔴 #335 FLAGDEALERHTC — đại lý có thuộc mạng lưới **HTC** hay không. Job sinh KPI của
    /// nguồn lọc `and t.FlagDealerHTC = '1'` **cùng với** `FlagActive = '1'` và loại đích danh `VN101`
    /// (đại lý idocNet Test). Thiếu cột này thì không thể lọc đúng đại lý được sinh báo cáo.</summary>
    public string? FlagDealerHTC { get; set; }
    public string? OrgHCCID { get; set; }
    /// <summary>`NetworkHCCID` — mã mạng lưới bên HCC (khác `NetworkID` của bảng `CmCt_Mst_Network`).</summary>
    public string? NetworkHCCID { get; set; }
}

/// <summary>Bảng giá xe (Mst_CarPrice) — port 1:1 FrmCarPrice: giá theo Model/Spec/Color.</summary>
public sealed class CarPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; } = 10;   // audit 2026-09-03: KHÔNG có trong FrmCarPrice gốc (đã kiểm tra, không thấy field VAT) — do fire trước tự thêm để tiện tính giá gồm thuế, giữ lại vì không phá dữ liệu, nhưng lưu ý đây KHÔNG phải field 1:1.
    public DateTime EffectiveDate { get; set; } = DateTime.Now;   // audit 2026-09-03: THIẾU HOÀN TOÀN — CarPrice gốc là bảng giá THEO THỜI ĐIỂM (1 Model+Spec+Color có nhiều giá theo Effective_Date)
    public string SoType { get; set; } = "";  // audit 2026-09-03: THIẾU HOÀN TOÀN — 1 phần khóa hỗn hợp gốc (Model+Spec+Color+EffectiveDate+SoType)
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// audit 2026-09-03: entity "Customer"/"/api/customers"/"customer.html" (tick "port 1:1 FrmCustomerBase" trong
// manifest ban đầu) là TWIN TRACE SAI — FrmCustomerBase.cs thực ra chỉ là màn danh mục "Nguồn khách hàng"
// (CustomerBaseCode/Name/FlagActive, chỉ đọc — đã port đúng qua Masters category "CustomerBase").
// Nghiệp vụ khách hàng thật (CustomerCode/FullName/Phone/IDCard/Province/District/Gender/CusBaseCode...) là
// FrmNewCustomer/FrmMngCustomer (Views/SalesDealer) — đã port đúng, đủ field hơn, ở DealerCustomer/api/dealercustomers
// bên dưới (do một fire audit khác, độc lập, port đúng nguồn). "Customer" là bản trùng/thiếu field, KHÔNG có nơi
// nào khác trong code gọi tới (grep xác nhận) → xoá hẳn thay vì vá thêm, tránh 2 nguồn sự thật cho cùng 1 nghiệp vụ.

/// <summary>Nhân viên bán hàng (Mst_SalesMan) — port 1:1 FrmCreateSalesMan.</summary>
public sealed class SalesMan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SalesManCode { get; set; } = "";
    public string SalesManName { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? DepartmentCode { get; set; }
    public string? SalesType { get; set; }   // loại NVBH (SMType) — Support cập nhật
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Status { get; set; } = "1";
    // audit 2026-09-03: 19 field dưới đây bổ sung — port trước audit chỉ có 9/28 field thật (FrmCreateSalesMan.cs)
    public string? Gender { get; set; }             // 0=Nam/1=Nữ
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? ProvinceCode { get; set; }
    public string? QualificationCode { get; set; }  // trình độ chuyên môn
    public string? Specialized { get; set; }        // chuyên ngành
    public string? YearExperience { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Position { get; set; }           // chức vụ (text tự do trong nguồn — SMPosition)
    public string? PositionCode { get; set; }        // SMPositionCode (Mst_Position)
    public string? CertificateCode { get; set; }
    public string? SMHyundaiCode { get; set; }
    public string? IdentityCardNo { get; set; }
    public string? WebsiteLink { get; set; }         // bắt buộc nếu SalesType=TVBH
    public string? FacebookLink { get; set; }
    public string? FanpageLink { get; set; }
    public string? GroupLink { get; set; }
    public string? ZaloLink { get; set; }
    public string? AccountHTA { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>PDI inspection tracking (simplified) — KHÔNG có twin WinForm 1:1: FrmMngDlr_PDIRequest đã port đúng tại DlrPdiRequest; FrmMngPDI/FrmNewPDI đã port tại HtmvPdi. Entity này là flow tổng hợp HTC-track Requested→Inspecting→Passed/Failed per VIN.</summary>
public sealed class PdiRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string Status { get; set; } = "Requested";   // Requested → Inspecting → Passed/Failed
    public string? Inspector { get; set; }
    public string? Result { get; set; }                  // ghi chú kết quả / lỗi
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? InspectedAt { get; set; }
}

/// <summary>
/// Thu hồi xe (FrmMngCarRetrieve / FrmNewCarRetrieve) — thu hồi xe từ đại lý về kho HTC.
/// 🔴 **#117 đối chiếu với biz nguồn** `StorageCarRetrieveCreate/Approve/DetailUpdate/DetailDel_New20181119`
/// (`TERP.BizHTC/DataWH/Biz.HTC.WH.cs` 69175 / 69828 / 70205 / 70418) — bảng nguồn là **CẶP**
/// `Sto_CarRetrieve` (đầu) + `Sto_CarRetrieveDetail` (dòng theo xe).
/// Bản MiniHTC này là dạng **PHẲNG** (một dòng = một xe) nên các cột đầu/dòng được gộp chung;
/// GAP đã vá ở #117: bổ sung `RetrieveOrderNo` (số lệnh — nhiều xe chung một lệnh ở nguồn),
/// `RetrieveDtlStatus`, `DeliveryOrderNo`, `CreatedBy`, `ApprovedBy`, và **sửa mã trạng thái**.
/// 🔴 `Status` cũ dùng chuỗi dài "Pending"/"Approved"/"Rejected" — **SAI so với `TConst.Stage`**
/// của nguồn: **"P" / "A" / "R"**. Đã đổi ở #117 kèm UPDATE dữ liệu cũ trong Seeder.
/// ⚠️ Nguồn ghi SONG SONG `_dbMain` + `_dbWH` (69489/69492, 69947/69948) — nợ `_dbWH` chung fleet.
/// </summary>
public sealed class CarRetrieve
{
    /// <summary>Số LỆNH thu hồi — ở nguồn là khoá của `Sto_CarRetrieve`, nhiều xe dùng chung một lệnh.</summary>
    public string? RetrieveOrderNo { get; set; }
    /// <summary>Trạng thái DÒNG (`Sto_CarRetrieveDetail.RetrieveDtlStatus`), tách khỏi trạng thái đầu.</summary>
    public string RetrieveDtlStatus { get; set; } = "P";
    /// <summary>Nguồn ghi rỗng khi tạo; chỉ `DetailUpdate` mới đặt giá trị.</summary>
    public string? DeliveryOrderNo { get; set; }
    public string? CreatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string StorageCode { get; set; } = "";        // kho nhận xe (BẮT BUỘC — gviewCar_ValidateRow)
    public DateTime? ExpectedStartDate { get; set; }     // ngày dự kiến bắt đầu thu hồi (BẮT BUỘC)
    public DateTime? ExpectedEndDate { get; set; }       // ngày dự kiến kết thúc thu hồi (BẮT BUỘC)
    public string? FlagEarlyCancel { get; set; }         // cờ xe sắp hủy (từ Car, read-only)
    public string? RetrieveRemark { get; set; }          // ghi chú (TblCarRetrieveDetail.Remark)
    /// <summary>🔴 TConst.Stage: "P" chờ duyệt → "A" duyệt / "R" từ chối (sửa ở #117).</summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }

    // ===== #159 side-effect `Sto_DlvMinutes_Approve_New20190416` (Biz.HTC.WH.cs:138340, csproj 272) =====
    // Duyệt biên bản giao xe GHI NGƯỢC "ngày xuất kho" lên CHỨNG TỪ NGUỒN của xe. Bốn nhánh theo loại
    // chứng từ, mỗi nhánh một CỘT KHÁC TÊN — đó là lý do port cũ bỏ sót cả ba.
    /// <summary>Ngày xuất kho thực tế của lệnh thu hồi (`Sto_CarRetrieveDetail.RetrieveOutDate`).</summary>
    public DateTime? RetrieveOutDate { get; set; }

    // ===== #170 parity `Sto_DlvMinutes_UpdateDlvEndDate_New20181115` (BizHTC.Storage.DlvMinutes.cs:9329) =====
    /// <summary>Ngày thu hồi XONG (`RetrieveEndDate`).</summary>
    public DateTime? RetrieveEndDate { get; set; }

    // ===== #170b nhat ky sua cuoi (LogLU*) — nguon ghi cap nay o moi buoc ghi =====
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Hủy xe (FrmCarCancel + FrmMngCarCancel) — hủy xe theo loại hủy, ghi nhận per-car; duyệt là governance thêm của web.</summary>
public sealed class CarCancel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string? CancelTypeCode { get; set; }          // CarCancelType (BẮT BUỘC nguồn — ERROROFDEALER mặc định)
    public string? CarCancelRemark { get; set; }         // TblRejectCar.CarCancelRemark per car
    public string? FlagEarlyCancel { get; set; }         // cờ xe sắp hủy
    public string? FlagMapVIN { get; set; }              // cờ map VIN
    public string Status { get; set; } = "Requested";   // Requested → Approved / Rejected (governance web thêm)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Cấu hình hệ thống (key-value) — port 1:1 các FrmMngConfig*/Setup của 2010.HTC.</summary>
public sealed class SysConfig
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ConfigKey { get; set; } = "";
    public string ConfigValue { get; set; } = "";
    public string? Description { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Kế hoạch/chỉ tiêu KD (FrmMngBusinessPlan) — chỉ tiêu bán theo đại lý/model/tháng.</summary>
public sealed class BusinessPlan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string Month { get; set; } = "";      // YYYYMM
    public int TargetQty { get; set; }
    public int ActualQty { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Header duyệt kế hoạch KD theo năm (Mst_BPL_BusinessPlan) — port 1:1 FrmMngBusinessPlan (2010.HTC/Sales).
/// audit 2026-09-03: port trước CHỈ có BusinessPlan (số mục tiêu/thực tế phẳng theo tháng-model, KHÔNG có
/// vòng đời duyệt Pending→Approved1→Approved2/Cancel, KHÔNG có Version INIT/ACTUAL). Bổ sung header này để
/// mô hình đúng vòng đời duyệt — CHƯA port đủ chi tiết 3 loại kế hoạch (Rtl/Ord/BO) × 12 tháng theo từng Model
/// (BPL_BusinessPlanDtl) — đó là gap CÒN LẠI, ghi rõ không suy diễn đã xong.</summary>
public sealed class BusinessPlanHeader
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BusinessPlanCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public int YearPlan { get; set; }
    public string Version { get; set; } = "INIT";        // INIT | ACTUAL

    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG mã nguồn `TConst.BusinessPlanStatus` (`BPL_BusinessPlan.BusinessPlanStatus`):
    /// "P" chờ duyệt → "A1" duyệt cấp 1 → "A2" duyệt cấp 2 · "A" đã duyệt (dùng cho DÒNG chi tiết).
    /// ⚠️ Port cũ dùng chuỗi TỰ ĐẶT "Pending"/"Approved1"/"Approved2" và **tự thêm "Cancelled" —
    /// mã mà nguồn KHÔNG CÓ**; ngược lại **thiếu mã "A"**.
    /// Đọc dữ liệu cũ: Pending→"P", Approved1→"A1", Approved2→"A2", Cancelled→"A2" (nguồn không có huỷ).
    /// </summary>
    public string Status { get; set; } = "P";
    public string? HTCStaffInCharge { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? Approve1At { get; set; }
    /// <summary>Người duyệt cấp 1 (`Appr1By`) — nguồn nhân bản sang phiên bản mới.</summary>
    public string? Approve1By { get; set; }
    public DateTime? Approve2At { get; set; }
    /// <summary>Người duyệt cấp 2 (`Appr2By`).</summary>
    public string? Approve2By { get; set; }
    /// <summary>Số lần lập kế hoạch (`TimesPlan`) — nguồn giữ khi nhân bản phiên bản.
    /// ⚠️ #177: kiểu phải NULLABLE — `_UnApprove2` gán `DBNull.Value` cho cột này khi bỏ duyệt.</summary>
    public int? TimesPlan { get; set; }
    public DateTime? CancelledAt { get; set; }

    // ===== #177 parity `BPL_BusinessPlan_UnApprove2` (DataWH/BizHTC.zTemp.cs:50378, csproj 276) =====
    /// <summary>Nhật ký sửa cuối — nguồn ghi `LogLUDateTime`/`LogLUBy` ở mọi bước duyệt/bỏ duyệt.</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>
/// 🔴 DÒNG chi tiết kế hoạch kinh doanh (`BPL_BusinessPlanDtl`) — **gap đã ghi rõ ở comment header
/// từ đợt audit 2026-09-03, nay port**: mỗi dòng là 1 `ModelCode`, mang **3 LOẠI kế hoạch × 12 tháng**:
/// bán lẻ (`Rtl_`), đặt hàng (`Ord_`), back-order (`BO_`).
/// Không có bảng này thì kế hoạch năm **không có số liệu nào** — chỉ còn vỏ vòng đời duyệt.
/// </summary>
public sealed class BusinessPlanDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BusinessPlanCode { get; set; } = "";
    public int YearPlan { get; set; }
    public string ModelCode { get; set; } = "";

    /// <summary>Trạng thái RIÊNG của dòng (`BusinessPlanDtlStatus`).
    /// ⚠️ Nguồn KHÔNG lan y hệt trạng thái header: khi nhân bản phiên bản, header ghi **"A2"**
    /// còn dòng ghi **"A"** (`BizHTC.zTemp.cs:50185` vs `:50221`).</summary>
    public string BusinessPlanDtlStatus { get; set; } = "P";
    /// <summary>Phiên bản của DÒNG (`VersionDtl`) — tách khỏi `Version` của header.</summary>
    public string VersionDtl { get; set; } = "INIT";

    /// <summary>Tổng số hợp đồng bán lẻ cả năm (`Rtl_TotalQtyDeal`).</summary>
    public decimal Rtl_TotalQtyDeal { get; set; }
    /// <summary>Tổng back-order cả năm (`BO_TotalQtyBO`).</summary>
    public decimal BO_TotalQtyBO { get; set; }

    // ----- Kế hoạch bán lẻ (Rtl_) theo 12 tháng -----
    public decimal Rtl_QtyM1 { get; set; }
    public decimal Rtl_QtyM2 { get; set; }
    public decimal Rtl_QtyM3 { get; set; }
    public decimal Rtl_QtyM4 { get; set; }
    public decimal Rtl_QtyM5 { get; set; }
    public decimal Rtl_QtyM6 { get; set; }
    public decimal Rtl_QtyM7 { get; set; }
    public decimal Rtl_QtyM8 { get; set; }
    public decimal Rtl_QtyM9 { get; set; }
    public decimal Rtl_QtyM10 { get; set; }
    public decimal Rtl_QtyM11 { get; set; }
    public decimal Rtl_QtyM12 { get; set; }

    // ----- Kế hoạch đặt hàng (Ord_) theo 12 tháng -----
    public decimal Ord_QtyM1 { get; set; }
    public decimal Ord_QtyM2 { get; set; }
    public decimal Ord_QtyM3 { get; set; }
    public decimal Ord_QtyM4 { get; set; }
    public decimal Ord_QtyM5 { get; set; }
    public decimal Ord_QtyM6 { get; set; }
    public decimal Ord_QtyM7 { get; set; }
    public decimal Ord_QtyM8 { get; set; }
    public decimal Ord_QtyM9 { get; set; }
    public decimal Ord_QtyM10 { get; set; }
    public decimal Ord_QtyM11 { get; set; }
    public decimal Ord_QtyM12 { get; set; }

    // ----- Kế hoạch back-order (BO_) theo 12 tháng -----
    public decimal BO_QtyM1 { get; set; }
    public decimal BO_QtyM2 { get; set; }
    public decimal BO_QtyM3 { get; set; }
    public decimal BO_QtyM4 { get; set; }
    public decimal BO_QtyM5 { get; set; }
    public decimal BO_QtyM6 { get; set; }
    public decimal BO_QtyM7 { get; set; }
    public decimal BO_QtyM8 { get; set; }
    public decimal BO_QtyM9 { get; set; }
    public decimal BO_QtyM10 { get; set; }
    public decimal BO_QtyM11 { get; set; }
    public decimal BO_QtyM12 { get; set; }
}

/// <summary>Lái thử xe (FrmMstCarDriverTest — TCMotor): khách đăng ký lái thử → xác nhận → hoàn tất.</summary>
public sealed class TestDrive
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? DealerCode { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } = "Booked";   // Booked → Done → Cancelled
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Yêu cầu bảo hành dịch vụ (TCMotor Warranty Claim) — ĐL claim BH lên hãng: mã lỗi + phụ tùng + công.</summary>
public sealed class WarrantyClaimTC
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ClaimNo { get; set; } = "";
    public string Vin { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? ErrorCode { get; set; }
    public decimal PartsCost { get; set; }
    public decimal LaborCost { get; set; }
    public string Status { get; set; } = "Submitted";   // Submitted → Approved/Rejected → Paid
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }
}

/// <summary>
/// Đơn mua xe từ hãng (`Ord_PurchaseOrder` — port 1:1 `OrderPOCreate_New2018119` /
/// `OrderPOCancel_New20181119`, 2010.HTC `Biz.HTC.WH.cs:27908/28160`).
/// ⚠️ Nguồn **KHÔNG có cột trạng thái**, vòng đời chỉ là cờ `FlagActive`: Create gán "1" (28085),
/// Cancel guard "1" rồi gán "0" (28241) — giống hệt `Ord_POCommand` (cụm lệnh đặt hàng).
/// </summary>
public sealed class PurchaseOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Số đơn mua (`Ord_PurchaseOrder.POCode`).</summary>
    public string POCode { get; set; } = "";
    public string OrderMonth { get; set; } = "";
    public string? ProductionMonth { get; set; }
    public string? ExpectedMonth { get; set; }
    /// <summary>Cờ hiệu lực — trục vòng đời DUY NHẤT của cụm ("1" còn hiệu lực / "0" đã huỷ).</summary>
    public string FlagActive { get; set; } = "1";
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng đơn mua xe (`Ord_PurchaseOrderDetail`): spec + model + màu + số lượng.</summary>
public sealed class PurchaseOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PurchaseOrderId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// ⚠️ **ENTITY TỰ CHẾ — KHÔNG có bảng nguồn.** Đã kiểm đủ 3 cách trên cả TCMotor lẫn 2010.HTC:
/// `SaveData("...PO"/"...PurchaseOrder")`, `insert into`, và grep tên cột `["PoNo"]` — **đều 0 hit**;
/// TCMotor chỉ có master `Ser_MST_Supplier`, không có bảng PO nhà cung cấp.
/// Cột `SupplierCode`/`Total` và chuỗi `Draft→Sent→Received` không tương ứng nguồn nào.
/// Giữ nguyên để không phá dữ liệu/UI đang có; **cần người quyết định xoá hay giữ** (đã ghi nợ).
/// Đơn mua xe THẬT của nguồn là <see cref="PurchaseOrder"/> (`Ord_PurchaseOrder`).
/// </summary>
public sealed class SupplierPO
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PoNo { get; set; } = "";
    public string SupplierCode { get; set; } = "";
    public string? Note { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Draft";   // Draft → Sent → Received (hoặc Cancelled)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    public DateTime? ReceivedAt { get; set; }
}

/// <summary>Định mức BOM bảo dưỡng (FrmMstBOMMng — TCMotor): header theo model+cấp BD.</summary>
public sealed class Bom
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BomCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? MaintLevel { get; set; }   // cấp bảo dưỡng (1000km/5000km...)
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng BOM: 1 phụ tùng + số lượng định mức.</summary>
public sealed class BomLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long BomId { get; set; }
    public string PartSku { get; set; } = "";
    public string? PartName { get; set; }
    public decimal Qty { get; set; } = 1;
}

/// <summary>Gia hạn bảo hành (FrmMstWarrantyExtension — TCMotor): mua thêm thời hạn BH cho xe.</summary>
public sealed class WarrantyExtension
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string? ItemCode { get; set; }   // hạng mục gia hạn
    public int ExtraMonths { get; set; }
    public decimal Fee { get; set; }
    public string Status { get; set; } = "Requested";  // Requested → Paid → Activated (hoặc Cancelled)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ActivatedAt { get; set; }
}

/// <summary>Phí bảo hiểm (Mst_InsuranceFee — port 1:1 FrmMst_InsuranceFee): hợp đồng + phí + tỷ lệ %.</summary>
public sealed class InsuranceFee
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string? InsCompanyCode { get; set; }
    public string? InsTypeCode { get; set; }
    public string? ContractNo { get; set; }
    public decimal Fee { get; set; }
    public decimal Percent { get; set; }
    public DateTime? EffStartDate { get; set; }  // ngày hiệu lực (TblMst_InsuranceFee.EffStartDate)
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Chương trình **hạn mức có ĐIỀU KIỆN – KHUYẾN MÃI** (`Mst_Quota`) —
/// nguồn: DMS40/0.01.Master.cs (csproj 122), `Mst_Quota_AddMultiX_New20220406` (4262) ghi tại 4621.
///
/// 🔴 #146 SỬA HIỂU SAI NGHIỆP VỤ: port cũ đọc bảng này là "số lượng xe theo đại lý/model/KỲ"
/// (`ModelCode`+`Period`+`Qty`+`UsedQty`) — **không có cột nào trong bốn cột đó tồn tại ở nguồn**.
/// Bảng thật là **cặp ĐIỀU KIỆN → KHUYẾN MÃI**: mua đủ `QtyCondition` của
/// `ModelCondition`/`SpecCodeCondition` thì được `QtyPromotion` của
/// `ModelPromotion`/`SpecCodePromotion`, hiệu lực theo **ngày DUYỆT đơn hàng** (`SOApprDate*`).
/// Bốn cột cũ giữ lại để đọc dữ liệu đã ghi, **KHÔNG ghi mới**.
/// </summary>
public sealed class Quota
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";

    // ===== #146 parity Mst_Quota: 12 cột thật của nguồn =====
    /// <summary>Mã chương trình (`QuotaCode`) — khoá nghiệp vụ cùng với `DealerCode`.</summary>
    public string? QuotaCode { get; set; }
    public string? QuotaName { get; set; }
    /// <summary>Dòng xe phải mua để đạt điều kiện (`ModelCondition`).</summary>
    public string? ModelCondition { get; set; }
    /// <summary>Dòng xe được thưởng (`ModelPromotion`).</summary>
    public string? ModelPromotion { get; set; }
    public string? SpecCodeCondition { get; set; }
    public string? SpecCodePromotion { get; set; }
    public decimal QtyCondition { get; set; }
    public decimal QtyPromotion { get; set; }
    /// <summary>Hiệu lực tính theo **ngày DUYỆT đơn hàng** (`SOApprDateFrom`/`SOApprDateTo`).</summary>
    public DateTime? SOApprDateFrom { get; set; }
    public DateTime? SOApprDateTo { get; set; }
    /// <summary>Ngày kết thúc **ban đầu** (`SOApprDateToInit`) — giữ lại khi chương trình được gia hạn.</summary>
    public DateTime? SOApprDateToInit { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    // --- ⛔ Bốn cột dưới đây KHÔNG có ở nguồn (port cũ hiểu sai). Giữ để đọc dữ liệu cũ, không ghi mới. ---
    public string ModelCode { get; set; } = "";
    public string Period { get; set; } = "";   // YYYYMM
    public int Qty { get; set; }
    public int UsedQty { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Hạn mức phân bổ theo SPEC (`Mng_Quota`) — khoá kép (DealerCode, SpecCode).
/// Nguồn: `Mng_Quota_UpdMultiX_New20230306` (0.01.Master.cs:6158) — **chỉ có ở WS 64-bit**.
/// 🔴 Đây mới là bảng "hạn mức số lượng" thật; `Mst_Quota` là chương trình điều kiện–khuyến mãi.
/// </summary>
/// <summary>
/// Tỉ lệ tối đa được duyệt đơn hàng theo (đại lý, dòng xe) (`Mst_SORateMax`) —
/// nguồn DMS40/0.01.Master.cs (csproj 122), `Mst_SORateMax_AddMultiX` (6936) ghi tại 7120.
/// 🔴 Chỉ có ở WS 64-bit (`_biz.Mst_SORateMax_Get` / `_AddMulti`).
/// Nguồn CHỈ THÊM MỚI: `Mst_SORateMax_CheckDB(…, TConst.Flag.No)` bắt cặp (đại lý, model)
/// **phải CHƯA tồn tại**; đại lý và model đều phải tồn tại và đang Active; `Rate` không được âm.
/// </summary>
// ========== LOG SỬA MỐC NGÀY CỦA ĐƠN HÀNG + CẤU HÌNH CHẠY JOB (#148) ==========
// Nguồn log: DataWH/Biz.HTC.WH.My.cs (csproj **273**, md5 7ef389d0… — verify 2 máy ở #142)
//   `Ord_SalesOrder_UpdateMulti` (19901) ghi 2 bảng log tại 20466 / 20538.
// Nguồn cấu hình job: DataWH/BizHTC.zTemp.cs (csproj **276**, md5 dbb71f7d…) tại 56576.
// 🔴 Cả hai cụm **chỉ có ở WS 64-bit** (`_biz.Ord_SalesOrder_UpdateMulti`,
//    `_biz.Ord_SalesOrder_SupportHistory`, `_biz.Mst_SettingRunJob_Get/_Save`).
//
// Mô hình log: **cặp Old/New cho từng MỐC NGÀY** (motif đã gặp ở #91–99) — bảng log lưu song song
// giá trị trước và sau, nên tra được "ai đổi ngày duyệt từ bao giờ sang bao giờ".

/// <summary>
/// Log sửa mốc ngày ở ĐẦU đơn hàng (`Ord_SalesOrder_SupportLog`).
/// Chụp cặp Old/New của **hai mốc duyệt** `ApprovedDate1`/`ApprovedDate2`.
/// </summary>
// ========== HẠ TẦNG PHIÊN · CHỐNG TRÙNG LỆNH · LOG TÍCH HỢP NGÂN HÀNG (#149) ==========
// Nguồn: BizHTC.System.cs (csproj **133**, md5 b605132a… khớp 2 máy) — `Sys_SessionHist_AddX` (2269)
//        BizHTC.Common.cs (csproj **109**) — `myUtils_ValidateId` / `_WH`
//        BankIntergration/BizHTC.MBBank.cs (csproj 310, md5 ec9f1442… khớp 2 máy)
// ⚠️ BƯỚC 3B: `BizHTC.Common.cs` **md5 KHÁC nhau giữa laptop và máy 150** (46c01560 / 68adf4ce,
//    3888 vs 3905 dòng) — đã đối chiếu riêng vùng `Sys_ValidateId`: **nội dung khớp**, chỉ lệch số dòng
//    (laptop 534/559, máy 150 551/576) và cùng modifier `public`/`private`.
// TWIN: `_biz.Sys_SessionHist_Add_New20181115` được **CẢ HAI** WS gọi ⇒ không lệch.

/// <summary>Lịch sử phiên đăng nhập dịch vụ (`Sys_SessionHist`).</summary>
// ========== CẤU HÌNH ĐẦU VÀO THUẬT TOÁN MAP VIN (#150) ==========
// Nguồn: DMS40/zTemp.0.20.MapVIN.cs (csproj <Compile> **127**, md5 b903cd2f… khớp nguyên file 2 máy)
//   `Config_MapVINCarCarInput_AddX` (74709) ghi 2 bảng tại 75082 / 75111;
//   `_Update` (75301) → `_UpdateX` (75421).
// 🔴 TWIN: cả cụm **chỉ có ở WS 64-bit** (`_Add` / `_Get` / `_Update`); WS 32-bit không có hàm nào.
//
// Bản chất: bộ **tham số đầu vào** cho thuật toán chia/map VIN — quyết định xe của hợp đồng nào được
// map trước, dựa trên **% đã đặt cọc** và **có bảo lãnh hay không**, tách theo phương thức thanh toán.

/// <summary>
/// Bộ cấu hình map VIN theo dòng xe, có **khoảng hiệu lực** (`Config_MapVINCarCarInput`).
/// 🔴 Ba luật của nguồn (`_AddX`):
/// · `EffDateStart` **bắt buộc**, và phải **≥ NGÀY MAI** — nguồn so với `dtimeSys.AddDays(1)`
///   ⇒ **không cho cấu hình có hiệu lực ngay hôm nay hoặc lùi về quá khứ**;
/// · `EffDateEnd` khi thêm luôn = `TConst.DateTimeSpecial.DateMax` = **"2100-01-01"** (vô hạn);
/// · không được trùng cặp (`ModelCode`, `EffDateStart`) trên bản ghi đang Active.
/// 🔴 `_Update` KHÔNG sửa nội dung: nguồn chặn nếu `FlagActive` khác `Inactive`
///   ⇒ "sửa" ở đây thực chất **chỉ là HUỶ HIỆU LỰC** (tắt cấu hình), khoá theo (CfgATMVIpCode, ModelCode).
/// </summary>
// ========== ĐỊNH NGHĨA GÓI BẢO TRÌ THEO DÒNG XE + KẾT QUẢ THEO XE (#151) ==========
// Nguồn: DataWH/Biz.HTC.WH.cs (csproj **272**) — `Mst_MaintainType_Add_New20181119` (7711) ghi 3 bảng
//        tại 8068 / 8093 / 8119; `_Update_New20181119` (8213) ghi lại 2 bảng con tại 8602 / 8628.
//        StorageFG/BizHTC.StorageFG.Frm.cs (csproj **151**, md5 d92ec381… verify 2 máy ở #138)
//        — `StoF_Maintain_Save_New20181115` (106) / `_SaveEval_New20181115` (1011) ghi `StoF_MaintainMix`.
// ⚠️ BƯỚC 3B: `Biz.HTC.WH.cs` lệch số dòng giữa 2 máy (212 978 vs 212 983) nhưng **vùng 7711–8660
//    md5 KHỚP HOÀN TOÀN** (1354fb06…) — dùng đúng quy trình so-vùng của C0-centesimussexagesimusseptimus.
// TWIN: cả hai WS đều gọi đủ bộ `_Add/_Update/_Delete/_Get_New20181119` ⇒ **không lệch**.
//
// 🔴 Phân biệt hai bảng tên gần giống — đã suýt nhầm ở BƯỚC 2:
//    `Mst_MaintainTaskItem`  = DANH MỤC hạng mục (đã port, entity `MstMaintainTaskItem`);
//    `MtnTp_MaintainTaskItem` = bảng NỐI "gói bảo trì ↔ hạng mục" — **chưa port**, chính là bảng dưới đây.

/// <summary>
/// Gói bảo trì theo dòng xe (`Mst_MaintainType`) — khoá kép (`MtnTp`, `ModelCode`).
/// Nguồn khi thêm bắt cặp khoá **phải CHƯA tồn tại** (`Mst_MaintainType_CheckDB(…, TConst.Flag.No)`).
/// </summary>
// ========== BA BẢNG VỆ TINH CÒN THIẾU CỦA CÁC CỤM ĐÃ PORT (#152) ==========
// Nguồn: TCFIntergration/BizHTC.TCFIntergration.cs (csproj **328**, md5 a4c9ec29… khớp 2 máy) — 1564 / 5412
//        BizHTC.Storage.DlvMinutes.cs            (csproj **120**, md5 0b3b957d… khớp 2 máy) — 4804 / 8698
//        HDDTIntergration/BizHTC.HDDTIntergration.cs (csproj **282**) — `Seq_Invoice_PrintNo` (20836) ghi tại 20906
// ⚠️ BƯỚC 3B: file HDDT **md5 cả file lệch 2 máy** (48140bda / 2933a7fa) nhưng **cùng 24 770 dòng** và
//    **vùng 20800–21000 md5 KHỚP** (444cc59f…) ⇒ dùng bản laptop, đúng quy trình so-VÙNG (C0-…septuagesimusquartus).

/// <summary>
/// Lịch sử file đính kèm thư bảo lãnh (`Pmt_GuaranteeAttachFileHis`).
/// 🔴 Là bảng **KHÁC** <see cref="PmtGuaranteeAttachFile"/> (bản hiện hành, đã port trước) — cùng bộ cột
/// nhưng lưu các bản đã bị thay thế. Nguồn ghi từ tích hợp TCF (`BizHTC.TCFIntergration.cs` 1564 / 5412).
/// ⚠️ Cột `AutoId` trong danh sách `insert` **đã bị comment** (`--AutoId,`) ⇒ để DB tự sinh, không port.
/// </summary>
public sealed class PmtGuaranteeAttachFileHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GuaranteeNo { get; set; } = "";
    public int FileIndex { get; set; }
    public string? GrtFilePath { get; set; }
    public string? GrtFileName { get; set; }
    /// <summary>Kích thước file tính bằng BYTE (`FileSizeInBytes`) — tên cột nói rõ đơn vị.</summary>
    public long FileSizeInBytes { get; set; }
    public string? GrtFileRemark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Vị trí GPS lúc KẾT THÚC giao xe theo biên bản (`GPS_DlvMinutesAddress`) — bảng vệ tinh của cụm
/// biên bản giao xe đã port. Nguồn: `BizHTC.Storage.DlvMinutes.cs` 4804 / 8698;
/// WS gọi `_biz.GPS_DlvMinutesAddress_UpdateAuto_New20181119` (job tự cập nhật).
/// </summary>
public sealed class GpsDlvMinutesAddress
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlvMnNo { get; set; } = "";
    public string? StorageCode { get; set; }
    public string? GPSDvNo { get; set; }
    public string VIN { get; set; } = "";
    /// <summary>Điểm nhận xe đã đăng ký (`PointRegisCode`) — đối chiếu với toạ độ thực tế bên dưới.</summary>
    public string? PointRegisCode { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? DlvEndGPSDateTime { get; set; }
    public decimal? MapLongitude { get; set; }
    public decimal? MapLatitude { get; set; }
    /// <summary>Địa chỉ do dịch vụ GPS trả về (`GPSAddress`).</summary>
    public string? GPSAddress { get; set; }
    /// <summary>Trạng thái dữ liệu GPS (`GPSStatus`).</summary>
    public string? GPSStatus { get; set; }
    /// <summary>Trạng thái của LẦN GỌI dịch vụ GPS (`CallGPSStatus`) — tách riêng khỏi <see cref="GPSStatus"/>.</summary>
    public string? CallGPSStatus { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Bộ CẤP SỐ IN hoá đơn (`Seq_Invoice_PrintNo`) — nguồn `Seq_Invoice_PrintNo` (HDDTIntergration:20836).
/// Khoá nghiệp vụ: bộ ba (`InvoiceIDType`, `SourceInvoiceCode`, `YearPrint`).
/// 🔴 BẪY TÊN CỘT: `YearPrint` **KHÔNG phải năm** — nguồn gán `dtimeTDate.ToString("yyyy-MM-dd")`
///    ⇒ giá trị là **NGÀY đầy đủ**, tức bộ đếm được cấp lại **THEO NGÀY**, không phải theo năm.
/// 🔴 Cách cấp số của nguồn (không phải identity DB):
///    · chưa có dòng cho bộ ba ⇒ `insert … select … left join … where f.InvoiceIDType is null` với `LastPrintNo = '0'`;
///    · đọc `LastPrintNo`, **+1**, rồi `update` ghi lại;
///    · số in trả về ghép theo `string.Format("{0:000}.{1}/{2}/{3}", next, ddMMyy, "19HTC", hậu tố)`
///      — `"19HTC"` là **mẫu số fix cứng trong code** (nguồn ghi chú: "Nguyễn Kiều Ngân báo fix cứng"),
///      hậu tố: `SourceInvoiceCode` = INVOICEREPLACE ⇒ "BBTHHĐ", = INVOICEADJ ⇒ "BBĐCHĐ", còn lại rỗng.
/// </summary>
public sealed class SeqInvoicePrintNo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceIDType { get; set; } = "";
    /// <summary>⚠️ Xem ghi chú lớp: đây là **NGÀY** ("yyyy-MM-dd"), không phải năm.</summary>
    public string YearPrint { get; set; } = "";
    /// <summary>`TConst.SourceInvoiceCode`: "INVOICEROOT" · "INVOICEREPLACE" · "INVOICEADJ".</summary>
    public string SourceInvoiceCode { get; set; } = "";
    /// <summary>Số in cuối đã cấp (`LastPrintNo`) — nguồn khởi tạo bằng chuỗi '0' và đọc bằng `Convert.ToDouble`.</summary>
    public decimal LastPrintNo { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class MstMaintainType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mã gói bảo trì (`MtnTp`).</summary>
    public string MtnTp { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? MtnTpName { get; set; }
    /// <summary>Số lần bảo trì của gói (`MtnTimes`) — nguồn đọc bằng `Convert.ToInt32` ⇒ số NGUYÊN.</summary>
    public int MtnTimes { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Hạng mục thuộc gói bảo trì (`MtnTp_MaintainTaskItem`) — bảng NỐI, không phải danh mục.
/// Mỗi `MtnTkItemCode` nguồn bắt **phải tồn tại và đang Active** (`Mst_MaintainTaskItem_CheckDB`).
/// </summary>
public sealed class MtnTpMaintainTaskItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MtnTp { get; set; } = "";
    public string ModelCode { get; set; } = "";
    /// <summary>Mã gói hạng mục cha (`MtnTkCode`).</summary>
    public string? MtnTkCode { get; set; }
    public string MtnTkItemCode { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Phụ tùng thuộc gói bảo trì (`MtnTp_Part`). Mỗi `PartCode` nguồn bắt **phải tồn tại** (`Mst_Part_CheckDB`).
/// </summary>
public sealed class MtnTpPart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MtnTp { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string PartCode { get; set; } = "";
    /// <summary>Số lượng phụ tùng dùng cho gói (`Qty`).</summary>
    public decimal Qty { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Kết quả bảo trì THEO TỪNG HẠNG MỤC của một xe (`StoF_MaintainMix`) — bảng con thứ hai của phiếu
/// bảo trì, song song với <see cref="StoFMaintainMain"/> (vốn ở mức XE).
/// Nối phiếu (`SF_MtnNo`) + xe (`VIN`) + gói (`MtnTp`,`ModelCode`) + hạng mục (`MtnTkCode`,`MtnTkItemCode`),
/// lưu **giá trị đo được** `MtnVal` và trạng thái riêng `MtnStatusMix`.
/// </summary>
public sealed class StoFMaintainMix
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SF_MtnNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? MtnTp { get; set; }
    public string? ModelCode { get; set; }
    public string? MtnTkCode { get; set; }
    public string? MtnTkItemCode { get; set; }
    /// <summary>Giá trị ghi nhận của hạng mục (`MtnVal`) — ví dụ số đo/kết quả kiểm.</summary>
    public string? MtnVal { get; set; }
    /// <summary>Trạng thái RIÊNG của dòng hạng mục (`MtnStatusMix`) — độc lập với trạng thái phiếu và mức xe.</summary>
    public string? MtnStatusMix { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class ConfigMapVinCarCarInput
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mã bộ cấu hình (`CfgATMVIpCode`).</summary>
    public string CfgATMVIpCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    public DateTime CreateDTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Dòng cấu hình map VIN (`Config_MapVINCarCarInputDtl`) — **không có cột ngày**: khoảng hiệu lực
/// nằm ở bảng đầu, dòng chỉ mô tả điều kiện.
/// </summary>
public sealed class ConfigMapVinCarCarInputDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CfgATMVIpCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    /// <summary>
    /// Phương thức thanh toán của hợp đồng đại lý (`DCPType`).
    /// ⚠️ Nguồn **KHÔNG có class hằng riêng** cho cột này — giá trị đến từ dữ liệu hợp đồng
    /// (`Dlr_Contract.DCPType`, xem `0.34.Contract.cs:795-846`). Cố ý **không suy đoán bảng mã**.
    /// </summary>
    public string? DCPType { get; set; }
    /// <summary>Cận DƯỚI của khoảng % đã đặt cọc (`ValPmtDepositPercentFrom`).</summary>
    public decimal ValPmtDepositPercentFrom { get; set; }
    /// <summary>Cận TRÊN của khoảng % đã đặt cọc (`ValPmtDepositPercentTo`).</summary>
    public decimal ValPmtDepositPercentTo { get; set; }
    /// <summary>Điều kiện "hợp đồng CÓ bảo lãnh hay không" (`FlagIsExistGuarantee`) — cờ "1"/"0".</summary>
    public string? FlagIsExistGuarantee { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class SysSessionHist
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mã phiên — nguồn ghi chú thẳng trên cột: `-- Tid`, tức **SessionId CHÍNH LÀ Tid** của lượt gọi.</summary>
    public string SessionId { get; set; } = "";
    /// <summary>Dịch vụ/người dùng GỐC đứng tên phiên (`RootSvCode`/`RootUserCode`) — khác cặp gọi trực tiếp bên dưới.</summary>
    public string? RootSvCode { get; set; }
    public string? RootUserCode { get; set; }
    public string? ServiceCode { get; set; }
    public string? UserCode { get; set; }
    public string? FunctionName { get; set; }
    public string? LanguageCode { get; set; }
    public DateTime? DateTimeLogin { get; set; }
    /// <summary>Thông tin phía TRONG hệ (`InfoInternal`) và phía NGOÀI (`InfoExternal`) — tách đôi có chủ ý.</summary>
    public string? InfoInternal { get; set; }
    public string? InfoExternal { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Bảng CHỐNG TRÙNG LỆNH (`Sys_ValidateId`) — **đúng MỘT cột**.
/// 🔴 Cơ chế của nguồn rất đáng chú ý: `insert into Sys_ValidateId values (@Id);` **không** SELECT trước.
/// Nếu Id đã tồn tại thì **ràng buộc khoá chính ném lỗi**, và `catch` biến nó thành mã lỗi "Id không hợp lệ"
/// (BizHTC.Common.cs — `myUtils_ValidateId`). Tức là dùng **chính khoá chính làm khoá chống đua**,
/// không dùng "kiểm tra rồi ghi" (vốn hở race condition).
/// ⚠️ Nguồn ghi vào **CẢ HAI** DB: `myUtils_ValidateId` (Main) và `myUtils_ValidateId_WH` (Warehouse).
/// </summary>
public sealed class SysValidateId
{
    /// <summary>Chính là Id chống trùng — nguồn dùng nó làm KHOÁ, không có cột phụ nào khác.</summary>
    public string ValidateId { get; set; } = "";
    public Guid OrgId { get; set; }
}

/// <summary>
/// Log gọi API ngân hàng (`OS_MBankLog`) — ghi từ cả MB Bank và VietinBank.
/// Lưu nguyên văn request/response để đối soát khi ngân hàng báo lệch.
/// </summary>
public sealed class OsMBankLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Thông tin lô/ngữ cảnh gọi (`BulkInfo`).</summary>
    public string? BulkInfo { get; set; }
    /// <summary>⚠️ Tên cột nguồn viết **thường chữ "n"**: `Functionname` (không phải "FunctionName") — giữ 1:1.</summary>
    public string? Functionname { get; set; }
    /// <summary>Nguyên văn request gửi đi (`RQ`).</summary>
    public string? RQ { get; set; }
    /// <summary>Nguyên văn response nhận về (`RS`).</summary>
    public string? RS { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class OrdSalesOrderSupportLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SOCode { get; set; } = "";
    /// <summary>Thời điểm/người sửa (`UpdDTime`/`UpdBy`) — nguồn lấy chính `LogLUDateTime`/`LogLUBy` của lượt sửa.</summary>
    public DateTime? UpdDTime { get; set; }
    public string? UpdBy { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? ApprovedDate1Old { get; set; }
    public DateTime? ApprovedDate1 { get; set; }
    public DateTime? ApprovedDate2Old { get; set; }
    public DateTime? ApprovedDate2 { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Log sửa mốc ngày ở DÒNG đơn hàng (`Ord_SalesOrderDetail_SupportLog`) — khoá dòng là bộ ba
/// (ModelCode, SpecCode, ColorCode), chụp cặp Old/New của **bốn mốc**:
/// duyệt · hạn nghĩa vụ đặt cọc · hạn bảo lãnh · hạn giao xe.
/// ⚠️ Nguồn khai bảng tạm đầu vào chỉ có `SOCode`+`ApprovedDate`; hai dòng
/// `DepositDutyEndDate` và `CarDueDate` **ĐÃ BỊ COMMENT** (Biz.HTC.WH.My.cs:20083-20084)
/// ⇒ hiện tại hàm **chỉ thực sự sửa `ApprovedDate`**; bốn cặp cột log vẫn giữ đủ vì bảng có sẵn.
/// </summary>
public sealed class OrdSalesOrderDetailSupportLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SOCode { get; set; } = "";
    public DateTime? UpdDTime { get; set; }
    public string? UpdBy { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public DateTime? ApprovedDateOld { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? DepositDutyEndDateOld { get; set; }
    public DateTime? DepositDutyEndDate { get; set; }
    public DateTime? GrtEndDateOld { get; set; }
    public DateTime? GrtEndDate { get; set; }
    public DateTime? CarDueDateOld { get; set; }
    public DateTime? CarDueDate { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Cấu hình bật/tắt từng JOB nền (`Mst_SettingRunJob`) — nguồn DataWH/BizHTC.zTemp.cs:56576.
/// Chỉ 3 cột nghiệp vụ: mã job, tên job, cờ bật. Là công tắc để tắt job mà không phải sửa lịch.
/// </summary>
public sealed class MstSettingRunJob
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string JobCode { get; set; } = "";
    public string? JobName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class MstSoRateMax
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    /// <summary>Tỉ lệ tối đa (`Rate`) — nguồn khai kiểu `float`, chỉ chặn `< 0` (không chặn > 1).</summary>
    public decimal Rate { get; set; }
    /// <summary>Cờ hiệu lực — nguồn ép cứng `TConst.Flag.Active` khi thêm.</summary>
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class MngQuota
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    /// <summary>Số lượng hạn mức (`QtyQuota`) — nguồn khai kiểu `float` trong bảng tạm.</summary>
    public decimal QtyQuota { get; set; }
    /// <summary>
    /// Cờ hiệu lực (`FlagActive`). ⚠️ Lệnh update của nguồn **luôn ép về "1"** —
    /// tức mọi lần sửa hạn mức đều **BẬT LẠI** dòng đang tắt.
    /// </summary>
    public string FlagActive { get; set; } = "1";
    public DateTime? UpdateDTime { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Lịch sử hạn mức (`Mng_QuotaHis`) — ảnh chụp dòng `Mng_Quota` **SAU** mỗi lần sửa.
/// 🔴 `VesionCode` (nguồn viết **thiếu chữ "r"**, giữ nguyên để port 1:1) = **chính chuỗi thời gian**
/// `LogLUDateTime` của lượt sửa ⇒ mỗi lượt sửa là một "phiên bản" định danh bằng mốc thời gian
/// (cùng motif với `VersionDTimeCurr` ở #130).
/// 🔴 `FunctionName` ghi TÊN HÀM đã gây ra thay đổi (`"Mng_Quota_UpdMultiX"`) — dấu vết nguồn gốc.
/// </summary>
public sealed class MngQuotaHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? VesionCode { get; set; }
    public string DealerCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public decimal QtyQuota { get; set; }
    public string? FlagActive { get; set; }
    public string? FunctionName { get; set; }
    public DateTime? UpdateDTime { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (ca thứ 4, phát hiện #57 bằng sweep tên bảng nguồn).
/// `MortgageRequest`/`MortgageCar` và <see cref="ReqMortgage"/>/<see cref="ReqMortgageCar"/>
/// **cùng map một bảng nguồn `RM_ReqMortgage`/`RM_ReqMortgageDtl`**.
/// Endpoint `/api/mortgages` đã trỏ sang <see cref="ReqMortgage"/>. Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
/// </summary>
public sealed class MortgageRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqRMNo { get; set; } = "";
    public string BankCode { get; set; } = "";     // Mst_Bank.FlagMortageBank='1'
    public string Status { get; set; } = "Pending"; // Pending → Approved → Finished (giải chấp)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}

/// <summary>Dòng xe trong đề nghị thế chấp (RM_ReqMortgageDtl): 1 VIN + trạng thái dòng.</summary>
public sealed class MortgageCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? EngineNo { get; set; }
    public string DtlStatus { get; set; } = "Pending";  // Pending → Approved → Finished (theo header)
}

/// <summary>Phiếu chi / thanh toán (Pmt_Payment — port 1:1 FrmNewPM/FrmMngPM):
/// header phiếu chi cho đại lý qua chuyển khoản. Pending → Approved / Rejected.</summary>
public sealed class PmtVoucher
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PMNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? BankAccountSend { get; set; }     // TK chuyển
    public string? BankAccountReceive { get; set; }  // TK nhận
    public decimal TotalAmount { get; set; }          // = Σ AmountCurrent các dòng
    public string Status { get; set; } = "Pending";   // Pending → Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }
}

/// <summary>Dòng phiếu chi (PMDetail): 1 chứng từ tham chiếu + lũy kế + chi kỳ này.
/// AmountTotal = AmountAccum + AmountCurrent (port đúng công thức FrmNewPM).</summary>
public sealed class PmtLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long VoucherId { get; set; }
    public string RefNo { get; set; } = "";           // số HĐ/VIN được chi
    public decimal AmountAccum { get; set; }          // lũy kế đã chi trước đó
    public decimal AmountCurrent { get; set; }        // chi kỳ này
}

/// <summary>Bảo lãnh / thư tín dụng ngân hàng (Guarantee — port 1:1 FrmNewGrt/FrmMngGrt + FrmEditGrtExpiredDate/EndDate):
/// bảo lãnh NH cho lô xe nhập. GrtType: BL(Bảo lãnh)/LCTC(LC trả chậm)/LCUP(LC Upas)/EPLC. Pending→Approved.</summary>
public sealed class Guarantee
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GrtNo { get; set; } = "";          // số nội bộ (GetGrtNo)
    public string? BankGrtNo { get; set; }           // số bảo lãnh do NH cấp
    public string DealerCode { get; set; } = "";     // đại lý chủ bảo lãnh (TblGuarantee.DealerCode)
    public string BankCode { get; set; } = "";
    public string GrtType { get; set; } = "BL";      // BL/LCTC/LCUP/EPLC
    public decimal GrtValue { get; set; }            // giá trị bảo lãnh
    public DateTime GrtDate { get; set; } = DateTime.Now;    // ngày phát hành
    public DateTime? DateExpired { get; set; }       // ngày hết hạn
    public string Status { get; set; } = "Pending";  // Pending → Approved
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }

    // ===== #160 parity + side-effect `RD_ReqInvoiceDtlApprove_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:128014, csproj 272; vùng md5 1e58bf10 khớp 2 máy) =====
    /// <summary>
    /// 🔴 Ngân hàng GIÁM SÁT (`Pmt_Guarantee.BankCodeMonitor`) — khi duyệt đề nghị giao hồ sơ,
    /// mã này được ghi sang `Car_Vin.HandOverBankCode` (ngân hàng nhận bàn giao hồ sơ xe).
    /// Khác <see cref="BankCode"/> (ngân hàng phát hành bảo lãnh).
    /// </summary>
    public string? BankCodeMonitor { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Danh sách hoá đơn xuất bán — port 1:1 FrmNewInvoice/FrmMngInvoice; header 1 lô hoá đơn (nhập Excel A2).
/// 🔴 **#126 đối chiếu bảng nguồn `Car_InvoiceList`** (2010.HTC `Biz.HTC.WH.cs:109183`,
/// hàm `CarInvoiceListCreate_New20181119` (109011); còn `CarInvoiceListDelete_New20181119`,
/// `Car_InvoiceList_Get` và `_GetWH`).
/// TWIN: đã diff TOÀN BỘ danh sách hàm ở cả hai WS — **4 hàm khớp hoàn toàn** (WS64 có thêm
/// `Pmt_Payment_InvoiceList_Get` nhưng đó là bảng `Pmt_*`, khác cụm).
/// GAP đã vá ở #126: bổ sung `CreatedBy`.
/// ⚠️ Nguồn ghi **cả `_dbMain` lẫn `_dbWH`** (109183-109185) — nợ `_dbWH` chung fleet
/// (đã kiểm dòng `_dbWH` **không bị comment**, theo luật C0-centesimusvigesimusquintus).
/// </summary>
public sealed class InvoiceList
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceListCode { get; set; } = "";   // số tham chiếu (GetInvoiceListNo)
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    /// <summary>#126: nguồn có `CreatedBy` (dòng 109155) — port cũ thiếu.</summary>
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Dòng hoá đơn (`Car_InvoiceListDetail` — 2010.HTC `Biz.HTC.WH.cs:109203`).
/// 🔴 GAP #126 (1): nguồn khoá dòng bằng **`InvoiceListCode`** (số tham chiếu, kiểu chuỗi), port cũ chỉ
/// có `ListId` (khoá nội bộ tự sinh) ⇒ **không map được dữ liệu thật** khi import từ SQL 228.
/// Nay giữ cả hai.
/// 🔴 GAP #126 (2): cột đại lý của nguồn tên **`InvoiceDealerCode`**, port cũ đặt `DealerCode` —
/// giữ tên cũ để không phá API nhưng bổ sung cột đúng tên nguồn.
/// ⚠️ Nguồn **KHÔNG ghi VIN** vào bảng chi tiết (chỉ `CarId`); cột `Vin` là của bản port cũ, giữ nguyên.
/// 🔴 Guard nguồn quan trọng: `myCar_CheckInvoiceListDetail(..., Flag.Inactive, ...)` (dòng 109155-109159)
/// ⇒ **một xe chỉ được nằm trong ĐÚNG MỘT danh sách hoá đơn**; xe đã có dòng ở list khác thì bị chặn.
/// </summary>
public sealed class InvoiceLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ListId { get; set; }
    /// <summary>Khoá dòng của nguồn (`Car_InvoiceListDetail.InvoiceListCode`).</summary>
    public string? InvoiceListCode { get; set; }
    public string? CarId { get; set; }
    public string? DealerCode { get; set; }
    /// <summary>Tên cột đúng theo nguồn (`InvoiceDealerCode`).</summary>
    public string? InvoiceDealerCode { get; set; }
    public string InvoiceNo { get; set; } = "";
    public string Vin { get; set; } = "";
    public DateTime? InvoiceDate { get; set; }
}

/// <summary>Biên bản bàn giao theo hối phiếu NH (Car_BankBillMinutes — port 1:1 FrmTaoBBBGTheoHoiPhieu/FrmQuanLyBBBGTheoHoiPhieu):
/// bàn giao lô xe cho ngân hàng theo hối phiếu/LC. Created → Received (nhận hối phiếu).</summary>
public sealed class BankBillMinutes
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BankBillMnNo { get; set; } = "";      // số BBBG
    public string BankCode { get; set; } = "";
    public DateTime? BankBillDate { get; set; }         // ngày hối phiếu
    public DateTime? BankBillReciveDate { get; set; }   // ngày nhận hối phiếu
    public string Status { get; set; } = "Created";     // Created → Received
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}

/// <summary>Dòng xe trong BBBG (Car_BankBillMinutesDtl): VIN + LC/bảo lãnh + số tiền claim.</summary>
public sealed class BankBillCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long BillId { get; set; }
    public string Vin { get; set; } = "";
    public string? EngineNo { get; set; }
    public string? LCNo { get; set; }                   // số bảo lãnh/LC (BankGuaranteeNo)
    public string? GuaranteeBankCode { get; set; }
    public decimal ClaimAmount { get; set; }
}

/// <summary>Yêu cầu vận chuyển xe (TransportRequest — port 1:1 FrmNewTransportRequest/FrmMngTransportRequest, Phase2):
/// đại lý + nhà vận chuyển + lô xe cần chở. Pending(Đang xử lý)→Approved(Phê duyệt)/Rejected(Từ chối).</summary>
public sealed class TransportRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TranspReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string TransporterCode { get; set; } = "";
    public string? TransContractNo { get; set; }
    /// <summary>
    /// Trạng thái yêu cầu (`Car_TransportReq.TransportReqStatus`) — mã nguồn dùng "P" chờ duyệt → "A" đã duyệt.
    /// 🔴 #142: trước đây port ghi "Pending"/"Approved"/"Rejected" (từ vựng tự chế) ⇒ đã đưa về mã nguồn,
    /// kèm UPDATE di trú dữ liệu cũ trong Seeder.
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    /// <summary>Ngày duyệt (`ApprovedDate`).</summary>
    public DateTime? DecidedAt { get; set; }

    // --- #142 parity Car_TransportReq ---
    public string? CreatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng xe trong YC vận chuyển (TranspReqDtl): VIN + DO + màu + kho.</summary>
public sealed class TransportReqCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string Vin { get; set; } = "";
    /// <summary>Số lệnh giao xe (`DeliveryOrderNo`).</summary>
    public string? DoNo { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — `Car_TransportReqDetail` nguồn KHÔNG có màu/kho.</summary>
    public string? ColorCode { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — xem ghi chú ở <see cref="ColorCode"/>.</summary>
    public string? StorageCode { get; set; }

    // --- #142 parity Car_TransportReqDetail ---
    /// <summary>🔴 Khoá DÒNG XE thật của nguồn (`CarId`) — nguồn định danh xe bằng CarId, không bằng VIN.</summary>
    public string? CarId { get; set; }
    /// <summary>Trạng thái dòng (`TransportReqDtlStatus`) — bám theo phiếu.</summary>
    public string TransportReqDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phí vận chuyển theo tuyến (Mst_TranspFee — port 1:1 FrmNewTranspFee/FrmMngTranspFee, Phase2):
/// ma trận phí tỉnh/huyện From→To + nhà VC + model → giá phí + số ngày dự kiến.</summary>
public sealed class TranspFee
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ProvinceCodeFrom { get; set; } = "";
    public string ProvinceCodeTo { get; set; } = "";
    public string? DistrictCodeFrom { get; set; }
    public string? DistrictCodeTo { get; set; }
    public string TransporterCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public decimal ValFee { get; set; }
    public int ExpectedDays { get; set; }
    public string? TFVCode { get; set; }   // phiên bản CPVT (batch) — port FrmMngTranspFeeHist
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // ===== #169 Mst_TranspPenaltyVer — bản biểu PHẠT trễ hạn =====
    /// <summary>Mức phạt ngày trễ cuối (`ValBased`) và mức cộng thêm cho mỗi ngày trễ sớm hơn (`ValEx`).
    /// Nguồn để hai giá trị này ở bảng RIÊNG `Mst_TranspPenaltyVer` (lấy `top 1` theo `FlagActive='1'`);
    /// MiniHTC gộp vào cùng bảng biểu phí và đánh dấu bằng `FlagPenaltyVer` — ánh xạ 1:1, ghi rõ để không lẫn.</summary>
    public decimal ValBased { get; set; }
    public decimal ValEx { get; set; }
    public string? TPVCode { get; set; }
    /// <summary>"1" = dòng này là **bản biểu phạt** đang hiệu lực (thay cho `Mst_TranspPenaltyVer.FlagActive`).</summary>
    public string? FlagPenaltyVer { get; set; }
}

/// <summary>Biên bản vận chuyển / giao nhận (TransportMinutes — port 1:1 FrmNewTransportMinutes/FrmMngTransportMinutes, Phase2):
/// biên bản giao nhận lô xe do nhà VC chuyển tới đại lý. Pending(Đang xử lý)→Approved(Phê duyệt)/Rejected(Từ chối).</summary>
public sealed class TransportMinutes
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TransportMinutesNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>⚠️ Cột riêng MiniHTC — bảng đầu `Car_TransportMinutes` nguồn KHÔNG có nhà vận chuyển.</summary>
    public string TransporterCode { get; set; } = "";
    /// <summary>
    /// 🔴 TRỤC TỔNG (`TransportMinutesStatus`, `TConst.TransportMinutesStatus`): "P" → "A" · "C" huỷ.
    /// Đây chỉ là MỘT trong BA trục — xem <see cref="DLTransportMinutesStatus"/> và
    /// <see cref="HTCTransportMinutesStatus"/>. Trước #142 port gộp cả ba vào đây với từ vựng tự chế.
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }

    // ===== #142 parity Car_TransportMinutes — BA TRỤC TRẠNG THÁI, mỗi trục MỘT BẢNG MÃ KHÁC NHAU =====
    // Đọc từ chữ ký `Car_TransportMinutes_CheckDB(…, strDLTransportMinutesStatusListToCheck,
    //   strHTCTransportMinutesStatusListToCheck, strTransportMinutesStatusListToCheck, …)`
    //   (Biz.HTC.WH.cs:53611) — luật C0-centesimustricesimusnonus.
    /// <summary>Trục ĐẠI LÝ (`DLTransportMinutesStatus`): chỉ **"P" → "A"** (2 giá trị, không có huỷ).</summary>
    public string DLTransportMinutesStatus { get; set; } = "P";
    /// <summary>Trục HTC (`HTCTransportMinutesStatus`): **"P" → "A1" → "A2" · "C"** (4 giá trị).</summary>
    public string HTCTransportMinutesStatus { get; set; } = "P";

    /// <summary>Ngày biên bản (`TransportMinutesDate`).</summary>
    public DateTime? TransportMinutesDate { get; set; }
    /// <summary>File biên bản đã ký (`FilePath`) — nhánh `_HTCAppr2AndSignAndSendMail` ghi.</summary>
    public string? FilePath { get; set; }
    public DateTime? DLCreatedDateTime { get; set; }
    public string? DLCreatedBy { get; set; }
    public DateTime? DLApprDateTime { get; set; }
    public string? DLApprBy { get; set; }
    public DateTime? HTCAppr1DateTime { get; set; }
    public string? HTCAppr1By { get; set; }
    public DateTime? HTCAppr2DateTime { get; set; }
    public string? HTCAppr2By { get; set; }
    public DateTime? HTCCancelDateTime { get; set; }
    public string? HTCCancelBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng xe trong BB vận chuyển (TransportMinutesDetail): VIN + DO + màu + trạng thái dòng.</summary>
public sealed class TransportMinutesCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MinutesId { get; set; }
    public string Vin { get; set; } = "";
    public string? DoNo { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — `Car_TransportMinutesDetail` nguồn không có màu/số máy.</summary>
    public string? ColorCode { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — xem ghi chú ở <see cref="ColorCode"/>.</summary>
    public string? EngineNo { get; set; }
    /// <summary>
    /// Trạng thái dòng (`TransportMinutesDtlStatus`, `TConst.TransportMinutesDtlStatus`): "P" · **"A"** · "R".
    /// 🔴 BẪY TÊN-vs-GIÁ-TRỊ: hằng tên là **`Approve1`** nhưng giá trị là **"A"**, KHÔNG phải "A1"
    /// (khác hẳn trục HTC ở bảng đầu, nơi "A1" là giá trị thật).
    /// </summary>
    public string DtlStatus { get; set; } = "P";

    // --- #142 parity Car_TransportMinutesDetail ---
    /// <summary>🔴 Khoá dòng xe thật của nguồn (`CarId`).</summary>
    public string? CarId { get; set; }
    /// <summary>Huỷ ở MỨC TỪNG XE (`CancelDateTime`/`CancelBy`) — độc lập với huỷ cả biên bản.</summary>
    public DateTime? CancelDateTime { get; set; }
    public string? CancelBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Lịch ngày làm việc/nghỉ (Holiday — port 1:1 FrmCreateHoliday/FrmMngHoliday, Phase2):
/// mỗi ngày 1 cờ IsHoliday. Reset năm sinh cuối tuần = nghỉ; toggle từng ngày.</summary>
public sealed class Holiday
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public DateTime HolidayDate { get; set; }   // 1 dòng / ngày
    public bool IsHoliday { get; set; }
    public string? Description { get; set; }
}

/// <summary>Kế hoạch vận chuyển xe từ kho (Sto_TranspPlan — port 1:1 FrmMngPlanTransport/FrmListPlanTransport, Phase2):
/// dòng KH chuyển xe kho→đại lý. Pending → Finished (khi duyệt StoTranspPlanApproved).</summary>
public sealed class TransportPlan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? CarId { get; set; }
    public string VINPlan { get; set; } = "";        // VIN kế hoạch (khoá duyệt)
    public string? Vin { get; set; }                 // VIN thực (khi đã gán)
    public string ModelCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? StorageCode { get; set; }
    public string? FProvinceCode { get; set; }       // từ tỉnh
    public string? TProvinceCode { get; set; }       // đến tỉnh
    /// <summary>Từ huyện (`FDistrictCode`) — nguồn BẮT BUỘC khi nhà vận chuyển duyệt.</summary>
    public string? FDistrictCode { get; set; }
    /// <summary>Đến huyện (`TDistrictCode`) — nguồn BẮT BUỘC khi nhà vận chuyển duyệt.</summary>
    public string? TDistrictCode { get; set; }
    public string? TransporterCode { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string Status { get; set; } = "Pending";  // Pending → Finished
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// 🔴 TRỤC TRẠNG THÁI THỨ HAI — phía NHÀ VẬN CHUYỂN duyệt (`Sto_TranspPlan.TransporterStatus`),
    /// **ĐỘC LẬP** với <see cref="Status"/> (duyệt nội bộ HTC):
    /// "P" chờ nhà vận chuyển duyệt → **"F" nhận chở** (`TConst.Stage.Finished`) · **"D" từ chối** (`Stage.Decline`).
    /// Nguồn: `TERP.BizTransporter/Report.cs:1403` — hệ `ERP.V15.DMSSales.Real`, **chỉ có trên máy 150**.
    /// Port cũ chỉ có 1 trục ⇒ **không biết nhà vận chuyển đã nhận chở hay đã từ chối**.
    /// </summary>
    public string TransporterStatus { get; set; } = "P";
    /// <summary>Thời điểm nhà vận chuyển duyệt/từ chối (`TransporterAppDate`).</summary>
    public DateTime? TransporterAppDate { get; set; }
    /// <summary>Người của nhà vận chuyển thao tác (`TransporterAppBy`).</summary>
    public string? TransporterAppBy { get; set; }
}

/// <summary>Yêu cầu vận chuyển thu hồi xe (StoTranspReq/retrieve — port 1:1 FrmNewRetrieveTransReq/FrmMngRetrieveTransReq, Phase2):
/// yêu cầu chở xe thu hồi từ đại lý về kho. Pending(Đang xử lý)→Approved(Phê duyệt)/Rejected(Từ chối).</summary>
public sealed class RetrieveRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TranspReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string TransporterCode { get; set; } = "";
    public string? Reason { get; set; }               // lý do thu hồi
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }
    public string TranspReqType { get; set; } = "Retrieve"; // Retrieve|StorageRearrCB|StorageRearrange — port FrmMngRearCBTranspReq/FrmMngRearrangeTranspReq (dùng chung bảng StoTranspReq)

    // ===== #156 parity Sto_TranspReq (nguồn: DataWH/Biz.HTC.WH.cs, csproj 272) —
    //       `Sto_TranspReq_Create_New20181119` (laptop 109410 / máy 150 109415) ghi tại 110501 / 110506.
    // TWIN: cả hai WS gọi cùng bộ `_Create/_Approve/_Del/_Get/_GetWH_New20181119` ⇒ không lệch.
    /// <summary>Số hợp đồng vận chuyển (`TransportContractNo`).</summary>
    public string? TransportContractNo { get; set; }
    public string? CreatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    /// <summary>
    /// ⚠️ #156 — cột RIÊNG MiniHTC. 🔴 Ở NGUỒN `TranspReqType` nằm ở **BẢNG CHI TIẾT**
    /// (<see cref="RetrieveReqCar.TranspReqType"/>), tức **mỗi XE một loại** — một phiếu có thể trộn
    /// nhiều loại. Port cũ nâng nó lên bảng đầu (một loại cho cả phiếu) ⇒ lệch mô hình.
    /// Giữ lại để đọc dữ liệu cũ và làm giá trị mặc định khi tạo dòng.
    /// </summary>
    public string? TranspReqTypeHeaderOnly { get; set; }
}

/// <summary>Dòng xe trong YC thu hồi (StoTranspReqDtl): VIN + kho nhận về.</summary>
public sealed class RetrieveReqCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string Vin { get; set; } = "";
    public string? StorageCode { get; set; }
    public string DtlStatus { get; set; } = "Pending";

    // ===== #156 parity Sto_TranspReqDtl (Biz.HTC.WH.cs:110501) =====
    /// <summary>
    /// 🔴 Loại yêu cầu vận chuyển ở MỨC DÒNG (`TranspReqType`, `TConst.TranspReqType`,
    /// Const.Main.cs:633-640): **"CARTRANSPORT" · "STORAGEREARRANGE" · "CARRETRIEVE" · "STORAGEREARRCB"**.
    /// Đây mới là chỗ nguồn lưu loại — KHÔNG phải ở bảng đầu.
    /// </summary>
    public string? TranspReqType { get; set; }
    /// <summary>Số chứng từ tham chiếu (`RefOrdNo`) — với "CARTRANSPORT" nguồn nối về `DeliveryOrderNo`.</summary>
    public string? RefOrdNo { get; set; }
    /// <summary>
    /// 🔴 Khoá dòng xe (`CarId`). Nguồn RẼ NHÁNH theo loại (Biz.HTC.WH.cs:109610-109619):
    /// loại **STORAGEREARRANGE** hoặc **STORAGEREARRCB** ⇒ đặt `DBNull` (điều chuyển kho **không gắn xe cụ thể**);
    /// các loại còn lại ⇒ lấy từ đầu vào.
    /// </summary>
    public string? CarId { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Trạng thái đóng thùng xe (Car_VIN packing — port 1:1 FrmUpdateVIN_TypeCB → CarVINUpdate_TypeCB, Phase2):
/// mỗi VIN 1 dòng; cập nhật đã đóng thùng (TypeCB='1') + loại thùng + spec thực + số seri + ngày kiểm.</summary>
public sealed class VinPacking
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string TypeCB { get; set; } = "0";        // '1' = đã đóng thùng
    public string? LoaiThung { get; set; }           // loại thùng
    public string? ActualSpec { get; set; }          // spec thực tế
    public string? SerialNo { get; set; }
    public DateTime? InspectionDate { get; set; }     // ngày kiểm định
    public string? AVNCode { get; set; }              // mã AVN (đầu DVD/màn hình) — port FrmUpdateCVActualSpec
    public DateTime? AVNDate { get; set; }
    public string? AVNScreenSerialNo { get; set; }    // số seri màn hình AVN
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Yêu cầu sửa/bảo hành thiết bị GPS (GPSF_GPSClaim — port 1:1 FrmGPSF_GPSClaimNew/FrmGPSF_GPSClaimMng, StoFGPS):
/// 3 chiều trạng thái. Claim: Pending→Approved; Received: ''→Progress→Finished (nhận TB về sửa); Fix: ''→Finished.</summary>
/// <summary>
/// File đính kèm của yêu cầu bảo hành thiết bị GPS (`GPSF_GPSClaimAttachFile`) —
/// nguồn StorageFG/BizHTC.ZTempGPS.cs (csproj **153**, md5 cd3c409e… khớp 2 máy), ghi tại 5900 và 6763.
/// TWIN: cụm `GPSF_GPSClaim*` có ở **CẢ HAI** WS ⇒ không lệch.
/// </summary>
public sealed class GpsClaimAttachFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GPSClaimNo { get; set; } = "";
    public int FileIndex { get; set; }
    public string? GPSFilePath { get; set; }
    public string? GPSFileName { get; set; }
    /// <summary>Loại file (`GPSFileType`) — phân biệt ảnh trước/sau sửa, biên bản…</summary>
    public string? GPSFileType { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class GpsClaim
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GpsClaimNo { get; set; } = "";
    public string GpsDvNo { get; set; } = "";            // số thiết bị GPS
    public string? BeforeFixRemark { get; set; }         // tình trạng trước sửa
    public string? Remark { get; set; }
    public string ClaimStatus { get; set; } = "Pending"; // Pending → Approved
    public string ReceivedStatus { get; set; } = "";     // '' → Progress → Finished
    public string FixStatus { get; set; } = "";          // '' → Finished
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Phiếu nhập kho thiết bị GPS (StoF_GPSIn — port 1:1 FrmStoF_GPSIn/FrmMngStoF_GPSIn, StoFGPS):
/// nhập lô thiết bị GPS vào kho theo loại nhập + kho.</summary>
public sealed class GpsIn
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SFGPSInNo { get; set; } = "";
    /// <summary>
    /// Loại nhập (`GPSInType`) — `TConst.GPSInType` (Const.Main.StorageFG.cs:38-42):
    /// **"FIRST_IN"** nhập lần đầu · **"RE_IN"** nhập lại. 🔴 KHÔNG phải nhãn tuỳ ý:
    /// nguồn rẽ nhánh guard tồn kho theo đúng 2 giá trị này (xem endpoint POST /api/gpsins).
    /// </summary>
    public string? GpsInType { get; set; }
    public string StorageCode { get; set; } = ""; // kho GPS
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>Trạng thái phiếu (`GPSInStatus`, `TConst.GPSInStatus`): "P" chờ duyệt → "A" đã duyệt.</summary>
    public string GPSInStatus { get; set; } = "P";
    // --- #138 parity StoF_GPS* : TRỤC DUYỆT nguồn ghi mà port cũ thiếu sạch ---
    /// <summary>Người lập (`CreateBy`).</summary>
    public string? CreateBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng thiết bị nhập (StoF_GPSInDtl): số thiết bị + số hộp + trạng thái map.</summary>
public sealed class GpsInDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long InId { get; set; }
    public string GpsDvNo { get; set; } = "";     // số thiết bị GPS
    public string? GpsBoxNo { get; set; }         // số hộp
    /// <summary>
    /// ⚠️ Cột RIÊNG của MiniHTC ('1' = đã gắn lên xe) — `StoF_GPSInDtl` KHÔNG có cột này.
    /// Trước #138 nó bị dùng THAY chỗ `GPSInStatusDtl` (sai ngữ nghĩa); nay giữ lại vì
    /// endpoint /api/gpsouts đang dùng, nhưng trạng thái duyệt đã tách sang `GPSInStatusDtl`.
    /// </summary>
    public string MapStatus { get; set; } = "0";
    /// <summary>
    /// Trạng thái duyệt của DÒNG (`GPSInStatusDtl`) — nguồn đặt "P" khi lưu, và khi duyệt gán
    /// `t.GPSInStatusDtl = f.GPSInStatus` (BizHTC.StorageFG.Frm.cs:4019) ⇒ dòng **luôn bám theo phiếu**.
    /// </summary>
    public string GPSInStatusDtl { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phiếu xuất kho thiết bị GPS (StoF_GPSOut — port 1:1 FrmStoF_GPSOut/FrmMngStoF_GPSOut, StoFGPS):
/// xuất lô thiết bị GPS khỏi kho (để gắn lên xe) cho người nhận.</summary>
public sealed class GpsOut
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SFGPSOutNo { get; set; } = "";
    public string StorageCode { get; set; } = "";
    public string? UserCodeReceived { get; set; }   // người nhận
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>Trạng thái phiếu (`GPSOutStatus`, `TConst.GPSOutStatus`): "P" → "A".</summary>
    public string GPSOutStatus { get; set; } = "P";
    // --- #138 parity StoF_GPS* : TRỤC DUYỆT nguồn ghi mà port cũ thiếu sạch ---
    /// <summary>Người lập (`CreateBy`).</summary>
    public string? CreateBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng thiết bị xuất (StoF_GPSOutDtl): số thiết bị + số hộp + trạng thái map.</summary>
public sealed class GpsOutDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long OutId { get; set; }
    public string GpsDvNo { get; set; } = "";
    /// <summary>⚠️ `StoF_GPSOutDtl` nguồn **KHÔNG có** cột số hộp (chỉ bảng NHẬP mới có) — cột riêng MiniHTC.</summary>
    public string? GpsBoxNo { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — xem ghi chú ở <see cref="GpsInDetail.MapStatus"/>.</summary>
    public string MapStatus { get; set; } = "0";
    /// <summary>Trạng thái duyệt của dòng (`GPSOutStatusDtl`) — bám theo phiếu (…Frm.cs:5042).</summary>
    public string GPSOutStatusDtl { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Địa điểm nhận xe của đại lý (Mst_PointRegis — port 1:1 FrmMst_PointRegis, StoFGPS):
/// geofence điểm giao/nhận xe: toạ độ + bán kính, để đối chiếu GPS xe giao đúng địa điểm.</summary>
public sealed class PointRegis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PointRegisCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string PointRegisName { get; set; } = "";
    public double MapLatitude { get; set; }
    public double MapLongitude { get; set; }
    public double Radius { get; set; }               // bán kính (m)
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tồn/gán thiết bị GPS ↔ VIN (Sto_StoBalanceGPS — port 1:1 FrmMngSto_StoBalanceGPS + FrmUnmapThietBi, StoFGPS):
/// theo dõi thiết bị GPS đang gắn trên xe (VIN) nào, ở đại lý nào. Map (gắn) → Unmap (gỡ).</summary>
public sealed class GpsBalance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GpsDvNo { get; set; } = "";        // số thiết bị (khoá)
    public string? Vin { get; set; }                 // VIN đang gắn
    public string? DealerCode { get; set; }
    public string? DealerName { get; set; }
    public string? Address { get; set; }
    public string? StorageCode { get; set; }
    public DateTime? MapVINDateTime { get; set; }    // ngày gắn GPS vào VIN
    public string Status { get; set; } = "Unmapped"; // Mapped / Unmapped

    // 🔴 #138: `Sto_StoBalanceGPS` có **BA trục trạng thái ĐỘC LẬP**, không phải một.
    //    `Sto_StoBalanceGPS_CheckDB(..., strBlockStatusListToCheck, strInStatusListToCheck, strMapStatusListToCheck)`
    //    nhận ba danh sách RIÊNG ⇒ port cũ gộp hết vào một `Status` (Mapped/Unmapped) là MẤT THÔNG TIN.
    //    Giá trị: "1" Active / "0" Inactive (luật `dmssales-flag-values-1-0-not-yn`).
    /// <summary>Thiết bị bị KHOÁ (`BlockStatus`) — "1" = đang khoá, không cho giao dịch.</summary>
    public string BlockStatus { get; set; } = "0";
    /// <summary>Đang NẰM TRONG KHO (`InStatus`) — "1" sau khi duyệt phiếu nhập, "0" sau khi duyệt phiếu xuất.</summary>
    public string InStatus { get; set; } = "0";
    /// <summary>Đã GẮN lên xe (`MapStatus`) — trục thứ ba, độc lập với hai trục trên.</summary>
    public string MapStatus { get; set; } = "0";
}

/// <summary>Lịch sử gắn/gỡ GPS theo VIN (Sto_StoTransactionGPS — port 1:1 FrmMngVinHistoryMap, StoFGPS):
/// mỗi lần gắn 1 thiết bị lên VIN = 1 dòng (MapDateTime); gỡ ra → set UnMapDateTime. Audit trail của GpsBalance.</summary>
public sealed class GpsTransaction
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string GpsDvNo { get; set; } = "";
    public string? VINAddress { get; set; }
    public DateTime MapDateTime { get; set; } = DateTime.Now;
    public DateTime? UnMapDateTime { get; set; }   // null = đang gắn

    // --- Cột nhật ký của nguồn (`Sto_StoTransactionGPS`, `BizHTC.ZTempGPS.cs:1712-1745`) ---
    // 🔴 Luồng GỠ MAP TỰ ĐỘNG (#50) trước đây **không ghi dòng nhật ký nào** ⇒ mất audit trail.
    /// <summary>Kho của thiết bị lúc phát sinh giao dịch (`StorageCode`).</summary>
    public string? StorageCode { get; set; }
    /// <summary>Số hộp GPS (`GPSBoxNo`).</summary>
    public string? GpsBoxNo { get; set; }
    /// <summary>VIN thật của xe tại thời điểm ghi (`VINReal`).</summary>
    public string? VinReal { get; set; }
    /// <summary>Loại giao dịch (`RefType`) — vd `Sto_StoBalanceGPS_UNMapVIN` cho lượt gỡ map.</summary>
    public string? RefType { get; set; }
    /// <summary>Mã tham chiếu (`RefCode00`) — nguồn ghi **số lô gỡ map** vào đây.</summary>
    public string? RefCode00 { get; set; }
    /// <summary>Tên hàm sinh giao dịch (`FunctionName`, nguồn viết HOA) — dấu vết ai/cái gì tạo dòng này.</summary>
    public string? FunctionName { get; set; }
    /// <summary>Trạng thái map SAU giao dịch (`MapStatus`).</summary>
    public string? MapStatusAfter { get; set; }
    public string? BlockStatus { get; set; }
    public string? InStatus { get; set; }
    public string? UnMapBy { get; set; }
    public string? Remark { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
}

/// <summary>Thanh toán phí AVN (áo vỏ nylon) theo tháng (Pmt_PaymentAVN — port 1:1 FrmTaoThanhToanAVN, 2010.HTC Sales/Purchase):
/// phiếu thu phí phụ kiện AVN theo tháng, nhiều dòng VIN, đơn giá cố định/xe, tổng = Σ UnitPriceAVN.</summary>
public sealed class AvnPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtNo { get; set; } = "";
    public DateTime PmtMonth { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng VIN trong phiếu thanh toán AVN — port 1:1 grid FrmTaoThanhToanAVN, 2010.HTC.</summary>
public sealed class AvnPaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long AvnPaymentId { get; set; }
    public string Vin { get; set; } = "";
    public string? AvnCode { get; set; }
    public DateTime? AvnDate { get; set; }
    public DateTime? InStorageDate { get; set; }
    public string? EngineNo { get; set; }
    public string? SerialNo { get; set; }
    public string? ModelCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public decimal UnitPriceAVN { get; set; }
}

/// <summary>Thanh toán phí GPS theo tháng (Pmt_PaymentGPS — port 1:1 FrmTaoThanhToanGPS/QuanLyThanhToanGPS, 2010.HTC Sales/Purchase):
/// phiếu thu phí duy trì GPS theo tháng, gồm nhiều dòng VIN. Tự tính AmountGPS = PriceGPS × ActualCostGPSDate (ngày tính phí thực = ngày dự kiến − ngày khấu trừ).</summary>
public sealed class GpsPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtNo { get; set; } = "";
    public DateTime PmtMonth { get; set; }
    public decimal TotalWithoutVAT { get; set; }
    public decimal AmountVAT { get; set; }
    public decimal TotalAfterVAT { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng VIN trong phiếu thanh toán GPS — port 1:1 grid FrmTaoThanhToanGPS, 2010.HTC.</summary>
public sealed class GpsPaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long GpsPaymentId { get; set; }
    public string Vin { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecDescription { get; set; }
    public string? GpsId { get; set; }
    public DateTime CostGPSStartDate { get; set; }
    public DateTime CostGPSEndDate { get; set; }
    public int DeductDate { get; set; }        // số ngày khấu trừ (>=0)
    public decimal PriceGPS { get; set; }
    public string? ContractGPS { get; set; }
    public int PlanCostGPSDate { get; set; }    // tự tính = số ngày (start..end) + 1
    public int ActualCostGPSDate { get; set; }  // tự tính = PlanCostGPSDate - DeductDate
    public decimal AmountGPS { get; set; }      // tự tính = PriceGPS * ActualCostGPSDate
}

/// <summary>Đồng bộ ngày xuất kho VIN-GPS sang Veloca (StoF_GPSIn — port 1:1 FrmDongBoNgayXuatKho, 2010.HTC/StoFGPS):
/// import Excel danh sách VIN+GPS ID+ngày xuất kho, đồng bộ sang hệ thống GPS bên thứ 3 (Veloca API — mô phỏng ở đây, không gọi thật).
/// SyncStatus: Pending(chưa đồng bộ)→Synced(đã đồng bộ).</summary>
public sealed class GpsInstall
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string GpsNo { get; set; } = "";
    public DateTime DateActive { get; set; }
    public string SyncStatus { get; set; } = "Pending";
    public DateTime? SyncedAt { get; set; }
    public string MapStatus { get; set; } = "0";  // 0=chưa map VIN, 1=đã map — port FrmSto_StoBalanceGPS
    public string? GpsMapVINNo { get; set; }      // số lô map VIN (auto-gen theo lượt Apply)
    public DateTime? MappedAt { get; set; }

    /// <summary>
    /// 🔴 TRỤC TRẠNG THÁI THỨ HAI, ĐỘC LẬP với <see cref="MapStatus"/> (`Sto_StoBalanceGPS.InStatus`):
    /// "1" thiết bị ĐANG TRONG KHO · "0" ĐÃ XUẤT kho.
    /// Port cũ chỉ có trục "đã map VIN hay chưa" ⇒ không phân biệt được thiết bị **còn trong kho**
    /// với thiết bị **đã xuất mà chưa map**.
    /// </summary>
    public string InStatus { get; set; } = "1";

    /// <summary>Kho chứa thiết bị (`StorageCode`) — job auto-unmap chạy trên kho "STOGPS".</summary>
    public string? StorageCode { get; set; }

    /// <summary>
    /// Số HỘP GPS (`GPSBoxNo`) — nguồn tách RIÊNG khỏi số thiết bị (`GPSDvNo`, ở đây là <see cref="GpsNo"/>).
    /// Port cũ gộp thành 1 cột ⇒ mất khả năng tra theo hộp. (Thực thể `GpsOutDetail` của chính MiniHTC
    /// đã tách đúng 2 cột này — chỗ đó đúng, chỗ này gộp.)
    /// </summary>
    public string? GpsBoxNo { get; set; }

    /// <summary>VIN THẬT của xe (`VINReal`) — khác <see cref="Vin"/> là VIN đang được map.
    /// Lệch nhau ⇒ map sai xe; port cũ không có cột này nên không phát hiện được.</summary>
    public string? VinReal { get; set; }

    /// <summary>Loại chứng từ nguồn (`RefNo_Type`) — truy ngược thiết bị về chứng từ nhập/xuất.</summary>
    public string? RefNoType { get; set; }
    /// <summary>Khoá chứng từ nguồn (`RefNo_PK`).</summary>
    public string? RefNoPk { get; set; }

    /// <summary>Cờ KHOÁ thiết bị (`BlockStatus`) — thiết bị bị khoá thì không cho thao tác.</summary>
    public string BlockStatus { get; set; } = "0";

    /// <summary>
    /// 🔴 Vị trí xe do API GPS trả về (`VINAddress`) — **là căn cứ để GỠ MAP TỰ ĐỘNG**:
    /// job `AutoUnMapVin` coi `Address` RỖNG nghĩa là **thiết bị đã bị tháo khỏi xe** và gỡ map.
    /// Không có cột này thì không thực hiện được luật đó.
    /// </summary>
    public string? VinAddress { get; set; }

    /// <summary>Số lô GỠ map VIN (`GPSUnMapVINNo`) — nguồn sinh 1 số cho mỗi lượt gỡ.</summary>
    public string? GpsUnMapVINNo { get; set; }
    public DateTime? UnMappedAt { get; set; }
    /// <summary>Người/hệ thống thực hiện gỡ map (`UnMapBy`) — nguồn ghi cùng `UnMapDateTime`
    /// (`BizHTC.ZTempGPS.cs:1684-1688`, hệ `ERP.V15.DMSSales.Real` chỉ có trên máy 150).</summary>
    public string? UnMapBy { get; set; }

    /// <summary>
    /// 🔴 VIN mà thiết bị VỪA BỊ GỠ khỏi (`Sto_StoBalanceGPS.VINUnMap`) — nguồn ghi khi gỡ map
    /// (`Biz.HTC.WH.cs:185876`: `drScan["VINUnMap"] = dt_Sto_StoBalanceGPS.Rows[0]["VINReal"]`)
    /// và ĐỌC LẠI khi PHỤC HỒI map (`mySto_StoBalanceGPS_RecoverMapX_New20181119` :186584 → :186592).
    /// Không có cột này thì màn `FrmUnmapRecover` KHÔNG THỂ hoạt động: không biết phục hồi về VIN nào.
    /// </summary>
    public string? VinUnMap { get; set; }

    /// <summary>Địa chỉ thiết bị do Veloca trả về lúc gỡ map (`GPSAddress`, `Biz.HTC.WH.cs:185882`) —
    /// KHÁC <see cref="VinAddress"/> (vị trí XE do API GPS trả về khi còn gắn).</summary>
    public string? GpsAddress { get; set; }

    /// <summary>Cờ "bán thật" (`FlagRealSale`) — nguồn đặt "1" khi `ProjectCode = Veloca`, ngược lại "0"
    /// (`Biz.HTC.WH.cs:185877-185881`). MiniHTC chưa có trục ProjectCode ⇒ mặc định "0" = nhánh else của nguồn.</summary>
    public string FlagRealSale { get; set; } = "0";

    /// <summary>Nhật ký sửa cuối (`LogLUDateTime`) — nguồn ghi ở CẢ hai luồng gỡ map và phục hồi map.</summary>
    public DateTime? LogLUDateTime { get; set; }
    /// <summary>Người sửa cuối (`LogLUBy`).</summary>
    public string? LogLUBy { get; set; }

    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thanh toán phí lưu kho theo tháng (Pmt_PaymentStorage — port 1:1 FrmQuanLyThanhToanLuuKho/FrmSuaThanhToanLuuKho, 2010.HTC Sales/Purchase):
/// mỗi dòng VIN có phí lưu kho + phí che phủ, TotalAmount(dòng)=CostCoat+CostStorage (đã gồm VAT), AmountTotal(header)=Σ dòng,
/// TotalBeforeVAT=AmountTotal/1.1, VatAmount=AmountTotal-TotalBeforeVAT. Trạng thái P(mới tạo)→A1→A2→F(đã ký)/C(từ chối/hủy);
/// ký HTV + ký TCMS độc lập (P=chưa ký/A=đã ký); sửa/từ chối/xóa CHỈ khi Status=P và cả 2 bên CHƯA ký (=P, khớp guard gốc).</summary>
public sealed class StoragePayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtNo { get; set; } = "";
    public DateTime PmtMonth { get; set; }
    public decimal TotalBeforeVAT { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountTotal { get; set; }
    public string HtvSignStatus { get; set; } = "P";
    public DateTime? HtvSignAt { get; set; }
    public string TcmsSignStatus { get; set; } = "P";
    public DateTime? TcmsSignAt { get; set; }
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng VIN trong phiếu thanh toán lưu kho — port 1:1 grid FrmQuanLyThanhToanLuuKho, 2010.HTC.</summary>
public sealed class StoragePaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StoragePaymentId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? ColorExtNameVN { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? StorageDate { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    public decimal CostCoat { get; set; }
    public decimal CostStorage { get; set; }
    public decimal TotalAmount { get; set; }    // tự tính = CostCoat + CostStorage
    public string? Remark { get; set; }
}

/// <summary>Thanh toán phí PDI theo tháng (Pmt_PaymentPDI — port 1:1 FrmQuanLyThanhToanPDI/FrmSuaThanhToanPDI, 2010.HTC Sales/Purchase):
/// cùng cấu trúc/guard với StoragePayment — mỗi dòng VIN có phí kiểm tra vào (CostInCheck) + phí kiểm tra ra (CostOutCheck),
/// TotalPrice(dòng)=CostInCheck+CostOutCheck; sửa/từ chối/xóa CHỈ khi Status=P và cả 2 bên CHƯA ký (=P, khớp guard gốc).</summary>
public sealed class PdiFeePayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtNo { get; set; } = "";
    public DateTime PmtMonth { get; set; }
    public decimal TotalBeforeVAT { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountTotal { get; set; }
    public string HtvSignStatus { get; set; } = "P";
    public DateTime? HtvSignAt { get; set; }
    public string TcmsSignStatus { get; set; } = "P";
    public DateTime? TcmsSignAt { get; set; }
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng VIN trong phiếu thanh toán PDI — port 1:1 grid FrmQuanLyThanhToanPDI, 2010.HTC.</summary>
public sealed class PdiFeePaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PdiFeePaymentId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? ColorExtName { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? StoreDate { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    public decimal CostInCheck { get; set; }
    public decimal CostOutCheck { get; set; }
    public decimal TotalPrice { get; set; }    // tự tính = CostInCheck + CostOutCheck
}

/// <summary>Thanh toán phí vận tải + bảo hiểm theo tháng (Pmt_TransportIns — port 1:1 FrmQuanLyThanhToanVanTaiBaoHiem/FrmTaoThanhToanVanTaiBaoHiem, 2010.HTC Sales/Purchase):
/// cùng cấu trúc/guard với StoragePayment/PdiFeePayment — mỗi dòng VIN: ValTransport(dòng)=TFValReal(phí vận tải)+InsuranceCost(phí bảo hiểm)-TPValReal(phạt trễ hạn);
/// AmountTotal(header)=Σ dòng (đã gồm VAT); TotalBeforeVAT=AmountTotal/1.1. Sửa/từ chối/xóa CHỈ khi Status=P và cả 2 bên CHƯA ký (=P).</summary>
public sealed class TransportInsPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtNo { get; set; } = "";
    public DateTime PmtMonth { get; set; }
    public decimal TotalBeforeVAT { get; set; }
    public decimal VatAmount { get; set; }
    public decimal AmountTotal { get; set; }
    public string HtvSignStatus { get; set; } = "P";
    public DateTime? HtvSignAt { get; set; }
    public string TcmsSignStatus { get; set; } = "P";
    public DateTime? TcmsSignAt { get; set; }
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng VIN trong phiếu thanh toán vận tải + bảo hiểm — port 1:1 grid FrmTaoThanhToanVanTaiBaoHiem, 2010.HTC.</summary>
public sealed class TransportInsPaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TransportInsPaymentId { get; set; }
    public string Vin { get; set; } = "";
    public string? CarId { get; set; }
    public string? DlvMnNo { get; set; }
    public string? TProvinceName { get; set; }
    public DateTime? ExpectedDlvEndDate { get; set; }
    public DateTime? DlvEndDate { get; set; }
    public decimal TFValReal { get; set; }      // phí vận tải
    public decimal TPValReal { get; set; }      // phạt trễ hạn
    public decimal PriceCar { get; set; }
    public decimal InsuranceCost { get; set; }  // phí bảo hiểm
    /// <summary>⚠️ Cột RIÊNG MiniHTC (nguồn không có) — tổng tiền dòng do bản port tự tính.</summary>
    public decimal ValTransport { get; set; }

    // ===== #154 parity Pmt_TransportInsDetail: 17 cột nguồn ghi mà port cũ thiếu =====
    // Nguồn: DMS40/0.34.Contract.cs (csproj 125, md5 e2f3680f… verify 2 máy ở #139) — ghi tại 15703.
    /// <summary>Tổng giá trị dòng (`TotalPrice`).</summary>
    public decimal TotalPrice { get; set; }
    /// <summary>Ghi chú tỉnh ĐI (`FProvinceRemark`) và ghi chú theo chuẩn (`StandardRemark`) — hai ghi chú RIÊNG, khác `Remark`.</summary>
    public string? FProvinceRemark { get; set; }
    public string? StandardRemark { get; set; }
    /// <summary>⚠️ Tên cột nguồn SAI CHÍNH TẢ: `TrasportInsDtlStatus` (thiếu chữ "n") — giữ 1:1.</summary>
    public string TrasportInsDtlStatus { get; set; } = "P";
    public string? FStorageCode { get; set; }
    public string? FProvinceName { get; set; }
    public string? TStorageCode { get; set; }
    /// <summary>Mốc bắt đầu/kết thúc bảo hiểm (`InvStartDate`/`InvEndDate`).</summary>
    public DateTime? InvStartDate { get; set; }
    public DateTime? InvEndDate { get; set; }
    public decimal ExpectedDays { get; set; }
    /// <summary>Số ngày TRỄ (`DelayDate`) và tiền phạt trễ — ⚠️ nguồn viết `DelayPenaty` (thiếu chữ "l"), giữ 1:1.</summary>
    public decimal DelayDate { get; set; }
    public decimal DelayPenaty { get; set; }
    public decimal TransportCost { get; set; }
    /// <summary>Tỉ lệ phí bảo hiểm (`InsurancePercent`).</summary>
    public decimal InsurancePercent { get; set; }
    /// <summary>Loại yêu cầu vận chuyển (`TranspReqType`) — cùng bảng mã với cụm vận chuyển (#142).</summary>
    public string? TranspReqType { get; set; }
    public string? InsuranceContractNo { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }   // tự tính = TFValReal + InsuranceCost - TPValReal
    public string? Remark { get; set; }
}

/// <summary>Vi phạm của nhân viên bán hàng (HR_SalesManViolate — port 1:1 FrmCreateSalesManViolate/FrmMngSalesManViolate, SalesDealer):
/// ghi nhận kỷ luật NVBH theo loại vi phạm + thời hạn. ViolateNumber tự tăng theo từng NV (lần vi phạm thứ n).</summary>
public sealed class SalesManViolate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SalesManCode { get; set; } = "";
    public string? SalesManName { get; set; }
    public string DealerCode { get; set; } = "";
    public string ViolateTypeId { get; set; } = "";       // loại vi phạm (Mst_ViolateType); "VV"=vĩnh viễn, "TT"=tạm thời
    public int ViolateNumber { get; set; }                // lần vi phạm thứ n (auto +1 theo NV)
    public DateTime? ViolateDateStart { get; set; }
    public DateTime? ViolateDateEnd { get; set; }         // bắt buộc khi ViolateTypeId=="TT"
    public string? IdentityCardNo { get; set; }
    public string? PhoneNo { get; set; }
    public string? SMType { get; set; }                   // snapshot loại NV lúc vi phạm (HR_SalesManViolate.SMType)
    public string? SmDateOfBirth { get; set; }            // snapshot ngày sinh (HR_SalesManViolate.SMDateOfBirth)
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// #204 parity — bảng nguồn THẬT là `Mst_SalesMan` (tên `Mst_DlSalesMan` trong tài liệu cũ KHÔNG tồn tại;
/// nguồn chỉ có `Mst_SalesMan` và `Dlr_SalesMan`). Khoá của cả hai là `SMCode`; `SMHyundaiCode` thuộc
/// `Mst_SalesMan` (xác minh qua alias `msm.SMHyundaiCode` trong BizHTC.Contract.cs).
/// 🔴 NỢ HỢP NHẤT: <see cref="DealerSalesMan"/> cũng map CHÍNH bảng này (cùng khoá `SMCode`, cùng `SMStatus`)
/// ⇒ hai entity một bảng. Chưa gộp ở lượt này để giữ "một biến" (xem luật `C0-trecentesimus`).
/// Port 1:1 FrmMngSalesManHTC / FrmMngSalesManApproved (SalesDealer): NVBH tại đại lý;
/// duyệt = cấp SMHyundaiCode (mã Hyundai). SMStatus: THUVIEC/CHINGTHUC/CTVIEN/NGHIVIEC.
/// </summary>
public sealed class DlSalesMan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SMCode { get; set; } = "";
    public string SMName { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? SMHyundaiCode { get; set; }        // mã Hyundai cấp (có = đã duyệt)
    public string SMStatus { get; set; } = "THUVIEC"; // THUVIEC/CHINGTHUC/CTVIEN/NGHIVIEC
    // 🔴 #204: nguồn dùng `SMGender` và `SMPhoneNo`. Tên cũ `Sex` **0 hit** trên toàn `TERP.BizHTC`
    //    (tên tự đặt), `PhoneNo` có hit nhưng thuộc bảng khách hàng — không phải cột của NVBH.
    public string? SMGender { get; set; }             // 0=Nam, 1=Nữ
    public DateTime? DateOfBirth { get; set; }
    public string? SMPhoneNo { get; set; }
    public string? IdentityCardNo { get; set; }
    public DateTime? StartDate { get; set; }    // ngày bắt đầu công tác — port FrmQuanLyLSCongTac
    public DateTime? EndDate { get; set; }      // ngày kết thúc công tác
    public string? SMReason { get; set; }       // lý do (nghỉ việc/chuyển...)
    public string? SMDesc { get; set; }         // mô tả chi tiết
    // audit 2026-09-03: bổ sung field thiếu — nguồn thật FrmMngSalesManHTC.cs (btnImportExcel_Click), KHÔNG
    // phải FrmMngSalesManApproved.cs (đó chỉ là màn báo cáo đọc-chốt-tháng, không có logic ghi các field này).
    public string? BDHStatus { get; set; }          // CHALLENGE (đang thử thách) | APPOINT (đã bổ nhiệm)
    public DateTime? ChallengeStartDate { get; set; }
    public DateTime? ChallengeEndDate { get; set; }
    public string? QualityRank { get; set; }        // LV0 Tập sự / LV1 Đạt chuẩn / LV2 Cao cấp / LV3 Chuyên gia
    public string? AccountHTA { get; set; }         // "1"=Có / "0"=Không
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Lịch sử bảo dưỡng xe tồn kho theo kỳ (VIN_MaintainPeriodHist — port 1:1 FrmMaintenanceHistory, Maintenance):
/// mỗi lần bảo dưỡng xe kho = 1 dòng; MtnTimes tự tăng theo VIN+loại; MtnNextDate = lần này + chu kỳ.</summary>
public sealed class CarMaintenance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string? StorageCode { get; set; }
    public string? ModelCode { get; set; }
    public string MtnType { get; set; } = "MAINTAINANCE";  // MAINTAINANCE (thường) / EXT (gia hạn)
    public int MtnTimes { get; set; }                       // lần bảo dưỡng thứ n (theo VIN+loại)
    public DateTime MtnDate { get; set; } = DateTime.Now;   // ngày bảo dưỡng lần này
    public DateTime? MtnNextDate { get; set; }              // ngày bảo dưỡng kế
    public string? UserCode { get; set; }
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Xe trong kho bảo dưỡng gia hạn (StoF_MaintainMain — port 1:1 FrmMaintenanceWarehouse, Maintenance):
/// theo dõi xe vào/ra bảo dưỡng gia hạn. MtnExtStatusMain: NG(chưa)→IN(đang BD gia hạn)→OUT(xong ra kho).</summary>
/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (ca thứ 4, phát hiện #B06). Lớp này và
/// <see cref="StoFMaintainMain"/> **cùng port bảng nguồn `StoF_MaintainMain`**. Đã hợp nhất về
/// <see cref="StoFMaintainMain"/> (đúng khoá nguồn `SF_MtnNo` + `VIN`, có `MtnStatusMain`/`UserCodeMtn`/kho).
/// `/api/maintext` KHÔNG còn ghi vào lớp này; giữ lại để không phá dữ liệu đã lưu.
/// </summary>
public sealed class MaintainExt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? StorageCode { get; set; }
    public DateTime? MtnExtStartDTime { get; set; }   // vào BD gia hạn (MtnExtIn)
    public DateTime? MtnExtEndDTime { get; set; }     // ra khỏi BD gia hạn (MtnExtOut)
    public string? MtnExtRemark { get; set; }
    public string MtnExtStatusMain { get; set; } = "NG"; // NG / IN / OUT
    // audit 2026-09-03: bổ sung — nguồn FrmMaintenanceWarehouse.gvMaintenanceWarehouse_ShowingEditor chỉ cho
    // sửa tay 3 cột: UserCodeMtnExt/MtnExtStatusMain/MtnExtRemark — UserCodeMtnExt bị thiếu hoàn toàn ở port trước.
    public string? UserCodeMtnExt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Biểu chiết khấu / phạt bán xe theo ngày hiệu lực (Mst_Discount — port 1:1 FrmDiscount, Admin/Product):
/// mỗi ngày hiệu lực 1 dòng % chiết khấu + các % phạt/chi phí. Bản mới nhất ≤ ngày bán = áp dụng.</summary>
public sealed class Discount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public DateTime EffectiveDate { get; set; }          // ngày hiệu lực (khoá)
    public decimal DiscountPercent { get; set; }         // % chiết khấu
    public decimal PenaltyPercent { get; set; }          // % phạt
    public decimal PenaltyPercentTCKT { get; set; }      // % phạt TCKT
    public decimal FnExpPercent { get; set; }            // % chi phí tài chính
    public decimal PmtDsTCGPercent { get; set; }         // % chiết khấu thanh toán TCG
    public string Status { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Giá thiết bị/phụ kiện theo spec xe (Mst_DevicePrice_Spec — port 1:1 FrmMst_DevicePrice_Spec, Admin/Product):
/// giá thiết bị gắn theo spec + VAT + ngày hiệu lực. PriceVAT = Price * (1 + VAT/100).</summary>
public sealed class DevicePrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? SpecDescription { get; set; }
    public string? DeviceTypeCode { get; set; }
    public string DeviceCode { get; set; } = "";
    public string? DeviceName { get; set; }
    public decimal Price { get; set; }
    public decimal VAT { get; set; } = 10;
    public decimal PriceVAT { get; set; }            // = Price * (1 + VAT/100)
    public DateTime? EffectiveDate { get; set; }
    public string Status { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Giá bán xe TCG theo spec (Mst_TCGCarSalePrice — port 1:1 FrmMstTCGCarSalePrice, Admin/Product):
/// đơn giá bán xe cho TCG (tài chính) theo mã spec. Upsert theo SpecCode.</summary>
public sealed class TcgSalePrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SpecCode { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public string Status { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Lệnh sửa chữa RO (Ser_RO — port 1:1 FrmRepairOrder, TCMotor DMSCarSv/Services):
/// header lệnh sửa chữa xe tại xưởng dịch vụ. HasRO→InGarage→Repaired→CheckEnd→Paid→Finished.</summary>
public sealed class RepairOrder
{
    /// <summary>🔴 #446 §12 APPID — khoá nối LỆNH SỬA về CUỘC HẸN sinh ra nó.
    /// Nguồn lọc `BuildClauseConditionList("and", "ro.**AppId**", strAppId, "|")` trong
    /// `Ser_RO_Get_ByAppId` (`BizCarSv.Appointment.cs:2086`) — đây là cách màn lịch hẹn biết
    /// "cuộc hẹn này đã có báo giá chưa" để đổi nhãn nút giữa *Tạo báo giá* và *Xem báo giá*.
    /// Thiếu cột này thì **không có đường nào đi từ cuộc hẹn sang báo giá**.</summary>
    public string? AppId { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RONo { get; set; } = "";
    public string LicensePlate { get; set; } = "";     // biển số
    public string? Vin { get; set; }
    /// <summary>
    /// ⚠️ #301 SỬA CHÚ THÍCH SAI: đây **KHÔNG phải "chủ xe"**. Nguồn tách hẳn hai khái niệm:
    ///   `CusName   = isnull(ro.CusName, isnull(cus.ContName, cus.CusName))` — **người mang xe đến**
    ///   `OwnerName = cus.CusName` (thẳng, không dự phòng)                   — **chủ xe** trên hồ sơ
    /// Xe công ty đi bảo dưỡng thì hai tên này khác nhau; gộp làm một là mất thông tin đối chiếu.
    /// </summary>
    public string? CusName { get; set; }
    public string? Km { get; set; }                    // số km
    public DateTime? CheckInDate { get; set; }         // khách tới
    public DateTime? PlanedDeliveryDate { get; set; }  // dự kiến giao
    public string? CusRequest { get; set; }            // yêu cầu KH
    public string? CarStatus { get; set; }             // tình trạng tiếp nhận xe
    public bool CusWaiting { get; set; }               // khách chờ
    public string Status { get; set; } = "HasRO";      // HasRO→InGarage→Repaired→CheckEnd→Paid→Finished
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // Từ chối lệnh sửa chữa (FrmROReject)
    public string? RejectNote { get; set; }
    public DateTime? RejectedAt { get; set; }
    // Bổ sung 2026-09-05 cho màn Lịch sử dịch vụ (FrmServiceHistory) — lưới gốc hiển thị các cột này;
    // nguồn `Ser_ServiceHistory_Get` (BizCarSv.Service01.cs:432) select `ro.*` join Ser_Customer + Ser_Car.
    public string? DealerCode { get; set; }            // Ser_RO.DealerCode — dùng cho luật CanShowDetail

    /// <summary>
    /// 🔴 #283 CREATOR — người lập lệnh sửa chữa (`Ser_RO.Creator`). Cần cho màn tra CHÉO ĐẠI LÝ của tổng
    /// đài iCIC: cột này bị **CHE thành `******`** khi đại lý gọi API xem lệnh của đại lý khác.
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// 🔴 #310 RECEPTIONFNO — phiếu TIẾP NHẬN sinh ra lệnh này. Port cũ chỉ có chiều ngược
    /// (<c>Reception.RONO</c>) và là **một cột đơn** ⇒ mô hình hoá quan hệ **1-1**.
    /// Nguồn là **1-nhiều**: `Ser_ReceptionF_Delivery` (`Tab.cs:8487`) lấy số lệnh bằng
    ///   `select top 1 t_ro.RONo from Ser_RO t_ro where t.ReceptionFNo = t_ro.ReceptionFNo order by t_ro.CreatedDate desc`
    /// ⇒ **một phiếu tiếp nhận có thể sinh NHIỀU lệnh**, màn giao xe hiện **lệnh TẠO GẦN NHẤT**.
    /// ⚠️ Ngay trên đó, dòng đọc thẳng `--, ro.RONo` **đã bị comment** — luật B: port dòng ĐANG CHẠY.
    /// </summary>
    public string? ReceptionFNo { get; set; }

    // ===== 🔴 #320 PARITY VỚI BẢN LIVE `Ser_RO_Create_New20230220` (`Service.RO.cs:3001`) =====
    // 🆕 Tìm bằng sweep MỚI `_audit/sweep_twin_column_delta.js`: so **TẬP CỘT ĐƯỢC GHI** giữa các bản
    //   cùng gốc. Cụm `Ser_RO_Create` có **9 bản**, chênh tới **28 cột**: bản LIVE ghi **61** cột,
    //   bản trần chỉ **51**. Port cũ có 50 ⇒ thiếu 30 (3 trong đó chỉ là đổi tên).
    // ⚠️ TRACE TWIN đã làm: WS `:10603` gọi `_New20230220` ⇒ 8 bản còn lại CHẾT.
    public string? AdvisoryCode { get; set; }        // CVDV tư vấn
    public string? AdvisoryPhone { get; set; }
    /// <summary>CARID — khoá kỹ thuật của xe, **KHÁC** `Vin` (số khung). Nguồn ghép xe theo cột này.</summary>
    public string? CarID { get; set; }
    public string? CarWashRequested { get; set; }    // khách yêu cầu rửa xe
    public string? CusTypeID { get; set; }           // loại KH — bản chụp trên lệnh (chuỗi isnull #301)
    public string? DlrPDIReqNo { get; set; }         // số yêu cầu PDI của đại lý
    public string? EngineerID { get; set; }
    public string? FlagOnlyPoint { get; set; }
    public string? FlagPause { get; set; }           // tạm dừng sửa chữa

    // ===== 🔴 #341 HAI MỐC còn thiếu của chuỗi trạng thái (`SerROStatusUpdate`) =====
    /// <summary>CHECKENDDATE — mốc **KIỂM TRA CUỐI CÙNG** (bước `CheckEnd`).
    /// Nguồn lưu `"yyyy-MM-dd HH:mm"` ⇒ cắt GIÂY, như mọi mốc khác của lệnh.</summary>
    public DateTime? CheckEndDate { get; set; }

    /// <summary>TOTALACTHOURS — **tổng giờ công thực tế** của lệnh, ghi kèm ở bước `Repaired`.
    /// ⚠️ Nguồn chỉ ghi khi tham số **khác rỗng** (`if (!IsEmpty(strTotalActHours))`) ⇒ rỗng thì
    /// GIỮ NGUYÊN giá trị cũ — khác nhóm "rỗng = xoá" của đường sửa xe (#334).</summary>
    public decimal? TotalActHours { get; set; }
    public string? IDCardNo { get; set; }            // CMND/CCCD — bản chụp trên lệnh (chuỗi isnull #301)
    public string? InsNo { get; set; }               // số đơn bảo hiểm
    public decimal? InsuranceDeductible { get; set; }// mức khấu trừ bảo hiểm
    public string? InvoiceBy { get; set; }
    public string? LevelOfInspection { get; set; }   // mức kiểm tra
    public string? ModifyBy { get; set; }
    public DateTime? ModifyDate { get; set; }
    public string? PayByCard { get; set; }
    public decimal? PlanedDuration { get; set; }     // thời lượng dự kiến
    public DateTime? ReminderMaintanceDate { get; set; }  // nhắc bảo dưỡng (nguồn viết thiếu chữ "e": Maintance)
    public decimal? ReminderMaintanceKm { get; set; }
    public string? ServiceStatus { get; set; }       // ⚠️ KHÁC `Status` (trạng thái lệnh) — trục riêng
    public string? TermsOfRepair { get; set; }
    public string? UseSHPart { get; set; }           // dùng phụ tùng SH
    public string? WorkDoneSoon { get; set; }        // yêu cầu làm nhanh
    public string? CreatedBy { get; set; }
    public string? LogLUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }

    // ===== 🔴 #321 PARITY `Ser_RO_Update_New20230220` (`Service.RO.cs:4069`) =====
    // Cụm `Ser_RO_Update` có **8 bản**, chênh **23 cột**; WS `:10794` gọi `_New20230220` ⇒ 7 bản kia CHẾT.
    // Bản Update ghi **7 cột mà bản Create KHÔNG có** — đây là các trường phát sinh TRONG QUÁ TRÌNH sửa.

    /// <summary>🔴 BỐN VAI TRÒ KỸ THUẬT ghi cùng chỗ với `Assistant` (cố vấn dịch vụ, đã có):
    /// `Engineer` kỹ thuật viên · `QA` kiểm định · `Operator` thợ vận hành · `QuanDoc` **quản đốc**.
    /// ⚠️ Cả năm ghi **VÔ ĐIỀU KIỆN** (`Rows[0]["X"] = strX` + `alEffectiveColumn.Add`) ⇒ truyền rỗng là
    ///    **XOÁ** người đang gán, không phải "giữ nguyên". Khác hẳn nhóm ngày bên dưới.</summary>
    public string? Engineer { get; set; }
    public string? QA { get; set; }
    public string? Operator { get; set; }
    /// <summary>QUANDOC — quản đốc xưởng (nguồn để nguyên tiếng Việt không dấu trong tên cột).</summary>
    public string? QuanDoc { get; set; }

    /// <summary>
    /// 🔴 SCHEDULEDATE — ngày HẸN vào xưởng. Ghi **VÔ ĐIỀU KIỆN** qua
    /// `Convert.ToDateTime(strScheduleDate).ToString("yyyy-MM-dd HH:mm")`.
    /// ⚠️ **KHÔNG có guard rỗng** ⇒ nguồn **ném `FormatException`** nếu tham số rỗng/không parse được.
    ///    Cùng rủi ro với `CheckInDate` (cũng vô điều kiện), trong khi `StartDate`/`FinishedDate` thì CÓ guard.
    ///    Bất đối xứng này CÓ THẬT trong một hàm — xem chú thích ở endpoint.
    /// ⚠️ Định dạng `"yyyy-MM-dd HH:mm"` ⇒ nguồn **cắt mất GIÂY** khi lưu.
    /// </summary>
    public DateTime? ScheduleDate { get; set; }

    /// <summary>STARTDATE — giờ BẮT ĐẦU sửa. Nguồn CÓ guard `if (IsNullOrEmpty) {} else {…}` ⇒ rỗng thì
    /// **GIỮ NGUYÊN** giá trị cũ (đối xứng với `FinishedDate`).</summary>
    public DateTime? StartDate { get; set; }

    public string? TrademarkNameModel { get; set; }    // Ser_RO.TrademarkNameModel — hiệu/dòng xe
    public string? ColorCode { get; set; }             // #301: ro.ColorCode (BẢN CHỤP), car chỉ dự phòng

    // ===== 🔴 #301 BẢN CHỤP KHÁCH + XE TRÊN CHÍNH LỆNH SỬA CHỮA =====
    // Nguồn LIVE `Ser_RO_GetStatusList02_WH_New20230220` (`BizCarSv.Service.RO.cs:840`, đọc ở :990-1035)
    // trả **mọi** thông tin khách/xe qua `isnull(ro.X, <master>.X)` ⇒ **lệnh giữ bản chụp của riêng nó,
    // bảng master chỉ là DỰ PHÒNG**. Sửa hồ sơ khách/xe hôm nay **không được** làm đổi lệnh của năm ngoái.
    // Port cũ đọc thẳng cột của lệnh, không có cột chụp nào trong số này ⇒ mất cả bản chụp lẫn dự phòng.
    public string? CusID { get; set; }
    public string? CusAddress { get; set; }
    public string? CusTel { get; set; }
    public string? CusMobile { get; set; }
    public string? CusTaxCode { get; set; }
    public string? ModelID { get; set; }
    public string? EngineNo { get; set; }
    public string? TradeMarkCode { get; set; }
    public string? BatteryNo { get; set; }
    public string? SerialNo { get; set; }
    public DateTime? WarrantyRegistrationDate { get; set; }
    public DateTime? WarrantyExpiresDate { get; set; }
    public decimal? WarrantyKM { get; set; }
    public string? Assistant { get; set; }             // Ser_RO.Assistant — cố vấn dịch vụ
    public DateTime? ActualDeliveryDate { get; set; }  // Ser_RO.ActualDeliveryDate — "Giờ giao xe thực tế"
    public DateTime? FinishedDate { get; set; }        // Ser_RO.FinishedDate — khoá sắp xếp (order by desc)

    // ===== 🔴 #266: 10 cột THẺ HỘI VIÊN / ĐIỂM của `TblSerRO` (DbDefine.cs:864-875) =====
    // Tìm bằng sweep `_audit/sweep_tblconst_tail.js` (#261) — chúng nằm ở KHỐI PHỤ cuối lớp hằng.
    // ⚠️ TRACE: chỉ file `Views/Services/FrmInvoice.cs` (SỐNG, md5 `4c587940` — KHỚP 2 máy) dùng thật.
    //    Các file `FrmInvoice - Copy.cs` / `- Copy (2).cs` **KHÔNG có trong .csproj ⇒ DEAD**, không tin.

    /// <summary>FLAGCARDEXIST — khách có thẻ hội viên hay không.</summary>
    public string? FlagCardExist { get; set; }
    /// <summary>FLAGISDLQUERY — đã tra cứu thông tin hội viên hay chưa.</summary>
    public string? FlagIsDLQuery { get; set; }

    /// <summary>
    /// 🔴 Nhóm hậu tố **`Inv`** = **CHỐT tại thời điểm lập HOÁ ĐƠN** (snapshot), KHÔNG phải giá trị hiện
    /// tại của thẻ. Cùng họ với cặp `CostInit`/`CostActual` (#231): hoá đơn phải giữ số liệu lúc phát hành,
    /// điểm/hạng đổi sau đó không được làm đổi hoá đơn cũ.
    /// </summary>
    public string? CardNoInv { get; set; }            // CARDNOINV — số thẻ lúc lập hoá đơn
    public string? CardTypeInv { get; set; }          // CARDTYPEINV — hạng thẻ lúc lập hoá đơn
    public string? CardTypeExpectInv { get; set; }    // CARDTYPEEXPECTINV — hạng DỰ KIẾN sau giao dịch
    public decimal? PointEndInv { get; set; }         // POINTENDINV — điểm cuối kỳ
    public decimal? PointRankTotalInv { get; set; }   // POINTRANKTOTALINV — tổng điểm xét hạng
    public decimal? PointConsumptionPrm { get; set; } // POINTCONSUMPTIONPRM — điểm tiêu dùng

    // ===== 🔴 #328 PARITY `SerROStatusUpdatePaid_New20230228` (`Service.RO.cs:5736`) =====
    // TRACE TWIN: WS `:11483` gọi bản `_New20230228`; bản `_New20230220` ở `:11428` **đã bị comment**
    //   ⇒ trong 7 bản của cụm này, bản có ngày MỚI NHẤT thắng — nhưng chỉ biết được nhờ đọc WS.
    /// <summary>PAIDCREATEDDATE — mốc THANH TOÁN. ⚠️ Nguồn lưu `"yyyy-MM-dd HH:mm"` ⇒ **cắt GIÂY**.</summary>
    public DateTime? PaidCreatedDate { get; set; }

    /// <summary>ISCUSPAYMENTALL — khách có trả TOÀN BỘ không. ⚠️ Cùng cột mà #326 dùng để quyết định
    /// **ghi nợ hãng bảo hiểm** lúc giao xe; ở bước THANH TOÁN nó được **lưu lên lệnh**.</summary>
    public string? IsCusPaymentAll { get; set; }

    public decimal? AmountFromMC { get; set; }        // tiền do hãng (MC) chi trả
    /// <summary>POINTTOTAL — **điểm tích XÉT HẠNG** của hội viên (chú thích nguồn).
    /// ⚠️ KHÁC `PointRankTotalInv` = **điểm tích TIÊU DÙNG**. Hai loại điểm, tên gần giống nhau.</summary>
    public decimal? PointTotal { get; set; }
    public decimal? AmountDiscountOther { get; set; } // giảm giá khác

    public string? MemberNo { get; set; }             // MEMBERNO — số hội viên (FrmInvoice.cs:707)

    /// <summary>
    /// 🔴 POINTVOUCHER — điểm quy đổi thành TIỀN GIẢM. `FrmInvoice.cs:648`:
    ///   `AmountFinal = TongTienSauThue − AmountDiscount − AmountDiscountOther − PointVoucher`
    /// ⇒ **trừ THẲNG vào tiền cuối cùng**, không phải chỉ để hiển thị.
    /// (:1634 còn trừ tiếp `AmountFromMC` khi tính tổng sau sửa chữa.)
    /// </summary>
    public decimal? PointVoucher { get; set; }

    // ===== 🔴 #277b: hai cột LỌC của màn chăm sóc 72h — nguồn đọc chúng ở **ĐẦU LỆNH** (`ro.`), không
    //   phải ở dòng hạng mục. MiniHTC vốn chỉ có `RoServiceItem.ROType` (loại công việc TỪNG DÒNG) ⇒
    //   nếu join nhầm sang đó thì lọc PDI sẽ sai hẳn tầng.
    /// <summary>ISREREPAIR — lệnh **SỬA LẠI** (khách quay lại vì lỗi cũ). Nguồn lọc `= '0'` để loại.</summary>
    public string? IsReRepair { get; set; }
    /// <summary>ROTYPE ở ĐẦU lệnh sửa chữa. Nguồn loại `PDI` nhưng **giữ dòng `is null`**.</summary>
    public string? ROType { get; set; }

    /// <summary>
    /// 🔴 #271 `Ser_RO.SyncVelocaFlag` — LSC này đã đồng bộ sang hệ **Veloca** hay chưa ("1" = rồi).
    /// Nguồn (`BizCarSv.ZTemp.cs:17559`) chỉ cho lấy LSC **chưa đồng bộ**:
    ///   `and (t.SyncVelocaFlag is null or t.SyncVelocaFlag = '0')`
    /// kèm chú thích của tác giả: *"trước phục vụ test nên mở cho 1 RO được đồng bộ nhiều lần"* ⇒ cờ này
    /// là **chốt chống đồng bộ lặp**, không phải cột trang trí.
    /// ⚠️ Cờ chỉ có ở nhánh LẤY MỘT LSC; nhánh TÌM KIẾM danh sách **không** lọc cờ này.
    /// </summary>
    public string? SyncVelocaFlag { get; set; }
    // ===== #464 §12: hai khối JSON giao dịch Loyalty mà WebMethod thật KHÔNG nhận =====
    /// <summary>`Crd_DealSerRO` dạng JSON — client dựng và gửi, nhưng WS live không khai tham số này.
    /// MiniHTC nhận và LƯU để không mất dữ liệu (lệch nguồn CỐ Ý).</summary>
    public string? CrdDealSerROJson { get; set; }
    /// <summary>`Crd_DealSerRODtl` (danh sách dòng) dạng JSON — cùng lý do trên.</summary>
    public string? CrdDealSerRODtlJson { get; set; }
}

/// <summary>Dòng công việc dịch vụ trong RO (Ser_RO_ServiceItems): mã CV + nguyên nhân + kết quả + kỹ thuật viên.</summary>
public sealed class RoServiceItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RoId { get; set; }
    public string SerCode { get; set; } = "";
    public string? SerName { get; set; }
    public string? Cause { get; set; }                 // nguyên nhân
    public string? Result { get; set; }                // kết quả
    /// <summary>⚠️ Một tên KTV dạng chữ. Danh sách KTV thật của hạng mục nằm ở
    /// <see cref="RoServiceItemEngineer"/> (1-n, do hệ thống tự sinh).</summary>
    public string? Engineer { get; set; }              // kỹ thuật viên

    /// <summary>Loại công việc của hạng mục (Ser_ROServiceItems.ROType: BDD/SCC/SCD/SCS/PDI/SPK)
    /// — quyết định hạng mục này nhận nhóm KTV "sửa chữa chung" hay "đồng sơn".</summary>
    public string? ROType { get; set; }

    /// <summary>
    /// Hệ số công của hạng mục dịch vụ (Ser_ROServiceItems.FACTOR).
    /// ⚠️ Với dòng DỊCH VỤ, hệ số đóng vai trò "số lượng": tiền = Factor × Price × (1 + VAT%),
    /// KHÔNG có cột Quantity riêng.
    /// ⚠️ Mặc định của nguồn là <b>0</b> (`isnull(sri.Factor,0)`) — khác báo giá phụ tùng dùng mặc định 1.
    /// Hệ số 0 ⇒ hạng mục thành 0 đồng.
    /// </summary>
    public decimal Factor { get; set; }

    /// <summary>Đơn giá công của hạng mục (Ser_ROServiceItems.PRICE).</summary>
    public decimal Price { get; set; }

    /// <summary>Thuế suất theo PHẦN TRĂM (Ser_ROServiceItems.VAT), nguồn tính `VAT*0.01`.</summary>
    public decimal Vat { get; set; }

    /// <summary>Giờ công thực tế (ACTMANHOUR).</summary>
    public decimal? ActManHour { get; set; }

    /// <summary>🔴 #342 INSURANCEPRICE — **giá hãng bảo hiểm đã duyệt** cho dòng này.
    /// Quy tắc tính tiền công nợ bảo hiểm rẽ theo chính cột này:
    ///   `when InsurancePrice > 0 then InsurancePrice` ⇒ dùng **NGUYÊN GIÁ THOẢ THUẬN**,
    ///     KHÔNG nhân `Factor`, KHÔNG cộng VAT;
    ///   `when isnull(InsurancePrice,0) <= 0 then <công thức thường>` ⇒ tính như dòng bình thường.
    /// ⇒ Thiếu cột này thì mọi dòng rơi vào nhánh thường và số công nợ **luôn sai** khi có giá duyệt.</summary>
    public decimal? InsurancePrice { get; set; }
    public decimal Amount { get; set; }                // tiền công

    /// <summary>
    /// 🔴 #280 `ExpenseType` — **ĐỐI TƯỢNG THANH TOÁN** của dòng công (`TConst.Ser_ROType`):
    /// `ROREPAIR` · `ROINSURANCE` · `ROWARRANTY` · `LOCAL` · `GENERAL`.
    /// Toàn bộ phân loại của báo cáo CSI và màn chăm sóc 72h dựa vào cột này (nguồn dò bằng bốn phép
    /// `select top 1 … where ROID = @… and ExpenseType = '<mã>'`).
    /// ⚠️ Dòng BÁO GIÁ (`ServiceQuotationLabor`/`ServiceQuotationPart`) đã có cột cùng tên từ trước —
    /// đó là **tầng báo giá**, khác tầng LỆNH SỬA CHỮA này.
    /// </summary>
    public string? ExpenseType { get; set; }
    /// <summary>🔴 #367 CAMID — mã **chương trình khuyến mại** áp cho dòng dịch vụ. Nguồn gửi Veloca
    /// (`Table 20`) lọc mã này trên CHÍNH hai bảng dòng RO ⇒ **không có cột này thì bảng khuyến mại
    /// gửi sang Veloca VĨNH VIỄN RỖNG**, chứ không phải "chưa có dữ liệu".</summary>
    public string? CamID { get; set; }
}

/// <summary>Dòng phụ tùng trong RO (Ser_RO_PartItems): mã PT + ĐVT + SL cần + đơn giá.</summary>
/// <summary>
/// #253 ẢNH ĐÍNH KÈM LỆNH SỬA CHỮA `Ser_RO_Attachment` — port 1:1
/// `Views/Services/FrmROAttachment.cs` (407 dòng, md5 `d81f1052` — KHỚP 2 máy).
/// Cột lấy từ hằng của form (:26-30): `ID` · `IMAGE` · `IMAGEPATH` · `IMAGENAME` + khoá `RONO`.
/// ⚠️ `RONo` hiển thị được form ghép tiền tố `"LS-"` (:203) — đó là **định dạng HIỂN THỊ**,
///    KHÔNG lưu vào DB; port giữ mã trần.
/// </summary>
public sealed class RoAttachment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RONo { get; set; } = "";
    public string ImageName { get; set; } = "";   // IMAGENAME — tên file, bị 4 guard (xem endpoint)
    public string? ImagePath { get; set; }        // IMAGEPATH (thêm 2016-07-16 theo comment nguồn)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class RoPartItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RoId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public decimal NeedQty { get; set; } = 1;
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Hệ số của dòng phụ tùng (Ser_ROPartItems.FACTOR) — mặc định nguồn là <b>0</b>.
    /// Tiền = Factor × Quantity × Price × (1 + VAT%).
    /// </summary>
    public decimal Factor { get; set; }

    /// <summary>Thuế suất theo PHẦN TRĂM (Ser_ROPartItems.VAT).</summary>
    public decimal Vat { get; set; }

    /// <summary>Thành tiền dòng, đã gồm hệ số và thuế.</summary>
    public decimal Amount { get; set; }

    public string? Note { get; set; }

    /// <summary>🔴 #337 FLAGACCESSORY — dòng này là **PHỤ KIỆN** hay phụ tùng sửa chữa.
    /// Báo cáo KPI cộng doanh thu phụ tùng với `and sri.FlagAccessory = '0'` ⇒ **loại phụ kiện ra**;
    /// phụ kiện được cộng riêng ở nhóm khác. Từ vựng cờ "1"/"0".</summary>
    public string? FlagAccessory { get; set; }

    /// <summary>🔴 #342 INSURANCEPRICE — **giá hãng bảo hiểm đã duyệt** cho dòng này.
    /// Quy tắc tính tiền công nợ bảo hiểm rẽ theo chính cột này:
    ///   `when InsurancePrice > 0 then InsurancePrice` ⇒ dùng **NGUYÊN GIÁ THOẢ THUẬN**,
    ///     KHÔNG nhân `Factor`, KHÔNG cộng VAT;
    ///   `when isnull(InsurancePrice,0) <= 0 then <công thức thường>` ⇒ tính như dòng bình thường.
    /// ⇒ Thiếu cột này thì mọi dòng rơi vào nhánh thường và số công nợ **luôn sai** khi có giá duyệt.</summary>
    public decimal? InsurancePrice { get; set; }

    /// <summary>#280 `ExpenseType` — đối tượng thanh toán của dòng phụ tùng, cùng bộ mã
    /// <see cref="RoServiceItem.ExpenseType"/>.</summary>
    public string? ExpenseType { get; set; }
    /// <summary>#367 CamID — khuyến mại áp cho dòng phụ tùng (xem <see cref="RoServiceItem.CamID"/>).</summary>
    public string? CamID { get; set; }
}

/// <summary>Phiếu yêu cầu xuất kho phụ tùng cho RO (Ser_RO_StockRequisition — port 1:1 FrmROStockRequisition, TCMotor DMSCarSv):
/// gắn với 1 lệnh sửa chữa, xuất phụ tùng từ kho. Draft → Issued (đã xuất).</summary>
public sealed class StockReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqNo { get; set; } = "";            // PX-...
    public string RONo { get; set; } = "";             // lệnh sửa chữa liên quan
    public string Status { get; set; } = "Draft";      // Draft → Issued
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? IssuedAt { get; set; }
    // GAP đã vá 2026-09-05: form gốc FrmROStockRequisition có các ô txtAssistant/txtPalteNo/txtFrameNo/txtNote,
    // nguồn join Ser_RO.Assistant + Ser_Customer.PlateNo/FrameNo (BizCarSv.Inventory.cs Ser_ROStockRequisition_Get)
    // — bản port trước bỏ sót toàn bộ 5 cột này.
    public string? DealerCode { get; set; }            // Ser_ROStockRequisition.DealerCode
    public string? Assistant { get; set; }             // Ser_RO.Assistant (cố vấn dịch vụ)
    public string? PlateNo { get; set; }               // Ser_Customer.PlateNo (biển số)
    public string? FrameNo { get; set; }               // Ser_Customer.FrameNo (số khung)
    public string? Note { get; set; }                  // ghi chú phiếu (txtNote)
}

/// <summary>Dòng phụ tùng phiếu xuất (Ser_RO_StockRequisitionDtl): mã PT + vị trí + SL + ĐVT.</summary>
public sealed class StockReqLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Location { get; set; }              // vị trí kho
    public decimal Quantity { get; set; } = 1;
    public string? Unit { get; set; }
}

/// <summary>Phiếu tiếp nhận xe dịch vụ (Ser_ReceptionF — port 1:1 FrmSerReceptionFMng, TCMotor DMSCarSv):
/// front-desk tiếp nhận xe khách, có thể gắn RO. Pending(Tiếp nhận) → Approved(Giao xe).</summary>
public sealed class Reception
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReceptionFNo { get; set; } = "";
    public string PlateNo { get; set; } = "";          // biển số
    public string? ModelName { get; set; }
    public string? CusName { get; set; }
    public string? CusAddress { get; set; }
    public string? CusPhoneNo { get; set; }
    public string? CusRequest { get; set; }
    public string? RONO { get; set; }
    // ===== #522 §12 CÁC CỘT PHIẾU TIẾP NHẬN MÀ BẢN PORT CŨ THIẾU =====
    // Nguồn `Ser_ReceptionF_ReceptionX_New20210727` (`BizCarSv.ZTemp.cs:21199`) nhận **26** tham số;
    // bản port đầu chỉ giữ biển số/tên khách/yêu cầu. Bảy cột dưới đây là dữ liệu **nghiệp vụ thật**.
    /// <summary>Số km khi tiếp nhận (`strKm`).</summary>
    public string? Km { get; set; }
    /// <summary>Mức nhiên liệu (`strFuelLevel`).</summary>
    public string? FuelLevel { get; set; }
    /// <summary>Mức kiểm tra — nguồn CHỈ nhận "1", "2", "3" (guard cứng).</summary>
    public string? LevelOfInspection { get; set; }
    /// <summary>Trạng thái quay lại sửa (`strBackRepairStatus`).</summary>
    public string? BackRepairStatus { get; set; }
    /// <summary>🔴 SAI CHÍNH TẢ TRONG NGUỒN: `strWarrantlyStatus` (đúng ra "Warranty").
    /// Chép **nguyên văn** để khớp dữ liệu — xem luật HẰNG ≠ GIÁ TRỊ.</summary>
    public string? WarrantlyStatus { get; set; }
    /// <summary>🔴 SAI CHÍNH TẢ TRONG NGUỒN: `strInsuaranceStatus` (đúng ra "Insurance").</summary>
    public string? InsuaranceStatus { get; set; }
    /// <summary>🔴 SAI CHÍNH TẢ TRONG NGUỒN: `strRemarkErrOrther` (đúng ra "Other").</summary>
    public string? RemarkErrOrther { get; set; }
    /// <summary>Khoá khách hàng / khoá xe (`strCusID` / `strCarID`) — nguồn nhận KHOÁ, không nhận biển số.</summary>
    public string? CusID { get; set; }
    public string? CarID { get; set; }
    /// <summary>#506 §12 `DealerCode` — nguồn lọc phiếu tiếp nhận theo đại lý
    /// (`SqlTemplate_Ser_ReceptionF.zzB_tbl_Ser_ReceptionF_Filter_zzE(strDealerCode, …)`).</summary>
    public string? DealerCode { get; set; }                  // RO liên kết (nếu đã lập lệnh)
    public string Status { get; set; } = "Pending";    // Pending(Tiếp nhận) → Approved(Giao xe)

    /// <summary>
    /// 🔴 #271 LỊCH HẸN mà phiếu tiếp nhận này thực hiện (`AppId` của nguồn).
    /// Nguồn `Ser_ReceptionF_Reception_New20210727` (`BizCarSv.ZTemp.cs:19627`) — khối **CHỈ CÓ TRÊN
    /// MÁY 150** — sau khi tiếp nhận xong thì gọi `HCC_Appointment_FinishOSX` **khi và chỉ khi**
    /// `AppId` khác rỗng ⇒ khách vãng lai (không hẹn trước) KHÔNG đẩy gì sang HCC.
    /// Cặp đôi với `HCC_Appointment_AddOSX` lúc TẠO lịch hẹn (#270): mở ở đó, đóng ở đây.
    /// </summary>
    public string? AppNo { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliveredAt { get; set; }
}

/// <summary>Phiếu nhập kho phụ tùng (Ser_Inv_StockIn — port 1:1 FrmStockInCreate, TCMotor DMSCarSv/Inventory):
/// nhập phụ tùng vào kho. Draft → Posted (ghi sổ, tăng tồn PartStock).</summary>
/// <summary>🔴 #416 MỘT LẦN LƯU KHO CỦA MỘT PHỤ TÙNG (`Ser_Inv_PartInstance`) — bản ghi nối
/// **phiếu NHẬP** với **phiếu XUẤT** của cùng một lô hàng ở cùng một vị trí kho.
/// Đây là bảng mà mọi báo cáo lãi/lỗ phụ tùng dựa vào: không có nó thì không ghép được
/// giá vốn với giá bán theo từng lô.</summary>
public sealed class PartInstance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DealerCode { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartID { get; set; }
    /// <summary>⚠️ Nguồn lọc `Status not in ('4','5')` — **danh sách ĐEN**, không phải danh sách trắng.</summary>
    public string? Status { get; set; }
    public string? StockInNo { get; set; }
    public long? StockInId { get; set; }
    /// <summary>⚠️ Nguồn lọc `StockOutNo like '%%'` — trông như không lọc gì, nhưng `like` **loại NULL**
    /// ⇒ thực chất là "chỉ lấy lô ĐÃ XUẤT". Một bộ lọc nghiệp vụ trá hình.</summary>
    public string? StockOutNo { get; set; }
    public long? StockOutId { get; set; }
    public string? LocationID { get; set; }
    public decimal Quantity { get; set; }
    /// <summary>Giá nhập của lô (dự phòng khi dòng chi tiết phiếu nhập không có giá).</summary>
    public decimal? SIPrice { get; set; }
    /// <summary>Giá xuất của lô (dự phòng khi dòng chi tiết phiếu xuất không có giá).</summary>
    public decimal? SOPrice { get; set; }
    /// <summary>#472 `SIVAT` — %VAT lúc nhập, dùng trong công thức giá trị tồn của báo cáo kho:
    /// `Price*Qty + SIVAT*0.01*Price*Qty`.</summary>
    public decimal? SIVAT { get; set; }
    public DateTime? DateIn { get; set; }
    public DateTime? DateOut { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class PartStockIn
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockInNo { get; set; } = "";
    public DateTime StockInDate { get; set; } = DateTime.Now;
    public string? StockInType { get; set; }
    public string WarehouseCode { get; set; } = "";
    public string? Staff { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG `TConst.Ser_Inv_StockIn` cua nguon (`Const.Main.cs:203-210`):
    /// **"1" Mới tạo · "2" Tiến hành · "3" Kết thúc · "4" Điều chỉnh · "5" Hủy**.
    /// ⚠️ Port cũ `Draft/Posted/Rejected` = **3 trạng thái**, thiếu hẳn "2" Tiến hành (bước trung gian
    /// bắt buộc — nguồn KHÔNG cho nhảy thẳng 1→3) và "4" Điều chỉnh (phiếu cũ bị phiếu điều chỉnh thay).
    /// </summary>
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PostedAt { get; set; }
    public string? RejectReason { get; set; }
    public string? RejectedBy { get; set; }
    public DateTime? RejectedAt { get; set; }

    // ===== 🔴 #244: 4 cột nguồn `Ser_Inv_StockIn` mà màn tra PT-cho-phiếu-thanh-toán CẦN =====
    // Nguồn `Ser_Mst_Part_GetForSupplierPayment` (BizCarSv.Inventory.StockOut.cs:14890) select đủ 4 cột này.
    public string? DealerCode { get; set; }
    public string? SupplierID { get; set; }
    public string? TSTRequestNo { get; set; }   // số yêu cầu xuất NCC
    public string? BillNo { get; set; }         // số hoá đơn NCC

    // ===== 🔴 #265: 16 cột nguồn `TblSerInvStockIn` (DbDefine.cs:1122-1148) mà port cũ THIẾU =====
    // Tìm bằng sweep `_audit/sweep_tblconst_tail.js` (#261).
    public string? StockInID { get; set; }

    /// <summary>
    /// 🔴 #305 FLAGSYNCVELOCA — cờ đã đồng bộ phiếu NHẬP sang Veloca ("0" chưa · "1" đã).
    /// Bộ lọc nguồn nhận **ba** giá trị: rỗng = **LẤY CẢ HAI** (`'' = @strFlagSyncVeloca or …`).
    /// </summary>
    public string FlagSyncVeloca { get; set; } = "0";

    /// <summary>
    /// 🔴 #305 SYNCVELOCADTIME — **THỜI ĐIỂM** đồng bộ, cột RIÊNG với cờ.
    /// `OSVeloca_Ser_Inv_StockIn_UpdFlagSyncVeloca` (`StockIn.cs:9716`) ghi **cả hai cùng lúc**:
    /// `set t.FlagSyncVeloca = '1', t.SyncVelocaDTime = @strLogLUDateTime`.
    /// ⚠️ Và nguồn cho **lọc theo khoảng** `SyncVelocaDTimeFrom/To` ⇒ không phải cột trang trí:
    /// thiếu nó thì không tra được "đã đẩy những phiếu nào trong khung giờ X".
    /// ⚠️ Nguồn **cố ý KHÔNG** đụng `LogLUDateTime`/`LogLUBy` (hai dòng đó bị comment) — đồng bộ sang
    /// đối tác **không tính là người dùng sửa chứng từ**.
    /// </summary>
    public DateTime? SyncVelocaDTime { get; set; }

    public string? StatusText { get; set; }     // nguồn lưu CẢ NHÃN trạng thái
    public string? Description { get; set; }
    public string? UserCode { get; set; }

    // --- khối VẬN CHUYỂN (4) — giống phiếu xuất (#264) ---
    public string? DriverName { get; set; }
    public string? DrivingLicense { get; set; }
    public string? DriverID { get; set; }
    public string? TruckNo { get; set; }

    /// <summary>STOCKOUTNO — số phiếu XUẤT tương ứng (nhập do kho khác xuất sang).</summary>
    public string? StockOutNo { get; set; }

    // --- khối ĐIỀU CHỈNH (5) ---
    /// <summary>🔴 `IsAdjustment` — phiếu nhập có **CỜ RIÊNG** đánh dấu là phiếu điều chỉnh,
    /// KHÁC phiếu xuất (#264) vốn chỉ có `OldStockOutID`. Ở đây có **cả cờ lẫn Old ID**.</summary>
    public string? IsAdjustment { get; set; }
    public string? AdjustmentBy { get; set; }
    public DateTime? AdjustmentDate { get; set; }
    public string? AdjustmentNote { get; set; }
    public string? OldStockInID { get; set; }

    /// <summary>
    /// 🔴 MẮT XÍCH `Ser_Order_Part` → nhập kho: `OrderPartId`/`OrderPartNo` nối phiếu nhập về **đơn đặt
    /// phụ tùng** (cụm TST, #234), còn `FlagOrderNCC` đánh dấu nhập theo đơn đặt NCC.
    /// Thiếu ba cột này thì không truy được hàng nhập về từ đơn nào.
    /// </summary>
    public string? OrderPartId { get; set; }
    public string? OrderPartNo { get; set; }
    public string? FlagOrderNCC { get; set; }
}

/// <summary>Dòng phụ tùng nhập (Ser_Inv_StockInDetail): mã PT + vị trí + SL + đơn giá + VAT.</summary>
public sealed class PartStockInLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StockInId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Location { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    public decimal VAT { get; set; }

    /// <summary>#244: đơn vị tính — nguồn lấy từ `Ser_MST_Part.Unit` qua join, màn tra hiển thị cột này.</summary>
    public string? Unit { get; set; }
}

/// <summary>Tồn kho phụ tùng (Ser_Inv_PartStock): số tồn theo kho + mã PT + vị trí. Cập nhật khi Post phiếu nhập/xuất.</summary>
public sealed class PartStock
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string WarehouseCode { get; set; } = "";
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Location { get; set; }
    public decimal OnHand { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phiếu xuất kho phụ tùng (Ser_Inv_StockOut — port 1:1 FrmStockOutCreate, TCMotor DMSCarSv/Inventory):
/// xuất phụ tùng khỏi kho. Draft → Posted (ghi sổ, TRỪ tồn PartStock, guard tồn không đủ).</summary>
public sealed class PartStockOut
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockOutNo { get; set; } = "";
    public DateTime StockOutDate { get; set; } = DateTime.Now;
    public string? StockOutType { get; set; }
    public string WarehouseCode { get; set; } = "";
    public string? Reason { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG `TConst.Ser_Inv_StockOut` (`Const.Main.cs:232-239`) — cùng bộ mã với phiếu nhập:
    /// **"1" Mới tạo · "2" Tiến hành · "3" Kết thúc · "4" Điều chỉnh · "5" Hủy**.
    /// </summary>
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PostedAt { get; set; }

    // `RejectBy`/`RejectDate`/`RejectDescription` của nguồn — port đặt tên `Rejected*`, giữ nguyên.
    public string? RejectReason { get; set; }
    public string? RejectedBy { get; set; }
    public DateTime? RejectedAt { get; set; }

    // ===== 🔴 #264: 13 cột nguồn `TblSerInvStockOut` (DbDefine.cs:1257-1283) mà port cũ THIẾU =====
    // Tìm bằng sweep `_audit/sweep_tblconst_tail.js` (#261).
    /// <summary>
    /// 🔴 #292 `Description` — mô tả phiếu xuất (`TblSerInvStockOut.Description`, DbDefine.cs:1263).
    /// ⚠️ **KHÁC** `Reason` đã có: `Reason` là lý do xuất kho; `Description` là mô tả chung, và chính là
    /// cột mà `UpdateStockOut` ghi. #264 port thiếu cột này nên `PUT` không lưu được mô tả.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 🔴 #304 FLAGSYNCVELOCA — cờ **đã đồng bộ phiếu xuất sang Veloca**, port cũ THIẾU HẲN.
    /// Nguồn đặt `Flag.Inactive` ("0") **ngay khi tạo** phiếu (4 chỗ: `StockOut.cs:704/753/1222/1271`,
    /// ghi vào cả DB đại lý lẫn DB kho), rồi `OSVeloca_Ser_Inv_StockOut_UpdFlagSyncVeloca` nâng lên "1".
    /// Bộ lọc của nguồn nhận **ba** giá trị: "0" chưa đồng bộ · "1" đã đồng bộ · **"" = LẤY CẢ HAI**
    /// (`StockOut.cs:18468` + mệnh đề `( '' = @strFlagSyncVeloca or siso.FlagSyncVeloca = @… )`).
    /// </summary>
    public string FlagSyncVeloca { get; set; } = "0";

    /// <summary>🔴 #305 SYNCVELOCADTIME — #304 port cờ nhưng **THIẾU mốc thời gian đi kèm**.
    /// Nguồn `UpdFlagSyncVeloca` ghi CẢ HAI cùng lúc và cho lọc theo khoảng thời gian đồng bộ.</summary>
    public DateTime? SyncVelocaDTime { get; set; }

    /// <summary>#304 STOCKOUTDATETIME — mốc thời gian dùng để tính giá vốn bình quân và để đẩy sang
    /// Veloca (`ApprDTimeUTC`). Khác <see cref="StockOutDate"/> ở chỗ có GIỜ.</summary>
    public DateTime? StockOutDateTime { get; set; }

    public string? StockOutTypeText { get; set; }   // STOCKOUTTYPETEXT — nguồn lưu CẢ NHÃN loại xuất
    public string? StatusText { get; set; }         // STATUSTEXT — nguồn lưu CẢ NHÃN trạng thái
    public string? UserCode { get; set; }
    public string? CusID { get; set; }
    public string? DealerCode { get; set; }

    // --- khối VẬN CHUYỂN (4 cột) — phiếu xuất có thông tin xe + tài xế ---
    public string? TruckNo { get; set; }
    public string? DriverName { get; set; }
    public string? DriverID { get; set; }
    public string? DrivingLicense { get; set; }

    /// <summary>
    /// --- khối ĐIỀU CHỈNH (5 cột) — 🔴 đây là thứ giải thích mã trạng thái **"4" Điều chỉnh**:
    /// phiếu cũ **không bị sửa tại chỗ** mà bị một phiếu MỚI thay, và phiếu mới trỏ ngược về phiếu cũ
    /// bằng `OldStockOutID`/`OldStockOutNo`. Thiếu cặp này thì mất dấu vết chuỗi điều chỉnh.
    /// </summary>
    public string? AdjustmentBy { get; set; }
    public DateTime? AdjustmentDate { get; set; }
    public string? AdjustmentNote { get; set; }
    public string? OldStockOutID { get; set; }
    public string? OldStockOutNo { get; set; }

    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng phụ tùng xuất (Ser_Inv_StockOutDetail): mã PT + vị trí + SL.</summary>
public sealed class PartStockOutLine
{
    // 🔴 #368 §12 — năm cột dưới đây là ĐẦU VÀO của báo cáo tổng hợp phiếu xuất
    //   (`#tbl_InvF_InventoryOutCover_*`). Thiếu chúng thì báo cáo chỉ ra được SỐ LƯỢNG,
    //   mọi cột tiền đều bằng 0 mà không báo lỗi.
    /// <summary>Đơn giá xuất trên dòng phiếu (`Ser_Inv_StockOutDetail.Price`).</summary>
    public decimal? Price { get; set; }
    /// <summary>Thuế suất % của dòng phiếu (`Ser_Inv_StockOutDetail.VAT`).</summary>
    public decimal? Vat { get; set; }
    /// <summary>Đơn vị tính — nguồn lấy từ MASTER phụ tùng qua `left join`, không nằm trên dòng phiếu.</summary>
    public string? UnitCode { get; set; }
    /// <summary>🔴 Hệ số của dòng LỆNH SỬA CHỮA tương ứng, nếu dòng xuất này gắn với một RO.
    /// Có giá trị ⇒ đơn giá xuất tính theo `RoFactor × RoPrice` (giá tính cho KHÁCH),
    /// **không** theo <see cref="Price"/> (giá kho). Xem endpoint `/api/stockouts/cover`.</summary>
    public decimal? RoFactor { get; set; }
    /// <summary>Đơn giá của dòng lệnh sửa chữa tương ứng (xem <see cref="RoFactor"/>).</summary>
    public decimal? RoPrice { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StockOutId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Location { get; set; }
    public decimal Quantity { get; set; } = 1;
}

/// <summary>Giá bán phụ tùng theo ngày hiệu lực (Ser_Inv_PartPrice — port 1:1 FrmPartPriceCreate, TCMotor DMSCarSv/Inventory):
/// giá bán (PriceOut) theo mã PT + ngày hiệu lực. Giá áp dụng = bản mới nhất ≤ ngày. PriceVAT = Price*(1+VAT/100).</summary>
public sealed class PartPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal Price { get; set; }               // giá bán (PriceOut)
    public decimal VAT { get; set; } = 10;
    public decimal PriceVAT { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string Status { get; set; } = "1";

    // ===== 🔴 #295 parity `TblSer_Inv_PartPrice` (DbDefine.cs:1990-1999): 2 cột port cũ THIẾU =====
    /// <summary>REMARK — ghi chú cho mốc giá.</summary>
    public string? Remark { get; set; }
    /// <summary>ISACTIVE — cờ hiệu lực, **CỘT RIÊNG, KHÁC `Status`** đã có. Nguồn giữ cả hai:
    /// `Status` là trạng thái nghiệp vụ của mốc giá, `IsActive` là cờ bật/tắt bản ghi.</summary>
    public string? IsActive { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Xe của khách hàng (Ser_Car — port 1:1 FrmCustomerCar, TCMotor DMSCarSv/Customer):
/// registry xe dịch vụ, gắn khách↔xe (VIN/biển số/số máy/số khung/model/màu). Reception/RO tham chiếu.</summary>
public sealed class CustomerCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string PlateNo { get; set; } = "";          // biển số
    public string? FrameNo { get; set; }               // số khung
    public string? EngineNo { get; set; }              // số máy
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? PlateColorCode { get; set; }        // màu biển (trắng/vàng/xanh)
    public string? CusCode { get; set; }
    public string? CusName { get; set; }
    public string? CusPhone { get; set; }
    public DateTime? SaleDate { get; set; }            // ngày bán xe
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Báo giá sửa chữa (header: theo RO, tổng công + phụ tùng + VAT) — port 1:1 FrmQuotation (TblSerRO/Quotation, TCMotor).</summary>
public sealed class ServiceQuotation
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string QuoteNo { get; set; } = "";
    public string? RONo { get; set; }
    public string? Vin { get; set; }
    public string? PlateNo { get; set; }
    public string? CusName { get; set; }
    public decimal LaborTotal { get; set; }
    public decimal PartTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal GrandTotal { get; set; }
    public string Status { get; set; } = "Draft"; // Draft -> Approved -> Cancelled
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // GAP đã vá 2026-09-05: bản port trước THIẾU TOÀN BỘ phần bảo hiểm của FrmQuotation.
    // Nguồn: txtInsuranceDeductible + checkInsuranceDeductible() — ô "Mức khấu trừ bảo hiểm"
    // CHỈ hiện/áp dụng khi báo giá có ít nhất 1 dòng ExpenseType = ROInsurance.
    public decimal InsuranceDeductible { get; set; }   // Ser_RO.InsuranceDeductible — mức khấu trừ bảo hiểm
    public decimal InsuranceTotal { get; set; }        // tổng tiền phần bảo hiểm (sau VAT)
    public bool HasInsuranceItem { get; set; }         // có dòng bảo hiểm không → điều kiện hiện ô khấu trừ
}

/// <summary>Dòng công (labor) trong báo giá sửa chữa — port 1:1 FrmQuotation grid CV, TCMotor.</summary>
public sealed class ServiceQuotationLabor
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceQuotationId { get; set; }
    public string SerCode { get; set; } = "";
    public string? SerName { get; set; }
    public decimal StdManHour { get; set; }   // giờ định mức
    public decimal ActManHour { get; set; }   // giờ thực tế
    public decimal Factor { get; set; } = 1;  // hệ số giá
    public decimal Price { get; set; }        // đơn giá giờ công
    public decimal Vat { get; set; } = 10;
    public decimal Amount { get; set; }        // thành tiền (gồm VAT)
    // GAP đã vá 2026-09-05: 2 cột lưới gốc bị bỏ sót (srInsurancePrice + phân loại chi phí)
    public string ExpenseType { get; set; } = "";  // TblSerROServiceItems.ExpenseType — "ROInsurance" = dòng bảo hiểm
    public decimal InsurancePrice { get; set; }    // TblSerROServiceItems.InsurancePrice — phần bảo hiểm chi trả
}

/// <summary>Dòng phụ tùng trong báo giá sửa chữa — port 1:1 FrmQuotation grid PT, TCMotor.</summary>
public sealed class ServiceQuotationPart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceQuotationId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Vat { get; set; } = 10;
    public decimal Amount { get; set; }        // thành tiền (gồm VAT)
    // GAP đã vá 2026-09-05: 2 cột lưới gốc bị bỏ sót (paInsurancePrice + phân loại chi phí)
    public string ExpenseType { get; set; } = "";  // TblSerROPartItems.ExpenseType — "ROInsurance" = dòng bảo hiểm
    public decimal InsurancePrice { get; set; }    // TblSerROPartItems.InsurancePrice — phần bảo hiểm chi trả
}

/// <summary>Gói dịch vụ (header: gồm công dịch vụ + phụ tùng bán kèm) — port 1:1 FrmServicePackageCreate/Search (TblSerServicePackage, TCMotor/Services).</summary>
public sealed class ServicePackage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PackageNo { get; set; } = "";
    public string? PackageName { get; set; }
    // ===== #548 §12 CÁC CỘT MÀ `SerServicePackageUpdate` GHI =====
    /// <summary>Đại lý sở hữu gói — cùng `PackageNo` tạo thành khoá chống trùng của nguồn.</summary>
    public string? DealerCode { get; set; }
    /// <summary>Thời gian thực hiện gói (`TakingTime`).</summary>
    public string? TakingTime { get; set; }
    public string? Description { get; set; }
    public string? Creator { get; set; }
    public DateTime? CreatedDate { get; set; }
    /// <summary>Cờ gói CÔNG KHAI — nguồn dùng nó ở nhánh `Union` thứ nhất (#546).</summary>
    public string? IsPublicFlag { get; set; }
    /// <summary>1 = dùng **giá chung**; 0 = dùng **giá riêng của gói** (chú thích nguyên văn của nguồn).</summary>
    public string? IsUserBasePrice { get; set; }
    public decimal ServiceTotal { get; set; }
    public decimal PartTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng công dịch vụ trong gói — port 1:1 FrmServicePackageCreate grid CV, TCMotor.</summary>
public sealed class ServicePackageService
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServicePackageId { get; set; }
    public string SerCode { get; set; } = "";
    // ===== #552 §12 BA CỘT NỮA MÀ `ProcessSaveServicePackageServiceItem` GHI =====
    /// <summary>Giờ công THỰC TẾ của dòng (`ActManHour`) — khác `StdManHour` (định mức, nằm ở danh mục).</summary>
    public decimal? ActManHour { get; set; }
    public decimal? VAT { get; set; }
    public string? Note { get; set; }
    /// <summary>#547 §12 ĐỐI TƯỢNG THANH TOÁN của dòng công (`TConst.Ser_ROType`:
    /// `ROREPAIR · ROINSURANCE · ROWARRANTY · LOCAL · GENERAL`). Nguồn **bắt buộc**, rỗng là ném lỗi.</summary>
    public string? ExpenseType { get; set; }
    /// <summary>#547 §12 LOẠI CÔNG VIỆC (`TConst.Ser_ROType_**New**`: `BDD · SCC · SCD · SCS · PDI · SPK`).
    /// ⚠️ Hai lớp hằng tên gần giống nhau nhưng **khác hẳn nghĩa** — xem chú thích endpoint.</summary>
    public string? ROType { get; set; }
    public string? SerName { get; set; }
    public decimal Price { get; set; }
    public decimal Factor { get; set; } = 1;
    public decimal Amount { get; set; }
}

/// <summary>Dòng phụ tùng trong gói dịch vụ — port 1:1 FrmServicePackageCreate grid PT, TCMotor.</summary>
public sealed class ServicePackagePart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServicePackageId { get; set; }
    public string PartCode { get; set; } = "";
    // ===== #552 §12 CÁC CỘT MÀ `ProcessSaveServicePackagePartItem` GHI =====
    /// <summary>SỐ LƯỢNG phụ tùng trong gói (`Quantity`) — trước nay MiniHTC **chỉ có `Factor`**,
    /// nên gói hai cái lọc dầu và gói một cái **không phân biệt được**.</summary>
    public decimal? Quantity { get; set; }
    public decimal? VAT { get; set; }
    public string? Note { get; set; }
    public string? ExpenseType { get; set; }
    public string? PartName { get; set; }
    public decimal Price { get; set; }
    public decimal Factor { get; set; } = 1;
    public decimal Amount { get; set; }
}

/// <summary>Dòng sao kê ngân hàng — port 1:1 FrmBank_BankStatement (TCMotor/Sales/Payment). Import Excel sao kê, đối soát (reconcile) với mã thanh toán DMS qua PaymentCodeDMS.</summary>
public sealed class BankStatementLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BStatementNo { get; set; } = "";     // số sao kê / lô
    public string? TransactionDate { get; set; }        // ngày giao dịch (varchar giữ nguyên format sao kê)
    public string? TransactionCode { get; set; }        // mã giao dịch (khoá đối chiếu trong file)
    public decimal DebitVal { get; set; }               // ghi nợ
    public decimal CreditVal { get; set; }              // ghi có
    public decimal BalanceVal { get; set; }             // số dư
    public string? RemittanceDetail { get; set; }       // nội dung chuyển khoản
    public string? BankSendCode { get; set; }
    public string? AccountSendName { get; set; }
    public string? AccountSendNo { get; set; }
    public string? BankReceiveCode { get; set; }
    public string? AccountReceiveName { get; set; }
    public string? AccountReceiveNo { get; set; }
    public string? ActVoucherCode { get; set; }         // mã chứng từ kế toán
    public string? PaymentCodeDMS { get; set; }         // mã thanh toán DMS đã đối soát
    public string? FlagTnxType { get; set; }            // loại giao dịch
    public string? DealerSendCode { get; set; }
    public string? DealerReceiveCode { get; set; }
    public string MatchStatus { get; set; } = "N";      // N=chưa đối soát, Y=đã khớp PaymentCodeDMS
    public DateTime CreatedAt { get; set; }
}

/// <summary>Master hãng bảo hiểm dịch vụ (Ser_Insurance) — port 1:1 FrmInsuranceCreate/Search (TCMotor DMSCarSv). Mã + tên Việt/Anh + địa chỉ + email/SĐT/fax + MST + mô tả.</summary>
public sealed class SerInsurance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsNo { get; set; } = "";
    public string? InsVieName { get; set; }
    public string? InsEngName { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? TaxCode { get; set; }
    public string? Description { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master quy đổi đơn vị TST↔DMS (TST_Mst_Exchange_Unit) — port 1:1 FrmTST_Mst_Exchange_Unit (TCMotor DMSCarSv). Theo mã phụ tùng TST: đơn vị TST/DMS + tỷ lệ quy đổi.</summary>
public sealed class TstExchangeUnit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TSTPartCode { get; set; } = "";
    public string? VieName { get; set; }
    public string? TSTUnit { get; set; }
    public string? DMSUnit { get; set; }
    public decimal ExchangeRate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master phụ tùng TST (TST_Mst_Part) — port 1:1 FrmTST_Mst_Part (TCMotor DMSCarSv).
/// Mã + tên HTC/Việt/Anh + đơn vị + VAT + giá TST + nhóm/loại.
/// #212: đồng bộ cả bảng từ hệ TST/Bravo qua `TST_SavePartAll` (BizCarSv.Bravo.cs:79) — xem
/// `POST /api/tstparts/sync-all`. Lệnh đó chỉ ghi `TSTPartCode` · `TSTPrice` · `LUDTime`.</summary>
/// <summary>
/// #247 BẢNG TẠM PHỤ TÙNG TST `TST_Mst_Part_Temp` — nguồn `TST_Mst_Part_Temp_Get`
/// (BizCarSv.Bravo.cs:223, md5 `44509215` — khớp 2 máy).
/// 🔴 KHÁC `TST_Mst_Part` (lớp <see cref="TstPart"/>): đây là bảng **TẠM**, và trong TOÀN BỘ solution
///    DMSCarSv **chỉ có đường ĐỌC** — không hàm nào ghi vào nó, không màn client nào gọi.
///    ⇒ dữ liệu do hệ NGOÀI nạp; API này phục vụ hệ ngoài qua gateway `WSCarSv.asmx.cs:40231`.
/// ⚠️ Nguồn `select t.*` nên chỉ **hai cột được xác nhận** (từ hai bộ lọc): `TSTPartCode` · `TSTVieName`.
///    KHÔNG bịa thêm cột (luật `C0-trecentesimusquadragesimusseptimus`).
/// ⚠️ Tên cột tên-tiếng-Việt ở đây là `TSTVieName`, KHÁC `VieName` của bảng chính.
/// </summary>
public sealed class TstPartTemp
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TSTPartCode { get; set; } = "";
    public string? TSTVieName { get; set; }
}

public sealed class TstPart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TSTPartCode { get; set; } = "";
    public string? VieNameHTC { get; set; }
    public string? VieName { get; set; }
    public string? EngName { get; set; }
    public string? Unit { get; set; }
    public decimal VAT { get; set; }

    /// <summary>
    /// 🔴 #245: nguồn có **BỐN loại giá** (xem dưới); cột này tương ứng `TSTPriceNormal` (giá thường).
    /// Giữ tên `TSTPrice` để không vỡ dữ liệu + lệnh đồng bộ đã có (#212).
    /// </summary>
    public decimal TSTPrice { get; set; }

    // ===== 🔴 #245: 16 cột nguồn `TST_Mst_Part_Get01` mà port cũ THIẾU =====
    // Nguồn `BizCarSv.Service.cs:18409` (md5 5e5d6f20 — khớp 2 máy) gọi API Bravo rồi **ánh xạ tên**
    // từ trường Bravo sang cột DMS. Bảng ánh xạ (:18545-18567) — tên hai bên KHÁC HẲN nhau:
    //   ItemCode→TSTPartCode · ItemName→VieName · StandardPrice→TSTPriceList · UnitPrice→TSTPriceNormal
    //   · UrgentPrice→TSTPriceUrgent · WarrantyPrice→TSTPriceWarranty · QtyDA→DongAnhStockStatus
    //   · QtyCM→CaiMepStockStatus · QtyHM→HoChiMinhStockStatus · Comment→Remark · Model→ModelList
    //   · List_ItemCode_New→TSTPartCodeNew · List_ItemCode_Old→TSTPartCodeOld

    // ===== 🔴 #262 ĐÍNH CHÍNH #245 — phân biệt CỘT DB vs CỘT CỦA DATATABLE API =====
    // Sweep #261 chỉ ra `TblTSTMSTPart` (DbDefine.cs:695-721) là **lớp hằng của BẢNG DB** `TST_Mst_Part`.
    // Đối chiếu với bảng ánh xạ Bravo mà #245 dùng (`TST_Mst_Part_Get01`, BizCarSv.Service.cs:18517-18567):
    //   bảng đó dựng một `DataTable` **TRONG BỘ NHỚ** để TRẢ VỀ client — KHÔNG phải schema bảng DB.
    // ⇒ Hai hệ tên cho cùng khái niệm:
    //     DataTable API (#245)      |  CỘT DB THẬT (DbDefine)
    //     `TSTPriceWarranty`        |  `TSTWarrantyPrice`
    //     `TSTPriceUrgent`          |  `TSTUrgentPrice`
    //     `TSTPriceList` (StandardPrice) |  *(không có cột DB tương ứng)*
    //     `TaxRate`, `*StockStatus`, `TSTPartCodeNew/Old` |  *(không có cột DB — chỉ có ở phản hồi Bravo)*
    // ⇒ GIỮ các cột #245 (chúng phục vụ dữ liệu trả từ Bravo) nhưng **bổ sung cột DB thật còn thiếu**,
    //   và ghi rõ nhóm nào là gì để lượt sau không nhầm khi map schema.

    // --- 13 CỘT DB THẬT còn thiếu (TblTSTMSTPart) ---
    public decimal? TSTPriceBefore { get; set; }     // TSTPRICEBEFORE — giá kỳ trước
    public decimal? TSTCost { get; set; }            // TSTCOST
    public DateTime? DateEffect { get; set; }        // DATEEFFECT — ngày hiệu lực giá
    public string? TSTUnit { get; set; }             // TSTUNIT — đơn vị theo NCC, KHÁC `Unit`
    public string? GroupCode { get; set; }           // GROUPCODE (MiniHTC đang có `PartGroup` — giữ cả hai)
    public string? GroupName { get; set; }
    public string? TypeCode { get; set; }            // TYPECODE (MiniHTC đang có `PartType`)
    public string? TypeName { get; set; }
    public decimal? TSTWarrantyPrice { get; set; }   // TSTWARRANTYPRICE — tên DB của giá bảo hành
    public decimal? TSTUrgentPrice { get; set; }     // TSTURGENTPRICE   — tên DB của giá gấp
    public string? UpdateBy { get; set; }            // UPDATEBY
    public DateTime? UpdateDateTime { get; set; }    // UPDATEDATETIME
    public string? LUBy { get; set; }                // LUBY (đã có LUDTime)

    /// <summary>MinOrderQuantity — số lượng đặt tối thiểu của NCC. (có ở CẢ hai hệ tên)</summary>
    public decimal? MinOrderQuantity { get; set; }

    // --- 🔴 BỐN loại giá: port cũ gộp còn MỘT ⇒ mất giá niêm yết / giá gấp / giá bảo hành ---
    public decimal? TSTPriceList { get; set; }       // StandardPrice — giá niêm yết
    public decimal? TSTPriceUrgent { get; set; }     // UrgentPrice   — giá đặt GẤP
    public decimal? TSTPriceWarranty { get; set; }   // WarrantyPrice — giá dùng cho BẢO HÀNH

    /// <summary>TaxRate — thuế suất do NCC trả về (khác `VAT` vốn của bảng PT nội bộ).</summary>
    public decimal? TaxRate { get; set; }

    // --- 🔴 tồn kho theo BA KHO của NCC (tên cột nguồn là *StockStatus* nhưng giá trị là Qty*) ---
    public string? DongAnhStockStatus { get; set; }      // QtyDA
    public string? CaiMepStockStatus { get; set; }       // QtyCM
    public string? HoChiMinhStockStatus { get; set; }    // QtyHM

    // --- mã thay thế: NCC trả về DANH SÁCH mã mới/cũ của cùng phụ tùng ---
    public string? TSTPartCodeNew { get; set; }
    public string? TSTPartCodeOld { get; set; }

    public string? Remark { get; set; }      // Comment
    public string? ModelList { get; set; }   // Model — danh sách xe áp dụng
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }

    public string? PartGroup { get; set; }
    public string? PartType { get; set; }
    public string FlagActive { get; set; } = "1";
    /// <summary>#212 parity: nguồn ghi mốc đồng bộ vào `LUDTime` (không phải `UpdatedAt` của port).</summary>
    public DateTime? LUDTime { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thư viện kỹ thuật (Ser_InsuranceContract) — port 1:1 FrmInsuranceContractCreate/Search (TCMotor DMSCarSv/Admin). Theo mã HĐ (auto): số HĐ + loại thanh toán + hiệu lực + hãng BH (InsNo→SerInsurance) + hạn mức.</summary>
public sealed class SerInsuranceContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InContractCode { get; set; } = "";
    public string? InContractNo { get; set; }
    public string? TypePayment { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public string? InsNo { get; set; }
    public decimal PaymentLimit { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Đơn giá thuê thiết bị GPS (Mst_UnitPriceGPS) — port 1:1 FrmMst_UnitPriceGPS (2010.HTC/Sales/Product). Theo số hợp đồng: đơn giá GPS + ngày hiệu lực. Upsert-by-ContractNo.</summary>
public sealed class MstUnitPriceGPS
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractNo { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public DateTime? EffStartDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }

    // ===== #187 parity `Mst_UnitPriceGPS_Update` (DataWH/Biz.HTC.WH.cs:196766, csproj 272) =====
    /// <summary>Nhật ký sửa cuối — nguồn ghi ở lệnh `_Update` (field-mask ba cột).</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Lệnh xuất kho phụ tùng theo đơn KH (Ser_InvStockOutOrder header) — port 1:1 FrmStockOutOrderCreate (TCMotor DMSCarSv/Inventory). Header: số lệnh + ngày + khách hàng; state-machine Created→CreateStockOut/Finished/Rejected. KHÁC phiếu xuất thẳng (/api/stockouts).</summary>
public sealed class SerStockOutOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderNo { get; set; } = "";
    public DateTime? OrderDate { get; set; }
    public string? CusName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Note { get; set; }
    public decimal TotalQty { get; set; }
    public string Status { get; set; } = "Created";
    public string SourceType { get; set; } = "CUS";   // CUS = đơn khách hàng; RO = theo lệnh sửa chữa (FrmStockOutOrderSvCreate)
    public string? RONo { get; set; }                  // số lệnh sửa chữa (khi SourceType=RO)
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // ===== 🔴 #263: 9 cột nguồn `TblSerInvStockOutOrder` (DbDefine.cs:1306-1320) mà port cũ THIẾU =====
    // Tìm bằng sweep `_audit/sweep_tblconst_tail.js` (#261).
    public DateTime? RequestDeliveryTime { get; set; }  // REQUESTDELIVERYTIME — ngày YÊU CẦU giao
    public string? Priority { get; set; }               // PRIORITY — độ ưu tiên
    public string? BackOrderIndex { get; set; }         // BACKORDERINDEX — lần đặt lại (hàng thiếu)

    /// <summary>STATUSTEXT — nguồn lưu **cả nhãn** cạnh mã trạng thái (xem bộ hằng ở Program.cs).</summary>
    public string? StatusText { get; set; }

    public string? UserCode { get; set; }               // USERCODE — người lập
    public string? CusID { get; set; }                  // CUSID — mã khách (khác CusName đang có)
    public string? DealerCode { get; set; }             // DEALERCODE
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    /// <summary>
    /// 🔴 #293 STOCKOUTTYPE — loại lệnh xuất (`TConst.Ser_StockOutType`): `"1"` StockService · `"2"` StockNormal.
    /// Cột này **quyết định có áp guard sửa hay không**: luật "đã có phiếu xuất thì cấm sửa" CHỈ áp cho
    /// lệnh xuất **THƯỜNG** (`"2"`), lệnh xuất **DỊCH VỤ** (`"1"`) KHÔNG bị chặn.
    /// ⚠️ Chú thích trong lớp hằng nguồn ghi nhầm sang nghĩa khác ("Nhập, còn số lượng" / "Đã xuất hết")
    /// — đó là comment của một hằng khác bị chép sang; giá trị "1"/"2" mới là thứ dùng thật.
    /// </summary>
    public string? StockOutType { get; set; }

    /// <summary>#293 DESCRIPTION — mô tả lệnh xuất. Nguồn `SerStockOutOrderUpdate` ghi cột này
    /// (KHÁC `Note` port cũ đang dùng).</summary>
    public string? Description { get; set; }
}

/// <summary>
/// 🔴 #294 BẢNG NỐI LỆNH XUẤT ↔ PHIẾU XUẤT — `Ser_Inv_StockOutOrderStockOut`
/// (lớp hằng `TblSerInvStockOutOrderStockOut`, `DbDefine.cs:1378-1387`).
/// Đây chính là bảng mà #293 **chưa có** nên phải dùng xấp xỉ; nay port thật ⇒ gỡ xấp xỉ đó.
///
/// 🔴 QUAN HỆ **NHIỀU-NHIỀU**: một lệnh xuất có thể sinh **NHIỀU** phiếu xuất (giao nhiều đợt —
/// xem `BackOrderIndex` "lần đặt lại"), nên nguồn tách hẳn bảng nối thay vì để một cột khoá ngoại.
/// Người ghi: `SerStockOutOrderStockOutCreate` (`StockOut.cs:7952`), được gọi từ **`SerStockOutCreate`**
/// (bản LIVE, `:540`) ⇒ **link sinh ra đúng lúc TẠO PHIẾU XUẤT từ một lệnh**.
/// (Hai chỗ gọi còn lại nằm trong `SerStockOutCreate_New20240115`/`SerStockOutUpdate_New20240115` — bản
/// CHẾT đã xác định ở #292.)
/// </summary>
public sealed class SerStockOutOrderStockOut
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>STOCKOUTORDERID / STOCKOUTORDERNO — lệnh xuất (giữ CẢ khoá lẫn số, đúng như nguồn).</summary>
    public long StockOutOrderId { get; set; }
    public string? StockOutOrderNo { get; set; }

    /// <summary>STOCKOUTID / STOCKOUTNO — phiếu xuất sinh ra từ lệnh trên.</summary>
    public long StockOutId { get; set; }
    public string? StockOutNo { get; set; }

    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng chi tiết lệnh xuất kho theo đơn (Ser_InvStockOutOrderDetail) — thuộc SerStockOutOrder. Mã PT + tên + ĐVT + SL yêu cầu.</summary>
public sealed class SerStockOutOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long OrderId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public decimal OrderQuantity { get; set; }
}

/// <summary>Chứng chỉ nhân viên bán hàng (Mst_SalesManCertificate) — port 1:1 FrmMst_SalesManCertificateCreate/Mng/Update (2010.HTC/Admin/Product). Gán chứng chỉ cho NVBH theo mã Hyundai + loại NV + hiệu lực. KHÁC catalog Certificate (code/name) — đây là bản GÁN có hạn. Upsert by (SMHyundaiCode+CertificateCode).</summary>
public sealed class SalesManCertificate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>
    /// Số chứng chỉ (SMCERNO) — ĐỊNH DANH THẬT của bản ghi ở nguồn.
    /// Một NVBH có thể được cấp CÙNG MỘT mã chứng chỉ NHIỀU LẦN (cấp lại / gia hạn),
    /// mỗi lần là một dòng riêng với khoảng hiệu lực riêng ⇒ khoá phải là SMCerNo,
    /// KHÔNG phải cặp (SMHyundaiCode, CertificateCode) — khoá cặp sẽ đè mất lịch sử cấp.
    /// </summary>
    public string SMCerNo { get; set; } = "";

    public string SMHyundaiCode { get; set; } = "";
    public string CertificateCode { get; set; } = "";
    public string? CertificateName { get; set; }
    public string? SMType { get; set; }
    public string? DepartmentCode { get; set; }
    public string? DealerCode { get; set; }                 // đại lý NVBH (TblMst_SalesManCertificate.DealerCode — WinForm line 196)

    /// <summary>Ngày cấp chứng chỉ (EFFSTARTCERTIFICATE) — bắt buộc.</summary>
    public DateTime? EffStartDate { get; set; }

    /// <summary>Ngày kết thúc chứng chỉ (EFFENDCERTIFICATE) — KHÔNG bắt buộc (nguồn đã comment phần bắt buộc).</summary>
    public DateTime? EffEndDate { get; set; }

    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>Khóa đào tạo NVBH (Mst_Training header) — port 1:1 FrmMst_TrainingCreate/Mng/Update (2010.HTC/Admin/Product). Khóa: mã + tên + phòng ban + đại lý + giảng viên. KHÁC catalog "Training" (code/name) — đây là khóa có giảng viên + danh sách tham gia. Upsert-by-TrainingUserCode.</summary>
public sealed class TrainingCourse
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TrainingUserCode { get; set; } = "";
    public string? TrainingName { get; set; }
    public string? Department { get; set; }
    public string? DealerCode { get; set; }
    public string? TrainerCode { get; set; }
    public string? TrainerName { get; set; }
    public string? Description { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Bản ghi tham gia đào tạo (Mst_TrainingDtl) — port 1:1 FrmMst_TrainingDtlCreate/Mng/Update. Thuộc khóa: NVBH (mã Hyundai) + ngày tổ chức + kết quả đầu vào/ra. Guard trùng NVBH+ngày trong 1 khóa.</summary>
public sealed class TrainingParticipant
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CourseId { get; set; }

    /// <summary>
    /// Mã bản ghi tham gia (TRAININGDTLCODE) — định danh thật ở nguồn.
    /// Cùng một NVBH có thể tham gia LẠI một khoá ở đợt khác, nên khoá phải là mã này;
    /// khoá theo (khoá học, NVBH) là hẹp hơn nguồn.
    /// </summary>
    public string TrainingDtlCode { get; set; } = "";

    public string? SMHyundaiCode { get; set; }

    /// <summary>Tên NVBH — nguồn trả kèm trong lưới tra cứu (cột MS_SMNAME).</summary>
    public string? SMName { get; set; }

    public DateTime? OrganizeDate { get; set; }
    public string? FormalityTraining { get; set; }  // hình thức đào tạo (TblMst_TrainingDtl.FormalityTraining — WinForm line 168)
    public string? Place { get; set; }              // địa điểm (TblMst_TrainingDtl.Place — WinForm line 170)
    public string? ResultIn { get; set; }
    public string? ResultOut { get; set; }

    /// <summary>Cờ hiệu lực (FLAGACTIVE) — nguồn có lọc theo cờ này trong Mst_TrainingDtl_Get.</summary>
    public string FlagActive { get; set; } = "1";

    /// <summary>Vết cập nhật (LOGLUDATETIME / LOGLUBY).</summary>
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>Đề nghị thu hồi hồ sơ xe (RD_ReqRedeem header) — port 1:1 FrmNewRedeem/FrmMngRedeem (2010.HTC/Sales/Redeem). Header: số ĐN + ngày + đại lý; state-machine Created→Approved/Rejected. Chi tiết theo VIN, loại Trực tiếp/Bảo lãnh.</summary>
public sealed class RedeemRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqRedeemNo { get; set; } = "";
    public DateTime? CreatedDate { get; set; }
    public string? DealerCode { get; set; }
    public string? Note { get; set; }
    public int VinCount { get; set; }

    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG mã nguồn (`RD_ReqRedeem.DMReqStatus`, `TConst.Stage`):
    /// "P" chờ duyệt → "A" đã duyệt · "R" từ chối.
    /// ⚠️⚠️ Ở nghiệp vụ này header là **GIÁ TRỊ DẪN XUẤT**: nguồn duyệt **TỪNG DÒNG (theo VIN)**,
    /// rồi kiểm "còn dòng nào ở P không"; **hết dòng P thì header mới tự chuyển "A"**
    /// (`Biz.HTC.WH.cs:126588-126624`). KHÔNG phải header lan xuống dòng.
    /// Đọc dữ liệu cũ: Created→"P", Approved→"A", Rejected→"R".
    /// </summary>
    public string Status { get; set; } = "P";
    public string? CreatedBy { get; set; }
    /// <summary>Thời điểm header được duyệt — nguồn chỉ ghi khi TẤT CẢ dòng đã duyệt.</summary>
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // --- #140 parity RD_ReqRedeem ---
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng VIN của đề nghị thu hồi (RD_ReqRedeemDtl) — thuộc RedeemRequest. VIN + xe + loại thu hồi (DIRECT=Trực tiếp / GUARANTEE=Bảo lãnh).</summary>
public sealed class RedeemRequestLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RequestId { get; set; }
    public string? VIN { get; set; }
    public string? CarId { get; set; }
    /// <summary>`TypeDMReq` (`TConst.RDType`): DIRECT trực tiếp · GUARANTEE bảo lãnh.</summary>
    public string RedeemType { get; set; } = "DIRECT";

    /// <summary>
    /// 🔴 Trạng thái RIÊNG của DÒNG (`DMReqDtlStatus`) — **đây mới là nơi thao tác duyệt xảy ra**.
    /// Nguồn tạo ở "P", duyệt từng dòng thành "A".
    /// </summary>
    public string DMReqDtlStatus { get; set; } = "P";

    public string? DealerCode { get; set; }

    /// <summary>
    /// 🔴 Ngân hàng đang nhận thế chấp (`MortageBankCode`).
    /// Khi TẠO: **cấm là "HTC.HO"**. Khi DUYỆT giải chấp: nguồn **ghi đè thành "HTC.HO"**
    /// (`TConst.BANKHTC.HTCHO`) — tức **chuyển quyền thế chấp xe về HTC** — và ghi **cả trên bảng VIN**.
    /// </summary>
    public string? MortageBankCode { get; set; }

    /// <summary>Mã danh sách hồ sơ xe liên quan (`DRListCode`).</summary>
    public string? DRListCode { get; set; }

    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Remark { get; set; }

    // --- #140 parity RD_ReqRedeemDtl ---
    /// <summary>Ngày đề nghị giải chấp của DÒNG (`DMReqDate`).</summary>
    public DateTime? DMReqDate { get; set; }
    /// <summary>
    /// 🔴 MẮT NỐI sang đề nghị THẾ CHẤP (`ReqRMNo`) — cặp với `RM_ReqMortgageDtl.ReqDMNo`.
    /// Nhờ cặp này mà duyệt giải chấp biết phải đóng dòng thế chấp nào về "F".
    /// </summary>
    public string? ReqRMNo { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Đề nghị giao hóa đơn/hồ sơ thu hồi (RD_ReqInvoice header) — port 1:1 FrmNewRDInvoice/FrmMngRDInvoice (2010.HTC/Sales/Redeem). Header: số ĐN + ngày + đại lý; state-machine Created→Approved/Rejected. Chi tiết theo VIN, loại nhận: Đại lý / Ngân hàng BL / Ngân hàng LC.</summary>
public sealed class RedeemInvoiceRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqRDInvoiceNo { get; set; } = "";
    public DateTime? CreatedDate { get; set; }
    public string? DealerCode { get; set; }
    public string? Note { get; set; }
    public int VinCount { get; set; }

    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG mã nguồn (`RD_ReqInvoice.ReqIVStatus`, `TConst.Stage`): "P" → "A" · "R".
    /// ⚠️ Header là **GIÁ TRỊ DẪN XUẤT** — chỉ "A" khi mọi dòng đã duyệt (giống đề nghị giải chấp #55).
    /// Đọc dữ liệu cũ: Created→"P", Approved→"A", Rejected→"R".
    /// </summary>
    public string Status { get; set; } = "P";
    public string? CreatedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    // ===== #160 parity + side-effect `RD_ReqInvoiceDtlApprove_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:128014, csproj 272; vùng md5 1e58bf10 khớp 2 máy) =====
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng VIN của đề nghị giao HĐ thu hồi (RD_ReqInvoiceDtl) — thuộc RedeemInvoiceRequest. VIN + xe + loại ĐN giao (DEALER=Đại lý / BANKBL=Ngân hàng BL / BANKLC=Ngân hàng LC).</summary>
public sealed class RedeemInvoiceRequestLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RequestId { get; set; }
    public string? VIN { get; set; }
    public string? CarId { get; set; }
    /// <summary>`TypeRDReqIv` — nơi nhận hồ sơ: DEALER đại lý · BANKBL ngân hàng bảo lãnh · BANKLC ngân hàng LC.</summary>
    public string ReqType { get; set; } = "DEALER";

    /// <summary>
    /// 🔴 Trạng thái RIÊNG của DÒNG (`RDReqIvDtlStatus`) — **nơi thao tác duyệt thực sự xảy ra**.
    /// Nguồn tạo ở "P", duyệt từng dòng thành "A"; header chỉ chuyển "A" khi **hết dòng "P"**
    /// (comment nguyên văn của nguồn: *"Nếu Dtl đã được duyệt hết thì chuyển trạng thái Mng"*,
    /// `Biz.HTC.WH.cs:128126-128142`).
    /// </summary>
    public string RDReqIvDtlStatus { get; set; } = "P";

    public string? DealerCode { get; set; }
    /// <summary>Ngân hàng đang nhận thế chấp (`MortageBankCode`).
    /// ⚠️ KHÁC đề nghị giải chấp (#55): ở nghiệp vụ giao hồ sơ này nguồn **KHÔNG** ghi đè thành "HTC.HO" khi duyệt.</summary>
    public string? MortageBankCode { get; set; }

    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? Remark { get; set; }

    // Thông tin hoá đơn mang từ bộ thực thể song trùng cũ sang, để không mất khả năng đã có.
    public string? HTCInvoiceNo { get; set; }
    public string? InvoiceNoFactory { get; set; }
    public string? TCGInvoiceNo { get; set; }

    // ===== #160 parity + side-effect `RD_ReqInvoiceDtlApprove_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:128014, csproj 272; vùng md5 1e58bf10 khớp 2 máy) =====
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>NVBH đại lý + duyệt BĐH (Mst_DlSalesMan) — port 1:1 FrmMngSalesManApproved/FrmMngSalesManHTC (2010.HTC/SalesDealer). Đại lý đăng ký NVBH → HTC/BĐH duyệt. 2 trạng thái: SMStatus (thử việc/chính thức/nghỉ/CTV) + BDHStatus (duyệt). KHÁC master SalesMan đơn giản. Upsert-by-SMCode.</summary>
public sealed class DealerSalesMan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SMCode { get; set; } = "";
    public string? SMHyundaiCode { get; set; }
    public string? SMName { get; set; }
    public string? DealerCode { get; set; }
    public string? SMEmail { get; set; }
    public string? SMPhoneNo { get; set; }
    public string? IdentityCardNo { get; set; }
    public string? SMGender { get; set; }
    public string? ProvinceCode { get; set; }
    public string? QualificationCode { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string SMStatus { get; set; } = "THUVIEC";   // THUVIEC/CHINHTHUC/NGHIVIEC/CTVIEN
    /// <summary>
    /// 🔴 #204 — `TConst.BDHStatus` (Const.Main.cs:1227) chỉ có HAI giá trị: **`CHALLENGE`** (đang thử thách)
    /// và **`APPOINT`** (đã bổ nhiệm). Bộ `Pending/Approved/Rejected` của port cũ KHÔNG có ở nguồn, và ý nghĩa
    /// cũng khác: đây là trạng thái **BỔ NHIỆM**, không phải một vòng duyệt có nhánh từ chối.
    /// </summary>
    public string BDHStatus { get; set; } = "CHALLENGE";
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Ghi nhận khách đến xem xe (Dlr_CtmVisit) — port 1:1 FrmCusVisit (2010.HTC/Sales/RetailContract). Log walk-in showroom: mã ghi (theo thời điểm) + đại lý + giới tính + độ tuổi + model quan tâm. Insert-only, không sửa/xóa.</summary>
public sealed class CustomerVisit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CusVisitCode { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? Gender { get; set; }
    public string? RangeAgeCode { get; set; }
    public string? ModelCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Master thương hiệu xe dịch vụ (Ser_MST_TradeMark) — port 1:1 FrmTradeMarkCreate/Search (TCMotor DMSCarSv/Admin). Mã + tên thương hiệu. Upsert-by-code + toggle.</summary>
public sealed class ServiceTradeMark
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TradeMarkCode { get; set; } = "";
    public string? TradeMarkName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Thư viện kỹ thuật (Ser_Technical_Library) — port 1:1 FrmSer_Technical_Library (TCMotor DMSCarSv). Kho tri thức sửa chữa lặp: triệu chứng / nguyên nhân / giải pháp theo model/xe.</summary>
public sealed class TechnicalLibrary
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TechnicalLibraryCode { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? PlateNo { get; set; }
    public string? Model { get; set; }
    public string? Engine { get; set; }
    public string? Gear { get; set; }
    public string? ReRepairType { get; set; }
    public string? ReRepairRemark { get; set; }    // triệu chứng
    public string? ReRepairReason { get; set; }    // nguyên nhân
    public string? ReRepairSolution { get; set; }  // giải pháp
    public string? ExclusionTest { get; set; }
    public string IsActive { get; set; } = "1";
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>Master nhà cung cấp phụ tùng (Ser_MST_Supplier) — port 1:1 FrmMstSupplierCreate/Search (TCMotor DMSCarSv). Mã + tên + địa chỉ + SĐT + fax.</summary>
public sealed class SerMstSupplier
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SupplierCode { get; set; } = "";
    public string? SupplierName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    /// <summary>#574 §12 Hai cột người liên hệ — nguồn `SerSupplierDebitDetailGet` chọn `d.ContactName`,
    /// `d.ContactPhone` ở bảng danh mục nhà cung cấp (bản bảo hiểm sinh đôi chọn `TelePhone`/`Email`).</summary>
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Phiếu điều chỉnh tồn kho (header) — port 1:1 FrmStockAdjCreate/Search (TCMotor DMSCarSv). Điều chỉnh SL tồn phụ tùng, duyệt theo trạng thái.</summary>
public sealed class StockAdj
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockAdjNo { get; set; } = "";
    public string? StorageCode { get; set; }
    public string? Remark { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG `TConst.Ser_StockAdj` cua nguon (TCMotor `Const.Main.cs:279-283`):
    /// **"0" = Mới tạo · "1" = Kết thúc** — CHỈ 2 trạng thái.
    /// ⚠️ Port cũ dùng "Draft"/"Approved"/**"Rejected"** — sai mã, và **"Rejected" là trạng thái BỊA**:
    /// nguồn KHÔNG có nhánh huỷ phiếu điều chỉnh (khác phiếu xuất kho vốn có FrmSOReject).
    /// </summary>
    public string AdjStatus { get; set; } = "0";
    /// <summary>Ngày điều chỉnh do NGƯỜI DÙNG nhập (`Ser_Inv_StockOutAdj.StockOutDate`) — không phải ngày tạo bản ghi.</summary>
    public DateTime? StockOutDate { get; set; }
    /// <summary>Đại lý thực hiện (`Ser_Inv_StockOutAdj.DealerCode`) — trục phân tách dữ liệu của nguồn.</summary>
    public string? DealerCode { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    /// <summary>Thời điểm KẾT THÚC phiếu (status "1"), lúc tồn kho thực sự bị điều chỉnh.</summary>
    public DateTime? ApprovedAt { get; set; }

    // ===== #176 parity `Ser_StockAdj_Update` (BizCarSv.Inventory.Stock.cs:5300) =====
    /// <summary>Nhật ký sửa cuối (`LogLUDateTime`/`LogLUBy`) — nguồn ghi ở CẢ `_Create` lẫn `_Update`
    /// trên bảng `Ser_Inv_StockAdj` (KHÁC bảng `Ser_Inv_StockOutAdj` của cụm `StockOutAdjCreate`).</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết phiếu điều chỉnh tồn kho — port 1:1 StockAdj detail (TCMotor DMSCarSv).</summary>
public sealed class StockAdjLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StockAdjId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public decimal QtyBalance { get; set; }   // SL tồn hiện tại
    public decimal QtyAdjust { get; set; }     // SL điều chỉnh (+/-)
    /// <summary>
    /// 🔴 Kho CÂN ĐỐI (`Ser_Inv_StockAdjDetail.BalanceLocationID`) — nơi BỊ TRỪ số lượng khi kết thúc phiếu.
    /// Port cũ chỉ có 1 `StorageCode` ở header ⇒ **mất trục vị trí theo DÒNG** của nguồn.
    /// </summary>
    public string? BalanceLocation { get; set; }
    /// <summary>Kho ĐÍCH (`Ser_Inv_StockAdjDetail.InStockLocationID`) — nơi số lượng được chuyển sang.</summary>
    public string? InStockLocation { get; set; }
}

/// <summary>Master loại công việc dịch vụ (Ser_MST_ServiceType) — port 1:1 FrmServiceTypeCreate/Search (TCMotor DMSCarSv). Tên loại công việc + cờ hoạt động.</summary>
public sealed class SerServiceType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TypeName { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master kho dịch vụ (Ser_Stock) — port 1:1 FrmStockCreate/Search (TCMotor DMSCarSv). Mã kho + tên + liên lạc + địa chỉ + email.</summary>
public sealed class SerStock
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockNo { get; set; } = "";
    public string? StockName { get; set; }
    public string? Contact { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master loại phụ tùng dịch vụ (Ser_MST_PartType) — port 1:1 FrmPartTypeCreate/Search (TCMotor DMSCarSv). Tên loại phụ tùng + cờ hoạt động.</summary>
public sealed class SerPartType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TypeName { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master kỳ khảo sát JD Power (Ser_MST_JDPowerTerm) — port 1:1 FrmJDPowerTermCreate/Search (TCMotor DMSCarSv). Mã kỳ + nội dung + ngày bắt đầu/kết thúc.</summary>
public sealed class JDPowerTerm
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string JDPTermCode { get; set; } = "";
    public string? JDPTermName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Chi tiết thanh toán PDI theo xe (Pmt_PaymentPDIDetail) — port 1:1 FrmSuaThanhToanPDI (2010.HTC). Sửa ngày nhập kho/xuất kho từng VIN; StorageDays = xuất - nhập. Upsert theo VIN.</summary>
public sealed class PdiStoragePayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorExtName { get; set; }
    public string? StorageCodeInit { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? StoreDate { get; set; }        // ngày nhập kho
    public DateTime? DeliveryOutDate { get; set; }  // ngày xuất kho
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Cập nhật trạng thái xe — port 1:1 FrmUpdateCar_Status (2010.HTC). Batch cập nhật TTCStatus (hoàn thành TT chậm) + CPTCStatus theo CarId, upsert theo CarId.</summary>
public sealed class CarStatusUpdate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";
    public string TTCStatus { get; set; } = "0";
    public string CPTCStatus { get; set; } = "0";
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Trạng thái hoạt động của xe + thông tin huỷ xe (Car_Car.FlagActive / CarCancel* —
/// port 1:1 FrmCapNhatTTHuyXe, 2010.HTC TERP.HTCClient/Views/Sales).
/// Màn nhập Excel danh sách mã xe rồi HUỶ HÀNG LOẠT hoặc PHỤC HỒI HÀNG LOẠT.
/// Khác <see cref="CarCancel"/> (quy trình huỷ 1 xe có mã phiếu + duyệt): đây là thao tác
/// kỹ thuật theo lô, phục vụ nghiệp vụ map VIN.
/// </summary>
public sealed class CarActiveStatus
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Mã xe — khoá upsert của màn (nguồn nhập Excel đúng 1 cột CarId).</summary>
    public string CarId { get; set; } = "";

    /// <summary>VIN đã map cho xe. CÓ VIN thì cấm cả huỷ lẫn phục hồi (guard của nguồn).</summary>
    public string? Vin { get; set; }

    /// <summary>"1" = đang hoạt động, "0" = đã huỷ (TConst.Flag.Active/Inactive).</summary>
    public string FlagActive { get; set; } = "1";

    public string? CarCancelRemark { get; set; }
    public DateTime? CarCancelDate { get; set; }
    public string? CarCancelBy { get; set; }

    /// <summary>Loại huỷ — nguồn luôn ghi "NONE" ở cả hai thao tác huỷ và phục hồi.</summary>
    public string CarCancelType { get; set; } = "NONE";

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
}

/// <summary>Cập nhật spec theo CarID — port 1:1 FrmUpdateSpec_CarID (2010.HTC). Batch đổi SpecCode cho xe (import Excel), upsert theo CarId.</summary>
public sealed class CarSpecUpdate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Thông tin dữ liệu đăng kiểm/thị phần (Mst_RegistrationInfo) — port 1:1 FrmMst_ThongTinDuLieuDangKiem_ThiPhan (2010.HTC). Số liệu đăng kiểm theo (năm × tỉnh): SL + % + tổng tiền.</summary>
public sealed class RegistrationInfo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RegistYear { get; set; } = "";
    public string ProvinceCode { get; set; } = "";
    public string? ProvinceName { get; set; }
    public int Qty { get; set; }
    public decimal RegistPercent { get; set; }
    public decimal TotalAmount { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master giấy chứng nhận thùng theo loại xe (Mst_CabinCertificate) — port 1:1 FrmQLTTXeXuatHoaDon (2010.HTC). Số GCN thùng + loại xe.</summary>
public sealed class CabinCertificate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CabinCertificateNo { get; set; } = "";
    public string? CarType { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master loại thiết bị (Mst_DeviceType) — port 1:1 FrmQLLoaiThietBi (2010.HTC). Mã + tên + cờ hoạt động.</summary>
public sealed class DeviceType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DeviceTypeCode { get; set; } = "";
    public string? DeviceTypeName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Loại thiết bị theo spec xe (Mst_DeviceType_Spec) — port 1:1 FrmQLLoaiThietBiTheoXe (2010.HTC). Gán loại thiết bị áp cho từng spec xe; khóa kép (DeviceTypeCode × SpecCode).</summary>
public sealed class DeviceTypeSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DeviceTypeCode { get; set; } = "";
    public string? DeviceTypeName { get; set; }
    public string SpecCode { get; set; } = "";
    public string? SpecDescription { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }

    // ===== #188 parity `Mst_DeviceType_Spec_Update` / `_Delete` (DataWH/Biz.HTC.WH.cs, csproj 272) =====
    /// <summary>Nhật ký sửa cuối — nguồn `_Update` luôn ghi cặp này (field-mask một cột `FlagActive`).
    /// 🔴 Bảng này là master mà #162 dùng để SUY RA thiết bị của xe khi lập packing list
    /// (`JOIN Mst_DeviceType_Spec` theo `ActualSpec`, lọc `FlagActive='1'`).</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Số tiền chiết khấu TT được duyệt theo VIN (PRD_PaymentReqDiscount_VIN) — port 1:1 FrmImportExl_PaymentReqDiscount (2010.HTC). Import số tiền HTC duyệt cho từng VIN trong đề nghị chiết khấu; upsert theo (PRDiscountNo × VIN).
/// Mở rộng thêm các cột chi tiết VIN (CarId/SpecCode/DeliveryDate/DlrContractNo/SMName/UnitPriceActual/AmountDealerRequest/CustomerName) để phục vụ FrmPayReDiscount/FrmMngPaymentReqDiscountDealer (đề nghị + duyệt 2 cấp).</summary>
public sealed class PaymentReqDiscountVin
{
    // 🔴 #128: tên lớp nói "…_VIN" nhưng **bảng nguồn tên đó KHÔNG TỒN TẠI**.
    //    Bảng thật là **`PRD_PaymentReqDiscountDtl`** (ghi tại `DataWH/Biz.HTC.WH.My.cs:1312`,
    //    trong `PRD_PaymentReqDiscount_Create_New20190507`). Giữ tên lớp để không phá API,
    //    nhưng từ nay tra cứu nguồn phải tìm `…Dtl`, đừng tìm `…_VIN`.
    // 🔴 TWIN lệch bit ở BA hàm: WS 32-bit gọi `_Create` / `_Get` / `_UpdateMulti` (nằm trong
    //    `DataWH/**Delete.**Biz.HTC.WH.My.cs` — csproj `<None>`, **FILE CHẾT**), còn WS 64-bit gọi
    //    `_Create_New20190507` / `_Get_New20210527` / `_UpdateMulti_New20190809` ở
    //    `Biz.HTC.WH.My.cs` (csproj `<Compile>`, **LIVE**). Canonical = bản 64-bit.
    //    Cùng mẫu đã gặp ở #127 (luật C0-centesimusvigesimusseptimus).
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PRDiscountNo { get; set; } = "";
    public string VIN { get; set; } = "";
    /// <summary>🔴 Nguồn để **NULL khi tạo** (`DBNull.Value`, dòng 1269) — chỉ điền khi HTC duyệt.</summary>
    public decimal? AmountHTCAppr { get; set; }
    /// <summary>Ngày HTC duyệt cho dòng này (`HTCApprDate`) — nguồn NHẬN từ bảng đầu vào lúc tạo.</summary>
    public DateTime? HTCApprDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CarId { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    public DateTime? DeliveryEndDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DlrContractNo { get; set; }
    public string? SMName { get; set; }
    public DateTime? CusInvoiceDate { get; set; }
    public decimal UnitPriceActual { get; set; }
    public decimal AmountDealerRequest { get; set; }
    public string? CustomerName { get; set; }
}

/// <summary>Đề nghị chiết khấu TT theo VIN — header (PRD_PaymentReqDiscount) — port 1:1 FrmPayReDiscount (tạo đề nghị, đại lý) + FrmMngPaymentReqDiscountDealer (duyệt 2 cấp, HTC), 2010.HTC/Sales.
/// Status: Draft(đại lý lập)→Approved1→Approved2(HTC duyệt 2 cấp)/Cancelled.</summary>
public sealed class PaymentReqDiscount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PRDiscountNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public string SPCode { get; set; } = "";
    public string? Remark { get; set; }
    /// <summary>
    /// 🔴 #128 SỬA TAXONOMY: port cũ dùng `"Draft"` — **SAI**. Nguồn dùng `TConst.PRDiscountStatus`
    /// (`Const.Main.cs:1114-1120`): **"P" chờ duyệt · "A1" duyệt cấp 1 · "A2" duyệt cấp 2 · "C" huỷ**.
    /// Đây là bộ 4 giá trị RIÊNG của cụm chiết khấu, **không phải** `TConst.Stage`
    /// (đúng luật C0-centesimusnonus: mỗi cụm có thể có hằng trạng thái riêng).
    /// Kèm câu UPDATE migrate dữ liệu cũ trong Seeder.
    /// </summary>
    public string Status { get; set; } = "P";
    /// <summary>#128: nguồn lấy từ `dtDB_AreaDealer.AreaCodeDealer` khi tạo (Biz.HTC.WH.My.cs:1250).</summary>
    public string? AreaCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? Approve1At { get; set; }
    /// <summary>Người duyệt cấp 1 (`Appr1By`).</summary>
    public string? Appr1By { get; set; }
    public DateTime? Approve2At { get; set; }
    /// <summary>Người duyệt cấp 2 (`Appr2By`).</summary>
    public string? Appr2By { get; set; }
    public DateTime? CancelledAt { get; set; }
    public string? CancelBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Mẫu hợp đồng của đại lý (Dlr_Mst_DealerContractForm) — port 1:1 FrmDlr_Mst_DealerContractForm (2010.HTC). Gán mã mẫu hợp đồng (ContractFNo) cho từng đại lý; upsert theo DealerCode.</summary>
public sealed class DealerContractForm
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ContractFNo { get; set; } = "";
    public string? ContractFName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Kho tại đại lý (Dlr_StorageLocal) — port 1:1 FrmDlr_StorageLocal (2010.HTC). Master kho địa phương của từng đại lý; khóa kép (DealerCode × StorageCode).</summary>
public sealed class DealerStorageLocal
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string StorageCode { get; set; } = "";
    public string? StorageName { get; set; }
    public string? DealerName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Cập nhật thông tin hóa đơn/thế chấp theo VIN — port 1:1 FrmCapNhatThongTinHoaDon (2010.HTC). Batch cập nhật số HĐ nhà máy, số vận đơn, số ĐK/NG, thông tin thế chấp NH + ngày giải chấp, upsert theo VIN.</summary>
public sealed class CarVinInvoiceInfo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? InvoiceNoFactory { get; set; }
    public DateTime? InvoiceFactoryDate { get; set; }
    public string? BillNo { get; set; }
    public string? CQNo { get; set; }
    public string? CONo { get; set; }
    public string? MortageBankCode { get; set; }
    public DateTime? MortageStartDate { get; set; }
    public DateTime? MortageEndDate { get; set; }
    public DateTime? RedeemDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master loại nhân viên bán hàng theo phòng ban (Mst_SalesManType) — port 1:1 FrmStaffType (TCMotor). Khóa kép (DepartmentCode × SMType).</summary>
public sealed class SalesManType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DepartmentCode { get; set; } = "";
    public string SMType { get; set; } = "";
    public string? SMTypeName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Hồ sơ phiếu thùng theo VIN — port 1:1 FrmUpdateCarVIN_CBInvoice (Car_VIN CB info, TCMotor). Batch cập nhật số/ngày phiếu xuất xưởng có thùng (CB) + ngày giao phiếu, upsert theo VIN.</summary>
public sealed class CarVinCBInfo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? CBNo { get; set; }
    public DateTime? CBDate { get; set; }
    public DateTime? DateDeliveryCBInvoice { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Thu hồi hóa đơn HTCV — port 1:1 FrmThuHoiHD (VAT_HTCVInvoice_Invoice_Deleted, TCMotor). Import danh sách số HĐ để thu hồi (đánh dấu đã xóa); ghi log + đếm khớp InvoiceLine.</summary>
public sealed class InvoiceRecall
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceNo { get; set; } = "";
    public string? Reason { get; set; }
    public string? RecalledBy { get; set; }
    public DateTime RecalledAt { get; set; }
    public bool MatchedInvoice { get; set; }   // có khớp 1 dòng InvoiceLine không
}

/// <summary>Gán loại hợp đồng cho xe — port 1:1 FrmUpdContractTypeForCar (TCMotor). Batch cập nhật ContractType theo CarId (import Excel), upsert theo CarId.</summary>
public sealed class CarContractType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public string? SOCode { get; set; }
    public string ContractType { get; set; } = "";
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Kích hoạt lại xe đã hủy — port 1:1 FrmReactiveCar (TCMotor). Chọn xe đã hủy (CarCancel Approved) → kích hoạt lại; ghi log + đổi CarCancel.Status='Reactivated'.</summary>
public sealed class CarReactivation
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? Reason { get; set; }
    public string? ReactivatedBy { get; set; }
    public DateTime ReactivatedAt { get; set; }
}

/// <summary>Cấu hình hóa đơn theo spec xe (Mst_CarInvoice) — port 1:1 FrmCarSpecInvoice (TCMotor). Ánh xạ SpecCode → thông tin xuất hóa đơn (spec HĐ, loại xe, số chỗ, loại phương tiện, VAT).</summary>
public sealed class CarInvoiceSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? SpecCodeInvoice { get; set; }
    public string? VehiclesType { get; set; }
    public int NumberOfSeats { get; set; }
    public string? CarType { get; set; }
    public decimal VAT { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Biên bản bàn giao hồ sơ (header) — port 1:1 FrmInBienBanBGHS (IN_BienBanBGHS, TCMotor). Bàn giao hồ sơ xe theo lô: mỗi xe kèm các số giấy tờ (CQ/CO/CB/tờ khai/bảo lãnh/HĐ) + SL bản gốc/sao y.</summary>
public sealed class DocHandoverMinute
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BBBGNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? DealerName { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? Remark { get; set; }
}

/// <summary>Chi tiết xe trong biên bản bàn giao hồ sơ — port 1:1 IN_BienBanBGHS detail (TCMotor).</summary>
public sealed class DocHandoverMinuteCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DocHandoverMinuteId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelProductionCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? EngineNo { get; set; }
    public string? CQNo { get; set; }          // số đăng kiểm
    public string? CONo { get; set; }          // số nguồn gốc
    public string? CBNo { get; set; }          // số PXX xe có thùng
    public string? DeclarationNo { get; set; } // tờ khai nhập khẩu
    public string? BankGuaranteeNo { get; set; }
    public string? BankName { get; set; }
    public string? DlrCtrNo { get; set; }
    public string? HTCInvoiceNo { get; set; }
    public string? TransportMinutesNo { get; set; }
    public int QtyInvoiceOriginal { get; set; }
    public int QtyTransportMnOriginal { get; set; }
    public int QtyTransportMnCopy { get; set; }
}

/// <summary>Lệnh cân bằng/điều chuyển kho (header) — port 1:1 FrmMngRearCBSC (Sto_RearrangeCB, TCMotor/Sales/Logistic). Lệnh điều chuyển xe giữa các kho theo danh sách VIN (from→to), duyệt theo trạng thái.</summary>
public sealed class StoRearCB
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StoRearCBNo { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG mã nguồn (`Sto_RearrangeCB.RearCBStatus`, `TConst.Stage`):
    /// "P" chờ duyệt → "A" đã duyệt · "R" từ chối · "C" huỷ (SQL nguồn lọc `not in ('R','C')`).
    /// Port cũ dùng chuỗi tự đặt Draft/Approved/Rejected. Đọc data cũ: Draft→"P", Approved→"A", Rejected→"R".
    /// </summary>
    public string RearCBStatus { get; set; } = "P";
    public string? ApprovedBy { get; set; }
    public string? Remark { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
}

/// <summary>Chi tiết xe trong lệnh cân bằng/điều chuyển kho — port 1:1 Sto_RearrangeCBDetail (TCMotor).</summary>
public sealed class StoRearCBDtl
{
    /// <summary>🔴 Trạng thái RIÊNG của DÒNG (`Sto_RearrangeCBDetail.RearCBDtlStatus`) — port cũ thiếu.
    /// Nguồn dùng chính cột này để chặn trùng: dòng đã có lệnh đóng thùng mà chưa bị "R"/"C" thì cấm tạo lệnh mới.</summary>
    public string RearCBDtlStatus { get; set; } = "P";
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StoRearCBId { get; set; }
    public string VIN { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? EngineNo { get; set; }
    public string? ColorCode { get; set; }
    public string? StorageCodeFrom { get; set; }
    public string? StorageCodeTo { get; set; }
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public string? CBReqNo { get; set; }
    public string? TenLoaiThung { get; set; }
    public string? Remark { get; set; }

    // ===== #159 side-effect `Sto_DlvMinutes_Approve_New20190416` (Biz.HTC.WH.cs:138340, csproj 272) =====
    // Duyệt biên bản giao xe GHI NGƯỢC "ngày xuất kho" lên CHỨNG TỪ NGUỒN của xe. Bốn nhánh theo loại
    // chứng từ, mỗi nhánh một CỘT KHÁC TÊN — đó là lý do port cũ bỏ sót cả ba.
    /// <summary>Ngày xuất kho thực tế của lệnh điều chuyển ĐÓNG THÙNG (`RearCBOutDate`) — nguồn chỉ ghi
    /// khi `TypeCB` KHÁC `TConst.CVTypeCB.ChuaDongThung`, và `RearCBDtlStatus` đang là "A" hoặc "F".</summary>
    public DateTime? RearCBOutDate { get; set; }

    // ===== #170 parity `Sto_DlvMinutes_UpdateDlvEndDate_New20181115` (BizHTC.Storage.DlvMinutes.cs:9329) =====
    /// <summary>Ngày điều chuyển đóng thùng XONG (`RearCBEndDate`).</summary>
    public DateTime? RearCBEndDate { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string? ConfirmBy { get; set; }
}

/// <summary>Phiên đăng nhập hệ thống — port 1:1 FrmMngSession (Session, TCMotor). Giám sát phiên đang mở + kill phiên hết hạn theo thời gian truy cập cuối.</summary>
public sealed class AppSession
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SessionId { get; set; } = "";
    public string UserCode { get; set; } = "";
    public DateTime DateTimeLogin { get; set; }
    public DateTime DateTimeLastAccess { get; set; }
    public string? LanguageCode { get; set; }
    public string? PartnerCode { get; set; }
    public string? PartnerUserCode { get; set; }
    public string? OtherInfo { get; set; }
}

/// <summary>Đề nghị cân bằng kho (header) — port 1:1 FrmMngCBReq (Sto_CBReq, TCMotor/Sales/Purchase). Đề nghị điều chuyển/cân bằng tồn kho xe theo danh sách VIN, duyệt theo trạng thái.</summary>
public sealed class StoCBReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CBReqNo { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG mã nguồn (`Sto_CBReq.CBReqStatus`, `TConst.Stage`): "P" → "A" · "R" · "C".
    /// Port cũ dùng chuỗi tự đặt. Đọc data cũ: Draft→"P", Approved→"A", Rejected→"R".
    /// </summary>
    public string CBReqStatus { get; set; } = "P";
    public string? ApprovedBy { get; set; }
    public string? Remark { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Chi tiết xe trong đề nghị cân bằng kho — port 1:1 Sto_CBReqDtl (TCMotor).</summary>
public sealed class StoCBReqDtl
{
    /// <summary>🔴 Trạng thái RIÊNG của DÒNG (`Sto_CBReqDetail.CBReqDtlStatus`) — port cũ thiếu.</summary>
    public string CBReqDtlStatus { get; set; } = "P";
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StoCBReqId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? EngineNo { get; set; }
}

/// <summary>Bảo hành xe tồn kho — port 1:1 FrmMngInv_CarWarranty (Inv_CarWarranty, TCMotor). Theo dõi mốc bảo hành theo VIN + gửi KH xác nhận bảo hành (CustomerConfirmDate).</summary>
public sealed class InvCarWarranty
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? PlateNo { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? DealerCode { get; set; }
    public string? DealerCodeBuyer { get; set; }
    public DateTime? ReceiveDate { get; set; }
    public DateTime? StoreDateExpired { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? WarrantyDate { get; set; }
    public DateTime? CustomerConfirmDate { get; set; }
    public DateTime? HTCVDateExpired { get; set; }
    public DateTime? DealerDateExpired { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master loại thùng đóng gói xe — port 1:1 FrmMst_LoaiThung (Mst_LoaiThung, TCMotor). LoaiThung = mã, TenLoaiThung = tên.</summary>
public sealed class LoaiThungMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string LoaiThung { get; set; } = "";
    public string? TenLoaiThung { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Thông báo giao xe (header) — port 1:1 FrmMngMsgDeliveryCar (Msg_MsgDlvCar, TCMotor/Sales/Logistic). HTC/NPP gửi thông báo giao xe tới đại lý; đại lý theo dõi + đánh dấu đã đọc.</summary>
public sealed class MsgDlvCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MsDlvNo { get; set; } = "";
    public DateTime MsDateTime { get; set; }
    public string DealerCode { get; set; } = "";
    public string MsType { get; set; } = "M";        // M = thông báo giao xe, C = hủy thông báo giao xe
    public string MsReadStatus { get; set; } = "N";  // N = chưa đọc, Y = đã đọc
    public string? SendBy { get; set; }
    public DateTime? ReadAt { get; set; }
}

/// <summary>Chi tiết xe trong thông báo giao xe — port 1:1 Msg_MsgDlvCarDtl (TCMotor).</summary>
public sealed class MsgDlvCarDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MsgDlvCarId { get; set; }
    public string CarId { get; set; } = "";
    public string? CarSpecCode { get; set; }
    public string? CarColorCode { get; set; }
    public DateTime? CQEndDate { get; set; }
}

/// <summary>Master loại hợp đồng — port 1:1 FrmMst_ContractType (Mst_ContractType, TCMotor). ContractType = mã, mô tả + cờ hoạt động.</summary>
public sealed class ContractTypeMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractType { get; set; } = "";
    public string? ContractTypeDesc { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Master thời gian chạy DOAT (Dealer Order Allocation Time) — port 1:1 FrmMst_DOATSettingTime (Mst_DOATSettingTime, TCMotor). Cấu hình 2 khung giờ auto tạo lệnh giao xe (First/Second run).</summary>
public sealed class DOATSettingTime
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DOATSTNo { get; set; } = "";
    public string FlagFirstRunTime { get; set; } = "0";
    public string FlagSecondRunTime { get; set; } = "0";
    public DateTime CreatedAt { get; set; }
}

/// <summary>Lịch sử chính sách đơn hàng theo xe — port 1:1 FrmMngHisOrderPolicy (Car_CarHisOrderPolicy, TCMotor/Sales/Purchase). Ghi nhận chính sách đơn hàng áp cho từng xe (theo SO + CarId) kèm log kiểm toán.</summary>
public sealed class CarHisOrderPolicy
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SOCode { get; set; } = "";
    public string CarId { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? CrtTypeCode { get; set; }
    public string? ColorCode { get; set; }
    public string? ColorName { get; set; }
    public string OrderPolicyCode { get; set; } = "";
    public string? OrderPolicyName { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? LogLUBy { get; set; }
    public DateTime LogLUDateTime { get; set; }
}

/// <summary>Định mức khuyến mãi theo thẻ hội viên × chương trình — port 1:1 FrmMember (Crd_Member promotion, TCMotor/Customer). QtyRemain = QtyAllocated - QtyUsed.</summary>
public sealed class CustomerPromotion
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CardNo { get; set; } = "";
    public string ProgramCode { get; set; } = "";
    public string? ProgramName { get; set; }
    public DateTime? EffDate { get; set; }
    public int QtyAllocated { get; set; }
    public int QtyUsed { get; set; }
    public string? Remark { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đề nghị giao xe (header: đại lý gửi HTC duyệt) — port 1:1 FrmNewDR/FrmHTCMngDR/FrmDRApproved (Dlr_DR, 2010.HTC/Sales).</summary>
public sealed class DeliveryRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DRNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public DateTime? RequestDate { get; set; }
    public string Status { get; set; } = "Draft"; // Draft -> Sent -> Approved / Rejected
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng xe trong đề nghị giao xe — port 1:1 FrmNewDR detail (Dlr_DRDetail, 2010.HTC).</summary>
public sealed class DeliveryRequestDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DeliveryRequestId { get; set; }
    public string CarId { get; set; } = "";
    public string? ModelCode { get; set; }
    public DateTime? DeliveryStartDate { get; set; }
    public string? Remark { get; set; }
}

/// <summary>Dự kiến đơn hàng theo tháng (header: đại lý/tháng/NV phụ trách) — port 1:1 FrmQuanLyDuKienDH (Plan_EstimateOrder, 2010.HTC/Sales).</summary>
public sealed class EstimateOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string EstOrderNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? MonthEstimate { get; set; }        // "yyyy-MM"
    public string? HtcStaffInCharge { get; set; }
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG `TConst.PLEOrdStatus` (`Const.Main.DMS40.cs:70-78`), cột
    /// `Plan_EstimateOrder.PLEOrdStatus`: **"P" chờ duyệt · "A1" duyệt cấp 1 · "A2" duyệt cấp 2**.
    /// ⚠️ Port cũ `Draft → Confirmed` = **một bước duy nhất**, mất cả hai cấp duyệt của nguồn
    /// (`Plan_EstimateOrder_Appr1` / `_Appr2`) lẫn đường **huỷ duyệt** (`_Cancel`).
    /// ⚠️ `PLEOrdStatus` là hằng RIÊNG của DMS40, không dùng chung `TConst.Stage` — trùng giá trị
    /// P/A1/A2 nhưng là bộ khác, nên tra cứu phải theo đúng lớp hằng này.
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    /// <summary>Ngày/người duyệt cấp 1 (`Appr1DTime`/`Appr1By`).</summary>
    public DateTime? Appr1DTime { get; set; }
    public string? Appr1By { get; set; }
    /// <summary>Ngày/người duyệt cấp 2 (`Appr2DTime`/`Appr2By`).</summary>
    public DateTime? Appr2DTime { get; set; }
    public string? Appr2By { get; set; }
    /// <summary>
    /// Ngày/người **HUỶ DUYỆT** (`CancelDTime`/`CancelBy`) — nguồn `Plan_EstimateOrder_Cancel`
    /// KHÔNG xoá/huỷ đơn mà **trả trạng thái về "P"** từ "A1" hoặc "A2".
    /// </summary>
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
}

/// <summary>Dòng model trong dự kiến đơn hàng — port 1:1 FrmQuanLyDuKienDH detail, 2010.HTC.</summary>
public sealed class EstimateOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long EstimateOrderId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public int Quantity { get; set; }
    // ===== #154 parity Plan_EstimateOrderDtl: 20 cột số lượng + trục trạng thái/dấu vết =====
    // Nguồn: DataWH/BizHTC.zTemp.cs (csproj 276, md5 dbb71f7d… verify 2 máy ở #142) — ghi tại 53087.
    // 🔴 Port cũ chỉ có `Quantity` (một con số) — nguồn có **20 loại số lượng** đặt cạnh nhau để
    //    người lập kế hoạch so sánh; `Quantity` là cột BỊA, giữ để đọc dữ liệu cũ.
    /// <summary>Đã bán cho KHÁCH (`QtySellCustomer`) và bán cho ĐẠI LÝ (`QtySellDealer`) — hai kênh tách riêng.</summary>
    public decimal QtySellCustomer { get; set; }
    public decimal QtySellDealer { get; set; }
    public decimal QtyInStock { get; set; }
    /// <summary>Đang trên đường về (`QtyOnWay`).</summary>
    public decimal QtyOnWay { get; set; }
    public decimal QtyBuyDealer { get; set; }
    /// <summary>Chưa xác định (`QtyUnKnown`) — ⚠️ nguồn viết hoa chữ "K" giữa từ, giữ 1:1.</summary>
    public decimal QtyUnKnown { get; set; }
    // --- Bốn nhóm back-order, tên cột nguồn viết bằng TIẾNG VIỆT không dấu — giữ nguyên 1:1 ---
    /// <summary>BO chưa xuất kho (`QtyBOChuaXuatKho`).</summary>
    public decimal QtyBOChuaXuatKho { get; set; }
    /// <summary>BO không VIN, kỳ QUÁ KHỨ (`QtyBOKhongVINQuaKhu`).</summary>
    public decimal QtyBOKhongVINQuaKhu { get; set; }
    public decimal QtyBOKhongVINHienTai { get; set; }
    public decimal QtyBOKhongVINTuongLai { get; set; }
    /// <summary>Đã đặt CÓ mã xe (`QtyOrdCarID`) và KHÔNG có mã xe (`QtyOrdNotCarID`).</summary>
    public decimal QtyOrdCarID { get; set; }
    public decimal QtyOrdNotCarID { get; set; }
    /// <summary>Dự kiến đặt kỳ N+1 (`QtyEOrdN1`).</summary>
    public decimal QtyEOrdN1 { get; set; }
    /// <summary>Dự kiến bán cho khách kỳ N+0…N+3 (`QtyESellCusN0..N3`) — chuỗi dự báo 4 kỳ.</summary>
    public decimal QtyESellCusN0 { get; set; }
    public decimal QtyESellCusN1 { get; set; }
    public decimal QtyESellCusN2 { get; set; }
    public decimal QtyESellCusN3 { get; set; }
    /// <summary>Trạng thái DÒNG (`PLEOrdDtlStatus`) — bám theo `PLEOrdStatus` của bảng đầu ("P"/"A1"/"A2").</summary>
    public string PLEOrdDtlStatus { get; set; } = "P";
    /// <summary>Mốc sửa gần nhất (`LUDateTime`/`LUBy`) — TÁCH khỏi `LogLU*`.</summary>
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Ánh xạ xe ↔ đơn hàng SX (màu/mô tả/số SO) — port 1:1 FrmWO_Mapping (TblWOMapping, 2010.HTC/Sales).</summary>
public sealed class WOMapping
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";
    public string? ColorCode { get; set; }
    public string? ColorNameVN { get; set; }
    public string? Description { get; set; }
    public string? SoCode { get; set; }
    public string? WorkOrderNoTemp { get; set; }  // WinForm core output: CarCarMapWorkOrder (line 193)
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Kế hoạch bán hàng theo quý (đại lý × model × năm, Q1-Q4) — port 1:1 FrmSalePlan (2010.HTC/Sales).</summary>
public sealed class SalePlan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public int YearPlan { get; set; }
    public int Q1 { get; set; }
    public int Q2 { get; set; }
    public int Q3 { get; set; }
    public int Q4 { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thông tin thùng xe tải theo VIN (chứng nhận/CO/hóa đơn thùng) — port 1:1 FrmUpdate_Cabin (Tbl_UpdateCabin, 2010.HTC/Sales).</summary>
public sealed class CabinInfo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? CabinCertificateNo { get; set; }   // số giấy chứng nhận thùng
    public DateTime? CabinCertificateDate { get; set; }
    public string? CabinCONo { get; set; }             // số nguồn gốc (CO)
    public string? CabinInvoiceNo { get; set; }        // số hóa đơn thùng
    public DateTime? CabinInvoiceDate { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Yêu cầu chiết khấu thanh toán (theo bảo lãnh/ngân hàng) — port 1:1 FrmReq_PaymentDiscount (2010.HTC/Sales). Duyệt: Draft→Approved/Rejected.</summary>
public sealed class PaymentDiscountReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? GuaranteeNo { get; set; }
    public string? BankGuaranteeNo { get; set; }
    public string? BankCode { get; set; }
    public string? SpecDescription { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Status { get; set; } = "Draft"; // Draft -> Approved / Rejected
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>File đính kèm khiếu nại đơn phụ tùng — port 1:1 FrmSer_OrderComplainAttachment (Ser_OrderComplainAttachment, TCMotor/TST).</summary>
public sealed class OrderComplainAttachment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ComplainNo { get; set; } = "";
    public string FileName { get; set; } = "";
    public string? ImageType { get; set; }   // loại ảnh (OrderComplainImageType)
    public string? FileNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Định mức tồn tối thiểu (theo model/spec/đại lý) — port 1:1 FrmSt_MinInvBalance (TblSt_MinInvBalance, Admin/Product 2010.HTC).</summary>
public sealed class MinInvBalance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    // ===== #153 parity St_MinInvBalance (nguồn: DMS40/0.01.Master.cs, csproj 122, md5 c690f1f1… verify 2 máy ở #144)
    //       `St_MinInvBalance_AddX` (10130) ghi 3 bảng tại 10468 / 10501 / 10520;
    //       `_UpdateX` (11686) xoá-ghi lại hai bảng con. 🔴 Chỉ có ở WS 64-bit.
    /// <summary>Số định mức (`StMinInvNo`) — **khoá nghiệp vụ**, port cũ không có cột này.</summary>
    public string StMinInvNo { get; set; } = "";
    /// <summary>
    /// 🔴 Cờ ÁP CHO TẤT CẢ ĐẠI LÝ (`FlagAllDealer`) — "1"/"0".
    /// Guard nguồn (0.01.Master.cs:11767): `FlagAllDealer = "0"` thì bảng
    /// <see cref="StMinInvBalanceDealer"/> **phải có ít nhất 1 dòng**; `= "1"` thì bảng con để rỗng.
    /// </summary>
    public string FlagAllDealer { get; set; } = "0";
    public DateTime CreateDTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    /// <summary>Mốc NGỪNG hiệu lực (`InactiveDTime`/`InactiveBy`) — tách khỏi `LogLU*`.</summary>
    public DateTime? InactiveDTime { get; set; }
    public string? InactiveBy { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    /// <summary>Tổng số lượng định mức (`TotalQty`) — nguồn đọc `Convert.ToInt32` và bắt **> 0**.</summary>
    public decimal TotalQty { get; set; }
    public string FlagActive { get; set; } = "1";

    // --- ⚠️ #153: bốn cột dưới là cột RIÊNG của MiniHTC, KHÔNG có ở nguồn ---
    /// <summary>⚠️ Nguồn KHÔNG gộp danh sách vào chuỗi — dùng bảng con <see cref="StMinInvBalanceSpec"/>. Giữ để đọc dữ liệu cũ.</summary>
    public string ModelList { get; set; } = "";
    /// <summary>⚠️ Cột riêng MiniHTC — xem <see cref="ModelList"/>.</summary>
    public string? SpecMix { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC — nguồn dùng bảng con <see cref="StMinInvBalanceDealer"/>.</summary>
    public string? DealerList { get; set; }
    /// <summary>⚠️ Cột riêng MiniHTC, đứng thay `LogLUDateTime` của nguồn.</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Spec thuộc định mức tồn tối thiểu (`St_MinInvBalanceSpec`) — bảng nối, chỉ khoá + dấu vết.</summary>
public sealed class StMinInvBalanceSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StMinInvNo { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Đại lý áp dụng định mức (`St_MinInvBalanceDealer`) — bảng nối.
/// Rỗng khi <see cref="MinInvBalance.FlagAllDealer"/> = "1" (áp cho tất cả đại lý).
/// </summary>
public sealed class StMinInvBalanceDealer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StMinInvNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Đề nghị đăng ký xe lái thử (Car_TestCar) — port 1:1 FrmNewRegister_TestCar (2010.HTC/Sales). Header đề nghị + danh sách VIN được đăng ký làm xe lái thử, có hiệu lực từ-đến.
/// Khác TestDrive (lịch hẹn khách lái thử) — đây là đề nghị NỘI BỘ phân bổ VIN làm demo/xe lái thử.</summary>
public sealed class CarTestCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TestCarCode { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? Remark { get; set; }

    /// <summary>
    /// 🔴 Trạng thái đề nghị (`Car_TestCar.TestCarStatus`, `TConst.Stage`): "P" chờ duyệt → "A" đã duyệt · "R" từ chối.
    /// Port cũ (bộ này) **KHÔNG có trạng thái nào** — chỉ lưu được dữ liệu, không có vòng đời duyệt.
    /// </summary>
    public string TestCarStatus { get; set; } = "P";
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedBy { get; set; }
    public string? RejectReason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #190 parity `Car_TestCar_Finished` (BizHTC.Car.cs:6762) — cum CHI CO o WS 64-bit =====
    /// <summary>Mốc KẾT THÚC chạy thử (`FinishedDate`/`FinishedBy`) — nguồn ghi khi chuyển "A" sang "F".</summary>
    public DateTime? FinishedDate { get; set; }
    public string? FinishedBy { get; set; }
}

/// <summary>Dòng VIN trong đề nghị đăng ký xe lái thử — port 1:1 grid FrmNewRegister_TestCar (Car_TestCarDtl, 2010.HTC).</summary>
public sealed class CarTestCarDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TestCarId { get; set; }
    public string CarId { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? SoDonHang { get; set; }
    public string? ColorCode { get; set; }
    public string? ColorName { get; set; }

    /// <summary>
    /// 🔴 Trạng thái RIÊNG của DÒNG xe (`Car_TestCarDtl.TestCarStatusDtl`): "P" · "A" · "R".
    /// ⚠️ Chính cột này là căn cứ của luật CHỐNG ĐĂNG KÝ TRÙNG XE:
    /// nguồn đếm số đề nghị chứa cùng `CarId` **có trạng thái dòng thuộc ('P','A')**;
    /// &gt; 1 ⇒ **báo lỗi** (`BizHTC.Car.cs:4942-5008`, hàm `mycheck_Car_TestCar_CarId`).
    /// </summary>
    public string TestCarStatusDtl { get; set; } = "P";

    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    public decimal UnitPriceActual { get; set; }
}

/// <summary>Đơn hàng gốc DMS40 (DMS40_Ord_SalesOrderRoot) — port 1:1 FrmUpgradeMngOrderDealer/FrmUpgradeOrderApprove/FrmUpgradeOrderApprovePlan, 2010.HTC/Sales/Upgrade.
/// Header gom nhiều dòng model/spec/color của 1 đợt đặt hàng kế hoạch; duyệt SỐ LƯỢNG theo dòng (khác D4OSORA/Dms40SoRootApproval — duyệt cả ĐƠN theo rule).
/// Status: P(chờ duyệt)→A(đã duyệt 1 phần/toàn phần, có thể duyệt tiếp Approved2)→F(hoàn tất)/C(hủy).
/// ĐƠN GIẢN HOÁ: bỏ qua nhánh "duyệt đặc biệt PA→F"/mirror WH/join Model-Spec-Color master (quá sâu để trace 1:1 trong 1 fire).</summary>
public sealed class Dms40SoRoot
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SORCode { get; set; } = "";
    public string? SOType { get; set; }
    public string DealerCode { get; set; } = "";
    public string? SPCode { get; set; }
    public DateTime? OrderMonth { get; set; }
    public DateTime? ProductionMonth { get; set; }
    public DateTime? ExpectedMonth { get; set; }
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprDTime { get; set; }

    // ===== #163 parity `DMS40_Ord_SalesOrderRoot_Finish_New20210521` (DataWH/BizHTC.Order.cs:1478, csproj 315) =====
    /// <summary>Thời điểm hoàn tất (`FinishDTime`) — nguồn ghi cùng `SORStatus = 'F'`; port cũ chỉ đổi trạng thái.</summary>
    public DateTime? FinishDTime { get; set; }
    /// <summary>Người hoàn tất (`FinishBy`).</summary>
    public string? FinishBy { get; set; }
    /// <summary>🔴 Mã đơn bán (`Ord_SalesOrder.SOCode`) mà bước hoàn tất TỰ SINH ra từ đơn gốc này.</summary>
    public string? GeneratedSoCode { get; set; }

    // ===== #208 parity `DMS40_Ord_SalesOrderRoot_Cancel1/Cancel2_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:78681 / 79242) — hai lệnh ghi CÙNG bộ cột dưới đây. =====
    /// <summary>Mốc HUỶ đơn gốc (`CancelDTime`/`CancelBy`) — port cũ chỉ đổi trạng thái, không ghi dấu vết.</summary>
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng model/spec/color trong đơn hàng gốc DMS40 — port 1:1 grid FrmUpgradeOrderApprovePlan (DMS40_Ord_SalesOrderRootDetail).</summary>
public sealed class Dms40SoRootDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SoRootId { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public decimal UnitPriceInit { get; set; }
    public decimal RequestedQuantity { get; set; }
    public decimal Approved1Quantity { get; set; }
    public decimal Approved2Quantity { get; set; }
    public decimal CancelQuantityTotal { get; set; }
    public string? Remark { get; set; }

    // ===== #163 parity `DMS40_Ord_SalesOrderRoot_Finish_New20210521` =====
    /// <summary>Ngày yêu cầu giao của dòng (`RequestedDate`) — nguồn bê sang dòng đơn bán khi hoàn tất.</summary>
    public DateTime? RequestedDate { get; set; }
    /// <summary>Ngày duyệt cấp 1 của dòng (`Approved1Date`) — nguồn map thành `ApprovedDate` của dòng đơn bán.</summary>
    public DateTime? Approved1Date { get; set; }
    /// <summary>Ngày duyệt cấp 2 (`Approved2Date`) — bước hoàn tất **sao chép từ `Approved1Date`**, không nhập tay.</summary>
    public DateTime? Approved2Date { get; set; }
    /// <summary>Trạng thái RIÊNG của dòng (`SORStatusDtl`) — nguồn cập nhật theo trạng thái header ở bước hoàn tất.</summary>
    public string? SORStatusDtl { get; set; }
    // #208: nguồn cascade `SORStatusDtl` kèm `LogLU*` trên dòng chi tiết khi huỷ.
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Hạn bảo hành theo model (tháng + km) — port 1:1 FrmWarrantyExpires (TblMst_WarrantyExpires, Admin/Product 2010.HTC).</summary>
public sealed class WarrantyExpires
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public int WarrantyMonths { get; set; }
    public decimal WarrantyKM { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Kho/bãi (master mã/tên/địa chỉ/tỉnh/loại kho) — port 1:1 FrmStorage (TblStorage, Admin/Product 2010.HTC).</summary>
public sealed class Storage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StorageCode { get; set; } = "";
    public string? StorageName { get; set; }
    public string? StorageAddress { get; set; }
    public string? ProvinceCode { get; set; }
    public string? StorageType { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tùy chọn/cấu hình chuẩn theo model (spec grade) — port 1:1 FrmStandarOption (TblCarStdOpt, Admin/Product 2010.HTC).</summary>
public sealed class CarStdOption
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string StdCode { get; set; } = "";
    public string? StdDesc { get; set; }
    public string? GradeCode { get; set; }
    public string? GradeDesc { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhà vận tải (master đơn vị vận chuyển) — port 1:1 FrmTransporter (Tbl_Transpoter, Admin/Product 2010.HTC).</summary>
public sealed class Transporter
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TransporterCode { get; set; } = "";
    public string? TransporterName { get; set; }
    public string? Address { get; set; }
    public string? PhoneNo { get; set; }
    public string? FaxNo { get; set; }
    public string? DirectorFullName { get; set; }
    public string? DirectorPhoneNo { get; set; }
    public string? ContactorPhoneNo { get; set; }  // WinForm TblTranspoter.ContactorPhoneNo (line 45)
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Xe tải của nhà vận tải — port 1:1 FrmTransporterCar (Tbl_Mst_TransporterCar, Admin/Product 2010.HTC).</summary>
public sealed class TransporterCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TransporterCode { get; set; } = "";
    public string PlateNo { get; set; } = "";
    public string FlagActive { get; set; } = "1";
}

/// <summary>Tài xế của nhà vận tải — port 1:1 FrmTransporterDriver (Tbl_Mst_TransporterDriver, Admin/Product 2010.HTC).</summary>
public sealed class TransporterDriver
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TransporterCode { get; set; } = "";
    public string DriverId { get; set; } = "";
    public string? DriverFullName { get; set; }
    public string? DriverLicenseNo { get; set; }
    public string? DriverPhoneNo { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>Chữ ký số đại lý (chứng thư số CA) — port 1:1 FrmQLChuKyDienTu (Tbl_Dlr_CA, Admin/DMS40 2010.HTC).</summary>
public sealed class DealerCA
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string? CaSubject { get; set; }   // tên chủ thể chữ ký
    public string? CaIssuer { get; set; }     // nhà cung cấp/CA issuer
    public string? Serial { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tỉ lệ phân bổ kho theo model/spec/màu cho 3 miền (Bắc/Trung/Nam) — port 1:1 FrmMst_StorageRate (Tbl_Auto_MapVIN_StorageRate, Admin/DMS40 2010.HTC).</summary>
public sealed class StorageRate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StorageCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ColorExtCode { get; set; }
    public decimal MBVal { get; set; }   // miền Bắc
    public decimal MTVal { get; set; }   // miền Trung
    public decimal MNVal { get; set; }   // miền Nam
    /// <summary>
    /// ✅ #147 — cột NÀY CÓ THẬT ở nguồn: tuy không nằm trong danh sách `insert into Mst_StorageAreaRate`,
    /// `Mst_StorageAreaRate_CheckDB` **đọc** `FlagActive` (0.01.Master.cs:8999) ⇒ DB có cột, chỉ dùng default.
    /// (Đối chiếu bằng danh sách `insert` là CHƯA đủ để kết luận "cột thừa".)
    /// </summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC, đứng thay `LogLUDateTime` của nguồn.</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    // --- #147 parity Mst_StorageAreaRate ---
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Gói bảo dưỡng theo mốc (loại BD × model, gồm hạng mục CV + vật tư) — port 1:1 FrmMaintenance (Admin/Maintenance, 2010.HTC).</summary>
public sealed class MaintPackage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TypeCode { get; set; } = "";   // MTNTP
    public string? TypeName { get; set; }         // MTNTPNAME
    public int Times { get; set; }                // MTNTIMES — mốc BD lần thứ n
    public string? ModelCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Hạng mục công việc trong gói bảo dưỡng — port 1:1 FrmMaintenance grid works, 2010.HTC.</summary>
public sealed class MaintPackageWork
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MaintPackageId { get; set; }
    public string? WorkItemCode { get; set; }        // MTNTKCODE (hạng mục)
    public string WorkContentCode { get; set; } = ""; // MTNTKITEMCODE (nội dung CV)
}

/// <summary>Vật tư trong gói bảo dưỡng (mã + SL) — port 1:1 FrmMaintenance grid supplies, 2010.HTC.</summary>
public sealed class MaintPackageSupply
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MaintPackageId { get; set; }
    public string SupplyCode { get; set; } = "";     // PARTCODE
    public decimal Qty { get; set; }
}

/// <summary>Vật tư bảo dưỡng (master mã/tên/ĐVT chuẩn+thường) — port 1:1 FrmSupplies (Admin/Maintenance, 2010.HTC).</summary>
public sealed class MaintSupply
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string? Name { get; set; }
    public string? StandardUnit { get; set; }   // PARTUNITCODESTD
    public string? CommonUnit { get; set; }      // PARTUNITCODEDEFAULT
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Hạng mục công việc bảo dưỡng (master, Mst_MTNTK) — port 1:1 FrmWorkItems (Admin/Maintenance, 2010.HTC). Mã + tên hạng mục — là cha của MaintWorkContent (ItemCode tham chiếu WorkItemCode). Upsert-by-code + toggle.</summary>
public sealed class MaintWorkItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string WorkItemCode { get; set; } = "";   // MTNTKCODE
    public string? WorkItemName { get; set; }         // MTNTKNAME
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tỷ lệ duyệt đơn hàng tối đa theo đại lý+model (Mst_RateApprOrderModelMax) — port 1:1 FrmRateApprOrderModelMax (2010.HTC/Admin/Dealer). Composite key DealerCode+ModelCode. Upsert.</summary>
public sealed class RateApprOrderModelMax
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public decimal RateApprMax { get; set; }
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC: `Mst_RateApprOrderModelMax` nguồn **không có** `FlagActive`
    /// (grep toàn nguồn: 0 dòng nhắc). Giữ lại vì endpoint `/toggle` đang dùng.</summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC, đứng thay `LogLUDateTime` của nguồn.</summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    // --- #147 parity Mst_RateApprOrderModelMax: dấu vết chuẩn của nguồn ---
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Cấu hình mẫu phụ lục hợp đồng theo loại ĐH+HT thanh toán+model (Ctr_ContractTypeModel) — port 1:1 FrmCtr_ContractTypeModel (TCMotor DMSales.Foton/Admin/Product). Composite key SOType+PmtMethodNo+ModelCode → ContractType áp dụng. Upsert.</summary>
public sealed class ContractTypeModel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SOType { get; set; } = "";
    public string PmtMethodNo { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string ContractType { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tùy chọn tiêu chuẩn theo model+hạng (Car_Std_Opt) — port 1:1 FrmStandarOption (TCMotor DMSales.Foton/Admin/Product). Composite key ModelCode+StdCode → mô tả + GradeCode/GradeDesc. Upsert.</summary>
public sealed class CarStdOpt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string StdCode { get; set; } = "";
    public string? StdDesc { get; set; }
    public string? GradeCode { get; set; }
    public string? GradeDesc { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Điều khoản thanh toán chiết khấu/công nợ (Mst_PaymentTerm) — port 1:1 FrmMst_PaymentTerm (TCMotor DMSales.Foton/Admin/Product). Mã ĐK + tên + số ngày đến hạn TT/BL/CL/NHS. Upsert-by-code + toggle. KHÁC catalog "PaymentTerm" (FrmMst_Dieu_Khoan_ThanhToan).</summary>
public sealed class PaymentTermMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DCPType { get; set; } = "";
    public string? DCPTypeName { get; set; }
    public int PaymentDueDays { get; set; }
    public int GuaranteeDueDays { get; set; }
    public int PaymentCLDueDays { get; set; }
    public int PaymentNHSDueDays { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Mã lỗi khiếu nại/chẩn đoán bảo hành (Mst_Complaint_And_Diagnostic_Error_Code_Mng) — port 1:1 FrmMstComplaintAndDiagnosticErrorCodeMng (TCMotor DMSCarSv/Admin). Mã + tên + mô tả + loại lỗi + số km/ngày còn bảo hành áp dụng. Upsert-by-code + toggle.</summary>
public sealed class ComplaintErrorCode
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ErrorCode { get; set; } = "";
    public string? ErrorName { get; set; }
    public string? ErrorDesc { get; set; }
    public string? ErrorTypeCode { get; set; }
    public int WarrantyDate { get; set; }
    public int WarrantyKm { get; set; }
    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Loại bảo hành RO (Ser_MST_ROWarrantyType) — port 1:1 FrmMstWarrantyTypeMng (TCMotor DMSCarSv/Admin).
/// Loại chính (XM/SB/PT/TC/BT) + loại chi tiết (A/B/P/W/S/R/C).
/// Danh sách loại ảnh bắt buộc nằm ở bảng chi tiết <see cref="ROWarrantyTypePhoto"/>, KHÔNG phải một cột chuỗi.
/// </summary>
public sealed class ROWarrantyType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Khoá tự tăng của nguồn (ROWTID) — bảng chi tiết loại ảnh nối về đây.</summary>
    public string? ROWTID { get; set; }

    public string ROWTypeCode { get; set; } = "";
    public string? ROWTypeName { get; set; }
    public string ROWTypeDtlCode { get; set; } = "";
    public string? ROWTypeDtlName { get; set; }

    /// <summary>
    /// ⚠️ KHÔNG phải dữ liệu gốc — đây là CHUỖI HIỂN THỊ do lưới nguồn tự dựng từ bảng chi tiết
    /// (nối mã loại ảnh bằng ", ", có loại trừ). Giữ lại cho dữ liệu cũ; nguồn sự thật là
    /// <see cref="ROWarrantyTypePhoto"/>.
    /// </summary>
    public string? ROWPhotoType { get; set; }

    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// Loại ảnh chứng minh bắt buộc cho một loại bảo hành (Ser_MST_ROWarrantyType_PhotoType —
/// port 1:1 FrmMstWarrantyTypeMng, TCMotor DMSCarSv/Admin).
/// MỘT loại bảo hành đòi NHIỀU loại ảnh; nguồn trả về thành một bảng kết quả riêng.
/// </summary>
public sealed class ROWarrantyTypePhoto
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Khoá nối về <see cref="ROWarrantyType"/> (theo Id nội bộ của MiniHTC).</summary>
    public long ROWarrantyTypeId { get; set; }

    /// <summary>Mã loại ảnh (ROWPTCODE) — tra ở master Ser_MST_ROWarrantyPhotoType.</summary>
    public string ROWPTCode { get; set; } = "";

    /// <summary>Tên loại ảnh (ROWPTNAME).</summary>
    public string? ROWPTName { get; set; }
}

/// <summary>
/// Hạng mục công bảo hành theo model — port 1:1 FrmMstWarrantyWorkMng (TCMotor DMSCarSv/Admin).
/// ⚠️ Tên bảng THẬT ở nguồn là <c>Ser_MST_ROWarrantyWork</c> (biz Ser_MST_ROWarrantyWork_Get);
/// <c>Mst_Warranty_Work_Mng</c> chỉ là tên lớp hằng phía client.
/// Mã CV + tên + model + loại áp dụng + giờ định mức + giá định mức + giá bán + thuế.
/// </summary>
public sealed class WarrantyWorkMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Khoá tự tăng của nguồn (ROWWID).</summary>
    public string? ROWWID { get; set; }

    public string ROWWorkCode { get; set; } = "";
    public string? ROWWorkName { get; set; }
    public string ModelCode { get; set; } = "";
    public string? AppTypeCode { get; set; }
    public decimal RateHour { get; set; }
    public decimal RatePrice { get; set; }
    public decimal Price { get; set; }
    public decimal VAT { get; set; }
    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Khoang/ngăn kho (Mst_Compartment) — port 1:1 FrmMst_Compartment (TCMotor DMSCarSv/Admin). Mã + tên khoang. Upsert-by-code + toggle.</summary>
/// <summary>#520 Danh mục **màu biển số** (`Mst_PlateColor`) — port 1:1 `Mst_PlateColor_Get`
/// (`BizCarSv.Master.cs:5743`), sống qua **kênh ClientService** (`Mst_PlateColorService.cs:40`), xem #519.</summary>
/// <summary>#566 §12 Danh mục **mạng lưới** (`CmCt_Mst_Network`) — mỗi bản ghi là một hệ thống con
/// (một đại lý hoặc HTC) kèm **các địa chỉ dịch vụ** để hệ khác gọi sang.</summary>
public sealed class NetworkMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string NetworkID { get; set; } = "";
    public string? NetworkName { get; set; }
    public string? GroupNetworkID { get; set; }
    public string? CoreAddr { get; set; }
    public string? PingAddr { get; set; }
    public string? XSysAddr { get; set; }
    public string? WSUrlAddr { get; set; }
    public string? DBUrlAddr { get; set; }
    public string? WAUrlAddr { get; set; }
    public string? FlagActive { get; set; }
    public DateTime? LogLUDTimeUTC { get; set; }
    public string? LogLUBy { get; set; }
}

public sealed class PlateColorMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PlateColorCode { get; set; } = "";
    public string? PlateColorName { get; set; }
    /// <summary>Mã màu dạng HEX để vẽ trên giao diện (`Mst_PlateColor.ColorHexCode`).</summary>
    public string? ColorHexCode { get; set; }
    /// <summary>Thứ tự hiển thị — nguồn `order by t.IndexColor` (**không** sắp theo mã/tên).</summary>
    public int? IndexColor { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public sealed class CompartmentMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CompartmentCode { get; set; } = "";
    public string? CompartmentName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhân viên (Mst_Staff) — port 1:1 FrmMst_Staff (TCMotor DMSCarSv/Admin). Mã + tên nhân viên. Upsert-by-code + toggle.</summary>
public sealed class StaffMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StaffCode { get; set; } = "";
    public string? StaffName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Mã VIN gốc theo model (Mst_VINModelOrginal) — port 1:1 FrmVINModelOrginal (TCMotor DMSCarSv/Admin). Tiền tố VIN + model + mã xuất xứ. Composite key VINCode+ModelCode. Upsert.</summary>
public sealed class VinModelOrginalMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VINCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? OrginalCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Giới hạn giá công phát sinh theo loại BH chi tiết (Mst_Extra_Work_Limitation_Mng) — port 1:1 FrmMstExtraWorkLimitationMng (TCMotor DMSCarSv/Admin). Composite key ExtraWorkCode(ROWArisCode)+WarrantyDtlCode(ROWTypeDtlCode) → MaxPrice ghi đè theo loại BH. KHÁC ExtraWorkMst (giá tối đa chung, không theo loại BH).</summary>
public sealed class ExtraWorkLimitationMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ExtraWorkCode { get; set; } = "";
    public string? ExtraWorkName { get; set; }
    public string WarrantyDtlCode { get; set; } = "";
    public decimal MaxPrice { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Gia hạn bảo hành theo VIN — port 1:1 FrmMstWarrantyExtensionDateMng (TCMotor DMSCarSv/Admin).
/// ⚠️ Tên bảng THẬT ở nguồn là <c>Ser_MST_ROWarrantyRenewal</c> (biz Ser_MST_ROWarrantyRenewal_Save),
/// không phải "Mst_Warranty_Extension_Date_Mng" — đó chỉ là tên lớp hằng phía client.
/// Khoá upsert đúng của nguồn là CẶP (VIN, WrtReneCateCode) — mỗi VIN chỉ có MỘT bản gia hạn
/// cho MỖI LOẠI gia hạn; gia hạn lại cùng loại thì ĐÈ lên dòng cũ.
/// </summary>
public sealed class WarrantyExtensionDateLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";

    /// <summary>
    /// Khoá tự tăng của nguồn (ROWRID = @@Identity của Ser_MST_ROWarrantyRenewal).
    /// Nguồn dùng nó cho thao tác XOÁ; giữ lại để đối chiếu dữ liệu nhập từ hệ cũ.
    /// </summary>
    public string? ROWRID { get; set; }

    /// <summary>Mã loại gia hạn (WRTRENECATECODE) — nửa còn lại của khoá upsert, BẮT BUỘC.</summary>
    public string? ExtCategoryCode { get; set; }

    /// <summary>Tên loại gia hạn (WRTRENECATENAME).</summary>
    public string? ExtCategoryName { get; set; }

    /// <summary>Ngày gia hạn mới (WRTRENEDATE).</summary>
    public DateTime? ExtensionDate { get; set; }

    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";

    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// ⚠️ KHÔNG CÒN DÙNG. Bản port trước ánh xạ nhầm cột <c>ROWRID</c> (khoá tự tăng) thành "số RO"
    /// và lấy nó làm nửa khoá upsert. Bảng nguồn KHÔNG có cột số RO nào. Giữ lại để không mất dữ liệu cũ.
    /// </summary>
    public string? RONo { get; set; }
}

/// <summary>Phân công công đoạn sửa chữa theo RO (Ser_AssignmentWork header) — port 1:1 FrmSer_AssignmentWork (TCMotor DMSCarSv/Services). Header theo RO; 7 công đoạn (SCC/SCD/SCDB/SCKSC/SCLR/SCN/SCS) mỗi công đoạn gán khoang (Cavity) + kế hoạch/thực tế bắt đầu-kết thúc → SerAssignmentWorkStage.</summary>
/// <summary>#532 NHẬT KÝ THỜI GIAN LÀM VIỆC trên lệnh sửa chữa (`Ser_ROWorkTime`) — mỗi dòng là
/// **một mốc bấm giờ**: bắt đầu / kết thúc / chạy-dừng. Nguồn: `BizCarSv.zzzzCode.cs:208
/// InsertSer_ROWorkTime` (bản thứ HAI trong file — bản ở `:25` có chú thích `// _dbAction ????`).</summary>
public sealed class RoWorkTime
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Số nhật ký, nguồn xin từ `SequenceGetForDMS_Util(… SequenceTypeDMS.ROWorkTime …)`.</summary>
    public string ROWTNo { get; set; } = "";
    public string? ROID { get; set; }
    public string RONo { get; set; } = "";
    /// <summary>Mốc bấm giờ (`StandardizeDTime` ⇒ giữ cả giờ, khác `StandardizeDate` của #530).</summary>
    public DateTime? PointDateTime { get; set; }
    /// <summary>Cờ CHẠY/DỪNG — chỉ nhận "1"/"0"; ở luồng tạm dừng nguồn truyền `bPause ? Yes : No`.</summary>
    public string? FlagPlay { get; set; }
    public string? FlagBegin { get; set; }
    public string? FlagEnd { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

// ===== #528 §12 KẾ HOẠCH PHÂN CÔNG THEO **BẢY** CÔNG ĐOẠN (Ser_AssignmentWork) =====
// Nguồn `BizCarSv.AssignmentOfWork.cs:203 Ser_AssignmentWork_CreateX` nhận **21** tham số kế hoạch:
//   bảy nhóm `SCC / SCD / SCN / SCS / SCDB / SCLR / SCKSC` × ba cột (bắt đầu · kết thúc · khoang).
public sealed class SerAssignmentWork
{
    /// <summary>Khoá lệnh sửa chữa — nguồn `Convert.ToInt32(strROID)` (KHÔNG guard rỗng).</summary>
    public string? ROID { get; set; }
    public DateTime? SCCPlanStartDTime { get; set; }
    public DateTime? SCCPlanFinishDTime { get; set; }
    public string? SCCCavityID { get; set; }
    public DateTime? SCDPlanStartDTime { get; set; }
    public DateTime? SCDPlanFinishDTime { get; set; }
    public string? SCDCavityID { get; set; }
    public DateTime? SCNPlanStartDTime { get; set; }
    public DateTime? SCNPlanFinishDTime { get; set; }
    public string? SCNCavityID { get; set; }
    public DateTime? SCSPlanStartDTime { get; set; }
    public DateTime? SCSPlanFinishDTime { get; set; }
    public string? SCSCavityID { get; set; }
    public DateTime? SCDBPlanStartDTime { get; set; }
    public DateTime? SCDBPlanFinishDTime { get; set; }
    public string? SCDBCavityID { get; set; }
    public DateTime? SCLRPlanStartDTime { get; set; }
    public DateTime? SCLRPlanFinishDTime { get; set; }
    public string? SCLRCavityID { get; set; }
    public DateTime? SCKSCPlanStartDTime { get; set; }
    public DateTime? SCKSCPlanFinishDTime { get; set; }
    public string? SCKSCCavityID { get; set; }
    /// <summary>#531 §12 Cờ **PHÁT SINH** (`FlagArise`) — khác `FlagPause`: cột này được ghi
    /// **ngay trên bảng phân công** và **KHÔNG đảo giá trị**.</summary>
    public string? FlagArise { get; set; }
    /// <summary>#531 §12 Loại công việc phát sinh (`WorkTypeArise`).</summary>
    public string? WorkTypeArise { get; set; }
    /// <summary>#530 §12 Loại công việc lúc TẠM DỪNG (`WorkTypePause`) — cột **duy nhất** mà
    /// `Ser_AssignmentWork_UpdateFlagPause` thực sự ghi vào bảng này.</summary>
    public string? WorkTypePause { get; set; }
    // ===== #529 §12 MỐC THỰC TẾ theo bảy công đoạn (Ser_AssignmentWork.SC*Actual*DTime) =====
    public DateTime? SCCActualStartDTime { get; set; }
    public DateTime? SCCActualFinishDTime { get; set; }
    public DateTime? SCDActualStartDTime { get; set; }
    public DateTime? SCDActualFinishDTime { get; set; }
    public DateTime? SCNActualStartDTime { get; set; }
    public DateTime? SCNActualFinishDTime { get; set; }
    public DateTime? SCSActualStartDTime { get; set; }
    public DateTime? SCSActualFinishDTime { get; set; }
    public DateTime? SCDBActualStartDTime { get; set; }
    public DateTime? SCDBActualFinishDTime { get; set; }
    public DateTime? SCLRActualStartDTime { get; set; }
    public DateTime? SCLRActualFinishDTime { get; set; }
    public DateTime? SCKSCActualStartDTime { get; set; }
    public DateTime? SCKSCActualFinishDTime { get; set; }
    /// <summary>Loại công việc lúc BẮT ĐẦU / KẾT THÚC (`WorkTypeStart` / `WorkTypeFinish`).</summary>
    public string? WorkTypeStart { get; set; }
    public string? WorkTypeFinish { get; set; }
    public DateTime? CreateDTime { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RONo { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Kỹ thuật viên được phân công cho một RO theo LOẠI CÔNG VIỆC
/// (Ser_AssignmentWorkEngineer — port 1:1 FrmSer_AssignmentWork, TCMotor DMSCarSv/Services).
/// MỘT RO phân cho NHIỀU kỹ thuật viên; mỗi dòng là một cặp (kỹ thuật viên, loại công việc).
/// Nguồn lưu cả danh sách theo kiểu XOÁ HẾT rồi GHI LẠI mỗi lần lưu phân công.
/// </summary>
public sealed class SerAssignmentWorkEngineer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long AssignmentWorkId { get; set; }

    /// <summary>Mã kỹ thuật viên (nguồn dùng EngineerID, ghép ra EngineerNo/EngineerName khi đọc).</summary>
    public string EngineerNo { get; set; } = "";

    /// <summary>Loại công việc: SCC/SCD/SCN/SCS/SCDB/SCLR/SCKSC (TConst.Ser_AssignmentWork_WorkType).</summary>
    public string WorkType { get; set; } = "";

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// Kỹ thuật viên gắn với TỪNG hạng mục dịch vụ của RO (Ser_ROServiceItemsEngineer —
/// port 1:1 hiệu ứng phụ của FrmSer_AssignmentWork, TCMotor DMSCarSv/Services).
/// ⚠️ Bảng này KHÔNG do người dùng nhập: nguồn TỰ SINH khi lưu phân công kỹ thuật viên,
/// bằng cách phân phối KTV vào từng hạng mục theo loại công việc của hạng mục đó.
/// </summary>
public sealed class RoServiceItemEngineer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Hạng mục dịch vụ của RO (<see cref="RoServiceItem"/>).</summary>
    public long RoServiceItemId { get; set; }

    public string SerCode { get; set; } = "";
    public string EngineerNo { get; set; } = "";
}

/// <summary>Dòng công đoạn trong phân công RO — thuộc SerAssignmentWork. StageCode (SCC/SCD/SCDB/SCKSC/SCLR/SCN/SCS) + khoang gán + kế hoạch/thực tế bắt đầu-kết thúc.</summary>
public sealed class SerAssignmentWorkStage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long AssignmentWorkId { get; set; }
    public string StageCode { get; set; } = "";
    public string? CavityId { get; set; }
    public DateTime? PlanStart { get; set; }
    public DateTime? PlanFinish { get; set; }
    public DateTime? ActualStart { get; set; }
    public DateTime? ActualFinish { get; set; }
}

/// <summary>Nội dung công việc bảo dưỡng (theo hạng mục) — port 1:1 FrmWorkContents (Admin/Maintenance, 2010.HTC).</summary>
public sealed class MaintWorkContent
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContentCode { get; set; } = "";   // MTNTKITEMCODE
    public string? ItemCode { get; set; }            // MTNTKCODE (hạng mục cha)
    public string? Content { get; set; }             // MTNTKITEMNAME
    public int DisplayOrder { get; set; }            // VIEWIDX
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Bản ghi tính giá vốn bình quân phụ tùng (mỗi lần tính = 1 snapshot) — port 1:1 FrmPartCostManagement/FrmCaluCost/FrmReportHistoryCost (Tbl_Ser_PartCost_Calculate, TCMotor).</summary>
public sealed class PartCostSnapshot
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal AverageCost { get; set; }

    /// <summary>Số lượng TỒN ĐẦU KỲ (nguồn: SLD của #tbl_Open).</summary>
    public decimal OpeningQty { get; set; }

    /// <summary>Giá trị TỒN ĐẦU KỲ (nguồn: TGD).</summary>
    public decimal OpeningValue { get; set; }

    /// <summary>Số lượng NHẬP TRONG KỲ (nguồn: SLN).</summary>
    public decimal InQty { get; set; }

    /// <summary>Giá trị NHẬP TRONG KỲ, ĐÃ GỒM VAT (nguồn: TGN).</summary>
    public decimal InValue { get; set; }

    /// <summary>Tổng lượng dùng chia = OpeningQty + InQty (nguồn: SLD + SLN).</summary>
    public decimal TotalQty { get; set; }

    /// <summary>Tổng giá trị dùng chia = OpeningValue + InValue (nguồn: TGD + TGN).</summary>
    public decimal TotalValue { get; set; }

    /// <summary>Đầu kỳ tính giá vốn (nguồn: bỏ trống thì mặc định 1990-01-01).</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>Cuối kỳ tính giá vốn (nguồn ghi CalculateDateTime = ToDate + " 23:59:59").</summary>
    public DateTime? ToDate { get; set; }

    public string Method { get; set; } = "Average"; // Average | FIFO
    public DateTime CalculatedAt { get; set; } = DateTime.Now;
}

/// <summary>File đính kèm đề nghị bảo hành (ảnh/chứng từ theo ĐN) — port 1:1 FrmROAttachment (Ser_ROAttachment, TCMotor).</summary>
public sealed class WarrantyAttachment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceWarrantyClaimId { get; set; }
    public string FileName { get; set; } = "";
    public string? FileNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh mục loại tài liệu hồ sơ bảo hiểm (Mst_Attachment) — port 1:1 phần catalog của FrmInsuranceAttachmentAdd, TCMotor DMSCarSv/Insurance.</summary>
public sealed class InsuranceAttachmentType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string? Name { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Checklist hồ sơ bảo hiểm đã thu theo RO (Ser_InsuranceAttachment — port 1:1 FrmInsuranceAttachmentAdd, TCMotor DMSCarSv/Insurance):
/// đánh dấu loại tài liệu nào (theo InsuranceAttachmentType) đã có cho 1 RO. Tồn tại bản ghi = đã tích chọn.</summary>
public sealed class InsuranceAttachment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RONo { get; set; } = "";
    public string AttachmentCode { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 #268 NHẬT KÝ CHUYỂN TRẠNG THÁI của đề nghị bảo hành — `Ser_ROWarrantyReportTransaction`.
/// Nguồn: `BizCarSv.WarrantyReport.cs:1701 ProcessSaveSerROWarrantyReportTransaction` —
/// **10 chỗ gọi**, tức MỌI lần đổi trạng thái đều ghi lại một dòng. Port cũ thiếu hẳn bảng này ⇒
/// đề nghị chỉ còn trạng thái HIỆN TẠI, mất sạch dấu vết ai đổi, khi nào, vì sao.
///
/// ⚠️ Lớp hằng `TblSerROWarrantyReportTransaction` (DbDefine.cs:947-960) chỉ liệt kê **6** cột, còn câu
/// INSERT thật ghi **9** (thêm `CreatedBy`, `LogLUDateTime`, `LogLUBy`). Lại một lần `Tbl*` thiếu cột —
/// nguồn sự thật là CÂU GHI.
/// ⚠️ Nguồn gõ sai chính tả ngay trong hằng: `ROWRTransactionID = "ROWRTRANACTIONID"` (thiếu chữ "S" —
/// TRAN**A**CTION). Giữ nguyên khi grep DB nguồn; port dùng tên đúng chính tả.
/// </summary>
public sealed class ServiceWarrantyClaimTransaction
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>ROWID — đề nghị bảo hành mà dòng nhật ký này thuộc về.</summary>
    public long ClaimId { get; set; }

    /// <summary>CREATOR — **người/bên tạo bước chuyển** (do WS truyền vào), KHÁC `CreatedBy` là tài
    /// khoản đăng nhập thực hiện. Nguồn giữ cả hai vì đại lý có thể thao tác thay cho người khác.</summary>
    public string? Creator { get; set; }

    /// <summary>CURRENTSTATUS — trạng thái ĐÍCH sau bước chuyển (`Ser_WarrantyReport_Status`).</summary>
    public string CurrentStatus { get; set; } = "";

    /// <summary>NOTE — lý do; nguồn bắt buộc với từ chối/hoàn trả.</summary>
    public string? Note { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Đề nghị bảo hành dịch vụ (đại lý gửi HTC duyệt theo RO) — port 1:1 FrmWarrantyReportDealerSearch/HTCSearch/HTCApproved (Ser_ROWarrantyReport, TCMotor).</summary>
public sealed class ServiceWarrantyClaim
{
    // 🔴 #369 §12 — HAI mã phân loại BCBH. Nguồn dùng CHÚNG để chọn **luật duyệt nào áp dụng**,
    //   và hai luật đó **ngược chiều nhau** (xem `/api/warrantyclaims/{id}/approve-check`).
    //   Thiếu chúng ⇒ không có cách nào biết phải áp luật nào, mọi hồ sơ sẽ bị xét bằng một luật
    //   duy nhất — sai đúng một nửa số trường hợp.
    /// <summary>Loại BCBH: `XM` xe mới chưa bán · `SB` sau bán (mặc định) · `PT` bảo hành phụ tùng
    /// · `TC` thiện chí · `BT` bản tin/chiến dịch.</summary>
    public string? ROWTypeCode { get; set; }
    /// <summary>Loại chi tiết: `A` AVN · `B` ắc quy · `P` sơn · `W` thông thường · `S` phụ tùng
    /// · `R` thiện chí · `C` bản tin/chiến dịch.</summary>
    public string? ROWTypeDtlCode { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ClaimNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? RONo { get; set; }
    public string? Vin { get; set; }
    public string? PlateNo { get; set; }
    public string? WarrantyType { get; set; }
    public string? PartCode { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    // Pending(Chưa gửi) -> Sent(Chờ xem xét) -> Confirmed(Chờ duyệt) -> Accepted/Rejected; Reverted(HTC hoàn trả) quay lại đại lý.
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// 🔴 TRỤC TRẠNG THÁI THỨ HAI — đồng bộ đề nghị sang API của HÃNG HMC (TConst.HMCApiStatus),
    /// ĐỘC LẬP hoàn toàn với <see cref="Status"/> (luồng duyệt nội bộ đại lý ↔ HTC):
    /// "P" chờ gửi · "A" gửi thành công · "R" gửi lỗi.
    /// Nguồn set "P" NGAY KHI TẠO đề nghị (BizCarSv.WarrantyReport.cs:2563).
    /// </summary>
    public string HMCApiStatus { get; set; } = "P";

    /// <summary>Thời điểm đồng bộ sang HMC (SyncHMCDateTime).</summary>
    public DateTime? SyncHMCDateTime { get; set; }

    /// <summary>Số biên nhận claim do HMC trả về (ClmRcptNo) — bằng chứng hãng đã nhận.</summary>
    public string? ClmRcptNo { get; set; }

    /// <summary>
    /// SỐ LẦN đã gửi THÀNH CÔNG sang HMC (HMCApiQtyA) — tăng 1 mỗi lần gửi thành công.
    /// ⚠️ Không chỉ để thống kê: từ lần đẩy THỨ 2 trở đi, số serial claim gửi hãng phải đổi
    /// ký tự "-" thành CHỮ CÁI theo số lần (lần 2 = A, lần 3 = B…) — xem <see cref="ClmNoSrl"/>.
    /// Chỉ có ở bản biz `Ser_ROWarrantyReport_SendHMCX_20260227` (máy 150), bản laptop KHÔNG có.
    /// </summary>
    public int HMCApiQtyA { get; set; }

    /// <summary>Số serial claim gửi hãng (clmNoSrl) — đã áp luật đổi "-" thành chữ cái khi gửi lại.</summary>
    public string? ClmNoSrl { get; set; }

    /// <summary>
    /// 🔴 HÃNG bảo hành chịu claim (`Ser_ROWarrantyReport.WarrantySerCode`) — CHỈ đề nghị của
    /// "HMC" hoặc "HTMV" mới được đẩy sang API hãng HMC. Bản biz cũ KHÔNG lọc cột này
    /// ⇒ đẩy nhầm cả đề nghị của hãng khác sang HMC.
    /// ⚠️ Cột này xuất hiện **duy nhất 1 lần trong toàn hệ nguồn và chỉ trên máy 150**
    /// (`BizCarSv.WarrantyReport.cs:23496`); KHÔNG tìm thấy chỗ GHI trong code C# ⇒ giá trị
    /// do nơi khác đặt (WinForm/DB). Ở đây cho nhập trực tiếp khi tạo đề nghị.
    /// </summary>
    public string? WarrantySerCode { get; set; }

    /// <summary>
    /// Thời điểm đề nghị được duyệt (`ApprovedDate`) — job đẩy HMC chỉ lấy đề nghị duyệt trong
    /// **3 ngày 5 giờ** gần nhất. ⚠️ SUY LUẬN CỦA TÔI: nguồn lọc `ApprovedDate` trên đề nghị đang ở
    /// trạng thái "CONF", nên ở đây ghi mốc này khi đề nghị CHUYỂN VÀO trạng thái Confirmed.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    public string? HtcNote { get; set; }

    // ===== 🔴 #302 CỘT THẬT CỦA `Ser_ROWarrantyReport` MÀ PORT CŨ THIẾU =====
    // Nguồn LIVE `Ser_ROWarrantyReport_Get_New20230417` (`WarrantyReport.cs:13388`) trả `td.*` —
    // dưới đây là các cột trong `td.*` mà MiniHTC chưa có chỗ chứa.
    public string? ROWNo { get; set; }          // số phiếu đề nghị bảo hành
    public string? ROID { get; set; }           // khoá lệnh sửa chữa (nguồn join `td.ROID = ro.ROID`)
    public string? CusID { get; set; }
    public string? CarID { get; set; }
    public string? Creator { get; set; }
    public string? Assistant { get; set; }      // cố vấn dịch vụ
    public string? Km { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public string? CusRequest { get; set; }     // yêu cầu của khách
    public string? CarStatus { get; set; }      // tình trạng xe lúc tiếp nhận
    public string? NaturalCode { get; set; }    // mã HIỆN TƯỢNG
    public string? CauseCode { get; set; }      // mã NGUYÊN NHÂN
    public DateTime? StartDate { get; set; }
    public string? ROWTID { get; set; }         // loại bảo hành (`Ser_MST_ROWarrantyType`)
    public string? ErrorCodeCD { get; set; }
    public string? ErrorCodePN { get; set; }
    public string? FlagReadySend { get; set; }  // sẵn sàng gửi hãng
    public string? PartIDError { get; set; }    // phụ tùng LỖI (join `Ser_MST_Part` để lấy mã + tên)
    public string? ApprovedBy { get; set; }
    public string? CreatedBy { get; set; }

    // ===== 🔴 #322 BẢN CHỤP KHÁCH + XE **TRÊN CHÍNH PHIẾU BẢO HÀNH** =====
    // Nguồn LIVE `Ser_ROWarrantyReport_Update_V2` (`WarrantyReport.cs:4174`) GHI 34 cột, trong đó có
    // khối khách/xe dưới đây — port cũ (#302) chỉ lấy cột từ hàm **Get** nên bỏ sót chúng.
    //
    // ⚠️ NGHỊCH LÝ CÓ THẬT, ghi lại để không ai "sửa cho hợp lý":
    //   hàm **Update GHI** bản chụp này lên phiếu, nhưng hàm **Get LIVE** (`_New20230417`, #302) lại
    //   **ĐỌC từ LỆNH SỬA CHỮA** (`ro.FrameNo`/`ro.PlateNo`/`ro.Warranty*`) chứ không đọc mấy cột này.
    //   ⇒ Bản chụp trên phiếu **được ghi nhưng KHÔNG được màn chi tiết dùng**. Vẫn phải port: các màn/báo
    //     cáo khác đọc thẳng bảng, và mất cột thì mất dữ liệu lịch sử.
    public string? CusName { get; set; }
    public string? CusAddress { get; set; }
    public string? CusTel { get; set; }
    public string? ModelID { get; set; }
    public string? BatteryNo { get; set; }
    public string? SerialNo { get; set; }
    public DateTime? WarrantyRegistrationDate { get; set; }
    public DateTime? WarrantyExpiresDate { get; set; }
    public decimal? WarrantyKM { get; set; }
    /// <summary>NOTE — ghi chú của ĐỀ NGHỊ. ⚠️ KHÁC `HtcNote` (ghi chú của HTC khi duyệt/từ chối).</summary>
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 DÒNG PHỤ TÙNG của đề nghị bảo hành (`Ser_ROWarrantyReportPartItems` — 1-n theo ROWID).
/// Port cũ CHỈ có 1 cột vô hướng <c>ServiceWarrantyClaim.PartCode</c> ⇒ mỗi đề nghị chỉ khai được
/// ĐÚNG 1 phụ tùng, không có số lượng / đơn giá / VAT / nguồn gốc PT ⇒ mất toàn bộ chiều chi tiết
/// và mọi luật kiểm tra theo dòng của nguồn (BizCarSv.WarrantyReport.cs:960-1290, máy 150 canonical).
/// </summary>
/// <summary>
/// 🔴 #303 DÒNG CÔNG của đề nghị bảo hành — `Ser_ROWarrantyReportServiceItems`, **chưa từng port**.
/// Port cũ chỉ có dòng PHỤ TÙNG (<see cref="WarrantyClaimPartItem"/>) ⇒ đề nghị bảo hành **chỉ có
/// tiền phụ tùng, không có tiền công** — báo cáo chấp thuận bảo hành thiếu hẳn một nửa số tiền.
/// Cột lấy từ bản đồ ghi thật `lstMapFN` (`WarrantyReport.cs:206-217`), không lấy theo lưới.
/// </summary>
public sealed class WarrantyClaimServiceItem
{
    /// <summary>🔴 #396 §12 Mốc duyệt LAN xuống dòng công khi HTC duyệt đề nghị.</summary>
    public DateTime? ApprovedDate { get; set; }
    /// <summary>#396 §12 Người duyệt, lan xuống dòng công.</summary>
    public string? ApprovedBy { get; set; }
    public long Id { get; set; }                 // ItemID
    public Guid OrgId { get; set; }
    /// <summary>ROWID — khoá về đề nghị bảo hành.</summary>
    public long ClaimId { get; set; }

    public string? SerID { get; set; }           // khoá dịch vụ (join `Ser_MST_Service` lấy mã + tên)
    public string? SerCode { get; set; }
    public string? SerName { get; set; }

    /// <summary>
    /// 🔴 ROWSerType (`TConst.ROWSerType`, `Const.Main.cs:483`): **"CVC" công việc chính** · "CVPSN".
    /// Báo cáo chấp thuận chỉ lấy **MỘT** dòng đại diện — `top 1 itemid … where ROWSerType = 'CVC'`.
    /// ⚠️ Cột này **KHÔNG có trong `lstMapFN` của hàm Create** ⇒ nguồn không ghi nó lúc tạo, nhưng báo cáo
    ///    LẠI LỌC theo nó. Dòng nào chưa được đặt "CVC" ở đâu đó sẽ **không bao giờ ra báo cáo**.
    /// </summary>
    public string? ROWSerType { get; set; }

    public decimal Factor { get; set; } = 1;
    public decimal Price { get; set; }
    public decimal VAT { get; set; }
    /// <summary>Giờ công định mức, nguồn lấy qua join `ser_mst_service.StdManHour` (xem #297).</summary>
    public decimal? StdManHour { get; set; }

    /// <summary>Trạng thái RIÊNG của dòng. ⚠️ Báo cáo chấp thuận **KHÔNG lọc theo cột này**
    /// (`--AND rwrs.WarrantyStatus = 'ACCE'` đã bị comment) — xem chú thích ở endpoint báo cáo.</summary>
    public string? WarrantyStatus { get; set; }

    public string? Note { get; set; }
    /// <summary>BULLETINID — bản tin kỹ thuật. ⚠️ Nguồn coi chuỗi **"0" như RỖNG** (bỏ qua, không ghi).</summary>
    public string? BulletinID { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

public sealed class WarrantyClaimPartItem
{
    /// <summary>🔴 #396 §12 Mốc duyệt LAN xuống dòng phụ tùng khi HTC duyệt đề nghị.</summary>
    public DateTime? ApprovedDate { get; set; }
    /// <summary>#396 §12 Người duyệt, lan xuống dòng phụ tùng.</summary>
    public string? ApprovedBy { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>ROWID — khoá về đề nghị bảo hành (<see cref="ServiceWarrantyClaim.Id"/>).</summary>
    public long ClaimId { get; set; }

    /// <summary>PartID/PartCode — mã phụ tùng.</summary>
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }

    /// <summary>
    /// 🔴 LOẠI PHỤ TÙNG trong đề nghị bảo hành (`TConst.ROWPartType`) — trục mà port cũ THIẾU HẲN:
    /// "PTC" phụ tùng chính · "PTTT" phụ tùng thay thế · "VTP" vật tư phụ.
    /// Nguồn BẮT BUỘC nhập (rỗng ⇒ "PT {mã} loại phụ tùng không được trống!") và
    /// mỗi đề nghị chỉ được ĐÚNG 1 dòng "PTC" ("Báo cáo bảo hành chỉ có 1 phụ tùng chính!").
    /// </summary>
    public string RowPartType { get; set; } = "PTTT";

    /// <summary>
    /// Nguồn gốc phụ tùng (`TConst.ROWPartOrderType`): "TST" mua qua đơn đặt TST · "OTHER" nguồn khác.
    /// Nguồn BẮT BUỘC nhập ("Chưa có nguồn gốc phụ tùng!") và chỉ nhận đúng 2 giá trị này.
    /// </summary>
    public string PartOrderType { get; set; } = "OTHER";

    /// <summary>Số đơn đặt phụ tùng — BẮT BUỘC khi <see cref="PartOrderType"/> = "TST".</summary>
    public string? PartOrderNo { get; set; }

    public decimal Quantity { get; set; } = 1;
    public decimal Price { get; set; }
    /// <summary>Hệ số (Factor) — nhân vào thành tiền theo nguồn.</summary>
    public decimal Factor { get; set; } = 1;
    public decimal Vat { get; set; }
    /// <summary>Giá bảo hiểm chi trả (InsurancePrice).</summary>
    public decimal InsurancePrice { get; set; }
    /// <summary>Loại chi phí (ExpenseType) của dòng.</summary>
    public string? ExpenseType { get; set; }
    /// <summary>Trạng thái bảo hành RIÊNG của dòng (WarrantyStatus) — độc lập trạng thái đề nghị.</summary>
    public string? WarrantyStatus { get; set; }
    /// <summary>Cờ phụ tùng chính (FlagMainPart) — nguồn lưu tách khỏi <see cref="RowPartType"/>.</summary>
    public string? FlagMainPart { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Chăm sóc khách hàng sau dịch vụ (Ser_CustomerCare — port 1:1 FrmCustomerCare, TCMotor DMSCarSv/Customer):
/// CRM follow-up. CareType (TConst.SerCareType): 24h / 72h / dob (sinh nhật) / man (nhắc bảo dưỡng).
public sealed class CustomerCare
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CareNo { get; set; } = "";
    /// <summary>
    /// #220 parity `TConst.SerCareType` (Const.Main.cs:349): **`24h` · `72h` · `dob` · `man`**.
    /// Bộ `CARE24H/CARE72H/DOB/MAINT` của port cũ KHÔNG có ở nguồn ⇒ đã đổi + migration.
    /// ⚠️ Mã nguồn viết **thường** (`24h`, `dob`), không viết hoa — giữ đúng chính tả nguồn (luật
    /// `C0-trecentesimusvicesimusseptimus`) vì SQL nguồn so bằng `=` trên chuỗi.
    /// </summary>
    public string CareType { get; set; } = "24h";
    public string? RONo { get; set; }
    public string? PlateNo { get; set; }
    public string? CusName { get; set; }
    public string? CusPhone { get; set; }
    public DateTime? ContactDate { get; set; }           // ngày dự kiến liên hệ
    /// <summary>Trạng thái liên hệ — port đúng 4 mã của nguồn (TConst SerCareStatus):
    /// PEND (chưa liên hệ) → CINFB (đã liên hệ, chưa phản hồi) / CIFB (đã liên hệ, đã phản hồi) / REJ (không cần liên hệ).
    /// Giữ thêm Pending/Contacted/Closed cho dữ liệu cũ đã tạo trước khi vá.</summary>
    public string Status { get; set; } = "PEND";
    public string? Result { get; set; }                  // kết quả liên hệ

    // ===== #210 parity `Ser_CustomerCare_Update` / `Ser_CustomerCare72h_UpdateStatus_New20180622`
    //       (DMSCarSv V20.2023.Release.V2 — TERP.BizCarSv/BizCarSv.Customer.cs:11958 / 15728) =====
    // 🔴 Nguồn KHÔNG cập nhật `Status` trong lệnh sửa thường: trạng thái liên hệ được biểu diễn bằng
    //    HAI CỜ ĐỘC LẬP `IsCall` (đã gọi) và `IsFeedback` (đã phản hồi) — đúng bằng taxonomy
    //    `SerCareStatus`: PEND (chưa gọi) · CINFB (gọi rồi, chưa phản hồi) · CIFB (gọi rồi, đã phản hồi).
    //    `Status` chỉ được ghi trực tiếp bởi `Ser_CustomerCareStatusUpdate*` (caller truyền vào) và khi TẠO (PEND).
    public string? IsCall { get; set; }
    public string? IsFeedback { get; set; }
    /// <summary>Nội dung khách phản hồi (`CusFeedback`) — nguồn tách riêng với `Note`.</summary>
    public string? CusFeedback { get; set; }
    public string? IsSendmail { get; set; }
    public string? Note { get; set; }


    /// <summary>#278 DealerCode — đại lý của phiếu CSKH. Bản tổng đài iCIC lọc **danh sách đại lý bị
    /// loại** trên CẢ BA bảng (tt/cus/car); MiniHTC gộp về một cột trên phiếu.</summary>
    public string? DealerCode { get; set; }
    // ===== 🔴 #457 §12 HAI KHOÁ THẬT của phiếu CSKH (`Ser_CustomerCare.CusID` / `.CarID`) =====
    /// <summary>`CusID` — nguồn nối `inner join Ser_Customer cus on tt.CusID = cus.CusID`.</summary>
    public string? CusID { get; set; }
    /// <summary>`CarID` — nguồn nối `join ser_car car on tt.carID = car.carID **and tt.CusID = car.CusID**`
    /// (nối HAI cột, và là INNER) ⇒ xe đã sang tên chủ khác thì phiếu CSKH biến mất khỏi danh sách.</summary>
    public string? CarID { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ContactedAt { get; set; }
}

/// <summary>
/// #218 parity `Ser_CustomerCareMaintance` — NHẮC BẢO DƯỠNG theo phiếu CSKH
/// (port 1:1 `FrmCSCCustomerCareMaintance`, TCMotor DMSCarSv/Customer; nguồn `BizCarSv.Customer.cs:16472`).
/// 🔴 Bảng RIÊNG, khoá `CusCareID` — quan hệ 1-1 với phiếu <see cref="CustomerCare"/>; nguồn **upsert**:
/// chưa có thì insert, có rồi thì update đúng ba cột `DateAppointment` · `ContactDate` · `Note`.
/// ⚠️ Mỗi lần lưu, nguồn còn gọi `Ser_CustomerCareStatusUpdate` để đặt `Status` trên PHIẾU CHÍNH
/// ⇒ một thao tác chạm HAI bảng.
/// </summary>
public sealed class CustomerCareMaintance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Số phiếu CSKH (nguồn: `CusCareID`) — khoá upsert.</summary>
    public string CareNo { get; set; } = "";
    /// <summary>Ngày hẹn bảo dưỡng (`DateAppointment`).</summary>
    public string? DateAppointment { get; set; }
    public DateTime? ContactDate { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

/// <summary>Chăm sóc khách hàng nhân dịp SINH NHẬT
/// port 1:1 FrmCSCCustomerCareDOB / FrmCustomerCareBth, TCMotor DMSCarSv/Customer).
/// ⚠️ Là BẢNG RIÊNG ở nguồn, KHÔNG phải một loại của <see cref="CustomerCare"/>:
/// có khoá riêng (CareBthId) và **bộ trạng thái riêng "0/1/2"**, khác hẳn PEND/CINFB/CIFB/REJ.
/// </summary>
public sealed class CustomerCareBirthday
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Khoá của bản ghi ở nguồn (CAREBTHID).</summary>
    public string? CareBthId { get; set; }

    public string CusId { get; set; } = "";
    public string? DealerCode { get; set; }

    /// <summary>
    /// Ngày sinh nhật đã CHUẨN HOÁ VỀ NĂM HIỆN TẠI (DATEBTH) — nguồn không lưu năm sinh gốc ở đây
    /// mà lưu ngày sinh nhật của năm nay để lọc/nhắc.
    /// </summary>
    public DateTime? DateBth { get; set; }

    /// <summary>Trạng thái liên hệ: "0" chưa liên hệ · "1" đã liên hệ · "2" không liên hệ.</summary>
    public string Status { get; set; } = "0";

    public DateTime? ContactDate { get; set; }
    public string? Remark { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    // ===== #217 parity `Ser_CustomerCareBth_Update` (BizCarSv.Customer.cs:14982) =====
    // Đối chiếu từng cột: nguồn ghi 11 cột; entity có 8 ⇒ bổ sung 3 cột dưới đây.
    /// <summary>Người TẠO phiếu (`CreatedBy`) — nguồn giữ riêng, không dùng chung với `UpdatedBy`.</summary>
    public string? CreatedBy { get; set; }
    /// <summary>⚠️ Nguồn viết `LogLuDateTime` (chữ **u** thường ở giữa) — giữ đúng chính tả của nguồn
    /// để đối chiếu sau này không lệch; các bảng khác dùng `LogLUDateTime`.</summary>
    public DateTime? LogLuDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Phiếu khảo sát chăm sóc khách hàng sau dịch vụ (Ser_CustomerCare24h / Ser_CustomerCare72h —
/// port 1:1 FrmCSCCustomerCare24h/72h, TCMotor DMSCarSv/Customer).
/// Mỗi phiếu CSKH (<see cref="CustomerCare"/>) có TỐI ĐA MỘT bản khảo sát: nguồn đọc
/// "top 1 * where CusCareID = ..." rồi insert-nếu-chưa-có / update-nếu-đã-có (upsert theo CareNo).
/// Bộ 6 câu hỏi dùng CHUNG cho cả 24h và 72h (form 24h nạp hằng số của lớp SerCusCare72hQA).
/// 🔴 #220 — ÁNH XẠ TÊN CỘT giữa hai bảng nguồn (chúng là HAI bảng riêng, tên cột khác nhau):
///   `Ser_CustomerCare72h` → `Ser_CustomerCare24h`
///     `FyourCSSH`        → `FyourCSSH24`
///     `WFBasicNeeds`     → `WFBasicNeed24s`   ⚠️ hậu tố **"24s"** chứ không phải "s24" — chính tả nguồn
///     `YourCarProblem`   → `YourCarProblem24`
///     `YourRIWN`         → `YourRIWN24`
///     `YourSatisfyQSv`   → `YourSatisfyQSv24`
///     `YourHopeOfOur`    → `YourHopeOfOur24`
///     `FinishedDate`     → `FinishedDate24` · `ContactDate` → `ContactDate24`
///   Các cột chung không đổi tên: `CusCareID` · `ROID` · `OrderID` · `Note` · `CreatedBy` · `CreatedDate` ·
///   `LogLUBy` · `LogLUDateTime`.
///   MiniHTC gộp một entity cho cả hai (phân biệt bằng <see cref="CustomerCare.CareType"/>); giữ tên **không
///   hậu tố** của bản 72h. Ghi ánh xạ ở đây để lượt sau tách bảng (nếu cần) không phải dò lại nguồn.
/// </summary>
public sealed class CustomerCareSurvey
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Số phiếu CSKH được khảo sát (nguồn: CusCareID) — khoá upsert.</summary>
    public string CareNo { get; set; } = "";

    /// <summary>Lệnh sửa chữa gắn với phiếu (nguồn: ROID).</summary>
    public string? RONo { get; set; }

    /// <summary>Ngày hoàn thành sửa chữa hiển thị trên form (nguồn: FinishedDate24, ô chỉ đọc).</summary>
    public DateTime? FinishedDate { get; set; }

    /// <summary>Ngày liên hệ khách (nguồn: ContactDate24).</summary>
    public DateTime? ContactDate { get; set; }

    // --- 6 câu trả lời khảo sát, lưu đúng MÃ đáp án của nguồn (vd "YourCarProblem_Yes") ---

    /// <summary>Câu 1 — Xe làm dịch vụ có vấn đề gì không? (YourCarProblem_Yes/_No)</summary>
    public string? YourCarProblem { get; set; }

    /// <summary>Câu 2 — Có hài lòng về chất lượng dịch vụ không? (YourSatisfyQSv_Yes/_No/_Consider)</summary>
    public string? YourSatisfyQSv { get; set; }

    /// <summary>Câu 3 — Thái độ phục vụ và tư vấn của nhân viên (FyourCSSH_OK/_Nomarl/_No/_Other)</summary>
    public string? FyourCSSH { get; set; }

    /// <summary>Câu 4 — Sẵn sàng quay lại xưởng lần sửa chữa tiếp theo? (YourRIWN_Yes/_No)</summary>
    public string? YourRIWN { get; set; }

    /// <summary>Câu 5 — Cơ sở vật chất đáp ứng nhu cầu chưa? (WFBasicNeeds_OK/_Nomarl/_No/_Other)</summary>
    public string? WFBasicNeeds { get; set; }

    /// <summary>Câu 6 — Mong muốn ở sự phục vụ của công ty (tự luận; nguồn ghi cùng giá trị với Note).</summary>
    public string? YourHopeOfOur { get; set; }

    /// <summary>Ghi chú phiếu (nguồn: Note24).</summary>
    public string? Note { get; set; }


    // ===== #211 parity `Ser_CustomerCare72h` (DMSCarSv V20.2023.Release.V2 —
    //       TERP.BizCarSv/BizCarSv.Customer.cs:16010-16130, hàm `Ser_CustomerCare72h_UpdateStatus_New20180622`) =====
    // Đối chiếu từng cột: nguồn ghi 44 cột; entity đang có 11 ⇒ bổ sung 33 cột dưới đây.
    /// <summary>Loại phiếu CSKH (`CusCareType`) — nguồn lưu lại ngay trên bản khảo sát.</summary>
    public string? CusCareType { get; set; }
    /// <summary>⚠️ Nguồn gán `OrderID = strCusCareID` (CÙNG giá trị với `CusCareID`) — giữ nguyên,
    /// không suy diễn thành mã đơn hàng.</summary>
    public string? OrderID { get; set; }
    public string? Remark { get; set; }
    /// <summary>Email nhận bản khảo sát (`SurveyGmail`).</summary>
    public string? SurveyGmail { get; set; }
    /// <summary>Mốc gửi/ghi khảo sát (`SurveyDateTime`) — nguồn đặt = thời điểm lưu.</summary>
    public DateTime? SurveyDateTime { get; set; }

    // 🔴 `Survey1..Survey28`: bộ câu hỏi TRẮC NGHIỆM, là **cột RỜI** trên chính bảng khảo sát
    //    (không phải bảng con) — giữ đúng dạng nguồn, cùng kiểu với cụm `DlsDealSurvey` đã port.
    public string? Survey1 { get; set; }
    public string? Survey2 { get; set; }
    public string? Survey3 { get; set; }
    public string? Survey4 { get; set; }
    public string? Survey5 { get; set; }
    public string? Survey6 { get; set; }
    public string? Survey7 { get; set; }
    public string? Survey8 { get; set; }
    public string? Survey9 { get; set; }
    public string? Survey10 { get; set; }
    public string? Survey11 { get; set; }
    public string? Survey12 { get; set; }
    public string? Survey13 { get; set; }
    public string? Survey14 { get; set; }
    public string? Survey15 { get; set; }
    public string? Survey16 { get; set; }
    public string? Survey17 { get; set; }
    public string? Survey18 { get; set; }
    public string? Survey19 { get; set; }
    public string? Survey20 { get; set; }
    public string? Survey21 { get; set; }
    public string? Survey22 { get; set; }
    public string? Survey23 { get; set; }
    public string? Survey24 { get; set; }
    public string? Survey25 { get; set; }
    public string? Survey26 { get; set; }
    public string? Survey27 { get; set; }
    public string? Survey28 { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Chiến dịch marketing HTC gửi đại lý (Ser_CampaignMarketing — port 1:1 FrmSer_CampaignMarketing/Mng + FrmListDealer(Update),
/// TCMotor DMSCarSv/Ser_CampaignMarketing): header + điều kiện áp dụng (VIN/biển số/đại lý, lưu CSV theo đúng cách nhập tay của WinForm)
/// + danh sách phụ tùng khuyến mãi kèm % giảm.</summary>
public sealed class CampaignMarketing
{
    // ===== 🔴 #392 §12 TRẠNG THÁI + DẤU DUYỆT của chiến dịch marketing =====
    /// <summary>Trạng thái: `P` = chờ duyệt (Pending) · `A` = đã duyệt (Approve).
    /// 🔴 Khi duyệt, nguồn **lan trạng thái này xuống SÁU bảng con** — xem
    /// `POST /api/campaignmarketings/{no}/approve`.</summary>
    public string CamMarketingStatus { get; set; } = "P";
    /// <summary>Thời điểm duyệt (`ApprDTime`).</summary>
    public DateTime? ApprDTime { get; set; }
    /// <summary>Người duyệt (`ApprBy`).</summary>
    public string? ApprBy { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    public string CamName { get; set; } = "";
    public string? CamDesc { get; set; }
    public DateTime EffDateStart { get; set; }
    public DateTime EffDateEnd { get; set; }
    public DateTime? WarrantyDateStart { get; set; }
    public DateTime? WarrantyDateEnd { get; set; }
    public string? ConditionVin { get; set; }        // CSV
    public string? ConditionPlateNo { get; set; }     // CSV
    public string? ConditionDealer { get; set; }      // CSV
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phụ tùng khuyến mãi trong chiến dịch marketing (dòng) — port 1:1 grid gridCPart, TCMotor.</summary>
public sealed class CampaignMarketingPart
{
    /// <summary>#392 §12 Trạng thái LAN từ chiến dịch cha khi duyệt (`CamMarketingPartStatus`).</summary>
    public string? CamMarketingPartStatus { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CampaignId { get; set; }
    public string PartCode { get; set; } = "";
    public decimal PercentDiscount { get; set; }
}

/// <summary>Chăm sóc KH chương trình MACE hãng (Ser_CustomerCareMace — port 1:1 FrmCustomerCareMace/Update/ApointDate, TCMotor DMSCarSv/Customer):
/// chương trình CSKH riêng theo MaceType (mã do hãng quy định), khác CustomerCare thường (24h/72h/DOB/Maint).
/// WinForm gốc chỉ SEARCH + cập nhật trạng thái liên hệ (không tạo tay từng bản — nguồn phát sinh từ hãng);
/// ở đây thêm POST tạo để có đường nhập liệu thủ công tương đương.</summary>
// ===== #490 §12: bốn khoá/cột của `Ser_CustomerCareMace` mà bản port cũ chưa có =====
public sealed class CustomerCareMace
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CareNo { get; set; } = "";
    public string MaceType { get; set; } = "";
    public string? RONo { get; set; }
    public string? Vin { get; set; }
    public string? CusName { get; set; }
    public string Status { get; set; } = "Pending";  // Pending(Chưa liên hệ)/Contacted(Đã liên hệ)/NotContacted(Không liên hệ)
    public DateTime? ContactDate { get; set; }
    public DateTime? ApointDate { get; set; }
    public DateTime? MaceRecomentDate { get; set; }
    public string? Remark { get; set; }
    /// <summary>#490 `DealerCode` — nguồn lọc `t.DealerCode` và ghi từ `Ser_RO.DealerCode`.</summary>
    public string? DealerCode { get; set; }
    /// <summary>#490 `CusID` / `CarID` — hai khoá nối sang khách và xe (nguồn nối `t.CusID = cus.CusID`,
    /// `t.CarId = car.CarId`; vế `and t.cusId = car.CusId` **đã bị COMMENT** ở nguồn).</summary>
    public string? CusID { get; set; }
    public string? CarID { get; set; }
    /// <summary>#490 `ROID` — nguồn nối `join ser_ro ro on t.ROID = ro.ROID` (**INNER**).</summary>
    public string? ROID { get; set; }
    /// <summary>#490 `CreatedDate` — nguồn ghi mốc tạo phiếu nhắc.</summary>
    public DateTime? CreatedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phụ tùng nợ khách (Ser_Part_OO — port 1:1 FrmNewSerPartOO/FrmMngSerPartOO, TCMotor DMSCarSv/Services):
/// PT hết hàng nhưng đã hứa khách theo biển số, chờ đặt hàng về trả tiếp. Upsert theo (PlateNo, PartCode).</summary>
public sealed class PartBackorder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PlateNo { get; set; } = "";
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? CarType { get; set; }
    public string? StaffCode { get; set; }         // CVDV
    /// <summary>Đại lý ghi nhận khoản nợ phụ tùng (Ser_Part_OO.DealerCode).</summary>
    public string? DealerCode { get; set; }
    public decimal QtyOwed { get; set; }
    public decimal QtyReturned { get; set; }
    public DateTime? PromiseDate { get; set; }      // NgayHenTra
    public DateTime? OrderDate { get; set; }        // NgayDatHang
    public DateTime? ExpectedDate { get; set; }     // NgayVeDK
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Khách hàng dịch vụ (Ser_Customer — port 1:1 FrmCustomerInfo, TCMotor DMSCarSv/Customer):
/// customer master dịch vụ (cá nhân/tổ chức) + người liên hệ. CustomerCar/Care tham chiếu theo CusCode.</summary>
public sealed class ServiceCustomer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CusCode { get; set; } = "";
    public string CusName { get; set; } = "";
    public string? CusTypeID { get; set; }             // loại KH (cá nhân/tổ chức)
    public string? Address { get; set; }
    public string? Mobile { get; set; }
    public string? Tel { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? Sex { get; set; }                   // True=nam, False=nữ (theo gốc)
    public DateTime? DOB { get; set; }
    public string? ContName { get; set; }              // người liên hệ (tổ chức)
    public string? ContMobile { get; set; }
    public string? ContTel { get; set; }
    // #301: ContAddress ĐÃ CÓ sẵn phía dưới (dòng ~5415) — không khai lại. Nguồn dùng nó làm
    //   DỰ PHÒNG CẤP 3 cho địa chỉ trên lệnh sửa chữa: isnull(ro.CusAddress, isnull(cus.Address, cus.ContAddress)).

    // ===== #221 parity `CustomerCreate` / `CustomerUpdate` (DMSCarSv —
    //       TERP.HTCServiceClient/DbServices/MstCustomerService.cs:72 / :249) =====
    // Đối chiếu từng trường: nguồn gửi **28 trường**; entity có 13 ⇒ bổ sung 15 trường dưới đây.
    /// <summary>Đại lý quản lý khách hàng.</summary>
    public string? DealerCode { get; set; }
    /// <summary>Tỉnh/thành.</summary>
    public string? ProvinceCode { get; set; }
    /// <summary>Quận/huyện.</summary>
    public string? DistrictCode { get; set; }
    /// <summary>Số fax của khách.</summary>
    public string? Fax { get; set; }
    /// <summary>Website (khách doanh nghiệp).</summary>
    public string? Website { get; set; }
    /// <summary>Số CMND/CCCD.</summary>
    /// <summary>🔴 #362 SALESCUSID — **mã khách hàng bên hệ SALES**. Đồng bộ sang Veloca ưu tiên mã này,
    /// chỉ rơi về mã CarSv khi nó trống. Chú thích nguồn (`20240401`): *"Dùng mã KH Sales để đồng bộ từ
    /// CarSv → VelocaDV… Mục đích: 1 KH được đồng bộ ở cả DMS và Veloca đều có 1 mã KH người dùng duy nhất"*.
    /// ⇒ Thiếu cột này thì mọi khách sang Veloca đều mang mã CarSv ⇒ **hỏng đúng mục đích hợp nhất mã**.</summary>
    public string? SalesCusID { get; set; }
    public string? IDCardNo { get; set; }
    /// <summary>Ngân hàng giao dịch.</summary>
    public string? Bank { get; set; }
    /// <summary>Số tài khoản ngân hàng.</summary>
    public string? BankAccountNo { get; set; }
    /// <summary>Loại hình tổ chức (khách doanh nghiệp).</summary>
    public string? OrgTypeID { get; set; }
    /// <summary>Cờ khách LẺ (form FrmCustomerNormal truyền riêng, nguồn nhận cả ở Create lẫn Update).</summary>
    public string? IsNormal { get; set; }
    /// <summary>Cờ có người liên hệ đại diện.</summary>
    public string? IsContact { get; set; }
    /// <summary>Địa chỉ người liên hệ.</summary>
    public string? ContAddress { get; set; }
    /// <summary>Fax người liên hệ.</summary>
    public string? ContFax { get; set; }
    /// <summary>Giới tính người liên hệ.</summary>
    public string? ContSex { get; set; }
    /// <summary>Ghi chú.</summary>
    public string? Note { get; set; }

    public string? ContEmail { get; set; }

    /// <summary>#269 `Ser_Customer.IsActive` (đặt tên `FlagActive` theo lệ của `ServiceCar`).
    /// Job NoShow lọc **theo cờ của KHÁCH HÀNG**, không phải cờ của XE — đọc kỹ alias trong nguồn:
    /// `and cus.IsActive = @strIsActive`.</summary>
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đơn đặt phụ tùng từ NCC (Ser_Order_Part — port 1:1 FrmSer_Order_Part, TCMotor DMSCarSv/TST):
/// đơn mua phụ tùng gửi nhà cung cấp. OrderPartStatus: Pending(Mới tạo)→Approved(Đã gửi NCC)→Finished(Hoàn thành).</summary>
public sealed class OrderPart
{
    // ===== 🔴 #389 §12 TÁM CỘT của màn SỬA đơn đặt phụ tùng (`Ser_Part_OrderUpdate`) =====
    //   Nguồn ghi 19 cột trong `alColumnEffective`; MiniHTC trước lượt này thiếu tám cột dưới đây,
    //   nên màn sửa không thể port đủ. Xem `PUT /api/orderparts/{no}`.
    /// <summary>Số đơn do NGƯỜI DÙNG tự đặt (`OrderNoUser`) — khác số hệ thống sinh.
    /// 🔴 Thuộc nhóm **rỗng = XOÁ** (nguồn có nhánh `else → DBNull`).</summary>
    public string? OrderNoUser { get; set; }
    /// <summary>Ngày nhận hàng (`ReceivePartDate`). 🔴 **rỗng = XOÁ**.</summary>
    public DateTime? ReceivePartDate { get; set; }
    /// <summary>Ngày duyệt (`ApprovedDate`). 🔴 **rỗng = XOÁ**.</summary>
    public DateTime? ApprovedDate { get; set; }
    /// <summary>Số xác nhận của HTC (`ConfirmNo`, issue 985 — có guard trùng riêng).
    /// 🔴 **rỗng = XOÁ**.</summary>
    public string? ConfirmNo { get; set; }
    /// <summary>Phí khách chịu (`CusCharges`, issue 1017). 🔴 **rỗng = XOÁ**.</summary>
    public string? CusCharges { get; set; }
    /// <summary>Cờ HTC đã xác nhận (`HTCConfirm`). ⚠️ **rỗng = GIỮ** (không có nhánh else).</summary>
    public string? HTCConfirm { get; set; }
    /// <summary>Cho giao hàng từng phần (`PartialShipment`). ⚠️ **rỗng = GIỮ**.</summary>
    public string? PartialShipment { get; set; }
    /// <summary>Hình thức vận chuyển (`TypeTransport`). ⚠️ **rỗng = GIỮ**.</summary>
    public string? TypeTransport { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderPartNo { get; set; } = "";
    public string SupplierCode { get; set; } = "";
    public string? WarehouseCode { get; set; }

    /// <summary>
    /// 🔴 #234 Trạng thái đơn theo `TConst.OrderPartStatus` (Const.Main.cs:536) — **MỘT KÝ TỰ**:
    /// "P" Mới tạo · "A" Đã duyệt/gửi NCC · "F" Hoàn thành · "R" Từ chối.
    /// ⚠️ Port cũ lưu chuỗi dài ("Pending"/"Approved"/"Finished"/"Rejected") trong khi **dòng chi tiết**
    ///    `OrderPartLine.OrderPartStatusDtl` lại lưu "A"/"F"/"R" ⇒ header và dòng dùng **hai bộ mã khác nhau**,
    ///    lọc theo trạng thái sẽ trượt. Nay thống nhất về mã 1 ký tự của nguồn; dữ liệu cũ đọc được
    ///    qua bảng ánh xạ `orderPartLegacyStatusMap` trong Program.cs.
    /// </summary>
    public string OrderPartStatus { get; set; } = "P";

    public DateTime CreatedAt { get; set; } = DateTime.Now;   // CreateDTime
    public DateTime? SentAt { get; set; }                     // ` ApprDTime
    public DateTime? FinishedAt { get; set; }                 // ` FinishDTime

    // ===== 🔴 #234: cột nguồn `Ser_Order_Part` mà port cũ THIẾU =====
    // Nguồn: `Entities/TST/Ser_Order_Part.cs` (md5 bc2a2708) + chữ ký `Ser_Order_Part_Save` / `_Appr`
    // (Ser_Order_PartService.cs:141 / :182).

    /// <summary>DealerCode — đại lý đặt hàng. Port cũ không có ⇒ không biết đơn của đại lý nào.</summary>
    public string? DealerCode { get; set; }

    /// <summary>SupplierID — 🔴 KHÁC `SupplierCode`: nguồn gửi `SupplierID` trong `_Save`,
    /// còn `SupplierName` lấy qua join (`msp_SupplierName`). Giữ cả hai, không gộp.</summary>
    public string? SupplierID { get; set; }

    public string? PartGroupID { get; set; }            // nhóm phụ tùng
    public string? DeliveryFormCode { get; set; }       // hình thức giao hàng (Mst_DeliveryForm)
    public string? DeliveryLocationCode { get; set; }   // địa điểm giao hàng — tra Mst_DeliveryLocation (#232)
    public DateTime? EstimatedDeliverDate { get; set; } // ngày giao dự kiến
    public string? VIN { get; set; }
    public string? Remark { get; set; }

    // --- ba trường CHỈ nhập lúc DUYỆT (`Ser_Order_Part_Appr` gửi, `_Save` KHÔNG gửi) ---
    public DateTime? RequestSuppierDate { get; set; }   // ngày yêu cầu NCC (nguồn viết thiếu chữ "l": Suppier)
    public DateTime? ResponseSuppierDate { get; set; }  // ngày NCC phản hồi
    public string? OrderSuppierNo { get; set; }         // số đơn đặt phía NCC

    /// <summary>
    /// SupplierStatus — 🔴 trạng thái phía NCC, **cột RIÊNG** với `OrderPartStatus`.
    /// Mã là SỐ NHẢY (`TConst.SupplierStatus`, Const.Main.cs:544): "1" Chờ duyệt · "2" Đã duyệt, chờ hoàn
    /// thiện · "4" Đã hoàn thiện · "7" Đơn lỗi, chờ kinh doanh điều chỉnh. **Không có 3/5/6.**
    /// </summary>
    public string? SupplierStatus { get; set; }

    /// <summary>OrderPartType (`TConst.OrderPartType`): "TST" · "OTHER".</summary>
    public string? OrderPartType { get; set; }

    public string? TSTID { get; set; }
    public DateTime? SupplierLUDTime { get; set; }
    // ===== 🔴 #236 BA CỘT TỔNG THẬT — đã đối chiếu biz, SỬA suy luận sai ở #235 =====
    // Nguồn `BizCarSv.A.02.OrderPart.cs:1050-1074` (md5 72e6623f, 5014 dòng — khớp 2 máy) gom từ dòng
    // rồi `update Ser_Order_Part` **ĐÚNG BA cột**:
    //     TotalValOrderBeforeDc = Sum(TPBeforeDc)
    //     TotalValOrderAfterDc  = Sum(TPAfterDc)
    //     TotalValOrderAfterVAT = Sum(TPAfterVAT)
    // ⚠️ POCO client `Entities/TST/Ser_Order_Part.cs` **CHỈ khai 1 trong 3** (`TotalValOrderAfterVAT`)
    //    ⇒ POCO cũng THIẾU CỘT, không phải danh sách đầy đủ của bảng.
    public decimal? TotalValOrderBeforeDc { get; set; }  // Sum(TPBeforeDc) — tổng trước chiết khấu
    public decimal? TotalValOrderAfterDc { get; set; }   // Sum(TPAfterDc)  — tổng sau chiết khấu
    public decimal? TotalValOrderAfterVAT { get; set; }  // Sum(TPAfterVAT) — tổng sau VAT

    /// <summary>
    /// 🔴 `ValDiscount` — TÊN GÂY NHẦM của nguồn. #235 tôi đoán nó là "tổng tiền chiết khấu của đơn"
    /// và tính `Sum(TPBeforeDc − TPAfterDc)` — **SAI**. Đối chiếu biz:
    /// `Ser_Order_PartDtl_Create(... object objValDiscount ...)` ghi thẳng vào
    /// `dt_OrderPart_Detail.Rows[0]["DiscountRate"]` (BizCarSv.A.02.OrderPart.cs:4913)
    /// ⇒ `ValDiscount` chỉ là **tên tham số** của `DiscountRate` **CỦA DÒNG**, KHÔNG phải cột tổng đầu đơn.
    /// Nguồn **không có** cột tổng chiết khấu ở đầu đơn (muốn biết thì lấy
    /// `TotalValOrderBeforeDc − TotalValOrderAfterDc`). Giữ cột vì POCO có khai, nhưng **không tự tính**.
    /// </summary>
    public decimal? ValDiscount { get; set; }

    // --- vết ghi/duyệt ---
    public string? CreateBy { get; set; }
    public string? ApprBy { get; set; }
    public string? FinishBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng phụ tùng đặt (Ser_Order_Part_Dtl): mã PT + SL đặt + đơn giá.</summary>
public sealed class OrderPartLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long OrderPartId { get; set; }
    public string PartCode { get; set; } = "";     // part_PartCode (enrich từ Mst_Part)
    public string? PartName { get; set; }          // part_VieName  (enrich từ Mst_Part)
    public decimal OrderQty { get; set; } = 1;     // QtyOrd — số lượng ĐẶT
    public decimal Price { get; set; }             // Price

    // ===== 🔴 #235: cột nguồn `Ser_Order_PartDtl` mà port cũ THIẾU =====
    // Nguồn `Entities/TST/Ser_Order_PartDtl.cs` (md5 b85c8e5f, khớp 2 máy) — 28 tên, chia BA nhóm:
    //  (a) 18 cột thật của bảng · (b) 4 cột enrich prefix `part_` (lấy từ `Mst_Part` qua join)
    //  (c) 5 cột TỒN KHO tính sẵn cho lưới, chú thích "Cột L/Q/N/M/P" = vị trí cột file Excel xuất ra.

    public string? PartID { get; set; }            // PartID — khoá kỹ thuật, KHÁC part_PartCode
    public string? Unit { get; set; }              // part_Unit (enrich)
    public decimal? MinQuantity { get; set; }      // part_MinQuantity (enrich) — SL đặt tối thiểu
    public string? Remark { get; set; }

    /// <summary>QtyAppr — 🔴 SỐ LƯỢNG DUYỆT. **Mọi thành tiền tính theo cột này, KHÔNG theo `QtyOrd`**
    /// (xem công thức ở `RecalcOrderPartLine` trong Program.cs).</summary>
    public decimal? QtyAppr { get; set; }

    // --- khối giá: 3 cột NHẬP (UPBeforeDc · DiscountRate · VAT) + 5 cột DẪN XUẤT ---
    public decimal? UPBeforeDc { get; set; }       // đơn giá trước chiết khấu   (NHẬP)
    public decimal? DiscountRate { get; set; }     // % chiết khấu               (NHẬP)
    public decimal? VAT { get; set; }              // % VAT                      (NHẬP)
    public decimal? TPBeforeDc { get; set; }       // thành tiền trước CK        (dẫn xuất)
    public decimal? UPAfterDc { get; set; }        // đơn giá sau CK             (dẫn xuất)
    public decimal? TPAfterDc { get; set; }        // thành tiền sau CK          (dẫn xuất)
    public decimal? ValVAT { get; set; }           // tiền VAT                   (dẫn xuất)
    public decimal? TPAfterVAT { get; set; }       // tổng tiền sau VAT          (dẫn xuất)

    public string? OrderSuppierNo { get; set; }    // số đơn NCC (nguồn viết thiếu chữ "l")
    public string? TSTID { get; set; }

    // ===== 🔴 #307 HAI ĐƠN VỊ TRÊN CÙNG MỘT DÒNG (`Ser_Order_Part_Get`, `A.02.OrderPart.cs:1782`) =====
    // Nguồn trả **hai** cột đơn vị và chúng **khác nhau**:
    //   `part_Unit`        = đơn vị ĐẶT HÀNG   — TST: `ISNULL(tmeu.TSTUnit, part.Unit)` · OTHER: `part.Unit`
    //   `part_UnitStockIn` = đơn vị NHẬP KHO   — **LUÔN** `part.Unit` (của master), không bao giờ đổi
    // ⇒ đơn đặt TST tính bằng **đơn vị BÁN của hãng** (thùng/hộp), kho nhập bằng **đơn vị lẻ**.
    //   Port cũ chỉ có MỘT cột `Unit` ⇒ không phân biệt được, mọi số lượng bị hiểu cùng một đơn vị.
    public string? UnitStockIn { get; set; }

    /// <summary>
    /// 🔴 SL QUY ĐỔI ĐƠN VỊ BÁN. Nguồn: `OTHER` ⇒ **hằng 1.0**; `TST` ⇒ `ISNULL(tmeu.ExchangeRate, 1.0)`
    /// (tra `Ser_Mst_TSTExchangeUnit` theo `PartCode = TSTPartCode`).
    /// ⚠️ Khối `case` **KHÔNG có `else`** ⇒ loại đơn ngoài hai giá trị cho ra **NULL**, không phải 1.0.
    /// Lưu lại trên dòng để số liệu cũ không đổi khi master tỷ lệ quy đổi thay đổi về sau.
    /// </summary>
    public decimal? ExchangeRate { get; set; }

    /// <summary>
    /// Cột N — tổng SL đã nhập kho theo đơn này (chỉ phiếu nhập **Kết thúc**).
    /// 🔴 #312 ĐÍNH CHÍNH #307: hai cột này là **DẪN XUẤT**, nguồn KHÔNG lưu chúng trên dòng đơn đặt.
    /// `Ser_Order_Part_Get` dựng chúng trong temp `#tbl_Ser_Order_PartDtl_StockInDetail` bằng cách
    /// **SUM dòng phiếu NHẬP KHO** (`Ser_Inv_StockInDetail`) của các phiếu `Status = '3'`, gom theo
    /// (`OrderPartNo`, `PartID`). #307 thêm chúng làm cột LƯU + cho ghi qua DTO ⇒ giá trị chỉ là thứ
    /// client gửi lên, **không bao giờ phản ánh lượng đã nhập thật**.
    /// ⇒ Giữ cột để đọc dữ liệu cũ, nhưng endpoint nay **TÍNH LẠI** từ phiếu nhập (xem Program.cs #312).
    /// </summary>
    public decimal? TotalQuantityIn { get; set; }

    /// <summary>
    /// Cột L — `TotalQuantityIn` quy về ĐƠN VỊ ĐẶT.
    /// 🔴 #312: công thức của nguồn là **CHIA**: `TST ⇒ TotalQuantityIn / ExchangeRate` ·
    /// `OTHER ⇒ TotalQuantityIn` (không đổi). **Ngược chiều** với cột M (SL chưa về ĐV bán) vốn **NHÂN**
    /// tỷ lệ — hai phép ngược nhau trong CÙNG một màn, rất dễ port nhầm chiều.
    /// ⚠️ Khối `case` **không có `else`** ⇒ loại đơn lạ cho ra **NULL**.
    /// </summary>
    public decimal? TotalQuantityInExchangeRate { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    /// <summary>
    /// Trạng thái RIÊNG của TỪNG DÒNG đơn đặt (Ser_Order_PartDtl.ORDERPARTSTATUSDTL) —
    /// nguồn ghi độc lập với trạng thái header: `= Pending` khi tạo, `= Approved` khi duyệt.
    /// ⚠️ Nhờ cột này mà đơn đặt có thể duyệt/nhận TỪNG PHẦN (dòng này duyệt, dòng kia chưa);
    /// gộp vào trạng thái header là mất khả năng đó.
    /// P (Pending) → A (Approved) → F (Finished) · R (Rejected).
    /// </summary>
    public string OrderPartStatusDtl { get; set; } = "P";
}

/// <summary>Khiếu nại đơn đặt phụ tùng (Ser_OrderComplain — port 1:1 FrmSer_OrderComplain/FrmSer_OrderComplainMng, TCMotor DMSCarSv/TST):
/// KN 2 chiều. DMSStatus: P(Mới tạo)→A(Đã gửi). TSTStatus: ''→Processing(Chờ duyệt)→Pending(Đang xử lý)→Resolved(Đã xử lý).</summary>
public sealed class OrderComplain
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ComplainNo { get; set; } = "";
    public string OrderPartNo { get; set; } = "";       // đơn đặt PT liên quan
    public string? ComplainType { get; set; }
    public string? Content { get; set; }
    /// <summary>Trạng thái phía DMS/đại lý (TConst.DMSOrderComplainStatus): "P" mới tạo → "A" đã gửi.</summary>
    public string DMSStatus { get; set; } = "P";

    /// <summary>
    /// Trạng thái phía TST (TConst.TSTOrderComplainStatus) — ⚠️ mã lưu là SỐ NHẢY, không liên tục:
    /// "1" Chờ duyệt · "15" Đang xử lý · "21" KHÔNG chấp thuận · "31" CHẤP THUẬN.
    /// ⚠️ Nguồn set "1" NGAY KHI TẠO khiếu nại (cùng lúc với DMSStatus="P"), KHÔNG có trạng thái rỗng.
    /// 🔴 Kết cục PHÂN ĐÔI (21 vs 31) — port cũ gộp thành một "Resolved" nên mất kết quả nghiệp vụ.
    /// </summary>
    public string TSTStatus { get; set; } = "1";

    /// <summary>`TSTSolution` — phương án xử lý do phía NCC/TST nhập (client KHÔNG gửi lên).</summary>
    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;   // CreateDTime

    // ===== 🔴 #233: 16 cột nguồn `Ser_OrderComplain` mà port cũ THIẾU =====
    // Nguồn: `Entities/TST/Ser_OrderComplain.cs` + `Ser_OrderComplainService.Ser_OrderComplain_Save`
    // (Ser_OrderComplainService.cs:104 — 17 trường client GỬI LÊN).

    /// <summary>DealerCode — đại lý khiếu nại. Port cũ không có ⇒ không biết khiếu nại của ai.</summary>
    public string? DealerCode { get; set; }

    // --- vật tư bị khiếu nại (4 cột, đều BẮT BUỘC ở form) ---
    public string? PartCode { get; set; }        // Mã vật tư
    public string? VieName { get; set; }         // Tên vật tư
    public decimal? Quantity { get; set; }       // Số lượng — form bắt buộc LÀ SỐ và > 0
    public string? VINCode { get; set; }         // Số VIN

    /// <summary>RequestOrderNo — số yêu cầu giao hàng (BẮT BUỘC), khác `OrderPartNo` là đơn đặt hàng.</summary>
    public string? RequestOrderNo { get; set; }

    // --- phía NCC/TST: server ghi, client KHÔNG gửi (không nằm trong Ser_OrderComplain_Save) ---
    public string? TSTOrderComplainNo { get; set; }   // Số khiếu nại bên NCC
    public string? TSTEmployeeCode { get; set; }      // Nhân viên NCC xử lý

    // --- 🔴 KHỐI GIAO NHẬN + LẮP ĐẶT (7 cột) — port cũ thiếu TRỌN mảng nghiệp vụ này ---
    // ⚠️ Bất đối xứng của form (`checkForm`, FrmSer_OrderComplain_Detail.cs:466-493):
    //    `DeliveryDateTime` · `DeliveryBy` · `TransportUnit` **BẮT BUỘC**;
    //    `DeliveryLocation` · `ReceiveBy` · `AssembleDateTime` · `AssembleBy` **KHÔNG** bị kiểm. Giữ đúng.
    public DateTime? DeliveryDateTime { get; set; }   // Ngày giao nhận (bắt buộc)
    public string? DeliveryBy { get; set; }           // Người giao nhận (bắt buộc)
    public string? TransportUnit { get; set; }        // Đơn vị vận tải (bắt buộc)
    public string? DeliveryLocation { get; set; }     // Địa điểm giao hàng — tra `Mst_DeliveryLocation` (#232)
    public string? ReceiveBy { get; set; }            // Người nhận
    public DateTime? AssembleDateTime { get; set; }   // Ngày lắp đặt
    public string? AssembleBy { get; set; }           // Người lắp đặt

    // --- vết ghi ---
    public string? CreateBy { get; set; }             // CreateBy
    public DateTime? LogLUDTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Thanh toán nhà cung cấp (Ser_SupplierPayment — port 1:1 FrmSer_SupplierPayment, TCMotor DMSCarSv/TST):
/// thanh toán cho đơn đặt PT đã hoàn thành. SupplierPaymentStatus: P(Mới tạo)→A(Đã duyệt).</summary>
public sealed class SupplierPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentNo { get; set; } = "";
    public string SupplierCode { get; set; } = "";
    public string? OrderPartNo { get; set; }           // đơn đặt PT liên quan

    /// <summary>Đại lý lập phiếu (Ser_SupplierPayment.DEALERCODE) — nguồn lọc báo cáo theo cột này.</summary>
    public string? DealerCode { get; set; }

    /// <summary>Tổng tiền phiếu — cộng từ <see cref="SupplierPaymentLine"/> khi có chi tiết.</summary>
    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }
    public string Status { get; set; } = "P";          // P → A (SupplierPaymentStatus)
    public DateTime CreatedAt { get; set; } = DateTime.Now;   // CreateDTime
    public DateTime? ApprovedAt { get; set; }          // ApprDTime

    // ===== 🔴 #237: cột nguồn `Ser_SupplierPayment` mà port cũ THIẾU =====
    // Nguồn `Entities/TST/Ser_SupplierPayment.cs` (md5 0c9f1037) + chữ ký
    // `Ser_SupplierPayment_Save` (Ser_SupplierPaymentService.cs:140) — client gửi **8 trường**.

    /// <summary>SupplierID — 🔴 KHÁC `SupplierCode`; `_Save` gửi `SupplierID`.</summary>
    public string? SupplierID { get; set; }

    public string? Address { get; set; }        // địa chỉ NCC in trên phiếu
    public string? TSTRequestNo { get; set; }   // số yêu cầu phía TST

    /// <summary>PaymentType (`TConst.PaymentType`, Const.Main.cs:100): "PMT" Thanh toán ·
    /// "PMC" Cấn trừ · "PMA" Điều chỉnh. Port cũ KHÔNG có ⇒ ba loại phiếu bị gộp làm một.</summary>
    public string? PaymentType { get; set; }

    /// <summary>Description — form chặn **> 1000 ký tự** (FrmSer_SupplierPayment.cs:746).</summary>
    public string? Description { get; set; }

    // vết ghi/duyệt
    public string? PaymentBy { get; set; }
    public string? CreateBy { get; set; }
    public string? ApprBy { get; set; }
    public DateTime? LogLUDTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Dòng chi tiết thanh toán nhà cung cấp (Ser_SupplierPaymentDtl — TCMotor DMSCarSv).
/// MỘT phiếu thanh toán gồm NHIỀU dòng phụ tùng; tiền của phiếu là TỔNG các dòng, không nhập tay.
/// </summary>
public sealed class SupplierPaymentLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Số phiếu thanh toán (SUPPLIERPAYMENTNO) — khoá nối về header.</summary>
    public string PaymentNo { get; set; } = "";

    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }

    /// <summary>
    /// SỐ LƯỢNG THANH TOÁN (QTYPAY) — ⚠️ KHÁC số lượng xuất kho: một lần xuất có thể
    /// thanh toán làm nhiều đợt, nên nguồn tính tiền theo QtyPay chứ không theo Quantity.
    /// </summary>
    public decimal QtyPay { get; set; }

    public decimal Price { get; set; }

    /// <summary>Thuế suất theo PHẦN TRĂM (nguồn tính `VAT*0.01`).</summary>
    public decimal Vat { get; set; }

    /// <summary>Thành tiền dòng = QtyPay × Price × (1 + VAT%).
    /// #237: tương ứng cột nguồn `PriceAfterVAT` (giữ tên `Amount` để không vỡ dữ liệu cũ).</summary>
    public decimal Amount { get; set; }

    // ===== 🔴 #237: cột nguồn `Ser_SupplierPaymentDtl` mà port cũ THIẾU =====
    // Bộ dòng client gửi lên đọc từ `FrmSer_SupplierPayment.cs:702-718` (12 cột).
    public string? PartID { get; set; }
    public string? Unit { get; set; }

    /// <summary>🔴 `StockInID` / `StockInNo` — dòng thanh toán **gắn với MỘT LẦN NHẬP KHO cụ thể**.
    /// Thiếu cặp này thì không đối chiếu được phiếu trả tiền với lô hàng đã nhập.</summary>
    public string? StockInID { get; set; }
    public string? StockInNo { get; set; }

    public decimal? QtyInventory { get; set; }   // tồn tại thời điểm lập phiếu (tổng)
    public string? LocationID { get; set; }      // vị trí kho

    // ===== 🔴 #260: 3 cột nguồn `Ser_SupplierPaymentDtl` mà #237 còn THIẾU =====
    // `DbDefine.cs:3150-3152` — ba cột này nằm TÁCH RIÊNG dưới khối chính của lớp hằng nên dễ bỏ sót.

    /// <summary>
    /// 🔴 `InStockQuantity` — chú thích NGUYÊN VĂN của nguồn (DbDefine.cs:3152):
    /// "Tồn kho theo **vị trí**, dùng để check số lượng trả ko đc vượt quá".
    /// ⚠️ KHÁC <see cref="QtyInventory"/> (tồn TỔNG tại thời điểm lập phiếu). Guard "số lượng trả"
    ///    so với cột NÀY, không phải cột kia.
    /// </summary>
    public decimal? InStockQuantity { get; set; }

    public string? LocationCode { get; set; }    // enrich theo LocationID
    public string? LocationName { get; set; }

    /// <summary>SupplierPaymentDtlStatus — trạng thái RIÊNG của dòng (`TConst.SupplierPaymentStatus` P/A).</summary>
    public string SupplierPaymentDtlStatus { get; set; } = "P";

    public DateTime? LogLUDTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Yêu cầu báo giá phụ tùng (Req_PartPrice — port 1:1 FrmReq_PartPrice/Mng, TCMotor DMSCarSv/TST):
/// DMS xin TST báo giá PT. DMSStatus P→A→F; TSTStatus Pending(chờ)→Quoted(đã báo giá)→Finished.</summary>
public sealed class ReqPartPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqNo { get; set; } = "";
    /// <summary>
    /// Trạng thái phía DMS (TConst.DMSReqPartPriceStatus): "P" mới tạo → "A" đã gửi → "F" hoàn thiện,
    /// và **"R" TỪ CHỐI** — nhánh port cũ thiếu hẳn.
    /// </summary>
    public string DMSStatus { get; set; } = "P";

    /// <summary>
    /// Trạng thái phía TST (TConst.TSTReqPartPriceStatus) — ⚠️ mã lưu là SỐ NHẢY, **không có mã 3**:
    /// "1" Chờ duyệt · "2" Đã duyệt, chờ hoàn thiện · "4" Đã hoàn thiện.
    /// Nguồn set "1" ngay khi tạo (cùng lúc DMSStatus="P").
    /// </summary>
    public string TSTStatus { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;   // CreateDTime
    public DateTime? QuotedAt { get; set; }

    // ===== 🔴 #238: cột nguồn `Req_PartPrice` mà port cũ THIẾU =====
    // Nguồn `Entities/TST/Req_PartPrice.cs` (md5 35f57589) + `Req_PartPrice_Save`
    // (Req_PartPriceService.cs:254) — client gửi `ReqPartPriceNo` · `DealerCode` · `Description`.
    public string? DealerCode { get; set; }
    public string? Description { get; set; }

    /// <summary>TSTReqPartPriceID / TSTSentDate — định danh + ngày gửi sang phía TST (server ghi).</summary>
    public string? TSTReqPartPriceID { get; set; }
    public DateTime? TSTSentDate { get; set; }

    /// <summary>IsUpdatePrice — cờ "đã cập nhật giá vào bảng giá" (khác với "đã báo giá").</summary>
    public string? IsUpdatePrice { get; set; }

    // vết ghi theo TỪNG MỐC — nguồn tách riêng Create/Appr/Finish, không gộp một cặp LogLU.
    public string? CreateBy { get; set; }
    public DateTime? ApprDTime { get; set; }
    public string? ApprBy { get; set; }
    public DateTime? FinishDTime { get; set; }
    public string? FinishBy { get; set; }
    public DateTime? LUDTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng PT xin báo giá (Req_PartPriceDtl): mã PT + SL yêu cầu + giá TST báo (điền sau).</summary>
public sealed class ReqPartPriceLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string PartCode { get; set; } = "";         // ` DMSPartCode
    public string? PartName { get; set; }              // VieName
    public decimal ReqQty { get; set; } = 1;
    public decimal QuotedPrice { get; set; }           // TST điền (` TSTPrice)

    // ===== 🔴 #238: cột nguồn `Req_PartPriceDtl` mà port cũ THIẾU =====
    /// <summary>ReqPartPriceNo — nguồn nối dòng về header bằng SỐ PHIẾU (không phải Id).</summary>
    public string? ReqPartPriceNo { get; set; }

    /// <summary>🔴 `DMSPartCode` và `TSTPartCode` là HAI mã KHÁC NHAU: mã đại lý dùng và mã NCC trả về.
    /// Cả điểm của màn này là ÁNH XẠ hai mã đó — gộp một cột là mất mục đích nghiệp vụ.</summary>
    public string? TSTPartCode { get; set; }

    public string? DeliveryFormCode { get; set; }   // hình thức giao hàng của dòng
    public string? VINCode { get; set; }            // xin giá theo VIN cụ thể
    public DateTime? DateEffect { get; set; }       // ngày hiệu lực của giá NCC trả
    public string? Remark { get; set; }

    /// <summary>Trạng thái RIÊNG của dòng (`ReqPartPriceDtlStatus`).</summary>
    public string ReqPartPriceDtlStatus { get; set; } = "P";

    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Nhóm sửa chữa (Ser_GroupRepair — port 1:1 FrmGroupRepairCreate, TCMotor DMSCarSv/Admin):
/// nhóm tổ sửa chữa (Đồng/Sơn/Máy/Điện...). Engineer thuộc 1 nhóm.</summary>
public sealed class GroupRepair
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupRCode { get; set; } = "";
    public string GroupRName { get; set; } = "";
    public string? Note { get; set; }
    public string Status { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Kỹ thuật viên dịch vụ (Ser_Engineer — port 1:1 FrmEngineerCreate + FrmEmployeeCreate/Search, TCMotor DMSCarSv/Admin):
/// KTV thuộc 1 nhóm sửa chữa. RO service items tham chiếu KTV. FrmEmployeeCreate là màn nhập chi tiết hơn
/// trên CÙNG bảng Ser_Engineer (thêm loại nhân viên CVDV/KTV chung/KTV đồng-sơn/Khác + ngày làm việc).</summary>
public sealed class ServiceEngineer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string EngineerNo { get; set; } = "";
    public string EngineerName { get; set; } = "";
    public string? GroupRCode { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "1";
    /// <summary>🔴 #338 ENGINEERTYPE — vai trò KTV. Chú thích cũ ghi "1=CVDV, 2=…, 3=…, 4=Khác"
    /// là **suy đoán chưa kiểm**: nguồn dùng **mã CHỮ**, đủ **bảy** giá trị —
    /// `CVDV` (cố vấn dịch vụ) · `BDN` · `SCC` · `KTVD` (đồng) · `KTVS` (sơn) · `NVPT` (phụ tùng) · `KHAC`.
    /// Báo cáo KPI lọc thẳng theo các mã chữ này ⇒ dùng mã số thì mọi chỉ tiêu nhân sự = 0.
    /// Không chỗ nào trong MiniHTC rẽ nhánh theo giá trị cũ nên đổi chú thích là an toàn.</summary>
    public string? EngineerType { get; set; }
    /// <summary>🔴 #338 DEALERCODE — báo cáo KPI đếm KTV **theo từng đại lý**
    /// (`inner join Ser_Engineer se on md.DealerCode = se.DealerCode`). Thiếu cột này thì mọi
    /// đại lý sẽ dùng chung một con số KTV toàn hệ thống.</summary>
    public string? DealerCode { get; set; }
    public DateTime? StartWorkDate { get; set; }
    public DateTime? FinishWorkDate { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Chiến dịch dịch vụ/marketing (Ser_Campaign — port 1:1 FrmCampaignCreate, TCMotor DMSCarSv/Admin):
/// chiến dịch chăm sóc/khuyến mãi có thời hạn (StartDate ≤ FinishDate) + danh sách xe/khách liên hệ.</summary>
public sealed class Campaign
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    public string CamName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public string? Content { get; set; }
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Liên hệ trong chiến dịch (Ser_CamContact): xe/khách trong danh sách chiến dịch + trạng thái liên hệ.</summary>
public sealed class CampaignContact
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CampaignId { get; set; }
    public string? PlateNo { get; set; }
    public string? CusName { get; set; }
    public string? Address { get; set; }
    public string ContactStatus { get; set; } = "Pending";  // Pending → Contacted
    // GAP đã vá 2026-09-05: lưới FrmCamp_CustomerList (màn chọn KH vào chiến dịch) mang 13 cột,
    // bản port trước chỉ giữ 3 (PlateNo/CusName/Address) — mất sạch thông tin định danh & liên hệ.
    public string? CusID { get; set; }          // TblSerCustomer.CusID — định danh khách hàng
    public string? CarID { get; set; }          // TblSerCar.CarID — định danh xe
    public DateTime? DOB { get; set; }          // TblSerCustomer.DOB — ngày sinh (dùng cho CD sinh nhật)
    public string? TradeMarkCode { get; set; }  // TblSerCar.TradeMarkCode — hiệu xe
    public string? ModelName { get; set; }      // TblModel.ModelName — dòng xe
    public string? Mobile { get; set; }         // TblSerCustomer.Mobile — ĐT khách
    public string? Email { get; set; }          // TblSerCustomer.Email
    // Người liên hệ thay mặt khách (bandedGridColCont*) — nguồn lấy từ sourceRow, KHÔNG từ ô lưới
    public string? ContName { get; set; }       // TblSerCustomer.ContName
    public string? ContTel { get; set; }        // TblSerCustomer.ContTel
    public string? ContMobile { get; set; }     // TblSerCustomer.ContMobile
    public string? ContEmail { get; set; }      // TblSerCustomer.ContEmail
    // 2 cột của Ser_CamContact được thêm khi đưa KH vào chiến dịch
    public DateTime? ContactDate { get; set; }  // TblSer_CamContact.ContactDate — ngày đã liên hệ
    public string? Remark { get; set; }         // TblSer_CamContact.Remark — ghi chú liên hệ
}

/// <summary>Hóa đơn dịch vụ (Ser_Invoice — port 1:1 FrmInvoice, TCMotor DMSCarSv/Services):
/// hóa đơn thu tiền cho 1 RO. SubTotal(công+PT) + VAT − chiết khấu = TotalAmount. Draft→Paid (đẩy RO sang Paid).</summary>
public sealed class ServiceInvoice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceNo { get; set; } = "";
    public string RONo { get; set; } = "";
    public decimal SubTotal { get; set; }              // Σ tiền công + Σ (SL×đơn giá PT)
    public decimal VatPercent { get; set; } = 10;
    public decimal VatAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }           // SubTotal + VAT − chiết khấu
    public string? PaymentType { get; set; }           // Tiền mặt/Chuyển khoản/Thẻ
    public string Status { get; set; } = "Draft";      // Draft → Paid
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaidAt { get; set; }
    // GAP đã vá 2026-09-05: FrmInvoice tách RIÊNG 2 loại chiết khấu + có cụm tiền/điểm
    // mà bản port gộp hết vào DiscountAmount ⇒ mất khả năng đối soát chiết khấu hãng vs chiết khấu khác.
    public decimal AmountFromMC { get; set; }          // Ser_RO.AmountFromMC (txtAmountDiscount) — chiết khấu từ hãng
    public decimal AmountDiscountOther { get; set; }   // Ser_RO.AmountDiscountOther (txtAmountDiscountOther) — chiết khấu khác
    public decimal TotalBeforeTax { get; set; }        // txtTongTienTruocThue — tổng tiền trước thuế
    public decimal TotalAfterTax { get; set; }         // txtTongTienSauThue — tổng tiền sau thuế
    // Tích điểm hội viên hiển thị ngay trên hoá đơn
    public decimal PointTotal { get; set; }            // txtPointTotal — tổng điểm tích
    public string? CardTypeExpect { get; set; }        // txtCardTypeExpect — hạng thẻ dự kiến sau tích điểm
}

/// <summary>Lệnh đặt xe từ nhà máy (POCommand — port 1:1 FrmNewHMCOrder/FrmMngHMCOrder, TCMotor DMSales.Foton):
/// đơn đặt xe tải Foton lên hãng theo tháng. Draft(Nháp)→Sent(Đã gửi hãng).</summary>
public sealed class POCommand
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PoCmdCode { get; set; } = "";
    public string OrderMonth { get; set; } = "";       // YYYYMM tháng đặt hàng
    /// <summary>Tháng sản xuất (`Ord_POCommand.ProductionMonth`) — nguồn ghi khi tạo lệnh, port cũ thiếu.</summary>
    public string? ProductionMonth { get; set; }
    /// <summary>Tháng dự kiến về (`ExpectedMonth`) — nguồn ghi khi tạo lệnh, port cũ thiếu.</summary>
    public string? ExpectedMonth { get; set; }
    /// <summary>
    /// 🔴 Nguồn dùng **cờ `Ord_POCommand.FlagActive`** ("1" còn hiệu lực / "0" đã huỷ), **KHÔNG có cột
    /// trạng thái**: `grep "POCommandStatus|PoCmdStatus"` toàn hệ = **0 hit**.
    /// `OrderPOCommandCreate` gán `FlagActive = Flag.Active` (Biz.HTC.WH.cs:28478);
    /// `OrderPOCommandCancel` guard `Flag.Active` rồi gán `Flag.Inactive` (28636).
    /// ⚠️ `Draft → Sent` của port cũ là **trạng thái BỊA** — nguồn không có bước "gửi hãng".
    /// </summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>Người tạo lệnh (`CreatedBy`) — nguồn ghi khi tạo, port cũ thiếu.</summary>
    public string? CreatedBy { get; set; }
    /// <summary>⚠️ Giữ để đọc dữ liệu cũ, KHÔNG dùng làm điều kiện nghiệp vụ (xem <see cref="FlagActive"/>).</summary>
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
}

/// <summary>Dòng lệnh đặt (POCommandDetail): spec + màu + SL + cảng + nhà máy.</summary>
public sealed class POCommandLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PoCmdId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? SpecDesc { get; set; }
    public string? ColorCode { get; set; }
    public string? PortCode { get; set; }              // cảng nhận
    public string? PlantCode { get; set; }             // nhà máy sản xuất
    /// <summary>Số LC tạm (`Ord_POCommandDetail.LCTemp`) — cột đầu tiên nguồn ghi cho mỗi dòng, port cũ thiếu.</summary>
    public string? LCTemp { get; set; }
    /// <summary>Mã model (`ModelCode`) — nguồn ghi riêng, tách khỏi `SpecCode`; port cũ thiếu.</summary>
    public string? ModelCode { get; set; }
    public int Quantity { get; set; } = 1;
}

/// <summary>Proforma Invoice nhập xe (Pi — port 1:1 FrmNewPI/FrmMngPI, TCMotor DMSales.Foton):
/// PI lô xe nhập từ hãng. ExpectedMonth = ProductionMonth + 1 tháng (tự tính). Draft→Confirmed.</summary>
public sealed class Pi
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PiNo { get; set; } = "";
    public string? RefNo { get; set; }
    public DateTime ProductionMonth { get; set; }      // tháng sản xuất
    public DateTime? OrderMonth { get; set; }          // tháng đặt
    public DateTime ExpectedMonth { get; set; }        // = ProductionMonth + 1 tháng
    public string Status { get; set; } = "Draft";      // Draft → Confirmed
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng PI (PiDetail): spec/model/màu + cảng/nhà máy + WO + SL + đơn giá.</summary>
public sealed class PiLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PiId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? PortCode { get; set; }
    public string? PlantCode { get; set; }
    public string? WorkOrderNo { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
}

/// <summary>Thư tín dụng nhập khẩu (LC — port 1:1 FrmNewLC/FrmMngLC, TCMotor DMSales.Foton):
/// LC mở tại ngân hàng cho 1 hợp đồng nhập xe. Open(mở)→Closed(tất toán); cờ hết hạn.</summary>
public sealed class LetterOfCredit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string LCNo { get; set; } = "";
    public string ContractNo { get; set; } = "";       // số hợp đồng
    public string BankName { get; set; } = "";          // ngân hàng mở LC
    public decimal Amount { get; set; }
    public DateTime? OpenDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = "Open";        // Open → Closed
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tờ khai hải quan (Tkhq — port 1:1 FrmNewTKHQ/FrmMngTKHQ, TCMotor DMSales.Foton):
/// tờ khai HQ cho lô xe nhập theo hợp đồng, mở tại 1 cảng, gồm nhiều packing list. Open→Cleared(thông quan).</summary>
public sealed class Tkhq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DeclarationNo { get; set; } = "";     // số TKHQ
    public string ContractNo { get; set; } = "";
    public string? PortCode { get; set; }
    public DateTime? OpenDate { get; set; }             // ngày mở tờ khai
    public string? Remark { get; set; }
    public string Status { get; set; } = "Open";        // Open → Cleared (thông quan)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ClearedAt { get; set; }
}

/// <summary>Packing list trong TKHQ (Tkhq_PL): số PL + ngày tàu chạy cuối.</summary>
public sealed class TkhqPL
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TkhqId { get; set; }
    public string PackingListNo { get; set; } = "";
    public DateTime? ShippingDateEnd { get; set; }
}

/// <summary>Lệnh giao xe cho đại lý (DeliveryOrder — port 1:1 FrmNewDO/FrmMngDO, TCMotor DMSales.Foton):
/// giao lô xe từ kho tới đại lý. Draft(Nháp)→Delivered(Đã giao).</summary>
/// <summary>
/// Đại lý thuộc một ĐỢT tự sinh lệnh giao (`Auto_Car_DeliveryOrder_Dealer`) — nguồn
/// `DMS40_Car_DeliveryOrder_CreateAuto_New20190125` duyệt bảng này để biết cần sinh lệnh cho những đại lý nào.
/// </summary>
public sealed class AutoDoDealer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string D4CDONo { get; set; } = "";
    public string DealerCode { get; set; } = "";
}

/// <summary>
/// Xe thuộc một ĐỢT tự sinh lệnh giao (`Auto_Car_DeliveryOrder_Car`), khoá `(D4CDONo, DealerCode, CarId)`.
/// ⚠️ Bảng nguồn chỉ có `CarId`; `VIN`/`StorageCode` được nguồn lấy bằng join `Car_Car` → `Car_VIN`.
/// MiniHTC giữ sẵn `VIN` để tra kho hiện tại, có ghi rõ nguồn gốc.
/// </summary>
public sealed class AutoDoCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string D4CDONo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string CarId { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? StorageCode { get; set; }
}

public sealed class DeliveryOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DoNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG cột `Car_DeliveryOrder.DeliveryOrderStatus` của nguồn (`TConst.Stage`):
    /// **"P" Chờ duyệt · "A1" Duyệt cấp 1 · "A2" Duyệt cấp 2 · "R" Từ chối**.
    /// Toàn nguồn chỉ có **3 điểm ghi** cột này (Biz.HTC.WH.cs:49868/50413/50619) và **không điểm nào**
    /// đặt trạng thái "đã giao" ⇒ `Delivered` của port cũ là **trạng thái BỊA**; việc giao xe thực tế
    /// nằm ở màn Biên bản giao xe (`BizHTC.Storage.DlvMinutes`), ghi vào `Car_DeliveryOrderDetail`.
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliveredAt { get; set; }
    // Duyệt lệnh giao (FrmApproveDO) — duyệt 2 cấp; mỗi cấp ghi CẢ ngày LẪN người duyệt.
    public DateTime? Approved1At { get; set; }
    public DateTime? Approved2At { get; set; }
    /// <summary>Người duyệt cấp 1 (`Car_DeliveryOrder.ApprovedBy1`) — port cũ chỉ lưu thời điểm.</summary>
    public string? ApprovedBy1 { get; set; }
    /// <summary>Người duyệt cấp 2 (`ApprovedBy2`).</summary>
    public string? ApprovedBy2 { get; set; }
    public string? RejectReason { get; set; }
    public DateTime? RejectedAt { get; set; }

    // ===== #175 parity `DMS40_Car_DeliveryOrder_CreateAuto_New20190125` (DMS40/zTemp.0.31.Car.cs:4601, csproj 280) =====
    /// <summary>Địa chỉ giao — nguồn lấy từ `Mst_Dealer.DealerAddress01`, KHÔNG nhập tay.</summary>
    public string? DeliveryAddress { get; set; }
    /// <summary>Đơn vị vận chuyển ghi trên lệnh giao (`TransportCompanyName/PhoneNo/FaxNo`).</summary>
    public string? TransportCompanyName { get; set; }
    public string? TransportCompanyPhoneNo { get; set; }
    public string? TransportCompanyFaxNo { get; set; }
    /// <summary>🔴 Số ĐỢT tự sinh (`D4CDONo`) và loại luật (`D4CDOType`: "RULE1"/"RULE2") đã sinh ra lệnh này —
    /// null nghĩa là lệnh lập tay. Nhờ hai cột này mới truy được lệnh giao nào thuộc đợt nào.</summary>
    public string? D4CDONo { get; set; }
    public string? D4CDOType { get; set; }
}

/// <summary>Dòng xe trong DO (DoDetail): VIN + model + màu + kho + ngày giao dự kiến.</summary>
public sealed class DeliveryOrderCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DoId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? StorageCode { get; set; }
    public DateTime? DeliveryExpectDate { get; set; }
    // Sửa lệnh giao (FrmEditDO): ngày giao thực tế bắt đầu/kết thúc + ngày xuất kho
    public DateTime? DeliveryStartDate { get; set; }
    public DateTime? DeliveryEndDate { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    /// <summary>Ghi chú giao xe theo DÒNG (`Car_DeliveryOrderDetail.DeliveryRemark`) — nguồn cho sửa cùng
    /// `DeliveryOutDate` qua `CarDeliveryOrderDetailUpdate_New20181119`.</summary>
    public string? DeliveryRemark { get; set; }
    /// <summary>Trạng thái xác nhận của DÒNG xe (`ConfirmStatus`) — trục RIÊNG, khác trạng thái của lệnh:
    /// nguồn guard sửa dòng theo `"P,A"`, và khi xoá dòng thì `A`/`F` phải kiểm thêm hồ sơ xe.</summary>
    public string ConfirmStatus { get; set; } = "P";

    // ===== #170 parity `Sto_DlvMinutes_UpdateDlvEndDate_New20181115` (BizHTC.Storage.DlvMinutes.cs:9329) =====
    /// <summary>Ngày/người XÁC NHẬN giao xong (`ConfirmDate`/`ConfirmBy`) — nguồn ghi cùng `ConfirmStatus = "F"`.</summary>
    public DateTime? ConfirmDate { get; set; }
    public string? ConfirmBy { get; set; }
    /// <summary>Nhật ký sửa cuối của DÒNG lệnh xuất xe.</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    /// <summary>#175 — `Car_DeliveryOrderDetail.CarId`: nguồn dựng dòng chi tiết bằng `CarId` (join `Car_Car`),
    /// VIN chỉ là cột đi kèm. Không có cột này thì không nối được về xe theo đúng khoá của nguồn.</summary>
    public string? CarId { get; set; }
}

/// <summary>Đề nghị làm hồ sơ đăng ký xe (Car_DocReq — port 1:1 FrmNewDocReq/FrmMngDocReq, TCMotor DMSales.Foton):
/// đề nghị làm hồ sơ đăng ký cho lô xe đã giao. Draft→Submitted(đã nộp)→Done(hoàn tất).</summary>
public sealed class DocReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DocReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái ĐỀ NGHỊ (`Car_DocReqList.DRListStatus`) theo `TConst.Stage`:
    /// **"P" chờ duyệt · "A1" duyệt cấp 1 · "A2" duyệt cấp 2 · "F" hoàn tất**.
    /// ⚠️ Port cũ `Draft/Submitted/Done` là **tên tự đặt**, và quan trọng hơn: port cũ đặt trạng thái
    /// **CHỈ Ở HEADER** trong khi nguồn có **HAI TẦNG** — header `DRListStatus` và **dòng `DRDtlStatus`**;
    /// duyệt cấp 2 / từ chối / huỷ của nguồn đều thao tác **theo TỪNG XE**, không theo cả đề nghị.
    /// </summary>
    public string Status { get; set; } = "P";
    /// <summary>
    /// 🔴 Loại đề nghị (`Car_DocReqList.TypeCRR`, `TConst.CarDocReqType`):
    /// **"NORMAL" thường · "SPECIAL" đặc biệt · "DEALER" đại lý tạo · "DEALERTCG" đại lý TCG**.
    /// Quyết định LUỒNG DUYỆT: loại **NORMAL duyệt 1 lần là nhảy thẳng P→"A2"** (ghi luôn người/ngày
    /// duyệt cấp 2), các loại khác chỉ lên "A1" rồi phải duyệt cấp 2 theo từng xe.
    /// Nguồn có **3 đường tạo riêng**: `CarDocReqCreateHTC` · `CreateDealer` · `TCGCreateDealer`.
    /// </summary>
    public string TypeCRR { get; set; } = "NORMAL";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? DoneAt { get; set; }
    public DateTime? ApprovedDate1 { get; set; }
    public string? ApprovedBy1 { get; set; }
    public DateTime? ApprovedDate2 { get; set; }
    public string? ApprovedBy2 { get; set; }
    /// <summary>
    /// Người TẠO đề nghị (`Car_DocReqList.CreatedBy`) — không chỉ để hiển thị: khi huỷ đề nghị, nguồn
    /// lấy đại lý **của người tạo** rồi mới kiểm quyền truy cập (`CarDocReqListCancel`, Biz.HTC.WH.cs:84977-84996):
    /// *"Đại lý được Hủy đề nghị do Đại lý tạo ra. Không được hủy đề nghị do HTC tạo hộ"*.
    /// </summary>
    public string? CreatedBy { get; set; }
    /// <summary>Ngày huỷ đề nghị (`Car_DocReqList.CancelDate`).</summary>
    public DateTime? CancelDate { get; set; }
    /// <summary>Người huỷ đề nghị (`Car_DocReqList.CancelBy`).</summary>
    public string? CancelBy { get; set; }
}

/// <summary>Dòng xe làm hồ sơ (Car_DocReqDtl): VIN + model + màu + số máy + tiền.</summary>
public sealed class DocReqCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DocReqId { get; set; }
    public string Vin { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? EngineNo { get; set; }
    public decimal AmountTotal { get; set; }
    /// <summary>
    /// 🔴 Trạng thái của **TỪNG XE** trong đề nghị (`Car_DocReqDtl.DRDtlStatus`) — trục mà port cũ THIẾU HẲN.
    /// "P" chờ · "A1" theo header · "A2" đã duyệt cấp 2 · "R" từ chối · "C" huỷ.
    /// Guard nguồn: duyệt cấp 2 chỉ từ **"A1"**; **từ chối chỉ từ "A2"** (từ chối SAU khi đã duyệt cấp 2);
    /// huỷ từ **"A1" hoặc "A2"**.
    /// </summary>
    public string DRDtlStatus { get; set; } = "P";
    /// <summary>Ngày/người duyệt cấp 1 của DÒNG — chỉ luồng **TCG** dùng: `CarDocReqTCGDtlApprove2`
    /// ghi CẢ BỐN cột duyệt cùng lúc vì duyệt một lần là qua cả hai cấp.</summary>
    public DateTime? ApprovedDate1 { get; set; }
    public string? ApprovedBy1 { get; set; }
    public DateTime? ApprovedDate2 { get; set; }
    public string? ApprovedBy2 { get; set; }
    public DateTime? RejectDate { get; set; }
    public string? RejectBy { get; set; }
    /// <summary>Ghi chú khi từ chối/huỷ dòng (`Car_DocReqDtl.Remark`).</summary>
    public string? Remark { get; set; }
    public DateTime? LetterRepresentationDate { get; set; }  // ngày tờ trình — port FrmUpdateDocReq
    public string? LetterRepresentationNo { get; set; }      // số tờ trình
    public int? LoanSupportDay { get; set; }                 // số ngày hỗ trợ vay vốn
}

/// <summary>Hợp đồng ngoại (CO) — port 1:1 FrmNewCO/FrmMngCO (DMSales.Foton). Gom nhiều dòng LC_Temp của PI vào 1 số hợp đồng ngoại.</summary>
public sealed class ForeignContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractNo { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public sealed class ForeignContractLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ContractId { get; set; }
    public string RefNo { get; set; } = "";
    public string LcTemp { get; set; } = "";
}

/// <summary>Đề nghị giấy tờ xe (DR / CDR) — port 1:1 FrmNewDR/FrmMngDR (DMSales.Foton). Yêu cầu làm giấy tờ cho lô xe, giao tới người/địa chỉ nhận.</summary>
public sealed class CarDocRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RequestNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string ReceivedPerson { get; set; } = "";
    public string ReceivedAddress { get; set; } = "";
    public string Status { get; set; } = "Draft"; // Draft → Done(duyệt) / Rejected(từ chối)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
    public string? RejectReason { get; set; }     // FrmDRApproved — từ chối
    public DateTime? RejectedAt { get; set; }
}
public sealed class CarDocRequestCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RequestId { get; set; }
    public string CarId { get; set; } = "";   // VIN/CarId
    public string? Remark { get; set; }
    public DateTime? DeliveryStartDate { get; set; }

    // ===== #161 parity `RD_ReqInvoiceCreate_New20240617` =====
    /// <summary>
    /// 🔴 Loại yêu cầu hồ sơ xe (`Car_DocReqDtl.TypeCRR`, `TConst.CarDocReqType`):
    /// **"NORMAL" · "SPECIAL" · "DEALER" · "DEALERTCG"**.
    /// Bản 2024 chỉ chấp nhận **NORMAL** hoặc **DEALER** khi tạo đề nghị giao hồ sơ
    /// (`InvalidTypeCRR`), và nếu là **DEALER** thì `TypeRDReqIv` của dòng bắt buộc phải là **DEALER**.
    /// (Tên property đặt `CarDocReqTypeCRR` để không lẫn với các cột TypeCRR khác trong hệ.)
    /// </summary>
    public string? CarDocReqTypeCRR { get; set; }

    // ===== #209 parity `CarDocReqDtlReject_New20181119` (DataWH/Biz.HTC.WH.cs:83426) =====
    // 🔴 Nguồn thao tác ở mức **DÒNG** (`Car_DocReqDtl`), không phải header: cả họ lệnh đều là
    //    `...DtlApprove2` / `...DtlCancel` / `...DtlReject` / `...DtlDelete`.
    /// <summary>Trạng thái của DÒNG (`DRDtlStatus`) — trục riêng, khác trạng thái đề nghị (`DRListStatus`).</summary>
    public string? DRDtlStatus { get; set; }
    public DateTime? RejectDate { get; set; }
    public string? RejectBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Packing List (PL) — port 1:1 FrmNewPL/FrmMngPL. Danh sách đóng gói lô xe lên tàu theo LC, cảng,
/// ngày lên tàu / đến cảng.
/// 🔴 **#123 đối chiếu với bảng nguồn `CT_PackingList`** (2010.HTC `Biz.HTC.WH.cs:34698`,
/// hàm `ContractPackingListCreate_New20190923`; hàm duyệt `ContractPackingListApproved_New20181115`
/// ở `BizHTC.Contract.cs:396`). Cột nguồn ↔ cột port cũ lệch tên, **giữ tên port cũ để không phá API**
/// nhưng ghi rõ tại đây: `PackingListNo`→`PLNo`, `LCNo`→`LcNo`, `CreatedDate`→`CreatedAt`.
/// GAP đã vá ở #123: bổ sung `ShippingDateEnd` và `PLStatus`.
/// 🔴 `ShippingDateEnd` (ngày đến cảng THỰC TẾ) khi tạo được nguồn **gán BẰNG**
/// `ShippingDateEndExpected` (dòng 34688-34689) — không để trống chờ cập nhật sau.
/// 🔴 `PLStatus` = `TConst.Stage.Finished` (**"F"**) **ngay khi tạo**. Nhánh phân biệt theo
/// `PLType != HTMV` (đặt "P") **đã bị COMMENT** ở nguồn (34691-34693) ⇒ hiện mọi PL đều "F" ngay.
/// Đây là dấu vết đổi nghiệp vụ, giữ nguyên hành vi hiện hành.
/// `PLType` theo `TConst.PLType` (`Const.Main.cs:877`): **HTMV · HMC** (HMI/CNTCG đã bị comment).
/// </summary>
/// <summary>
/// Mốc theo dõi vòng đời XE theo VIN (`VIN_MyStatus`) — nguồn
/// `ContractPackingListCreate_New20190923` (DataWH/Biz.HTC.WH.cs:33899, csproj 272) ghi tại 35533.
/// 🔴 TWIN: `_Create`/`_CreateAuto` bản **20190923 CHỈ có ở WS 64-bit**; WS 32-bit vẫn `_New20181119`.
///
/// Nguồn tạo dòng NGAY khi lập packing list, với `MapDateTime` và `DeliveryOutDate` **để NULL** —
/// hai mốc này được điền ở các bước sau của vòng đời (map VIN, xuất kho). Tức đây là **bảng mốc rỗng
/// dựng sẵn**, không phải bảng ghi khi sự kiện xảy ra.
/// </summary>
public sealed class VinMyStatus
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    /// <summary>Mốc map VIN (`MapDateTime`) — nguồn để NULL lúc tạo.</summary>
    public DateTime? MapDateTime { get; set; }
    /// <summary>Mốc xuất kho (`DeliveryOutDate`) — nguồn để NULL lúc tạo.</summary>
    public DateTime? DeliveryOutDate { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class PackingList
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Cột nguồn: `PackingListNo`.</summary>
    public string PLNo { get; set; } = "";
    /// <summary>Cột nguồn: `LCNo`.</summary>
    public string LcNo { get; set; } = "";
    public string? PortCode { get; set; }
    /// <summary>HTMV | HMC (TConst.PLType).</summary>
    public string? PLType { get; set; }
    public DateTime ShippingDateStart { get; set; }        // ngày lên tàu
    public DateTime ShippingDateEndExpected { get; set; }  // ngày DK đến cảng
    /// <summary>Ngày đến cảng THỰC TẾ — nguồn gán bằng ngày dự kiến khi tạo.</summary>
    public DateTime? ShippingDateEnd { get; set; }
    /// <summary>"F" ngay khi tạo (nhánh đặt "P" theo PLType đã bị comment ở nguồn).</summary>
    public string PLStatus { get; set; } = "F";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
/// <summary>
/// 🔴 LỊCH SỬ DI CHUYỂN KHO của xe (`Sto_StorageTransaction` — 2010.HTC ERP.V15.DataWH).
/// Port cũ THIẾU HOÀN TOÀN: có kho, có packing list, có VIN nhưng **không có vết xe đã nằm kho nào,
/// từ ngày nào tới ngày nào** ⇒ không dựng lại được lịch sử lưu kho và không tính được phí lưu kho.
/// </summary>
public sealed class StorageTransaction
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Vin { get; set; } = "";

    /// <summary>Số chứng từ phát sinh giao dịch (RefNo) — vd số packing list.</summary>
    public string RefNo { get; set; } = "";

    /// <summary>Loại chứng từ (`TConst.Sto_StorageTransaction_RefType`): "PL" packing list · "BBGN" biên bản giao nhận.</summary>
    public string RefType { get; set; } = "PL";

    /// <summary>Kho ĐI (StorageCode) — nguồn BẮT BUỘC.</summary>
    public string StorageCode { get; set; } = "";

    /// <summary>Kho ĐẾN (StorageCodeTo) — nguồn KHÔNG bắt buộc (guard đã bị comment): còn trống = xe chưa xuất kho.</summary>
    public string? StorageCodeTo { get; set; }

    /// <summary>Thời điểm vào kho (DTimeFrom) — nguồn BẮT BUỘC.</summary>
    public DateTime DTimeFrom { get; set; }

    /// <summary>Thời điểm rời kho (DTimeTo) — nguồn KHÔNG bắt buộc (guard đã bị comment).</summary>
    public DateTime? DTimeTo { get; set; }

    /// <summary>
    /// 🔴 CỜ NHẬP-XUẤT TRONG NGÀY (FlagInDay) — bật khi xe rời kho trước ĐÚNG TRONG NGÀY đã vào kho này.
    /// Lý do ghi ngay trong nguồn: *"nếu nhập xuất trong ngày thì CHỈ tính phí lưu kho của ngày hôm đó
    /// cho KHO XUẤT thôi"* ⇒ đây là **cờ ảnh hưởng tiền**, không phải cờ thống kê.
    /// </summary>
    public string FlagInDay { get; set; } = "0";

    public string? Remark { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDTime { get; set; } = DateTime.Now;
}

public sealed class PackingListVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PLId { get; set; }
    public string Vin { get; set; } = "";
    public string? CrateType { get; set; }   // loại thùng (LoaiThung)
}

/// <summary>Chi tiết tờ khai hải quan (CT_TKHQ) — port 1:1 FrmNewCT_TKHQ (DMSales.Foton). Tờ khai HQ khai trực tiếp lô VIN (khác Tkhq theo packing-list).</summary>
public sealed class CtTkhq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DeclarationNo { get; set; } = "";
    public DateTime OpenDate { get; set; }       // ngày mở tờ khai
    public string? PortCode { get; set; }
    public string? Remark { get; set; }
    public DateTime? TaxPaymentDate { get; set; } // ngày nộp thuế — port FrmMngCT_TKHQ, PHẢI >= OpenDate
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public sealed class CtTkhqVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CtTkhqId { get; set; }
    public string Vin { get; set; } = "";
}

/// <summary>Đơn đặt hàng (Sales Order — So) — port 1:1 FrmOrder (DMSales.Foton). Đại lý gửi đơn đặt xe: loại kế hoạch/ngoài KH, hình thức thanh toán, dòng model/số lượng.</summary>
public sealed class SalesOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SoCode { get; set; } = "";
    public string OrderType { get; set; } = "Plan";     // Plan (kế hoạch) / UnPlan (ngoài KH)
    public string? PayType { get; set; }                 // VONDAILY / BAOLANH / LC
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái theo ĐÚNG `TConst.Stage` cua nguon (`Const.Main.cs:113-129`), cột `Ord_SalesOrder.SOStatus`:
    /// **"P" Chờ duyệt · "A1" Duyệt cấp 1 · "A2" Duyệt cấp 2 · "C" Huỷ · "R" Từ chối**.
    /// ⚠️ Port cũ `Draft → Sent → …`: **cả "Draft" lẫn "Sent" đều là trạng thái BỊA** — nguồn
    /// (`OrderSOCreate_New20181119`, Biz.HTC.WH.cs:24581) tạo đơn là **"P" ngay**, không có bước gửi.
    /// Và port cũ **thiếu hẳn "C" Huỷ** (`OrderSOCancel_New20181119`) — nguồn phân biệt Huỷ với Từ chối.
    /// </summary>
    public string Status { get; set; } = "P";
    /// <summary>Chính sách bán áp cho đơn khi duyệt cấp 1 (`Ord_SalesOrder.SPCode`) — nguồn ghi ở Approve1.</summary>
    public string? SPCode { get; set; }
    /// <summary>Người duyệt cấp 1 (`ApprovedBy1`) — nguồn ghi kèm `ApprovedDate1`; port cũ chỉ có thời điểm.</summary>
    public string? ApprovedBy1 { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    // Duyệt (FrmOrderApprove) — cấp 1 nhập chính sách bán/tháng dự kiến/tháng SX/ngày giao; cấp 2 duyệt cuối
    public string? SalesPolicy { get; set; }
    public DateTime? ExpectedMonth { get; set; }
    public DateTime? ProductionMonth { get; set; }
    public DateTime? LatestDeliveryDate { get; set; }
    public DateTime? Approved1At { get; set; }
    public DateTime? Approved2At { get; set; }
    public string? RejectReason { get; set; }
    public DateTime? RejectedAt { get; set; }
    // Cập nhật ngày mốc SO (FrmMng_SO_Approved_Date): ngày duyệt / hết hạn nghĩa vụ cọc / hết hạn BL / đến hạn giao xe
    public DateTime? ApprovedDate { get; set; }
    public DateTime? DepositDutyEndDate { get; set; }
    public DateTime? GrtEndDate { get; set; }
    public DateTime? CarDueDate { get; set; }
    public decimal PenalizeActual { get; set; }   // tiền phạt trả chậm thực tế (FrmUpdatePenaltyPmtDelayReal)
}
/// <summary>Lượt duyệt tự động đơn hàng DMS40 (D4OSORA — port 1:1 FrmDuyetTuDongDonHang, 2010.HTC/Sales/Upgrade):
/// chọn luật (Rule1/Rule2/Rule2A/Rule3/RuleCancel) rồi chạy 1 lượt duyệt/hủy hàng loạt SO đang chờ (Status=Sent).
/// RuleCancel → chuyển các SO Sent thành Rejected; các luật khác → duyệt thẳng lên Approved2 (bỏ qua Approved1, khớp "duyệt tự động").</summary>
public sealed class Dms40SoRootApproval
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ApprovalNo { get; set; } = "";
    public string RuleType { get; set; } = "";
    public int AffectedCount { get; set; }
    public DateTime RunAt { get; set; } = DateTime.Now;
}

public sealed class SalesOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SalesOrderId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ContractType { get; set; }
    public string? YearProduction { get; set; }
    public int RequestedQuantity { get; set; }
    public DateTime? RequestedDate { get; set; }
    public decimal UnitPrice { get; set; }
    public string? RemarkDL { get; set; }
    /// <summary>SL duyệt cấp 1 — nguồn lấy TỪ INPUT người duyệt (chỉ guard `>= 0`), cho phép duyệt
    /// một phần hoặc duyệt 0. Port cũ gán cứng `= RequestedQuantity` ⇒ mất khả năng duyệt một phần.</summary>
    public int? ApprovedQuantity { get; set; }
    /// <summary>Ngày giao duyệt theo TỪNG DÒNG, lấy từ input người duyệt (port cũ gán = ExpectedMonth của header).</summary>
    public DateTime? ApprovedDate { get; set; }
    /// <summary>🔴 Màu xe — **một phần KHOÁ đối chiếu dòng** của nguồn:
    /// `|SOCode||SpecCode||ModelCode||ColorCode|` (Biz.HTC.WH.cs:25417). Port cũ thiếu ⇒ khoá hẹp hơn nguồn.</summary>
    public string? ColorCode { get; set; }
    /// <summary>Đơn giá duyệt (`UnitPriceInit`) — nguồn bắt buộc **>= 1.0**, không phải chỉ > 0.</summary>
    public decimal? UnitPriceInit { get; set; }
    /// <summary>Hạng ưu tiên map VIN — nguồn gán **cứng 5.0**, cố tình KHÔNG lấy input (`MapVINRanking = 5.0;`).</summary>
    public decimal? MapVINRanking { get; set; }
    /// <summary>Ghi chú của người duyệt cấp 1 theo dòng (`Ord_SalesOrderDetail.Remark`).</summary>
    public string? Remark { get; set; }

    /// <summary>#166 — Xe đã map vào dòng đơn bán (`Ord_SalesOrderDetail.CarId`). Nguồn
    /// `DMS40_CT_DealerContract_SaveX` tra dòng SO **theo CarId** để lấy `ApprovedDate`, rồi dùng ngày đó
    /// làm **mốc hiệu lực** khi tìm điều khoản thanh toán của xe. Không có cột này thì không lập được hợp đồng đại lý.</summary>
    public string? CarId { get; set; }
}

/// <summary>Giao dịch bán lẻ của đại lý (DealerDeal) — port 1:1 FrmNewDeal/FrmMngDeal (DMSales.Foton/SalesDealer). Đại lý bán xe cho khách: 3 vai trò KH (mua/lái/đứng tên), kiểu bán lẻ, cờ PDI.</summary>
public sealed class DealerDeal
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string? DealNoUser { get; set; }            // số HĐ bán lẻ user
    public string DealerCode { get; set; } = "";
    public string CustomerCodeBuyer { get; set; } = "";  // người mua
    public string? CustomerCodeDriver { get; set; }      // người lái
    public string? CustomerCodeHolder { get; set; }      // người đứng tên
    public string? DlrContractNo { get; set; }
    public string? BankCode { get; set; }                // mã ngân hàng tài trợ (Support sửa)
    public string? CtmCareFlag { get; set; }             // cờ kiểm chứng CSKH (EditDeal_KiemChung)
    /// <summary>
    /// 🔴 #279 `FlagInitDeal` — cờ giao dịch KHỞI TẠO. Báo cáo SSI loại các dòng này
    /// (`and dd.FlagInitDeal = '0'`, `BizHTC.DealerSales.cs:6601`).
    /// ⚠️ `WholesaleDeal` đã có cột cùng tên từ trước — **khác bảng, khác nghiệp vụ**; grep thấy tên cột
    /// "đã có" mà kết luận không cần thêm là bẫy (xem #277 với `ROType`).
    /// </summary>
    public string? FlagInitDeal { get; set; }

    // ===== #196 parity `OSHCC_DLS_Deal_UpdCtmCareFlagX_New20260805` (HCC/BizHTC.HCC.cs:3654, CHỈ có trên máy 150) =====
    /// <summary>Mốc DUYỆT kiểm chứng — nguồn ghi cùng lúc 3 cột này với `CtmCareFlag`.</summary>
    public DateTime? CtmCareUpdDate { get; set; }
    public string? CtmCareUpdBy { get; set; }
    public string? CtmCareRemark { get; set; }
    public string SalesType { get; set; } = "";          // kiểu bán lẻ
    public string FlagPDI { get; set; } = "1";           // 1 = có PDI, 0 = không
    public string? ReasonNotPDI { get; set; }
    public DateTime DealDate { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // FrmNewDealToDealer — chuyển xe sang đại lý khác: buyer là 1 đại lý, SalesType F7
    public string? DealerCodeBuyer { get; set; }
    public string? SalesManCode { get; set; }
}
public sealed class DealerDealDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DealId { get; set; }
    public string CarId { get; set; } = "";              // VIN/CarID
    public string? CusInvoiceNo { get; set; }            // số hóa đơn khách
    public DateTime? CusInvoiceDate { get; set; }
    public string? PlateNo { get; set; }                 // biển số xe (EditDeal sửa)
    public decimal PriceAFVAT { get; set; }              // giá sau VAT
    /// <summary>
    /// Giá bán của dòng (`DLS_DealDetail.Price`) — cột RIÊNG, khác `PriceAFVAT`.
    /// `Dls_DealDetail_UpdatePrice` (Biz.HTC.WH.hkt.cs:6771) sửa đúng cột này và ghi lịch sử
    /// <see cref="DlsDealDetailHisUpdPrice"/>.
    /// </summary>
    public decimal? Price { get; set; }
    /// <summary>Ngày giao xe cho khách (`DLS_DealDetail.DeliveryDate`) — `CarDeliveryDate_Update`
    /// cập nhật cột này theo cặp khoá `DealNo` + `CarId`.</summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// 🔴 #273 `Dls_DealDetail.WarrantyExpiresDate` — ngày hết hạn bảo hành của CHIẾC XE trong hợp đồng.
    /// Cần cho payload tạo hội viên Loyalty (`WarrantyExpiryDate`) ở bước duyệt kiểm chứng bán lẻ.
    /// Nguồn: `TERP.BizHTC/HCC/BizHTC.HCC.cs:3856` (hàm `..._New20260805` — **chỉ có trên máy 150**).
    /// </summary>
    public DateTime? WarrantyExpiresDate { get; set; }
}

/// <summary>
/// HOÁ ĐƠN TCG — phần đầu (`VAT_TCGInvoice`).
/// 🔴 **BẪY TWIN LỚN, đã trace kỹ:** WS 32-bit và 64-bit gọi **HAI BẢN KHÁC HẲN NHAU**:
/// · `TERP.WSHTC/App_Code/WSHTC.cs:34444` → `VAT_TCGInvoiceCreate_**New20181119**`;
/// · `TERP.WSHTC.64/WSHTC.asmx.cs:46231` → `VAT_TCGInvoiceCreate_**New20201210**`.
/// Bản 64-bit (mới hơn 2 năm) nằm ở **FILE KHÁC**: `TERP.BizHTC/HDDTIntergration/
/// BizHTC.HDDTIntergration.cs:20115` → gọi tiếp `VAT_TCGInvoiceCreateX_20201210` (14549).
/// Khớp memory `dmssales-biz-wh-not-always-authoritative-trace-ws`: bảng này được ghi ở **4 file**
/// (`BizHTC.InvoiceHTC_TCG.cs`, `Biz.HTC.WH.cs`, `HDDTIntergration.cs`, `…_TCG_20160924.cs`)
/// nên **không được chọn theo file, phải trace từ WS**. Canonical đã chọn = **bản 64-bit / 2020**.
/// 🔴 `VatTCGStatus` KHÔNG dùng cặp "A"/"R" như đa số cụm khác: tạo = **"P"** (Pending),
/// **duyệt = "F" (Stage.Finished)**, **không duyệt = "C" (Stage.Cancel)** — xem `VAT_TCGInvoiceApproveX`
/// (`HDDTIntergration.cs:19061`, dòng +132: `bApprove ? Stage.Finished : Stage.Cancel`).
/// ⚠️ Nguồn ghi song song `_dbMain` + `_dbWH` — nợ `_dbWH` chung fleet; có hàm đọc `_GetWH` riêng.
/// </summary>
public sealed class VatTcgInvoice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TCGInvoiceCode { get; set; } = "";
    /// <summary>Hoá đơn gốc khi đây là hoá đơn điều chỉnh.</summary>
    public string? SourceInvoiceCode { get; set; }
    /// <summary>Loại điều chỉnh (`InvoiceAdjType`).</summary>
    public string? InvoiceAdjType { get; set; }
    public string? InvoiceIDType { get; set; }
    public string? RefNo { get; set; }
    /// <summary>"P" tạo → "F" duyệt / "C" huỷ (KHÔNG phải A/R).</summary>
    public string VatTCGStatus { get; set; } = "P";
    /// <summary>Số hoá đơn — nguồn để NULL lúc tạo, chỉ điền khi duyệt.</summary>
    public string? TCGInvoiceNo { get; set; }
    public DateTime? TCGInvoiceDate { get; set; }
    /// <summary>Mã hoá đơn phía hệ thống HĐĐT — NULL lúc tạo.</summary>
    public string? OS_HDDT_InvoiceCode { get; set; }
    public string? OS_HDDT_RefNo { get; set; }
    public string? VAT { get; set; }
    public string? FlagView { get; set; }
    public string? TInvoiceCode { get; set; }
    public string? FlagImport { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// DÒNG hoá đơn TCG theo xe (`VAT_TCGInvoiceDetail`). Khoá dòng = cặp (`TCGInvoiceCode`, `VIN`).
/// 🔴 `TInvoicePrice` nguồn **luôn ghi 0** khi tạo (`dr["TInvoicePrice"] = 0;`), không lấy từ đầu vào.
/// 🔴 `TCGStatusDetail` = "P" khi tạo — trạng thái DÒNG tách khỏi `VatTCGStatus` của phần đầu.
/// `ProductionMonth` đi qua `StandardizeMonth` (chuẩn hoá tháng sản xuất), `CustomsClearanceDate`
/// qua `StandardizeDateOrDBNull` nên rỗng thì thành NULL chứ không phải ngày mặc định.
/// </summary>
public sealed class VatTcgInvoiceDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TCGInvoiceCode { get; set; } = "";
    public string VIN { get; set; } = "";
    public decimal? TCGUnitPrice { get; set; }
    public decimal? TCGVAT { get; set; }
    /// <summary>Nguồn luôn ghi 0 khi tạo.</summary>
    public decimal TInvoicePrice { get; set; }
    public string? BrandName { get; set; }
    public string? CarType { get; set; }
    public DateTime? CustomsClearanceDate { get; set; }
    public string? InvoiceNoFactory { get; set; }
    public string? InvoiceFactorySearch { get; set; }
    public string? ProductionMonth { get; set; }
    public string TCGStatusDetail { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// HỢP ĐỒNG NGOẠI — nhập khẩu xe (`CT_ContractOversea` — port 1:1
/// `ContractContractOverseaCreate_New20181119` / `…Delete_New20181119`, 2010.HTC
/// `TERP.BizHTC/DataWH/Biz.HTC.WH.cs` dòng 32211 / 32497).
/// TWIN: cả WS 32-bit (`WSHTC.cs:6069`) lẫn 64-bit (`WSHTC.64:7980`) **cùng bản**.
/// 🔴 Bảng đầu chỉ có **3 cột nghiệp vụ** (`ContractNo`, `CreatedDate`, `CreatedBy`) — toàn bộ
/// nội dung hợp đồng nằm ở **`Ord_PerformanceInvoiceDetail`**: một lệnh `Create` ghi CẢ HAI bảng,
/// và gán `ContractNo` xuống từng dòng PI detail (dòng 32485-32486).
/// ⇒ Đây là quan hệ **hợp đồng ngoại ↔ dòng Proforma Invoice**, không phải header/detail thông thường.
/// 🔴 Guard: `ContractNo` phải dài ít nhất `MinLengthCode` (**5**, `Const.Main.cs:322`);
/// bảng `Ord_PILCTemp` đầu vào **không được rỗng**; khoá chi tiết **không trùng**.
/// RBAC nguồn `myCommon_CheckHTCDirect` — nợ chung fleet.
/// ⚠️ Nguồn ghi song song `_dbMain` + `_dbWH` — nợ `_dbWH` chung fleet.
/// </summary>
public sealed class CtContractOversea
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractNo { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// THƯ TÍN DỤNG L/C (`CT_LC` — port 1:1 `ContractLCCreate_New20181119` (33077) /
/// `ContractLCDelete_New20181119` (33273)). TWIN: cả hai bit cùng bản.
/// Khoá là `LCNo`; `ContractNo` trỏ về <see cref="CtContractOversea"/>.
/// 🔴 `Delete` là **XOÁ THẬT** (`dt_CT_LC.Rows[0].Delete()`, dòng 33348).
/// </summary>
public sealed class CtLc
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string LCNo { get; set; } = "";
    public string ContractNo { get; set; } = "";
    public string? BankName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// PROFORMA INVOICE — phần đầu (`Ord_PerformanceInvoice`, 2010.HTC `Biz.HTC.WH.cs:29435`).
/// ⚠️ Tên bảng nguồn dùng **"Performance"** nhưng nghiệp vụ là **Proforma Invoice** (PI) — giữ nguyên
/// tên cột theo nguồn, đừng "sửa" thành Proforma.
/// `FlagAutoPL` cho biết có tự sinh Packing List hay không (`ContractPackingListCreateAuto_New20190923`).
/// Ba cột tháng tách bạch: `OrderMonth` (tháng đặt) · `ProductionMonth` (tháng sản xuất) ·
/// `ExpectedMonth` (tháng dự kiến về).
/// </summary>
public sealed class OrdPerformanceInvoice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RefNo { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? OrderMonth { get; set; }
    public string? ProductionMonth { get; set; }
    public string? ExpectedMonth { get; set; }
    /// <summary>Có tự sinh Packing List hay không ("1"/"0").</summary>
    public string? FlagAutoPL { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// DÒNG Proforma Invoice (`Ord_PerformanceInvoiceDetail`) — nơi chứa nội dung thật của hợp đồng ngoại.
/// 🔴 `ContractNo` **KHÔNG do người dùng nhập** ở lệnh tạo PI: nó được
/// `ContractContractOverseaCreate` **gán xuống** khi ký hợp đồng ngoại (dòng 32485).
/// Dòng chưa gắn hợp đồng thì cột này rỗng — đó là cách hệ nguồn phân biệt PI đã/chưa vào hợp đồng.
/// `LCTemp` là mã L/C tạm ghi trên dòng, khác với <see cref="CtLc"/> (L/C thật đã phát hành).
/// </summary>
public sealed class OrdPerformanceInvoiceDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RefNo { get; set; } = "";
    /// <summary>Mã L/C TẠM ghi trên dòng (khác CT_LC đã phát hành).</summary>
    public string? LCTemp { get; set; }
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? WorkOrderNo { get; set; }
    public string? PortCode { get; set; }
    public string? PlantCode { get; set; }
    public decimal? Quantity { get; set; }
    /// <summary>Do ContractContractOverseaCreate gán xuống, không phải người dùng nhập.</summary>
    public string? ContractNo { get; set; }
}

/// <summary>
/// Danh mục MÀN HÌNH / CHỨC NĂNG hệ thống (`Sys_Object`) — mảnh cuối của bộ RBAC
/// (`Sys_User` #119 → `Map_SG_SU`/`Map_SG_SO` #120 → `Sys_Object` ở đây).
/// Cột lấy theo `mySql_GetClauseColumnForSysObjectInfo` (2010.HTC `BizHTC.Common.cs:1547`).
/// 🔴 Bảng này **KHÔNG có hàm ghi** trong biz (không `SaveData("Sys_Object")`, không WS `Save…`) —
/// là **danh mục tĩnh do DBA nạp**, chỉ được ĐỌC. Vì vậy nó cũng không nằm trong danh sách 38 bảng
/// dựng từ `SaveData(` ở #117. MiniHTC vẫn cho ghi để nạp danh mục, nhưng ghi rõ đây là điểm khác.
/// 🔴 `ObjectType` theo `TConst.SysObjectType` (`Const.Main.cs:166`): **WS · WSFUNC · APP · MENU ·
/// SCR · BTN** — tức phân quyền xuống tới **từng NÚT**, không chỉ từng màn hình.
/// (`BIZFUNC` có trong nguồn nhưng **đã bị comment**, không dùng.)
/// 🔴 `ObjectCodeParent` tạo **cây phân cấp** (APP → MENU → SCR → BTN);
/// `ObjectCodeExec` + `PhysicalAssembly` + `PhysicalClass` là thông tin nạp form WinForm;
/// `FlagExecModal` cho biết mở dạng modal hay không.
/// ⚠️ Câu SQL của nguồn đặt bí danh `FlagActive` thành **`SOFlagActive`** để tránh đụng cột cùng tên
/// của `Sys_User` khi join — tên CỘT thật vẫn là `FlagActive`.
/// </summary>
public sealed class SysObject
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ObjectCode { get; set; } = "";
    /// <summary>WS | WSFUNC | APP | MENU | SCR | BTN (TConst.SysObjectType).</summary>
    public string? ObjectType { get; set; }
    public string? ObjectName { get; set; }
    /// <summary>Mã cha — tạo cây APP → MENU → SCR → BTN.</summary>
    public string? ObjectCodeParent { get; set; }
    /// <summary>Mã đối tượng được thực thi khi kích hoạt.</summary>
    public string? ObjectCodeExec { get; set; }
    public string? PhysicalAssembly { get; set; }
    public string? PhysicalClass { get; set; }
    /// <summary>Mở dạng modal hay không ("1"/"0").</summary>
    public string? FlagExecModal { get; set; }
    public string? PartnerCode { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// Map NHÓM ↔ NGƯỜI DÙNG (`Map_SG_SU` — port 1:1 `SysSaveMapSysGroupSysUser_New20181119`,
/// 2010.HTC `TERP.BizHTC/DataWH/Biz.HTC.WH.cs:16421`; hàm đọc `SysGetMapSysGroupSysUser`
/// ở `BizHTC.System.cs:659`). TWIN: cả WS 32-bit lẫn 64-bit **cùng bản** `_New20181119`.
/// 🔴 Lưu theo kiểu **XOÁ TRẮNG rồi CHÈN LẠI theo NHÓM**: nguồn `delete from Map_SG_SU where GroupCode
/// in (danh sách)` rồi `ResetAllDataRowState(Added)` + `SaveData` — nghĩa là bảng gửi lên phải là
/// **TOÀN BỘ thành viên của (các) nhóm đó**, gửi thiếu là **mất quyền** người không có trong danh sách.
/// 🔴 Nguồn **ép `PartnerCode = TConst.Sys_Partner.Desktop` ("DESKTOPAPPHTC")** cho MỌI dòng ghi vào
/// (`MyForceNewColumn` + vòng gán, dòng 16529-16533) — không lấy từ đầu vào. Bản ghi tạo từ web
/// (`"WEBHTC"`) sẽ **không** sinh ra qua hàm này.
/// </summary>
public sealed class MapSysGroupSysUser
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupCode { get; set; } = "";
    public string UserCode { get; set; } = "";
    /// <summary>Nguồn LUÔN ghi "DESKTOPAPPHTC" (TConst.Sys_Partner.Desktop).</summary>
    public string? PartnerCode { get; set; }
}

/// <summary>
/// Map NHÓM ↔ QUYỀN MÀN HÌNH (`Map_SG_SO` — port 1:1 `SysSaveMapSysGroupSysObject_New20181119`,
/// `Biz.HTC.WH.cs:16587`; hàm đọc `SysGetMapSysGroupSysObject` ở `BizHTC.System.cs:1188`).
/// `ObjectCode` trỏ sang `Sys_Object` (danh mục màn hình/chức năng) — bảng đó **chưa port**, ghi nợ.
/// 🔴 Cùng kiểu **xoá trắng theo nhóm rồi chèn lại** như `Map_SG_SU`.
/// ⚠️ Xem `### C0-bug10`: nhánh này của nguồn dựng mệnh đề WHERE bằng **NỐI CHUỖI** thay vì
/// `BuildClauseConditionList` như nhánh `Map_SG_SU` — lỗ hổng SQL injection ngay trong hàm gán QUYỀN.
/// MiniHTC dùng tham số hoá của EF nên **không nhân bản lỗ hổng**.
/// </summary>
public sealed class MapSysGroupSysObject
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupCode { get; set; } = "";
    /// <summary>Mã màn hình/chức năng (`Sys_Object.ObjectCode` — bảng đó chưa port).</summary>
    public string ObjectCode { get; set; } = "";
    public string? PartnerCode { get; set; }
}

/// <summary>
/// NGƯỜI DÙNG hệ thống (`Sys_User` — port 1:1 `SysSaveUser_New20181119` /
/// `SysResetUserPassword_New20181119` / `CommonChangeUserPassword_New20181119`,
/// 2010.HTC `TERP.BizHTC/DataWH/Biz.HTC.WH.cs` dòng 16095 / 15970 / 30).
/// TWIN: **cả WS 32-bit lẫn 64-bit gọi CÙNG bản** `_New20181119` (không lệch như cụm TCG #118).
/// 🔴 Luật `PasswordTemplate`: nếu giá trị mật khẩu gửi lên **đúng bằng chuỗi mẫu `"********"`**
/// (`TConst.HTCConst.PasswordTemplate`, `Const.Main.cs:321`) thì nguồn **LOẠI cột `UserPassword`
/// khỏi danh sách ghi** (`alEffColForNotChangePw.Remove("UserPassword")`) — tức **KHÔNG đổi mật khẩu**.
/// Đây là cách form che mật khẩu mà vẫn lưu được các trường khác; port giữ nguyên hành vi này.
/// 🔴 Một lệnh lưu xử lý **BA nhóm thay đổi cùng lúc** theo `DataRowState`: `Deleted` (xoá thật),
/// `Modified` (tách tiếp thành đổi/không đổi mật khẩu), `Added`.
/// ⚠️ **KHÁC BIỆT CÓ CHỦ ĐÍCH VỀ AN NINH** — xem `### C0-bug9`: nguồn lưu mật khẩu **PLAINTEXT}
/// và so sánh trực tiếp (`StringEqual(strPasswordOld, Rows[0]["UserPassword"])`, dòng 90).
/// MiniHTC **KHÔNG nhân bản lỗ hổng đó**: cột này lưu **SHA-256 của mật khẩu**, đặt tên
/// `UserPasswordHash` cho rõ nghĩa. Mọi luật nghiệp vụ khác giữ nguyên 1:1.
/// </summary>
public sealed class SysUser
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string UserCode { get; set; } = "";
    public string? UserName { get; set; }
    /// <summary>🔴 SHA-256, KHÔNG phải plaintext như nguồn (xem C0-bug9).</summary>
    public string? UserPasswordHash { get; set; }
    public string? PartnerCode { get; set; }
    public string? DealerCode { get; set; }
    public string? BankCode { get; set; }
    public string? TransporterCode { get; set; }
    public string? InsCompanyCode { get; set; }
    /// <summary>Quyền quản trị hệ thống ("1"/"0").</summary>
    public string? FlagSysAdmin { get; set; }
    /// <summary>Quyền CHỈ XEM ("1"/"0") — tách riêng khỏi FlagSysAdmin.</summary>
    public string? FlagSysViewer { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// NHÓM người dùng (`Sys_Group` — port 1:1 `SysSaveGroup_New20181119`,
/// 2010.HTC `Biz.HTC.WH.cs:16285`). TWIN: cả 32-bit lẫn 64-bit cùng bản.
/// 🔴 Nguồn lưu nhóm bằng `SaveData("Sys_Group", dt)` **KHÔNG truyền `alColumnEffective`** —
/// tức ghi TOÀN BỘ cột của dòng, khác hẳn cách lưu `Sys_User` (có lọc cột). Đó là chủ đích.
/// Việc gán người dùng vào nhóm và gán quyền nằm ở các bảng map riêng
/// (`SysSaveMapSysGroupSysUser`, `SysSaveMapSysGroupSysObject`) — **chưa port**, ghi nợ.
/// </summary>
public sealed class SysGroup
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupCode { get; set; } = "";
    public string? GroupName { get; set; }
    public string? PartnerCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Master NGÂN HÀNG (`Mst_Bank` — nguồn `Mst_Bank_CheckDB`, 2010.HTC
/// `TERP.BizHTC/DataWH/Biz.HTC.WH.cs:355`; khoá là `BankCode`).
/// 🔴 `BankCodeParent` cho thấy master này có **cấu trúc CHA–CON**: chi nhánh trỏ về ngân hàng mẹ.
/// Master này là thứ đã chặn guard ở **#94** và **#98** (sửa mã ngân hàng của hợp đồng / của giao dịch
/// bán lẻ) — nay đã có, các guard đó mở khoá được.
/// </summary>
public sealed class MstBank
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BankCode { get; set; } = "";
    public string? BankName { get; set; }
    /// <summary>Mã ngân hàng mẹ — rỗng nghĩa là bản ghi gốc, không phải chi nhánh.</summary>
    public string? BankCodeParent { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// Master QUẬN/HUYỆN (`Mst_District` — nguồn tra tại `Biz.HTC.WH.hkt.cs:9076`).
/// 🔴 Khoá là **CẶP** (`ProvinceCode`, `DistrictCode`), không phải mình `DistrictCode` —
/// nguồn luôn lọc đồng thời cả hai (`and t.ProvinceCode = @… and t.DistrictCode = @…`).
/// Master này đã chặn guard ở **#92** (sửa tỉnh/huyện của biên bản giao xe).
/// </summary>
/// <summary>
/// #227 parity `Mst_Province` — danh mục TỈNH/THÀNH (DMSCarSv, `BizCarSv.Master.cs:8588` `Mst_Province_Get`).
/// Nguồn `select mpg.*` và lọc theo 6 cột: `ProvinceCode` · `ProvinceName` · `FlagActive` ·
/// `CreatedDate` · `CreatedBy` · `AreaCode` ⇒ đó là bộ cột của bảng.
/// ⚠️ `AreaCode` (vùng/miền) là cột **có lọc riêng ở nguồn** — không phải cột trang trí.
/// </summary>
public sealed class MstProvince
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ProvinceCode { get; set; } = "";
    public string? ProvinceName { get; set; }
    public string? AreaCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
}

public sealed class MstDistrict
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ProvinceCode { get; set; } = "";
    public string DistrictCode { get; set; } = "";
    public string? DistrictName { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// Master LOẠI HÌNH BÁN của đại lý (`Mst_DealerSalesType` — nguồn `Mst_DealerSalesType_CheckDB`,
/// 2010.HTC `TERP.BizHTC/BizHTC.DealerSales.cs:138`; khoá `SalesType`).
/// 🔴 Guard của nguồn tra theo **CẶP** (`SalesType`, `FlagActive`) trong cùng một lệnh
/// `GetTableContents` — tức loại hình đã ngưng thì coi như KHÔNG tồn tại, không phải "tồn tại nhưng khoá".
/// `SalesGroupType` gom các loại hình thành nhóm. Master này đã chặn guard ở **#94**.
/// </summary>
public sealed class MstDealerSalesType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SalesType { get; set; } = "";
    public string? SalesTypeNameVN { get; set; }
    /// <summary>Nhóm loại hình bán.</summary>
    public string? SalesGroupType { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// Bộ đếm sinh mã dùng chung (`Seq_*` — port 1:1 `Seq_Common_Get_New20181115` /
/// `Seq_Common_MyGet` / `Seq_Common_Raw`, 2010.HTC `BizHTC.Marketing.cs` dòng 16924 / 16868 / 16854).
/// 🔴 Nguồn KHÔNG có một bảng đếm duy nhất: mỗi loại mã trỏ tới **một bảng `Seq_*` riêng**
/// (`Seq_Id`, `Seq_MRKFilePath`, `Seq_HMCList`, `Seq_GPSUnMapVINNo`, `Seq_PrintVAT`,
/// `Seq_BulkInfo`, `Seq_RequestId`, `Seq_CarReq`, `Seq_ATApprOrdNo`, `Seq_GrtClaimExtNo`),
/// và lấy số bằng thủ thuật `insert … values(null); delete … where AutoID = @@Identity; select @@Identity`
/// — tức **mượn IDENTITY của SQL Server rồi xoá ngay dòng vừa chèn**. MiniHTC dùng Postgres nên port
/// thành **một bảng đếm có khoá là tên bảng `Seq_*`**, giữ nguyên việc mỗi loại mã đếm riêng.
/// 🔴 HAI loại mã dùng CHUNG một bộ đếm: `TCGIV` và `HTCIV` đều đếm trên `Seq_PrintVAT`;
/// `CTRM` và `CDV` đều đếm trên `Seq_CarReq`. Đó là chủ đích của nguồn, không phải nhầm.
/// </summary>
public sealed class SeqCounter
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Tên bảng đếm của nguồn, ví dụ "Seq_PrintVAT".</summary>
    public string SeqTableName { get; set; } = "";
    /// <summary>Giá trị đã cấp gần nhất; mỗi lần lấy mã thì tăng 1.</summary>
    public long LastValue { get; set; }
}

/// <summary>
/// Master **KPI** (`Mst_KPI` — port 1:1 `Mst_KPI_Get_New20181115`, 2010.HTC
/// `BizHTC.Marketing.cs:7662`; `Mst_KPI_CheckDB` tại 7590 khoá theo cặp (`KPICode`, `KPIType`)).
/// 🔴 Cột cờ hoạt động của ba master `Mst_KPI` / `Mst_KPIType` / `MRK_Mst_AreaMarket` tên là
/// **`FlagAcitve`** — **sai chính tả NGUYÊN VĂN trong DB** (Acitve thay vì Active), xác nhận ở
/// `BizHTC.Marketing.cs` dòng 7808 / 8080 / 11961. Giữ nguyên, KHÔNG "sửa" thành `FlagActive`
/// kẻo lệch tên cột với hệ nguồn.
/// </summary>
/// <summary>
/// 🔴 #329 BÁO CÁO KPI XƯỞNG DỊCH VỤ — `Report_KPI` (`Report_KPICreate_New20221101`,
/// `BizCarSv.zzzzCode.cs:1150`). MiniHTC mới có **danh mục** KPI (`MstKpi`/`MstKpiType`), **chưa có
/// bản ghi báo cáo**. Đây là cụm lệch LỚN NHẤT của sweep #320: **44 cột** giữa hai bản.
///
/// TRACE TWIN: WS `:27809` gọi `Report_KPICreate_**New20221101**` (100 cột) ⇒ bản trần (56 cột) CHẾT.
/// ⚠️ Cả hai bản nằm trong `zzzzCode.cs` — tên file gợi ý "code rác", **nhưng bản LIVE lại ở đó**.
///   Không được loại một hàm chỉ vì tên file; chỉ WS mới quyết định.
/// ⚠️ Sweep đánh dấu ⚠️ (ủy quyền ghi) cho cả hai ⇒ số cột có thể còn hụt; đã đối chiếu tay danh sách
///   `[\"X\"] =` trong trọn thân hàm để lấy đủ 100.
///
/// 🔴 TỪ VỰNG LOẠI CÔNG VIỆC (lặp trong hầu hết tên cột) — **SCC** · **SCD** · **SCS** · **SPK** ·
/// **BDD** · **BDN**; mỗi loại lại chia theo NGUỒN TIỀN: `…RoRepair` (sửa chữa) · `…RoInsurance`
/// (bảo hiểm) · `…RoWarranty` (bảo hành) · `…Local` (nội bộ). Đừng gộp bốn nguồn tiền làm một.
/// </summary>
public sealed class ReportKpi
{
    // ===== 🔴 #403 §12 KỲ BÁO CÁO — ba cột khung mà bản port cũ THIẾU HẲN =====
    //   Nguồn `RptKPICreate` **luôn** gán `RptYear` · `RptMonth` · `RptBy` (cùng `DealerCode`,
    //   `Status`), và guard `CheckExistRptKPIYearMonth` dựa trên đúng bộ ba (đại lý, năm, tháng).
    //   Không có hai cột kỳ này thì **báo cáo KPI không có danh tính kỳ** và guard trùng kỳ
    //   **không thể viết được**.
    /// <summary>Năm của kỳ báo cáo (`RptYear`).</summary>
    public string? RptYear { get; set; }
    /// <summary>Tháng của kỳ báo cáo (`RptMonth`).</summary>
    public string? RptMonth { get; set; }
    /// <summary>Người lập báo cáo (`RptBy`) — khác <see cref="CreatedBy"/> của tầng port.</summary>
    public string? RptBy { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public decimal? AccessoryAmountAfterVAT { get; set; }
    public decimal? AccessoryAmountOut { get; set; }
    public decimal? AdvisoryNumber { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public decimal? CabinetPaintNumber { get; set; }
    public decimal? CarPerAdviserDay { get; set; }
    public decimal? CavityBPNumber { get; set; }
    public decimal? CavityCopperNumber { get; set; }
    public decimal? CavityMaintainNumber { get; set; }
    public decimal? CavityOtherNumber { get; set; }
    public decimal? CavityParkingNumber { get; set; }
    public decimal? CavityQtyPerEngineerBDNSCC { get; set; }
    public decimal? CavityRONumber { get; set; }
    public decimal? CountBDD { get; set; }
    public decimal? CountBDDLocal { get; set; }
    public decimal? CountBDDPerCavityMaintain { get; set; }
    public decimal? CountBDDRoRepair { get; set; }
    public decimal? CountCarService { get; set; }
    public decimal? CountSCC { get; set; }
    public decimal? CountSCCLocal { get; set; }
    public decimal? CountSCCPerCavityRO { get; set; }
    public decimal? CountSCCRoInsurance { get; set; }
    public decimal? CountSCCRoRepair { get; set; }
    public decimal? CountSCCRoWarranty { get; set; }
    public decimal? CountSCD { get; set; }
    public decimal? CountSCDLocal { get; set; }
    public decimal? CountSCDPerCavityCopper { get; set; }
    public decimal? CountSCDRoInsurance { get; set; }
    public decimal? CountSCDRoRepair { get; set; }
    public decimal? CountSCDRoWarranty { get; set; }
    public decimal? CountSCS { get; set; }
    public decimal? CountSCSLocal { get; set; }
    public decimal? CountSCSPerCabinetPaint { get; set; }
    public decimal? CountSCSPerCavityBP { get; set; }
    public decimal? CountSCSRoInsurance { get; set; }
    public decimal? CountSCSRoRepair { get; set; }
    public decimal? CountSCSRoWarranty { get; set; }
    public decimal? CountSPK { get; set; }
    public decimal? CountSPKLocal { get; set; }
    // #335: ba cot PDI — co trong 105 cot cua Report_KPICreateX_New20221101 nhung KHONG
    //   nam trong 98 cot ma #329 port tu Report_KPICreate_New20221101. Chinh la mot phan
    //   cua do lech 61 vs 105 da ghi o #330.
    public decimal? CountPDI { get; set; }
    public decimal? CountPDIRoRepair { get; set; }
    public decimal? CountPDILocal { get; set; }
    public decimal? CountSPKRoRepair { get; set; }
    public DateTime? DateReport { get; set; }
    public string? DealerCode { get; set; }
    public decimal? EmploymentRate { get; set; }
    // ===== 🔴 #339 SỬA KIỂU: ba cột dưới đây là **SỐ ĐẾM**, không phải chuỗi =====
    // Nguồn: `(select count(0) from #tbl_Ser_Engineer where IsEngineer='KTVD') EnginerBP` …
    // #329 sinh entity bằng bảng phân loại theo TÊN cột và xếp nhầm ba cột này vào nhóm text
    //   (`EnginerBP` · `SparePartsStaff` · `StaffOrther`), trong khi hai cột **cùng nhóm nghiệp vụ**
    //   là `ServiceTechnicianQty` / `PaintingTechnicianQty` lại đúng `decimal?` vì tên có đuôi "Qty".
    //   ⇒ Phân loại theo TÊN là nguồn lỗi; phải theo **biểu thức SQL sinh ra cột**.
    public decimal? EnginerBP { get; set; }
    public decimal? EnginerNumber { get; set; }
    public decimal? LaborProductivity { get; set; }
    public string? LogLUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public decimal? PaintingTechnicianQty { get; set; }
    public decimal? PartAmountLocal { get; set; }
    public decimal? PartAmountOut { get; set; }
    public decimal? PartAmountRoInsurance { get; set; }
    public decimal? PartAmountRoRepair { get; set; }
    public decimal? PartAmountRoWarranty { get; set; }
    public decimal? PartAmountShell { get; set; }
    public decimal? PartProfitRate { get; set; }
    public decimal? RevenuePerAdviser { get; set; }
    public decimal? RevenuePerKTVBDN { get; set; }
    public decimal? RevenuePerKTVSCC { get; set; }
    public decimal? RevenuePerKTVSCD { get; set; }
    public decimal? RevenuePerKTVSCS { get; set; }
    public decimal? SerProfitRate { get; set; }
    public decimal? ServiceAmountBDDLocal { get; set; }
    public decimal? ServiceAmountBDDRoRepair { get; set; }
    public decimal? ServiceAmountSCCLocal { get; set; }
    public decimal? ServiceAmountSCCRoInsurance { get; set; }
    public decimal? ServiceAmountSCCRoRepair { get; set; }
    public decimal? ServiceAmountSCCRoWarranty { get; set; }
    public decimal? ServiceAmountSCDLocal { get; set; }
    public decimal? ServiceAmountSCDRoInsurance { get; set; }
    public decimal? ServiceAmountSCDRoRepair { get; set; }
    public decimal? ServiceAmountSCDRoWarranty { get; set; }
    public decimal? ServiceAmountSCSLocal { get; set; }
    public decimal? ServiceAmountSCSRoInsurance { get; set; }
    public decimal? ServiceAmountSCSRoRepair { get; set; }
    public decimal? ServiceAmountSCSRoWarranty { get; set; }
    public decimal? ServiceAmountSPKLocal { get; set; }
    // #336: hai cot doanh thu tien cong PDI — cung nhom thieu voi 3 cot CountPDI* cua #335.
    public decimal? ServiceAmountPDIRoRepair { get; set; }
    public decimal? ServiceAmountPDILocal { get; set; }
    public decimal? ServiceAmountSPKRoRepair { get; set; }
    public decimal? ServiceProductivity { get; set; }
    public decimal? ServiceTechnicianQty { get; set; }
    public decimal? ShellAmountOut { get; set; }
    public decimal? SparePartsStaff { get; set; }
    public decimal? StaffOrther { get; set; }
    public string? Status { get; set; }
    public decimal? UnitPriceBDN { get; set; }
    public decimal? UnitPriceSCC { get; set; }
    public decimal? UnitPriceSCD { get; set; }
    public decimal? UnitPriceSCS { get; set; }
    public decimal? WorkDayQty { get; set; }
    public decimal? WorkHourActualQty { get; set; }
    public decimal? WorkHourBDNQty { get; set; }
    public decimal? WorkHourFeeQty { get; set; }
    public decimal? WorkHourPerCarRO { get; set; }
    public decimal? WorkHourQty { get; set; }
    public decimal? WorkHourSCCQty { get; set; }
    public decimal? WorkHourSCDQty { get; set; }
    public decimal? WorkHourSCSQty { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public sealed class MstKpi
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string KPICode { get; set; } = "";
    public string KPIType { get; set; } = "";
    /// <summary>🔴 Sai chính tả nguyên văn của nguồn: "FlagAcitve".</summary>
    public string FlagAcitve { get; set; } = "1";
}

/// <summary>Master LOẠI KPI (`Mst_KPIType` — `Mst_KPIType_Get_New20181115`, dòng 7949). Cờ cũng là `FlagAcitve`.</summary>
public sealed class MstKpiType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string KPIType { get; set; } = "";
    public string FlagAcitve { get; set; } = "1";
}

/// <summary>
/// Master VÙNG THỊ TRƯỜNG marketing (`MRK_Mst_AreaMarket` — `MRK_Mst_AreaMarket_Get_New20181115`,
/// dòng 11828). Cờ cũng là `FlagAcitve` (sai chính tả nguyên văn).
/// Master này là đích của `Mst_Dealer_UpdateMRKAMCode` (gán vùng cho đại lý) — hàm đó **chưa port**.
/// </summary>
public sealed class MrkMstAreaMarket
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKAMCode { get; set; } = "";
    public string FlagAcitve { get; set; } = "1";
}

/// <summary>Master LOẠI TÀI LIỆU (`Mst_DocType` — dòng 12238). Có cột `Seq` để sắp thứ tự hiển thị.</summary>
public sealed class MstDocType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DocType { get; set; } = "";
    /// <summary>Thứ tự hiển thị — nguồn `order by mdt.Seq`.</summary>
    public int? Seq { get; set; }
}

/// <summary>
/// Master LOẠI SỰ KIỆN (`Mst_EvenType` — `Mst_EvenType_Get`, dòng 12490).
/// ⚠️ Tên bảng nguồn **thiếu chữ t**: "EvenType" (đúng phải là EventType).
/// ⚠️ Quirk nguồn: bộ lọc chạy trên cột `met.EvenType` nhưng câu select lại lấy ra `met.DocType`
/// — hai cột khác nhau. Port giữ cả hai cột để không mất dữ liệu bên nào.
/// </summary>
public sealed class MstEvenType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string EvenType { get; set; } = "";
    /// <summary>Nguồn select ra cột này (xem quirk ở phần mô tả lớp).</summary>
    public string? DocType { get; set; }
}

/// <summary>
/// Master QUÝ (`Mst_Quater` — `Mst_Quater_Get`, dòng 12749).
/// ⚠️ Tên bảng thiếu chữ r ("Quater" thay vì Quarter), và tên cột trong câu select viết
/// **`Quatercode`** (chữ c thường) trong khi bộ lọc viết `QuaterCode`. SQL Server không phân biệt
/// hoa-thường nên nguồn chạy được; Postgres thì có, nên ở đây thống nhất dùng **`QuaterCode`**.
/// </summary>
public sealed class MstQuater
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string QuaterCode { get; set; } = "";
}

/// <summary>Master LOẠI FILE (`Mst_FileType` — `Mst_FileType_Get_New20181115`, dòng 13008).</summary>
/// <summary>#523 Tệp tải lên (`UploadFile_ForTab` — `BizCarSv.UploadFile.cs:1986`).
/// Nguồn ghi ra **đĩa** dưới `UploadedFiles\`; MiniHTC lưu **nội dung trong DB** (lệch CỐ Ý,
/// vì nền chạy không có đĩa bền) và giữ nguyên `FilePath` mà nguồn trả về.</summary>
public sealed class UploadedFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Tên tệp SAU khi nguồn gắn tiền tố `yyyyMMdd_HHmmss_fff`.</summary>
    public string FileName { get; set; } = "";
    /// <summary>Đường dẫn nguồn trả về: `UploadedFiles\&lt;FileName&gt;` (bản ghi LẦN HAI).</summary>
    public string FilePath { get; set; } = "";
    /// <summary>Phần mở rộng VIẾT HOA — nguồn kiểm bằng `Mst_FileTypeUpload_CheckDB`.</summary>
    public string? FileTypeCode { get; set; }
    public byte[]? Content { get; set; }
    public long SizeBytes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

public sealed class MstFileType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string FileType { get; set; } = "";
}

/// <summary>
/// Master ĐỢT GIẢI NGÂN (`Mst_Disbursment` — `Mst_Disbursment_Get`, dòng 13256).
/// ⚠️ Nguồn LỆCH chính tả giữa hai chỗ trong CÙNG một hàm: câu select lấy `md.DisbursmentCode`
/// (có s) còn bộ lọc dựng trên `DisburmentCode` (**thiếu s**). Ở đây dùng dạng của câu select.
/// </summary>
public sealed class MstDisbursment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DisbursmentCode { get; set; } = "";
}

/// <summary>
/// TÀI LIỆU marketing (`Mst_Doc` — port 1:1 `Mst_Doc_Get/Create/Delete_New20181115`,
/// dòng 13515 / 13726 / 13904). `Delete` là **XOÁ THẬT** (`DataRow.Delete()`).
/// ⚠️ Xem `### C0-bug8`: hàm `Create` của nguồn gán `Remark = TConst.Flag.Active` — ghi cờ "1" vào
/// cột GHI CHÚ, làm mất ghi chú người dùng nhập (`strRemark` bị bỏ không dùng). Port ghi đúng `strRemark`.
/// </summary>
public sealed class MstDoc
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DocName { get; set; } = "";
    public string DocType { get; set; } = "";
    public string? FileNameActual { get; set; }
    public string? FilePath { get; set; }
    public string FlagActive { get; set; } = "1";
    public string? Remark { get; set; }
}

/// <summary>
/// CHIẾN DỊCH ĐẠI LÝ theo quý — phần đầu (`MRK_CampaignDL` — port 1:1 cụm 7 hàm
/// `MRK_CampaignDL_Get/Save/Update/Approve` + `MRK_CampaignDLRegisterDtl_Get` +
/// `MRK_CampaignDLActualDtl_Get` + `MRK_CampaignDLQuarterKPI_Get`, tất cả `_New20181115`;
/// 2010.HTC `BizHTC.Marketing.cs` dòng 8517 / 9042 / 10724 / 10368 / 10989 / 11252 / 11516).
/// Đây là **cụm lớn nhất của module Marketing: SÁU bảng** ghi trong một lệnh `Save`.
/// Khoá nghiệp vụ = bộ **BA**: (`DealerCode`, `MRKCamDLYear`, `MRKCamDLQuarter`).
/// 🔴 Trạng thái dùng `TConst.MRKCampaignStatus` — **dùng CHUNG với cụm `MRK_Campaign`** (#111),
/// chỉ "P"/"A". Nhưng tên CỘT thì mỗi bảng một kiểu: `MRKCamDLStatus` · `…StatusRegister` ·
/// `…StatusRegisterDtl` · `…StatusActual` · `…StatusActualDtl` · `…StatusQKPI`.
/// 🔴 `Approve` đồng bộ trạng thái xuống **CẢ NĂM bảng con**, không chỉ phần đầu.
/// 🔴 `Save` mang cờ `strFlagIsDelete` (luật C0-centesimusdecimus) và xoá **cả sáu bảng** trước khi chèn lại.
/// 🔴 `Update` **chỉ sửa BA trường**: `BonusPoint`, `ObligationKPI`, `KPIRank` — mọi cột điểm khác
/// (`SalePromotionPoint`, `BrandingPoint`, `TotalRealPoint`, `TotalRegisterPoint`, `DegreeCompletionKPI`)
/// được **đọc lại từ DB và ghi nguyên**, tức là do hệ thống tính, người dùng không sửa được.
/// ⚠️ Tên cột `CreateDateTime`/`CreateBy` ở bảng này **KHÔNG có chữ d** (khác `CreatedDateTime`/`CreatedBy`
/// của mọi cụm khác trong cùng file) — giữ nguyên theo nguồn.
/// ⚠️ Guard `Mst_KPI_CheckDB` chưa port — MiniHTC không có master `Mst_KPI`.
/// </summary>
public sealed class MrkCampaignDL
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    /// <summary>Điểm khuyến mãi — hệ thống tính, Update không sửa.</summary>
    public decimal? SalePromotionPoint { get; set; }
    /// <summary>Điểm thương hiệu — hệ thống tính, Update không sửa.</summary>
    public decimal? BrandingPoint { get; set; }
    /// <summary>Điểm thưởng — MỘT trong ba trường Update sửa được.</summary>
    public decimal? BonusPoint { get; set; }
    public decimal? TotalRealPoint { get; set; }
    public decimal? TotalRegisterPoint { get; set; }
    public decimal? DegreeCompletionKPI { get; set; }
    /// <summary>KPI nghĩa vụ — Update sửa được.</summary>
    public string? ObligationKPI { get; set; }
    /// <summary>Hạng KPI — Update sửa được.</summary>
    public string? KPIRank { get; set; }
    public string? RegisterFilePath { get; set; }
    public string? ResultFilePath { get; set; }
    public string MRKCamDLStatus { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    /// <summary>Nguồn đặt tên KHÔNG có chữ d: `CreateDateTime`.</summary>
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    /// <summary>Nguồn đặt tên KHÔNG có chữ d: `CreateBy`.</summary>
    public string? CreateBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phiếu ĐĂNG KÝ của chiến dịch đại lý (`MRK_CampaignDLRegister`).</summary>
public sealed class MrkCampaignDLRegister
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCamDLRegisterNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    /// <summary>Tên phiếu đăng ký (nguồn: `MRKPICamDLRegisterName`, có chèn "PI" giữa tên).</summary>
    public string? MRKPICamDLRegisterName { get; set; }
    public string? RegisterFilePath { get; set; }
    public string? ReportFilePath { get; set; }
    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    public DateTime? ReportDateEnd { get; set; }
    public string MRKCamDLStatusRegister { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng KPI ĐĂNG KÝ (`MRK_CampaignDLRegisterDtl`) — số lượng và chất lượng cam kết.</summary>
public sealed class MrkCampaignDLRegisterDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCamDLRegisterNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    public string KPICode { get; set; } = "";
    public string? KPIType { get; set; }
    public decimal? QtyRegister { get; set; }
    public decimal? ValQualityRegister { get; set; }
    public string MRKCamDLStatusRegisterDtl { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phiếu THỰC HIỆN của chiến dịch đại lý (`MRK_CampaignDLActual`).</summary>
public sealed class MrkCampaignDLActual
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCamDLActualNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    public string? MRKPICamDLActualName { get; set; }
    public string? RegisterFilePath { get; set; }
    public string? ReportFilePath { get; set; }
    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    public DateTime? ReportDateEnd { get; set; }
    public string MRKCamDLStatusActual { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng KPI THỰC HIỆN (`MRK_CampaignDLActualDtl`) — số lượng và chất lượng đạt được.</summary>
public sealed class MrkCampaignDLActualDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCamDLActualNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    public string KPICode { get; set; } = "";
    public string? KPIType { get; set; }
    public decimal? QtyActual { get; set; }
    public decimal? ValQualityActual { get; set; }
    public string MRKCamDLStatusActualDtl { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// TỔNG HỢP KPI theo quý (`MRK_CampaignDLQuarterKPI`) — nơi đối chiếu đăng ký với thực hiện:
/// `TotalQtyRegister` vs `TotalQtyActual`, và quy ra `ActualPoint` trên `RegisterStandardPoint`.
/// </summary>
public sealed class MrkCampaignDLQuarterKPI
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string MRKCamDLYear { get; set; } = "";
    public string MRKCamDLQuarter { get; set; } = "";
    public string KPICode { get; set; } = "";
    public string? KPIType { get; set; }
    public decimal? RegisterStandardPoint { get; set; }
    public decimal? TotalQtyRegister { get; set; }
    public decimal? TotalQtyActual { get; set; }
    public decimal? AvgValQualityActual { get; set; }
    public decimal? ActualPoint { get; set; }
    public string MRKCamDLStatusQKPI { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// CHIẾN DỊCH marketing — phần đầu (`MRK_Campaign` — port 1:1 cụm 3 hàm
/// `MRK_Campaign_Get/Save/Approve_New20181115`, 2010.HTC `BizHTC.Marketing.cs`
/// dòng 15916 / 16167 / 16628).
/// 🔴 Hằng trạng thái riêng `TConst.MRKCampaignStatus` (`Const.Main.cs:832`): chỉ `Pending = "P"`
/// và `Approve = "A"` — giống cấu trúc `MRKScopLimitStatus` của #109 nhưng là **lớp hằng khác**.
/// 🔴 `Save` mang cờ `strFlagIsDelete`: "1" thì **chỉ xoá**, ngược lại là **upsert** (xoá rồi chèn lại
/// cả đầu lẫn chi tiết). Sửa bản ghi đã có thì nó **phải đang "P"**.
/// 🔴 `Approve` cập nhật **CẢ HAI bảng**: `MRKCampaignStatus` ở đầu và `MRKCampaignStatusDetail` từng dòng.
/// ⚠️ Chi tiết ghi nhận trong nguồn: hàm `Approve` gán `MRKCampaignStatus = TConst.MRKScopLimitStatus.Approve`
/// — dùng nhầm hằng của cụm ScopeLimit. Vô hại vì cả hai đều là "A", nhưng là copy-paste lệch.
/// ⚠️ Guard `Mst_EvenType_CheckDB` (loại sự kiện) CHƯA port — MiniHTC không có master `Mst_EvenType`
/// (tên nguồn thiếu chữ t: "EvenType"). Ghi nợ, không bịa master rỗng.
/// </summary>
public sealed class MrkCampaign
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCampaignNo { get; set; } = "";
    public string MRKCampaignName { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>Loại sự kiện (`Mst_EvenType` — nguồn viết thiếu chữ t).</summary>
    public string? EvenType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    /// <summary>"P" chờ duyệt / "A" đã duyệt (TConst.MRKCampaignStatus).</summary>
    public string MRKCampaignStatus { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// FILE của chiến dịch marketing (`MRK_CampaignDetail`). Mỗi dòng là một file kèm theo chiến dịch.
/// ⚠️ Guard `Mst_FileType_CheckDB` (loại file) CHƯA port — MiniHTC không có master `Mst_FileType`.
/// </summary>
public sealed class MrkCampaignDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKCampaignNo { get; set; } = "";
    public string? FileType { get; set; }
    public string? FileNameActual { get; set; }
    public string? FilePath { get; set; }
    /// <summary>Đồng bộ từ phần đầu khi duyệt.</summary>
    public string MRKCampaignStatusDetail { get; set; } = "P";
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Hồ sơ KPI / giải ngân marketing theo quý của đại lý (`MRK_KPIDisbursment` — port 1:1 cụm 2 hàm
/// `MRK_KPIDisbursment_Get/Save_New20181115`, 2010.HTC `BizHTC.Marketing.cs` dòng 15243 / 15545).
/// Khoá nghiệp vụ = bộ **BỐN**: (`KPIDisbursmentYear`, `QuaterCode`, `DealerCode`, `KPIDisbursmentType`).
/// 🔴 Một hàm `Save` LÀM CẢ HAI VIỆC: tham số `strFlagIsDelete = "1"` thì **chỉ xoá**; ngược lại là
/// **upsert** (xoá theo khoá bốn rồi chèn lại). Không có hàm Delete riêng.
/// 🔴 `KPIDisbursmentType` theo `TConst.KPIDisburmentType` — ⚠️ **tên lớp hằng thiếu chữ s**
/// ("Disburment") trong khi tên cột lại có ("Disbursment"), và bản thân cả hai đều sai so với
/// "Disbursement" chuẩn. Ba giá trị: `KPICommit = "KPICOMMIT"` (cam kết KPI) ·
/// `KPIResult = "KPIRESULT"` (kết quả KPI) · `KPIDBTable = "KPIDBTable"` (bảng giải ngân).
/// ⚠️ Giá trị thứ ba **KHÔNG viết hoa toàn bộ** như hai giá trị kia — nguồn so sánh bằng
/// `StringEqualIgnoreCase` nên vẫn khớp, nhưng **giá trị ghi xuống DB thì giữ nguyên dạng gốc**.
/// ⚠️ Guard `Mst_Quater_CheckDB` (mã quý phải tồn tại và Active) CHƯA port — MiniHTC không có
/// master `Mst_Quater`; ghi nợ chứ không bịa master rỗng.
/// </summary>
public sealed class MrkKpiDisbursment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string KPIDisbursmentYear { get; set; } = "";
    public string QuaterCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>KPICOMMIT | KPIRESULT | KPIDBTable.</summary>
    public string KPIDisbursmentType { get; set; } = "";
    public string? FileNameActual { get; set; }
    public string? FilePath { get; set; }
    /// <summary>Nguồn luôn ghi "1" khi lưu (TConst.Flag.Active).</summary>
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// HẠN MỨC ngân sách marketing theo năm — phần đầu (`MRK_ScopeLimit` — port 1:1 cụm 4 hàm
/// `MRK_ScopeLimit_Get/Save/Approve` + `MRK_ScopeLimitDetail_Get`, 2010.HTC
/// `BizHTC.Marketing.cs` dòng 14570 / 14092 / 14808 / 15031).
/// 🔴 Cụm này có **hằng trạng thái RIÊNG** `TConst.MRKScopLimitStatus` (chú ý tên hằng **thiếu chữ e**:
/// "ScopLimit"), chỉ hai giá trị: `Pending = "P"` · `Approve = "A"` — **không dùng `TConst.Stage`**
/// như cụm chi phí marketing (#106-#108), nên không có "R"/"F"/"M" ở đây.
/// 🔴 `Save` là **XOÁ TRẮNG rồi INSERT lại** cả phần đầu lẫn chi tiết; nếu bản ghi đã tồn tại thì
/// **phải đang "P"**, và `CreatedDateTime`/`CreatedBy` gốc được **giữ nguyên**, không bị đặt lại.
/// 🔴 `Approve` cập nhật **CẢ HAI bảng**: `MRKScopeLimitStatus` ở phần đầu và đồng bộ xuống
/// `MRKScopeLimitStatusDetail` của từng dòng.
/// </summary>
public sealed class MrkScopeLimit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKScopeLimitNo { get; set; } = "";
    public string MRKScopeLimitYear { get; set; } = "";
    /// <summary>"P" chờ duyệt / "A" đã duyệt (TConst.MRKScopLimitStatus).</summary>
    public string MRKScopeLimitStatus { get; set; } = "P";
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// DÒNG hạn mức ngân sách marketing theo đại lý (`MRK_ScopeLimitDetail`).
/// Khoá dòng = cặp (`MRKScopeLimitNo`, `DealerCode`).
/// 🔴 **Bất đối xứng có chủ đích của nghiệp vụ: 4 QUÝ hạn mức nhưng 6 ĐỢT giải ngân**
/// (`Amount1..4QuaterScopeLimit` vs `Amount1..6DisbursmentCash`) — không phải lỗi đánh máy, đừng "sửa"
/// thành 4/4. Tên cột giữ nguyên chính tả nguồn: **`Quater`** (đúng phải là Quarter) và
/// **`Disbursment`** (đúng phải là Disbursement).
/// 🔴 Guard nguồn: **cả 10 số tiền đều không được âm** (một điều kiện `||` gộp, một mã lỗi chung
/// `MRK_ScopeLimit_Save_InvalidAmount`).
/// </summary>
public sealed class MrkScopeLimitDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MRKScopeLimitNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>Hạn mức quý 1.</summary>
    public decimal Amount1QuaterScopeLimit { get; set; }
    /// <summary>Hạn mức quý 2.</summary>
    public decimal Amount2QuaterScopeLimit { get; set; }
    /// <summary>Hạn mức quý 3.</summary>
    public decimal Amount3QuaterScopeLimit { get; set; }
    /// <summary>Hạn mức quý 4.</summary>
    public decimal Amount4QuaterScopeLimit { get; set; }
    /// <summary>Tiền giải ngân đợt 1.</summary>
    public decimal Amount1DisbursmentCash { get; set; }
    /// <summary>Tiền giải ngân đợt 2.</summary>
    public decimal Amount2DisbursmentCash { get; set; }
    /// <summary>Tiền giải ngân đợt 3.</summary>
    public decimal Amount3DisbursmentCash { get; set; }
    /// <summary>Tiền giải ngân đợt 4.</summary>
    public decimal Amount4DisbursmentCash { get; set; }
    /// <summary>Tiền giải ngân đợt 5.</summary>
    public decimal Amount5DisbursmentCash { get; set; }
    /// <summary>Tiền giải ngân đợt 6.</summary>
    public decimal Amount6DisbursmentCash { get; set; }
    /// <summary>Đồng bộ từ phần đầu khi duyệt.</summary>
    public string MRKScopeLimitStatusDetail { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// FILE ĐÍNH KÈM của dòng chi phí marketing (`MKT_MarketingFeeDetailAttach` — port 1:1 cụm 4 hàm
/// `MKT_MarketingFeeDetailAttachGet/Save/Approved/Rejected_New20181115`, 2010.HTC
/// `BizHTC.Marketing.cs` dòng 4990 / 5169 / 5603 / 5888).
/// Khoá dòng = bộ **BỐN**: (`MKTFeeCode`, `MKTActivityCode`, `FileAttachType`, `Idx`) — nguồn dựng
/// chuỗi `"|{0}||{1}||{2}||{3}|"` để bắt trùng ngay trong bảng đầu vào.
/// 🔴 `FileAttachType` theo `TConst.MKTFileAttachType`: **DESIGNIMAGE · ACTUALIMAGE · CONTRACT · INVOICE**.
/// 🔴 `Save` **XOÁ rồi INSERT lại theo TỪNG LOẠI hồ sơ** (`delete … where MKTFeeCode + MKTActivityCode
/// + FileAttachType`) — nộp lại ảnh thiết kế không đụng gì tới hợp đồng hay hoá đơn.
/// </summary>
public sealed class MktFeeDetailAttach
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MKTFeeCode { get; set; } = "";
    public string MKTActivityCode { get; set; } = "";
    /// <summary>DESIGNIMAGE | ACTUALIMAGE | CONTRACT | INVOICE.</summary>
    public string FileAttachType { get; set; } = "";
    /// <summary>Số thứ tự file trong cùng một loại hồ sơ.</summary>
    public int Idx { get; set; }
    public string FilePath { get; set; } = "";
    public string? FileDesc { get; set; }
    public string? FileType { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// PHIẾU chi phí marketing — phần đầu (`MKT_MarketingFee` — port 1:1 cụm 7 hàm
/// `MKT_MarketingFeeGet/Create/Update/Finished/Delete/ForceDelete/Approve_New20181115`, 2010.HTC
/// `TERP.BizHTC/BizHTC.Marketing.cs` dòng 6764 / 6259 / 6582 / 2981 / 7037 / 7235 / 7395).
/// 🔴 Vòng đời `MKTStatus` (`TConst.Stage`): **"P"** tạo → **"A"** duyệt / **"R"** từ chối → **"F"** kết thúc.
/// 🔴 **HAI cách xoá khác hẳn nhau:**
/// · `Delete` chỉ khi phiếu còn **"P"**, và **chặn nếu có dòng chi tiết đã "A"**;
/// · `ForceDelete` ngược lại — chỉ khi phiếu **đã "A"**, có RBAC `CheckHTCDirect`, và **xoá dây chuyền**
///   `MKT_MarketingFeeDetailAttach` + `MKT_MarketingFeeDetail` trước rồi mới xoá phần đầu.
/// (Câu xoá `MKT_MarketingFeeAttach` trong nguồn đang **bị comment** — chưa port, ghi nợ.)
/// 🔴 `Finished` đòi phiếu đang "A", mọi dòng chi tiết đã chốt, và với mỗi dòng "A": ba cờ hồ sơ của
/// hoạt động (`FlagDesignImage/FlagActualImage/FlagContract` = "1") thì trạng thái hồ sơ tương ứng phải "A";
/// riêng **`StatusInvoice` KHÔNG có cờ gate — LUÔN bắt buộc "A"**.
/// ⚠️ Xem `### C0-bug6` trong sổ nâng cấp: guard "A hoặc R" của `Finished` viết `!= A || != R` (luôn đúng).
/// </summary>
public sealed class MktFee
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MKTFeeCode { get; set; } = "";
    public string MKTFeeName { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string? Remark { get; set; }
    /// <summary>"P" → "A"/"R" → "F".</summary>
    public string MKTStatus { get; set; } = "P";
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// DÒNG chi phí marketing (`MKT_MarketingFeeDetail`). Khoá dòng = cặp (`MKTFeeCode`, `MKTActivityCode`) —
/// nguồn dựng khoá `"|{MKTFeeCode}||{MKTActivityCode}|"` để bắt trùng ngay trong bảng đầu vào.
/// 🔴 Guard nguồn khi tạo: `Qty` **không null và > 0**, `Price` **không null và > 0.0**, và
/// `MKTActivityCode` phải là hoạt động **đang Active** (`Mst_MarketingActivity_CheckDB` với Flag.Active).
/// Bốn cột trạng thái hồ sơ (`StatusDesignImage/ActualImage/Contract/Invoice`) do cụm
/// `MKT_MarketingFeeDetail*` ghi — **chưa port**, ở đây khai báo sẵn vì `Finished` phải ĐỌC chúng.
/// </summary>
public sealed class MktFeeDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MKTFeeCode { get; set; } = "";
    public string MKTActivityCode { get; set; } = "";
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public string? Remark { get; set; }
    /// <summary>"P" → "A"/"R" (TConst.Stage).</summary>
    public string MKTFeeDetailStatus { get; set; } = "P";
    public string? StatusDesignImage { get; set; }
    public string? StatusActualImage { get; set; }
    public string? StatusContract { get; set; }
    public string? StatusInvoice { get; set; }
    /// <summary>Tổng tiền HTC hỗ trợ (nguồn viết thiếu chữ p: `TotalHTCSuport`).</summary>
    public decimal? TotalHTCSuport { get; set; }
    /// <summary>Ngày chốt dòng — nguồn dùng CHUNG cho cả duyệt lẫn từ chối.</summary>
    public DateTime? ApprovedDetailDate { get; set; }
    /// <summary>Người chốt dòng — cũng dùng chung cho duyệt lẫn từ chối.</summary>
    public string? ApprovedDetailBy { get; set; }
    /// <summary>Ghi chú của ĐẠI LÝ khi nộp ảnh thiết kế.</summary>
    public string? RemarkDlrDesignImage { get; set; }
    /// <summary>Ghi chú của HTC khi duyệt/trả lại ảnh thiết kế.</summary>
    public string? RemarkHTCDesignImage { get; set; }
    public DateTime? ApprovedDateDesignImage { get; set; }
    public string? ApprovedByDesignImage { get; set; }
    /// <summary>Ghi chú của ĐẠI LÝ khi nộp ảnh thực tế.</summary>
    public string? RemarkDlrActualImage { get; set; }
    /// <summary>Ghi chú của HTC khi duyệt/trả lại ảnh thực tế.</summary>
    public string? RemarkHTCActualImage { get; set; }
    public DateTime? ApprovedDateActualImage { get; set; }
    public string? ApprovedByActualImage { get; set; }
    /// <summary>Ghi chú của ĐẠI LÝ khi nộp hợp đồng.</summary>
    public string? RemarkDlrContract { get; set; }
    /// <summary>Ghi chú của HTC khi duyệt/trả lại hợp đồng.</summary>
    public string? RemarkHTCContract { get; set; }
    public DateTime? ApprovedDateContract { get; set; }
    public string? ApprovedByContract { get; set; }
    /// <summary>Ghi chú của ĐẠI LÝ khi nộp hoá đơn.</summary>
    public string? RemarkDlrInvoice { get; set; }
    /// <summary>Ghi chú của HTC khi duyệt/trả lại hoá đơn.</summary>
    public string? RemarkHTCInvoice { get; set; }
    public DateTime? ApprovedDateInvoice { get; set; }
    public string? ApprovedByInvoice { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Master **LOẠI hoạt động marketing** (`Mst_MarketingActivityType` — port 1:1 cụm 4 hàm
/// `Mst_MarketingActivityTypeGet/_Create/_Update/_Delete_New20181115`, 2010.HTC
/// `TERP.BizHTC/BizHTC.Marketing.cs` dòng 90 / 260 / 416 / 579).
/// 🔴 `Delete` của nguồn là **XOÁ THẬT** (`DataRow.Delete()` rồi `SaveData`), **không** phải hạ
/// `FlagActive` — và nguồn **không kiểm tra** loại này còn hoạt động nào đang dùng hay không.
/// 🔴 `FlagActive` theo `TConst.Flag`: **"1"** = Active, **"0"** = Inactive (KHÔNG phải "Y"/"N").
/// ⚠️ RBAC nguồn `myCommon_CheckHTCDirect` (chỉ HTC trực tiếp được ghi) — nợ chung toàn fleet.
/// </summary>
public sealed class MktActivityType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MKTActivityTypeCode { get; set; } = "";
    public string MKTActivityTypeName { get; set; } = "";
    /// <summary>"1" = Active, "0" = Inactive.</summary>
    public string FlagActive { get; set; } = "1";
    public string? Remark { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Master **HOẠT ĐỘNG marketing** (`Mst_MarketingActivity` — port 1:1 cụm 4 hàm
/// `Mst_MarketingActivityGet/_Create/_Update/_Delete`, 2010.HTC `BizHTC.Marketing.cs`
/// dòng 766 / 977 / 1164 / 1359). Lưu ý hàm Update của nguồn viết thường chữ n:
/// `Mst_MarketingActivity_Update_new20181115`.
/// 🔴 Guard FK: `MKTActivityTypeCode` **phải tồn tại** trong `Mst_MarketingActivityType`
/// (`Mst_MarketingActivityType_CheckDB` với `FlagExistToCheck = Flag.Yes`) — kiểm ở **cả Create lẫn Update**.
/// 🔴 Ba cờ hồ sơ bắt buộc kèm theo hoạt động: `FlagDesignImage` (ảnh thiết kế) · `FlagActualImage`
/// (ảnh thực tế) · `FlagContract` (hợp đồng) — giá trị "1"/"0".
/// 🔴 `Delete` cũng là **XOÁ THẬT**, giống loại hoạt động.
/// </summary>
public sealed class MktActivity
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MKTActivityCode { get; set; } = "";
    public string MKTActivityName { get; set; } = "";
    /// <summary>Khoá ngoại sang <see cref="MktActivityType"/>.</summary>
    public string? MKTActivityTypeCode { get; set; }
    /// <summary>"1" = Active, "0" = Inactive.</summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>Có bắt buộc ảnh thiết kế không ("1"/"0").</summary>
    public string? FlagDesignImage { get; set; }
    /// <summary>Có bắt buộc ảnh thực tế không ("1"/"0").</summary>
    public string? FlagActualImage { get; set; }
    /// <summary>Có bắt buộc hợp đồng không ("1"/"0").</summary>
    public string? FlagContract { get; set; }
    public string? Remark { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Hạn mức & thanh toán marketing theo NĂM của từng đại lý (`Rpt_Marketing` — port 1:1 cụm 5 hàm
/// `Rpt_MarketingGet/Create/UpdateMulti/Update/Approve_New20181115`, 2010.HTC
/// `TERP.BizHTC/BizHTC.Marketing.cs` dòng 1617 / 1811 / 2182 / 2567 / 2822).
/// Khoá nghiệp vụ = cặp (`MKTYear`, `DealerCode`); bốn quý `Limit1..4` (hạn mức) và `Payment1..4` (đã chi).
/// 🔴 `RptStatus` theo `TConst.Stage`: tạo mới = **"P"** (Pending), duyệt = **"A"** (Approved).
/// Lưu ý `Stage.Modify = "M"` được hằng số ghi rõ **"Chỉ dùng cho Marketing"** nhưng cụm 5 hàm này
/// KHÔNG dùng tới — thuộc các hàm `MKT_MarketingFee*` khác, chưa port.
/// 🔴 **Duyệt theo NĂM, không theo đại lý**: `Rpt_MarketingApprove` chạy
/// `update … set RptStatus='A' where MKTYear=@strMKTYear` — duyệt một phát cả năm.
/// 🔴 **`UpdateMulti` = XOÁ TRẮNG cả năm rồi INSERT lại**, không phải sửa từng dòng; các dòng mới
/// quay về trạng thái "P". Guard: toàn bộ năm phải đang "P".
/// ⚠️ RBAC nguồn: `myCommon_CheckHTCDirect` (chỉ HTC trực tiếp được ghi) + `myCommon_CheckAccessDealerData`
/// theo `BUPattern` — nợ RBAC chung toàn fleet, chưa port.
/// </summary>
public sealed class RptMarketing
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Năm 4 chữ số, nguồn chặn ngoài khoảng 1900..2100.</summary>
    public string MKTYear { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public decimal? Limit1 { get; set; }
    public decimal? Limit2 { get; set; }
    public decimal? Limit3 { get; set; }
    public decimal? Limit4 { get; set; }
    public decimal? Payment1 { get; set; }
    public decimal? Payment2 { get; set; }
    public decimal? Payment3 { get; set; }
    public decimal? Payment4 { get; set; }
    public string? Remark { get; set; }
    /// <summary>"P" = chờ duyệt, "A" = đã duyệt (TConst.Stage).</summary>
    public string RptStatus { get; set; } = "P";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Nhật ký gọi API **SBHOnline** (`OS_SBHOnline_Log` — port 1:1 `OS_SBHOnline_Log_Create`,
/// 2010.HTC `TERP.BizHTC/BizHTC.DealerSales.cs:4433`).
/// 🔴 Hai trục tên KHÁC NHAU, đừng lẫn:
/// · `FuncCall` = **hàm nghiệp vụ ERP** đã kích hoạt lần gọi — `DealerSalesDealCreate` ·
///   `DealerSalesDealUpdateMulti` · `DealerSalesDealDelete`;
/// · `FuncCode` = **lệnh API SBHOnline** được gọi — `fleet_owner_create` · `fleet_owner_update` ·
///   `fleet_create` · `fleet_update` · `fleet_car_id`.
/// 🔴 `ErrCode` **KHÔNG phải mã số**: thành công ghi `"0"`, thất bại ghi **nguyên văn thông điệp lỗi**
/// (`response.errorMessage`, hoặc `ex.Message + "/" + response.errorMessage` khi vỡ deserialize).
/// 🔴 Hàm nguồn **không có guard nào** (region Check để RỖNG) và `catch` nuốt trọn — ở đây
/// `RollbackSafety` cũng bị comment, khác `DSL_LogCarSvCreate` (vẫn rollback).
/// ⚠️ `strFunctionName` trong nguồn ghi nhầm là `"DSL_LogCarSv_Create"` (copy-paste từ hàm kia);
/// vô hại vì `catch` trống nên chuỗi đó không bao giờ được dùng.
/// </summary>
public sealed class SbhOnlineApiLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Hàm nghiệp vụ ERP đã kích hoạt lần gọi.</summary>
    public string? FuncCall { get; set; }
    /// <summary>Lệnh API SBHOnline được gọi.</summary>
    public string? FuncCode { get; set; }
    /// <summary>JSON gửi đi.</summary>
    public string? RQ { get; set; }
    /// <summary>JSON nhận về (khi lỗi là `response.content` thô).</summary>
    public string? RT { get; set; }
    public string? DealNo { get; set; }
    /// <summary>Rỗng ở các lệnh cấp chủ xe (`fleet_owner_*`); có giá trị ở lệnh cấp xe.</summary>
    public string? CarId { get; set; }
    /// <summary>"0" = thành công; khác "0" là THÔNG ĐIỆP lỗi nguyên văn.</summary>
    public string? ErrCode { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Nhật ký đẩy xe đã bán sang **CarService** (`DLS_LogCarSv` — port 1:1 `DSL_LogCarSvCreate`,
/// 2010.HTC `TERP.BizHTC/BizHTC.DealerSales.cs:3974`; hàm đọc `DSL_LogCarSvGet_New20181115` (4104)
/// là bản LIVE mà **cả WS 32-bit lẫn 64-bit** đều gọi).
/// 🔴 Quy ước: `ErrCode = "0"` là **THÀNH CÔNG**; khác "0" là mã lỗi CarService trả về.
/// 🔴 `FuncCode` — nhánh thành công ghi **tên lệnh** đã gọi, nhánh lỗi ghi **PVal** (function code nơi lỗi).
/// Ba lệnh có thật ở các điểm gọi: `SerCustomerCarSalesCreate` · `OS_Ser_CarSalesUpd` · `OS_Ser_CarSalesDelX`.
/// 🔴 Nguồn **NUỐT mọi lỗi**: `catch` chỉ rollback rồi thoát, không ném — hàm ghi log không được phép
/// làm hỏng luồng bán xe. Endpoint port giữ đúng ngữ nghĩa đó (trả `logged=false`, không phải lỗi 4xx).
/// </summary>
public sealed class CarSvLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DealNo { get; set; }
    public string? DealerCode { get; set; }
    public string? CarId { get; set; }
    public string? VIN { get; set; }
    /// <summary>Lệnh CarService đã gọi (thành công) hoặc PVal nơi lỗi.</summary>
    public string? FuncCode { get; set; }
    /// <summary>"0" = thành công; khác "0" = mã lỗi.</summary>
    public string? ErrCode { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>
/// Nhật ký gọi API GPS (`GPS_LogGPS` — port 1:1 `GPS_LogGPS_Add`, 2010.HTC
/// `StorageFG/BizHTC.ConnGPSVelocaDMS.cs:1476`).
/// 🔴 Mỗi lần gọi API ghi **HAI dòng** chung một `LogId`:
/// · `LogType = "RQ"` ghi **trước khi gửi**, `Status` để **rỗng**;
/// · `LogType = "RS"` ghi **sau khi nhận**, `Status` = kết quả trả về, hoặc **"FALSE"** khi lỗi/không nhận được.
/// (`TConst.LogTypeGPS`, `Const.Main.cs:733-737`.)
/// 🔴 `FunctionType` theo `TConst.TypeCallGPS` (724-731): **MAPVIN · OUTSTO · GETADDRESSONLINE ·
/// SEARCHADDRESSS · DMSUNMAPVIN** — lưu ý `SEARCHADDRESSS` **thừa một chữ S nguyên văn nguồn**.
/// </summary>
public sealed class GpsCallLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Khoá nối cặp RQ↔RS của cùng một lần gọi.</summary>
    public string LogId { get; set; } = "";
    /// <summary>"RQ" gửi đi / "RS" nhận về.</summary>
    public string LogType { get; set; } = "RQ";
    /// <summary>Trạng thái trả về; rỗng ở dòng RQ, "FALSE" khi lỗi.</summary>
    public string? Status { get; set; }
    public string? Exception { get; set; }
    public string? DataSend { get; set; }
    public string? DataResponse { get; set; }
    /// <summary>Khoá bản ghi phía DMS (`iDMSKey`) — giữ nguyên tên cột của nguồn.</summary>
    public string? IDMSKey { get; set; }
    public string? FunctionName { get; set; }
    /// <summary>Loại lệnh gọi theo `TConst.TypeCallGPS`.</summary>
    public string? FunctionType { get; set; }
    /// <summary>Số lần thử (`Trycount` — nguồn luôn truyền "1" ở mọi điểm gọi hiện có).</summary>
    public string? Trycount { get; set; }
    public string? Url { get; set; }
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Kế hoạch bán lẻ theo tháng (`Rpt_PlanRetail` — port 1:1 `Rpt_PlanRetail_Create/Approve/Cancel`,
/// 2010.HTC `BizHTC.Report.cs:33001/33644/33900`).
/// 🔴 Trạng thái theo `TConst.PRStatus` (`Const.Main.DMS40.cs:826-831`):
/// **"P" mới tạo · "A" duyệt · "C" từ chối**.
/// ⚠️ Guard phản trực giác: **duyệt vào từ "P" HOẶC "C"** (bản đã từ chối vẫn duyệt lại được),
/// còn **từ chối chỉ vào từ "P"**.
/// 📌 Khoá nghiệp vụ là bộ ba `PlanMonth` + `PlanTimes` (lần lập trong tháng) + `DealerCode`;
/// `PlanTimesPrev` trỏ về lần lập trước để so sánh.
/// ⚠️ Phần SINH DỮ LIỆU của nguồn là một câu SQL tổng hợp rất lớn
/// (`RptSQLQuery.mySql_Rpt_PlanRetail_Create`) — **chưa port**, đã ghi nợ; endpoint tạo ở đây nhận
/// dữ liệu dòng từ client.
/// </summary>
public sealed class PlanRetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PlanMonth { get; set; } = "";
    public string PlanTimes { get; set; } = "";
    public string? PlanTimesPrev { get; set; }
    public string DealerCode { get; set; } = "";
    public string PRStatus { get; set; } = "P";
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? CancelDate { get; set; }
    public string? CancelBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
}

/// <summary>Dòng chi tiết kế hoạch bán lẻ (`Rpt_PlanRetailDtl`).</summary>
public sealed class PlanRetailDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PlanRetailId { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public int Quantity { get; set; }
}

/// <summary>Kế hoạch bán lẻ gộp theo MODEL (`Rpt_PlanRetailModel`) — nguồn ghi cùng lúc với bảng chi tiết.</summary>
public sealed class PlanRetailModel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PlanRetailId { get; set; }
    public string ModelCode { get; set; } = "";
    public int Quantity { get; set; }
}

/// <summary>
/// Lịch sử ĐẨY Sổ Bảo Hành online (`Rpt_PushSBHOnline_History` — port 1:1 `SBHOnline_HistoryCreate`,
/// 2010.HTC `BizHTC.DealerSales.cs:~4310`, được `RePush_SBHOnline` gọi).
/// 📌 Hàm ghi lịch sử nằm ở **file KHÁC** với hàm đẩy, và là `public void` (không phải `DataSet`) —
/// quét ranh giới hàm bằng mẫu chỉ bắt `public DataSet` sẽ gán nhầm sang hàm `DSL_LogCarSvGet` phía trên.
/// ⚠️ Giữ nguyên tên cột `FlagSucsess` **sai chính tả trong nguồn** để đối chiếu dữ liệu được.
/// </summary>
public sealed class SbhOnlinePushHistory
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string CarId { get; set; } = "";
    public string VIN { get; set; } = "";
    public DateTime PushDate { get; set; } = DateTime.Now;
    public string? PushBy { get; set; }
    /// <summary>Đích đẩy (`PushTo`) — hệ thống online nhận dữ liệu.</summary>
    public string? PushTo { get; set; }
    public string? PushStatus { get; set; }
    /// <summary>Cờ thành công (`FlagSucsess` — sai chính tả nguyên văn nguồn): "1" thành công / "0" thất bại.</summary>
    public string FlagSucsess { get; set; } = "0";
}

/// <summary>
/// Lịch sử sửa NGÂN HÀNG tài trợ của giao dịch bán lẻ (`DLS_Deal_UpdateBankCode_His` — port 1:1
/// `Support_DLS_Deal_UpdateBankCode`, 2010.HTC `Biz.HTC.WH.hkt.cs:7848`).
/// 🔴 Guard đặc thù của nguồn: `DLS_Deal.DealerCodeBuyer` **phải RỖNG** — nếu giao dịch là bán cho
/// ĐẠI LÝ khác thì báo lỗi `Support_DLS_Deal_UpdateBankCode_DealerCodeBuyerInvalid`;
/// tức chỉ sửa ngân hàng cho giao dịch **bán khách lẻ**.
/// ✅ Guard mã NH mới phải có trong `Mst_Bank` **và đang hoạt động** ĐÃ port ở #116
/// (master `Mst_Bank` bổ sung ở #115).
/// </summary>
public sealed class DealUpdBankCodeHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string? BankCodeOld { get; set; }
    public string? BankCodeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa SỐ/NGÀY HOÁ ĐƠN KHÁCH của dòng bán lẻ (`Dls_DealDetailCusInvoice_HisUpd` — port 1:1
/// `Dls_DealDetailCusInvoice_Update`, 2010.HTC `Biz.HTC.WH.hkt.cs:7116`).
/// 🔴 Cùng mẫu ba nhánh với <see cref="SalesManUpdDeptSMTypeHis"/>: chỉ ghi cột **thực sự đổi**
/// (cả hai / chỉ số HĐ / chỉ ngày HĐ); giá trị mới để trống ⇒ **giữ nguyên giá trị hiện tại**;
/// nếu **cả hai** đều trống ⇒ **bỏ qua dòng**, không ghi gì.
/// </summary>
public sealed class DealDetailCusInvoiceHisUpd
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string CarId { get; set; } = "";
    public string? CusInvoiceNoOld { get; set; }
    public string? CusInvoiceNoNew { get; set; }
    public DateTime? CusInvoiceDateOld { get; set; }
    public DateTime? CusInvoiceDateNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa PHÒNG BAN / LOẠI NVBH (`Mst_SalesMan_UpdateDepartmentAndSMType_His` — port 1:1
/// `Support_Mst_SalesMan_UpdateDepartmentAndSMType`, 2010.HTC `Biz.HTC.WH.hkt.cs:7470`).
/// 🔴 Nguồn có **BA nhánh update khác nhau** tuỳ trường nào thực sự đổi:
/// cả hai đổi ⇒ ghi `DepartmentCode`+`SMType`; chỉ phòng ban đổi ⇒ chỉ ghi `DepartmentCode`;
/// chỉ loại đổi ⇒ chỉ ghi `SMType`. Giá trị mới để trống ⇒ **giữ nguyên giá trị hiện tại trong DB**
/// (không ghi rỗng đè lên). Lịch sử luôn lưu đủ cả 4 giá trị cũ/mới.
/// </summary>
public sealed class SalesManUpdDeptSMTypeHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SMCode { get; set; } = "";
    public string? DepartmentCodeOld { get; set; }
    public string? DepartmentCodeNew { get; set; }
    public string? SMTypeOld { get; set; }
    public string? SMTypeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Ảnh chụp biên bản giao xe TRƯỚC KHI XOÁ (`Sto_DlvMinutes_HisDel` — port 1:1
/// `Sto_DlvMinutes_DeleteSupport`, 2010.HTC `Biz.HTC.WH.cs:139257`).
/// 🔴 Kiểu lịch sử **KHÁC** họ `*_Upd*_His`: không lưu cặp cũ/mới mà **sao chép TOÀN BỘ dòng**
/// (nguồn chép **69 cột**) sang bảng này rồi mới `delete` khỏi `Sto_DlvMinutes`.
/// ⚠️ MiniHTC chỉ giữ được các cột đang có trên <see cref="TranspDlvConfirm"/> — phần còn lại đã ghi nợ.
/// </summary>
public sealed class DlvMinutesHisDel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlvMinutesNo { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? TransporterCode { get; set; }
    public string? FDlvMnStatus { get; set; }
    public string? TDlvMnStatus { get; set; }
    public string? ConfirmStatus { get; set; }
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }
    public string? FProvinceCode { get; set; }
    public string? FDistrictCode { get; set; }
    public string? TProvinceCode { get; set; }
    public string? TDistrictCode { get; set; }
    public string? Remark { get; set; }
    /// <summary>Số xe trong biên bản lúc xoá (bảng con bị xoá theo, không giữ được từng dòng).</summary>
    public int CarCount { get; set; }
    public DateTime DelDTime { get; set; } = DateTime.Now;
    public string? DelBy { get; set; }
}

/// <summary>
/// PHIẾU HUỶ hợp đồng bán lẻ — phần đầu (`Dlr_ContractCancel` — 2010.HTC `BizHTC.Contract.cs:2976`,
/// trong `Dlr_ContractCancel_SaveX_**New20230306**` (2375); csproj `&lt;Compile&gt;` 110 ⇒ LIVE).
/// 🔴 **Bảng ghi bằng `insert into … select`, KHÔNG qua `SaveData`** ⇒ đây là **mỏ mới** mở ở #136:
/// kiểm kê bằng `grep 'SaveData("…")'` (#117) **không bắt được** nhóm bảng này.
/// 🔴 **TWIN — cụm `Dlr_ContractCancel_*` CHỈ có ở WS 64-bit** (`_Save`, `_Get_New20230306`,
/// `_GetWH_New20230306`, `_ApproveMulti`, `_CancelMulti`); WS 32-bit **chỉ có**
/// `ContractDealerContractCancel_New20181119` — hàm CŨ, gọi `_SaveX` (1726) **không ghi
/// `Dlr_ContractCancelCar`**. Ca thứ NĂM cùng dạng "chỉ 64-bit", và lặp lại đúng mẫu #129:
/// bản mới **ghi thêm cả một bảng**.
/// 🔴 `ContractCancelStatus` theo `TConst.ContractCancelStatus` (`Const.Main.**DMS40**.cs:140-145`):
/// **"P" chờ duyệt · "A" đã duyệt · "C" huỷ** — hằng nằm ở file DMS40, không phải `Const.Main.cs`.
/// </summary>
public sealed class DlrContractCancel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Số phiếu huỷ (khoá nghiệp vụ).</summary>
    public string ContractCNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>"P" → "A" duyệt / "C" huỷ (TConst.ContractCancelStatus, DMS40).</summary>
    public string ContractCancelStatus { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// DÒNG phiếu huỷ theo MODEL (`Dlr_ContractCancelDtl` — `BizHTC.Contract.cs:3010`).
/// Gộp nhóm theo (`SpecCode`, `ModelCode`, `ColorCode`) với `Qty` — **song song với
/// `Dlr_ContractDtl`** của hợp đồng gốc (#129).
/// Có trạng thái DÒNG riêng `ContractCancelDtlStatus`, tách khỏi trạng thái phiếu.
/// </summary>
public sealed class DlrContractCancelDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractCNo { get; set; } = "";
    /// <summary>Hợp đồng gốc bị huỷ.</summary>
    public string DlrContractNo { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? ContractUpdateType { get; set; }
    public decimal? Qty { get; set; }
    public string ContractCancelDtlStatus { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// XE bị huỷ trong phiếu (`Dlr_ContractCancelCar` — `BizHTC.Contract.cs:3042`).
/// 🔴 **CHỈ có ở bản `_SaveX_New20230306`**; bản cũ `_SaveX` (1726) mà WS 32-bit dùng **không ghi bảng này**
/// — đúng mẫu đã gặp ở #129 (`Dlr_ContractCar`).
/// `CtrCarId` trỏ về đúng dòng xe của hợp đồng gốc (<see cref="DlrContractCar"/>, #129) ⇒ **huỷ ở mức
/// TỪNG XE**, không phải huỷ cả nhóm model.
/// `CtrCType` là **loại huỷ** (master `Mst_ContractCancelType`, join tại dòng 1479);
/// `CtrCTDNo` là số chứng từ kèm theo.
/// ⚠️ Nguồn có dòng `drScan["CtrCType"] = null;` (2725) trong một nhánh — loại huỷ **có thể để trống**.
/// </summary>
public sealed class DlrContractCancelCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractCNo { get; set; } = "";
    public string DlrContractNo { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    /// <summary>Trỏ về dòng xe của hợp đồng gốc ("&lt;SốHĐ&gt;.01"…).</summary>
    public string CtrCarId { get; set; } = "";
    public DateTime? DlvExpectedDate { get; set; }
    /// <summary>Loại huỷ (Mst_ContractCancelType) — có thể để trống.</summary>
    public string? CtrCType { get; set; }
    /// <summary>Số chứng từ kèm theo lần huỷ.</summary>
    public string? CtrCTDNo { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// HẠNG MỤC của gói bảo dưỡng (`Mst_MaintainTaskItem` — port 1:1 cụm 4 hàm
/// `Mst_MaintainTaskItem_Create/_Update/_Delete/_Get_New20181119`, 2010.HTC `Biz.HTC.WH.cs:7236`).
/// TWIN: cả WS 32-bit lẫn 64-bit **khớp hoàn toàn** (5/5 hàm, kể cả `Mst_MaintainTask_Get` của bảng cha).
/// Khoá dòng = cặp (`MtnTkCode`, `MtnTkItemCode`) — mã gói bảo dưỡng + mã hạng mục.
/// 🔴 `ViewIdx` là **thứ tự hiển thị** hạng mục trong gói; `FlagActive` nguồn **luôn đặt `Flag.Active`
/// ("1") khi tạo**, không nhận từ đầu vào.
/// ⚠️ Bảng cha `Mst_MaintainTask` chỉ có hàm `_Get` (không có Create/Update/Delete) ⇒ **danh mục gói
/// bảo dưỡng do DBA nạp**, chỉ hạng mục bên trong mới sửa được qua ứng dụng.
/// </summary>
public sealed class MstMaintainTaskItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mã gói bảo dưỡng (bảng cha `Mst_MaintainTask` — chỉ đọc).</summary>
    public string MtnTkCode { get; set; } = "";
    public string MtnTkItemCode { get; set; } = "";
    public string? MtnTkItemName { get; set; }
    /// <summary>Thứ tự hiển thị trong gói.</summary>
    public int? ViewIdx { get; set; }
    public string FlagActive { get; set; } = "1";
}

/// <summary>
/// CẤU HÌNH KỲ BÁO CÁO kế hoạch bán lẻ (`St_SettingRptPlanRetail` — port 1:1
/// `St_SettingRptPlanRetail_Create`, 2010.HTC `BizHTC.Report.cs:35073`).
/// 🔴 Cụm này **chỉ có ở WS 64-bit** (`_biz.St_SettingRptPlanRetail_Create`); 32-bit không có
/// — ca thứ TƯ cùng dạng (sau #131 `Mst_MinInventory`, #132 `DLS_DealAttachFile`, #133 `Mst_BankDealer`).
/// 🔴 **`PlanTimes` TỰ TĂNG, không nhận từ client**: nguồn truy vấn bản ghi mới nhất của cùng
/// `PlanMonth` (order by `PlanTimes desc`), lấy `PlanTimes + 1`; nếu chưa có bản nào thì **"1"**
/// (dòng 35127-35145). ⇒ Mỗi tháng có thể có **nhiều lần chốt kế hoạch**, đánh số 1, 2, 3…
/// `ReportDate` là **ngày chốt số liệu** của lần đó — đây chính là tham số mà báo cáo kế hoạch bán lẻ
/// (`Rpt_PlanRetail`, port ở #100) dùng để biết "chốt theo mốc nào".
/// </summary>
public sealed class StSettingRptPlanRetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Tháng kế hoạch.</summary>
    public string PlanMonth { get; set; } = "";
    /// <summary>Lần chốt thứ mấy trong tháng — server tự tăng, không nhận từ client.</summary>
    public string PlanTimes { get; set; } = "1";
    /// <summary>Ngày chốt số liệu của lần này.</summary>
    public DateTime? ReportDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreateDTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// KẾ HOẠCH SẢN XUẤT THEO NGÀY (`WO_ScheduleDetailDate` — 2010.HTC `BizHTC.WorkOrder.cs:563`,
/// trong `WO_Schedule_Add_New20181115` (174); csproj `&lt;Compile&gt;` dòng 134 ⇒ LIVE).
/// TWIN: cả WS 32-bit lẫn 64-bit **khớp hoàn toàn** (5/5 hàm, đã diff toàn bộ danh sách).
/// 🔴 Đây là **TẦNG THỨ BA** của lịch sản xuất, port cũ mới có hai tầng đầu:
/// · `WO_Schedule` (đầu) → <see cref="WoSchedule"/>;
/// · `WO_ScheduleDetail` (theo model/spec/màu, tổng số lượng) → <see cref="WoScheduleLine"/>;
/// · **`WO_ScheduleDetailDate` (theo TỪNG NGÀY `PlanDate` + `QtyPlan`)** — tầng này **thiếu hẳn**,
///   nên port cũ **không biết kế hoạch rải ra ngày nào**, chỉ biết tổng.
/// 🔴 Guard nguồn: `QtyPlan` **âm ⇒ ném lỗi**; `QtyPlan == 0` ⇒ **`continue`, KHÔNG ghi dòng**
/// (dòng 544) — tức ngày không có kế hoạch thì **không tồn tại dòng**, không phải dòng với số 0.
/// ⚠️ Cả ba bảng của cụm chỉ ghi `_dbMain`, **không có `_dbWH`** (dòng 561-564) — khác đa số cụm khác;
/// ghi chú cho lượt trả nợ `_dbWH` (luật C0-centesimusvigesimusquintus).
/// </summary>
public sealed class WoScheduleDetailDate
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string WorkOrderNo { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    /// <summary>Ngày kế hoạch — mỗi ngày một dòng.</summary>
    public DateTime PlanDate { get; set; }
    /// <summary>Số lượng kế hoạch ngày đó; nguồn KHÔNG ghi dòng khi giá trị = 0.</summary>
    public decimal QtyPlan { get; set; }
}

/// <summary>
/// THIẾT BỊ kèm theo dòng hoá đơn HTC (`VAT_HTCInvoiceDeviceDetail` — 2010.HTC
/// `HDDTIntergration/BizHTC.HDDTIntergration.cs:4140`).
/// Khoá dòng = bộ (`HTCInvoiceCode`, `VIN`, `DeviceCode`): một xe trên hoá đơn có thể kèm nhiều thiết bị.
/// 🔴 `SpecCode` của bảng này **lấy từ cột `ActualSpec` của bảng đầu vào** (dòng 4131), **không phải**
/// từ `SpecCode` — tức lưu **spec THỰC TẾ của xe**, không phải spec khai trên chứng từ.
/// Nếu port theo phản xạ "SpecCode ← SpecCode" sẽ ghi sai dữ liệu.
/// ⚠️ Nguồn ghi cả `_dbMain` lẫn `_dbWH` (4140-4142), dòng `_dbWH` **không bị comment**.
/// </summary>
public sealed class VatHtcInvoiceDeviceDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string HTCInvoiceCode { get; set; } = "";
    public string VIN { get; set; } = "";
    /// <summary>Lấy từ `ActualSpec` của đầu vào — spec THỰC TẾ của xe.</summary>
    public string? SpecCode { get; set; }
    public string? DeviceTypeCode { get; set; }
    public string DeviceCode { get; set; } = "";
    public DateTime? EffectiveDate { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// LỊCH SỬ NGHỈ VIỆC của nhân viên bán hàng (`Mst_SalesManHistoryInactive` — 2010.HTC
/// `Biz.HTC.WH.cs:19236`, ghi bên trong `Mst_SalesMan_Update_New20230306` (18505)).
/// 🔴 **Không có hàm riêng**: bảng chỉ được ghi **như một tác dụng phụ của lệnh SỬA nhân viên** —
/// khi NVBH bị cho nghỉ, nguồn chụp lại nguyên trạng hồ sơ vào đây. Vì vậy tra bằng danh sách WS
/// sẽ **không thấy** cụm nào cho bảng này; phải tra bằng `SaveData("…")`.
/// 🔴 Cột `SMFlagActive` **lấy từ `FlagActive` của bảng nhân viên** (`dtrMS["FlagActive"]`, dòng 19211)
/// — đổi tên khi sang bảng lịch sử; đừng tìm cột `SMFlagActive` ở `Mst_SalesMan`.
/// 🔴 Ba cột do lệnh sửa truyền vào (không chép từ hồ sơ): `IdentityCardNo`, `SMEndDate`,
/// `SMReason`, `SMDesc` — tức **lý do và ngày nghỉ là dữ liệu MỚI**, phần còn lại là ảnh chụp hồ sơ cũ.
/// ⚠️ Nguồn ghi `_dbMain` (19236) và `_dbWH` (19259) nhưng **bằng HAI DataTable khác nhau**
/// (`dt_…` vs `dtDB_…_Main`) — ghi chú lại cho lượt trả nợ `_dbWH`.
/// </summary>
public sealed class SalesManHistoryInactive
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SMCode { get; set; } = "";
    public string? SMHyundaiCode { get; set; }
    public string? DealerCode { get; set; }
    public string? SMStatus { get; set; }
    /// <summary>Số CMND/CCCD — do lệnh sửa truyền vào, không chép từ hồ sơ.</summary>
    public string? IdentityCardNo { get; set; }
    /// <summary>Chép từ `Mst_SalesMan.FlagActive` (đổi tên khi sang bảng lịch sử).</summary>
    public string? SMFlagActive { get; set; }
    public DateTime? SMStartDate { get; set; }
    /// <summary>Ngày nghỉ — dữ liệu MỚI của lần cho nghỉ này.</summary>
    public DateTime? SMEndDate { get; set; }
    /// <summary>Lý do nghỉ — dữ liệu MỚI.</summary>
    public string? SMReason { get; set; }
    /// <summary>Diễn giải thêm — dữ liệu MỚI.</summary>
    public string? SMDesc { get; set; }
    public DateTime InactiveDateTime { get; set; } = DateTime.Now;
    public string? InactiveBy { get; set; }
}

/// <summary>
/// FILE ĐÍNH KÈM của giao dịch bán lẻ (`DLS_DealAttachFile` — 2010.HTC `Biz.HTC.WH.cs:94717`,
/// hàm `DealerSalesDealUpdateAttachFileMulti`; hàm đọc `OSHCC_Dls_DealAttachFileGet`).
/// 🔴 Cụm này **CHỈ có ở WS 64-bit** — `TERP.WSHTC` (32-bit) không có hàm nào
/// (cùng ca với `Mst_MinInventory` ở #131, luật C0-centesimustricesimusprimus).
/// Khoá dòng = cặp (`DealNo`, `FileIndex`); `UpdateMulti` nhận **bảng nhiều dòng** trong một lệnh.
/// ⚠️ Tiền tố cột là **`Dls…`** (`DlsFilePath`/`DlsFileName`/`DlsFileType`/`DlsRemark`) —
/// mỗi bảng đính kèm trong hệ dùng một tiền tố riêng, xem hai lớp dưới.
/// </summary>
public sealed class DlsDealAttachFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public int FileIndex { get; set; }
    public string? DlsFilePath { get; set; }
    public string? DlsFileName { get; set; }
    public string? DlsFileType { get; set; }
    public string? DlsRemark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// FILE ĐÍNH KÈM của thư bảo lãnh (`Pmt_GuaranteeAttachFile` — 2010.HTC
/// `TCFIntergration/BizHTC.TCFIntergration.cs:1403`, csproj `&lt;Compile&gt;` dòng 328 ⇒ LIVE).
/// Khoá dòng = cặp (`GuaranteeNo`, `FileIndex`).
/// 🔴 Bảng này có **`FileSizeInBytes`** — hai bảng đính kèm anh em KHÔNG có cột này; đây là luồng
/// tích hợp TCF (công ty tài chính) nên dung lượng file được ghi lại.
/// ⚠️ Tiền tố cột là **`Grt…`** (`GrtFilePath`/`GrtFileName`/`GrtFileRemark`) — khác `Dls…` và `BkTrans…`.
/// ⚠️ Không WS nào gọi trực tiếp: bảng được ghi **bên trong luồng tích hợp TCF**.
/// </summary>
public sealed class PmtGuaranteeAttachFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GuaranteeNo { get; set; } = "";
    public int FileIndex { get; set; }
    public string? GrtFilePath { get; set; }
    public string? GrtFileName { get; set; }
    /// <summary>Dung lượng file — chỉ bảng này có (luồng TCF).</summary>
    public long? FileSizeInBytes { get; set; }
    public string? GrtFileRemark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// FILE ĐÍNH KÈM của giao dịch ngân hàng (`RQ_BankingTransAttachFile` — 2010.HTC
/// `BankIntergration/BizHTC.VietinBank.cs:1125`, csproj `&lt;Compile&gt;` dòng 311 ⇒ LIVE).
/// Khoá dòng = cặp (`RQ_BankingTransNo`, `FileIndex`); danh sách cột lấy từ
/// `MyBuildDBDT_Common("#input_RQ_BankingTransAttachFile", …)` (dòng 7008-7019).
/// 🔴 Hai cờ riêng của bảng này: `FlagPush` — nguồn **đặt `TConst.Flag.Active` ("1") cho MỌI dòng
/// ngay trước khi lưu** (dòng 1119-1121), tức đánh dấu **đã đẩy sang ngân hàng**; và `FlagDlrCtr`
/// (file thuộc hợp đồng đại lý hay không).
/// ⚠️ Tiền tố cột là **`BkTrans…`** (`BkTransFileType`/`BkTransFilePath`/`BkTransFileName`).
/// ⚠️ Không WS nào gọi trực tiếp: ghi bên trong luồng tích hợp VietinBank.
/// </summary>
// ================= 12 BẢNG VỆ TINH của đề nghị GD ngân hàng (#137) =================
// Nguồn: BizHTC.VietinBank.cs — `RQ_BankingTransactions_SaveX_20220817` (7638), csproj <Compile> 311.
// 🔴 TWIN: `RQ_BankingTransactions_SaveX` (5196) là bản CHẾT (không ai gọi; caller duy nhất ở 5129 gọi
//    bản _20220817). Bản chết ghi 10 bảng, bản LIVE ghi **14** — 4 bảng LC (GrtLC/GrtLCDtl/PmtLC/PmtLCDtl)
//    CHỈ CÓ ở bản LIVE ⇒ lặp lần thứ 3 mẫu "bản mới ghi thêm cả một nhóm bảng" (#129, #136).
// Cấu trúc: 1 đề nghị (RQ_BankingTransactions) → 3 nhánh nghiệp vụ, mỗi nhánh 1 bảng "đầu" + 1 bảng chi tiết:
//    Pmt (giải ngân) · Grt (bảo lãnh) · Wrt (bảo đảm/tài sản) — và bản LIVE thêm 2 nhánh L/C: PmtLC, GrtLC.
//    Ctr / WrtCtr = danh sách HỢP ĐỒNG đại lý gắn vào đề nghị (WrtCtr có thêm LTV + GrtValue).

/// <summary>Nhánh GIẢI NGÂN của đề nghị (`RQ_BankingTransPmt`).</summary>
// ================= 4 HỌ PHIẾU THANH TOÁN DỊCH VỤ THEO XE (#139) =================
// Nguồn: DMS40/0.34.Contract.cs (csproj <Compile> 125) — md5 e2f3680f… khớp cả 2 máy.
// 🔴 TWIN: toàn bộ cụm `Pmt_Payment{GPS,AVN,Storage,PDI}_*` **CHỈ có ở WS 64-bit**; WS 32-bit
//    KHÔNG có một hàm nào ⇒ ca "chỉ 64-bit" thứ SÁU (#131, #132, #133, #135, #136, #139).
//
// Bốn họ dùng CHUNG một máy trạng thái ba trục (xem `*_CheckDB` — mỗi tham số
// `str*StatusListToCheck` là một trục, luật C0-centesimustricesimusnonus):
//    DocStatus  : "P" → "A1" (duyệt 1) → "A2" (duyệt 2) → "F" (hoàn tất) · "R" từ chối · "C" huỷ
//    TCMSSignStatus / HTVSignStatus : bảng mã riêng (N/P/C/A/A1/A2/F/R/D)
// 🔴 THỨ TỰ KÝ BẮT BUỘC: **TCMS ký TRƯỚC, HTV ký SAU** — guard của `*_HTVApproveAndSign` đòi
//    TCMSSignStatus = "A" (còn `*_TCMSApproveAndSign` chỉ đòi cả hai còn "P").
//    Và **chỉ HTV ký mới đóng phiếu** (đặt DocStatus = "F"); TCMS ký không đổi DocStatus.
// 🔴 Huỷ (`*_Cancel`) chỉ được khi phiếu còn "P" — đã duyệt 1 là KHÔNG huỷ được nữa.
//
// ⚠️ BỐN HỌ GẦN GIỐNG NHƯNG KHÔNG ĐỒNG NHẤT — chỗ dễ port ẩu nhất:
//    · GPS/Storage có `VAT`; **AVN KHÔNG có VAT**.
//    · PDI đặt tên KHÁC HẲN: `PmtPDINo` (không phải PaymentPDINo), `CreateDTime` (không phải
//      CreateDateTime), `Appr1DTime/Appr1By` (không phải App1DTime/App1By), `PmtPDIStatusDtl`
//      (không phải …DtlStatus); và có thêm `LUDateTime/LUBy`, `HTVSignBy/TCMSSignBy`,
//      `AmountVAT`, `TotalAmountAfterVAT` thay cho cặp `AmountTotal`+`VAT`.

/// <summary>Phiếu thanh toán phí thiết bị GPS theo tháng (`Pmt_PaymentGPS`).</summary>
// ========== CHỤP DỮ LIỆU NHÂN SỰ BÁN HÀNG THEO THÁNG (#141) ==========
// Nguồn SQL: `SQLQuery.RptSQLQuery.mySql_HR_SalesManOfMonth_ApprAuto()` (RptSQLQuery.cs:15105),
// gọi từ `HR_SalesManOfMonth_ApprAuto_New20221026` (DataWH/Biz.HTC.WH.cs:17214, csproj 272).
//
// 🔴 TWIN — ca "WS gọi vào FILE CHẾT", lần thứ BA (sau #127, #128) và nặng nhất:
//    · `_ApprAuto_New20181119` được **CẢ HAI** WS 32-bit và 64-bit gọi, nhưng thân hàm CHỈ tồn tại ở
//      `DataWH/Delete.Biz.HTC.WH.My.cs` — file khai báo `<None>` trong csproj ⇒ **KHÔNG được build**.
//    · Bản thật duy nhất là `_ApprAuto_New20221026`, và **chỉ WS 64-bit** gọi.
//    · Thêm một lớp bẫy: `BizHTC.zzzzCode.cs` (csproj 141, file SỐNG) có `HR_SalesManOfMonth_Save`
//      + `_ApprAuto_New20181115` cũng ghi đúng 2 bảng này — nhưng **không WS nào gọi** ⇒ xác.
//    ⇒ Đọc nhầm bất kỳ nhánh nào trong ba nhánh trên đều ra bộ cột THIẾU.
//
// Bản chất: **ảnh chụp (snapshot) toàn bộ nhân viên bán hàng theo THÁNG**, không phải chứng từ.
// Bảng đầu = 2 con số đếm/đại lý; bảng chi tiết = bản sao hồ sơ từng nhân viên tại thời điểm chốt.
// Ba đợt nâng cấp đọc được ngay trong comment SQL: 20221026 (thêm mã Hyundai, CCCD, thâm niên,
// chứng chỉ, HTA, BĐH, xếp loại) và **20260316** (thêm dấu vết sửa trạng thái + chế tài vi phạm).

/// <summary>Chốt tháng nhân sự bán hàng theo ĐẠI LÝ (`HR_SalesManOfMonth`).</summary>
// ========== KẾ HOẠCH ĐẶT HÀNG GỬI NHÀ MÁY + QUY ĐỔI SPEC MỚI (#144) ==========
// Nguồn: DMS40/zTemp.0.30.Order.cs (csproj <Compile> **128**) — `Ord_OrderPlan_HTMV_Create` (9461)
//        DMS40/0.01.Master.cs      (csproj <Compile> **122**) — `Mst_ATMV_NewSpec_Add` (512)
// md5 2d05c7d2… / c690f1f1… — khớp nguyên file trên CẢ HAI máy.
//
// 🔴 TWIN: cả HAI cụm **chỉ có ở WS 64-bit** (`TERP.WSHTC.64/WSHTC.asmx.cs`); WS 32-bit
//    (`TERP.WSHTC/App_Code/WSHTC.cs` — mã thật của bit 32 nằm trong **App_Code**, không ở gốc project)
//    KHÔNG có hàm nào ⇒ ca "chỉ 64-bit" thứ **BẢY**.
// ⚠️ Bẫy phụ đã tránh: hai file `TERP.WSHTC.64/BK.WSHTC/WSHTC.asmx.20210208.cs` và
//    `WSHTC.asmx - Copy.cs` cũng chứa các hàm này nhưng KHÔNG có trong csproj ⇒ bản sao lưu.

/// <summary>
/// Kế hoạch đặt hàng gửi nhà máy HTMV (`Ord_OrderPlan_HTMV`) — bảng đầu.
/// 🔴 Nguồn **TỰ SINH** kế hoạch: `Ord_OrderPlan_HTMV_Create` chỉ nhận đúng một tham số nghiệp vụ
/// (`strFlagIsMonth`) rồi tính toàn bộ số liệu từ BO / tồn kho / đơn hàng và ghi cả header lẫn chi tiết.
/// Số phiếu lấy từ bộ sinh mã `TConst.SequenceTypeDMS40.ORPNo`.
/// </summary>
// ========== DANH MỤC MÀU XE + SPEC ÁP DỤNG (#145) ==========
// Nguồn: BizHTC.MasterData.cs (csproj <Compile> **115**, md5 01910ed5… khớp nguyên file 2 máy)
//   `Mst_CarColor_Save` (3382) → `_SaveX` (3552), ghi 2 bảng tại 3974 / 4022.
//   Hàm đọc `Mst_CarColor_Get_New20181119` KHÔNG nằm cùng file mà ở `DataWH/Biz.HTC.WH.cs:2448`
//   (luật C0-centesimusquadragesimusseptimus: đừng khoanh vùng đọc theo file).
//
// 🔴 TWIN dạng "ĐỌC ở cả hai, GHI chỉ 64-bit": WS 32-bit chỉ gọi `_Get_New20181119`;
//    WS 64-bit gọi thêm `_Save` và `Mst_CarColorSpec_Get`. Tức đại lý (bit 32) chỉ TRA CỨU màu,
//    còn sửa danh mục là việc của bản 64-bit.
//
// Khoá nghiệp vụ là CẶP (ModelCode, ColorCode) — không phải một mã màu đơn lẻ:
// cùng mã màu ở hai dòng xe khác nhau là hai bản ghi khác nhau.

/// <summary>Danh mục màu xe theo dòng xe (`Mst_CarColor`) — khoá kép (ModelCode, ColorCode).</summary>
public sealed class MstCarColor
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    /// <summary>Loại màu NGOẠI thất (`ColorExtType`).</summary>
    public string? ColorExtType { get; set; }
    public string? ColorExtCode { get; set; }
    public string? ColorExtName { get; set; }
    /// <summary>Tên màu ngoại thất tiếng Việt (`ColorExtNameVN`) — nguồn lưu song ngữ.</summary>
    public string? ColorExtNameVN { get; set; }
    public string? ColorIntCode { get; set; }
    public string? ColorIntName { get; set; }
    public string? ColorIntNameVN { get; set; }
    /// <summary>
    /// Phụ phí màu (`ColorFee`). ⚠️ Nguồn đọc bằng `Convert.ToDouble` ⇒ là SỐ TIỀN cộng thêm
    /// cho màu đặc biệt, không phải tỉ lệ.
    /// </summary>
    public decimal ColorFee { get; set; }
    /// <summary>Cờ hiệu lực (`FlagActive`) — "1"/"0", chuẩn hoá qua `StandardizeFlag`.</summary>
    public string FlagActive { get; set; } = "1";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Spec áp dụng cho một màu (`Mst_CarColorSpec`) — bảng nối 3 khoá, KHÔNG có cột nghiệp vụ nào khác.
/// Nguồn khi lưu **xoá sạch spec của cặp (ModelCode, ColorCode) rồi ghi lại** danh sách mới.
/// </summary>
public sealed class MstCarColorSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class OrdOrderPlanHtmv
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderPlanNo { get; set; } = "";
    /// <summary>Kỳ kế hoạch (`PeriodDate`).</summary>
    public DateTime? PeriodDate { get; set; }
    /// <summary>
    /// Cờ kỳ tính theo THÁNG (`FlagIsMonth`) — "1"/"0" (luật `dmssales-flag-values-1-0-not-yn`).
    /// Đây là **tham số nghiệp vụ DUY NHẤT** của hàm sinh kế hoạch.
    /// </summary>
    public string? FlagIsMonth { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    /// <summary>Mốc sửa gần nhất (`UpdateDTime`/`UpdateBy`) — `_Update` chỉ sửa số, không xoá-ghi lại.</summary>
    public DateTime? UpdateDTime { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Dòng kế hoạch theo SPEC (`Ord_OrderPlan_HTMVDetail`) — **8 loại số lượng** đặt cạnh nhau
/// để người duyệt so sánh trước khi chốt số gửi nhà máy.
/// </summary>
public sealed class OrdOrderPlanHtmvDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderPlanNo { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string? ModelCode { get; set; }
    /// <summary>Số lượng back-order ĐÃ DUYỆT (`QtyBOApp`).</summary>
    public decimal QtyBOApp { get; set; }
    /// <summary>Số lượng đơn hàng đang chờ (`QtySalesOrderP`).</summary>
    public decimal QtySalesOrderP { get; set; }
    /// <summary>Tồn kho HTC (`QtyStock`).</summary>
    public decimal QtyStock { get; set; }
    /// <summary>Back-order phía nhà máy (`QtyBOHTMV`).</summary>
    public decimal QtyBOHTMV { get; set; }
    /// <summary>Tồn kho ĐẠI LÝ (`QtyStockDealer`) — tách riêng khỏi tồn HTC.</summary>
    public decimal QtyStockDealer { get; set; }
    public decimal QtySalesOrderPlan { get; set; }
    public decimal QtySalesOrder { get; set; }
    /// <summary>Số lượng nhà máy DUYỆT (`QtyHTMVApp`) — con số chốt cuối cùng.</summary>
    public decimal QtyHTMVApp { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Bộ quy đổi spec mới ATMV (`Mst_ATMV_NewSpec`) — bảng đầu: chỉ mã + cờ hiệu lực.
/// Nguồn khi thêm mới bắt buộc mã **CHƯA tồn tại** (`CheckDB(…, Flag.Inactive)`) và đặt `FlagActive = "1"`.
/// </summary>
public sealed class MstAtmvNewSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ATMVNSCode { get; set; } = "";
    public DateTime CreateDTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    /// <summary>Cờ hiệu lực (`FlagActive`) — "1"/"0"; nguồn tạo luôn ở "1".</summary>
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Dòng quy đổi (`Mst_ATMV_NewSpecDtl`): mỗi spec có **khoảng hiệu lực riêng** + số lượng quy đổi.
/// 🔴 Nguồn bắt buộc CẢ HAI mốc `EffDateStart`/`EffDateEnd` không được rỗng (0.01.Master.cs:177…),
/// và chuẩn hoá bằng `StdDate` — tức lưu **NGÀY**, không kèm giờ.
/// </summary>
public sealed class MstAtmvNewSpecDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ATMVNSCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    /// <summary>Số lượng quy đổi (`QtyMap`).</summary>
    public decimal QtyMap { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class HrSalesManOfMonth
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    /// <summary>Tháng chốt (`HRMonth`) — nguồn lấy ngày ĐẦU tháng (`HRMonthStart`).</summary>
    public DateTime? HRMonth { get; set; }
    /// <summary>
    /// Số nhân viên ĐANG làm việc. ⚠️ Nguồn khai kiểu số thực (`IsNull(…, 0.0)`) dù là đếm người —
    /// giữ `decimal` để port 1:1, không tự đổi sang int.
    /// </summary>
    public decimal QtySMWorking { get; set; }
    /// <summary>Số nhân viên NGHỈ việc trong tháng.</summary>
    public decimal QtySMNoWorking { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Ảnh chụp hồ sơ TỪNG nhân viên bán hàng tại tháng chốt (`HR_SalesManOfMonthDtl`, 47 cột).
/// Đây là bản SAO tại thời điểm chốt — cố ý trùng cột với `Mst_SalesMan`, KHÔNG join động.
/// </summary>
public sealed class HrSalesManOfMonthDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public DateTime? HRMonth { get; set; }
    public string DealerCode { get; set; } = "";
    public string SMCode { get; set; } = "";
    public string? SMName { get; set; }
    public string? SMGender { get; set; }
    public DateTime? SMDateOfBirth { get; set; }
    public string? SMPhoneNo { get; set; }
    public string? SMEmail { get; set; }
    public string? SMAddress { get; set; }
    public string? ProvinceCode { get; set; }
    public string? QualificationCode { get; set; }
    public string? SMSpecialized { get; set; }
    public decimal SMYearExperence { get; set; }
    public DateTime? SMStartDate { get; set; }
    public DateTime? SMEndDate { get; set; }
    public string? DepartmentCode { get; set; }
    public string? SMPosition { get; set; }
    public string? SMType { get; set; }
    public string? CertificateCode { get; set; }
    public string? SMFlagActive { get; set; }
    public string? WebsiteLink { get; set; }
    public string? FacebookLink { get; set; }
    public string? FanpageLink { get; set; }
    public string? GroupLink { get; set; }
    public string? ZaloLink { get; set; }
    /// <summary>
    /// Trạng thái nhân viên (`SMStatus`) — comment nguồn ghi rõ:
    /// **"0" nghỉ việc · "1" chính thức · "2" thử việc · "3" CTV**.
    /// 🔴 Nâng cấp 20221026 đổi định nghĩa "đang làm việc" thành **1 hoặc 2 hoặc 3** (trước chỉ là "0"
    /// theo nghĩa cũ) — nên đừng suy trạng thái từ tên cột.
    /// </summary>
    public string? SMStatus { get; set; }
    /// <summary>Số ngày làm việc trong hệ thống (`DaysOfService`).</summary>
    public decimal DaysOfService { get; set; }
    /// <summary>Danh sách đại lý Hyundai đã từng làm việc (`ListDealerHyundai`) — nguồn nối chuỗi sẵn.</summary>
    public string? ListDealerHyundai { get; set; }
    public DateTime? EffEndCertificate { get; set; }
    /// <summary>Tài khoản HTA (`AccountHTA`).</summary>
    public string? AccountHTA { get; set; }
    /// <summary>Trạng thái BĐH (`BDHStatus`): **"CHALLENGE"** đang thử thách · **"APPOINT"** đã bổ nhiệm.</summary>
    public string? BDHStatus { get; set; }
    public DateTime? ChallengeStartDate { get; set; }
    public DateTime? ChallengeEndDate { get; set; }
    /// <summary>Xếp loại chất lượng (`QualityRank`): **"PRO"** chuyên nghiệp · **"NORMAL"** thông thường.</summary>
    public string? QualityRank { get; set; }
    /// <summary>Mã nhân viên phía Hyundai (`SMHyundaiCode`) — thêm ở đợt 20221026.</summary>
    public string? SMHyundaiCode { get; set; }
    public string? IdentityCardNo { get; set; }
    // --- Đợt nâng cấp 20260316: dấu vết sửa trạng thái + chế tài vi phạm ---
    public string? UpdateStatusBy { get; set; }
    public DateTime? UpdateStatusDtime { get; set; }
    public decimal ViolateNumber { get; set; }
    public string? ViolateTypeId { get; set; }
    /// <summary>⚠️ Nguồn chụp CẢ mã lẫn TÊN loại vi phạm — cố ý phi chuẩn hoá vì là ảnh chụp.</summary>
    public string? ViolateTypeName { get; set; }
    public DateTime? ViolateDateStart { get; set; }
    public DateTime? ViolateDateEnd { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class PmtPaymentGps
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentGPSNo { get; set; } = "";
    /// <summary>Tháng thanh toán (`PmtMonth`).</summary>
    public string? PmtMonth { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    public decimal AmountTotal { get; set; }
    /// <summary>Thuế suất VAT — nguồn ghi thẳng literal '0.1' khi tạo phiếu (là TỈ LỆ, không phải tiền thuế).</summary>
    public decimal VAT { get; set; }
    /// <summary>Trạng thái phiếu (`PaymentGPSStatus`, `TConst.PaymentGPSStatus`, Const.Main.DMS40.cs:626).</summary>
    public string PaymentGPSStatus { get; set; } = "P";
    public DateTime? App1DTime { get; set; }
    public string? App1By { get; set; }
    public DateTime? App2DTime { get; set; }
    public string? App2By { get; set; }
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    // --- Hai chữ ký + hai mốc duyệt (chung cho cả 4 họ) ---
    /// <summary>Trạng thái ký phía HTV (`HTVSignStatus`, `TConst.HTVSignStatus`): N/P/C/A/A1/A2/F/R/D.</summary>
    public string HTVSignStatus { get; set; } = "P";
    public DateTime? HTVSignDTime { get; set; }
    /// <summary>Trạng thái ký phía TCMS (`TCMSSignStatus`) — cùng bảng mã với HTV.</summary>
    public string TCMSSignStatus { get; set; } = "P";
    public DateTime? TCMSSignDTime { get; set; }
    /// <summary>Đường dẫn file đã ký. Nguồn chuyển file từ thư mục `*_Temp` sang thư mục chính khi ký.</summary>
    public string? FilePath { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng xe của phiếu phí GPS (`Pmt_PaymentGPSDetail`) — 6 mốc ngày tính tiền thuê thiết bị.</summary>
public sealed class PmtPaymentGpsDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentGPSNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? CarId { get; set; }
    public string? GPSID { get; set; }
    public DateTime? GPSStartDate { get; set; }
    public DateTime? CostGPSStartDate { get; set; }
    public DateTime? RetailDate { get; set; }
    public DateTime? CostGPSEndDate { get; set; }
    public DateTime? PlanCostGPSDate { get; set; }
    /// <summary>Ngày trừ (`DeductDate`) — dùng cắt bớt kỳ tính phí.</summary>
    public DateTime? DeductDate { get; set; }
    public DateTime? ActualCostGPSDate { get; set; }
    public decimal PriceGPS { get; set; }
    public decimal AmountGPS { get; set; }
    /// <summary>Số hợp đồng GPS làm căn cứ tính tiền — endpoint xoá thiết bị đã join cột này (xem chú thích ở /api/gpsinstalls).</summary>
    public string? ContractGPS { get; set; }
    public string PaymentGPSDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phiếu thanh toán thiết bị AVN (màn hình giải trí) theo tháng (`Pmt_PaymentAVN`).</summary>
public sealed class PmtPaymentAvn
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentAVNNo { get; set; } = "";
    public string? PmtMonth { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    /// <summary>⚠️ Họ AVN **KHÔNG có cột VAT** (khác GPS/Storage) — đã đối chiếu danh sách cột `insert into`.</summary>
    public decimal AmountTotal { get; set; }
    public string PaymentAVNStatus { get; set; } = "P";
    public DateTime? App1DTime { get; set; }
    public string? App1By { get; set; }
    public DateTime? App2DTime { get; set; }
    public string? App2By { get; set; }
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    // --- Hai chữ ký + hai mốc duyệt (chung cho cả 4 họ) ---
    /// <summary>Trạng thái ký phía HTV (`HTVSignStatus`, `TConst.HTVSignStatus`): N/P/C/A/A1/A2/F/R/D.</summary>
    public string HTVSignStatus { get; set; } = "P";
    public DateTime? HTVSignDTime { get; set; }
    /// <summary>Trạng thái ký phía TCMS (`TCMSSignStatus`) — cùng bảng mã với HTV.</summary>
    public string TCMSSignStatus { get; set; } = "P";
    public DateTime? TCMSSignDTime { get; set; }
    /// <summary>Đường dẫn file đã ký. Nguồn chuyển file từ thư mục `*_Temp` sang thư mục chính khi ký.</summary>
    public string? FilePath { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Dòng xe của phiếu AVN (`Pmt_PaymentAVNDetail`).
/// ⚠️ Nguồn có dòng `--, CarId` **đã comment** ⇒ theo luật "port dòng ACTIVE, không port comment"
///    bảng này KHÔNG có CarId (khác 3 họ kia).
/// </summary>
public sealed class PmtPaymentAvnDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentAVNNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? EngineNo { get; set; }
    public DateTime? InStorageDate { get; set; }
    public DateTime? AVNDate { get; set; }
    public string? SerialNo { get; set; }
    public string? AVNCode { get; set; }
    public string PaymentAVNDtlStatus { get; set; } = "P";
    public decimal UnitPriceAVN { get; set; }
    /// <summary>Cờ đã thanh toán AVN — "1"/"0".</summary>
    public string? FlagPmtAVN { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Phiếu thanh toán phí lưu kho + phủ sơn theo tháng (`Pmt_PaymentStorage`) — do JOB sinh.</summary>
public sealed class PmtPaymentStorage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentStorageNo { get; set; } = "";
    public string? PmtMonth { get; set; }
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    /// <summary>Nguồn ghi cứng "WSHTC" — phiếu do JOB sinh, không phải người dùng lập.</summary>
    public string? CreateBy { get; set; }
    public decimal AmountTotal { get; set; }
    public decimal VAT { get; set; }
    public string PaymentStorageStatus { get; set; } = "P";
    public DateTime? App1DTime { get; set; }
    public string? App1By { get; set; }
    public DateTime? App2DTime { get; set; }
    public string? App2By { get; set; }
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    // --- Hai chữ ký + hai mốc duyệt (chung cho cả 4 họ) ---
    /// <summary>Trạng thái ký phía HTV (`HTVSignStatus`, `TConst.HTVSignStatus`): N/P/C/A/A1/A2/F/R/D.</summary>
    public string HTVSignStatus { get; set; } = "P";
    public DateTime? HTVSignDTime { get; set; }
    /// <summary>Trạng thái ký phía TCMS (`TCMSSignStatus`) — cùng bảng mã với HTV.</summary>
    public string TCMSSignStatus { get; set; } = "P";
    public DateTime? TCMSSignDTime { get; set; }
    /// <summary>Đường dẫn file đã ký. Nguồn chuyển file từ thư mục `*_Temp` sang thư mục chính khi ký.</summary>
    public string? FilePath { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng xe của phiếu lưu kho (`Pmt_PaymentStorageDetail`) — tách riêng tiền PHỦ SƠN và tiền LƯU KHO.</summary>
public sealed class PmtPaymentStorageDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentStorageNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? CarId { get; set; }
    public string? StorageCodeInit { get; set; }
    public DateTime? StorageDate { get; set; }
    /// <summary>Ngày duyệt bước 2 của chứng từ gốc (`ApprovedDate2`) — mốc bắt đầu tính phí.</summary>
    public DateTime? ApprovedDate2 { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    public string? DealerCode { get; set; }
    public DateTime? InCostStorageDate { get; set; }
    /// <summary>Bậc lưu kho (`LevelStorage`) — đơn giá lưu kho tăng theo bậc.</summary>
    public string? LevelStorage { get; set; }
    public DateTime? OutCostStorageDate { get; set; }
    public decimal CostStorageMonth { get; set; }
    public decimal PriceCoat { get; set; }
    public decimal PriceStorage { get; set; }
    public decimal CostCoat { get; set; }
    public decimal CostStorage { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStorageDtlStatus { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Phiếu thanh toán phí PDI theo tháng (`Pmt_PaymentPDI`) — do JOB sinh.
/// 🔴 Họ này đặt tên LỆCH khỏi 3 họ kia ở gần như MỌI cột (xem ghi chú đầu cụm).
/// </summary>
public sealed class PmtPaymentPdi
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>⚠️ `PmtPDINo` — KHÔNG phải "PaymentPDINo".</summary>
    public string PmtPDINo { get; set; } = "";
    public string? PmtMonth { get; set; }
    /// <summary>⚠️ `CreateDTime` — KHÔNG phải "CreateDateTime".</summary>
    public DateTime CreateDTime { get; set; } = DateTime.Now;
    public string? CreateBy { get; set; }
    /// <summary>⚠️ PDI tách 3 cột tiền, thay cho cặp `AmountTotal` + `VAT` của 3 họ kia.</summary>
    public decimal TotalAmount { get; set; }
    public decimal AmountVAT { get; set; }
    public decimal TotalAmountAfterVAT { get; set; }
    /// <summary>⚠️ Chỉ họ PDI mới có cặp sửa gần nhất này.</summary>
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    /// <summary>⚠️ `Appr1DTime`/`Appr1By` — 3 họ kia là "App1DTime"/"App1By" (thiếu chữ "r").</summary>
    public DateTime? Appr1DTime { get; set; }
    public string? Appr1By { get; set; }
    public DateTime? Appr2DTime { get; set; }
    public string? Appr2By { get; set; }
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    /// <summary>⚠️ Chỉ họ PDI mới lưu NGƯỜI ký (3 họ kia chỉ lưu mốc thời gian + trạng thái).</summary>
    public string? HTVSignBy { get; set; }
    public string? TCMSSignBy { get; set; }
    public string PmtPDIStatus { get; set; } = "P";
    // --- Hai chữ ký + hai mốc duyệt (chung cho cả 4 họ) ---
    /// <summary>Trạng thái ký phía HTV (`HTVSignStatus`, `TConst.HTVSignStatus`): N/P/C/A/A1/A2/F/R/D.</summary>
    public string HTVSignStatus { get; set; } = "P";
    public DateTime? HTVSignDTime { get; set; }
    /// <summary>Trạng thái ký phía TCMS (`TCMSSignStatus`) — cùng bảng mã với HTV.</summary>
    public string TCMSSignStatus { get; set; } = "P";
    public DateTime? TCMSSignDTime { get; set; }
    /// <summary>Đường dẫn file đã ký. Nguồn chuyển file từ thư mục `*_Temp` sang thư mục chính khi ký.</summary>
    public string? FilePath { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Dòng xe của phiếu PDI (`Pmt_PaymentPDIDetail`) — tách phí kiểm tra ĐẦU VÀO và ĐẦU RA.</summary>
public sealed class PmtPaymentPdiDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PmtPDINo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? CarId { get; set; }
    public DateTime? StoreDate { get; set; }
    public string? StorageCodeInit { get; set; }
    public DateTime? DlvStartDate { get; set; }
    /// <summary>Số biên bản giao xe làm căn cứ (`DlvMnNo`).</summary>
    public string? DlvMnNo { get; set; }
    public string? DealerCode { get; set; }
    public decimal CostInCheck { get; set; }
    public decimal CostOutCheck { get; set; }
    /// <summary>⚠️ `PmtPDIStatusDtl` — 3 họ kia đặt là "…DtlStatus" (đảo thứ tự chữ).</summary>
    public string PmtPDIStatusDtl { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class RqBankingTransPmt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? PaymentNo { get; set; }
    public string? PaymentType { get; set; }
    public string? DisbursementType { get; set; }
    public decimal TransferAmount { get; set; }
    public int LoanPeriod { get; set; }
    public DateTime? LoanPeriodDate { get; set; }
    public string? TransferRemark { get; set; }
    public decimal InterestRate { get; set; }
    public string? ReceivingUnit { get; set; }
    public string? BankAccountReceive { get; set; }
    public string? BankNameReceive { get; set; }
    public string? ProvinceName { get; set; }
    public string? Remark { get; set; }
    public DateTime? DisbursementRequestDate { get; set; }
    public DateTime? FirstInterestPmtDate { get; set; }
    public string? CreditContractNo { get; set; }
    public DateTime? CreditContractDate { get; set; }
    public string? Purpose { get; set; }
    public string? InvoiceNo { get; set; }
    public string? Representative { get; set; }
    /// <summary>Có uỷ quyền hay không — cờ "1"/"0" (luật `dmssales-flag-values-1-0-not-yn`).</summary>
    public string? FlagAuthority { get; set; }
    public string? AuthorityInfo { get; set; }
    public string? PaymentAccount { get; set; }
    public string? PaymentBankCode { get; set; }
    public decimal LoanLimit { get; set; }
    public decimal AmountDisbursed { get; set; }

    /// <summary>🔴 #276 `TransactionID` — mã giao dịch do **VIB** cấp khi đẩy file ký GIẢI NGÂN.
    /// Nguồn `VIB_BankTransactionFile` (`BizHTC.VPBank.cs:5262`, cây **chỉ có trên máy 150**) ghi nó vào
    /// **MỌI dòng** `RQ_BankingTransPmt` của đề nghị (`update … where RQ_BankingTransNo = @…`, không lọc
    /// thêm gì) ⇒ mã giao dịch gắn theo ĐỀ NGHỊ, không theo từng dòng thanh toán.</summary>
    public string? TransactionID { get; set; }

    public string BkTransPmtStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết theo XE của nhánh giải ngân (`RQ_BankingTransPmtDtl`).</summary>
public sealed class RqBankingTransPmtDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    /// <summary>Khoá dòng XE của hợp đồng đại lý (`CarId`) — cùng hệ khoá với `Dlr_ContractCar` (#129).</summary>
    public string? CarId { get; set; }
    public string? DlrCtrNo { get; set; }
    public decimal PmtPercent { get; set; }
    public decimal PmtAmount { get; set; }
    public decimal AmountActual { get; set; }
    public string? HTCInvoiceNo { get; set; }
    public string BkTransPmtDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Nhánh giải ngân theo L/C (`RQ_BankingTransPmtLC`) — CHỈ có ở bản LIVE _20220817.</summary>
public sealed class RqBankingTransPmtLC
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? PaymentType { get; set; }
    public string? DisbursementType { get; set; }
    public int LoanPeriod { get; set; }
    public DateTime? LoanPeriodDate { get; set; }
    public decimal InterestRate { get; set; }
    public string? Remark { get; set; }
    public string BkTransPmtLCStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Chi tiết nhánh giải ngân L/C (`RQ_BankingTransPmtLCDtl`) — CHỈ có ở bản LIVE.
/// 🔴 Khác hẳn các bảng `*Dtl` còn lại: KHÔNG chi tiết theo xe/hợp đồng mà theo **thư bảo lãnh**
///    (`BankGuaranteeNo` + DealerCode + BankCode + hạn mở/hết hạn + 3 mức tiền).
/// </summary>
public sealed class RqBankingTransPmtLCDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? BankGuaranteeNo { get; set; }
    public string? DealerCode { get; set; }
    public string? BankCode { get; set; }
    public DateTime? DateOpen { get; set; }
    public DateTime? DateExpired { get; set; }
    public decimal Amount { get; set; }
    public decimal AmountPmt { get; set; }
    public decimal AmountDisbursement { get; set; }
    public string BkTransPmtLCDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Nhánh BẢO LÃNH của đề nghị (`RQ_BankingTransGrt`).</summary>
public sealed class RqBankingTransGrt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? GuaranteeType { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? DateExpiredValue { get; set; }
    public string? GrtForm { get; set; }
    public string? GrtReceive { get; set; }
    public string? GrtReceiveAddress { get; set; }
    public string? BizResNumber { get; set; }
    public string? GrtRecPerson { get; set; }
    public string? GrtRecPosition { get; set; }
    public string? GrtRecDepartment { get; set; }
    public string? GrtRecPersonAddress { get; set; }
    public DateTime? DisbursementRequestDate { get; set; }
    /// <summary>#276 `TransactionID` do VIB cấp khi đẩy file ký BẢO LÃNH — nhánh song song với
    /// <see cref="RqBankingTransPmt.TransactionID"/>.</summary>
    public string? TransactionID { get; set; }

    public string BkTransGrtStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết theo XE của nhánh bảo lãnh (`RQ_BankingTransGrtDtl`).</summary>
public sealed class RqBankingTransGrtDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? CarId { get; set; }
    public string? DlrCtrNo { get; set; }
    public decimal GrtPercent { get; set; }
    public decimal GrtAmount { get; set; }
    public decimal AmountActual { get; set; }
    public string BkTransGrtDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Nhánh bảo lãnh theo L/C (`RQ_BankingTransGrtLC`) — CHỈ có ở bản LIVE.
/// Hai vế UT (trả trước) / UP (trả sau): mỗi vế có tiền + tỉ lệ + ngày TTC + ngày hiệu lực riêng.
/// </summary>
public sealed class RqBankingTransGrtLC
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? GrtLCType { get; set; }
    public decimal TotalAmount { get; set; }
    public string? PaymentNo { get; set; }
    public decimal AmountLCUT { get; set; }
    public decimal ProportionLCUT { get; set; }
    public DateTime? DateTTCLCUTValue { get; set; }
    public DateTime? DateLCUTValue { get; set; }
    public string? BankCodeLCUT { get; set; }
    public string? BankAccountLCUT { get; set; }
    public decimal AmountLCUP { get; set; }
    public decimal ProportionLCUP { get; set; }
    public DateTime? DateTTCLCUPValue { get; set; }
    public DateTime? DateLCUPValue { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string? ValidBank { get; set; }
    public string? GrtLCReceive { get; set; }
    public string? GrtLCReceiveAddress { get; set; }
    public string? BizResNumber { get; set; }
    public string? GrtLCRecPersonAddress { get; set; }
    public string BkTransGrtLCStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết theo XE của nhánh bảo lãnh L/C (`RQ_BankingTransGrtLCDtl`) — CHỈ có ở bản LIVE.</summary>
public sealed class RqBankingTransGrtLCDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? CarId { get; set; }
    public string? DlrCtrNo { get; set; }
    public decimal GrtPercent { get; set; }
    public decimal GrtAmount { get; set; }
    public decimal AmountActual { get; set; }
    public string BkTransGrtLCDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Nhánh BẢO ĐẢM / tài sản (`RQ_BankingTransWrt`) — 3 khoản cộng thêm, mỗi khoản một bộ
/// cờ + số tiền + ghi chú (ký quỹ bổ sung · hạn mức tín dụng · khoản khác).
/// </summary>
public sealed class RqBankingTransWrt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? BkTransType { get; set; }
    public string? AdditionalMarginFlag { get; set; }
    /// <summary>⚠️ Tên cột nguồn viết SAI chính tả: `AdditionalMarginAmout` (thiếu chữ "n") — GIỮ NGUYÊN để port 1:1.</summary>
    public decimal AdditionalMarginAmout { get; set; }
    public string? AdditionalMarginRemark { get; set; }
    public string? CreditAmountFlag { get; set; }
    public decimal CreditAmount { get; set; }
    public string? CreditAmountRemark { get; set; }
    public string? OtherFlag { get; set; }
    public decimal OtherAmount { get; set; }
    public string? OtherRemark { get; set; }
    public string? AssetGrtFlag { get; set; }
    public decimal TotalAssetAmount { get; set; }
    public decimal TotalAssetGrtAmount { get; set; }
    public string BkTransWrtStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết theo XE của nhánh bảo đảm (`RQ_BankingTransWrtDtl`).</summary>
public sealed class RqBankingTransWrtDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>⚠️ Nguồn liệt kê `BkTransType` TRƯỚC `RQ_BankingTransNo` ở bảng này (thứ tự cột khác 13 bảng kia).</summary>
    public string? BkTransType { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? CarId { get; set; }
    public string? DlrCtrNo { get; set; }
    public decimal AmountActual { get; set; }
    public string BkTransWrtDtlStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Hợp đồng đại lý gắn vào đề nghị (`RQ_BankingTransCtr`).</summary>
public sealed class RqBankingTransCtr
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? DlrCtrNo { get; set; }
    public string? SpecCode { get; set; }
    public DateTime? ContractDate { get; set; }
    public string? BkTransCtrPcpNo { get; set; }
    public DateTime? BkTransCtrPcpDate { get; set; }
    public string? AssemblyStatus { get; set; }
    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public string BkTransCtrStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Hợp đồng đại lý gắn vào NHÁNH BẢO ĐẢM (`RQ_BankingTransWrtCtr`) — cùng bộ cột với
/// `RQ_BankingTransCtr` nhưng có THÊM `LTV`, `GrtValue`, `BkTransCtrDate`.
/// </summary>
public sealed class RqBankingTransWrtCtr
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public string? DlrCtrNo { get; set; }
    public string? SpecCode { get; set; }
    public DateTime? ContractDate { get; set; }
    public string? BkTransCtrPcpNo { get; set; }
    public DateTime? BkTransCtrPcpDate { get; set; }
    public string? AssemblyStatus { get; set; }
    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    /// <summary>Tỉ lệ cho vay trên giá trị tài sản (loan-to-value).</summary>
    public decimal LTV { get; set; }
    public decimal GrtValue { get; set; }
    public DateTime? BkTransCtrDate { get; set; }
    public string BkTransWrtCtrStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class RqBankingTransAttachFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RQ_BankingTransNo { get; set; } = "";
    public int FileIndex { get; set; }
    public string? BkTransFileType { get; set; }
    public string? BkTransFilePath { get; set; }
    public string? BkTransFileName { get; set; }
    /// <summary>"1" = đã đẩy sang ngân hàng — nguồn đặt cho MỌI dòng ngay trước khi lưu.</summary>
    public string? FlagPush { get; set; }
    /// <summary>File thuộc hợp đồng đại lý hay không.</summary>
    public string? FlagDlrCtr { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// TỒN KHO TỐI THIỂU theo dòng xe (`Mst_MinInventory` — port 1:1 cụm 4 hàm
/// `Mst_MinInventory_CreateMulti_New20210604` (`Biz.HTC.WH.cs:201728`) / `_Update_New20210605` /
/// `_Delete_New20210605` (202688) / `_Get_New20210605`).
/// 🔴 **CA TWIN KIỂU MỚI — màn CHỈ TỒN TẠI Ở WS 64-bit:** `TERP.WSHTC.64/WSHTC.asmx.cs` có đủ 4 hàm,
/// còn `TERP.WSHTC/App_Code/WSHTC.cs` (32-bit) **KHÔNG có hàm nào** của cụm này.
/// Không phải "hai bit gọi hai bản khác nhau" (#118, #124, #127, #129) mà là **thiếu hẳn ở một bit** —
/// màn được thêm năm **2021**, sau khi bản 32-bit ngừng cập nhật. ⇒ Luật C0-centesimustricesimusprimus.
/// 🔴 `CreateMulti` ghi bằng **`insert … select from #tbl_Mst_MinInventory`** (201984-202001),
/// không qua `SaveData` ⇒ tra bảng nguồn bằng `SaveData("…")` sẽ **KHÔNG thấy** hàm tạo, chỉ thấy
/// hàm `Delete` (202773). Đây là lý do bảng này suýt bị bỏ sót.
/// ⚠️ Nguồn ghi cả `_dbMain` lẫn `_dbWH` (202003-202008, hai lệnh `ExecQuery` riêng).
/// </summary>
public sealed class MstMinInventory
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? ModelCode { get; set; }
    /// <summary>Số lượng tồn tối thiểu cần giữ.</summary>
    public decimal? QtyInv { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// LỊCH LÀM VIỆC (`Mst_Calendar` — port 1:1 `Mst_Calendar_Get/_ResetYear/_UpdateStatusValue_New20181119`
/// và `_GetForDepositDuty_New20181115`, `Biz.HTC.WH.cs:15776` / `15917`).
/// TWIN: cả WS 32-bit lẫn 64-bit **khớp hoàn toàn** (4/4 hàm, đã diff toàn bộ danh sách).
/// 🔴 Khoá là **CẶP** (`CalendarType`, `Date`) — một ngày có thể mang nhiều loại lịch khác nhau.
/// 🔴 `ResetYear` **sinh toàn bộ ngày của một năm**: quét từng ngày rồi đặt `StatusValue` theo
/// **thứ trong tuần** (`htDayOfWeek[dtimeScan.DayOfWeek]`, dòng 15772) — tức người dùng khai báo
/// giá trị cho Thứ 2…Chủ nhật rồi hệ thống trải ra cả năm. `UpdateStatusValue` sửa **từng ngày** lẻ sau đó.
/// 🔴 Hàm `_GetForDepositDuty` cho thấy lịch này dùng để **tính hạn nghĩa vụ đặt cọc** — không phải
/// lịch trang trí: sửa một ngày là đổi hạn tính tiền.
/// </summary>
public sealed class MstCalendar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Loại lịch — một phần của khoá cặp.</summary>
    public string CalendarType { get; set; } = "";
    public DateTime Date { get; set; }
    /// <summary>Giá trị trạng thái ngày (làm việc / nghỉ…), `ResetYear` đặt theo thứ trong tuần.</summary>
    public string? StatusValue { get; set; }
}

/// <summary>
/// XE trong hợp đồng bán lẻ (`Dlr_ContractCar` — 2010.HTC `Biz.HTC.WH.cs:93277`, **chỉ có ở bản
/// `DealerSalesDealCreate_SellToDealer_New20230306`**, bản 2018 mà WS 32-bit gọi KHÔNG ghi bảng này).
/// 🔴 **NỞ DÒNG THEO TỪNG XE**: một dòng `Dlr_ContractDtl` có `Qty = 3` sẽ sinh **3 dòng** ở đây.
/// 🔴 `CtrCarId` do nguồn **TỰ SINH** theo mẫu `string.Format("{0}.{1:00}", DlrContractNo, j)`
/// (dòng 93244) ⇒ dạng **`&lt;SốHĐ&gt;.01`, `.02`, `.03`…** — số thứ tự **2 chữ số**, đếm từ 1.
/// KHÔNG nhận từ client.
/// 🔴 Hai cờ khởi tạo `TConst.Flag.Inactive` (**"0"**): `FlagCancel` (đã huỷ chưa) và
/// `FlagDelivery` (đã giao chưa) — đây là nơi theo dõi từng xe của hợp đồng.
/// ⚠️ Nguồn ghi cả `_dbMain` lẫn `_dbWH` (93277-93280), dòng `_dbWH` **không bị comment**.
/// </summary>
public sealed class DlrContractCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrContractNo { get; set; } = "";
    /// <summary>Nguồn tự sinh: "&lt;SốHĐ&gt;.01", ".02"… (2 chữ số).</summary>
    public string CtrCarId { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public DateTime? DlvExpectedDate { get; set; }
    /// <summary>"1" = đã huỷ; khởi tạo "0".</summary>
    public string FlagCancel { get; set; } = "0";
    /// <summary>"1" = đã giao; khởi tạo "0".</summary>
    public string FlagDelivery { get; set; } = "0";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// LỊCH SỬ DÒNG hợp đồng bán lẻ (`Dlr_ContractDtlHis` — 2010.HTC, ghi ở **7 điểm**:
/// `Biz.HTC.WH.cs` 92527 / 93305 / 112277 / 113313 / 133720 / 134196 và
/// `BizHTC.RetailContract.cs` 1731 / 4937). Cột đọc từ bản canonical
/// `DealerSalesDealCreate_SellToDealer_New20230306` (93285-93308) — bản WS 64-bit gọi, xác định ở #129.
/// 🔴 **Cơ chế lịch sử KIỂU THỨ BA**, khác hai kiểu đã port trước:
/// · #91-#99 họ `*_His`: lưu **cặp Old/New** từng cột;
/// · #124 `DLS_DealerCustomer_Upd`: lưu **snapshot** toàn bộ bản ghi;
/// · **ở đây**: lưu **snapshot dòng theo PHIÊN BẢN** — mỗi dòng mang `VersionDTimeCurr` bằng đúng mốc
///   của phần đầu tại thời điểm đó, nên **nhóm theo `VersionDTimeCurr` sẽ dựng lại được nguyên trạng
///   bảng dòng ở từng phiên bản**. Không có cột Old/New.
/// 🔴 Dòng lịch sử chép từ `dtDetail_groupBy` (đã GỘP NHÓM theo Spec/Model/Color, `Qty = SumQty`) —
/// tức lịch sử đi theo `Dlr_ContractDtl`, **không** theo `Dlr_ContractCar` (bảng nở dòng của #129).
/// ⚠️ `ContractUpdateType` để **NULL khi TẠO MỚI**; chỉ các hàm SỬA (112266, 113305) mới truyền giá trị
/// từ bảng đầu vào ⇒ dòng lịch sử có `ContractUpdateType` rỗng nghĩa là **bản ghi lúc tạo**.
/// ⚠️ Nguồn ghi cả `_dbMain` lẫn `_dbWH` (93305-93308), dòng `_dbWH` **không bị comment**.
/// </summary>
public sealed class DlrContractDtlHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mốc phiên bản — bằng đúng `Dlr_Contract.VersionDTimeCurr` lúc ghi. Khoá nhóm lịch sử.</summary>
    public DateTime VersionDTimeCurr { get; set; }
    public string DlrContractNo { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    /// <summary>Tổng số lượng của nhóm (SumQty), không phải một xe.</summary>
    public int Qty { get; set; }
    /// <summary>NULL nghĩa là bản ghi lúc TẠO; có giá trị nghĩa là bản ghi do một lần SỬA.</summary>
    public string? ContractUpdateType { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? DlvExpectedDate { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Lịch sử sửa NGÂN HÀNG của HĐ bán lẻ (`Dlr_Contract_UpdateBankCode_His` — port 1:1
/// `Support_Dlr_Contract_UpdateBankCode`, 2010.HTC `Biz.HTC.WH.cs:114595`).
/// ✅ Guard mã ngân hàng mới phải có trong `Mst_Bank` ĐÃ port ở #116 (master bổ sung ở #115).
/// </summary>
public sealed class DlrContractUpdBankCodeHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrContractNo { get; set; } = "";
    public string? BankCodeOld { get; set; }
    public string? BankCodeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa KIỂU BÁN của HĐ bán lẻ (`Dlr_Contract_UpdateSalesType_His` — port 1:1
/// `Support_Dlr_Contract_UpdateSalesType`, `Biz.HTC.WH.cs:114292`).
/// ✅ Guard kiểu bán mới phải có trong `Mst_DealerSalesType` ĐÃ port ở #116 (master bổ sung ở #115).
/// 🔴 Nguồn CHỈ kiểm TỒN TẠI, KHÔNG kiểm `FlagActive` — khác helper `Mst_DealerSalesType_CheckDB`.
/// </summary>
public sealed class DlrContractUpdSalesTypeHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrContractNo { get; set; } = "";
    public string? SalesTypeOld { get; set; }
    public string? SalesTypeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa NHÂN VIÊN BÁN HÀNG của HĐ bán lẻ (`Dlr_Contract_UpdateSMCode_His` — port 1:1
/// `Support_Dlr_Contract_UpdateSMCode`, `Biz.HTC.WH.hkt.cs:8563`).
/// 🔴 Guard nguồn (ĐÃ port được vì MiniHTC có đủ cột): NVBH mới phải tồn tại,
/// **`SMStatus = "1"`** và **`SMType = "TVBH"`** — không phải NVBH nào cũng gán được.
/// </summary>
public sealed class DlrContractUpdSMCodeHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrContractNo { get; set; } = "";
    public string? SMCodeOld { get; set; }
    public string? SMCodeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa GIÁ dòng giao dịch bán lẻ (`Dls_DealDetail_HisUpdPrice` — port 1:1
/// `Dls_DealDetail_UpdatePrice`, 2010.HTC `Biz.HTC.WH.hkt.cs:6771`).
/// Luật nguồn: tra `Car_Car` theo **VIN** để lấy `CarId`, **không lấy CarId từ input**; nếu không có xe
/// thì báo lỗi. Sau đó update `DLS_DealDetail.Price = PriceNew` theo cặp khoá `DealNo` + `CarId`.
/// TWIN: chỉ `TERP.WSHTC.64` (27520) gọi hàm này.
/// </summary>
public sealed class DlsDealDetailHisUpdPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string CarId { get; set; } = "";
    public decimal? PriceOld { get; set; }
    public decimal? PriceNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa NGÀY XUẤT KHO của biên bản giao xe
/// (`Sto_DlvMinutes_UpdateDlvStartDateAndDeliveryOutDate_His` — port 1:1
/// `Support_Sto_DlvMinutes_UpdateDlvStartDateAndDeliveryOutDate`, 2010.HTC `Biz.HTC.WH.hkt.cs:8155`).
/// 🔴 Cùng lớp với <see cref="CarDeliveryDateHisUpd"/>: **một giá trị `DateNew` ghi vào HAI bảng, HAI tên cột**
/// — `Sto_DlvMinutes.DlvStartDate` (join `DlvMnNo`+`VIN`) và
/// `Car_DeliveryOrderDetail.DeliveryOutDate` (join `DeliveryOrderNo`+`DeliveryVIN`).
/// </summary>
public sealed class DlvMinutesUpdDateHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DlvMnNo { get; set; }
    public string? DeliveryOrderNo { get; set; }
    public string VIN { get; set; } = "";
    public DateTime? DlvStartDateOld { get; set; }
    public DateTime? DlvStartDateNew { get; set; }
    public DateTime? DeliveryOutDateOld { get; set; }
    public DateTime? DeliveryOutDateNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử sửa TỈNH/HUYỆN tuyến giao (`Sto_DlvMinutes_UpdateProvinceAndDistrict_His` — port 1:1
/// `Support_Sto_DlvMinutes_UpdateProvinceAndDistrict`, `Biz.HTC.WH.hkt.cs:8856`).
/// ⚠️ Nguồn **kiểm tồn tại cặp tỉnh–huyện mới trong `Mst_District`** cho CẢ hai đầu tuyến (F và T)
/// trước khi cập nhật; lưu giá trị cũ/mới của cả 4 cột.
/// </summary>
public sealed class DlvMinutesUpdProvinceHis
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DlvMnNo { get; set; }
    public string VIN { get; set; } = "";
    public string? FProvinceCodeOld { get; set; }
    public string? FProvinceCodeNew { get; set; }
    public string? FDistrictCodeOld { get; set; }
    public string? FDistrictCodeNew { get; set; }
    public string? TProvinceCodeOld { get; set; }
    public string? TProvinceCodeNew { get; set; }
    public string? TDistrictCodeOld { get; set; }
    public string? TDistrictCodeNew { get; set; }
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>
/// Lịch sử cập nhật NGÀY GIAO XE (`CarDeliveryDate_HisUpd` — port 1:1 `CarDeliveryDate_Update`,
/// 2010.HTC `Biz.HTC.WH.cs:139603`).
/// 🔴 Một hành động sửa ngày giao ghi vào **BA bảng** với **ba tên cột khác nhau** nhưng **cùng một giá trị**:
/// `Sto_DlvMinutes.DlvEndDate` · `Car_DeliveryOrderDetail.DeliveryEndDate` · `DLS_DealDetail.DeliveryDate`.
/// Bảng này lưu **giá trị CŨ của cả ba cột** cùng giá trị mới, nên là dấu vết duy nhất để đối soát.
/// </summary>
public sealed class CarDeliveryDateHisUpd
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DlvMnNo { get; set; }
    public string VIN { get; set; } = "";
    public string? DeliveryOrderNo { get; set; }
    public string? CarId { get; set; }
    public string? DealNo { get; set; }
    public DateTime? DlvEndDateOld { get; set; }
    public DateTime? DeliveryEndDateOld { get; set; }
    public DateTime? DeliveryDateOld { get; set; }
    public DateTime? DlvEndDateNew { get; set; }
    public DateTime? DeliveryEndDateNew { get; set; }
    public DateTime? DeliveryDateNew { get; set; }
    /// <summary>Thời điểm cập nhật (`UpdDTime`).</summary>
    public DateTime UpdDTime { get; set; } = DateTime.Now;
    public string? UpdBy { get; set; }
}

/// <summary>File đính kèm sổ bảo hành theo HĐ bán lẻ (Dls_DealerDealAttach) — port 1:1 FrmEditDeal_SoBaoHanh (2010.HTC/SalesDealer). Metadata file (không lưu binary) — 1 file mới nhất mỗi DealNo, upsert.</summary>
public sealed class DealerDealAttach
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string FileName { get; set; } = "";
    public string? FilePath { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thiết lập hóa đơn theo model (Mst_InvoiceSetup) — port 1:1 FrmMst_InvoiceSetup (2010.HTC/Admin/Product). Cờ xuất HĐ HTMV/TCG theo model.</summary>
public sealed class InvoiceSetup
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string FlagInvoiceHTMV { get; set; } = "0";
    public string FlagInvoiceTCG { get; set; } = "0";
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #185 parity cum `Mst_InvoiceSetup_*` (DataWH/Biz.HTC.WH.cs, csproj 272) =====
    /// <summary>Nhật ký sửa cuối — nguồn ghi ở CẢ `_CreateMulti` lẫn `_Update`.
    /// ⚠️ Nguồn `_Update` ghi cột này bằng format **"yyyyMMddHH:mm:ss"** (thiếu gạch/khoảng trắng);
    /// toàn hệ dùng "yyyy-MM-dd HH:mm:ss" ở 671 chỗ, format kia chỉ 10 chỗ ⇒ lỗi gõ của nguồn.</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Ngưỡng tồn kho bán hàng (Mst_MngRateTonKhoBanHang) — port 1:1 FrmMstSalesInventoryThreshold (2010.HTC/Admin/Product). Ngưỡng bán hàng (NguongBH) theo đại lý + model.</summary>
public sealed class SalesInventoryThreshold
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public int NguongBH { get; set; }
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC: `Mst_MngRateTonKhoBanHang` nguồn **không có** `FlagActive`.</summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC, đứng thay `LogLUDateTime` của nguồn.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // --- #147 parity Mst_MngRateTonKhoBanHang: nguồn có Remark + dấu vết chuẩn ---
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Tài khoản ngân hàng (Mst_BankAccount) — port 1:1 FrmMstAccountBank (2010.HTC/Admin/Product). TK NH của HTC/đại lý, cờ TK dùng cho GrtClaim.</summary>
public sealed class BankAccount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AccountNo { get; set; } = "";
    public string? AccountName { get; set; }
    public string? BankCode { get; set; }
    public string? DealerCode { get; set; }
    public string FlagAccGrtClaim { get; set; } = "0";
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Số hiệu hóa đơn (Mst_InvoiceID) — port 1:1 FrmInvoiceID_HTC/HTCLD/TCG (2010.HTC/Admin/Product). Đăng ký số hiệu HĐ theo loại (HTC/HTCLD/TCG) + ngày hiệu lực.</summary>
public sealed class InvoiceID
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceIDCode { get; set; } = "";
    public string InvoiceIDType { get; set; } = "";   // HTC / HTCLD / TCG
    public DateTime EffectiveDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phân bổ xe theo vùng (Mst_CarAllocationByArea) — port 1:1 FrmMst_CarAllocationByArea (2010.HTC/Admin/Product). Tỷ lệ phân bổ theo model/spec cho 3 miền (tổng = 100%).</summary>
public sealed class CarAllocationByArea
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public decimal MBPercent { get; set; }   // % Miền Bắc
    public decimal MTPercent { get; set; }   // % Miền Trung
    public decimal MNPercent { get; set; }   // % Miền Nam
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Mã OCN xe (Mst_CarOCN) — port 1:1 FrmCarOCN (2010.HTC/Admin/Product). OCN theo model.</summary>
public sealed class CarOCN
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OCNCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string? OCNDesc { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Ngân hàng của đại lý — port 1:1 FrmDealerBank (2010.HTC/Admin/Product): tài khoản/hạn mức NH của
/// đại lý + cờ NH bảo lãnh / NH thanh toán.
/// 🔴 **#133 parity Mst_BankDealer — SỬA TÊN BẢNG trong mô tả:** port cũ ghi nguồn là `Mst_DealerBank`,
/// nhưng **bảng tên đó KHÔNG TỒN TẠI** (grep toàn `TERP.BizHTC`: 0 hit). Bảng thật là
/// **`Mst_BankDealer`** — **đảo thứ tự hai từ** (ghi tại `Biz.HTC.WH.cs:4779`).
/// Cùng loại lỗi với `PRD_PaymentReqDiscount_VIN` ở #127/#128: tên lớp/mô tả trỏ vào bảng không có thật,
/// khiến mọi lần tra cứu nguồn sau đó đều trượt.
/// 🔴 **TWIN:** cụm `Mst_BankDealer_*` **CHỈ có ở WS 64-bit** (32-bit không có hàm nào) — ca thứ ba
/// cùng dạng (sau `Mst_MinInventory` #131 và `DLS_DealAttachFile` #132).
/// WS gọi `Mst_BankDealer_**Create_20230922**` (`Biz.HTC.WH.cs:4830`), **không phải** bản
/// `Mst_BankDealer_Create` không hậu tố ở dòng 4661 — bản cũ **thiếu 5 tham số**
/// (`CreditContractNo`, `CreditContractDate`, `CreditAmount`, `BankBranchCode`, `BankBranchName`).
/// May là port cũ đã có đủ 5 cột này; GAP #133 chỉ còn `Remark` + dấu vết sửa.
/// </summary>
public sealed class DealerBank
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BankCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? BankBranchCode { get; set; }
    public string? BankBranchName { get; set; }      // audit 2026-09-03: bổ sung — thiếu ở fire trước
    public string? CreditContractNo { get; set; }
    public DateTime? CreditContractDate { get; set; }
    public decimal CreditAmount { get; set; }        // hạn mức tín dụng
    public string FlagBankGrt { get; set; } = "0";   // NH bảo lãnh
    public string FlagBankPmt { get; set; } = "0";   // NH thanh toán
    public string FlagActive { get; set; } = "1";
    /// <summary>#133: nguồn có `Remark` (Biz.HTC.WH.cs:4966) — port cũ thiếu.</summary>
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Ngưỡng tồn kho đại lý (Mst_DealerInventoryThreshold) — port 1:1 FrmMst_DealerInventoryThreshold (2010.HTC/Admin/Product). Ngưỡng SL tồn theo đại lý + model.</summary>
public sealed class DealerInventoryThreshold
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public int Qty { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Vùng đại lý (Mst_DealerZone) — port 1:1 FrmMst_DealerZone (2010.HTC/Admin/Product). Gán đại lý vào vùng.</summary>
public sealed class DealerZone
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ZoneCode { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Điều khoản thanh toán (Mst_PaymentTerm) — port 1:1 FrmMst_Dieu_Khoan_ThanhToan (2010.HTC/Admin/Product). ĐK thanh toán theo model/spec: % cọc, % bảo lãnh, số ngày.</summary>
public sealed class PaymentTerm
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PMTermNo { get; set; } = "";
    public DateTime EffectiveDateFrom { get; set; }
    public DateTime EffectiveDateTo { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string FlagDepositPmt { get; set; } = "0";
    public decimal DepositPercent { get; set; }
    public decimal GuaranteePercent { get; set; }
    public int GuaranteeDays { get; set; }
    public int DepositDutyEndDays { get; set; }
    public int GuaranteeEndDays { get; set; }
    public int DepositDealDateDays { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Model/quy cách áp dụng cho một điều khoản thanh toán (Mst_PaymentTermDetail —
/// port 1:1 lưới của FrmNew_Dieu_Khoan_ThanhToan, 2010.HTC/Admin/Product).
/// MỘT điều khoản áp dụng cho NHIỀU cặp (model, quy cách); nguồn lưu thành bảng chi tiết riêng.
/// Khoá nghiệp vụ của dòng là CẶP (ModelCode, SpecCode) — form ghép chuỗi "ModelCode|SpecCode" khi xoá dòng.
/// </summary>
public sealed class PaymentTermDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>Số điều khoản thanh toán (khoá nối về <see cref="PaymentTerm"/>).</summary>
    public string PMTermNo { get; set; } = "";

    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public string SpecCode { get; set; } = "";
    public string? SpecDescription { get; set; }

    /// <summary>Cờ áp dụng đặt cọc cho cặp model/quy cách này: "1" = có, "0" = không.</summary>
    public string FlagDepositPmt { get; set; } = "0";
}

/// <summary>Quy cách xe (Mst_CarSpec) — port 1:1 FrmCarSpec (2010.HTC/Admin/Product). Master spec: model/std-opt/grade/OCN/số chỗ/spec gốc.</summary>
public sealed class CarSpec
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SpecCode { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? StdOptCode { get; set; }
    public string? GradeCode { get; set; }
    public string? OCNCode { get; set; }
    public string? SpecDesc { get; set; }
    public string? RootSpec { get; set; }
    public int? NumberOfSeats { get; set; }
    public string FlagAmbulance { get; set; } = "0";
    public string FlagActive { get; set; } = "1";
    // audit 2026-09-03: 5 field dưới bổ sung — thiếu ở port trước (FrmCarSpec.cs)
    public string? AssemblyStatus { get; set; }      // CKD/CBU
    public string FlagInvoiceFactory { get; set; } = "0";
    public string FlagDepositPmt { get; set; } = "0";
    public string? OriginNo { get; set; }            // bắt buộc nếu AssemblyStatus=CBU
    public DateTime? QuotaDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Giá màn hình AVN (Mst_UnitPriceAVN) — port 1:1 FrmMst_AVNPrice (2010.HTC/Admin/Product). Đơn giá màn hình AVN theo mã + ngày hiệu lực.</summary>
public sealed class AVNPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AVNCode { get; set; } = "";
    public decimal UnitPriceAVN { get; set; }
    public DateTime? EffDateTime { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Điều kiện tự động tạo DO (Mst_DOATCondition + Dtl) — port 1:1 FrmNewSetupConditionForDOAuto/FrmMngSetupConditionForDOAuto (2010.HTC/Sales). Config auto-gen lệnh giao xe: % cọc, % hoàn thành ĐK, danh sách model.</summary>
public sealed class DOATCondition
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DOATConditionCode { get; set; } = "";
    public DateTime EffDateStart { get; set; }
    public DateTime EffDateEnd { get; set; }
    public string FlagCQEndDate { get; set; } = "0";
    public string FlagTaxPaymentDate { get; set; } = "0";
    public string FlagPtmCoc { get; set; } = "0";       // dùng đk % thanh toán cọc
    public decimal PtmCocFrom { get; set; }
    public decimal PtmCocTo { get; set; }
    public string FlagDutyComplete { get; set; } = "0"; // dùng đk % hoàn thành giao xe
    public decimal DutyCompleteFrom { get; set; }
    public decimal DutyCompleteTo { get; set; }
    public string FlagModel { get; set; } = "0";        // giới hạn theo danh sách model
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public sealed class DOATConditionModel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DOATConditionId { get; set; }
    public string ModelCode { get; set; } = "";
}

/// <summary>Đề nghị giao dịch ngân hàng (BankingTrans) — port 1:1 FrmDeNghiGDNganHang (2010.HTC/Sales/Payment). ĐN GD với ngân hàng: giải ngân GNTT / bảo lãnh LC / phát hành LC.</summary>
public sealed class BankingTrans
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    // ===== #205 parity: tên cột về ĐÚNG nguồn `RQ_BankingTransactions` (BankIntergration/BizHTC.VietinBank.cs) =====
    // Tên cũ `SoDeNghi` / `TransType` / `BankStatus` KHÔNG tồn tại trong CẢ solution 2010.HTC (sweep #205 tầng B).
    // Nguồn dùng: `RQ_BankingTransNo` · `BkTransType` · `BkTransStatus` · `BkTransBankStatus`.
    public string RQ_BankingTransNo { get; set; } = "";       // số đề nghị (auto)
    public string BankCode { get; set; } = "";       // ngân hàng
    public string BkTransType { get; set; } = "";       // loại ĐN GD: GNTT/BLLC/PHLC
    public DateTime? DisbursementDate { get; set; }   // ngày giải ngân
    public decimal AmountDisbursed { get; set; }      // số tiền giải ngân
    public decimal TotalAmount { get; set; }
    // ===== #206 parity: đối chiếu TỪNG CỘT với `RQ_BankingTransactions_SaveX_20220817`
    //       (BankIntergration/BizHTC.VietinBank.cs:7808-7852) — nguồn ghi ĐÚNG 19 cột. =====
    /// <summary>Trạng thái nội bộ (`BkTransStatus`) — tên cũ `Status` không phải tên cột nguồn.</summary>
    public string BkTransStatus { get; set; } = "Draft";     // Draft → Sent → Approved / Rejected
    public string? Remark { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;   // #206: nguồn `CreatedDate`
    /// <summary>⚠️ #206: nguồn KHÔNG có cột này — mốc "đã gửi" là của port. Giữ, đánh dấu rõ.</summary>
    public DateTime? SentAt { get; set; }
    public DateTime? ApprovedDate { get; set; }                  // #206: nguồn `ApprovedDate`
    // ✅ #206: `RefBankCode` và `BankRemark` ĐÃ CÓ sẵn ở cuối class này — kiểm lại thấy đủ, không thêm trùng.
    /// <summary>
    /// Trạng thái bên NGÂN HÀNG (`BkTransBankStatus`, `TConst.BkTransBankStatus` — hệ `ERP.DMS.HTC.VPBank.WS`,
    /// **chỉ có trên máy 150**): "N" · "P" · "C" · **"A0".."A5"** các mức duyệt · "F" hoàn tất · "R" từ chối.
    /// 📌 Port cũ chỉ ghi nhận P/A1/A2/A3 — nguồn có **A0 và A4, A5** nữa, cùng "F"/"C"/"R".
    /// </summary>
    public string BkTransBankStatus { get; set; } = "P";
    public DateTime? PushedToBankAt { get; set; }

    // --- #137 parity RQ_BankingTransactions: 9 cột nguồn ghi mà port cũ THIẾU ---
    /// <summary>Đại lý đứng tên đề nghị (`DealerCode`).</summary>
    public string? DealerCode { get; set; }
    /// <summary>Số ĐKKD của đại lý (`BizResNumber`) — nguồn ghi ở CẢ bảng đầu lẫn nhánh bảo lãnh.</summary>
    public string? BizResNumber { get; set; }
    public string? CreatedBy { get; set; }
    public string? ApprovedBy { get; set; }
    /// <summary>Hoàn tất (`FinishDate`/`FinishBy`) — trạng thái CUỐI, khác `ApprovedDate`.</summary>
    public DateTime? FinishDate { get; set; }
    public string? FinishBy { get; set; }
    public DateTime? CancelDate { get; set; }
    public string? CancelBy { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    /// <summary>Mã tham chiếu do ngân hàng cấp khi báo kết quả về (`RefBankCode`).</summary>
    public string? RefBankCode { get; set; }

    /// <summary>
    /// 🔴 #276 `LoanType` — loại hồ sơ VIB đã đẩy file ký cho đề nghị này (`DISBURSEMENT` / `GUARANTEE`).
    /// **RỖNG = VIB CHƯA từng đẩy file lần nào** — nguồn dùng đúng dấu hiệu này để chặn: lần đầu mà xin
    /// `ReSign = "Y"` (ký lại) là vô lý ⇒ ném lỗi.
    /// </summary>
    public string? LoanType { get; set; }
    /// <summary>Ghi chú của ngân hàng trả về (`BankRemark`).</summary>
    public string? BankRemark { get; set; }
    public DateTime? BankUpdatedAt { get; set; }

    // --- Nhóm GIẢI NGÂN (LD) — ngân hàng trả về ---
    /// <summary>Số khế ước giải ngân. ⚠️ Nguồn CÓ guard "Số LDNo trống!" nhưng **đã COMMENT** ⇒ KHÔNG bắt buộc.</summary>
    public string? LDNo { get; set; }
    public string? DisbursementTerm { get; set; }
    public decimal DisbursementInterestRate { get; set; }

    // --- Nhóm BẢO LÃNH (MD) ---
    /// <summary>Số bảo lãnh. ⚠️ Guard "Số MDNo trống!" của nguồn cũng **đã COMMENT** ⇒ KHÔNG bắt buộc.</summary>
    public string? MDNo { get; set; }
    public decimal GrtAmount { get; set; }
    public DateTime? GrtDateStart { get; set; }
    public DateTime? GrtDateEnd { get; set; }
    public string? GrtTerm { get; set; }
    public decimal GrtFee { get; set; }
    /// <summary>Ngày trả phí bảo lãnh chậm (`GrtLatePmtDate`).</summary>
    public DateTime? GrtLatePmtDate { get; set; }

    // --- Nhóm LC ---
    public string? LCNo { get; set; }
    public decimal LCAmount { get; set; }
    public DateTime? LCStartDate { get; set; }
    public DateTime? LCEndDate { get; set; }
}

/// <summary>Biên bản giao xe (Sto_DlvMinutes) — port 1:1 FrmDealerNewDlvMinutes/FrmHTCNewDlvMinutes (2010.HTC/Sales/DlvMinutes). BB giao/vận chuyển xe: VIN, tuyến đi-đến, ĐVVT + lái xe, ngày giao + checklist tình trạng xe (JSON ~25 mục OS/IS/SP/DA).</summary>
// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (nợ `### C0`, treo từ #14, hợp nhất ở #60).
// `DlvMinutes` (bảng "DlvMinutesSet") và <see cref="TranspDlvConfirm"/> (+ <see cref="DlvMinutesCheckItem"/>)
// **cùng map bảng nguồn `Sto_DlvMinutes`**. Cụm `/api/dlvminutes` đã trỏ sang `TranspDlvConfirm`.
// Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
public sealed class DlvMinutes
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlvMinutesNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? FProvinceCode { get; set; }
    public string? TProvinceCode { get; set; }
    public string? FDistrictCode { get; set; }
    public string? TDistrictCode { get; set; }
    public string TransporterCode { get; set; } = "";  // đơn vị vận tải
    public string? DriverCode { get; set; }             // lái xe
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }
    public string ChecklistJson { get; set; } = "{}";   // checklist tình trạng (item→bool)
    public string Status { get; set; } = "Draft";       // Draft → Confirmed
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
}

/// <summary>Đề nghị nhận xe/PDI (HTMV_PDI + Dtl) — port 1:1 FrmNewPDI (2010.HTC/Sales/HTMV). Đề nghị nhận xe để PDI theo VIN.</summary>
public sealed class HtmvPdi
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PDINo { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái đề nghị (`HTMV_PDI.PDIStatus`) theo `TConst.Stage` — nguồn tạo ở **"P"**
    /// (BizHTC.HTMV.cs:1384). Port cũ `Draft → Done` là tên tự đặt.
    /// ⚠️ Cụm này có **BA trục trạng thái**: header `PDIStatus`, và **HAI trục trên cùng một DÒNG** —
    /// <see cref="HtmvPdiDtl.PDIDtlStatus"/> và <see cref="HtmvPdiDtl.PDIStorageStatus"/>.
    /// </summary>
    public string Status { get; set; } = "P";
    /// <summary>Ghi chú (`HTMV_PDI.Remark`) — nguồn ghi khi tạo, port cũ thiếu.</summary>
    public string? Remark { get; set; }
    /// <summary>Người tạo (`CreatedBy`) — nguồn ghi khi tạo, port cũ thiếu.</summary>
    public string? CreatedBy { get; set; }
    /// <summary>Ngày/người duyệt (`ApprovedDate`/`ApprovedBy`) — nguồn khởi tạo NULL rồi ghi khi duyệt.</summary>
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
}
public sealed class HtmvPdiDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long HtmvPdiId { get; set; }
    public string VIN { get; set; } = "";
    public string? ColorCode { get; set; }
    public string? SpecCode { get; set; }
    public string? LCTemp { get; set; }
    public string? RefNo { get; set; }
    public string? ProductionMonth { get; set; }
    public string? EngineNo { get; set; }
    /// <summary>Mã model của dòng (`HTMV_PDIDtl.ModelCode`) — nguồn ghi riêng, port cũ thiếu.</summary>
    public string? ModelCode { get; set; }
    /// <summary>
    /// 🔴 Trạng thái DÒNG (`HTMV_PDIDtl.PDIDtlStatus`, `TConst.Stage`): tạo ở **"P"**;
    /// `HTMV_PDICancel_New20181115` guard `"P"` rồi gán **"C"** (BizHTC.HTMV.cs:2558+).
    /// </summary>
    public string PDIDtlStatus { get; set; } = "P";
    /// <summary>
    /// 🔴 Trạng thái KHO PDI của dòng (`PDIStorageStatus`, `TConst.PDIStorageStatus`
    /// — `Const.Main.cs:131-139`: N/P/C/A/A1/A2/F). Trục **thứ hai trên cùng một dòng**, độc lập với
    /// `PDIDtlStatus`. Bản LIVE `HTMV_PDIApprove_New20181115`
    /// (**MMSIntergration/BizHTC.MMSIntergration.cs:2213** — file KHÁC với Create/Cancel)
    /// guard `"P"` rồi gán **"F" (Finished)**, KHÔNG phải "A".
    /// </summary>
    public string PDIStorageStatus { get; set; } = "P";
    /// <summary>⚠️ Giữ để đọc dữ liệu cũ; trục thật là hai cột trên. Port cũ dùng Pending/Passed/Failed.</summary>
    public string PdiResult { get; set; } = "Pending";
}

/// <summary>Xe nhập kho PDI (PDI_VIN) — port 1:1 FrmStoragePDI (2010.HTC/Sales/HTMV). Xe tại kho PDI: model/spec/màu + số chìa/AVN/ắc quy.</summary>
public sealed class StoragePdiVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public string? OrderNoMMS { get; set; }
    public string? EngineNo { get; set; }
    public string? KeyNo { get; set; }          // số chìa khóa
    public string? AVNSerialNo { get; set; }    // serial màn hình AVN
    public string? BatteryNo { get; set; }      // số ắc quy
    public string FlagActive { get; set; } = "1";
    public string? Remark { get; set; }

    /// <summary>
    /// 🔴 #B03: Thời điểm HOÀN TẤT PDI = "thời gian nhập kho" (`PDI_VIN.FinishDTime`).
    /// Là bộ lọc **BẮT BUỘC** của báo cáo *Xe nhập kho và lắp GPS*
    /// (`Rpt_CarInStoAndMapGPS_New20181115`, `BizHTC.ZTempGPS.cs:9198`; form chặn rỗng:
    /// *"Chưa chọn Thời gian nhập kho!"*), và là cột hiển thị của lưới. Port trước không có ⇒
    /// báo cáo đó không thể tồn tại.
    /// </summary>
    public DateTime? FinishDTime { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (ca thứ 3, phát hiện #56 bằng sweep tên bảng nguồn).
/// `ReqInvoice`/`ReqInvoiceDtl` và <see cref="RedeemInvoiceRequest"/>/<see cref="RedeemInvoiceRequestLine"/>
/// **cùng map một bảng nguồn `RD_ReqInvoice`/`RD_ReqInvoiceDtl`** (grep `SaveData("RD_ReqInvoice"` — nguồn chỉ có 1 bảng).
/// Endpoint `/api/reqinvoices` đã trỏ sang <see cref="RedeemInvoiceRequest"/>. Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
/// </summary>
public sealed class ReqInvoice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqIVNo { get; set; } = "";
    public string Status { get; set; } = "Draft"; // Draft → Done
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
}
public sealed class ReqInvoiceDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqInvoiceId { get; set; }
    public string VIN { get; set; } = "";
    public string? HTCInvoiceNo { get; set; }
    public string? InvoiceNoFactory { get; set; }
    public string? TCGInvoiceNo { get; set; }
}

/// <summary>Hợp đồng đại lý (DC/DealerContract + Detail) — port 1:1 FrmNewDC/FrmMngDC (2010.HTC/Sales/Contract). HĐ đại lý mua xe: xe + đơn giá + tổng tiền + duyệt.</summary>
public sealed class DealerContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerContractNo { get; set; } = "";
    public string? DealerContractNoUser { get; set; }
    public string DealerCode { get; set; } = "";
    public DateTime? ContractDate { get; set; }
    public decimal TotalAmount { get; set; }
    /// <summary>
    /// 🔴 Trạng thái HĐ đại lý (`CT_DealerContract.ContractStatus`) theo `TConst.Stage`:
    /// **"P" chờ duyệt · "A" duyệt · "R" từ chối · "C" huỷ · "F" hoàn thành**.
    /// ⚠️ Port cũ `Draft/Approved/Rejected` = sai mã và **thiếu "C" lẫn "F"**.
    /// 📌 Bảng nguồn xác định bằng tên biến `dt_CT_DealerContract` tại dòng ghi `DealerContractNo`
    /// (Biz.HTC.WH.cs:30939) — KHÔNG phải `Dlr_Contract` (bảng đó là HĐ **bán lẻ**).
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    /// <summary>Người duyệt (`ApprovedBy`) — nguồn ghi ở CẢ duyệt/từ chối LẪN huỷ (dùng chung cột).</summary>
    public string? ApprovedBy { get; set; }
    /// <summary>Ghi chú người duyệt/huỷ (`CT_DealerContract.Remark`).</summary>
    public string? Remark { get; set; }
    /// <summary>Ngày nhận hợp đồng (`ReceiptContractDate`) — nguồn cập nhật ở `ContractDealerContractUpdate`.</summary>
    public DateTime? ReceiptContractDate { get; set; }
}
/// <summary>
/// Dòng hợp đồng đại lý (`CT_DealerContractDetail` — 2010.HTC `Biz.HTC.WH.cs:30962-30971`,
/// trong `ContractDealerContractCreate_New20181119` (30719)).
/// 🔴 GAP đã vá ở #123: nguồn khoá dòng bằng **`DealerContractNo`** (số hợp đồng, kiểu chuỗi),
/// port cũ chỉ có `DealerContractId` (khoá nội bộ) ⇒ **không map được dữ liệu thật** khi import
/// từ SQL 228. Nay giữ cả hai: `DealerContractId` cho liên kết nội bộ, `DealerContractNo` khớp nguồn.
/// 🔴 GAP thứ hai: nguồn có **`ContractDetailStatus`** riêng cho từng DÒNG (đặt `Stage.Pending` = "P"
/// khi tạo), tách khỏi `ContractStatus` của phần đầu — port cũ **thiếu hẳn trục trạng thái này**.
/// </summary>
public sealed class DealerContractDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DealerContractId { get; set; }
    /// <summary>Số hợp đồng — khoá dòng của nguồn (`CT_DealerContractDetail.DealerContractNo`).</summary>
    public string? DealerContractNo { get; set; }
    public string CarId { get; set; } = "";
    public decimal UnitPrice { get; set; }
    /// <summary>Trạng thái DÒNG, "P" khi tạo — tách khỏi ContractStatus của phần đầu.</summary>
    public string ContractDetailStatus { get; set; } = "P";
}

/// <summary>Biên bản hủy hợp đồng đại lý DMS40 (DMS40_DlrCtr_CancelMinutes) — port 1:1 FrmDMS40_DlrCtr_CancelMinutes (2010.HTC/Sales/DMS40). Hủy HĐ đại lý theo DlrCtrNo.</summary>
public sealed class DmsCancelMinutes
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CancelMinutesNo { get; set; } = "";
    public string DlrCtrNo { get; set; } = "";
    public string? Remark { get; set; }
    public string FlagIsDelete { get; set; } = "0";
    /// <summary>
    /// 🔴 Trạng thái biên bản (`CancelMinutesStatus`) theo `TConst.CancelMinutesStatus`
    /// (`Const.Main.DMS40.cs:189-195`): **"NS" chưa ký · "S" đã ký · "AJ" điều chỉnh · "C" huỷ**.
    /// ⚠️ Port cũ KHÔNG có trục này — tạo biên bản là **huỷ luôn hợp đồng ngay**, bỏ qua toàn bộ
    /// quy trình ký/duyệt bên dưới.
    /// </summary>
    public string CancelMinutesStatus { get; set; } = "NS";
    /// <summary>
    /// 🔴 Ký của đại lý trên biên bản (`DlrSignCcMnStatus`, `TConst.DlrSignCcMnStatus` 155-165):
    /// N/**P**/C/**A**/A1/A2/F/R/D.
    /// </summary>
    public string DlrSignCcMnStatus { get; set; } = "P";
    /// <summary>
    /// 🔴 Ký của HTC (`HTCSignCcMnStatus`, 172-182) — **HAI CẤP**: "P" → **"A1"** (cấp 1) → **"A"** (cấp 2),
    /// hoặc **"R"** từ chối.
    /// </summary>
    public string HTCSignCcMnStatus { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #183 parity `DMS40_DlrCtr_CancelMinutes_Update_New20181115` (DMS40/0.34.Contract.cs:7378) =====
    /// <summary>Đường dẫn file biên bản (`FilePath`) — cột DUY NHẤT mà lệnh `_Update` cho phép sửa,
    /// và chỉ khi client khai báo nó trong mask `Ft_Cols_Upd`.</summary>
    public string? FilePath { get; set; }
    /// <summary>Nhật ký sửa cuối — `_Update` LUÔN ghi cặp này, kể cả khi mask rỗng.</summary>
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Hủy NH phát hành bảo lãnh MD của HĐ đại lý DMS40 (DMS40_DlrCtr_CancelBankMD) — port 1:1 FrmDMS40_DlrCtr_CancelBankMD (2010.HTC/Sales/DMS40).</summary>
public sealed class DmsCancelBankMD
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CancelBankMDNo { get; set; } = "";
    public string DlrCtrNo { get; set; } = "";
    public string? BankCodeMD { get; set; }
    public string? Remark { get; set; }
    public string FlagIsDelete { get; set; } = "0";
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #182 parity cum `DMS40_DlrCtr_CancelBankMD_*` (DMS40/0.34.Contract.cs, csproj 125) =====
    /// <summary>
    /// 🔴 Trạng thái biên bản huỷ NH bảo lãnh MD (`CancelBankMDStatus`, `TConst.CancelBankMDStatus`):
    /// "N" · **"P" chờ** · "C" huỷ · **"A" đã duyệt** · "A1" · "A2" · **"F" hoàn tất** (+ "R" từ chối).
    /// `_Finish` và `_Reject` đều vào từ **"A"**.
    /// </summary>
    public string CancelBankMDStatus { get; set; } = "P";
    /// <summary>Mốc HOÀN TẤT (`FinishDTime`/`FinishBy`) — bước này gỡ NH bảo lãnh khỏi hợp đồng.</summary>
    public DateTime? FinishDTime { get; set; }
    public string? FinishBy { get; set; }
    /// <summary>Mốc TỪ CHỐI (`RejectDTime`/`RejectBy`).</summary>
    public DateTime? RejectDTime { get; set; }
    public string? RejectBy { get; set; }
    /// <summary>Ghi chú của đại lý (`RemarkDlr`) — nguồn ghi ở CẢ hai lệnh, KHÁC cột `Remark` lúc tạo.</summary>
    public string? RemarkDlr { get; set; }
    /// <summary>Nguồn ghi ĐỒNG THỜI hai cặp: `LUDTime`/`LUBy` (sửa lần cuối, nghiệp vụ) và
    /// `LogLUDateTime`/`LogLUBy` (nhật ký kỹ thuật) — xem luật C0-ducentesimusvicesimus.</summary>
    public DateTime? LUDTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Hợp đồng đại lý DMS40 (DMS40_CT_DealerContract) — port 1:1 FrmDMS40_CT_DealerContractHTC_New/FrmMngDMS40 (2010.HTC/Sales/DMS40). HĐ đại lý ký 2 bên: A=HTC, B=đại lý.</summary>
public sealed class DmsDealerContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrCtrNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? ContractDate { get; set; }
    /// <summary>
    /// 🔴 Ký bên B — đại lý (`DMS40_CT_DealerContract.DlrSignStatus`) theo `TConst.**DlrSignStatus**`
    /// (`Const.Main.DMS40.cs:91-99`): "N" · **"P" chờ** · "C" huỷ · **"A" đã duyệt** · "A1" · "A2" · "F".
    /// ⚠️ Port cũ dùng **"S"** — giá trị KHÔNG thuộc bộ hằng này (nguồn `DlrApprove` gán `Approved` = "A").
    /// </summary>
    public string DlrSignStatus { get; set; } = "P";
    /// <summary>
    /// 🔴 Ký bên A — HTC (`HTCSignStatus`) theo `TConst.**HTCSignStatus**` (108-117):
    /// "N" · "P" · "C" · "A" · **"A1" duyệt cấp 1** · **"A2" duyệt cấp 2** · "F" · **"R" từ chối**.
    /// ⚠️ Port cũ P→"S" một bước: mất **hai cấp duyệt của HTC** và mất cả nhánh **từ chối "R"**.
    /// </summary>
    public string HTCSignStatus { get; set; } = "P";
    /// <summary>
    /// 🔴 Trạng thái hợp đồng (`DlrCtrStatus`) theo `TConst.**DlrCtrStatus**` (147-153) —
    /// **KHÁC HẲN** `DlrCtrStatus1` (P/A/C/F) của `Dlr_Contract` ở cụm HĐ bán lẻ:
    /// **"NS" chưa ký · "S" đã ký · "AJ" đã điều chỉnh · "C" huỷ**.
    /// ⚠️ Port cũ `Draft/Signed/Cancelled` sai cả ba, và **tự suy** "cả hai bên ký ⇒ Signed";
    /// nguồn `HTCApprove2` vẫn giữ `DlrCtrStatus = NotSign` sau khi HTC duyệt cấp 2.
    /// </summary>
    public string DlrCtrStatus { get; set; } = "NS";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DlrApprDTime { get; set; }
    /// <summary>#165 — Người duyệt bên B (`DlrApprBy`); nguồn luôn ghi cặp `DlrApprDTime`/`DlrApprBy`.</summary>
    public string? DlrApprBy { get; set; }
    public DateTime? HTCAppr2DTime { get; set; }
    public string? BankCodeMD { get; set; }         // NH phát hành bảo lãnh MD — port FrmDMS40_SelectedBankMD
    public string FlagDlrCtrAdjust { get; set; } = "0"; // HĐ điều chỉnh
    /// <summary>
    /// 🔴 Số HỢP ĐỒNG GỐC mà bản này điều chỉnh (`DMS40_CT_DealerContract.DlrCtrNoParent`).
    /// Nguồn `_DlrApproveAdjust` (0.34.Contract.cs:4410-4640) chạy **HAI câu update**:
    /// · `on t.DlrCtrNo = f.DlrCtrNo` → HĐ **con** nhận `DlrSignStatus`/`DlrCtrStatus` mới;
    /// · `on t.DlrCtrNo = f.**DlrCtrNoParent**` → HĐ **gốc** bị đánh **`DlrCtrStatus = "AJ"`** (Adjusted).
    /// ⚠️ Không có cột này thì trạng thái **"AJ"** của `TConst.DlrCtrStatus` **không có đường vào** —
    /// đúng khoảng trống mà lượt #83 đã ghi nợ.
    /// </summary>
    public string? DlrCtrNoParent { get; set; }

    // ===== #164 parity HỌ HÀM **KHÔNG-điều-chỉnh** (`_HTCApprove1X` · `_HTCApprove2X_New20190531` ·
    //   `_HTCRejectX`, DMS40/zTemp.0.34.Contract.cs:318/769/1115, csproj 129) =====
    /// <summary>Thời điểm/người duyệt cấp 1 của bên A (`HTCAppr1DTime`/`HTCAppr1By`).</summary>
    public DateTime? HTCAppr1DTime { get; set; }
    public string? HTCAppr1By { get; set; }
    /// <summary>Người duyệt cấp 2 (`HTCAppr2By`) — port cũ chỉ có thời điểm.</summary>
    public string? HTCAppr2By { get; set; }
    // ===== #207 parity: đối chiếu TỪNG CỘT với `DMS40_CT_DealerContract_SaveX_New20190404`
    //       (DMS40/0.34.Contract.cs:1442) — nguồn ghi 33 cột; entity thiếu đúng 2 cột mốc huỷ dưới đây.
    /// <summary>Mốc HUỶ của đại lý (`CancelDTime`/`CancelBy`) — `DlrCancel_New20190404` ghi cùng lúc với
    /// `LogLU*` và `LUDTime`/`LUBy` (cả ba cặp nhận CÙNG một giá trị).</summary>
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    /// <summary>Thời điểm/người TỪ CHỐI (`RejectDTime`/`RejectBy`) — nguồn dùng CẶP CỘT RIÊNG, không dùng chung với duyệt.</summary>
    public DateTime? RejectDTime { get; set; }
    public string? RejectBy { get; set; }
    /// <summary>Đường dẫn file hợp đồng đã ký — `_HTCApprove2X` upload file rồi MOVE sang thư mục đích và ghi lại cột này.</summary>
    public string? FilePath { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    // ===== #166 parity `DMS40_CT_DealerContract_SaveX_New20190404` (DMS40/0.34.Contract.cs:1442, csproj 125) =====
    /// <summary>🔴 Loại điều khoản thanh toán của hợp đồng (`DCPType`) — nguồn guard bằng
    /// `DlrCtr_PaymentType_CheckDB(..., Active, Active)`: mã PHẢI tồn tại trong bảng điều khoản và còn hiệu lực.
    /// Đây chính là bảng `PaymentTermMst` (`/api/paymentterms`) đã port.</summary>
    public string? DCPType { get; set; }
    /// <summary>Tổng tiền hợp đồng — nguồn khởi tạo **cứng 0.0** lúc lưu, tính lại ở bước khác.</summary>
    public decimal TotalAmount { get; set; }
    /// <summary>Ngày/người TẠO (`CreateDTime`/`CreateBy`) — 🔴 khi lưu ĐÈ bản ghi cũ, nguồn **giữ nguyên**
    /// giá trị cũ, chỉ điền mới khi bản ghi chưa tồn tại.</summary>
    public DateTime? CreateDTime { get; set; }
    public string? CreateBy { get; set; }
    /// <summary>Ngày/người SỬA LẦN CUỐI (`LUDTime`/`LUBy`) — khác cặp `LogLU*` (nhật ký kỹ thuật).</summary>
    public DateTime? LUDTime { get; set; }
    public string? LUBy { get; set; }
    /// <summary>
    /// 🔴 Bộ SÁU cột điều khoản thanh toán chép từ `Mst_PaymentTerm` của **xe ĐẦU TIÊN**, và nguồn bắt
    /// **mọi xe trong hợp đồng phải cùng một `PMTermNo`** (lỗi `PaymentTermNotMatch`) — tức hợp đồng
    /// đại lý là **thuần nhất về điều khoản thanh toán**.
    /// </summary>
    public string? PMTermNo { get; set; }
    public decimal? DepositPercent { get; set; }
    public decimal? GuaranteePercent { get; set; }
    public int? GuaranteeDays { get; set; }
    public int? DepositDutyEndDays { get; set; }
    public int? GuaranteeEndDays { get; set; }
}

/// <summary>
/// Dòng hợp đồng đại lý DMS40 (`DMS40_CT_DealerContractDetail` — cột lấy từ
/// `DMS40_CT_DealerContract_SaveX`, DMS40/0.34.Contract.cs:1998-2012).
/// 🔴 Bảng này là ĐẦU VÀO của bước duyệt cấp 2: `_HTCApprove2X` vừa cập nhật `DlrCtrStatusDtl`
/// vừa **duyệt qua từng dòng để gắn hợp đồng vào xe** (`Car_Car`).
/// </summary>
public sealed class DmsDealerContractDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrCtrNo { get; set; } = "";
    public DateTime? ApprovedDate { get; set; }
    public string CarId { get; set; } = "";
    public string? OriginNo { get; set; }
    public double ProductionYear { get; set; }
    public decimal UnitPrice { get; set; }
    /// <summary>Trạng thái RIÊNG của dòng — `_HTCApprove2X` đặt `= DlrCtrStatus` của header (⇒ "S").</summary>
    public string? DlrCtrStatusDtl { get; set; }
    /// <summary>Cờ đã thanh toán đặt cọc (`FlagDepositPmt`).</summary>
    public string? FlagDepositPmt { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Công văn bảo lãnh/claim đại lý (GrtClaim + Detail) — port 1:1 FrmNewGrtClaim/FrmMngGrtClaim (2010.HTC/Sales/GrtClaim). Công văn bảo lãnh lô xe theo đại lý + phép nhận.</summary>
public sealed class GrtClaim
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GrtClaimNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? ContractDate { get; set; }
    public string FlagisHTC { get; set; } = "";   // phép nhận: HTC / DL
    /// <summary>
    /// ⚠️ **Nguồn KHÔNG có cột trạng thái cho HEADER**: bảng `Pmt_GrtClaim` chỉ gồm
    /// `GrtClaimNo`/`CreatedBy`/`CreatedDate`/`DealerCode`/`Remark` (BizHTC.Payment.cs:2705-2712),
    /// và `grep "GrtClaimStatus"` toàn hệ = **0 hit**.
    /// ⇒ `Draft/Issued/Cancelled` của port cũ là **trạng thái BỊA hoàn toàn**.
    /// Trục trạng thái THẬT nằm ở **DÒNG**: <see cref="GrtClaimDetail.VinSignStatus"/>.
    /// Giữ cột này để đọc dữ liệu cũ, **không dùng làm điều kiện nghiệp vụ**.
    /// </summary>
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? IssuedAt { get; set; }

    // ===== #172 parity cum `Pmt_GrtClaim_*` (DataWH/Biz.HTC.WH.My.cs, csproj 273) =====
    /// <summary>
    /// 🔴 Trang thai KY cua HEADER cong van (`Pmt_GrtClaim.SignStatus`, `TConst.SignStatus`):
    /// **"P" chua ky · "A" da ky · "C" huy**.
    /// ⚠️ SUA KET LUAN SAI cua luot truoc: comment cu khang dinh *"nguon KHONG co cot trang thai cho header,
    /// grep GrtClaimStatus = 0 hit"*. Cot co that — chi **khong ten `GrtClaimStatus`** ma ten `SignStatus`.
    /// Bon ham `_Approve` (5981) · `_DelMulti` (6322) · `_CancelMulti` (6549) · `_RejectGrtClaim` (7745)
    /// deu guard/ghi chinh cot nay. Grep sai TEN nen ket luan sai SU TON TAI.
    /// (Truc trang thai o DONG — <see cref="GrtClaimDetail.VinSignStatus"/> — van dung, hai truc SONG SONG.)
    /// </summary>
    public string SignStatus { get; set; } = "P";
    /// <summary>Moc KY (`SignDate`/`SignBy`) va duong dan file da ky (`FileSigned`) — `_Approve` ghi cung luot.</summary>
    public DateTime? SignDate { get; set; }
    public string? SignBy { get; set; }
    public string? FileSigned { get; set; }
    /// <summary>Moc HUY (`CancelDate`/`CancelBy`) — `_CancelMulti` ghi khi chuyen "A" sang "C".</summary>
    public DateTime? CancelDate { get; set; }
    public string? CancelBy { get; set; }
    /// <summary>Moc gui thong bao TU CHOI (`_RejectGrtClaim`) — chi cho cong van DA HUY ("C").</summary>
    public DateTime? RejectDate { get; set; }
    public string? RejectBy { get; set; }
    public string? RejectRemark { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}
public sealed class GrtClaimDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long GrtClaimId { get; set; }
    public string VIN { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public string? BankCode { get; set; }
    /// <summary>
    /// 🔴 Trạng thái ký của TỪNG VIN (`Pmt_GrtClaimDetail.VinSignStatus`) theo `TConst.VinSignStatus`
    /// (`Const.Main.DMS40.cs:531-536`): **"P" chưa ký · "A" đã ký · "C" huỷ**.
    /// Đây là trục trạng thái DUY NHẤT có thật của cụm công văn bảo lãnh — port cũ thiếu hẳn,
    /// thay bằng một trục bịa ở header.
    /// Nguồn tạo dòng ở "P" (Biz.HTC.WH.My.cs:5572), ký ⇒ "A" (6106), huỷ ⇒ "C" (6819).
    /// </summary>
    public string VinSignStatus { get; set; } = "P";

    // ===== #172: nguon ghi LogLU* cho tung DONG o ca _Approve lan _CancelMulti =====
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Đề nghị chiết khấu thanh toán sớm BL/LC theo VIN (Req_PaymentDiscount + Dtl — port 1:1 FrmReq_PaymentDiscount/FrmMngReq_PaymentDiscount, 2010.HTC/Sales):
/// 3 giai đoạn (Phase1/2/3), mỗi giai đoạn: AmountPhase (gốc BL/LC còn lại) × DiscountPercentPhase/100 × DiscountDateNumberPhase/365 = DiscountPricePhase (chiết khấu được hưởng khi trả sớm).
/// TotalDiscountPrice = Σ 3 giai đoạn. Status: Draft(đại lý lập)→Sent(gửi HTC)→Approved/Rejected.</summary>
public sealed class ReqPaymentDiscount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? PGDateEndFrom { get; set; }   // ngày tất toán BL từ
    public DateTime? PGDateEndTo { get; set; }     // ngày tất toán BL đến
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    public DateTime? DecidedAt { get; set; }
}

/// <summary>Dòng VIN trong đề nghị chiết khấu TT sớm — port 1:1 grid FrmReq_PaymentDiscount, 2010.HTC.</summary>
public sealed class ReqPaymentDiscountLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string VIN { get; set; } = "";
    public string? CarId { get; set; }
    public DateTime? PaymentEndDatePhase1 { get; set; }
    public decimal AmountPhase1 { get; set; }
    public int DiscountDateNumberPhase1 { get; set; }
    public decimal DiscountPercentPhase1 { get; set; }
    public decimal DiscountPricePhase1 { get; set; }   // tự tính
    public DateTime? PaymentEndDatePhase2 { get; set; }
    public decimal AmountPhase2 { get; set; }
    public int DiscountDateNumberPhase2 { get; set; }
    public decimal DiscountPercentPhase2 { get; set; }
    public decimal DiscountPricePhase2 { get; set; }   // tự tính
    public DateTime? PaymentEndDatePhase3 { get; set; }
    public decimal AmountPhase3 { get; set; }
    public int DiscountDateNumberPhase3 { get; set; }
    public decimal DiscountPercentPhase3 { get; set; }
    public decimal DiscountPricePhase3 { get; set; }   // tự tính
    public decimal TotalAmount { get; set; }
    public decimal TotalDiscountPrice { get; set; }    // tự tính = Σ 3 giai đoạn
}

/// <summary>Yêu cầu đóng thùng (Sto_CBReq + Detail) — port 1:1 FrmNewCBReq (2010.HTC/Sales/Purchase). Đóng thùng lô xe xuất khẩu theo VIN, kho đi→kho đến + loại đóng thùng.</summary>
public sealed class CBReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CBReqNo { get; set; } = "";
    public string Status { get; set; } = "Draft"; // Draft → Confirmed / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }

    // ===== #179 parity `Sto_CBReqApprove_New20181119` (DataWH/Biz.HTC.WH.cs:115992, csproj 272) =====
    /// <summary>Lý do duyệt/bỏ duyệt (`Sto_CBReq.Remark`) — nguồn nhận `strRemark` và ghi ở CẢ HAI ngả.</summary>
    public string? Remark { get; set; }
}
public sealed class CBReqDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CBReqId { get; set; }
    public string VIN { get; set; } = "";
    public string? StorageCodeFrom { get; set; }
    public string StorageCodeTo { get; set; } = "";
    public string? TypeCB { get; set; }
    public string? Remark { get; set; }

    /// <summary>#179 — Trạng thái RIÊNG của dòng (`Sto_CBReqDetail.CBReqDtlStatus`): nguồn cập nhật
    /// `'A'` khi duyệt và `'R'` khi bỏ duyệt, đồng bộ với header.</summary>
    public string? CBReqDtlStatus { get; set; }
}

/// <summary>Sắp xếp/chuyển kho (Sto_StorageRearrange + Detail) — port 1:1 FrmNewSC (2010.HTC/Sales/Purchase). Chuyển vị trí lưu kho lô xe theo VIN, kho hiện tại→kho đến.</summary>
public sealed class StorageRearrange
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SCNo { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái phiếu (`Sto_StorageRearrange.RearrangeStatus`) theo `TConst.Stage`:
    /// **"P" chờ duyệt → "A1" duyệt cấp 1 → "A2" duyệt cấp 2 · "R" từ chối**.
    /// ⚠️ Port cũ `Draft/Confirmed/Cancelled/Approved1`: sai mã, và **"Confirmed"/"Cancelled" đều BỊA** —
    /// nguồn không có xác nhận/huỷ, chỉ có hai cấp duyệt (`StorageStorageRearrangeApprove1/Approve2`).
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? Approved1At { get; set; }
    public DateTime? Approved2At { get; set; }
    public string? ApprovedBy1 { get; set; }
    public string? ApprovedBy2 { get; set; }
    /// <summary>Ghi chú của người duyệt — nguồn ghi ở CẢ hai cấp và CẢ hai nhánh (duyệt lẫn không duyệt).</summary>
    public string? Remark { get; set; }
}
public sealed class StorageRearrangeDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StorageRearrangeId { get; set; }
    public string VIN { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái của TỪNG XE (`Sto_StorageRearrangeDetail.RearrangeDtlStatus`) — trục port cũ THIẾU.
    /// Duyệt cấp 1 lan xuống 'A1' (không duyệt ⇒ 'R'); duyệt cấp 2 lan xuống 'A2'
    /// (**bỏ duyệt ⇒ trả dòng về 'P'**, không phải giữ nguyên).
    /// </summary>
    public string RearrangeDtlStatus { get; set; } = "P";
    /// <summary>Ngày dự kiến kết thúc chuyển kho theo dòng (`ExpectedEndDate`) — nguồn guard phải **>=**
    /// `ExpectedStartDate` khi sửa (`StorageStorageRearrangeDetailUpdate`).</summary>
    public DateTime? ExpectedStartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public string? StorageCodeFrom { get; set; }
    public string StorageCodeTo { get; set; } = "";
    public string? Remark { get; set; }

    // ===== #159 side-effect `Sto_DlvMinutes_Approve_New20190416` (Biz.HTC.WH.cs:138340, csproj 272) =====
    // Duyệt biên bản giao xe GHI NGƯỢC "ngày xuất kho" lên CHỨNG TỪ NGUỒN của xe. Bốn nhánh theo loại
    // chứng từ, mỗi nhánh một CỘT KHÁC TÊN — đó là lý do port cũ bỏ sót cả ba.
    /// <summary>Ngày xuất kho thực tế của lệnh điều chuyển (`RearrangeOutDate`) — nguồn ghi khi
    /// DUYỆT biên bản giao xe, và chỉ khi `RearrangeDtlStatus` đang là "A2" hoặc "F".</summary>
    public DateTime? RearrangeOutDate { get; set; }

    // ===== #170 parity `Sto_DlvMinutes_UpdateDlvEndDate_New20181115` (BizHTC.Storage.DlvMinutes.cs:9329) =====
    /// <summary>Ngày điều chuyển XONG (`RearrangeEndDate`) — nhận từ ngày nhận xe của biên bản giao xe.</summary>
    public DateTime? RearrangeEndDate { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string? ConfirmBy { get; set; }
}

/// <summary>Đề nghị bảo hiểm (Ins_InsuranceReq + Dtl) — port 1:1 FrmNewInsuranceReq (2010.HTC/Sales/Purchase). Đề nghị mua bảo hiểm cho lô VIN theo hãng + loại hình.</summary>
/// <summary>
/// 🔴 Master CÔNG TY BẢO HIỂM (`Mst_InsuranceCompany` — hệ `ERP.V15.DMSSales.Real`, **chỉ có trên máy 150**).
/// </summary>
public sealed class MstInsuranceCompany
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsCompanyCode { get; set; } = "";
    public string? InsCompanyName { get; set; }
    /// <summary>#569 §12 Bốn cột hồ sơ hãng bảo hiểm — nguồn `SerInsuranceDebitSearch` chọn thêm
    /// `si.Address`, `si.Fax`, `si.Website` (hai cột sau có chú thích *"huongkt add (menu danh sách hãng
    /// bảo hiểm nợ)"*) và `si.InsVieName` dùng làm tên hiển thị.</summary>
    public string? Address { get; set; }
    public string? Tel { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 Master LOẠI HÌNH BẢO HIỂM (`Mst_InsuranceType`).
/// ⚠️ **Khoá là BỘ BA**: `InsCompanyCode` + `InsTypeCode` + **`EffectiveDate`**
/// (`TERP.BizInsurance/InsReq.cs:86-97`) ⇒ mỗi công ty BH có nhiều **phiên bản theo NGÀY HIỆU LỰC**
/// cho cùng một mã loại hình. Bỏ `EffectiveDate` khỏi khoá là **mất lịch sử biểu phí**.
/// </summary>
public sealed class MstInsuranceType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsCompanyCode { get; set; } = "";
    public string InsTypeCode { get; set; } = "";
    /// <summary>Ngày hiệu lực — PHẦN CỦA KHOÁ, không phải cột phụ.</summary>
    public DateTime EffectiveDate { get; set; }
    public string? InsTypeName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public sealed class InsuranceReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsReqNo { get; set; } = "";
    public string InsCompanyCode { get; set; } = "";  // hãng BH
    public string InsTypeCode { get; set; } = "";      // loại hình BH
    /// <summary>
    /// 🔴 Trạng thái yêu cầu (`Ins_InsuranceReq.InsReqStatus`) theo `TConst.Stage`:
    /// **"P" chờ duyệt → "A" duyệt / "R" từ chối**. Dùng `Stage.Approved` = **"A"**, KHÔNG phải A1/A2.
    /// ⚠️ Port cũ `Draft → Confirmed → Approved/Rejected`: **"Confirmed" là bước BỊA** (nguồn duyệt
    /// thẳng từ "P"), và **"Cancelled" cũng bịa** — nguồn không có hàm huỷ, chỉ có **XOÁ**
    /// (`Ins_InsuranceReqDelete_New201811119`).
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
    /// <summary>Ngày duyệt/từ chối (`ApprovedDate`) — nguồn ghi cho CẢ hai nhánh, không riêng nhánh duyệt.</summary>
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    /// <summary>Ghi chú của người duyệt (`Ins_InsuranceReq.Remark`) — ghi cả khi duyệt lẫn khi từ chối.</summary>
    public string? Remark { get; set; }
}
public sealed class InsuranceReqDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long InsuranceReqId { get; set; }
    public string VIN { get; set; } = "";
    public DateTime? ExpectedStartDate { get; set; }
    public decimal InsAmount { get; set; }
    public int InsuranceDay { get; set; }
    public string? LocationFrom { get; set; }
    public string? LocationTo { get; set; }
    public decimal Price { get; set; }
    public decimal Rate { get; set; }
    public string? TransporterCode { get; set; }
    public string? Remark { get; set; }
    /// <summary>
    /// 🔴 Trạng thái của TỪNG XE (`Ins_InsuranceReqDtl.InsReqDtlStatus`) — trục port cũ THIẾU HẲN.
    /// Tạo ở "P"; khi duyệt/từ chối yêu cầu, nguồn **lan xuống MỌI dòng** bằng một câu update ('A'/'R').
    /// Guard: sửa dòng chỉ khi **"P"**; xoá dòng khi **"P" hoặc "A"**.
    /// </summary>
    public string InsReqDtlStatus { get; set; } = "P";
}

/// <summary>Cập nhật vị trí xe trong bãi (Vin.Location) — port 1:1 FrmLocationCar (2010.HTC/Sales/Logistic). Cập nhật vị trí lưu bãi theo VIN.</summary>
public sealed class CarLocation
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? LocationOld { get; set; }
    public string Location { get; set; } = "";   // vị trí mới
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (phát hiện #55).
/// `ReqRedeem`/`ReqRedeemDtl` và <see cref="RedeemRequest"/>/<see cref="RedeemRequestLine"/>
/// **cùng map một bảng nguồn `RD_ReqRedeem`/`RD_ReqRedeemDtl`** (nguồn chỉ có DUY NHẤT 1 bảng —
/// kiểm bằng grep `SaveData("RD_ReqRedeem"`).
/// Nghiệp vụ nay dùng <see cref="RedeemRequest"/>; endpoint `/api/reqredeems` đã trỏ sang bảng đó.
/// Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
/// </summary>
public sealed class ReqRedeem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqDMNo { get; set; } = "";
    public string Status { get; set; } = "Draft";   // Draft → Done
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
}
public sealed class ReqRedeemDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqRedeemId { get; set; }
    public string VIN { get; set; } = "";
    public string? CarId { get; set; }
    public string? DealerCode { get; set; }
    public string? TypeDMReq { get; set; }       // loại đề nghị giải chấp
    public string? BankCode { get; set; }        // ngân hàng bàn giao (không được HTC.HO)
}

/// <summary>Đặt hàng sản xuất (MnfPl_Order + Dtl) — port 1:1 FrmDatHangSX/FrmQLDatHangSX (2010.HTC/Sales/WorkOrder). Đơn đặt hàng sản xuất theo model/spec/màu/SL + thứ tự SX.</summary>
public sealed class MnfPlOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderNo { get; set; } = "";
    public string OrdType { get; set; } = "";     // loại đơn hàng
    public string? OrdMonth { get; set; }          // tháng đặt hàng (yyyy/MM)
    public string? Remark { get; set; }
    public string Status { get; set; } = "Draft"; // Draft → Sent
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
}
public sealed class MnfPlOrderDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MnfPlOrderId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? SpecDescription { get; set; }
    public string? ColorCode { get; set; }
    public int Quantity { get; set; } = 1;
    public int MnfPlIdx { get; set; }            // thứ tự SX (> 0)
}

/// <summary>Thiết bị gắn trên xe (Mng_Device_Car) — port 1:1 FrmMng_Device_Car/_Upd (2010.HTC/Sales). Gán loại thiết bị + hóa đơn nhập cho VIN.</summary>
public sealed class DeviceCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public string DeviceTypeCode { get; set; } = "";  // loại thiết bị
    public string? InputInvoiceNo { get; set; }
    public DateTime? InputInvoiceDate { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // ===== #162 parity Mng_Device_Car (Biz.HTC.WH.cs:35606) =====
    // 🔴 Nguồn KHÔNG nhận thiết bị từ đầu vào: nó JOIN `Mst_DeviceType_Spec` theo `Car_VIN.ActualSpec`
    //    (lọc `FlagActive = '1'`) để SUY RA thiết bị của xe. Tức lập packing list là tự gắn thiết bị
    //    theo spec THỰC TẾ của xe, không phải spec đặt hàng.
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (ca thứ 5, phát hiện #58 bằng sweep tên bảng nguồn).
/// `TestCarRegister`/`TestCarRegisterCar` và <see cref="CarTestCar"/>/<see cref="CarTestCarDtl"/>
/// **cùng map một bảng nguồn `Car_TestCar`/`Car_TestCarDtl`**.
/// Endpoint `/api/testcarregs` đã trỏ sang <see cref="CarTestCar"/>. Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
/// </summary>
public sealed class TestCarRegister
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TestCarCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string Status { get; set; } = "Draft";   // Draft → Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    public string? RejectReason { get; set; }
}
public sealed class TestCarRegisterCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TestCarRegisterId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string StatusDtl { get; set; } = "P";
}

/// <summary>Lịch sử đổi màu xe (Rpt_CarColorChangeHistory) — port 1:1 FrmChange_CarColor (2010.HTC/Sales). Đổi màu xe theo VIN, lưu màu cũ → màu mới.</summary>
public sealed class CarColorChange
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string ColorCodeOld { get; set; } = "";
    public string ColorCodeNew { get; set; } = "";
    public DateTime ChangedAt { get; set; } = DateTime.Now;
}

/// <summary>Hợp đồng nguyên tắc (Rpt_PrincipleContract) — port 1:1 FrmPrincipleContractNew/FrmMngPrincipleContract (2010.HTC/Sales). HĐ nguyên tắc đại lý: ngân hàng, người đại diện, thời hạn.</summary>
public sealed class PrincipleContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string PrincipleContractNo { get; set; } = "";
    public string BankInfo { get; set; } = "";
    public DateTime PrincipleContractDate { get; set; }
    public DateTime PrincipleContractExpectedDate { get; set; }
    public string Representative { get; set; } = "";
    public string JobTitle { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Master chính sách bán hàng (SPL_SalesPolicyMst) — port 1:1 FrmMstPolicy_New/Mng (2010.HTC/Sales). Chính sách hỗ trợ bán, dùng bởi duyệt SO. Header + dòng đại lý/năm SX/tiền hỗ trợ.</summary>
public sealed class SalesPolicyMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SPSRCode { get; set; } = "";      // mã chính sách (auto)
    public string SPNo { get; set; } = "";           // số hiệu văn bản
    public string? SPSRType { get; set; }            // loại chính sách
    public string? SPSRRoot { get; set; }
    public string? FormBusinessSupportCode { get; set; } // hình thức hỗ trợ
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string FlagMstValid { get; set; } = "1"; // trạng thái hiệu lực
    public string? Remark { get; set; }
    public string? FilePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public sealed class SalesPolicyMstDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PolicyId { get; set; }
    public string? DealerCode { get; set; }
    public string? YearOfManufacture { get; set; }
    public decimal AmountSupport { get; set; }
    public string? Remark { get; set; }
}

/// <summary>Hỗ trợ bán lẻ theo VIN gắn với chính sách bán hàng (SPL_SPSupportRetail) — port 1:1 FrmPolicySales_Mng (2010.HTC/Sales).
/// Nguồn gốc là 1 tra cứu tổng hợp (join SO/DO/HTCInvoice/PaymentReqDiscount để tính DateFullStatus="ngày đủ điều kiện"); ở đây ĐƠN GIẢN HOÁ thành trường nhập tay DateFullStatus (không tự tính từ join đa bảng — quá sâu để trace 1:1 trong 1 fire) + tham chiếu HTCInvoiceNo/HTCInvoiceDate nhập trực tiếp.</summary>
public sealed class SPSupportRetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string SPSRCode { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? SpecCode { get; set; }
    public string? ModelCode { get; set; }
    public string? PRDiscountNo { get; set; }
    public decimal AmountSupport { get; set; }
    public DateTime DateSupport { get; set; }
    public DateTime? DateFullStatus { get; set; }
    public string? HTCInvoiceNo { get; set; }
    public DateTime? HTCInvoiceDate { get; set; }
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Master VIN tối giản (nguồn Car_Vin+Car_Car join, cùng nguồn đã dùng cho MiniVehicle) — chỉ phục vụ guard tồn tại VIN cho SPSupportRetail/... KHÔNG phải Car_VIN đầy đủ như MiniVehicle.</summary>
public sealed class CarVinMaster
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? ModelCode { get; set; }
    public string? SpecCode { get; set; }
    public string? DealerCode { get; set; }

    /// <summary>
    /// 🔴 #B04: Mã màu của xe (`Car_VIN.ColorCode`). Bộ ba định danh xe của nguồn là
    /// (`ModelCode`, `SpecCode`, `ColorCode`) — port cũ chỉ có hai, nên **mọi báo cáo join
    /// `Mst_CarColor` theo `cv.ModelCode + cv.ColorCode`** đều không thực hiện được.
    /// Bằng chứng dùng thật: `Rpt_CarDeliveryNotAddressDealerRegis_New20181115`
    /// (`BizHTC.ZTempGPS.cs:8848-8850`) `inner join Mst_CarColor mcc on cv.ModelCode = mcc.ModelCode
    /// and cv.ColorCode = mcc.ColorCode`.
    /// </summary>
    public string? ColorCode { get; set; }

    // ===== #160 parity + side-effect `RD_ReqInvoiceDtlApprove_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:128014, csproj 272; vùng md5 1e58bf10 khớp 2 máy) =====
    /// <summary>Ngày KẾT THÚC thế chấp (`Car_Vin.MortageEndDate`) — nguồn đặt = hôm nay khi duyệt.</summary>
    public DateTime? MortageEndDate { get; set; }
    /// <summary>Ngân hàng nhận BÀN GIAO hồ sơ xe (`Car_Vin.HandOverBankCode`) — lấy từ
    /// `Pmt_Guarantee.BankCodeMonitor` của bảo lãnh còn hiệu lực duy nhất.</summary>
    public string? HandOverBankCode { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }

    // ===== #161 parity `RD_ReqInvoiceCreate_New20240617` (Biz.HTC.WH.cs:127483) =====
    /// <summary>
    /// 🔴 Ngày GIẢI CHẤP của xe (`Car_VIN.RedeemDate`). Bản 2024 bắt buộc cột này **PHẢI CÓ**
    /// mới cho tạo đề nghị giao hồ sơ (`InvalidRedeemDate`) — bản 2018 không kiểm.
    /// </summary>
    public DateTime? RedeemDate { get; set; }

    // ===== #164 parity khối `// Update Car_Car:` trong `_HTCApprove2X_New20190531` (zTemp.0.34.Contract.cs:1006) =====
    /// <summary>🔴 Số hợp đồng đại lý DMS40 đã GẮN vào xe (`Car_Car.DlrCtrNo`/`DealerContractNo`) —
    /// nguồn ghi khi bên A duyệt cấp 2, lấy từ **từng dòng** `DMS40_CT_DealerContractDetail`.</summary>
    public string? DlrCtrNo { get; set; }
    /// <summary>Cờ "xe đã thuộc hợp đồng đại lý DMS40" (`FlagDealerContractDMS40`) — nguồn gán cứng "1".</summary>
    public string? FlagDealerContractDMS40 { get; set; }

    // ===== #197 parity `CarDeliveryOrderApprove1_New20181119` (DataWH/Biz.HTC.WH.cs:50318) =====
    /// <summary>
    /// 🔴 Cờ "xe được phép ĐỔI VIN" (`Car_Car.FlagAllowChangeVIN`). Nguồn BẬT cờ này ("1") khi lệnh giao
    /// xe bị **TỪ CHỐI** ở duyệt cấp 1, và khi **XOÁ dòng xe** khỏi lệnh giao — tức trả xe về trạng thái
    /// còn đổi VIN được. Đặt ở đây theo tiền lệ #164 (`CarVinMaster` đang giữ cả các cột của `Car_Car`).
    /// </summary>
    public string? FlagAllowChangeVIN { get; set; }

    // ===== #201 parity `CarCarCancel_New20181119` (59995) / `CarCarReActive_New20181119` (60200) =====
    /// <summary>
    /// 🔴 Huỷ xe ở nguồn KHÔNG ghi vào bảng riêng — ghi thẳng **5 cột của `Car_Car`**:
    /// `FlagActive` ("0" khi huỷ / "1" khi kích hoạt lại), `CarCancelType` ("NONE" khi kích hoạt lại),
    /// `CarCancelRemark`, `CarCancelDate`, `CarCancelBy` (hai cột sau bị đặt NULL khi kích hoạt lại).
    /// Đặt ở đây theo tiền lệ #164/#197 (`CarVinMaster` đang giữ các cột của `Car_Car`).
    /// </summary>
    public string? FlagActive { get; set; }
    public string? CarCancelType { get; set; }
    public string? CarCancelRemark { get; set; }
    public DateTime? CarCancelDate { get; set; }
    public string? CarCancelBy { get; set; }

    /// <summary>#175 — Kho HIỆN TẠI của xe (`Car_VIN.StorageCodeCurrent`). Bước tự sinh lệnh giao lấy
    /// kho của dòng chi tiết TỪ CỘT NÀY, không phải kho khai báo trên phiếu.</summary>
    public string? StorageCodeCurrent { get; set; }
}

/// <summary>Điều kiện eligible chính sách hỗ trợ bán lẻ, gộp phẳng SPL_SalesPolicyMstDetail (DealerCode=null: áp dụng mọi đại lý) + SPL_SalesPolicyMstDetailDealer (DealerCode cụ thể) — phục vụ guard #4 SPSupportRetail.</summary>
public sealed class SalesPolicyEligibility
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SPSRCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string? DealerCode { get; set; }
}

/// <summary>Phiếu bảo trì xe lưu kho bãi (StoF_Maintain) — port 1:1 FrmMaintenanceSlipList/Detail (2010.HTC/Maintenance). Bảo dưỡng xe thành phẩm lưu kho, theo VIN.</summary>
public sealed class StoFMaintain
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SfMtnNo { get; set; } = "";       // số phiếu bảo trì
    /// <summary>Loại bảo trì — cột nguồn tên `StoFMtnType` (`insert into StoF_Maintain`,
    /// `BizHTC.StorageFG.Frm.cs:548`); giữ tên `MtnType` của MiniHTC để không phá dữ liệu đã có.</summary>
    public string MtnType { get; set; } = "";
    /// <summary>
    /// ⛔ #B07 DEPRECATED — `Draft`/`Done` là **trạng thái BỊA**: bảng nguồn `StoF_Maintain` không có cột nào
    /// tên `Status`. Trục thật là **HAI cột ĐỘC LẬP** <see cref="MtnStatus"/> và <see cref="MtnEvalStatus"/>.
    /// Giữ cột để không phá dữ liệu cũ; endpoint đã chuyển sang hai trục thật.
    /// </summary>
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }

    // ===== #B07 parity `StoF_Maintain` — vòng đời BỐN BƯỚC của nguồn (BizHTC.StorageFG.Frm.cs) =====
    //   Save (106) → Approve (740) → SaveEval (1011) → ApproveEval (1523).
    //   Port cũ chỉ có Save + "complete" ⇒ mất 2 bước ĐÁNH GIÁ và toàn bộ vết duyệt.
    /// <summary>Trục 1 — trạng thái DUYỆT PHIẾU (`MtnStatus`, `TConst.MtnStatus`): "P" chờ → "A" đã duyệt.
    /// `Approve` guard `MtnStatus="P"` **và** `MtnEvalStatus="P"` (:817-818).</summary>
    public string MtnStatus { get; set; } = "P";
    /// <summary>Trục 2 — trạng thái DUYỆT ĐÁNH GIÁ (`MtnEvalStatus`, `TConst.MtnEvalStatus`): "P" → "A".
    /// `SaveEval`/`ApproveEval` guard `MtnStatus="A"` **và** `MtnEvalStatus="P"` (:1091-1092 / :1601-1602).</summary>
    public string MtnEvalStatus { get; set; } = "P";
    /// <summary>Số lượng VIN trong phiếu (`QtyVIN`) — nguồn lưu thành cột, không đếm lại mỗi lần đọc.</summary>
    public int QtyVIN { get; set; }
    public DateTime? CreateDateTime { get; set; }
    public string? CreateBy { get; set; }
    /// <summary>Mốc sửa cuối (`LUDateTime`/`LUBy`) — nguồn gán **bằng chính mốc duyệt** ở cả hai bước
    /// duyệt (`t.LUDateTime = f.ApproveDateTime` :874 / `= f.ApproveEvalDateTime` :1657).</summary>
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? ApproveDateTime { get; set; }
    public string? ApproveBy { get; set; }
    public string? ApproveEvalBy { get; set; }
    public string? Remark { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    /// <summary>
    /// 🔴 #B06: Thời điểm DUYỆT ĐÁNH GIÁ phiếu bảo trì (`StoF_Maintain.APPROVEEVALDATETIME`).
    /// `FrmMaintenanceWarehouse.cs:99` ghi đè **giá trị hiển thị** của `MtnExtStartDTime` bằng chính cột này:
    /// `item.MtnExtStartDTime = listStoF_Maintain.Where(x => x.SF_MTNNO == item.SF_MtnNo).First().APPROVEEVALDATETIME;`
    /// ⇒ "ngày vào bảo dưỡng gia hạn" mà người dùng thấy là **ngày duyệt đánh giá của phiếu**, KHÔNG phải
    /// giờ bấm nút. Port cũ đặt `DateTime.Now` ⇒ sai nghiệp vụ.
    /// ⚠️ NỢ: `StoF_Maintain` của nguồn còn ~12 cột nữa mà lớp này chưa có (MTNSTATUS, MTNEVALSTATUS,
    ///    APPROVEDATETIME/BY, APPROVEEVALBY, CREATE*/LU*, QTYVIN, STOFMTNTYPE, REMARK, LOGLU*) —
    ///    thuộc phạm vi màn `FrmMaintenanceSlipList`, ghi nợ để lượt sau audit riêng.
    /// </summary>
    public DateTime? ApproveEvalDateTime { get; set; }
}
public sealed class StoFMaintainMain
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StoFMaintainId { get; set; }
    /// <summary>🔴 #B06: Số phiếu bảo trì (`SF_MtnNo`). Nguồn khoá dòng bằng **CẶP (`SF_MtnNo`, `VIN`)**
    /// — `update … on t.SF_MtnNo = f.SF_MtnNo and t.VIN = f.VIN`
    /// (`BizHTC.StorageFG.Frm.cs:2344-2345`) — nên chỉ có `VIN` là **khoá HẸP HƠN nguồn**:
    /// một VIN vào bảo trì nhiều lần thì port cũ ghi đè lẫn nhau.</summary>
    public string SfMtnNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string? MtnTp { get; set; }               // loại BT dòng
    public string? ModelCode { get; set; }
    public string? UserCodeMtn { get; set; }         // người bảo trì
    public string? StorageCodeInit { get; set; }     // kho ban đầu
    public string? StorageCodeCurrent { get; set; }  // kho hiện tại
    public string? MtnStatusMain { get; set; }       // trạng thái bảo trì
    public string? Remark { get; set; }

    // ===== #B06 HỢP NHẤT THỰC THỂ SONG TRÙNG (ca tiếp theo sau #56 `RD_ReqInvoice` và #60 `Sto_DlvMinutes`) =====
    // `MaintainExt` (endpoint `/api/maintext`) và lớp này **cùng port bảng nguồn `StoF_MaintainMain`**:
    // `MaintainExt` mang nhánh "bảo dưỡng GIA HẠN" của `FrmMaintenanceWarehouse`, lớp này mang nhánh
    // "dòng xe của phiếu bảo trì" của `FrmMaintenanceSlipDetail`. Giữ lớp này (đúng khoá + có
    // `MtnStatusMain`/`UserCodeMtn`/kho) và mang trọn nhóm `MtnExt*` sang.
    /// <summary>Người phụ trách bảo dưỡng gia hạn (`UserCodeMtnExt`) — khác <see cref="UserCodeMtn"/>.</summary>
    public string? UserCodeMtnExt { get; set; }
    public DateTime? MtnExtStartDTime { get; set; }
    public DateTime? MtnExtEndDTime { get; set; }
    public string? MtnExtRemark { get; set; }
    /// <summary>Trạng thái bảo dưỡng gia hạn (`MtnExtStatusMain`): NG chưa · IN đang · OUT xong.
    /// Nguồn hiển thị: rỗng ⇒ ép về **"NG"** (`FrmMaintenanceWarehouse.cs:95-96`).</summary>
    public string MtnExtStatusMain { get; set; } = "NG";
    /// <summary>🔴 Hai trục trạng thái RIÊNG của nguồn mà port cũ không có — `StoF_MaintainMain_CheckDB`
    /// nhận `strBeforeMtnStatusMainToCheck` và `strAfterMtnStatusMainToCheck` như hai danh sách ĐỘC LẬP
    /// (`BizHTC.StorageFG.Frm.cs:1781-1875`); vào/ra bảo dưỡng gia hạn **đòi `AfterMtnStatusMain = "0"`**.</summary>
    public string? BeforeMtnStatusMain { get; set; }
    public string? AfterMtnStatusMain { get; set; }
    public decimal? MapLatitude { get; set; }
    public decimal? MapLongitude { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
    // ⚠️ `CV_STOREDATE`/`CV_STORAGECODEINIT`/`CV_STORAGECODECURRENT` đọc ở form KHÔNG phải cột của bảng này:
    //    prefix `CV_` = enrich từ `Car_VIN` (luật C0-trecentesimussexagesimusquartus) ⇒ không tạo cột.
}

/// <summary>Master xe lái thử (Mst_CarDriverTest) — port 1:1 FrmMstCarDriverTestHTC/Dealer (DMSales.Foton/RetailContract). Xe dùng cho lái thử, biển số/VIN/model + hỗ trợ.</summary>
public sealed class CarDriverTest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DrvTestPlateNo { get; set; } = "";  // biển số (unique)
    public string DealerCode { get; set; } = "";
    public string? DrvTestVIN { get; set; }
    public string? DrvTestEngineNo { get; set; }
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";
    public string? CarDrvTestGPS { get; set; }
    public decimal Price { get; set; }
    public decimal AmountSupport1 { get; set; }
    public DateTime? DateSupport1 { get; set; }
    public decimal AmountSupport2 { get; set; }
    public DateTime? DateSupport2 { get; set; }
    public string? ClaimNoSupport { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Lượt khách thăm showroom (DLR_CtmVisit) — port 1:1 FrmCusVisit (DMSales.Foton/RetailContract). CRM: giới tính + độ tuổi + xe quan tâm.</summary>
public sealed class CtmVisit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CusVisitCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string Gender { get; set; } = "";       // 0/1
    public string RangeAge { get; set; } = "";      // độ tuổi
    public string ModelCode { get; set; } = "";     // xe quan tâm
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Lượt khách lái thử (DLR_DriveTest) — port 1:1 FrmNewTestDriver (DMSales.Foton/RetailContract). CRM: xe lái thử + khách hàng + GPLX + ngày lái thử.</summary>
public sealed class DriveTest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DriveTestCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string DriverTestType { get; set; } = ""; // HTC / dealer
    public string? DrvTestPlateNo { get; set; }      // xe lái thử
    public string TestModelCode { get; set; } = "";  // xe khách quan tâm
    public DateTime DriveDate { get; set; }
    public string? CustomerCode { get; set; }
    public string CustomerName { get; set; } = "";
    public string PhoneNo { get; set; } = "";
    public string Address { get; set; } = "";
    public string DriverLicenseNo { get; set; } = "";
    public string? RangeAge { get; set; }
    public string? Email { get; set; }
    /// <summary>
    /// Trạng thái lượt lái thử (`Dlr_DriveTest.DriverTestStatus`, `TConst.Stage`): "P" chờ → "A" duyệt / "R" từ chối.
    /// Mã đã khớp nguồn; GAP nằm ở chỗ nguồn (`DLR_DriveTestApprove_New20181119`) còn **ghi người/ngày duyệt**.
    /// </summary>
    public string DriverTestStatus { get; set; } = "P";
    /// <summary>Ngày/người duyệt (`ApprovedDate`/`ApprovedBy`) — nguồn ghi cho **CẢ hai nhánh** duyệt và từ chối.</summary>
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Hợp đồng bán lẻ (DlrContract) — port 1:1 FrmNewRetailContract/FrmMngRetailContractHTC (DMSales.Foton/RetailContract). HĐ đại lý bán lẻ cho khách, gắn NVBH + kiểu bán + dòng model/SL/giá/VAT.</summary>
public sealed class DlrContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>
    /// 🔴 #130 VersionDTimeCurr — **MỐC PHIÊN BẢN hiện hành** của hợp đồng
    /// (`Dlr_Contract.VersionDTimeCurr`, 2010.HTC `Biz.HTC.WH.cs:93118`).
    /// Mỗi lần tạo/sửa, nguồn đặt mốc này ở phần đầu **và** ghi cùng giá trị vào mọi dòng
    /// <see cref="DlrContractDtlHis"/> sinh ra ⇒ dùng để **nhóm các dòng thuộc CÙNG một phiên bản**.
    /// Port cũ thiếu hẳn cột này nên không dựng lại được lịch sử theo phiên bản.
    /// </summary>
    public DateTime? VersionDTimeCurr { get; set; }
    public string DlrContractNo { get; set; } = "";
    public string DlrContractNoUser { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string SalesManCode { get; set; } = "";
    public string SalesType { get; set; } = "";
    public string CustomerCode { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string IDCardNo { get; set; } = "";
    public string IDCardType { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public DateTime SignDate { get; set; }         // ngày ký HĐ
    public DateTime ContractDate { get; set; } = DateTime.Now;
    public string? BankCode { get; set; }
    /// <summary>
    /// 🔴 Trạng thái HĐ bán lẻ (`Dlr_Contract.DlrCtrStatus`) theo `TConst.DlrCtrStatus1`
    /// (`Const.Main.DMS40.cs:805-811`, chú thích nguồn ghi rõ *"Trạng thái hợp đồng bán lẻ"*):
    /// **"P" Mới tạo · "A" Xác nhận · "C" Hủy · "F" Hoàn thành**.
    /// ⚠️ Port cũ `Active → Cancelled` = **2 trạng thái tự đặt**, thiếu hẳn bước **xác nhận "A"**
    /// và trạng thái kết thúc **"F"**; cũng không phân biệt "mới tạo" với "đã xác nhận".
    /// ⚠️ `DlrCtrStatus1` là hằng RIÊNG của DMS40, không phải `TConst.Stage`.
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    /// <summary>Ngày/người XÁC NHẬN hợp đồng (`ApproveDTime`/`ApproveBy`) — nguồn `Dlr_Contract_ApproveMulti`.</summary>
    public DateTime? ApproveDTime { get; set; }
    public string? ApproveBy { get; set; }
    /// <summary>Ngày/người HUỶ (`CancelDTime`/`CancelBy`) — nguồn `Dlr_Contract_CancelMulti`.</summary>
    public DateTime? CancelDTime { get; set; }
    public string? CancelBy { get; set; }
    /// <summary>Ngày/người HOÀN THÀNH (`FinishDTime`/`FinishBy`) — đi kèm trạng thái "F".</summary>
    public DateTime? FinishDTime { get; set; }
    public string? FinishBy { get; set; }

    // ===== #158 parity Dlr_Contract (nguồn: DataWH/Biz.HTC.WH.cs, csproj 272 —
    //   `DealerSalesDealCreate_SellToDealer_New20230306`, khối dt_Dlr_Contract) =====
    /// <summary>Đại lý MUA (`DealerCodeBuyer`) — có khi hợp đồng sinh từ giao dịch bán buôn ĐL→ĐL.</summary>
    public string? DealerCodeBuyer { get; set; }
    public string? CreatedBy { get; set; }
    /// <summary>
    /// Bộ ba theo dõi PHIÊN BẢN hợp đồng: mốc cũ / số lần sửa / người sửa
    /// (`VersionDTimeOld`, `VersionCount`, `VersionUpdateBy`) — đi kèm <see cref="VersionDTimeCurr"/>
    /// đã có sẵn. Cùng motif "phiên bản định danh bằng mốc thời gian" ở #130/#146.
    /// </summary>
    public DateTime? VersionDTimeOld { get; set; }
    public int VersionCount { get; set; }
    public string? VersionUpdateBy { get; set; }
    public string FlagActive { get; set; } = "1";
    /// <summary>Cờ giao dịch đã HOÀN TẤT (`FlagDealFinish`) — tách khỏi <see cref="Status"/>.</summary>
    public string? FlagDealFinish { get; set; }
    /// <summary>Người giao dịch (`TransactorCode`) — khác `SMCode` (nhân viên bán).</summary>
    public string? TransactorCode { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}
/// <summary>
/// Dòng hợp đồng bán lẻ theo MODEL (`Dlr_ContractDtl` — 2010.HTC `Biz.HTC.WH.cs:93222`,
/// trong `DealerSalesDealCreate_SellToDealer_**New20230306**` (92880)).
/// 🔴 **#129 parity + TWIN lệch bit:** WS 32-bit gọi `…SellToDealer_New20181119` (92140), WS 64-bit gọi
/// `…_New20230306` — **bản 2023 ghi THÊM bảng `Dlr_ContractCar`** mà bản 2018 không có
/// (2018 ghi `Dlr_Contract` + `Dlr_ContractDtl`; 2023 ghi thêm `Dlr_ContractCar`).
/// ⇒ Canonical = bản 64-bit/2023; chọn bản 2018 sẽ **mất hẳn một bảng nghiệp vụ**.
/// 🔴 Dòng này là **GỘP NHÓM** theo (`SpecCode`, `ModelCode`, `ColorCode`) với `Qty` = tổng
/// (`dtDetail_groupBy`, cột `SumQty`) — KHÔNG phải một dòng một xe.
/// GAP #129: bổ sung `DlrContractNo` (khoá nghiệp vụ của nguồn — port cũ chỉ có `ContractId` nội bộ,
/// đúng mẫu C0-centesimusvigesimussextus), `ContractUpdateType`, `LogLUDateTime`, `LogLUBy`.
/// </summary>
public sealed class DlrContractDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ContractId { get; set; }
    /// <summary>Khoá nghiệp vụ của nguồn (`Dlr_ContractDtl.DlrContractNo`).</summary>
    public string? DlrContractNo { get; set; }
    /// <summary>Loại cập nhật hợp đồng — nguồn để NULL khi tạo (`DBNull.Value`, dòng 93236).</summary>
    public string? ContractUpdateType { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
    public string ModelCode { get; set; } = "";
    public string? SpecCode { get; set; }
    public string? ColorCode { get; set; }
    public int Qty { get; set; } = 1;
    public DateTime? DlvExpectedDate { get; set; }
    public decimal Price { get; set; }
    public decimal VAT { get; set; } = 10;
    public decimal AmountVAT { get; set; }
    public decimal TotalAmountAfterVAT { get; set; }
}

/// <summary>
/// Khách hàng của đại lý — port 1:1 FrmNewCustomer/FrmMngCustomer. Master KH cấp đại lý.
/// 🔴 **#124 đối chiếu bảng nguồn `DLS_DealerCustomer`** (2010.HTC `BizHTC.DealerSales.cs:2024`,
/// hàm `DealerSalesDealerCustomerCreate` (1872); sửa qua
/// `DealerSalesDealerCustomerUpdateAll_New20210109` (`Biz.HTC.WH.cs:108188`)).
/// 🔴 **BẪY TWIN LỆCH BIT ở riêng hàm `UpdateAll`**: WS 32-bit (`WSHTC.cs:29626`) gọi
/// `…UpdateAll_New20181119`, còn WS 64-bit (`WSHTC.64:41141`) gọi `…UpdateAll_**New20210109**`
/// — bản 2021 mới hơn **3 năm**, comment nguồn ghi rõ *"Nâng cấp ghi log"*. Các hàm khác của cụm
/// (Create/Update/Delete/Get) thì hai bit **giống nhau** ⇒ chỉ đúng MỘT hàm lệch, dễ bỏ sót.
/// Canonical đã chọn = **bản 64-bit / 2021**.
/// 📌 Lệch tên cột giữa port cũ và nguồn (giữ tên port cũ để không phá API):
/// `CustomerBaseCode`→`CusBaseCode`, `CreatedDate`→`CreatedAt`.
/// ⚠️ `CusTypeCode` **KHÔNG có trong bảng nguồn 2010.HTC** (grep toàn `TERP.BizHTC` = 0) — đây là cột
/// riêng của bản port Foton; giữ nguyên, nhưng đừng tìm nó ở nguồn 2010.HTC.
/// ⚠️ `BUCode` ở nguồn **chỉ dùng để kiểm quyền** (`myCommon_CheckAccessDealerData`), **không ghi**
/// vào bảng ⇒ cố ý không thêm cột này.
/// </summary>
public sealed class DealerCustomer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string CusTypeCode { get; set; } = "";     // loại khách hàng
    public string? CusBaseCode { get; set; }          // KH gốc (mặc định KH)
    public string FullName { get; set; } = "";
    public string? FullNameEN { get; set; }           // audit 2026-09-03: bổ sung — thiếu ở fire trước
    public string Address { get; set; } = "";
    public string PhoneNo { get; set; } = "";
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? ProvinceCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? IDCardNo { get; set; }
    public string? IDCardType { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? RepresentName { get; set; }        // audit 2026-09-03: bổ sung (người đại diện, KH doanh nghiệp)
    public string? Position { get; set; }              // audit 2026-09-03: bổ sung (chức vụ người đại diện)
    public string? CusAccountBank { get; set; }         // audit 2026-09-03: bổ sung (số TK ngân hàng KH)
    /// <summary>#124: nguồn có `CreatedBy` (BizHTC.DealerSales.cs:2021) — port cũ thiếu.</summary>
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// PHIẾU THANH TOÁN cho đại lý (`Pmt_Payment` — port 1:1 `PaymentPaymentCreate_New20191202`,
/// 2010.HTC `TERP.BizHTC/BankIntergration/BizHTC.MBBank.cs:32`).
/// 🔴 **BẪY TWIN + FILE CHẾT, cùng lúc:**
/// · WS 32-bit (`WSHTC.cs`) gọi `PaymentPaymentCreate_**New20181119**` — hàm này nằm ở
///   `DataWH/**Delete.**Biz.HTC.WH.My.cs:15765`, và file đó trong csproj là **`<None>`** (dòng 325)
///   ⇒ **FILE CHẾT, không nằm trong build**.
/// · WS 64-bit gọi `PaymentPaymentCreate_**New20191202**` ở `BizHTC.MBBank.cs` — csproj `<Compile>`
///   (dòng 310) ⇒ **LIVE**.
/// · Ngoài ra còn bản `_New20190611` ở `Biz.HTC.WH.cs:45880` **không WS nào gọi** — bản trung gian.
/// ⇒ Canonical = **bản 20191202 / MBBank.cs**. Nếu chọn theo "file DataWH quen thuộc" sẽ port **nhầm bản
/// trung gian** và **thiếu 2 cột** `PmtBakingStatus` + `BulkDetailId` (chỉ có ở bản 2019-12).
/// 🔴 `PaymentStatus` = `TConst.Stage.Pending` ("P") khi tạo; `ApprovedDate/By`, `PaymentEndDate`,
/// `ConfirmDate/By`, `AccountingRecordNo`, `BulkDetailId` đều để **NULL tường minh** lúc tạo.
/// 🔴 `PmtBakingStatus` = `TConst.Flag.No` (**"0"**) khi tạo — cờ đã đẩy sang ngân hàng hay chưa
/// (⚠️ tên cột nguồn viết **"Baking"**, thiếu chữ n so với "Banking" — giữ nguyên).
/// ⚠️ Nguồn ghi cả `_dbMain` lẫn `_dbWH` (460-463), dòng `_dbWH` **không bị comment**.
/// </summary>
public sealed class PmtPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? PaymentType { get; set; }
    public string? BankCodeSend { get; set; }
    public string? BankCodeReceive { get; set; }
    public string? BankPaymentNo { get; set; }
    public string? BankAccountSend { get; set; }
    public string? BankAccountReceive { get; set; }
    public string? AccountingRecordNo { get; set; }
    public decimal? TotalAmount { get; set; }
    /// <summary>"P" khi tạo (TConst.Stage).</summary>
    public string PaymentStatus { get; set; } = "P";
    public string? Funds { get; set; }
    public string? BankLending { get; set; }
    /// <summary>Cờ đã đẩy sang ngân hàng ("1"/"0"), khởi tạo "0". Tên cột nguồn thiếu chữ n: "Baking".</summary>
    public string? PmtBakingStatus { get; set; }
    /// <summary>Khoá dòng của lô gửi ngân hàng — NULL lúc tạo, điền khi đẩy đi.</summary>
    public string? BulkDetailId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? PaymentEndDate { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string? ConfirmBy { get; set; }

    // ===== #155 parity Pmt_Payment (nguồn: BankIntergration/BizHTC.MBBank.cs, csproj 310,
    //       md5 ec9f1442… khớp 2 máy) — `Pmt_Payment_Save_New20230306` (2818), gán cột tại 3428-3455.
    // 🔴 TWIN: `Pmt_Payment_Save` CHỈ có ở WS 64-bit, và WS gọi HAI bản
    //    (`_New20191202`, `_New20230306`); bản `_New20191202` **đã bị COMMENT** (MBBank.cs:575)
    //    ⇒ chỉ `_New20230306` là bản sống. Lại một ca "WS gọi vào hàm đã bị comment".
    public string? Remark { get; set; }
    /// <summary>Hình thức chuyển tiền (`TransferType`).</summary>
    public string? TransferType { get; set; }
    /// <summary>
    /// Kỳ hạn vay và lãi suất ở BẢNG ĐẦU (`LoanPeriod`/`InterestRate`).
    /// 🔴 Từ 20220325 nguồn **lấy theo bảng đầu, KHÔNG cho sửa ở dòng** (hai dòng gán ở
    /// <see cref="PmtPaymentDetail"/> đã bị comment) — xem luật `Funds` ở endpoint lưu.
    /// </summary>
    public decimal? LoanPeriod { get; set; }
    public decimal? InterestRate { get; set; }
}

/// <summary>
/// DÒNG phiếu thanh toán (`Pmt_PaymentDetail` — `BizHTC.MBBank.cs:460`).
/// Mỗi dòng gắn phiếu với **một xe** và (tuỳ nghiệp vụ) **một bảo lãnh** + **một hợp đồng đại lý**.
/// 🔴 Cột chỉ gồm **5**: `PaymentNo`, `CarId`, `GuaranteeNo`, `DlrCtrNo`, `Amount` — nguồn
/// **không** ghi VIN, không ghi trạng thái dòng. Khoá nối về đầu là `PaymentNo` (chuỗi), không phải Id.
/// </summary>
/// <summary>
/// Lịch sử lô chuyển tiền gửi ngân hàng (`Pmt_Payment_BulkDetailIHist`) —
/// nguồn `MBBank_MakeBulkPayment_v2_1` (BizHTC.MBBank.cs:4002) ghi tại 4574.
/// Mỗi lần đẩy một LÔ lệnh chi sang MB Bank sinh một dòng cho từng phiếu trong lô,
/// giữ lại `BulkDetailId` để đối soát ngược khi ngân hàng báo kết quả.
/// </summary>
public sealed class PmtPaymentBulkDetailIHist
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Thông tin lô (`BulkInfo`) — cùng khái niệm với `OsMBankLog.BulkInfo` (#149).</summary>
    public string? BulkInfo { get; set; }
    public string? BulkDetailId { get; set; }
    public string PaymentNo { get; set; } = "";
    public string? TransferType { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

public sealed class PmtPaymentDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentNo { get; set; } = "";
    public string? CarId { get; set; }
    /// <summary>Số bảo lãnh gắn với dòng thanh toán.</summary>
    public string? GuaranteeNo { get; set; }
    /// <summary>Số hợp đồng đại lý.</summary>
    public string? DlrCtrNo { get; set; }
    public decimal? Amount { get; set; }

    // ===== #155 parity Pmt_PaymentDetail (MBBank.cs:3484-3510) =====
    /// <summary>
    /// 🔴 Kỳ hạn vay / lãi suất của DÒNG — nguồn KHÔNG lấy từ đầu vào của dòng nữa:
    /// hai dòng `dr["LoanPeriod"] = …Input…` đã bị **comment** kèm ghi chú
    /// *"20220325. lấy theo MST ko cho sửa ở Dtl nữa"*.
    /// Giá trị được **rót xuống từ bảng đầu** và phụ thuộc cờ `Funds`:
    /// `Funds != "1"` ⇒ lấy `LoanPeriod`/`InterestRate` của phiếu; `Funds == "1"` ⇒ **để NULL**.
    /// </summary>
    public decimal? LoanPeriod { get; set; }
    public decimal? InterestRate { get; set; }
}

/// <summary>
/// KHẢO SÁT theo GIAO DỊCH bán lẻ (`DLS_DealSurvey` — port 1:1 `DealerSalesDealUpdate_Survey_New20190424`,
/// 2010.HTC `TERP.BizHTC/BizHTC.DealerSales.cs:5225`). Khoá là `DealNo`.
/// TWIN: cả WS 32-bit lẫn 64-bit **cùng bản** (đã diff TOÀN BỘ danh sách hàm của cụm theo luật
/// C0-centesimusvigesimusquartus — cụm này chỉ có 3 hàm và **khớp hoàn toàn**).
/// 🔴 **29 câu hỏi** `Survey1`..`Survey29` là **cột RỜI**, không phải bảng con — nguồn gán tuần tự
/// từng cột (dòng 5163-5222). Port giữ nguyên dạng cột rời để khớp schema khi import dữ liệu thật.
/// ⚠️ Nguồn ghi **CẢ HAI** DB: `_dbMain` + `_dbWH` (5225-5226) — khác hẳn <see cref="DlsVinSurvey"/>.
/// </summary>
public sealed class DlsDealSurvey
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string? Note { get; set; }
    public DateTime? ContactDate { get; set; }
    public string? Survey1 { get; set; }
    public string? Survey2 { get; set; }
    public string? Survey3 { get; set; }
    public string? Survey4 { get; set; }
    public string? Survey5 { get; set; }
    public string? Survey6 { get; set; }
    public string? Survey7 { get; set; }
    public string? Survey8 { get; set; }
    public string? Survey9 { get; set; }
    public string? Survey10 { get; set; }
    public string? Survey11 { get; set; }
    public string? Survey12 { get; set; }
    public string? Survey13 { get; set; }
    public string? Survey14 { get; set; }
    public string? Survey15 { get; set; }
    public string? Survey16 { get; set; }
    public string? Survey17 { get; set; }
    public string? Survey18 { get; set; }
    public string? Survey19 { get; set; }
    public string? Survey20 { get; set; }
    public string? Survey21 { get; set; }
    public string? Survey22 { get; set; }
    public string? Survey23 { get; set; }
    public string? Survey24 { get; set; }
    public string? Survey25 { get; set; }
    public string? Survey26 { get; set; }
    public string? Survey27 { get; set; }
    public string? Survey28 { get; set; }
    public string? Survey29 { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// KHẢO SÁT theo XE/VIN (`DLS_VINSurvey` — port 1:1 `DlsVINSurvey_Update_New20190424`,
/// `BizHTC.DealerSales.cs:7214`; hàm đọc `DLSVINSurveyGet_ICIC_New20181115`). Khoá là `VIN`.
/// Cùng bộ **29 câu** `Survey1`..`Survey29` như <see cref="DlsDealSurvey"/>, nhưng có thêm
/// `SurveyGmail` (email người khảo sát), `SurveyDateTime` (thời điểm khảo sát, nguồn tự đặt
/// `DateTime.Now`) và `SurveyPosition` (vị trí/chức danh người khảo sát).
/// 🔴 **KHÁC BIỆT ĐÁNG CHÚ Ý so với `DLS_DealSurvey`**: dòng ghi `_dbWH` ở đây **BỊ COMMENT**
/// (`//_dbWH.SaveData("DLS_VINSurvey", …)`, dòng 7215 và 7538) ⇒ bảng này **CHỈ ghi `_dbMain`**,
/// không đồng bộ sang DB Warehouse. Hai bảng khảo sát anh em nhưng **hành vi dual-write khác nhau** —
/// đừng suy từ bảng này sang bảng kia. (Ghi chú cho lượt trả nợ `_dbWH` sau này.)
/// </summary>
public sealed class DlsVinSurvey
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string? Note { get; set; }
    public DateTime? ContactDate { get; set; }
    /// <summary>Email người khảo sát (chỉ có ở bảng VIN, không có ở bảng Deal).</summary>
    public string? SurveyGmail { get; set; }
    public string? Survey1 { get; set; }
    public string? Survey2 { get; set; }
    public string? Survey3 { get; set; }
    public string? Survey4 { get; set; }
    public string? Survey5 { get; set; }
    public string? Survey6 { get; set; }
    public string? Survey7 { get; set; }
    public string? Survey8 { get; set; }
    public string? Survey9 { get; set; }
    public string? Survey10 { get; set; }
    public string? Survey11 { get; set; }
    public string? Survey12 { get; set; }
    public string? Survey13 { get; set; }
    public string? Survey14 { get; set; }
    public string? Survey15 { get; set; }
    public string? Survey16 { get; set; }
    public string? Survey17 { get; set; }
    public string? Survey18 { get; set; }
    public string? Survey19 { get; set; }
    public string? Survey20 { get; set; }
    public string? Survey21 { get; set; }
    public string? Survey22 { get; set; }
    public string? Survey23 { get; set; }
    public string? Survey24 { get; set; }
    public string? Survey25 { get; set; }
    public string? Survey26 { get; set; }
    public string? Survey27 { get; set; }
    public string? Survey28 { get; set; }
    public string? Survey29 { get; set; }
    /// <summary>Nguồn tự đặt `DateTime.Now` khi lưu, không nhận từ client.</summary>
    public DateTime? SurveyDateTime { get; set; }
    /// <summary>Vị trí/chức danh người khảo sát.</summary>
    public string? SurveyPosition { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>
/// NHẬT KÝ sửa khách hàng đại lý (`DLS_DealerCustomer_Upd` — 2010.HTC `Biz.HTC.WH.cs:108538`,
/// trong `DealerSalesDealerCustomerUpdateAll_New20210109`).
/// 🔴 **Khác hẳn họ bảng `*_His` đã port ở #91-#99**: các bảng kia lưu **cặp Old/New** từng cột,
/// còn bảng này lưu **SNAPSHOT TOÀN BỘ bản ghi SAU khi sửa** — nguồn lấy chính `DataTable` vừa lưu,
/// gọi `AcceptChanges()` rồi `SetAdded()` mọi dòng và `SaveData` sang bảng `_Upd`
/// ⇒ **schema giống hệt bảng chính**, không có cột Old/New/UpdBy nào.
/// ⇒ Muốn biết "đã đổi gì" thì phải **so hai snapshot liên tiếp**, không đọc được trực tiếp.
/// (Đây chính là phần "Nâng cấp ghi log" mà comment ở hàm 2021 nhắc tới.)
/// </summary>
public sealed class DealerCustomerUpdLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? CusBaseCode { get; set; }
    public string FullName { get; set; } = "";
    public string? FullNameEN { get; set; }
    public string? Address { get; set; }
    public string? PhoneNo { get; set; }
    public string? Email { get; set; }
    public string? TaxCode { get; set; }
    public string? ProvinceCode { get; set; }
    public string? DistrictCode { get; set; }
    public string? IDCardNo { get; set; }
    public string? IDCardType { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? RepresentName { get; set; }
    public string? Position { get; set; }
    public string? CusAccountBank { get; set; }
    /// <summary>Thời điểm chụp snapshot (MiniHTC thêm — nguồn không có, vì nó copy nguyên bản ghi).</summary>
    public DateTime LoggedAt { get; set; } = DateTime.Now;
    public string? LoggedBy { get; set; }
}

/// <summary>Yêu cầu PDI của đại lý (Dlr_PDIRequest) — port 1:1 FrmNewDlr_PDIRequest (DMSales.Foton/SalesDealer). Đại lý gửi yêu cầu PDI cho danh sách xe/RO.</summary>
public sealed class DlrPdiRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrPdiReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái yêu cầu (`Dlr_PDIRequest.DlrPDIReqStatus`) theo `TConst.Stage`:
    /// **"P" chờ duyệt → "A" đã duyệt** (dùng `Stage.Approved` = "A", **một cấp duyệt**).
    /// ⚠️ Port cũ `Draft → Done` (`/complete`) đặt tên như một bước "hoàn tất" tự phát, trong khi
    /// nguồn là **DUYỆT** (`DlrPDIRequestApprove`) có ghi người/ngày duyệt và **lan xuống mọi dòng**.
    /// Nguồn KHÔNG có từ chối/huỷ — ngoài duyệt chỉ còn **XOÁ** (`DlrPDIRequestDelete`, chỉ khi "P").
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
    /// <summary>Ngày/người duyệt (`ApprovedDate`/`ApprovedBy`).</summary>
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    /// <summary>Ghi chú của người duyệt (`Dlr_PDIRequest.Remark`).</summary>
    public string? Remark { get; set; }
}
public sealed class DlrPdiRequestDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DlrPdiReqId { get; set; }
    public string RONo { get; set; } = "";
    public DateTime? ROCreatedDate { get; set; }
    public string? ROStatus { get; set; }
    /// <summary>
    /// 🔴 Trạng thái của TỪNG DÒNG (`Dlr_PDIRequestDtl.DlrPDIReqDtlStatus`) — trục port cũ THIẾU.
    /// Tạo ở "P"; khi duyệt yêu cầu, nguồn lan xuống **'A'** cho mọi dòng bằng một câu update.
    /// ⚠️ Khác hẳn `ROStatus` (trạng thái lệnh sửa chữa được đồng bộ về) — hai trục độc lập.
    /// </summary>
    public string DlrPDIReqDtlStatus { get; set; } = "P";
}

/// <summary>Giá xe thực tế theo VIN (UpdateCarPrice) — port 1:1 FrmUpdateCar (DMSales.Foton). Cập nhật đơn giá thực tế cho từng xe (batch).</summary>
public sealed class CarActualPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CarId { get; set; } = "";   // VIN/CarID
    public decimal UnitPriceActual { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Xe đang thế chấp tại ngân hàng — port 1:1 FrmBankCarMortage + FrmDeliveryPlan (cụm Bank).</summary>
public sealed class BankCarMortage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string CarId { get; set; } = "";
    public string SOCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string BankCode { get; set; } = "";          // NH bao lanh/giam sat
    public string MortageBankCode { get; set; } = "";   // NH nhan the chap
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string GuaranteeType { get; set; } = "0";    // 0=BL NH giam sat, 1=BL NH phat hanh
    public string DeliveryRangeType { get; set; } = "DlvImmediate"; // DlvImmediate/DlvThisWeek/DlvNextWeek
    public DateTime? MortageStartDate { get; set; }
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Bảo lãnh ngân hàng (Pmt_Guarantee) — port 1:1 FrmBankGrt (cụm Bank). Header.</summary>
public sealed class BankGuarantee
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GuaranteeNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string BankCode { get; set; } = "";
    public string BankGuaranteeNo { get; set; } = "";   // so BL do NH cap
    /// <summary>
    /// Vai trò ngân hàng: "0" = NH GIÁM SÁT · "1" = NH PHÁT HÀNH.
    /// 🔴 Đây KHÔNG chỉ là nhãn phân loại — nguồn dùng nó để **RẼ NHÁNH TẦNG LỌC RBAC**
    /// (`TERP.BizBank/Report.cs:703-713`, hệ `ERP.V15.DMSSales.Real` chỉ có trên máy 150):
    /// vai trò "0" lọc theo <see cref="BankCodeMonitor"/>; vai trò "1" lọc theo mã đơn vị KD của ngân hàng.
    /// </summary>
    public string GuaranteeType { get; set; } = "0";     // 0=NH giam sat, 1=NH phat hanh

    /// <summary>
    /// 🔴 Mã NGÂN HÀNG GIÁM SÁT của bảo lãnh (`Pmt_Guarantee.BankCodeMonitor`) — port cũ THIẾU HẲN ở đây
    /// (chỉ có ở biên bản vận chuyển). Không có cột này thì **không thực hiện được nhánh RBAC vai trò "0"**.
    /// </summary>
    public string BankCodeMonitor { get; set; } = "";   // NH GIÁM SÁT của bảo lãnh

    /// <summary>Mã đơn vị kinh doanh của ngân hàng phát hành (`Mst_Bank.BankBUCode`) — căn cứ RBAC vai trò "1".</summary>
    public string? BankBUCode { get; set; }
    public int Term { get; set; }                         // ky han (thang)
    public DateTime? DateOpen { get; set; }
    public DateTime? DateExpired { get; set; }
    public DateTime? DateEnd { get; set; }
    public DateTime? DateRecieveGrtRoot { get; set; }     // ngày nhận LC/BL gốc — port FrmEditDateRecieveGrtRoot/FrmUpdateGrtDate
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 🔴 Trạng thái bảo lãnh theo ĐÚNG mã nguồn (`Pmt_Guarantee.GuaranteeStatus`, `TConst.Stage`):
    /// "P" chờ duyệt → "A" đã duyệt · "R" bị từ chối.
    /// ⚠️ Port cũ dùng chuỗi TỰ ĐẶT "Draft"/"Approved"/"Rejected"; chính hàm kiểm tra của nguồn
    /// (`myPayment_CheckBankGuaranteeNo`) lọc `GuaranteeStatus = "A"` ⇒ mã tự đặt không khớp.
    /// Đọc được dữ liệu cũ: Draft→"P", Approved→"A", Rejected→"R".
    /// </summary>
    public string Status { get; set; } = "P";
    public string FlagSettled { get; set; } = "0";        // 1 = da tat toan
    public string Remark { get; set; } = "";

    /// <summary>Lý do TỪ CHỐI (`RemarkReject`) — nguồn ghi riêng, không dùng chung `Remark`.</summary>
    public string? RemarkReject { get; set; }

    /// <summary>
    /// Kỳ hạn THỰC TẾ (`TermActual`) — nguồn ghi khi duyệt, TÁCH khỏi <see cref="Term"/> (kỳ hạn đăng ký).
    /// Khi duyệt, CẢ HAI phải &gt;= <c>WarningPeriod</c> (=3), mỗi cái một mã lỗi riêng.
    /// </summary>
    public int TermActual { get; set; }

    /// <summary>
    /// Kỳ CẢNH BÁO (`TermWarning`) — **giá trị DẪN XUẤT** của nguồn: `TermActual - WarningPeriod` (=3).
    /// Port cũ không có ⇒ mất mốc cảnh báo sắp hết hạn bảo lãnh.
    /// </summary>
    public int TermWarning { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    /// <summary>Người duyệt/từ chối (`ApprovedBy`) — nguồn ghi ở CẢ hai nhánh.</summary>
    public string? ApprovedBy { get; set; }
    public DateTime? SettledAt { get; set; }

    // ===== #186 parity `PaymentGuaranteeCreate_New20191217` (DataWH/BizHTC.zTemp.cs:14110, csproj 276) =====
    /// <summary>
    /// 🔴 Số ngày trả chậm (`NumberOfDaysDeferredPayment`) — nguồn **BẮT BUỘC** khi
    /// `GuaranteeType = LCUP` (LC Upas) và phải >= 0, thiếu thì ném
    /// `PaymentGuaranteeCreate_InvalidNumberOfDaysDeferredPayment`. Các loại khác không dùng.
    /// </summary>
    public int? NumberOfDaysDeferredPayment { get; set; }
}

/// <summary>Chi tiết bảo lãnh theo VIN (Pmt_GuaranteeDetail) — port 1:1 FrmBankGrt detail.</summary>
public sealed class BankGuaranteeDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long GuaranteeId { get; set; }
    public string VIN { get; set; } = "";
    public decimal GrtValue { get; set; }
    public decimal GrtPercent { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal DiscountPercent { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateWarning { get; set; }
    public DateTime? DateExpired { get; set; }
    public DateTime? DateEnd { get; set; }   // ngày kết thúc bảo lãnh (FrmEditGrtExpiredDate)
    public int DeferredPaymentDays { get; set; }  // số ngày trả chậm (FrmEditGrtSoNgayTCLC)
    public string? FlagDtlDiscount { get; set; }  // cờ chiết khấu dòng (FrmEditGrt)

    /// <summary>
    /// 🔴 Trạng thái RIÊNG của TỪNG DÒNG xe (`Pmt_GuaranteeDetail.GuaranteeDetailStatus`) — port cũ THIẾU HẲN.
    /// Nguồn tạo dòng ở "P" và khi duyệt/từ chối header thì **lan xuống TẤT CẢ dòng**
    /// (`Biz.HTC.WH.My.cs:10679`). Có cột này thì mới truy được từng xe đang ở trạng thái nào.
    /// </summary>
    public string GuaranteeDetailStatus { get; set; } = "P";

    // ===== #160 parity + side-effect `RD_ReqInvoiceDtlApprove_New20181119`
    //       (DataWH/Biz.HTC.WH.cs:128014, csproj 272; vùng md5 1e58bf10 khớp 2 máy) =====
    /// <summary>Ngày bắt đầu hiệu lực (`Pmt_GuaranteeDetail.DateStart`) — nguồn ĐẶT LẠI = hôm nay
    /// khi duyệt dòng đề nghị giao hồ sơ (không phải lúc tạo bảo lãnh).</summary>
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Lệnh xuất xe phía ngân hàng xác nhận (DO) — port 1:1 FrmBankDO. Header.</summary>
public sealed class BankDeliveryOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DONo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string SOCode { get; set; } = "";

    // 🔴 Port cũ KHÔNG có cột ngân hàng nào trên DO ⇒ **không lọc được DO theo ngân hàng**,
    // trong khi cổng `TERP.WSBank` cho ngân hàng đăng nhập xem chính bảng này
    // (`TERP.BizBank/Report.cs:282-294`, hệ `ERP.V15.DMSSales.Real` chỉ có trên máy 150).
    /// <summary>Ngân hàng PHÁT HÀNH bảo lãnh gắn với DO.</summary>
    public string BankCode { get; set; } = "";      // DO thuộc ngân hàng nào
    /// <summary>Ngân hàng GIÁM SÁT (`BankCodeMonitor`) — căn cứ RBAC vai trò "0".</summary>
    public string BankCodeMonitor { get; set; } = "";
    /// <summary>Mã đơn vị KD của ngân hàng phát hành (`BankBUCode`) — căn cứ RBAC vai trò "1".</summary>
    public string? BankBUCode { get; set; }
    /// <summary>Vai trò ngân hàng áp cho DO: "0" giám sát · "1" phát hành — QUYẾT ĐỊNH nhánh lọc quyền.</summary>
    public string GuaranteeType { get; set; } = "0";

    public string Status { get; set; } = "Open";   // Open -> Confirmed (khi tat ca xe da nhan)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
}

/// <summary>Chi tiết xe trên lệnh xuất, NH xác nhận nhận (DoDetail.Confirm_Status) — port 1:1 FrmBankDO detail.</summary>
public sealed class BankDoCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DeliveryOrderId { get; set; }
    public string VIN { get; set; } = "";
    public string CarId { get; set; } = "";
    public string BankGrtNo { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public DateTime? DeliveryExpectedDate { get; set; }
    public DateTime? DeliveryOutDate { get; set; }
    public string ConfirmStatus { get; set; } = "0";   // 0=chua nhan, 1=da nhan
    public string ConfirmRemark { get; set; } = "";
    public DateTime? ConfirmedAt { get; set; }
}

/// <summary>Biên bản vận chuyển xe (TransportMinutes) — port 1:1 FrmBankTransportMinutes. Dual-sign ĐL + HTC.</summary>
public sealed class BankTransportMinute
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Mã đơn vị KD của ngân hàng phát hành (`BankBUCode`) — căn cứ RBAC vai trò "1"
    /// (`TERP.BizBank/Report.cs:690-713`). Port cũ đã có `BankCode`/`BankCodeMonitor` nhưng thiếu cột này.</summary>
    public string? BankBUCode { get; set; }
    /// <summary>Vai trò ngân hàng: "0" giám sát · "1" phát hành — QUYẾT ĐỊNH nhánh lọc quyền.</summary>
    public string GuaranteeType { get; set; } = "0";
    public string TransportMinutesNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string BankCode { get; set; } = "";
    public string BankCodeMonitor { get; set; } = "";   // NH giam sat
    public string Status { get; set; } = "Draft";        // Draft -> Approved (Da ky) / Cancel (Da huy)
    public DateTime? DLApprDateTime { get; set; }         // DL ky
    public DateTime? HTCAppr2DateTime { get; set; }       // HTC ky
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Chi tiết xe trên biên bản vận chuyển — port 1:1 FrmBankTransportMinutes detail.</summary>
public sealed class BankTmCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TransportMinuteId { get; set; }
    public string VIN { get; set; } = "";
    public string CarId { get; set; } = "";
    public string EngineNo { get; set; } = "";
    public string SOCode { get; set; } = "";
    public string GuaranteeNo { get; set; } = "";
    public string DlrCtrNo { get; set; } = "";
    public string ColorCode { get; set; } = "";
}

/// <summary>
/// ⚠️ #203 ENTITY CHẾT — không route nào còn dùng. Cụm `/api/bankpms` đã chuyển sang <see cref="PmtPayment"/>
/// và <see cref="PmtPaymentDetail"/>. Giữ class để dữ liệu cũ trong DB không mất; sẽ xoá khi đã di trú.
/// ⚠️ #202 TRÙNG LẶP — thực thể này và <see cref="PmtPayment"/> là **CÙNG MỘT BẢNG NGUỒN** `Pmt_Payment`.
/// · Tên bảng ghi trong tài liệu cũ (`Pmt_PM` / `Pmt_PMDetail`) **KHÔNG TỒN TẠI** ở nguồn — đã grep toàn
///   `TERP.BizHTC`: họ `Pmt_*` chỉ có Payment / PaymentDetail / PaymentAVN / PaymentGPS / PaymentPDI /
///   PaymentStorage / Guarantee / GrtClaim… , không có `Pmt_PM`.
/// · Màn gốc `TERP.BankClient/Views/Bank/FrmMngPM.cs` (672 dòng) có đúng ba nút Search·Export·Close
///   ⇒ **màn CHỈ ĐỌC** của cổng ngân hàng trên bảng `Pmt_Payment`; cổng `TERP.WSBank` chỉ có 2 lệnh ghi,
///   đều thuộc cụm `DMS40_DlrCtr_CancelBankMD_*`. Các lệnh GHI phiếu thanh toán nằm ở cổng WSHTC
///   (`PaymentPaymentCreate/Approve/Reject/Cancel/Confirm`) — <see cref="PmtPayment"/> mới là bản port đúng.
/// 🔴 NỢ HỢP NHẤT (chưa làm ở lượt này vì cần đối chiếu nguồn từng cột): <see cref="PmtPaymentDetail"/>
///   hiện chỉ có 4 cột dữ liệu (CarId/GuaranteeNo/DlrCtrNo/Amount) trong khi <see cref="BankPaymentCar"/>
///   có thêm 11 cột (VIN, ModelCode, SpecCode, SOCode, ColorCode, AmountAccum, PercentAccum,
///   UnitPriceActual, AmountCurrent, PercentCurrent, BankGuaranteeNo). Phải soi `Pmt_PaymentDetail` ở nguồn
///   rồi mới gộp — KHÔNG gộp mù.
/// </summary>
public sealed class BankPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PaymentNo { get; set; } = "";
    public string BankPaymentNo { get; set; } = "";      // so phieu ben NH
    public string DealerCode { get; set; } = "";
    public string BankCodeSend { get; set; } = "";
    public string BankCodeReceive { get; set; } = "";
    public string BankAccountSend { get; set; } = "";
    public string BankAccountReceive { get; set; } = "";
    public string Funds { get; set; } = "";               // nguon tien
    public string BankLending { get; set; } = "";          // NH cho vay
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Draft";   // Draft -> Approved / Rejected
    public string AccountingRecordNo { get; set; } = "";   // so ghi so ke toan (gan khi duyet)
    public string Remark { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PaymentEndDate { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public decimal? InterestRate { get; set; }  // lãi suất vay (%) — port FrmUpdate_Pmt_Payment
    public int? LoanPeriod { get; set; }        // kỳ hạn vay (tháng)
}

/// <summary>Chi tiết phiếu thanh toán theo VIN — bảng nguồn THẬT là `Pmt_PaymentDetail`
/// (tên `Pmt_PMDetail` trong tài liệu cũ không tồn tại). Xem ghi chú trùng lặp ở <see cref="BankPayment"/>.</summary>
public sealed class BankPaymentCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PaymentId { get; set; }
    public string VIN { get; set; } = "";
    public string CarId { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string SOCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public decimal AmountAccum { get; set; }
    public decimal PercentAccum { get; set; }
    public decimal UnitPriceActual { get; set; }
    public decimal AmountCurrent { get; set; }
    public decimal PercentCurrent { get; set; }
    public string GuaranteeNo { get; set; } = "";
    public string BankGuaranteeNo { get; set; } = "";
    public string DlrCtrNo { get; set; } = "";
}

/// <summary>Hóa đơn VAT HTC (VAT_HTCInvoice) — port 1:1 FrmMngInvoice (cụm Bank). Header.</summary>
public sealed class VatInvoice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string HTCInvoiceCode { get; set; } = "";
    public string HTCInvoiceNo { get; set; } = "";       // so HD (gan khi phat hanh)
    public string InvoiceIDCode { get; set; } = "";       // ky hieu HD

    /// <summary>
    /// 🔴 #274 `VAT_HTCInvoice.InvoiceIDType` — LOẠI ký hiệu hoá đơn. Nguồn
    /// (`BizHTC.HDDTIntergration.cs:9038 VAT_HTCInvoiceImportNew_New20190816`) chặn thẳng:
    /// `if (strInvoiceIDType != TConst.InvoiceType.HTC) throw ..._InvalidInvoiceIDType`
    /// ⇒ đường nhập chỉ nhận hoá đơn loại **HTC**. Cột này còn là ĐIỀU KIỆN LỌC khi tìm hoá đơn liền kề
    /// để kiểm ngày (xem guard cận dưới/cận trên) — thiếu nó thì lọc sai tập so sánh.
    /// </summary>
    public string InvoiceIDType { get; set; } = "HTC";
    public decimal VAT { get; set; } = 10;
    public string DealerCode { get; set; } = "";
    public string BankCode { get; set; } = "";
    public string SourceInvoiceName { get; set; } = "";   // nguon HD

    // ===== #195 parity `VAT_HTCInvoiceCreate_Special_New20190816` (BizHTC.HDDTIntergration.cs:8329) =====
    /// <summary>
    /// Loại nguồn hoá đơn (`TConst.SourceInvoiceCode`): `INVOICEROOT` · `INVOICEADJ` · `INVOICEREPLACE`.
    /// Chỉ `INVOICEREPLACE` mới kích hoạt luật huỷ hoá đơn GỐC (xem `POST /api/vatinvoices`).
    /// </summary>
    public string? SourceInvoiceCode { get; set; }
    /// <summary>Mã hoá đơn GỐC bị thay thế/điều chỉnh (`RefNo`) — trỏ tới `HTCInvoiceCode` khác.</summary>
    public string? RefNo { get; set; }
    // #195b: nguồn ghi `LogLUDateTime`/`LogLUBy` trên CHÍNH bảng `VAT_HTCInvoice` (khối huỷ hoá đơn gốc).
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
    /// <summary>
    /// 🔴 MÃ TRA CỨU hoá đơn điện tử (`OS_HDDT_InvoiceCode` — nguồn chú thích thẳng: *"Số tra cứu hóa đơn"*).
    /// ⚠️ Mã này **do hệ HDDT/TVAN CẤP** qua `OS_MstSvTVAN_MstSv_Seq_Common_Get`
    /// (`BizHTC.InvoiceHTC_TCG.cs:539-556`) — **KHÔNG được tự sinh**.
    /// Port cũ tự bịa `"HDDT" + số hoá đơn` lúc phát hành ⇒ mã đó **tra cứu không ra** ở cổng hoá đơn điện tử.
    /// Còn rỗng = **chưa đẩy sang HDDT** (nguồn KHÔNG có cột trạng thái đẩy riêng — đây là trạng thái ngầm theo dữ liệu).
    /// </summary>
    public string OS_HDDT_InvoiceCode { get; set; } = "";

    /// <summary>Số tham chiếu gửi kèm sang hệ HDDT (`OS_HDDT_RefNo`) — port cũ thiếu hẳn.</summary>
    public string? OS_HDDT_RefNo { get; set; }

    /// <summary>Thời điểm nhận được mã tra cứu từ hệ HDDT.</summary>
    public DateTime? HddtSyncedAt { get; set; }

    /// <summary>Hình thức thanh toán gửi lên hoá đơn (`TConst.PaymentMethodCode`):
    /// CK chuyển khoản (nguồn dùng mặc định) · TM tiền mặt · TMCK · DTCN · TTD.</summary>
    public string PaymentMethodCode { get; set; } = "CK";

    // 🔴 SỐ LIỆU GỬI CƠ QUAN THUẾ — nguồn TÁCH doanh thu theo TỪNG THUẾ SUẤT
    // (BizHTC.InvoiceHTC_TCG.cs:302-322). Port cũ chỉ có 1 cột `VAT` ⇒ không dựng được
    // bảng kê thuế và không đối chiếu được với hoá đơn điện tử.
    /// <summary>Hàng KHÔNG chịu thuế (`ValGoodsNotTaxable`).</summary>
    public decimal ValGoodsNotTaxable { get; set; }
    /// <summary>Hàng KHÔNG phải kê khai tính thuế (`ValGoodsNotChargeTax`).</summary>
    public decimal ValGoodsNotChargeTax { get; set; }
    /// <summary>Tiền hàng chịu thuế suất 5% (`ValGoodsVAT5`).</summary>
    public decimal ValGoodsVAT5 { get; set; }
    /// <summary>Thuế của phần 5% (`ValVAT5`).</summary>
    public decimal ValVAT5 { get; set; }
    /// <summary>Tiền hàng chịu thuế suất 10% (`ValGoodsVAT10`).</summary>
    public decimal ValGoodsVAT10 { get; set; }
    /// <summary>Thuế của phần 10% (`ValVAT10`).</summary>
    public decimal ValVAT10 { get; set; }
    /// <summary>Tổng tiền hàng (`TotalValInvoice`).</summary>
    public decimal TotalValInvoice { get; set; }
    /// <summary>Tổng tiền thuế (`TotalValVAT`).</summary>
    public decimal TotalValVAT { get; set; }
    /// <summary>Tổng tiền thanh toán (`TotalValPmt`).</summary>
    public decimal TotalValPmt { get; set; }
    public string CurrencyCode { get; set; } = "VND";
    public decimal CurrencyRate { get; set; } = 1;
    public string InvoiceAdjType { get; set; } = "";       // loai dieu chinh (rong=goc)
    public string RootHTCInvoiceNo { get; set; } = "";     // HD goc (khi la HD dieu chinh)
    /// <summary>
    /// 🔴 Trạng thái hoá đơn theo ĐÚNG mã nguồn (`TConst.Stage`, cột `VAT_HTCInvoice.VatHTCStatus`):
    /// "P" chờ duyệt (tạo mới) → "F" đã duyệt/phát hành → "C" đã huỷ · "R" bị từ chối.
    /// ⚠️ Port cũ dùng chuỗi TỰ ĐẶT "Draft"/"Issued"/"Deleted" ⇒ không đối chiếu được với dữ liệu hệ nguồn.
    /// ⚠️ Nguồn tách 2 việc: **DUYỆT** (P→F, `Biz.HTC.WH.cs:122079`) và **GÁN SỐ HOÁ ĐƠN** (hàm riêng,
    /// `Biz.HTC.WH.cs:123182`); port cũ gộp cả hai vào một bước "issue".
    /// Đọc được dữ liệu cũ: Draft→"P", Issued→"F", Deleted→"C".
    /// </summary>
    public string VatHTCStatus { get; set; } = "P";
    public string DeleteReason { get; set; } = "";

    /// <summary>Thời điểm duyệt/huỷ (`ApprovedDate`) — nguồn ghi ở CẢ hai nhánh duyệt và huỷ.</summary>
    public DateTime? ApprovedDate { get; set; }
    /// <summary>Người duyệt/huỷ (`ApprovedBy`).</summary>
    public string? ApprovedBy { get; set; }  // VAT_HTCInvoice
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? HTCInvoiceDate { get; set; }
}

/// <summary>Chi tiết hóa đơn VAT theo VIN (VAT_HTCInvoiceDetail) — port 1:1 FrmMngInvoice detail.</summary>
public sealed class VatInvoiceCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long VatInvoiceId { get; set; }
    public string VIN { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string EngineNo { get; set; } = "";
    public string BrandName { get; set; } = "";
    public string CarType { get; set; } = "";
    public string InvoiceNoFactory { get; set; } = "";
    public string ProductionYear { get; set; } = "";
    public decimal HTCUnitPrice { get; set; }
    public DateTime? CustomsClearanceDate { get; set; }

    // ===== #194 parity `VAT_HTCInvoiceApproveX` (HDDTIntergration/BizHTC.HDDTIntergration.cs:5562, bản máy 150) =====
    // 🔴 Duyệt/huỷ duyệt hoá đơn KHÔNG chỉ đổi header: nguồn update MỌI dòng chi tiết
    //    (`F` khi duyệt kèm `ApprovedDate`/`ApprovedBy`, `C` khi huỷ duyệt — nhánh huỷ KHÔNG ghi mốc duyệt).
    public string HTCStatusDetail { get; set; } = "P";
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Công văn gia hạn bảo lãnh (Pmt_GrtClaimExt) — port 1:1 FrmMngGrtClaimPM. Header + ký.</summary>
public sealed class GrtClaimExt
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GrtClaimExtNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public int NumberOfGuaranteeExt { get; set; }        // so lan gia han
    public int TotalCarNoStart { get; set; }              // tong xe chua bat dau
    public string SignStatus { get; set; } = "P";         // P=chua ky, S=da ky
    public string FileName { get; set; } = "";            // file da ky (guard idempotent)
    public DateTime? SignDateTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #191 parity `Pmt_GrtClaimExt` (Biz.HTC.PaymentGrtExt.cs) — cụm CHỈ có ở WS 64-bit =====
    // ⚠️ `SignStatus` ở đây chính là cột `GrtClaimExtStatus` của nguồn; từ vựng ĐÚNG là P/A/C
    //    (`TConst.GrtClaimExtStatus`): P=Pending, A=Approved(đã ký), C=Cancel. Giá trị "S" của port cũ
    //    KHÔNG có trong nguồn — đã migrate S → A ở Seeder.
    public string? SignBy { get; set; }
    public string? CreatedBy { get; set; }
    /// <summary>Nguồn ghi ĐÈ `Remark` ở cả `_SaveX`, `_SignX` và `_CancelX`.</summary>
    public string? Remark { get; set; }
    public DateTime? CancelDateTime { get; set; }
    public string? CancelBy { get; set; }
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết công văn gia hạn theo VIN (Pmt_GrtClaimExtDtl) — port 1:1 FrmMngGrtClaimPM detail.</summary>
public sealed class GrtClaimExtCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long GrtClaimExtId { get; set; }
    public string CarId { get; set; } = "";
    public string VIN { get; set; } = "";
    public string GuaranteeNo { get; set; } = "";
    /// <summary>Cột `GrtClaimExtStatusDtl` của nguồn — đi theo header (P/A/C), do `_SignX`/`_CancelX` ghi.</summary>
    public string SignStatusDtl { get; set; } = "P";
    public DateTime? LogLUDateTime { get; set; }   // #191 parity: nguồn cập nhật LogLU* trên CẢ dòng chi tiết
    public string? LogLUBy { get; set; }
}

/// <summary>Bản ghi hỗ trợ sửa dữ liệu (Deal/HĐ theo VIN) — port 1:1 cụm Support (FrmSupportUpdatePrice/CarDeliveryDate/SMCode/BankCode).</summary>
public sealed class SupportRecord
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public decimal Price { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string SalesManCode { get; set; } = "";
    public string BankCode { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Log patch từng field (audit old→new) — port 1:1 cụm Support (bulk field-fix).</summary>
public sealed class SupportPatchLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SupportRecordId { get; set; }
    public string VIN { get; set; } = "";
    public string Field { get; set; } = "";      // price/deliveryDate/salesManCode/bankCode
    public string OldValue { get; set; } = "";
    public string NewValue { get; set; } = "";
    public DateTime PatchedAt { get; set; } = DateTime.Now;
}

/// <summary>Đề nghị thế chấp xe (RM_ReqMortgage) — port 1:1 FrmNewRM_ReqMortgage + FrmMngRM_ReqMortgage. Header.</summary>
public sealed class ReqMortgage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqRMNo { get; set; } = "";
    public string MortageBankCode { get; set; } = "";   // NH nhan the chap
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// 🔴 Trạng thái đề nghị thế chấp theo mã nguồn (`RM_ReqMortgage.RMStatus`, `TConst.Stage`):
    /// "P" chờ duyệt → "A" đang thế chấp → "F" đã giải chấp · "C" huỷ.
    /// Đọc dữ liệu cũ: Draft→"P", Approved→"A", Finished→"F", Cancelled→"C".
    /// </summary>
    public string Status { get; set; } = "P";
    public DateTime? MortageDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    /// <summary>Thời điểm giải chấp xong toàn bộ lô (`FinishDate`).</summary>
    public DateTime? FinishedAt { get; set; }

    // --- #140 parity RM_ReqMortgage: 8 cột nguồn ghi mà port cũ thiếu ---
    public string? CreatedBy { get; set; }
    /// <summary>Mốc sửa gần nhất (`LUDateTime`/`LUBy`) — tách khỏi `LogLU*` (dấu vết kỹ thuật).</summary>
    public DateTime? LUDateTime { get; set; }
    public string? LUBy { get; set; }
    public string? ApprovedBy { get; set; }
    /// <summary>Người xác nhận giải chấp xong (`FinishBy`).</summary>
    public string? FinishBy { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Chi tiết xe đề nghị thế chấp (RM_ReqMortgageDtl) — port 1:1 FrmNewRM_ReqMortgage detail.</summary>
public sealed class ReqMortgageCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqMortgageId { get; set; }
    public string VIN { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string EngineNo { get; set; } = "";
    public string CQNo { get; set; } = "";
    public string CONo { get; set; } = "";
    public string DeclarationNo { get; set; } = "";
    public DateTime? CODate { get; set; }

    /// <summary>
    /// 🔴 Trạng thái RIÊNG của DÒNG xe (`RM_ReqMortgageDtl.RMDtlStatus`, `TConst.Stage`):
    /// "P" chờ duyệt → "A" **đang thế chấp** → "F" **đã giải chấp**.
    /// Nguồn thao tác theo TỪNG VIN (`BizHTC.GiaiChap.cs:987 / 1384 / 3040`).
    /// </summary>
    public string RMDtlStatus { get; set; } = "P";

    /// <summary>Ngân hàng đang giữ thế chấp xe này (`MortageBankCode`) — ghi khi DUYỆT; khi GIẢI CHẤP đổi thành "HTC.HO".</summary>
    public string? MortageBankCode { get; set; }

    /// <summary>Ngày BẮT ĐẦU thế chấp (`MortageStartDate`) — nguồn ghi = ngày duyệt.</summary>
    public DateTime? MortageStartDate { get; set; }

    /// <summary>
    /// 🔴 Ngày GIẢI CHẤP (`RedeemDate`) — nguồn ghi khi duyệt **đề nghị giải chấp**, cùng lúc đóng dòng về "F".
    /// ⚠️ Đây là **mắt nối giữa 2 nghiệp vụ**: duyệt giải chấp (RD_ReqRedeem) tác động ngược lên dòng thế chấp.
    /// </summary>
    public DateTime? RedeemDate { get; set; }

    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }

    // --- #140 parity RM_ReqMortgageDtl: 8 cột nguồn ghi mà port cũ thiếu ---
    public string? CarId { get; set; }
    /// <summary>Đại lý của XE — nguồn để `DealerCode` ở DÒNG, bảng đầu KHÔNG có cột này.</summary>
    public string? DealerCode { get; set; }
    public DateTime? FinishDate { get; set; }
    public string? FinishBy { get; set; }
    /// <summary>
    /// 🔴 MẮT NỐI NGƯỢC sang nghiệp vụ giải chấp (`ReqDMNo`): khi TẠO đề nghị thế chấp nguồn gán
    /// `DBNull` (BizHTC.GiaiChap.cs:988), chỉ khi có đề nghị GIẢI CHẤP duyệt lên xe này mới điền số.
    /// Cặp với `RD_ReqRedeemDtl.ReqRMNo` ⇒ hai bảng chi tiết trỏ vào nhau HAI CHIỀU.
    /// </summary>
    public string? ReqDMNo { get; set; }
    public string? Remark { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Yêu cầu chứng từ QC/xuất xưởng (QC_DocReq) — port 1:1 FrmMngQCDocReq (Sales/HTMV). Header.</summary>
public sealed class QcDocReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DocReqNo { get; set; } = "";
    public string CreateBy { get; set; } = "";
    public string DocReqStatus { get; set; } = "Pending";   // Pending -> Approved / Cancel
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Chi tiết chứng từ QC theo VIN (QC_DocReqDtl) — port 1:1 FrmMngQCDocReq detail.</summary>
public sealed class QcDocReqCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long QcDocReqId { get; set; }
    public string OrderNo { get; set; } = "";       // Mv_OrderNo (so DH san xuat)
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public string VIN { get; set; } = "";           // Mv_VinReal
    public string EngineNo { get; set; } = "";
    public string OriginNo { get; set; } = "";       // so xuat xu
    public string FGFormNo { get; set; } = "";       // so phieu xuat xuong
    public string QCNo { get; set; } = "";           // so phieu QC
    public string ClearanceFormNo { get; set; } = ""; // so phieu thong quan
    public string DocDeliverTypeCode { get; set; } = "";
    public string DtlStatus { get; set; } = "Pending";
}

/// <summary>Đơn hàng nâng cấp (Upgrade Order) — port 1:1 FrmUpgradeOrder + FrmUpgradeMngOrderHtc. Header.</summary>
public sealed class UpgradeOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string OrderMonth { get; set; } = "";      // thang DH (yyyy-MM)
    public string OrderType { get; set; } = "";        // loai DH
    public string OrderPolicy { get; set; } = "";      // chinh sach DH
    public int TotalQty { get; set; }
    public string Status { get; set; } = "Draft";      // Draft -> Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Dòng đơn hàng nâng cấp (Upgrade Order Detail) — port 1:1 FrmUpgradeOrder detail.</summary>
public sealed class UpgradeOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long UpgradeOrderId { get; set; }
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public int Quantity { get; set; }
    public string PromotionModel { get; set; } = "";
    public decimal DiscountAmount { get; set; }
}

/// <summary>Tính chi phí tài chính / chiết khấu TT (DMS40_FnExp_Calc_FnExp_PmDc) — port 1:1 FrmDMS40_2019_FnExp_Calc. Header.</summary>
public sealed class FnExpCalc
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CaNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public decimal FnExpPercent { get; set; }         // lai suat CPTC (%/nam)
    public decimal TotalFnExp { get; set; }            // tong chi phi tai chinh (tinh)
    public string Status { get; set; } = "Draft";      // Draft -> Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Dòng tính chi phí tài chính theo xe (DMS40_FnExp_Calc_FnExp_PmDcDtl) — port 1:1 FrmDMS40_2019_FnExp_Calc detail.</summary>
public sealed class FnExpCalcLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long FnExpCalcId { get; set; }
    public string CarId { get; set; } = "";
    public string SOCode { get; set; } = "";
    public decimal FnDepositAmount { get; set; }       // tien coc
    public int FnDepositCountDate { get; set; }         // so ngay tinh cho coc
    public decimal FnGrtAmount { get; set; }            // tien bao lanh
    public int FnGrtCountDate { get; set; }             // so ngay tinh cho BL
    public decimal FnTotalAmount { get; set; }          // chi phi TC dong (tinh)
    public decimal PDAmount { get; set; }               // chiet khau TT
    public int TermActual { get; set; }
}

/// <summary>Lịch sản xuất / ETA xe nhập (WO_Schedule) — port 1:1 FrmImportETAMng + FrmImportETAInDetail. Header.</summary>
public sealed class WoSchedule
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ScheduleNo { get; set; } = "";
    public string CreatedBy { get; set; } = "";
    public string Status { get; set; } = "Open";      // Open -> Closed (khi het SL con lai)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng lịch SX theo model/spec/màu (WO_ScheduleDetail) — port 1:1 FrmImportETAMng detail.</summary>
public sealed class WoScheduleLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long WoScheduleId { get; set; }
    public string WorkOrderNo { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string SpecCode { get; set; } = "";
    public string ColorCode { get; set; } = "";
    public int QtyOrder { get; set; }        // SL dat hang
    public int QtyProduct { get; set; }       // SL da san xuat
    public int QtyRemain { get; set; }        // SL con lai (= QtyOrder - QtyProduct)
}

/// <summary>Giao dịch bán buôn xe ĐL→ĐL (Deal To Dealer) — port 1:1 FrmNewDealToDealer. Header.</summary>
/// <summary>
/// Bản ghi BÁO CÁO GỬI HÃNG HMC (`HMC_Report`) — nguồn `myDealerSales_GenerateHMCReport`
/// (BizHTC.DealerSales.cs:24, csproj 110, md5 c5cf9085… khớp 2 máy).
/// Mỗi lần bán/giao một chiếc xe sinh MỘT dòng; hãng đọc cột <see cref="PerformContents"/>.
///
/// 🔴 `PerformContents` là **chuỗi định dạng CỐ ĐỊNH DÀI ĐÚNG 54 KÝ TỰ** — hợp đồng dữ liệu với hãng:
/// `DistributorCode(5) + DealerCode(10, căn TRÁI) + PerformDate(8, yyyyMMdd) + VIN(17)
///  + DeliveryType(4) + SalesType(2, căn TRÁI) + CreatedDate(8, yyyyMMdd)` = **54**.
/// Nguồn **NÉM LỖI** nếu độ dài ≠ 54 (`HMCRpt_TransactionLength`) ⇒ đây là guard, không phải kiểm hình thức.
/// `DistributorCode` fix cứng **"A26AD"** (`TConst.HTCConst.HMCRpt_DistributorCode`).
/// `DeliveryType` (`TConst.HTCConst`): **"010A"** giao cho đại lý · **"001A"** giao người dùng cuối ·
/// **"100C"** đại lý trả · **"010C"** người dùng trả.
/// </summary>
public sealed class HmcReport
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DealerCode { get; set; }
    public string? DealNo { get; set; }
    public string? CarId { get; set; }
    public string VIN { get; set; } = "";
    /// <summary>Loại giao dịch gửi hãng (`DeliveryType`) — xem bảng mã ở ghi chú lớp.</summary>
    public string? DeliveryType { get; set; }
    public string? SalesType { get; set; }
    /// <summary>Ngày phát sinh nghiệp vụ (`PerformDate`) — đưa vào chuỗi 54 ký tự dạng yyyyMMdd.</summary>
    public DateTime? PerformDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    /// <summary>🔴 Chuỗi 54 ký tự gửi hãng — xem ghi chú lớp.</summary>
    public string? PerformContents { get; set; }
    /// <summary>
    /// Khoá tự tăng do DB cấp (`AutoID`). ⚠️ Nguồn ghi vào DB Main trước, đọc `select @@Identity`
    /// rồi **gán lại** trước khi ghi sang DB Warehouse — để hai DB CÙNG một AutoID.
    /// </summary>
    public long? AutoID { get; set; }
}

public sealed class WholesaleDeal
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string DealNoUser { get; set; } = "";       // so GD nguoi dung (bat buoc)
    public string BuyerDealerCode { get; set; } = "";   // dai ly mua
    public string SalesManCode { get; set; } = "";
    public string Status { get; set; } = "Draft";       // Draft -> Confirmed / Cancelled
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }

    // ===== #157 parity DLS_Deal (nguồn: DataWH/Biz.HTC.WH.cs, csproj 272) —
    //   `DealerSalesDealCreate_SellToDealer_New20230306` (92880), gán cột tại 93333-93350.
    // 🔴 TWIN: WS 32-bit chỉ có `_New20181119`; WS 64-bit có THÊM `_New20230306` ⇒ bản mới là canonical.
    //   (Các bản trong `Delete.BizHTC.*` — `_New20180928`, `_New20181017`, `_New20181115` — là XÁC.)
    /// <summary>Đại lý BÁN (`DealerCode`) — port cũ chỉ có bên MUA.</summary>
    public string? DealerCode { get; set; }
    public string? SalesType { get; set; }
    public DateTime? DealDate { get; set; }
    /// <summary>
    /// Ba vai khách hàng của giao dịch (`CustomerCodeBuyer`/`Holder`/`Driver`).
    /// ⚠️ Với bán buôn ĐL→ĐL nguồn gán `CustomerCodeBuyer = DealerCodeBuyer` (chính đại lý mua).
    /// </summary>
    public string? CustomerCodeBuyer { get; set; }
    public string? CustomerCodeHolder { get; set; }
    public string? CustomerCodeDriver { get; set; }
    public string? CreatedBy { get; set; }
    /// <summary>Cờ giao dịch KHỞI TẠO (`FlagInitDeal`).</summary>
    public string? FlagInitDeal { get; set; }
    /// <summary>
    /// 🔴 Số hợp đồng đại lý (`DlrContractNo`) do chính lượt bán buôn này SINH RA — xem nợ ghi ở
    /// endpoint `/api/dealerdeals/todealer`.
    /// </summary>
    public string? DlrContractNo { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Xe trên giao dịch bán buôn ĐL→ĐL — port 1:1 FrmNewDealToDealer detail.</summary>
public sealed class WholesaleDealCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long WholesaleDealId { get; set; }
    public string VIN { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public decimal UnitPrice { get; set; }

    // ===== #157 parity DLS_DealDetail (Biz.HTC.WH.cs:93352-93366) =====
    /// <summary>Khoá dòng xe (`CarId`) — nguồn định danh xe bằng CarId, VIN chỉ là thông tin hiển thị.</summary>
    public string? CarId { get; set; }
    /// <summary>
    /// 🔴 Số giao dịch TRƯỚC ĐÓ của cùng chiếc xe (`DealNoPrevious`) — chuỗi chuyền tay
    /// ĐL→ĐL nối lại được nhờ cột này. Port cũ mất hẳn, nên không truy được lịch sử sang tay.
    /// </summary>
    public string? DealNoPrevious { get; set; }
    public string? PlateNo { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public string? DeliveryStatus { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public string? ConfirmBy { get; set; }
    /// <summary>Cờ dòng HIỆN HÀNH (`FlagCurrent`) — chỉ một dòng của mỗi xe là "đang có hiệu lực".</summary>
    public string? FlagCurrent { get; set; }
    /// <summary>Khoá dòng xe bên HỢP ĐỒNG đại lý (`CtrCarId`) — nối sang `Dlr_ContractCar`.</summary>
    public string? CtrCarId { get; set; }
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Bản ghi giao dịch bán xe để sửa field — port 1:1 cụm FrmEditDeal_* (DealDate/PlateNo/SalesType/SoBaoHanh/KHGD/KiemChung).</summary>
public sealed class DealRecord
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? DealDate { get; set; }        // ngay giao dich
    public string PlateNo { get; set; } = "";       // bien so
    public string SalesType { get; set; } = "";      // kieu ban
    public string WarrantyNo { get; set; } = "";     // so bao hanh (SoBaoHanh)
    public string CustomerCode { get; set; } = "";   // KH giao dich (KHGD)
    public string VerifyStatus { get; set; } = "";   // kiem chung (KiemChung)
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Log patch field GD (audit old→new) — port 1:1 cụm FrmEditDeal_*.</summary>
public sealed class DealPatchLog
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DealRecordId { get; set; }
    public string DealNo { get; set; } = "";
    public string Field { get; set; } = "";
    public string OldValue { get; set; } = "";
    public string NewValue { get; set; } = "";
    public DateTime PatchedAt { get; set; } = DateTime.Now;
}

/// <summary>Đẩy Sổ Bảo Hành lên hệ thống online (SBHOnline) — port 1:1 Frm_RePostSBHOnline.</summary>
public sealed class SbhOnline
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string CarId { get; set; } = "";
    public string DealNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public DateTime? DeliveryDate { get; set; }
    public string PostStatus { get; set; } = "Pending";   // Pending -> Posted (co the day lai)
    public int PushCount { get; set; }
    public DateTime? LastPushAt { get; set; }
    /// <summary>
    /// 🔴 Ngày hết hạn bảo hành của xe (`WarrantyExpiresDate`) — `RePush_SBHOnline`
    /// (2010.HTC `Biz.HTC.WH.hkt.cs:5867`) **CHẶN đẩy** nếu cột này rỗng
    /// (`RePush_SBHOnline_InvalidWarrantyExpiresDate`).
    /// ⚠️ Dòng SQL `--and t.WarrantyExpiresDate is not null` trong nguồn **đã bị comment**, nhưng guard
    /// C# ngay bên dưới vẫn ACTIVE ⇒ port theo guard C# (luật "port dòng ACTIVE").
    /// </summary>
    public DateTime? WarrantyExpiresDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đồng bộ VIN↔GPS từ file (DMSVIN sync) — port 1:1 FrmDongBoVIN. 1 dòng = 1 map VIN-GPS.</summary>
public sealed class GpsVinSync
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public string GpsId { get; set; } = "";
    public DateTime MapTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhà vận chuyển xác nhận biên bản giao nhận (StoDlvMinutes confirm) — port 1:1 FrmMngDlvMinutes (TERP.TranspClient). Header.</summary>
public sealed class TranspDlvConfirm
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlvMinutesNo { get; set; } = "";
    public string TransporterCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    /// <summary>
    /// ⚠️ #199 KHÔNG CÓ Ở NGUỒN. Bảng nguồn `Sto_DlvMinutes` **không có cột nào tên `ConfirmStatus`**
    /// (đã grep toàn `TERP.BizHTC`), và hệ nhà vận chuyển `TERP.WSTransp` **không có lệnh ghi nào** cho
    /// biên bản giao nhận — màn gốc `TERP.TranspClient/Views/Transp/FrmMngDlvMinutes.cs` CHỈ ĐỌC
    /// (đúng ba nút: Export · Search · Close). Trục xác nhận THẬT là `FDlvMnStatus`/`TDlvMnStatus` (P/A/R).
    /// Giữ cột để không phá dữ liệu đã ghi, nhưng KHÔNG endpoint nào còn ghi vào nó nữa.
    /// </summary>
    public string ConfirmStatus { get; set; } = "Pending";
    public string Remark { get; set; } = "";
    public DateTime? ConfirmDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // --- Biên bản giao nhận có HAI PHÍA duyệt độc lập (Sto_DlvMinutes, 2010.HTC) ---
    // F = phía GIAO (kho/nhà máy HTC), T = phía NHẬN (đại lý). Mã trạng thái theo TConst.Stage: P/A.
    // Duyệt phía F KHÔNG tự chốt phía T — nguồn cố ý comment dòng cập nhật TDlvMnStatus (--20131126).

    /// <summary>Trạng thái duyệt phía giao (FDLVMNSTATUS): P = chờ duyệt, A = đã duyệt.</summary>
    public string FDlvMnStatus { get; set; } = "P";
    // (`DlvEndDate` đã khai báo ở trên) — `CarDeliveryDate_Update` cập nhật cột đó với
    // **guard `FDlvMnStatus = 'A'`**: chỉ biên bản đã duyệt phía F mới được sửa ngày giao.

    /// <summary>Trạng thái duyệt phía nhận (TDLVMNSTATUS): P = chờ duyệt, A = đã duyệt.</summary>
    public string TDlvMnStatus { get; set; } = "P";

    public DateTime? FApprovedDate { get; set; }
    public string? FApprovedBy { get; set; }
    public DateTime? TApprovedDate { get; set; }
    public string? TApprovedBy { get; set; }

    /// <summary>Yêu cầu vận chuyển gắn với biên bản (TRANSPREQNO / TRANSPREQTYPE).</summary>
    public string? TranspReqNo { get; set; }
    public string? TranspReqType { get; set; }

    /// <summary>
    /// 🔴 #B05: Số chứng từ NGUỒN của biên bản (`Sto_DlvMinutes.RefOrdNo`) — **khác** <see cref="TranspReqNo"/>.
    /// Báo cáo *Xe chuyển sai vùng thị trường chính* nối `sdm.RefOrdNo = cdod.DeliveryOrderNo`
    /// (`BizHTC.ZTempGPS.cs:9078-9081`) để lấy dòng lệnh giao xe đang ở `ConfirmStatus in ('A','F')`.
    /// Không có cột này thì không lần được biên bản về lệnh giao.
    /// </summary>
    public string? RefOrdNo { get; set; }

    /// <summary>Kho + địa chỉ hai đầu tuyến (FSTORAGECODE/TSTORAGECODE, FADDRESS/TADDRESS).</summary>
    public string? FStorageCode { get; set; }
    public string? TStorageCode { get; set; }
    public string? FAddress { get; set; }
    public string? TAddress { get; set; }

    /// <summary>Ngày xuất kho / ngày giao đến (DLVSTARTDATE / DLVENDDATE).</summary>
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }

    /// <summary>Tuyến giao theo BIÊN BẢN (`Sto_DlvMinutes`): `FProvinceCode`/`FDistrictCode` nơi đi,
    /// `TProvinceCode`/`TDistrictCode` nơi đến — `Support_Sto_DlvMinutes_UpdateProvinceAndDistrict`
    /// sửa đúng 4 cột này (khác cặp tỉnh/huyện theo XE ở entity khác).</summary>
    public string? FProvinceCode { get; set; }
    public string? FDistrictCode { get; set; }
    public string? TProvinceCode { get; set; }
    public string? TDistrictCode { get; set; }

    /// <summary>Xe tải + lái xe lúc GIAO (PLATENO/DRIVERID) và lúc NHẬN (TPLATENO/TDRIVERID) — có thể đổi giữa đường.</summary>
    public string? PlateNo { get; set; }
    public string? DriverId { get; set; }
    public string? TPlateNo { get; set; }
    public string? TDriverId { get; set; }

    /// <summary>Ghi chú riêng của từng phía (FREMARK / TREMARK).</summary>
    public string? FRemark { get; set; }
    public string? TRemark { get; set; }

    /// <summary>Số km ghi nhận hai phía (FSTATUS_IA_KM / TSTATUS_IA_KM) + ghi chú kèm.</summary>
    public string? FStatusIaKm { get; set; }
    public string? TStatusIaKm { get; set; }
    public string? FStatusIaRemark { get; set; }
    public string? TStatusIaRemark { get; set; }

    // ===== #168 parity `Sto_DlvMinutes_InputFee_New20190416` (8811) + `_Correct_New20190416` (4915) =====
    //   `TERP.BizHTC/BizHTC.Storage.DlvMinutes.cs`, csproj 120 — BƯỚC 3B: md5 cả file `0b3b957d` KHỚP 2 máy.
    /// <summary>Tên lái xe tại nơi NHẬN — `_Correct` bắt đủ **cả ba** `TPlateNo`/`TDriverId`/`TDriverName`
    /// (`Correct_InvalidTTVanTai`); port cũ đã có hai cột đầu, thiếu cột này ⇒ không dựng được guard.</summary>
    public string? TDriverName { get; set; }
    /// <summary>Mốc ĐÍNH CHÍNH của HTC (`CorrectDate`/`CorrectBy`).</summary>
    public DateTime? CorrectDate { get; set; }
    public string? CorrectBy { get; set; }
    /// <summary>
    /// 🔴 Phí vận chuyển thực tế (`TFValReal`) và tiền phạt trễ hạn (`TPValReal`) trên CHÍNH biên bản.
    /// `_InputFee` nhận cờ `strFlagFeeOrPer` chỉ chấp nhận **"FEE"** hoặc **"PER"** và **mỗi lần chỉ nhập
    /// MỘT trong hai**; cờ khác ⇒ `InputFee_InvalidFeeOrPer`.
    /// ⚠️ Khác hai cột cùng tên trên dòng đề nghị thanh toán phí vận chuyển (`TransportInsPaymentLine`).
    /// </summary>
    public decimal TFValReal { get; set; }
    public decimal TPValReal { get; set; }
    public string? TFRemark { get; set; }
    public DateTime? TFInputDate { get; set; }
    public string? TFInputBy { get; set; }
    /// <summary>Tình trạng thiết bị GPS tại nơi nhận (`TGPSDvStatus`) — `_Correct` ghi cùng lượt.</summary>
    public string? TGPSDvStatus { get; set; }

    // ===== #169 parity `Sto_DlvMinutes_Confirm_New20190416` (BizHTC.Storage.DlvMinutes.cs:2822, csproj 120) =====
    /// <summary>Mã bản biểu phí vận chuyển áp cho biên bản (`TFVCode`) — khoá tra `Mst_TranspFee`
    /// để lấy `ExpectedDays`, tức số ngày vận chuyển ĐỊNH MỨC của tuyến.</summary>
    public string? TFVCode { get; set; }
    /// <summary>
    /// 🔴 Tiền phạt trễ hạn **HỆ THỐNG TỰ TÍNH** khi đại lý xác nhận (`TPValSys`), bậc thang GIẢM DẦN:
    /// với n = (DlvEndDate − DlvStartDate).Days − ExpectedDays ngày trễ,
    /// TPValSys = Σ(i = n → 1) [ValBased + (i−1)·ValEx] = n·ValBased + ValEx·n(n−1)/2.
    /// Nguồn gán `TPValReal = TPValSys` ngay tại bước xác nhận; `_InputFee` (#168) mới là chỗ sửa tay sau đó.
    /// ⚠️ Nếu tuyến KHÔNG có dòng `Mst_TranspFee` khớp, nguồn **không ném lỗi** (throw bị comment) và
    /// nhánh tính phạt nằm trong `else` ⇒ kết quả là **không phạt**.
    /// </summary>
    public decimal TPValSys { get; set; }
    /// <summary>Mã bản biểu phạt đang hiệu lực (`TPVCode`, nguồn lấy từ `Mst_TranspPenaltyVer` `FlagActive='1'`).</summary>
    public string? TPVCode { get; set; }
    /// <summary>Thiết bị GPS gắn với xe lúc xác nhận (`GPSDvNo`, nguồn tra `Sto_StoBalanceGPS` theo VIN)
    /// cùng cặp mốc `DlvEndGPSDateTime`/`DlvEndGPSBy`.</summary>
    public string? GPSDvNo { get; set; }
    public DateTime? DlvEndGPSDateTime { get; set; }
    public string? DlvEndGPSBy { get; set; }
    /// <summary>Địa chỉ/kết quả trả về từ **Veloca** (`GPSDvAddress`/`GPSDvResponse`) — nguồn gọi API
    /// `Veloca_SearAddress` tối đa 3 lần và **nuốt mọi lỗi**, xác nhận vẫn thành công.</summary>
    public string? GPSDvAddress { get; set; }
    public string? GPSDvResponse { get; set; }
    /// <summary>Mốc bấm "Lưu" biên bản của đại lý (`DlvEndDateTime`/`DlvEndBy`) — **khác** `DlvEndDate`
    /// (ngày nhận xe do người dùng nhập).</summary>
    public DateTime? DlvEndDateTime { get; set; }
    public string? DlvEndBy { get; set; }

    // ===== #170b nhat ky sua cuoi (LogLU*) — nguon ghi cap nay o moi buoc ghi =====
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>
/// Một mục kiểm tra trên biên bản giao nhận xe (Sto_DlvMinutes cột FSTATUS_* / TSTATUS_*,
/// port 1:1 FrmHTCNewDlvMinutes / FrmHTCMngDlvMinutes, 2010.HTC TERP.HTCClient/Views/Sales/DlvMinutes).
/// Nguồn để 34 mục kiểm tra thành 68 cột phẳng (mỗi mục 2 cột F/T); ở đây mô hình hoá thành
/// bảng chi tiết một-dòng-một-mục để thêm/bớt mục không phải đổi schema.
/// Mỗi mục được chấm ĐỘC LẬP hai phía: F = bên giao ghi nhận, T = bên nhận ghi nhận.
/// </summary>
public sealed class DlvMinutesCheckItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TranspDlvConfirmId { get; set; }

    /// <summary>Nhóm mục kiểm tra: OS (ngoại thất), IS (nội thất), SP (phụ tùng kèm xe), DA (giấy tờ).</summary>
    public string ItemGroup { get; set; } = "";

    /// <summary>Mã mục kiểm tra, đúng phần đuôi tên cột nguồn (vd "Paint" trong FSTATUS_OS_PAINT).</summary>
    public string ItemCode { get; set; } = "";

    /// <summary>Kết quả kiểm tra phía GIAO — rỗng nghĩa là chưa chấm.</summary>
    public string? FStatus { get; set; }

    /// <summary>Kết quả kiểm tra phía NHẬN — rỗng nghĩa là chưa chấm.</summary>
    public string? TStatus { get; set; }
}

/// <summary>
/// File ngân hàng gửi kèm giao dịch tài trợ (`RQ_BankingTransBankFile`).
/// 🔴 Nguồn CHỈ nhận file khi trạng thái ngân hàng = "F", **hoặc** ngân hàng là VietinBank và trạng thái = "A4"
/// (`BizHTC.VPBank.cs:4000-4006`); và **tổng dung lượng file trong 1 lần gọi API &lt;= 10MB**.
/// </summary>
public sealed class BankingTransBankFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Khoá về <see cref="BankingTrans"/> (bảng nguồn \`RQ_BankingTransactions\`).</summary>
    public long BankingTransId { get; set; }
    public int FileIndex { get; set; }
    public string? FileType { get; set; }
    public string? FilePath { get; set; }
    public string FileName { get; set; } = "";
    public string? DocumentType { get; set; }
    /// <summary>Dung lượng file (byte).</summary>
    public long FileSize { get; set; }
    public string? Remark { get; set; }
    /// <summary>Trạng thái ngân hàng tại thời điểm gửi file.</summary>
    public string? BkTransBankStatus { get; set; }
    /// <summary>Trạng thái ký (`TConst.SigningStatus`): P chờ · A đã duyệt · F hoàn tất.</summary>
    public string? SignStatus { get; set; }

    // ===== #192 parity `RQ_BankingTransactions_SignBankFile` (BizHTC.VietinBank.cs:21812) — 64-bit only =====
    /// <summary>Số serial chứng thư số dùng để ký — nguồn BẮT BUỘC không rỗng, kiểm TRƯỚC khi tra DB.</summary>
    public string? SerialNumber { get; set; }
    public string? LogLUBy { get; set; }
    public DateTime? LogLUDateTime { get; set; }

    // ===== 🔴 #275: VỊ TRÍ ĐẶT Ô CHỮ KÝ trên file, do NGÂN HÀNG báo ngược về =====
    // Nguồn: `ERP.DMS.HTC.VPBank.WS/TERP.BizHTC/BizHTC.VPBank.cs:6285 UpdateTransBankFile` —
    //   cây `ERP.DMS.HTC.VPBank.WS` **CHỈ CÓ TRÊN MÁY 150** (laptop không có thư mục này).
    // Luồng hai bước: ngân hàng gọi `GetTransBankFile` lấy các file `SignStatus = 'P', rồi gọi
    //   `UpdateTransBankFile` báo lại **trang số mấy và toạ độ/kích thước ô ký**.
    /// <summary>PAGEIDX — trang đặt chữ ký. Nguồn kiểm `IsInteger64` (**số nguyên**).</summary>
    public long? PageIdx { get; set; }
    /// <summary>ELEMENTX/Y/WIDTH/HEIGHT — toạ độ và kích thước ô ký.
    /// ⚠️ Nguồn kiểm bốn giá trị này bằng `IsNumeric` (**cho phép thập phân**), KHÁC `PageIdx` — đừng
    /// "làm cho đồng bộ" thành số nguyên hết.</summary>
    public decimal? ElementX { get; set; }
    public decimal? ElementY { get; set; }
    public decimal? ElementWidth { get; set; }
    public decimal? ElementHeight { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Xe trên biên bản giao nhận vận chuyển — port 1:1 FrmMngDlvMinutes detail.</summary>
public sealed class TranspDlvConfirmCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long TranspDlvConfirmId { get; set; }
    public string VIN { get; set; } = "";
    public string ModelCode { get; set; } = "";

    // --- Tuyến vận chuyển: thuộc TỪNG XE, không phải header ---
    // 🔴 Căn cứ: hàm sửa lô của nguồn khoá theo **CẶP (số biên bản, VIN)** — một biên bản chở nhiều xe,
    // mỗi xe một tuyến riêng. Bộ thực thể song trùng cũ (`DlvMinutes`) để tuyến ở HEADER + 1 VIN/biên bản
    // ⇒ mô hình sai, không chở được nhiều xe.
    public string? FProvinceCode { get; set; }  // tuyến theo XE
    public string? TProvinceCode { get; set; }
    public string? FDistrictCode { get; set; }
    public string? TDistrictCode { get; set; }
    public string? DriverCode { get; set; }
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }
}

/// <summary>Bản ghi bán hàng cho báo cáo HMC (ReportHMC) — port 1:1 FrmHMCReport.</summary>
public sealed class HmcSalesRecord
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VIN { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public DateTime TransactionDate { get; set; }
    public string DeliveryType { get; set; } = "";     // loai giao xe
    public string SalesType { get; set; } = "";         // loai ban (2 ky tu -> N/O)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đơn hàng chưa giao / back-order (ReportBackOrder) — port 1:1 FrmBackOrderByModel + FrmBackOrderByDealer.</summary>
public sealed class BackOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string DealerName { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string SpecDesc { get; set; } = "";
    public int QtyOrder { get; set; }         // SL da dat
    public int QtyDelivered { get; set; }      // SL da giao
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// ⛔ **DEPRECATED — THỰC THỂ SONG TRÙNG** (ca thứ 6 — ca CUỐI của sweep, phát hiện #56, xử lý #59).
/// `GpsUnitPrice` và <see cref="MstUnitPriceGPS"/> **cùng map bảng nguồn `Mst_UnitPriceGPS`**,
/// cột gần như y hệt (ContractNo/UnitPrice/EffStartDate/FlagActive).
/// Endpoint `/api/gpsunitprices` đã trỏ sang <see cref="MstUnitPriceGPS"/>. Giữ lớp này để đọc dữ liệu cũ, **KHÔNG ghi mới**.
/// </summary>
public sealed class GpsUnitPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ContractNo { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public DateTime? EffStartDate { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Chi phí lưu kho theo kho + loại chi phí — port 1:1 FrmMst_QuanLyChiPhiLuuKho (Tbl_Mst_InventoryCost).</summary>
public sealed class InventoryCost
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StorageCode { get; set; } = "";
    public string? StorageName { get; set; }
    public string CostTypeCode { get; set; } = "";
    public string? CostTypeName { get; set; }
    public decimal UnitPrice { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// #232 ĐỊA ĐIỂM GIAO HÀNG `Mst_DeliveryLocation` — port 1:1 `FrmMst_DeliveryLocationMng` (525 dòng) +
/// `FrmMst_DeliveryLocation` (278 dòng, TCMotor DMSCarSv/TST); biz `BizCarSv.Master.cs:11108`
/// (`Mst_DeliveryLocation_GetX`) / `:11386` (`_Add`).
/// 🔴 **KHOÁ LÀ CẶP** `DeliveryLocationCode` + `DealerCode` — bằng chứng: `Mst_DeliveryLocation_Delete` và
///    `_Update` đều truyền CẢ HAI (Mst_DeliveryLocationService.cs:73, :99), và `Mst_DeliveryLocation_CheckDB`
///    nhận cả hai. ⇒ Mỗi đại lý có bộ địa điểm RIÊNG, hai đại lý được trùng mã.
/// ⚠️ Vì thế màn này KHÔNG hợp với catalog `MasterItem` chung (khoá chỉ `Category+Code`) — đã gỡ khỏi
///    `MasterCatalog` và port thành entity riêng, theo đúng tiền lệ BOM/ExtraWork.
/// Sáu cột của nguồn (danh sách `MyBuildDBDT_Common` + `insert into`, BizCarSv.Master.cs:11536).
/// </summary>
public sealed class DeliveryLocation
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DeliveryLocationCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string DeliveryLocationName { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Chiến dịch marketing dịch vụ (header: tên/mô tả/điều kiện đại lý) — port 1:1 FrmSer_CampaignMarketing (Tbl_Ser_CampaignMarketing, TCMotor).</summary>
public sealed class ServiceCampaign
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    public string? CamName { get; set; }
    public string? CamDesc { get; set; }
    public string? ConditionDealer { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = "Draft"; // Draft -> Active -> Closed
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phụ tùng giảm giá trong chiến dịch (detail: mã PT + % giảm) — port 1:1 FrmSer_CampaignMarketing grid, TCMotor.</summary>
public sealed class ServiceCampaignPart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceCampaignId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal PercentDiscount { get; set; }
}

/// <summary>
/// #228 BẢNG GIÁ GỬI TIN `Mst_PriceSend` — nguồn `SMS.V10/SMS.Biz/BizSMS.MasterData.cs:851`
/// (`Mst_PriceSend_Get`, hệ SMS.V10 **CHỈ có trên máy 150**).
/// Khoá nghiệp vụ **4 phần**: `CostType` + `SupplierCode` + `TelCoCode` + `EffectDate`
/// (đúng `select distinct` và `on` của nguồn). ⚠️ `BatchType` là cột thật nhưng **KHÔNG thuộc khoá**.
/// Ba cột tên (`CostTypeName`/`SupplierName`/`TelCoName`) ở nguồn lấy bằng `left join` sang
/// `Mst_CostType` / `Mst_Supplier` / `Mst_TelCo` — MiniHTC chưa port 3 master đó nên giữ dạng
/// phi chuẩn hoá (NỢ đã ghi manifest), không bịa ra 3 bảng master.
/// </summary>
public sealed class SmsPriceSend
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CostType { get; set; } = "";        // MPS.COSTTYPE   — loại tin (NM/BN…)
    public string SupplierCode { get; set; } = "";    // MPS.SUPPLIERCODE — nhà cung cấp SMS
    public string TelCoCode { get; set; } = "";       // MPS.TELCOCODE  — mạng nhận (VIETTEL…)
    public string? BatchType { get; set; }            // MPS.BATCHTYPE  — CSKH/QC, KHÔNG thuộc khoá
    public DateTime EffectDate { get; set; }          // MPS.EFFECTDATE — ngày bắt đầu hiệu lực
    public decimal UnitPrice { get; set; }            // MPS.UNITPRICE  — đơn giá 1 PHẦN tin
    public DateTime? LuDTime { get; set; }            // MPS.LUDTIME
    public string? LuBy { get; set; }                 // MPS.LUBY
    // ba cột enrich (nguồn lấy qua left join, xem chú thích lớp)
    public string? CostTypeName { get; set; }
    public string? SupplierName { get; set; }
    public string? TelCoName { get; set; }
}

/// <summary>Tài khoản SMS trả trước (số dư) — port 1:1 FrmSMSAccountMng (TblSMS_Account, TCMotor).</summary>
public sealed class SmsAccount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AccountName { get; set; } = "";
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #230: bổ sung theo `Acc_Balance_Get` (SMS.V10/SMS.Biz/BizSMS.Account.cs:1431) =====
    // Nguồn trả `Acc_Balance` (ACCOUNTCODE · BALANCE · OVERDRAFTTHRESHOLD) + left join `Acc_Account`
    // (AAACCOUNTNAME · AAFLAGACTIVE · AAFLAGSYSADMIN). Port cũ chỉ có tên + số dư.

    /// <summary>ACCOUNTCODE — **khoá thật** của tài khoản SMS; `AccountName` chỉ là tên hiển thị
    /// (nguồn lấy từ bảng khác: `Acc_Account.AccountName`).</summary>
    public string? AccountCode { get; set; }

    /// <summary>
    /// OVERDRAFTTHRESHOLD — 🔴 **HẠN MỨC THẤU CHI**: số dư được phép ÂM tới ngưỡng này.
    /// Nguồn kiểm bằng `(ab.Balance + ab.OverdraftThreshold) MyCheck` và chỉ báo lỗi khi `MyCheck &lt; 0`
    /// (BizSMS.Account.cs:212, :282). ⇒ chặn ở mốc `-OverdraftThreshold`, **KHÔNG** chặn ở 0.
    /// </summary>
    public decimal OverdraftThreshold { get; set; }

    /// <summary>AAFLAGACTIVE — cờ "1"/"0" của `Acc_Account`.</summary>
    public string FlagActive { get; set; } = "1";

    /// <summary>AAFLAGSYSADMIN — tài khoản quản trị SMS. 🔴 Quyết định **hàng rào dữ liệu**: nguồn thay
    /// `zzzzClauseWhere_FilterAbilityOfUser` bằng `(ab.AccountCode = @strAccountCode) and` khi KHÔNG phải SA,
    /// còn SA thì `-- Nothing.` (thấy hết). Xem `/api/smsbrandnames`.</summary>
    public string FlagSysAdmin { get; set; } = "0";
}

/// <summary>
/// #230 BRANDNAME CỦA TÀI KHOẢN SMS `Acc_BrandName` — nguồn `Acc_BrandName_Get`
/// (SMS.V10/SMS.Biz/BizSMS.Account.cs:1729, hệ **chỉ có trên máy 150**).
/// Dùng ở `FrmSendSMSAdvertisement` (:238) và `FrmSendSMSOther` (:238) để đổ danh sách người gửi,
/// và ở `SMS_Batch_Send` (:133) để lấy giá trị ghi vào `Sms_Send.BranchName`.
/// Cột theo `BuildColumns`: BRANDNAME (khoá, `select distinct abn.BrandName`) · ACCOUNTCODE · LUDTIME · LUBY.
/// </summary>
public sealed class SmsBrandName
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BrandName { get; set; } = "";
    public string? AccountCode { get; set; }
    public DateTime? LuDTime { get; set; }
    public string? LuBy { get; set; }
}

/// <summary>Giao dịch tài khoản SMS (nạp/trừ) — port 1:1 FrmSMSAccountMng ledger (Acc_Transaction, TCMotor).</summary>
public sealed class SmsAccountTx
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SmsAccountId { get; set; }
    public string TRefType { get; set; } = "";   // Topup | Deduct
    public decimal Value { get; set; }
    public decimal BalanceAfter { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedDTime { get; set; } = DateTime.Now;
}

/// <summary>Log gửi tin nhắn SMS (số ĐT/nội dung/trạng thái) — port 1:1 FrmSendSMS (TblSMS_Send, TCMotor).</summary>
/// <summary>Log gửi email tới khách hàng dịch vụ — port 1:1 FrmSendEmail (Email_SendEmail, TCMotor).</summary>
public sealed class EmailSend
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string? EmailType { get; set; }
    public string Subject { get; set; } = "";
    public string Body { get; set; } = "";

    /// <summary>
    /// 🔴 Trạng thái gửi là CỜ "1"/"0" của nguồn (`Constants.Flag`), KHÔNG phải chuỗi "Sent"/"Invalid":
    /// "0" chưa gửi (mới đưa vào hàng đợi) · "1" đã xử lý gửi.
    /// ⚠️ Nguồn tạo dòng hàng đợi ở "0" (`FrmSendEmail.cs:466` = `Flag.Inactive`) rồi mới gửi;
    /// chỉ khi gửi xong mới cập nhật "1" (`BizCarSv.SendMail.cs:616`). Port cũ đánh dấu
    /// "Sent" NGAY LÚC TẠO ⇒ không phân biệt được "đã xếp hàng" với "đã gửi".
    /// Giữ đọc được dữ liệu cũ: "Sent" ⇒ "1", "Invalid" ⇒ "0" + <see cref="InvalidEmail"/>.
    ///
    /// 🔴 #300 SỬA LẠI: trạng thái **KHÔNG chỉ có hai giá trị**. Báo cáo LIVE
    /// `Email_ReportCusReceivedEmail` (`SendMail.cs:4630`) ánh xạ **BA**:
    ///   `'0'` Chưa gửi · `'1'` Thành công · `'2'` **Thất bại**  (whitelist — **không có `else`**
    ///   ⇒ mã lạ và NULL cho ra **nhãn NULL**, ô trống trên lưới).
    /// Giá trị do **người gọi truyền vào** (`:322` `strStatus`, `:520` `strStatusNew`) nên `'2'` là giá trị
    /// hợp lệ mà job ghi khi gửi hỏng. Mô hình cũ chỉ mô tả "0"/"1" ⇒ **không có chỗ ghi GỬI HỎNG**,
    /// mọi lần gửi thất bại bị kẹt ở "0" (trông như còn trong hàng đợi).
    /// ⚠️ Nguồn ghi **DBNull khi tham số rỗng** (`:318`) ⇒ NULL cũng là giá trị thật.
    /// ⚠️ ĐỪNG lẫn với <see cref="EmailSendAutoTemp.Status"/>: bảng kia dùng SỐ và có mã **-1**.
    /// </summary>
    public string Status { get; set; } = "0";

    /// <summary>Địa chỉ email sai định dạng — lỗi DỮ LIỆU, tách khỏi kết quả gửi (như đã làm cho SMS).</summary>
    public bool InvalidEmail { get; set; }

    /// <summary>Địa chỉ gửi đi (FromAddress).</summary>
    public string? FromAddress { get; set; }

    /// <summary>Mã khách hàng nhận (CusID) — nguồn lưu để truy ngược email về khách.</summary>
    public string? CusId { get; set; }

    /// <summary>Gửi TỰ ĐỘNG hay gửi tay (IsAuto) — cờ "1"/"0".</summary>
    public string IsAuto { get; set; } = "0";

    public string? DealerCode { get; set; }
    /// <summary>Tên file đính kèm của riêng dòng này (FileAttachment).</summary>
    public string? FileAttachment { get; set; }
    /// <summary>Người thao tác (UserName).</summary>
    public string? UserName { get; set; }
    /// <summary>Ghi chú / lý do lỗi gửi (Note/Remark) — port cũ không có chỗ ghi lỗi gửi.</summary>
    public string? Note { get; set; }

    public DateTime SendDate { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 #300 BẢNG TẠM GỬI EMAIL TỰ ĐỘNG — `Email_SendEmailAutoTemp` (`BizCarSv.SendMail.cs:3829/4052`),
/// **chưa từng port**. Đây là **hàng đợi NGƯỜI NHẬN** của một lô gửi tự động: mỗi khách một dòng, dựng
/// sẵn `Subject`/`Body` đã ghép biến, rồi job mới đọc ra gửi và ghi kết quả sang `EmailSend`.
/// Thiếu bảng này thì **không biết một lô nhắm tới bao nhiêu khách**, chỉ biết đã gửi được bao nhiêu.
///
/// Hai `[WebMethod]` LIVE: `Email_SendEmailAutoTemp_Create` (`WSCarSv.asmx.cs:22308`) ·
/// `_Update` (`:22396`). Hai hàm đọc LIVE: `Temp_Email_Get` (`:22929`) · `Temp_Email_Get_Detail`.
/// ⚠️ Bảng nằm ở DB `@strDBName_CommonCenter` (dùng chung), không phải DB đại lý.
/// </summary>
public sealed class EmailSendAutoTemp
{
    public long Id { get; set; }                 // AutoTempID
    public Guid OrgId { get; set; }

    /// <summary>BATCHID — lô mà dòng này thuộc về. ⚠️ **KHÔNG có trong chữ ký `_Create`**: nguồn gán ở
    /// chỗ khác (lúc dựng lô), nên dòng vừa tạo có thể còn mồ côi.</summary>
    public string? BatchId { get; set; }

    public string? DealerCode { get; set; }
    public string? CusID { get; set; }
    public string? CusEmail { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }

    /// <summary>CURRENTDATE — ngày ghi nhận dòng (nguồn nhận dạng CHUỖI).</summary>
    public string? CurrentDate { get; set; }

    /// <summary>TYPEEMAIL — 1..7, cùng bảng mã với `EmailSend.EmailType`.</summary>
    public string? TypeEmail { get; set; }

    /// <summary>CONFIGAUTOID — cấu hình gửi tự động sinh ra dòng này.</summary>
    public string? ConfigAutoID { get; set; }

    /// <summary>
    /// 🔴 STATUS — **bảng mã SỐ, có mã ÂM**, khác hẳn cờ "1"/"0" của <see cref="EmailSend"/>:
    ///   `1` Thành công · `-1` **Thất bại** · `0` Chưa gửi · **`else` ⇒ "Lỗi"**
    /// (`Temp_Email_Get_Detail`, `SendMail.cs:5497` — viết `when 1`/`when -1`/`when 0` KHÔNG nháy).
    /// ⚠️ Có nhánh `else` ⇒ **blacklist**: NULL và mọi mã lạ đều hiện "Lỗi", không phải ô trống.
    /// ⚠️ Nguồn ghi **DBNull khi tham số rỗng** (`:3960`/`:4191`) ⇒ NULL là giá trị THẬT, hiển thị "Lỗi".
    /// </summary>
    public string? Status { get; set; }

    /// <summary>SENDTYPE — kiểu gửi của dòng.</summary>
    public string? SendType { get; set; }

    /// <summary>REMARK — ghi chú/lý do lỗi, `Temp_Email_Get_Detail` trả kèm trạng thái.</summary>
    public string? Remark { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 HEADER LÔ gửi email (`Email_BatchSendEmail` — BizCarSv.SendMail.cs:1002-1060).
/// Port cũ chỉ có chuỗi `BatchNo` lặp trên từng dòng, KHÔNG có bản ghi lô ⇒ mất
/// ngày hiệu lực, người gửi, và **file đính kèm dùng chung cho cả lô**.
/// </summary>
public sealed class EmailBatch
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string? DealerCode { get; set; }
    /// <summary>Ngày hiệu lực / hẹn gửi của lô (EffectDate).</summary>
    public DateTime? EffectDate { get; set; }
    /// <summary>Người gửi lô (SendBy).</summary>
    public string? SendBy { get; set; }
    /// <summary>
    /// Tên file đính kèm dùng chung cả lô (`AttachmentName`).
    /// ⚠️ Cột RIÊNG của MiniHTC — nguồn KHÔNG để file ở bảng đầu mà ở bảng con
    /// `DMS40_Email_BatchSendEmailFileAttach` (nhiều file / lô). Giữ lại vì client cũ đang dùng.
    /// </summary>
    public string? AttachmentName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== #143 parity DMS40_Email_BatchSendEmail =====
    // Nguồn: DMS40/0.34.Contract.cs — `_SaveX` (12927) / `_Job_SaveX` (13853), csproj 125.
    // 🔴 Đây là HẠ TẦNG DÙNG CHUNG: hơn 20 chỗ trong hệ xếp mail vào bảng này (VietinBank,
    //    TCFIntergration, PaymentDiscount, PaymentGrtExt, BizHTC.Car, BizHTC.Report, Biz.HTC.WH.My,
    //    BizHTC.zTemp, 0.34.Contract…) — không phải một màn hình.
    /// <summary>Mã cấu hình SMTP/hộp gửi (`ConfigCode`).</summary>
    public string? ConfigCode { get; set; }
    /// <summary>Mã MẪU email (`TEmailCode`) — nội dung lấy từ mẫu, bảng này không lưu nội dung.</summary>
    public string? TEmailCode { get; set; }
    /// <summary>Đường dẫn web-service xử lý lô (`WSPath`).</summary>
    public string? WSPath { get; set; }
    /// <summary>
    /// Trạng thái LÔ (`BatchStatus`, `TConst.BatchStatus`, Const.Main.DMS40.cs:214):
    /// N/**P**/C/**A**/A1/A2/F/R/D. Nguồn tạo ở **"P"**, gửi xong đặt **"A"**.
    /// ⚠️ KHÁC bảng mã của <see cref="EmailSend.Status"/> — dòng người-nhận dùng cờ "1"/"0",
    /// còn LÔ dùng bảng mã chữ. Hai tầng, hai từ vựng.
    /// </summary>
    public string BatchStatus { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

// --- #143: ba bảng con của lô mà port cũ THIẾU HẲN ---
// Port cũ gộp mọi người nhận vào `EmailSend` (tương đương `DMS40_Email_BatchSendEmailTo`),
// nên CC / BCC / file đính kèm nhiều-file không có chỗ chứa.

/// <summary>Người nhận CC của lô (`DMS40_Email_BatchSendEmailCC`).</summary>
public sealed class EmailBatchCc
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string EmailCode { get; set; } = "";
    /// <summary>Trạng thái gửi tới địa chỉ này (`BatchStatusCC`) — cùng bảng mã `TConst.BatchStatus`.</summary>
    public string BatchStatusCC { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Người nhận BCC của lô (`DMS40_Email_BatchSendEmailBCC`).</summary>
public sealed class EmailBatchBcc
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string EmailCode { get; set; } = "";
    public string BatchStatusBCC { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>File đính kèm của lô (`DMS40_Email_BatchSendEmailFileAttach`) — chỉ lưu ĐƯỜNG DẪN, nhiều file/lô.</summary>
public sealed class EmailBatchFileAttach
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string FilePath { get; set; } = "";
    public string BatchStatusFA { get; set; } = "P";
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}


/// <summary>Cấu hình gửi SMS tự động theo giờ/loại + ngày hiệu lực — port 1:1 FrmSMSSetAutoSend (TblSMS_ConfigSendAuto, TCMotor).</summary>
public sealed class SmsAutoConfig
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SmsType { get; set; } = "";      // Normal | Other | Advertise ...
    public string AutoTime { get; set; } = "";     // "HH:mm"
    public DateTime? EffectDate { get; set; }
    public string? SendMode { get; set; }
    public string? Description { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Cấu hình gửi email tự động theo giờ/loại — port 1:1 FrmAutoSendConfig (TblEmail_ConfigSendAuto, TCMotor).</summary>
public sealed class EmailAutoConfig
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string EmailType { get; set; } = "";
    public string AutoTime { get; set; } = "";     // "HH:mm"
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SendMode { get; set; }
    public string? Description { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// #229 ĐẦU LÔ TIN NHẮN `Sms_Batch` — port 1:1 `FrmSendSMSAdvertisement.RefineSave` (706 dòng, DMSCarSv)
/// + `SmsOutService.SMS_Batch_Send` (SmsOutService.cs:79). Cột lấy từ lớp hằng `TblSMS_Batch`
/// (DbDefine.cs:1886) — 13 cột lưu trữ; `MYCOUNT_*` là cột TÍNH nên không lưu.
/// 🔴 Port cũ sinh `BatchNo` rồi vứt: **không có bản ghi đầu lô nào** ⇒ mất `EffectDTime`
///    (mốc ngày mà bảng giá `Mst_PriceSend` dùng để chọn giá hiệu lực — xem #228), mất `CostInit`
///    /`CostActual` và mất dấu vết huỷ lô.
/// </summary>
public sealed class SmsBatch
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchId { get; set; } = "";          // BATCHID — trùng BatchNo của SmsSend
    public string? AccountCode { get; set; }           // ACCOUNTCODE — tài khoản SMS gửi lô
    public string BatchType { get; set; } = "CSKH";    // BATCHTYPE — CSKH / QC

    /// <summary>
    /// CONTENTSTEMPLATE — 🔴 trường GHÉP bằng dấu `|` (RefineSave): khi có chọn loại báo giá thì
    /// `&lt;loại người gửi&gt;|&lt;tên loại báo giá&gt;|&lt;nội dung&gt;`, không thì chỉ `&lt;loại người gửi&gt;`.
    /// ⚠️ Đoạn đầu KHÔNG phải giá trị người gửi mà là **LOẠI** người gửi: hằng `SmsSendKey.BrandName`
    ///    = chuỗi `"BrandName"`, `SmsSendKey.PhonePrefix` = chuỗi `"Đầu số"` (Constants.cs:441).
    /// </summary>
    public string? ContentsTemplate { get; set; }

    /// <summary>EFFECTDTIME — 🔴 ngày hiệu lực của LÔ. Đây chính là `@strDateEffect` mà
    /// `#tbl_Mst_PriceSend_Effect` (BizSMS.SMS.cs:279) dùng để chọn giá — KHÔNG phải ngày gửi.</summary>
    public DateTime EffectDTime { get; set; } = DateTime.Now;

    /// <summary>EFFECTSTATUS — theo `TConst.SmsStage` (N/P/G/C/F/R); nguồn tạo lô ở "P".</summary>
    public string EffectStatus { get; set; } = "P";

    /// <summary>REMARK — ⚠️ nguồn GHI ĐÈ bằng hằng `SmsSendKey.BrandName` (chuỗi "BrandName")
    /// ngay trong `SMS_Batch_Send` (SmsOutService.cs:99), bất kể form đặt gì. Giữ nguyên hành vi.</summary>
    public string? Remark { get; set; }

    public decimal CostInit { get; set; }              // COSTINIT — tiền ước tính lúc tạo lô
    public decimal CostActual { get; set; }            // COSTACTUAL — tiền thực sau khi gửi
    public DateTime CreatedDTime { get; set; } = DateTime.Now;  // CREATEDDTIME
    public string? CreatedBy { get; set; }             // CREATEDBY
    public DateTime? CancelDTime { get; set; }         // CANCELDTIME
    public string? CancelBy { get; set; }              // CANCELBY
}

public sealed class SmsSend
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BatchNo { get; set; } = "";
    public string Mobile { get; set; } = "";
    public string? SmsType { get; set; }
    public string Contents { get; set; } = "";

    /// <summary>
    /// Trạng thái gửi theo TConst.SmsStage (6 giá trị):
    /// "N" Null · "P" Pending (chờ gửi) · "G" Progress (đang gửi) · "C" Cancel (huỷ) ·
    /// "F" Finish (gửi xong) · "R" Reject (gửi lỗi/từ chối).
    /// ⚠️ Nguồn tạo lô SMS với `EffectStatus = P` rồi mới gửi bất đồng bộ (FrmSendSMS.cs:398);
    /// KHÔNG đánh dấu "đã gửi" ngay lúc tạo như port cũ.
    /// `FrmSMSMng` đọc lại lô lỗi bằng chính "R".
    /// </summary>
    public string Status { get; set; } = "P";

    /// <summary>Số điện thoại không hợp lệ (port cũ đánh dấu bằng Status="Invalid" — nay tách thành cờ riêng).</summary>
    public bool InvalidMobile { get; set; }

    /// <summary>
    /// 🔴 Tin KHÔNG DẤU hay CÓ DẤU (`FlagANSI`) — quyết định số ký tự mỗi phần tin, do đó quyết định TIỀN:
    /// ANSI 160 ký tự (1 phần) / 153 (nhiều phần); Unicode chỉ 70 / 67.
    /// ⚠️ Nguồn đọc cột này từ DB; tôi KHÔNG tìm thấy chỗ ghi nó trong code đã đọc ⇒ ở đây cho truyền vào,
    /// nếu không truyền thì suy từ nội dung (mọi ký tự &lt;= 127 ⇒ ANSI). Đây là SUY LUẬN của tôi.
    /// </summary>
    public bool FlagANSI { get; set; }

    /// <summary>Nhà mạng nhận (`TConst.TelCo`): VIETTEL · MOBIFONE · VINAPHONE · VIETNAMOBILE.
    /// Ảnh hưởng TIỀN với tin quảng cáo. ⚠️ Nguồn không có hàm suy nhà mạng từ đầu số trong vùng đã đọc
    /// ⇒ KHÔNG tự suy, phải truyền vào.</summary>
    public string? TelCo { get; set; }

    /// <summary>Loại lô (`TConst.BatchType`): "CSKH" chăm sóc khách hàng · "QC" quảng cáo.
    /// 🔴 Tin QC có BẢNG GIÁ RIÊNG theo bậc thang — tính nhầm là sai tiền.</summary>
    public string BatchType { get; set; } = "CSKH";

    /// <summary>Loại chi phí (`TConst.CostType`): "NM" thường · "BN" brandname.</summary>
    public string CostType { get; set; } = "NM";

    /// <summary>Dự án phát sinh tin (`TConst.ProjectCode`): DMS · IDEALER · LOYALTY.</summary>
    public string? ProjectCode { get; set; }

    /// <summary>Đơn giá 1 PHẦN tin.</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>SỐ PHẦN tin sau khi chia — chính là hệ số nhân tiền.</summary>
    public int MsgParts { get; set; } = 1;

    /// <summary>
    /// Thành tiền ƯỚC TÍNH = <see cref="UnitPrice"/> × <see cref="MsgParts"/>.
    /// #231: cột này tương ứng `Sms_Send.CostInit` của nguồn (BizSMS.SMS.cs:1316 `dblCostInit`).
    /// Giữ tên `Cost` để không vỡ dữ liệu đã tạo; cặp đôi của nó là <see cref="CostActual"/>.
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// #231 COSTACTUAL — tiền THỰC bị trừ. 🔴 Nguồn tách ĐÔI `CostInit` / `CostActual`:
    /// lúc xếp hàng chỉ có `CostInit`; khi gửi mới đặt `ss.CostActual = t.UnitPrice * t.MyPartCount`
    /// (BizSMS.SMS.cs:262); khi huỷ lô thì `CostActual` bị **đưa về 0** và số đã trừ được HOÀN lại
    /// (BizSMS.SMS.cs:172-183). ⇒ tiền hoàn tính từ `CostActual`, KHÔNG phải từ `Cost`/`CostInit`.
    /// </summary>
    public decimal CostActual { get; set; }

    /// <summary>Số lần đã thử gửi — nguồn giới hạn `SMSTryCountMax = 1` (KHÔNG tự gửi lại).</summary>
    public int TryCount { get; set; }

    public DateTime SendDate { get; set; } = DateTime.Now;

    // ===== #229: 11 cột nguồn `Sms_Send` mà port cũ THIẾU (SmsOutService.cs:107-136) =====

    /// <summary>SENDID — mã dòng gửi, nguồn sinh bằng `CUtils.TidNext(batchId, ref seq)` ⇒ **tuần tự trong lô**.</summary>
    public string? SendId { get; set; }

    /// <summary>SUPPLIERPHONENO — số của NCC dùng để gửi. 🔴 Nguồn gán `SystemGlobal.SMSSupplierPhoneNo`
    /// ở **CẢ HAI nhánh** if/else (SmsOutService.cs:131 và :138) — hai nhánh chỉ khác nhau ở `BranchName`.
    /// Đây là khoá tra `Mst_SupplierPhoneNo` → `SupplierCode` của bảng giá (#228).</summary>
    public string? SupplierPhoneNo { get; set; }

    /// <summary>BRANCHNAME — ⚠️ nguồn viết SAI CHÍNH TẢ: cột tên là `BranchName` nhưng nghĩa là **BrandName**
    /// (tên thương hiệu hiện trên máy người nhận). Chỉ có giá trị khi người gửi là BrandName;
    /// gửi bằng đầu số thì nguồn ghi chuỗi RỖNG (không phải NULL).</summary>
    public string? BranchName { get; set; }

    /// <summary>FLAGREPLY — nguồn luôn đặt `Constants.Flag.Inactive` ("0") khi tạo dòng gửi.</summary>
    public string FlagReply { get; set; } = "0";

    // A10..A16: nguồn lưu ngữ cảnh người nhận dưới dạng CẶP (AxxName, AxxValue) — túi thuộc tính chung.
    // Ở đường gửi này 7 cặp LUÔN mang đúng 7 nghĩa dưới đây nên port thành cột có tên thật.
    public string? CusID { get; set; }              // A10  CUSID
    public string? CusName { get; set; }            // A11  CUSNAME
    public string? Address { get; set; }            // A12  ADDRESS
    public string? CarID { get; set; }              // A13  CARID
    public string? PlateNo { get; set; }            // A14  PLATENO
    public string? TradeMarkModel { get; set; }     // A15  "TradeMarkCode|ModelName" (ghép bằng |)
    public string? SendType { get; set; }           // A16  SENDTYPE (loại tin gửi)
}

/// <summary>Mẫu email theo loại nghiệp vụ (tiêu đề + nội dung + file đính kèm) — port 1:1 FrmEmail_TempEmailCreate (TblEmail_TempEmail, TCMotor).</summary>
public sealed class EmailTemplate
{
    // ===== 🔴 #438 §12 DEALERCODE — cột nguồn LỌC bằng mà bản port THIẾU HẲN =====
    //   `Email_TempEmail_Get` nhận `strDealerCode`, biz dựng `BuildClause("and", "tmp.**DealerCode**", …)`,
    //   và **cả hai** lời gọi của `FrmEmail_TempEmailList` đều truyền `SystemGlobal.strDealerCode`.
    //   ⇒ Mẫu thư là dữ liệu **theo từng đại lý**, không phải dùng chung. Thiếu cột này thì mọi đại lý
    //     nhìn chung một tập mẫu — sai mô hình dữ liệu, và §12 KHÔNG bắt được (lệ #403: cột thiếu HẲN).
    public string? DealerCode { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TempType { get; set; } = "";   // loại email
    public string? TempName { get; set; }
    public string? TempSubject { get; set; }
    public string TempBody { get; set; } = "";
    public string? FileAttachment { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Mẫu tin nhắn SMS theo loại nghiệp vụ (nội dung + trạng thái) — port 1:1 FrmSMSTemplate (TblSMS_Template, TCMotor).</summary>
public sealed class SmsTemplate
{
    public long Id { get; set; }                // = TempID của nguồn (identity)
    public Guid OrgId { get; set; }
    public string SmsType { get; set; } = "";   // loại: MAINT/BIRTHDAY/PROMO/...

    /// <summary>
    /// 🔴 #299 DEALERCODE — port cũ THIẾU HẲN, và đây là **lỗi ghi đè chéo đại lý**.
    /// Nguồn `SerSMSTemplateCreate`/`Update` (`BizCarSv.Master.cs:7982/8156`) đều nhận `strDealerCode` và
    /// `SerSMSTemplateGet` lọc theo `strDealerCodeConditionList` ⇒ **mỗi đại lý có bộ mẫu SMS RIÊNG**.
    /// Port cũ upsert theo `SmsType` trong một `OrgId` ⇒ đại lý A sửa mẫu là **ĐÈ mẫu của đại lý B**.
    /// </summary>
    public string? DealerCode { get; set; }

    /// <summary>⚠️ `SmsName` **KHÔNG có trong `Ser_SMSTemplate`** — phát minh của port cũ. Nguồn chỉ có
    /// bốn trường nghiệp vụ: `DealerCode` · `SMSType` · `SMSBody` · `IsActive`. Giữ để không vỡ client cũ.</summary>
    public string? SmsName { get; set; }

    /// <summary>⚠️ Nguồn cho `SMSBody` = **DBNull** khi rỗng (`Master.cs:8045`), KHÔNG chặn.</summary>
    public string SmsBody { get; set; } = "";

    /// <summary>
    /// 🔴 #299 ISACTIVE — ba điểm lệch với port cũ:
    ///  (1) Nguồn ghi **DBNull khi tham số rỗng**, không ép "1". Port cũ luôn ép `"1"` ⇒ **không tạo được
    ///      mẫu đang TẮT**, phải tạo rồi toggle.
    ///  (2) Nhãn của nguồn so **SỐ không nháy** (`when 0` / `when 1`), khác mọi chỗ khác trong hệ so `'1'` chuỗi.
    ///  (3) Có nhánh `else N'Không kích hoạt'` ⇒ **NULL hiển thị là "Không kích hoạt"**, không phải nhãn rỗng.
    /// ⚠️ Từ vựng nhãn theo MÀN (luật `C0-...` #286/#289): màn này là **"Kích hoạt/Không kích hoạt"**,
    ///    còn `Ser_Customer` (`Customer.cs:2213`) dùng **"Hoạt động/Không hoạt động"** cho CÙNG cột `IsActive`.
    /// </summary>
    public string FlagActive { get; set; } = "1";

    // 🔴 #299 `NewIsActive` là **HẰNG CHẾT** (cùng dạng `NewStatus` ở #298): nguồn viết
    //   `select tmp.* , case tmp.IsActive … end as NewIsActive` — nếu bảng có cột thật thì `tmp.*` đã trả
    //   rồi ⇒ trùng tên cột, DataTable vỡ. Vậy nó chỉ là nhãn tính lúc đọc. KHÔNG port thành cột.
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Vị trí kho phụ tùng (mã/tên/loại/diện tích/chiều cao/kho) — port 1:1 FrmImportLocation (TblSerMstLocation, TCMotor).</summary>
public sealed class PartLocation
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string LocationCode { get; set; } = "";
    public string? LocationName { get; set; }
    public string? LocationType { get; set; }
    public decimal LocationSurface { get; set; }
    public decimal LocationHeight { get; set; }
    public string? StockNo { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh mục dịch vụ/công (master: mã/tên/giá vốn/giá bán/model/VAT) — port 1:1 FrmService/FrmImportService (TblSerMSTService, TCMotor).</summary>
public sealed class ServiceItemMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SerCode { get; set; } = "";
    public string? SerName { get; set; }
    public decimal Cost { get; set; }
    public decimal Price { get; set; }
    public string? Model { get; set; }
    public decimal Vat { get; set; }
    public string? Note { get; set; }
    public string FlagActive { get; set; } = "1";   // ISACTIVE của nguồn

    // ===== 🔴 #297 parity `TblSerMSTService` (DbDefine.cs:748-762): 6 cột port cũ THIẾU =====
    // 🆕 Từ sweep "Tbl* có CẢ Status LẪN IsActive" (#295/#296).
    public string? DealerCode { get; set; }

    /// <summary>SERTYPEID — loại dịch vụ (`Ser_Mst_ServiceType`). Nguồn ghi **DBNull khi rỗng**,
    /// không ghi chuỗi rỗng.</summary>
    public string? SerTypeID { get; set; }

    /// <summary>🔴 STDMANHOUR — **giờ công ĐỊNH MỨC** của dịch vụ. Đây là nguồn định mức cho các bảng
    /// dùng lại (`RoServiceItem`/`ServiceQuotationLabor` đã có cột cùng tên); thiếu ở DANH MỤC nghĩa là
    /// không có chỗ nào khai định mức gốc.</summary>
    public decimal? StdManHour { get; set; }

    /// <summary>FACTOR — hệ số giá của dịch vụ.</summary>
    public decimal? Factor { get; set; }

    /// <summary>STATUS — trạng thái nghiệp vụ, **KHÁC `FlagActive`** (cờ bật/tắt bản ghi).</summary>
    public string? Status { get; set; }

    /// <summary>
    /// 🔴 FLAGWARRANTY — dịch vụ này là **CÔNG BẢO HÀNH của hãng**. Nguồn mặc định `Inactive` và CHỈ bật
    /// khi `SerCode` trùng một công bảo hành đang hiệu lực trong `Ser_MST_ROWarrantyWork` — xem luật ghi đè
    /// ở `POST /api/serviceitems`.
    /// </summary>
    public string? FlagWarranty { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh mục model xe dịch vụ (mã/tên/nhãn hiệu/mã SX) — port 1:1 FrmModel/FrmImportModel (TblModel, TCMotor).</summary>
public sealed class ServiceModel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public string? TradeMarkCode { get; set; }
    public string? ProductionCode { get; set; }
    public string? DealerCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phiếu xuất kho phụ tùng dịch vụ (header) — port 1:1 FrmSerInventoryAccStockOut01 (TblSerInvStockOut, TCMotor).</summary>
public sealed class ServiceStockOut
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockOutNo { get; set; } = "";
    public string? ReceiverCode { get; set; }
    public DateTime? StockOutDate { get; set; }
    public decimal TotalQty { get; set; }

    /// <summary>Tổng tiền phiếu xuất (cộng Amount các dòng).</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Loại phiếu xuất (Ser_Inv_StockOut.STOCKOUTTYPE). <b>"2" = PHIẾU XUẤT THƯỜNG</b> —
    /// CHỈ loại này mới được tính vào doanh thu bán ngoài của báo cáo tổng hợp.
    /// </summary>
    public string? StockOutType { get; set; }

    public string Status { get; set; } = "Draft"; // Draft -> Confirmed (trừ tồn)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng phiếu xuất kho phụ tùng (detail) — port 1:1 FrmSerInventoryAccStockOut01 grid, TCMotor.</summary>
public sealed class ServiceStockOutLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceStockOutId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal Quantity { get; set; }

    /// <summary>Đơn giá xuất (Ser_Inv_StockOutDetail.PRICE) — phiếu xuất thường là nghiệp vụ BÁN nên có tiền.</summary>
    public decimal Price { get; set; }

    /// <summary>Thuế suất theo PHẦN TRĂM (Ser_Inv_StockOutDetail.VAT), nguồn tính `VAT*0.01`.</summary>
    public decimal Vat { get; set; }

    /// <summary>Thành tiền dòng = Quantity × Price × (1 + VAT%) — đúng biểu thức doanh thu của nguồn.</summary>
    public decimal Amount { get; set; }
}

/// <summary>Phiếu nhập kho phụ tùng dịch vụ (header) — port 1:1 FrmSerInventoryAccStockIn (TblSerInvStockIn, TCMotor).</summary>
public sealed class ServiceStockIn
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockInNo { get; set; } = "";
    public string? SupplierCode { get; set; }

    /// <summary>Đại lý nhập — nguồn lọc báo cáo nhập theo cột này.</summary>
    public string? DealerCode { get; set; }

    public DateTime? StockInDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Draft"; // Draft -> Confirmed (cộng tồn)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng phiếu nhập kho phụ tùng (detail) — port 1:1 FrmSerInventoryAccStockIn grid, TCMotor.</summary>
public sealed class ServiceStockInLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceStockInId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }

    /// <summary>Thuế suất dòng nhập theo PHẦN TRĂM (Ser_Inv_PartInstance.SIVAT) — GỒM trong tiền tính giá vốn.</summary>
    public decimal Vat { get; set; }

    /// <summary>
    /// Vị trí THỰC TẾ nhập vào kho (Ser_Inv_StockInDetail.ACTUALLOCATIONID → Ser_Mst_Location.LOCATIONCODE).
    /// ⚠️ Là vị trí của TỪNG DÒNG NHẬP, khác vị trí mặc định khai trên master phụ tùng:
    /// cùng một mã phụ tùng có thể nằm ở nhiều ô kệ qua các lần nhập.
    /// </summary>
    public string? ActualLocationCode { get; set; }

    /// <summary>Thành tiền TRƯỚC thuế (nguồn: `Quantity * Price`, cột Total của báo cáo nhập).</summary>
    public decimal TotalBeforeVat { get; set; }

    /// <summary>RIÊNG phần thuế (nguồn: `VAT * Price * Quantity * 0.01`, cột VATAmount).</summary>
    public decimal VatAmount { get; set; }

    /// <summary>Thành tiền đã gồm thuế = TotalBeforeVat + VatAmount.</summary>
    public decimal Amount { get; set; }
}

/// <summary>Phụ tùng nợ/chờ giao theo xe (outstanding part order) — port 1:1 FrmNewSerPartOO/FrmMngSerPartOO (Ser_Part_OO, TCMotor).</summary>
public sealed class ServicePartOO
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OONo { get; set; } = "";
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? PlateNo { get; set; }
    public decimal QtyNeeded { get; set; }             // TblSer_Part_OO.SoLuongNo — SL nợ khách
    public decimal QtyFulfilled { get; set; }          // TblSer_Part_OO.SoLuongTra — SL đã trả
    public string? Note { get; set; }                  // TblSer_Part_OO.GhiChu
    public string Status { get; set; } = "Open"; // Open -> Fulfilled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // GAP đã vá 2026-09-05: 6 cột của TblSer_Part_OO bị bỏ sót ở bản port trước;
    // lưới FrmImportSerPartOO/FrmMngSerPartOO đều hiển thị các cột này.
    public string? LoaiXe { get; set; }                // TblSer_Part_OO.LoaiXe — loại/dòng xe
    public string? CVDV { get; set; }                  // TblSer_Part_OO.CVDV — cố vấn dịch vụ
    public string? DealerCode { get; set; }            // TblSer_Part_OO.DealerCode — đại lý
    public DateTime? NgayDatHang { get; set; }         // TblSer_Part_OO.NgayDatHang — ngày đặt hàng
    public DateTime? NgayVeDuKien { get; set; }        // TblSer_Part_OO.NgayVeDuKien — ngày về dự kiến
    public DateTime? NgayHenTra { get; set; }          // TblSer_Part_OO.NgayHenTra — ngày hẹn trả khách
}

/// <summary>Xe khách trong hệ thống dịch vụ (biển số/khung/máy/km/bảo hành) — port 1:1 FrmCarInfo (TblSerCar, TCMotor).</summary>
public sealed class ServiceCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string FrameNo { get; set; } = "";   // VIN — khóa
    public string? PlateNo { get; set; }
    public string? EngineNo { get; set; }
    public string? ModelCode { get; set; }
    public string? ColorCode { get; set; }
    public string? TradeMark { get; set; }
    public int? ProductYear { get; set; }
    public decimal CurrentKm { get; set; }
    public DateTime? WarrantyDate { get; set; }
    public DateTime? WarrantyRegistrationDate { get; set; }   // ngày kích hoạt/đăng ký bảo hành (HTC cập nhật)
    public string? CusName { get; set; }
    public string? CusMobile { get; set; }

    /// <summary>
    /// 🔴 Mã XE HỘI VIÊN (`Ser_Car.MemberCarID`) — liên kết xe dịch vụ với hội viên Loyalty.
    /// Nguồn: `DMS-Loyalty/DMS/TERP.BizDMS/Biz.zzzz.iNOS.CarSv.cs` (hàm `CarSv_SerCarUpdate_MemberCarID`) —
    /// hệ **CHỈ có trên máy 150**, laptop KHÔNG có `TERP.BizDMS`.
    /// ⚠️ **Không được trùng trong cùng đại lý**: nguồn chặn nếu mã này đã gán cho xe khác đang hoạt động.
    /// </summary>
    public string? MemberCarID { get; set; }

    /// <summary>Đại lý quản lý xe (`DealerCode`) — phần khoá tra cứu của nguồn và phạm vi chống trùng.</summary>
    public string? DealerCode { get; set; }

    /// <summary>Mã khách hàng (`CusID`) — nguồn tra xe theo BỘ BA `DealerCode` + `FrameNo` + `CusID`.</summary>
    public string? CusID { get; set; }

    // ===== #222 parity `CarUpdate` (DMSCarSv — TERP.HTCServiceClient/DbServices/MstCarService.cs:109) =====
    // Đối chiếu từng trường ở TẦNG SERVICE (luật `C0-trecentesimustricesimusquintus`): nguồn gửi **20 trường**;
    // entity có 12 khớp tên + 3 lệch tên ⇒ bổ sung 8 trường dưới đây.
    //
    // 🔴 BA TRƯỜNG LỆCH TÊN (đã có dữ liệu, KHÔNG đổi tên để tránh vỡ dữ liệu — ghi ánh xạ tại đây,
    //    theo luật `C0-trecentesimustricesimusquartus`):
    //      nguồn `TradeMarkCode` → entity `TradeMark`
    //      nguồn `ModelID`       → entity `ModelCode`
    //      nguồn `IsActive`      → entity `FlagActive`
    /// <summary>🔴 #332 PLATECOLORCODE — **màu biển số** (trắng / vàng / xanh). Ở VN màu biển phân loại
    /// xe **cá nhân · kinh doanh vận tải · công vụ** ⇒ đây là thuộc tính PHÁP LÝ, không phải màu sắc.
    /// Bản `CustomerCar` (phía bán hàng) đã có cột này từ lâu; `ServiceCar` (`Ser_Car`, phía dịch vụ)
    /// **thiếu** — chính là toàn bộ độ lệch giữa hai đường ghi LIVE của `CarSv_Ser_CustomerCar_Create`.</summary>
    public string? PlateColorCode { get; set; }

    // ===== 🔴 #333 SÁU CỘT của họ `ProcessSaveCar01` — **KHÔNG kênh nào ghi đủ cả sáu** =====
    // Ma trận kênh ⇄ cột (đo bằng tập cột ghi thật, xem chú thích ở `POST /api/servicecars`).
    /// <summary>SERIALNO — mã **đài AVN** (chú thích nguồn `20210508` "Cập nhật mã đài AVN").</summary>
    public string? SerialNo { get; set; }
    /// <summary>BATTERYNO — mã **bình ắc quy** (chú thích nguồn `20210508`).</summary>
    public string? BatteryNo { get; set; }
    /// <summary>PRODUCTIONCODE — mã lô sản xuất. CHỈ kênh `_New20180622` (Sales + MBS) ghi.</summary>
    public string? ProductionCode { get; set; }
    /// <summary>CUSCONFIRMEDWARRANTYDATE — ngày khách XÁC NHẬN bảo hành. CHỈ kênh `_SBHOnline`.
    /// ⚠️ Kênh đó ghi khi **TẠO**, nhưng ở nhánh **SỬA** ba cột bảo hành bị **comment cả khối**
    /// (`//20210408`) ⇒ tạo thì lưu, sửa thì **rơi im lặng**. Xem chú thích ở endpoint.</summary>
    public DateTime? CusConfirmedWarrantyDate { get; set; }
    /// <summary>WARRANTYEXPIRESDATE — ngày hết hạn bảo hành. Cùng nhóm bất đối xứng tạo/sửa ở trên.</summary>
    public DateTime? WarrantyExpiresDate { get; set; }
    /// <summary>WARRANTYKM — số km hết hạn bảo hành. Cùng nhóm bất đối xứng tạo/sửa ở trên.</summary>
    public decimal? WarrantyKM { get; set; }
    /// <summary>Mã xe nội bộ của hệ dịch vụ (`CarID`) — khác `FrameNo` (số khung).</summary>
    public string? CarID { get; set; }
    /// <summary>Mã xe bên hệ BÁN HÀNG (`SalesCarID`) — cầu nối sang cụm Car_Car.</summary>
    public string? SalesCarID { get; set; }
    /// <summary>Ngày mua xe (`DateBuyCar`) — nguồn lưu dạng chuỗi.</summary>
    public string? DateBuyCar { get; set; }
    /// <summary>Hãng bảo hiểm của xe (`InsNo`) — trỏ sang cụm SerInsurance.</summary>
    public string? InsNo { get; set; }
    /// <summary>Số hợp đồng bảo hiểm (`InsContractNo`).</summary>
    public string? InsContractNo { get; set; }
    /// <summary>Ngày bắt đầu hiệu lực bảo hiểm (`InsStartDate`).</summary>
    public string? InsStartDate { get; set; }
    /// <summary>Ngày kết thúc hiệu lực bảo hiểm (`InsFinishedDate`).</summary>
    public string? InsFinishedDate { get; set; }
    /// <summary>Ghi chú xe (`Note`).</summary>
    public string? Note { get; set; }

    /// <summary>
    /// 🔴 #269 `Ser_Car.CurrentServiceDate` — **lần vào xưởng GẦN NHẤT**. Là khoá của job NoShow:
    /// xe có ngày này rơi vào cửa sổ quá khứ ⇒ khách **quá hạn chưa quay lại**.
    /// Nguồn: `TERP.BizCarSv/HCCIntergration/BizCarSv.HCC.cs:381 HCC_NoShow_CreateOS` —
    /// hàm **CHỈ CÓ TRÊN MÁY 150**, laptop grep ra 0 dòng.
    /// </summary>
    public DateTime? CurrentServiceDate { get; set; }

    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh mục phụ tùng dịch vụ (master lõi) — port 1:1 FrmPart (TblSerMSTPart, TCMotor).</summary>
public sealed class ServicePart
{
    /// <summary>🔴 #400 §12 CUSDEBT — tên cột nói 'công nợ khách' nhưng màn **Tồn kho tối ưu**
    /// dùng nó làm **số lượng BO (hàng đặt bù) NHẬP TAY**: form khai
    /// `private const string colBO = "CUSDEBT"; //Nhap truc tiep` — và khai **hai lần** cùng một cột
    /// (`colBO` và `colCusDebt` đều = `"CUSDEBT"`).
    /// ⇒ Đọc tên cột mà hiểu là công nợ là **sai nghĩa**. Xem `POST /api/serviceparts/update-bo`.</summary>
    public decimal? CusDebt { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }    // VieName
    public string? EngName { get; set; }
    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public string? Location { get; set; }
    public decimal Quantity { get; set; }
    public decimal MinQuantity { get; set; }
    public string? PartGroupCode { get; set; }
    public string? Model { get; set; }
    public string? Note { get; set; }
    public string FlagActive { get; set; } = "1";   // IsActive
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== 🔴 #261: 12 cột nguồn `TblSerMSTPart` (DbDefine.cs:663-693) mà port cũ THIẾU =====
    // Tìm ra bằng sweep "lớp Tbl* có hằng nằm SAU DÒNG TRỐNG" (sinh từ bài học #260).

    // --- 5 cột thuộc KHỐI CHÍNH ---
    public string? PartID { get; set; }              // PARTID — khoá kỹ thuật, khác PartCode
    public string? PartTypeID { get; set; }          // PARTTYPEID (đã có PartGroupCode ` PARTGROUPID)
    public string? DealerCode { get; set; }          // DEALERCODE — phụ tùng theo đại lý
    public decimal? VAT { get; set; }
    /// <summary>🔴 #380 § **CỘT DẪN XUẤT, KHÔNG PHẢI CỘT LƯU** — nguồn KHÔNG hề ghi cột này ở đâu;
    /// mọi nơi đều TÍNH lúc đọc: `(isnull(sb.TotalInStock,0) + isnull(sb.TotalInShipment,0))`.
    /// ⚠️ Nguồn có **HAI công thức khác nhau** cho cùng tên cột:
    ///   · `Appointment.cs:1323/2299` và `Service.RO.cs:362` = **tồn kho + hàng đang về**
    ///   · `PartOrder.cs:4415`        = **CHỈ tồn kho** (không cộng hàng đang về)
    /// ⇒ Cùng một tên, hai nghĩa tuỳ màn. Giữ cột để tương thích nhưng **KHÔNG nhận từ client**
    /// (xem endpoint tạo/sửa phụ tùng) — trước lượt này client gửi số nào cũng thành 'tồn kho'.</summary>
    public decimal? InventoryQuantity { get; set; }  // INVENTORYQUANTITY — KHÁC Quantity

    // --- 7 cột thuộc KHỐI PHỤ (nằm sau dòng trống, :684-692) ---
    public decimal? TotalPrice { get; set; }
    public string? BalanceLocationId { get; set; }
    public decimal? FreqUsed { get; set; }           // FREQUSED — tần suất sử dụng
    public DateTime? PriceEffect { get; set; }       // PRICEEFFECT — mốc hiệu lực giá

    /// <summary>🔴 `TSTPrice` / `TSTPriceBefore` — giá NCC hiện tại và giá TRƯỚC ĐÓ.
    /// Cặp này cho biết giá vừa đổi; thiếu vế sau thì không đối chiếu được biến động giá.</summary>
    public decimal? TSTPrice { get; set; }
    public decimal? TSTPriceBefore { get; set; }

    /// <summary>FLAGINTST — phụ tùng có nằm trong danh mục TST hay không.</summary>
    public string? FlagInTST { get; set; }
}

/// <summary>Nhóm phụ tùng phân cấp (cha-con) — port 1:1 FrmPartGroup (TblSerMSTPartGroup, TCMotor).</summary>
public sealed class PartGroup
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupCode { get; set; } = "";
    public string? GroupName { get; set; }
    public string? ParentCode { get; set; }   // nhóm cha (self-ref theo GroupCode); rỗng = gốc
    public int OrderId { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Công nợ khách hàng dịch vụ (theo RO) — port 1:1 FrmCusDebitCreate (TblCusDebit, TCMotor).</summary>
public sealed class CusDebit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DebitNo { get; set; } = "";
    /// <summary>#557 §12 Đại lý của công nợ — nguồn **bắt buộc** ở `checkCusDebitFieldEmpty` và dùng
    /// làm **nửa khoá xoá** (`where DealerCode = @DealerCode and CusDebitID = @CusDebitID`).</summary>
    public string? DealerCode { get; set; }
    // ===== #555 §12 MỘT BẢNG, BA LOẠI CÔNG NỢ =====
    /// <summary>Loại công nợ — nguồn dùng **cùng bảng `Ser_CusDebit`** cho cả ba màn, phân biệt bằng
    /// hằng literal: **"1"** khách hàng · **"2"** bảo hiểm · **"3"** nhà cung cấp.</summary>
    public string DebitType { get; set; } = "1";
    /// <summary>Số đơn bảo hiểm — khoá lọc của công nợ loại **"2"**.</summary>
    public string? InsNo { get; set; }
    /// <summary>Mã nhà cung cấp — khoá lọc của công nợ loại **"3"**.</summary>
    public string? SupplierCode { get; set; }
    /// <summary>Khoá phiếu nhập kho — công nợ loại "3" nối `Ser_Inv_StockIn` qua cột này
    /// (loại "1"/"2" nối `Ser_RO` qua `ROID` — **hai khoá nối khác nhau trên cùng bảng**).</summary>
    public string? StockInID { get; set; }
    public string? CusId { get; set; }
    public string? CusName { get; set; }
    public string? RONo { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? DebitDate { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Paid
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thu tiền trên công nợ khách hàng — port 1:1 FrmCusPaymentCreate (TblPayment, TCMotor).</summary>
public sealed class CusDebitPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CusDebitId { get; set; }
    /// <summary>Số phiếu thu (`Ser_Payment.PaymentNo`) — nguồn sinh bằng `SerDebitGeneratePaymentNo`.
    /// Nhiều dòng thu cùng 1 lần nộp dùng CHUNG số này (1 phiếu phân bổ vào nhiều công nợ).</summary>
    public string? PaymentNo { get; set; }
    /// <summary>Mã đại lý (DealerCode) — nguồn BẮT BUỘC (checkPaymentFieldEmpty).</summary>
    public string? DealerCode { get; set; }
    /// <summary>Tên người nộp tiền (PayPersonName) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonName { get; set; }
    /// <summary>Số CMND/CCCD người nộp tiền (PayPersonIDCardNo) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonIDCardNo { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime? PayDate { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Lịch hẹn dịch vụ (đặt xe vào khoang/bay theo giờ) — port 1:1 FrmAppList + FrmShowCavityStatus (TblSerAppRO, TCMotor).</summary>
public sealed class ServiceAppointment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AppNo { get; set; } = "";
    public string? CavityName { get; set; }        // khoang/bay sửa chữa
    public string? PlateNo { get; set; }
    public string? CusName { get; set; }
    public string? Mobile { get; set; }
    public string? ModelName { get; set; }
    public string? AppType { get; set; }           // loại hẹn (BD/SC...)
    public DateTime AppFrom { get; set; }
    public DateTime AppTo { get; set; }
    public string Status { get; set; } = "Booked"; // Booked -> Arrived -> Done / Cancelled
    public string? Note { get; set; }
    public string? EngineerNo { get; set; }        // CVDV nhận lịch hẹn — port bổ sung FrmQuotationApp
    public string? QuoteNo { get; set; }           // Báo giá ước tính gắn theo lịch hẹn (FK mềm tới ServiceQuotation.QuoteNo)

    /// <summary>Yêu cầu của khách khi đặt lịch (Ser_App.CusRequest).</summary>
    public string? CusRequest { get; set; }

    // ===== 🔴 #270: khoá tra cứu + trục ĐẨY SANG HCC =====
    // Nguồn: `BizCarSv.Tab.cs:4780 Ser_App_Create_ForTab` gọi `HCC_Appointment_AddOSX`
    //   (`HCCIntergration/BizCarSv.HCC.cs:28`) NGAY SAU khi ghi `Ser_App`.
    // 🔴 Khối gọi này **CHỈ CÓ TRÊN MÁY 150** — bản laptop của cùng file KHÔNG có (diff toàn file: đúng
    //   một khối 19 dòng này là thay đổi thực chất duy nhất).
    public string? DealerCode { get; set; }        // Ser_App.DealerCode — nối sang Mst_Dealer lấy OrgHCCID
    public string? CusID { get; set; }             // Ser_App.CusID
    public string? Vin { get; set; }               // Ser_Car.FrameNo của xe được hẹn

    /// <summary>
    /// 🔴 #319 CARID — khoá kỹ thuật của XE, **KHÁC** <see cref="Vin"/> (số khung).
    /// Nguồn `Ser_App_Create_ForTab` ghi `dt_Ser_App.Rows[0]["CarID"]` — nhưng **CHỈ ở bản máy 150**;
    /// bản laptop `V20.2023.Release.V2` của cùng hàm KHÔNG ghi cột này (file lệch **+18 dòng**).
    /// ⚠️ Các truy vấn khác của nguồn ghép xe theo `CarID` (vd bảo hành: `td.CarID = car.CarID`, #302),
    ///    nên thiếu `CarID` thì lịch hẹn không nối được sang hồ sơ xe theo đúng khoá của hệ.
    /// </summary>
    public string? CarID { get; set; }

    // ===== 🔴 #323 NGUỒN TÁCH **NGÀY** VÀ **GIỜ** THÀNH HAI CỘT RIÊNG — hai cặp =====
    // Nguồn ghi qua helper dùng chung `Function_UtilsSerApp` (`ZTemp.cs:23228`, 16 cột):
    //   `AppDateTime`     = `Convert.ToDateTime(str).ToString("yyyy-MM-dd")`  ⇒ **CHỈ NGÀY**
    //   `AppTime`         = ghi **CHUỖI THÔ**, không convert, không kiểm định dạng
    //   `AppDateTimeFrom` / `AppTimeFrom` = cặp thứ hai, cùng quy tắc
    //
    // 🔴 **TÊN CỘT NÓI DỐI**: `AppDateTime` nghe như có cả giờ, thực tế **chỉ chứa NGÀY**.
    //    Giờ nằm ở `AppTime` dạng chuỗi tự do (nguồn không parse ⇒ có thể là "08:30", "8h30"…).
    // 🔴 Hai cột ghi **ĐỘC LẬP** (mỗi cột một guard `if (!IsEmpty(...))`) ⇒ nguồn cho phép
    //    **có ngày mà không có giờ**, hoặc ngược lại. Port cũ gộp thành `AppFrom`/`AppTo` kiểu `DateTime`
    //    ⇒ **không biểu diễn được** hai trạng thái đó, và ép chuỗi giờ tự do phải parse được.
    // ⇒ Giữ 4 cột THÔ đúng như nguồn; `AppFrom`/`AppTo` vẫn là tiện ích đã dùng, không bỏ.
    public string? AppDateTime { get; set; }        // CHỈ ngày, "yyyy-MM-dd"
    public string? AppTime { get; set; }            // giờ, CHUỖI THÔ
    public string? AppDateTimeFrom { get; set; }    // CHỈ ngày, "yyyy-MM-dd"
    public string? AppTimeFrom { get; set; }        // giờ, CHUỖI THÔ

    /// <summary>APPTYPECODE — mã loại hẹn của nguồn. ⚠️ MiniHTC đã có `AppType` (nhãn/loại do port đặt);
    /// giữ CẢ HAI để không mất mã gốc.</summary>
    public string? AppTypeCode { get; set; }

    /// <summary>CVDVCODE — mã cố vấn dịch vụ, nguồn `.Trim()` trước khi ghi.
    /// ⚠️ MiniHTC đã có `EngineerNo` (cùng vai trò, tên khác); giữ cả hai, không gộp.</summary>
    public string? CVDVCode { get; set; }

    /// <summary>
    /// Trạng thái đẩy lịch hẹn sang HCC — cùng bộ mã với trục HMC của đề nghị bảo hành:
    /// "P" chờ đẩy · "A" đẩy thành công · "R" đẩy lỗi. `null` = không thuộc diện đẩy.
    /// ⚠️ Nguồn CHỈ đẩy ở nhánh **`_ForTab`** (kênh máy tính bảng); nhánh tạo lịch hẹn thường KHÔNG đẩy.
    /// </summary>
    public string? HCCPushStatus { get; set; }
    public DateTime? HCCPushDateTime { get; set; }
    public string? HCCPushNote { get; set; }

    /// <summary>
    /// 🔴 #271 Trục ĐÓNG lịch hẹn ở HCC (`HCC_Appointment_FinishOSX`), TÁCH RIÊNG khỏi trục MỞ
    /// (<see cref="HCCPushStatus"/>) vì nguồn gọi ở **hai hàm khác nhau, hai thời điểm khác nhau**:
    /// mở lúc tạo lịch hẹn (`Ser_App_Create_ForTab`), đóng lúc TIẾP NHẬN XE
    /// (`Ser_ReceptionF_Reception_New20210727`). Dùng chung một cột sẽ mất dấu một trong hai.
    /// Cùng bộ mã "P"/"A"/"R".
    /// </summary>
    public string? HCCFinishStatus { get; set; }
    public DateTime? HCCFinishDateTime { get; set; }

    // ===== 🔴 #282 PARITY `TblSerAppRO` (DbDefine.cs:878-903, md5 `d373e758` — KHỚP 2 máy): 8 cột THẬT
    //   của bảng `Ser_App` mà port cũ thiếu. Đối chiếu **đủ 23 hằng** của lớp, không lấy theo lưới màn hình.

    /// <summary>CREATOR — người tạo lịch hẹn. Nguồn truyền riêng, KHÁC tài khoản đăng nhập.</summary>
    public string? Creator { get; set; }

    public string? CusAddress { get; set; }    // CUSADDRESS
    public string? CusTel { get; set; }        // CUSTEL — số bàn, KHÁC `Mobile` đã có
    public string? InsNo { get; set; }         // INSNO — số đơn bảo hiểm gắn theo lịch hẹn

    /// <summary>CAVITYID — **khoá** của khoang/bay. Port cũ chỉ có `CavityName` (nhãn hiển thị);
    /// nguồn ghi khoá này xuống DB.</summary>
    public string? CavityID { get; set; }

    /// <summary>
    /// 🔴 SOURCE — **NGUỒN TẠO** lịch hẹn, là cột THẬT trong DB (`TblSerAppRO.Source`).
    /// ⚠️ Tham số `Channel` mà port thêm ở #270 (để quyết định có đẩy HCC hay không) là **do port tự đặt**,
    /// không có trong nguồn; nay `Channel` được ghi xuống chính cột `Source` này khi client không gửi
    /// `Source` riêng — để dữ liệu port khớp cột nguồn thay vì sinh khái niệm mới.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>🔴 FIRSTCONTACTDATETIME / LASTCONTACTDATETIME — mốc liên hệ **LẦN ĐẦU** và **GẦN NHẤT**
    /// với khách của lịch hẹn. Đây là phần nghiệp vụ TỔNG ĐÀI: một lịch hẹn có thể phải gọi nhiều lần,
    /// giữ cả hai mốc mới đo được "bao lâu mới liên hệ được lần đầu".</summary>
    public DateTime? FirstContactDateTime { get; set; }
    public DateTime? LastContactDateTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Hạng mục dịch vụ khách đặt trước trong một lịch hẹn (Ser_AppServiceItems —
/// port 1:1 FrmAppointment*, TCMotor DMSCarSv/Appointment).
/// MỘT lịch hẹn đặt NHIỀU dịch vụ — đây là nội dung chính của lịch hẹn, không phải phần phụ.
/// </summary>
public sealed class AppointmentServiceItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AppNo { get; set; } = "";
    public string SerCode { get; set; } = "";
    public string? SerName { get; set; }

    /// <summary>Giờ công định mức của dịch vụ (Ser_Mst_Service.StdManHour) — nguồn trả kèm để ước tính thời gian.</summary>
    public decimal? StdManHour { get; set; }

    public string? Note { get; set; }
}

/// <summary>
/// Phụ tùng khách đặt trước trong một lịch hẹn (Ser_AppPartItems —
/// port 1:1 FrmAppointment*, TCMotor DMSCarSv/Appointment).
/// </summary>
public sealed class AppointmentPartItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AppNo { get; set; } = "";
    public string PartCode { get; set; } = "";

    /// <summary>Tên phụ tùng tiếng Việt (Ser_Mst_Part.VieName).</summary>
    public string? PartName { get; set; }

    /// <summary>Tên phụ tùng tiếng Anh (Ser_Mst_Part.EngName) — nguồn trả cả hai.</summary>
    public string? EngName { get; set; }

    public string? Unit { get; set; }
    public decimal Quantity { get; set; }
    public string? Note { get; set; }
}

/// <summary>Công nợ bảo hiểm (hãng BH nợ tiền bồi thường theo RO) — port 1:1 FrmInsDebitSearch (TblCusDebit type InsuranceDebit, TCMotor).</summary>
public sealed class InsDebit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DebitNo { get; set; } = "";
    public string? InsNo { get; set; }
    public string? InsName { get; set; }
    public string? RONo { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? DebitDate { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Paid
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thu tiền trên công nợ bảo hiểm — port 1:1 FrmInsPaymentCreate (TblPayment, TCMotor).</summary>
public sealed class InsDebitPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long InsDebitId { get; set; }
    /// <summary>Số phiếu thu (`Ser_Payment.PaymentNo`) — nguồn sinh bằng `SerDebitGeneratePaymentNo`.
    /// Nhiều dòng thu cùng 1 lần nộp dùng CHUNG số này (1 phiếu phân bổ vào nhiều công nợ).</summary>
    public string? PaymentNo { get; set; }
    /// <summary>Mã đại lý (DealerCode) — nguồn BẮT BUỘC (checkPaymentFieldEmpty).</summary>
    public string? DealerCode { get; set; }
    /// <summary>Tên người nộp tiền (PayPersonName) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonName { get; set; }
    /// <summary>Số CMND/CCCD người nộp tiền (PayPersonIDCardNo) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonIDCardNo { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime? PayDate { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Công nợ nhà cung cấp phụ tùng (HTC nợ NCC theo phiếu nhập kho) — port 1:1 FrmSuplierDebitCreate/FrmSupplierDebitSearch
/// (TblCusDebit type SupplierDebit, TCMotor DMSCarSv/Debit). Cộng dồn theo (SupplierCode, StockInNo).</summary>
public sealed class SupplierDebit
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SupplierCode { get; set; } = "";
    public string? StockInNo { get; set; }
    public decimal DebitAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime? DebitDate { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Paid
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thanh toán trên công nợ nhà cung cấp — port 1:1 FrmSupplierPaymentCreate (TblPayment, TCMotor DMSCarSv/Debit).</summary>
public sealed class SupplierDebitPayment
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SupplierDebitId { get; set; }
    /// <summary>Số phiếu thu (`Ser_Payment.PaymentNo`) — nguồn sinh bằng `SerDebitGeneratePaymentNo`.
    /// Nhiều dòng thu cùng 1 lần nộp dùng CHUNG số này (1 phiếu phân bổ vào nhiều công nợ).</summary>
    public string? PaymentNo { get; set; }
    /// <summary>Mã đại lý (DealerCode) — nguồn BẮT BUỘC (checkPaymentFieldEmpty).</summary>
    public string? DealerCode { get; set; }
    /// <summary>Tên người nộp tiền (PayPersonName) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonName { get; set; }
    /// <summary>Số CMND/CCCD người nộp tiền (PayPersonIDCardNo) — nguồn BẮT BUỘC.</summary>
    public string? PayPersonIDCardNo { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime? PayDate { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 #272 NHẬT KÝ MỘT LƯỢT ĐẨY "khách quá hạn chưa quay lại" (NoShow) SANG HCC.
/// Nguồn: `HCCIntergration/BizCarSv.HCC.cs:485 HCC_NoShow_CreateOSX` (**chỉ có trên máy 150**) —
/// mỗi ĐẠI LÝ trong vòng lặp là **một lượt đẩy riêng**, nên nhật ký cũng theo đại lý + loại nhắc.
/// Cặp với danh sách ứng viên ở `GET /api/hcc/noshow` (#269).
/// </summary>
public sealed class HccNoShowPush
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    /// <summary>"6Month" hoặc "12Month" — nguồn chỉ sinh hai giá trị này (#269).</summary>
    public string NoShowType { get; set; } = "";
    public string DealerCode { get; set; } = "";

    /// <summary>Cửa sổ đã dùng để chọn ứng viên — lưu lại để đối soát về sau,
    /// vì cửa sổ trượt theo ngày chạy job.</summary>
    public DateTime WindowFrom { get; set; }
    public DateTime WindowTo { get; set; }

    /// <summary>Số ứng viên trong lượt. 🔴 Nguồn KHÔNG gọi HCC khi danh sách rỗng: guard
    /// `if (!IsNullOrEmpty(strOrgID))` mà `strOrgID` chỉ được gán BÊN TRONG vòng lặp dòng ⇒ danh sách
    /// rỗng thì nó ở lại `null`. Một guard "có dòng nào không" NGUỴ TRANG thành guard "có OrgID không".</summary>
    public int CandidateCount { get; set; }

    /// <summary>"P" chờ đẩy · "A" đẩy xong · "R" lỗi — cùng bộ mã với các trục HCC/HMC khác.</summary>
    public string PushStatus { get; set; } = "P";
    public DateTime? PushDateTime { get; set; }
    public string? PushNote { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 #287 ĐƠN ĐẶT PHỤ TÙNG GỬI NHÀ CUNG CẤP — bảng `Ser_Part_Order`.
/// ⚠️ **KHÁC HẲN** `Ser_Order_Part` (đơn đặt phụ tùng TST, đã port thành `OrderPart` ở #234): hai bảng
/// tên **đảo chữ** của nhau, khác bộ mã trạng thái, khác nghiệp vụ. Đừng gộp.
/// Nguồn cột: `BizCarSv.PartOrder.cs:756 Ser_Part_OrderCreate` (18 trường header).
/// </summary>
public sealed class SupplierPartOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }

    public string OrderNo { get; set; } = "";
    /// <summary>ORDERNOUSER — số đơn do NGƯỜI DÙNG đặt, khác số hệ thống sinh.</summary>
    public string? OrderNoUser { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? DealerCode { get; set; }

    /// <summary>
    /// 🔴 STATUS — **BỘ MÃ TRỘN**: cột này chứa CẢ mã SỐ lẫn mã CHỮ. Nguồn
    /// (`Ser_Part_OrderGet_StatusList`, `PartOrder.cs:2614-2623`) ánh xạ:
    ///   `'1'` Mới tạo · `'CONF'` Xác nhận · `'2'` Hàng đang về · `'3'` Hoàn thành
    /// ⚠️ Chỉ **một** mã chữ (`CONF`) xen giữa ba mã số — dấu vết một đợt đổi sang mã chữ làm DỞ DANG.
    /// Bản CHẾT `..._StatusList01` có bộ chữ đầy đủ (`CREA/CONF/REJ/FINS/CANC`) nhưng WS **không gọi**.
    /// ⚠️ Nguồn **KHÔNG có nhánh ELSE** ⇒ mã ngoài bốn giá trị trên cho ra **NULL**, không phải chuỗi rỗng.
    /// </summary>
    public string? Status { get; set; }

    public DateTime? ReceivePartDate { get; set; }
    public DateTime? SendDate { get; set; }
    public string? SupplierID { get; set; }
    public string? UserCreate { get; set; }
    public string? UserApproved { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? TypeOrder { get; set; }
    public string? HTCConfirm { get; set; }
    /// <summary>PARTIALSHIPMENT — cho phép giao HÀNG TỪNG PHẦN hay không.</summary>
    public string? PartialShipment { get; set; }
    public string? TypeTransport { get; set; }
    public string? VIN { get; set; }
    public string? ConfirmNo { get; set; }
    /// <summary>CUSCHARGES — đơn vị chịu phí (nguồn ghi chú "Đơn vị chịu phí Issue").</summary>
    public string? CusCharges { get; set; }

    /// <summary>
    /// 🔴 #298 ISACTIVE — **cờ XOÁ MỀM**, port #287 THIẾU HẲN.
    /// `Ser_Part_OrderCreate` ghi thẳng `= Constants.Flag.Active` (`PartOrder.cs:944`), và hàm LIVE
    /// `Ser_Part_OrderGet` lọc `and si.IsActive = '1'` ở **BA** chỗ (temp-table lọc, `#tblTemp`, và bản
    /// `_StatusList`). Thiếu cột này ⇒ danh sách trả về **CẢ đơn đã xoá**.
    /// ⚠️ `IsActive` **KHÁC** `Status`: `Status` là bước nghiệp vụ (1/CONF/2/3), `IsActive` là còn/đã xoá.
    /// Nguồn giữ CẢ HAI trên cùng bảng (luật `C0-quingentesimusquartus`).
    /// </summary>
    public string FlagActive { get; set; } = "1";

    // 🔴 #298 `TblSer_Part_Order.NewStatus` là **HẰNG CHẾT** — KHÔNG port thành cột.
    //   Chứng minh bằng cấu trúc, không phải phỏng đoán: mọi câu SELECT sinh ra nó đều viết
    //   `select si.* ... case ... end as NewStatus`. Nếu bảng THẬT có cột `NewStatus` thì `si.*` đã trả
    //   nó rồi ⇒ alias trùng tên, DataTable dựng lên sẽ vỡ. Vậy nó **chỉ là nhãn tính lúc đọc**.
    //   ⚠️ Và nhãn đó **KHÔNG THỐNG NHẤT** — ba bảng mã khác nhau cho CÙNG cột `Status`:
    //     (a) sau khi TẠO (`PartOrder.cs:958`):  1=Mới tạo · 2=**Đã gửi** · 3=**Đã duyệt**
    //     (b) `_StatusList01` (CHẾT, :2286):     CREA/CONF/REJ/FINS/CANC
    //     (c) LIVE `_StatusList` (:2612):        1=Mới tạo · CONF=Xác nhận · 2=**Hàng đang về** · 3=**Hoàn thành**
    //   (a) và (c) **mâu thuẫn**: cùng mã '2'/'3' mà nghĩa khác hẳn. #287 đã lấy (c) — giữ nguyên vì đó là
    //   bảng LIVE của màn danh sách; ghi lại (a) để ai đọc log sau khi TẠO không tưởng là port sai.

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>#287 Dòng của đơn đặt phụ tùng NCC (`Ser_Part_OrderDetail`).</summary>
public sealed class SupplierPartOrderLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long SupplierPartOrderId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    /// <summary>SL ĐẶT.</summary>
    public decimal Quantity { get; set; }
    /// <summary>🔴 SL ĐÃ GIAO — cùng với <see cref="Quantity"/> **SINH RA** trạng thái giao hàng,
    /// xem chú thích ở endpoint (trạng thái đó KHÔNG lưu thành cột).</summary>
    public decimal DeliveryQuantity { get; set; }
    /// <summary>⚠️ `Price`/`Amount` **KHÔNG có trong `Ser_Part_OrderDetail`** — là phát minh của port cũ.
    /// Tiền thật của nguồn tính từ `Cost` + `VAT` (xem dưới). Giữ hai cột này để không vỡ client cũ.</summary>
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }

    // ===== 🔴 #298 CỘT THẬT CỦA `Ser_Part_OrderDetail` MÀ PORT #287 THIẾU =====
    // ⚠️ Nguồn cột KHÔNG lấy được từ `TblSer_Part_OrderDetail` (DbDefine): lớp hằng đó **THIẾU 6 tên**
    //   (`MIP`/`OO`/`BO`/`OH`/`SOQ`/`ICC`) mà `Ser_Part_OrderDetailCreate` (`PartOrder.cs:664-720`) ghi thật.
    //   ⇒ **DbDefine KHÔNG phải danh sách cột đầy đủ** — phải đối chiếu hàm Create, y như bài học POCO ở #236.

    /// <summary>PARTID — khoá kỹ thuật của phụ tùng; nguồn join `Ser_Mst_Part` **theo PartID**,
    /// không theo `PartCode` (PartCode chỉ là cột enrich `p.PartCode`).</summary>
    public string? PartID { get; set; }

    public decimal? Factor { get; set; }        // hệ số
    public decimal? Cost { get; set; }          // 🔴 ĐƠN GIÁ THẬT dùng để tính tiền (KHÔNG phải Price)
    public decimal? VAT { get; set; }           // % VAT

    /// <summary>🔴 DISCOUNT — nguồn **ghi cột này nhưng KHÔNG dùng nó ở bất kỳ công thức tiền nào**:
    /// `BeforeTax = Cost*Quantity` · `AfterTax = Cost*Quantity*(100+VAT)/100` · `Amount = Sum(AfterTax)`.
    /// Chiết khấu **không được trừ**. Đây là hành vi của nguồn, không phải thiếu sót của port.</summary>
    public decimal? Discount { get; set; }

    public string? Model { get; set; }
    public string? HTCConfirm { get; set; }
    public DateTime? LastDateDelivery { get; set; }   // lần giao gần nhất

    // --- 6 mã KẾ HOẠCH PHỤ TÙNG, chỉ có trong hàm Create (DbDefine không khai) ---
    public decimal? MIP { get; set; }
    public decimal? OO { get; set; }
    public decimal? BO { get; set; }
    public decimal? OH { get; set; }
    public decimal? SOQ { get; set; }
    public decimal? ICC { get; set; }

    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    // 🔴 #298 HAI HẰNG CHẾT nữa trong `TblSer_Part_OrderDetail` — KHÔNG port thành cột:
    //   `PendingDeliveryQty` : nguồn TÍNH lúc đọc (:2044) — `DeliveryQuantity is null` ⇒ lấy trọn `Quantity`;

    //                          `Quantity-DeliveryQuantity <= 0` ⇒ 0; còn lại ⇒ hiệu. (Kẹp sàn 0, không âm.)
    //   `OrderQuantity`      : mọi chỗ đọc đều viết `0 OrderQuantity` (hằng số 0, :3433) ⇒ chưa từng dùng.
}

/// <summary>
/// 🔴 #290 CẤU HÌNH GỬI EMAIL TỰ ĐỘNG — `Email_ConfigSendAuto` (`BizCarSv.SendMail.cs:1124`).
/// Năm `[WebMethod]` sống: Create · Update · Delete · Get · Cancel (`WSCarSv.asmx.cs:21500-21827`).
/// Cột lấy từ **chữ ký `Email_ConfigSendAuto_Create`** (11 trường nghiệp vụ), không lấy theo lưới.
/// </summary>
/// <summary>🔴 #433 CẤU HÌNH MÁY CHỦ THƯ (`Email_Config`) — **KHÁC** <see cref="EmailConfigSendAuto"/>
/// (cái kia là lịch gửi tự động). Đây là thông số SMTP: địa chỉ, cổng, tài khoản, mật khẩu, SSL, thời gian chờ.</summary>
public sealed class EmailServerConfig
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>`IDCONFIG` — khoá do nguồn sinh khi tạo.</summary>
    public string IdConfig { get; set; } = "";
    /// <summary>⚠️ Nguồn ĐỌC có lọc đại lý (`Email_Config_Get(SystemGlobal.strDealerCode, …)`, thêm 2012)
    /// nhưng khi GHI lại truyền **chuỗi rỗng** ⇒ bản ghi tạo ra không mang mã đại lý.</summary>
    public string? DealerCode { get; set; }
    public string? MailServerAddress { get; set; }
    public string? MailServerUser { get; set; }
    /// <summary>🔴 Nguồn lưu **NGUYÊN VĂN**, không băm không mã hoá.</summary>
    public string? MailServerPassword { get; set; }
    public string? Port { get; set; }
    public string? TimeOut { get; set; }
    /// <summary>⚠️ Nguồn lưu chuỗi `"True"`/`"False"` (từ `Convert.ToString(chk.Checked)`), **không** phải
    /// `"1"`/`"0"` như quy ước cờ của hệ; lúc đọc lại so `== "False"`.</summary>
    public string? EnableSSL { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public sealed class EmailConfigSendAuto
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string? DealerCode { get; set; }

    /// <summary>AUTOTIME — GIỜ gửi trong ngày. Nguồn hiển thị `right(AutoTime, 11)` ⇒ cột lưu chuỗi dài hơn
    /// phần hiển thị; port giữ nguyên chuỗi, KHÔNG tự cắt.</summary>
    public string? AutoTime { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }

    /// <summary>SENDMODE — `1` Gửi một lần · `2` Gửi hàng ngày · `3` Gửi hàng tuần.</summary>
    public string? SendMode { get; set; }

    /// <summary>ISACTIVE — `0` Không kích hoạt · `1` Kích hoạt.</summary>
    public string? IsActive { get; set; }

    /// <summary>
    /// TYPEEMAIL — loại email tự động: `1`..`7` (xem bảng nhãn ở endpoint).
    /// ⚠️ Màn LỊCH SỬ GỬI còn có mã `0` với **nhãn RỖNG** (`then N''`) và gọi mã `3` là "Chúc mừng SN"
    /// thay vì "Mừng sinh nhật" — hai bảng nhãn khác nhau cho cùng cột (luật nhãn-theo-màn #286).
    /// </summary>
    public string? TypeEmail { get; set; }

    public DateTime? ConfigDate { get; set; }
    /// <summary>AUTODATE / AUTODAY — NGÀY trong tháng và THỨ trong tuần để chạy; đi kèm `SendMode`.</summary>
    public string? AutoDate { get; set; }
    public string? AutoDay { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Chia sẻ phụ tùng giữa đại lý (đại lý đăng PT tồn sẵn để chia sẻ) — port 1:1 FrmSharePart (TblSPSharePart, TCMotor).</summary>
public sealed class SharePart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ShareNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public decimal InStock { get; set; }        // INSTOCKQUANTITY: tồn hiện tại
    public decimal QuantityShare { get; set; }  // SL sẵn sàng chia sẻ (đã KẸP — xem #267)
    public string? Remark { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Closed
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ===== 🔴 #267: cột/luật nguồn `SP_SharePart` + `SP_SharePart_Detail` mà port cũ THIẾU =====
    // Nguồn: `BizCarSv.PartOrder.cs:4702 SP_SharePartCreate` (md5 `9483ca4a` — KHỚP 2 máy)
    //      + `Views/PartOrder/FrmSharePart.cs:222 btnShare_Click` (md5 `0b8022ef` — KHỚP 2 máy).

    /// <summary>MINQUANTITY — **tồn tối thiểu** của phụ tùng, CHỐT lúc đăng chia sẻ.
    /// Đây là chân kia của công thức trần chia sẻ: `SoLuongDcChiaSe = InStock − MinQuantity`.</summary>
    public decimal MinQuantity { get; set; }

    /// <summary>🔴 SL đại lý **YÊU CẦU** trước khi bị kẹp. Nguồn ghi đè thẳng `QuantityShare` bằng
    /// `SoLuongChiaSeThucTe` (`UPDATE … SET QuantityShare = t.SoLuongChiaSeThucTe`) nên **mất dấu số gốc**;
    /// port giữ lại số gốc để đối soát được vì sao SL lưu khác SL gửi.</summary>
    public decimal QuantityShareRequested { get; set; }

    /// <summary>🔴 FLAGLATEST — nguồn ghi `Flag.Active` khi tạo. Cột này **KHÔNG có trong lớp hằng**
    /// `TblSPSharePart` (DbDefine.cs:283-292) — chỉ lộ ra ở câu INSERT. Lại một bằng chứng: lớp `Tbl*`
    /// KHÔNG phải danh sách cột đầy đủ, câu ghi mới là nguồn sự thật.</summary>
    public string FlagLatest { get; set; } = "1";

    public string? Note { get; set; }              // NOTE — ghi chú ở MASTER (TblSPSharePart.Note)
    public string? CreatedBy { get; set; }         // CREATEDBY
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Thông báo kỹ thuật (bulletin) — số/nội dung/PT-DV liên quan/hết hạn/file — port 1:1 FrmBulletinHTCCreate (Tbl_Blt_Bulletin, TCMotor).</summary>
public sealed class Bulletin
{
    /// <summary>🔴 #377 §12 FILEATTACHMENT — **nội dung/tên tệp đính kèm**, KHÁC
    /// <see cref="FileNameAttachment"/> (chỉ là tên hiển thị). Nguồn trả cột này bằng một truy vấn
    /// con **đóng cứng số thông báo `'TEST201911'`** ⇒ mọi dòng nhận tệp của **một bản ghi TEST**.
    /// Xem `/api/bulletins/by-vin`.</summary>
    public string? FileAttachment { get; set; }
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BulletinNo { get; set; } = "";

    /// <summary>Số bản tin của hãng HMC (BULLETINNOHMC) — nguồn BẮT BUỘC nhập.</summary>
    public string? BulletinNoHMC { get; set; }

    public string? Remark { get; set; }

    /// <summary>⚠️ Dịch vụ/phụ tùng liên quan thực chất nằm ở <see cref="BulletinDtl"/> (1-n).
    /// Bốn cột này giữ cho dữ liệu cũ, không còn là nguồn sự thật.</summary>
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? SerCode { get; set; }
    public string? SerName { get; set; }

    public DateTime? DateExpired { get; set; }
    public string? FileNameAttachment { get; set; }
    public string FlagActive { get; set; } = "1";

    /// <summary>Ngày phát hành bản tin (CREATEDATE) — khác CreatedAt là mốc ghi bản ghi.</summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>Người phát hành bản tin (USERCREATE).</summary>
    public string? UserCreate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Dịch vụ / phụ tùng liên quan tới một bản tin kỹ thuật (Btl_BulletinDtl —
/// port 1:1 FrmBulletinHTCCreate/Modify, TCMotor DMSCarSv/Bulletin).
/// MỘT bản tin gắn NHIỀU cặp dịch vụ + phụ tùng; nguồn ghi từng dòng vào bảng riêng.
/// </summary>
public sealed class BulletinDtl
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BulletinNo { get; set; } = "";
    public string? SerCode { get; set; }
    public string? SerName { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
}

/// <summary>
/// VIN áp dụng của một bản tin kỹ thuật (Btl_Bulletin_VIN —
/// port 1:1 FrmBulletinHTCCreate/Modify + FrmBulletinDealerSearch, TCMotor DMSCarSv/Bulletin).
/// MỘT bản tin áp cho NHIỀU xe, và mỗi xe có trạng thái xử lý RIÊNG — nhờ đó đại lý
/// theo dõi được xe nào đã làm, xe nào chưa.
/// </summary>
public sealed class BulletinVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BulletinNo { get; set; } = "";

    /// <summary>Số khung xe (VINNO).</summary>
    public string VinNo { get; set; } = "";

    /// <summary>Đại lý phụ trách xe này (DEALERCODE).</summary>
    public string? DealerCode { get; set; }

    /// <summary>Trạng thái xử lý RIÊNG của xe này. Nguồn đọc `isnull(bv.Status,'P')` ⇒ mặc định "P" (chờ xử lý).</summary>
    public string Status { get; set; } = "P";
}

/// <summary>Báo giá phụ tùng dịch vụ (header) — port 1:1 FrmPartQuotation (TblSerInvQuote, TCMotor).</summary>
public sealed class PartQuote
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string QuoteNo { get; set; } = "";
    public string? CusId { get; set; }
    public string? CusName { get; set; }
    public string? Mobile { get; set; }
    public string? ReceiveName { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Remark { get; set; }

    /// <summary>Tổng tiền tính từ các dòng — CÓ nhân hệ số giảm giá của từng dòng (∑ PartQuoteLine.Amount).</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng tiền theo đúng cột `SumAmount` của nguồn:
    /// <c>sum(Quantity*Price + Quantity*Price*0.01*VAT)</c> — ⚠️ <b>KHÔNG</b> nhân <c>Factor</c>.
    /// 🔴 Nguồn BẤT ĐỐI XỨNG: từng DÒNG tính CÓ hệ số (`Amount`), còn TỔNG ở header tính KHÔNG hệ số
    /// (<c>BizCarSv.Inventory.Quote.cs</c> dòng 1586 vs 1777) ⇒ khi có dòng giảm giá thì
    /// <see cref="SumAmountNoFactor"/> ≠ <see cref="TotalAmount"/>. Giữ CẢ HAI để đối soát,
    /// KHÔNG tự "sửa cho khớp" vì đó là hành vi thật của hệ nguồn.
    /// </summary>
    public decimal SumAmountNoFactor { get; set; }
    public string Status { get; set; } = "Draft";   // Draft -> Sent -> Approved / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng báo giá phụ tùng (detail) — port 1:1 FrmPartQuotation grid, TCMotor.</summary>
public sealed class PartQuoteLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long PartQuoteId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public string? Unit { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    /// <summary>Thuế suất theo PHẦN TRĂM (nguồn tính `0.01*VAT`), vd 10 = 10%.</summary>
    public decimal Vat { get; set; }

    /// <summary>
    /// Hệ số giảm giá của dòng (Ser_Inv_QuotePartItems.Factor — "Hệ số" trên lưới, Issue 813).
    /// Mặc định 1 (không giảm). Nhân vào CẢ phần gốc LẪN phần thuế: nguồn tính
    /// `Qty*Price*Factor + Qty*Price*0.01*VAT*Factor`.
    /// </summary>
    public decimal Factor { get; set; } = 1m;

    /// <summary>Bảng giá đã áp cho dòng này (PartPriceId) — để truy vết giá lấy từ đâu.</summary>
    public string? PartPriceId { get; set; }

    public string? Note { get; set; }

    /// <summary>
    /// Thành tiền TRƯỚC thuế, đã nhân hệ số.
    /// 🔴 #313 PHÂN LOẠI: nguồn **KHÔNG lưu** hai cột tiền này. `Ser_Inv_QuotePartItems` chỉ có
    /// `Quantity`/`Price`/`Factor`/`VAT`; `Amount` và `AmountBeforeVAT` được **TÍNH lúc ĐỌC**
    /// (`BizCarSv.Inventory.Quote.cs:2070-2071`, Issue 813):
    ///   `Amount          = Qty*Price*Factor + Qty*Price*0.01*VAT*Factor`
    ///   `AmountBeforeVAT = Qty*Price*Factor`
    /// Đã kiểm: **không có** chỗ nào ghi hai tên này (`Rows[0][…]` / `strFN` / `alColumnEffective`).
    ///
    /// MiniHTC tính lúc GHI rồi LƯU lại. Khác #312 ở chỗ **client không gửi được** (DTO không nhận),
    /// nên không có lỗ hổng "gửi gì cũng thành tiền". Rủi ro còn lại là **LỆCH PHA**: nếu sau này có
    /// đường sửa `Quantity`/`UnitPrice`/`Factor`/`Vat` mà quên tính lại thì hai cột này ôi.
    /// ⇒ Endpoint đọc nay **TÍNH LẠI** như nguồn và trả kèm giá trị lưu để đối chiếu.
    /// </summary>
    public decimal AmountBeforeVat { get; set; }

    public decimal Amount { get; set; }
}

/// <summary>Hợp đồng bảo hiểm dịch vụ (NĐ bảo hiểm/hạn mức/hiệu lực) — port 1:1 FrmInsuranceContractCreate (Tbl_Ser_InsuranceContract, TCMotor).</summary>
public sealed class InsContract
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InContractNo { get; set; } = "";
    public string? InContractCode { get; set; }
    public string? InsNo { get; set; }       // mã nhà bảo hiểm
    public string? InsName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
    public decimal PaymentLimit { get; set; }
    public string? TypePayment { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Tham số cấu hình dịch vụ theo đại lý (ParamCode→ParamValue) — port 1:1 FrmDealerServiceOptional (Ser_Param, TCMotor).</summary>
public sealed class DealerServiceOption
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ParamCode { get; set; } = "";
    public string ParamValue { get; set; } = "";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Loại khách hàng dịch vụ (hệ số giá, cá nhân/tổ chức) — port 1:1 FrmCusTypeCreate (Ser_CusType, TCMotor).</summary>
public sealed class CustomerType
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CusTypeCode { get; set; } = "";
    public string? CusTypeName { get; set; }
    public decimal CusFactor { get; set; }        // hệ số giá dịch vụ
    public string CusPersonType { get; set; } = "Personal"; // Personal | Organization
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhóm khách hàng dịch vụ (header) — port 1:1 FrmCustomerGroupCreate (Tbl_SerCustomerGroup, TCMotor).</summary>
public sealed class CustomerGroup
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GroupNo { get; set; } = "";
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Khách hàng thành viên nhóm (detail) — port 1:1 FrmCustomerGroupCreate grid, TCMotor.</summary>
public sealed class CustomerGroupMember
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long CustomerGroupId { get; set; }
    public string CusId { get; set; } = "";
    public string? CusName { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
}

/// <summary>Khoang sửa chữa (bay) — mã/tên/loại khoang/giờ làm việc — port 1:1 FrmCavityCreate (Ser_Cavity, TCMotor).</summary>
public sealed class Cavity
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CavityNo { get; set; } = "";
    public string? CavityName { get; set; }
    public string? CompartmentType { get; set; }   // loại khoang (Tbl_Mst_Compartment)
    public string? StartWorkTime { get; set; }      // giờ bắt đầu ca
    public string? FinishWorkTime { get; set; }     // giờ kết thúc ca
    public string? Note { get; set; }
    public string FlagActive { get; set; } = "1";   // ISACTIVE của nguồn (đặt tên theo lệ port)

    // ===== 🔴 #296 parity `TblSerCavity` (DbDefine.cs:1621-1632): 5 cột port cũ THIẾU =====
    // 🆕 Tìm qua sweep "lớp Tbl* có CẢ `Status` LẪN `IsActive`" (sinh từ #295) — 6 lớp, đây là một.
    public string? DealerCode { get; set; }

    /// <summary>CAVITYTYPE — loại khoang theo nguồn.
    /// ⚠️ Port cũ có `CompartmentType` (từ `Tbl_Mst_Compartment`) — **KHÁC cột này**; giữ cả hai.</summary>
    public string? CavityType { get; set; }

    /// <summary>STATUS — trạng thái nghiệp vụ của khoang, **KHÁC `FlagActive`** (cờ bật/tắt bản ghi).
    /// Nguồn giữ CẢ HAI (luật `C0-quingentesimusquartus`).</summary>
    public string? Status { get; set; }

    /// <summary>
    /// 🔴 STARTUSEDATE / FINISHUSEDATE — **ngày ĐƯA VÀO / NGỪNG sử dụng khoang**.
    /// ⚠️ **KHÔNG PHẢI** `StartWorkTime`/`FinishWorkTime` đã có (giờ bắt đầu/kết thúc CA làm việc) — hai
    /// khái niệm khác hẳn, rất dễ tưởng "đã có rồi". (`StartWorkTime` thậm chí KHÔNG có trong `DbDefine`.)
    /// ⚠️ Nguồn so sánh CẢ `is null` LẪN `= ''` ⇒ cột lưu kiểu **CHUỖI**, không phải date.
    /// </summary>
    public string? StartUseDate { get; set; }
    public string? FinishUseDate { get; set; }

    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Cấp bảo dưỡng theo mốc km (KM → số lần BD) — port 1:1 FrmMstMaintenanceLevelMng (TCMotor).</summary>
public sealed class MaintenanceLevelMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public int Km { get; set; }              // KM: mốc km bảo dưỡng
    public int MaintenanceCount { get; set; } // MAINTANCES: số lần/cấp bảo dưỡng tại mốc
    public string? Note { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>#541 Danh mục **LOẠI GIA HẠN BẢO HÀNH** (`Ser_MST_ROWarrantyRenewalCategory`).
/// Chính là nửa còn lại của khoá upsert ở `WarrantyExtensionDateLog.ExtCategoryCode` (`WrtReneCateCode`)
/// — trước nay MiniHTC dùng mã đó **mà không có danh mục** để đối chiếu.</summary>
public sealed class WarrantyRenewalCategoryMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string WrtReneCateCode { get; set; } = "";
    public string? WrtReneCateName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>#539 ĐỊNH MỨC công phát sinh **theo loại bảo hành chi tiết** (`Ser_MST_ROWorkArisingQuota`).
/// Khoá nghiệp vụ = (`ROWArisCode`, `ROWTypeDtlCode`). Nguồn kiểm `ROWTypeDtlCode` phải có trong
/// `Ser_MST_ROWarrantyType` — nhưng **chỉ ở nhánh THÊM MỚI** (xem chú thích endpoint).</summary>
public sealed class RoWorkArisingQuotaMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ROWArisCode { get; set; } = "";
    public string? ROWArisName { get; set; }
    /// <summary>Mã loại bảo hành CHI TIẾT — khoá ngoại tới `Ser_MST_ROWarrantyType.ROWTypeDtlCode`.</summary>
    public string ROWTypeDtlCode { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>#538 Phụ tùng phát sinh **của CommonCenter** (`Ser_MST_PartExtra`) — **KHÁC** `ExtraPartMst`
/// (vốn port từ `Tbl_Mst_Extra_Parts_Mng`). Hai bảng khác nhau, cùng nói về "phụ tùng phát sinh":
/// bảng này có thêm `ROMSID` (khoá bộ định mức) và dùng tên cột `VieName`/`TotalLimit`.</summary>
public sealed class PartExtraMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Khoá bộ định mức — nguồn cho lọc `t.ROMSID`; `ExtraPartMst` **không có** cột này.</summary>
    public string? ROMSID { get; set; }
    public string PartCode { get; set; } = "";
    /// <summary>Tên tiếng Việt — nguồn đặt là `VieName` (không phải `PartName`).</summary>
    public string? VieName { get; set; }
    public string? Unit { get; set; }
    public decimal? Price { get; set; }
    /// <summary>Giới hạn tổng — nguồn đặt là `TotalLimit` (không phải `MaxQuantity`).</summary>
    public decimal? TotalLimit { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Phụ tùng phát sinh (mã/tên/ĐVT/giá/SL tối đa) — port 1:1 FrmMstExtraPartsMng (Tbl_Mst_Extra_Parts_Mng, TCMotor).</summary>
public sealed class ExtraPartMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }    // VieName
    public string? Unit { get; set; }
    public decimal Price { get; set; }
    public int MaxQuantity { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Công việc phát sinh (mã/tên/giá tối đa/VAT) — port 1:1 FrmMstExtraWorkMng (Tbl_Mst_Extra_Work_Mng, TCMotor).</summary>
public sealed class ExtraWorkMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ExtraWorkCode { get; set; } = "";   // ROWArisCode
    public string? ExtraWorkName { get; set; }          // ROWArisName
    public decimal MaxPrice { get; set; }
    public decimal Vat { get; set; }
    public string? Remark { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhà cung cấp phụ tùng dịch vụ (mã/tên/liên hệ/địa chỉ) — port 1:1 FrmMstSupplierCreate (TblSerMstSupplier, TCMotor).</summary>
public sealed class ServiceSupplier
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SupplierCode { get; set; } = "";
    public string? SupplierName { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? Address { get; set; }
    public string? DealerCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thời hạn bảo hành theo model (BH đại lý/HTV, km giới hạn, kỳ lưu kho) — port 1:1 FrmMngMst_WarrantyPeriod (Tbl_Mst_WarrantyPeriod, TCMotor).</summary>
public sealed class WarrantyPeriodMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public int DealerWarrantyPeriod { get; set; }   // tháng — BH tại đại lý
    public int HtcvWarrantyPeriod { get; set; }      // tháng — BH hãng (HTV)
    public int LimitedWarrantyKM { get; set; }        // số km giới hạn BH
    public int StoragePeriod { get; set; }            // tháng — kỳ lưu kho tối đa
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Kho ảo ↔ model được phép chứa — port 1:1 FrmMst_StorageGlobal (Tbl_Mst_StorageGlobal).</summary>
public sealed class StorageGlobalMap
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StorageCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Ký tự VIN → năm sản xuất thực tế + trạng thái lắp ráp — port 1:1 FrmMst_VINProductionYear_Actual (Tbl_Mst_VINProductionYear_Actual).</summary>
public sealed class VinProductionYear
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string VinChar { get; set; } = "";        // VINCHARACTERS: ký tự VIN đại diện năm (vị trí 10)
    public string ProductionYear { get; set; } = ""; // PRODUCTIONYEAR: năm SX
    public string? AssemblyStatus { get; set; }       // ASSEMBLYSTATUS: CKD/CBU...
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Biên độ tỉ lệ đặt hàng/kế hoạch theo đại lý + model — port 1:1 FrmMstTiLeDatHangKeHoach (Tbl_Mst_AmplitudeApprOrd).</summary>
public sealed class OrderAmplitude
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string? DealerName { get; set; }
    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public decimal AmplitudeOrdMax { get; set; }    // AMPLITUDEORDMAX: biên độ tối đa đặt hàng (%)
    public decimal AmplitudePlanMax { get; set; }   // AMPLITUDEPLANMAX: biên độ tối đa kế hoạch (%)
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC: `Mst_AmplitudeApprOrd` nguồn **không có** `FlagActive`.</summary>
    public string FlagActive { get; set; } = "1";
    /// <summary>⚠️ #147 — cột RIÊNG MiniHTC, đứng thay `LogLUDateTime` của nguồn.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // --- #147 parity Mst_AmplitudeApprOrd ---
    // (DealerName / ModelName cũng là cột riêng MiniHTC — nguồn chỉ lưu MÃ, tên lấy bằng join khi hiển thị.)
    public DateTime LogLUDateTime { get; set; } = DateTime.Now;
    public string? LogLUBy { get; set; }
}

/// <summary>Tham số hệ thống PDI (key-value, vd DEAL.PDIHOUR) — port 1:1 FrmMst_ParamPDI (Tbl_Mst_ParamPDI).</summary>
public sealed class ParamPdi
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ParamCode { get; set; } = "";   // PARAMCODE, vd DEAL.PDIHOUR
    public string? ParamName { get; set; }
    public string ParamValue { get; set; } = "";   // PARAMVALUE
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh sách email nhận cảnh báo theo loại nghiệp vụ — port 1:1 FrmMst_Warning_Email (Mst_EmailStaffWarning).</summary>
public sealed class WarningEmail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string WarningType { get; set; } = "";   // EMAILSWKEY: vd SALEMANCREATE
    public string? WarningName { get; set; }
    public string EmailList { get; set; } = "";      // EMAILSWVALUE: danh sách email cách nhau ; hoặc ,
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Hạn mức số ngày trễ vận tải theo đại lý + kho — port 1:1 FrmMst_QuanLyHanMucDoTreVanTai (Tbl_Mst_DelayTransports).</summary>
public sealed class DelayTransport
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string? DealerName { get; set; }
    public string StorageCode { get; set; } = "";
    public string? StorageName { get; set; }
    public int DelayDays { get; set; }   // DelayTransport: hạn mức số ngày trễ cho phép
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Master model chuẩn dịch vụ (Mst_CarModelStd — port 1:1 FrmMstCarModelStd, TCMotor DMSCarSv/Admin):
/// mã model + tên, dùng làm danh mục model tham chiếu cho các màn dịch vụ khác.</summary>
public sealed class CarModelStd
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string? ModelName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Video tư vấn dịch vụ (Ser_Mst_FilePathVideo — port 1:1 FrmSerMstFilePathVideoCreate/Search, TCMotor DMSCarSv/Admin):
/// thư viện video tư vấn hiển thị theo thứ tự (IdxView) + ảnh đại diện. FilePathAvatar lưu URL (thay browse-file bằng dán link).</summary>
public sealed class SerFilePathVideo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string FilePathVideoCode { get; set; } = "";
    public string? FilePathVideoName { get; set; }
    public string? FilePathVideo { get; set; }
    public string? FilePathAvatar { get; set; }
    public int IdxView { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>#526 Master **ĐẦU MỤC KIỂM TRA** khi tiếp nhận/giao xe (`Ser_Mst_ReceptionFAudit`).
/// Nguồn: `BizCarSv.Tab.cs:14946 Ser_Mst_ReceptionFAudit_Get` → thân thật `…_GetX` (`:15070`).
/// Khoá hợp = (`ReceptionFAudCode`, `ReceptionFAudType`) — nguồn nối bằng **cả hai** cột.</summary>
public sealed class ReceptionFAuditMst
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReceptionFAudCode { get; set; } = "";
    public string ReceptionFAudType { get; set; } = "";
    public string? ReceptionFAudName { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>#525 TỆP ĐÍNH KÈM phiếu tiếp nhận (`Ser_ReceptionFAttachFile`) — ảnh/tệp chụp lúc nhận xe.
/// Nguồn: cùng hàm `Ser_ReceptionF_ReceptionX_New20210727`, khối
/// `#region //// Refine and Check Ser_ReceptionFAttachFile`.</summary>
public sealed class ReceptionAttachFile
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReceptionFNo { get; set; } = "";
    /// <summary>Số thứ tự tệp trong phiếu (`FileIndex`, chuẩn hoá `StdParam`).</summary>
    public string? FileIndex { get; set; }
    /// <summary>🔴 Nguồn lưu **ĐƯỜNG DẪN**, không lưu nội dung — chuỗi thô, `StdDataInTable` dùng
    /// mã `""` nên **không chuẩn hoá gì cả** (giữ nguyên khoảng trắng/hoa thường).</summary>
    public string? ReceptionFilePath { get; set; }
    public string? ReceptionFileName { get; set; }
    /// <summary>Loại tệp (`StdParam`).</summary>
    public string? ReceptionFileType { get; set; }
    public string? Remark { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>#524 CHI TIẾT phiếu tiếp nhận (`Ser_ReceptionFDtl`) — mỗi dòng là **một đầu mục kiểm tra**
/// khi nhận xe. Nguồn: `Ser_ReceptionF_ReceptionX_New20210727` (`ZTemp.cs:21199`), khối
/// `#region //// Refine and Check Ser_ReceptionFDtl` + ba lần `SaveTemp` (Main/WH/Dealer).</summary>
public sealed class ReceptionDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReceptionFNo { get; set; } = "";
    /// <summary>Mã đầu mục kiểm tra — nguồn tra `Ser_Mst_ReceptionFAudit_CheckDB(code, type, exist, active)`.</summary>
    public string ReceptionFAudCode { get; set; } = "";
    public string ReceptionFAudType { get; set; } = "";
    /// <summary>Kết quả kiểm khi TIẾP NHẬN (nguồn chuẩn hoá bằng `StdFlag` ⇒ cờ "1"/"0").</summary>
    public string? ReceptionAudStatus { get; set; }
    /// <summary>🔴 Nguồn **tạo cột này rồi KHÔNG BAO GIỜ GÁN** trong hàm tiếp nhận ⇒ luôn rỗng;
    /// chỉ khâu GIAO XE mới điền. Xem chú thích tại endpoint.</summary>
    public string? DeliveryAudStatus { get; set; }
    /// <summary>Trạng thái dòng — nguồn gán cứng `TConst.ReceptionFStatus.Pending` = **"P"**.</summary>
    public string ReceptionFStatusDtl { get; set; } = "P";
    public string? Remark { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Ảnh mẫu trên phiếu tiếp nhận-giao xe (Ser_Mst_ModelAudImage — port 1:1 FrmSerMstModelAudImageCreate/Search, TCMotor DMSCarSv/Admin):
/// ảnh minh họa theo Model + đầu mục kiểm tra (ReceptionFAudType, mã tự do — chưa có master riêng), khóa hợp = (ModelCode, ReceptionFAudType).
/// FilePath lưu URL (thay browse-file bằng dán link).</summary>
public sealed class SerModelAudImage
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ModelCode { get; set; } = "";
    public string ReceptionFAudType { get; set; } = "";
    public string? FilePath { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Master code/name/status generic — port 1:1 loạt Frm masters (Bank/Color/DealerType/CarCancelType/...).</summary>
public sealed class MasterItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Category { get; set; } = "";   // 1 category = 1 màn Frm gốc
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string? ParentCode { get; set; }       // audit 2026-09-03: một số category có cha bắt buộc (District→ProvinceCode, Province→AreaCode) — trước đó bị bỏ sót
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Chi tiết khiếu nại theo xe (FrmChiTietKhieuNai — TCMotor DMSCarSv/Services): tra cứu lịch sử khiếu nại theo BIỂN SỐ.
/// Nguồn gốc là proxy sang hệ HCC (`iCIC_ListClaimByPlateNo` → API `DmsClaimGetByPlateNo`), lưới đúng 6 cột:
/// ClaimNo / CreatDate / ReceiveDate / DealerCode / CusRequest / ProcessDetail (lưới gốc read-only, không cho sửa).
/// </summary>
public sealed class ServiceComplaint
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PlateNo { get; set; } = "";        // khoá tra cứu (txtPlateNo)
    public string ClaimNo { get; set; } = "";        // gridColClaimNo — số khiếu nại
    public DateTime? CreatDate { get; set; }         // gridColCreateDate — ngày tạo (giữ nguyên tên gốc "CreatDate")
    public DateTime? ReceiveDate { get; set; }       // gridColReceiveDate — ngày tiếp nhận
    public string? DealerCode { get; set; }          // gridColDealerCode — đại lý
    public string? CusRequest { get; set; }          // gridColCusRequest — yêu cầu khách hàng
    public string? ProcessDetail { get; set; }       // gridColProcessDetail — chi tiết xử lý
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Voucher điểm hội viên (Crd_MemberVoucher — FrmMember_Voucher, TCMotor DMSCarSv/Services).
/// Nguồn lấy qua LoyaltyService.WA_OSCarSv_Crd_MemberVoucher_Get(memberNo).
/// Cột đúng lưới gốc: VoucherNo / PointVCTotal / PointVCRemain / PointVCLimit / PointExpireDate.
/// </summary>
public sealed class MemberVoucher
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string MemberNo { get; set; } = "";          // số hội viên
    public string VoucherNo { get; set; } = "";         // gridcolVoucherNo
    public decimal PointVCTotal { get; set; }           // gridcolPointVCTotal — tổng điểm voucher
    public decimal PointVCRemain { get; set; }          // gridcolPointVCRemain — giá trị còn lại
    public decimal PointVCLimit { get; set; }           // gridColPointVCLimit — điểm sử dụng TỐI ĐA mỗi lần
    public DateTime? PointExpireDate { get; set; }      // gridcolPointExpireDate — ngày hết hạn
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Điểm voucher đã áp vào 1 lệnh sửa chữa (Ser_RO_UpdateMemberVoucher).</summary>
public sealed class RoVoucherUse
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long RoId { get; set; }
    public string MemberNo { get; set; } = "";
    public string VoucherNo { get; set; } = "";
    public decimal PointVCUse { get; set; }             // gridColPointVCUse — điểm sử dụng lần này
    public DateTime AppliedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Lịch sử thay đổi thời gian GXDK — dự kiến giao xe của lệnh sửa chữa
/// (FrmHistoryGXDK — TCMotor DMSCarSv/Services; bảng nguồn Ser_Ro_PlanedDeliveryDate_His).
/// Mỗi lần đổi ngày dự kiến giao thì ghi thêm 1 dòng; dòng MỚI NHẤT mang FlagCurrent = "1".
/// Luật gốc: KHÔNG cho xoá dòng FlagCurrent = "1" ("Không được xóa Thời gian GXDK mới nhất.").
/// </summary>
public sealed class RoDeliveryDateHistory
{
    public long Id { get; set; }                        // ~ cột AUTOID của nguồn
    public Guid OrgId { get; set; }
    public long RepairOrderId { get; set; }             // ~ ROID
    public string RoNo { get; set; } = "";              // gridColRONo — số lệnh sửa chữa
    public string? PlateNo { get; set; }                // PlateNo — biển số
    public string? CusName { get; set; }                // gcol_CusName — tên khách hàng
    public DateTime PlanedDeliveryDate { get; set; }    // gcolDateGXDK — thời gian GXDK (dự kiến giao)
    public string? Remark { get; set; }                 // gcolNote (FieldName "REMARK") — ghi chú
    public string FlagCurrent { get; set; } = "1";      // "1" = bản mới nhất (KHÔNG được xoá), "0" = bản cũ
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Truy vấn / liên kết hội viên Hyundai theo đại lý (FrmQuery_LoyaltyMember — TCMotor DMSCarSv/Services).
/// Nguồn: LoyaltyService.Map_QueryDealer_Member_Create(dealerCode, memberNo, phone)
/// + tra cứu Crd_Member bên Loyalty (WA_Crd_Member_Get) lọc theo MemberNo + PhoneNo + trạng thái "APPROVE".
/// Mỗi lần truy vấn thành công ghi 1 bản ghi map Đại lý ↔ Hội viên.
/// </summary>
public sealed class DealerMemberQuery
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";        // đại lý thực hiện truy vấn
    public string MemberNo { get; set; } = "";          // txtMemberNo — mã hội viên
    public string PhoneNo { get; set; } = "";           // txtPhone — số điện thoại (chỉ chữ số)
    public string? CardNo { get; set; }                 // cc_CardNo trả về từ Loyalty
    public string MemberStatus { get; set; } = "APPROVE"; // nguồn chỉ tra hội viên đã APPROVE
    public DateTime QueriedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Hãng bảo hiểm (Ser_Insurance — FrmInsuranceCreate/Modify, TCMotor DMSCarSv/Admin).
/// Header khai báo hãng BH; kèm lưới khách hàng thuộc hãng (xem <see cref="ServiceInsuranceCustomer"/>).
/// </summary>
public sealed class ServiceInsurance
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsNo { get; set; } = "";        // txt_InsNo — mã hãng BH (bắt buộc, KHÔNG trùng)
    public string InsVieName { get; set; } = "";   // txt_InsVieName — tên tiếng Việt (bắt buộc)
    public string? InsEngName { get; set; }        // txt_InsEngName — tên tiếng Anh
    public string Address { get; set; } = "";      // txt_Address — địa chỉ (bắt buộc)
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? Taxcode { get; set; }           // txt_Taxcode — mã số thuế
    public string? Description { get; set; }
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// Khách hàng thuộc hãng bảo hiểm (Ser_InsuranceCustomer — lưới con của FrmInsuranceCreate).
/// Luật gốc (gviewPart_ValidateRow): CusId KHÔNG được trống và KHÔNG được trùng trong cùng 1 hãng.
/// </summary>
public sealed class ServiceInsuranceCustomer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ServiceInsuranceId { get; set; }
    public string CusId { get; set; } = "";        // gridCusID — bắt buộc + duy nhất trong hãng
    public string? CusName { get; set; }           // gridCusName
    public string? Address { get; set; }           // gridAddress
    public string? Mobile { get; set; }            // gridTelephone (FieldName = Mobile)
    public string? Description { get; set; }       // gridDescription
}

// ===== #B05 Ba bảng cho báo cáo "Xe chuyển sai vùng thị trường chính (tỉnh)" =====
// Nguồn: `Rpt_CarChangeProvinceWhenDelivery_New20181115` (`BizHTC.ZTempGPS.cs:8982`, twin LIVE của
// `FrmRptXeChuyenSaiVungTTChinh`). Ba bảng này trước đây KHÔNG CÓ trong MiniHTC ⇒ báo cáo không thể port.

/// <summary>
/// Nhật ký xe ĐỔI TỈNH theo GPS (`Rpt_CarChangeProvince`). Dữ liệu do job đồng bộ Veloca ghi vào:
/// `Rpt_CarChangeProvince_Save_New20181119` (`Biz.HTC.WH.cs:135185`) gọi WS GPS lấy bảng `DMS_CHANGE_CITY`
/// rồi `insert into Rpt_CarChangeProvince (AutoID, StorageCode, GPSDvNo, VIN, GPSProvinceCode, ChangeDateTime,
/// MapLongitude, MapLatitude, GPSAddress, Remark, LogLUDateTime, LogLUBy)` (:135359-135387).
/// ⚠️ `AutoID` là **số của phía GPS/Veloca**, KHÔNG phải identity của DB — nguồn chép thẳng `t.AutoID`
/// và dùng `max(AutoID)` làm mốc nước (`Rpt_MaxAutoIDChangeProvince`) cho lần đồng bộ sau.
/// 🔴 Nợ: job đồng bộ Veloca chưa port (MiniHTC không tới được Veloca) — xem log #B05.
/// </summary>
public sealed class RptCarChangeProvince
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>Số bản ghi phía GPS (`AutoID`) — mốc nước đồng bộ, không phải khoá của MiniHTC.</summary>
    public long AutoID { get; set; }
    public string? StorageCode { get; set; }
    public string GPSDvNo { get; set; } = "";
    public string VIN { get; set; } = "";
    /// <summary>Mã tỉnh THEO HỆ GPS (`GPSProvinceCode`) — phải qua <see cref="MapProvinceGpsDms"/>
    /// mới ra mã tỉnh của DMS.</summary>
    public string? GPSProvinceCode { get; set; }
    /// <summary>Thời điểm xe đổi tỉnh (`ChangeDateTime`) — báo cáo đòi `>= DlvEndGPSDateTime`.</summary>
    public DateTime? ChangeDateTime { get; set; }
    public decimal? MapLongitude { get; set; }
    public decimal? MapLatitude { get; set; }
    public string? GPSAddress { get; set; }
    public string? Remark { get; set; }
    public DateTime? LogLUDateTime { get; set; }
    public string? LogLUBy { get; set; }
}

/// <summary>Ánh xạ mã tỉnh GPS ↔ mã tỉnh DMS (`Map_ProvinceGPS_DMS`).
/// Nguồn chỉ dùng đúng hai cột này (`BizHTC.ZTempGPS.cs:9096-9097` + điều kiện
/// `mpgpsdms.ProvinceCode != md.ProvinceCode`) ⇒ chỉ khai hai cột có BẰNG CHỨNG, không bịa thêm.</summary>
public sealed class MapProvinceGpsDms
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GPSProvinceCode { get; set; } = "";
    public string ProvinceCode { get; set; } = "";
}

/// <summary>Danh mục tỉnh phía hệ GPS (`GPS_Mst_Province`). Nguồn dùng `GPSProvinceCode` + `GPSProvinceName`
/// (`BizHTC.ZTempGPS.cs:9107-9108`).</summary>
public sealed class GpsMstProvince
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string GPSProvinceCode { get; set; } = "";
    public string? GPSProvinceName { get; set; }
}


// ===== 🔴 #463 THẺ HỘI VIÊN LOYALTY (`Crd_Card`) — dùng ở tab hội viên của `FrmQuotation` =====
/// <summary>Ảnh chụp thẻ hội viên lấy từ **API Loyalty** (`CrdCard/WA_OSCarSv_Crd_Card_Get`), không phải
/// bảng của DMSCarSv. Nguồn trả 39 cột kiểu `object`; ở đây giữ các cột nghiệp vụ thật sự được màn dùng.</summary>
public sealed class LoyaltyCard
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>`CardNo` — số thẻ.</summary>
    public string CardNo { get; set; } = "";
    /// <summary>`MemberNo` — số hội viên (khoá lọc chính).</summary>
    public string MemberNo { get; set; } = "";
    public string? NetworkID { get; set; }
    public string? RankPolicyCode { get; set; }
    /// <summary>`CardTypeUse` / `CardTypeInit` — hạng đang dùng và hạng khởi tạo (taxonomy I/N/S/G/P).</summary>
    public string? CardTypeUse { get; set; }
    public string? CardTypeInit { get; set; }
    public string? CardTypeUsePrev { get; set; }
    public string? CardNoPrev { get; set; }
    /// <summary>`CardStatus` — trạng thái THẺ. ⚠️ KHÁC `Crd_Member.MemberStatus` mà bộ lọc dùng.</summary>
    public string? CardStatus { get; set; }
    public DateTime? EffDateStart { get; set; }
    public DateTime? EffDateEnd { get; set; }
    public DateTime? CardActiveDate { get; set; }
    /// <summary>Ba cặp Total/Block/Avail — điểm, tiền, lượt ghé.</summary>
    public decimal PointTotal { get; set; }
    public decimal PointBlock { get; set; }
    public decimal PointAvail { get; set; }
    public decimal AmountTotal { get; set; }
    public decimal AmountBlock { get; set; }
    public decimal AmountAvail { get; set; }
    public decimal QtyVisitTotal { get; set; }
    public decimal QtyVisitBlock { get; set; }
    public decimal QtyVisitAvail { get; set; }
    public decimal PointBonus { get; set; }
    public decimal PointCardRank { get; set; }
    public decimal TotalAmountPeriod { get; set; }
    public string? FlagExceptionally { get; set; }
    public string? DLCodeExceptionally { get; set; }
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}


// ===== #472 THAM SỐ CẤU HÌNH THEO ĐẠI LÝ (`Mst_Param`) =====
/// <summary>Nguồn tra `Mst_Param` theo bộ ba `DealerCode` + `ParamCode` + `ParamType`.
/// Ca dùng đầu tiên: `ParamCode = ParamType = "MCC"` quyết định **phương pháp tính giá vốn**
/// (`"FIFO"` hay không) trong báo cáo tồn kho.</summary>
public sealed class MstParam
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ParamCode { get; set; } = "";
    public string ParamType { get; set; } = "";
    public string? ParamValue { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}


// ===== 🔴 #482 DANH SÁCH VIN ĐẦY ĐỦ CỦA CHIẾN DỊCH (`Ser_CampaignMarketingFullVIN`) =====
/// <summary>Bảng con thứ **năm** của chiến dịch marketing — bốn bảng kia (`VIN`, `PlateNo`, `Dealer`,
/// `Part`) đã ghi nợ ở #392/#393. Chỉ nhánh **KHO** (`Ser_CampaignMarketing_Get_WH`) trả bảng này
/// ra kết quả; nhánh đại lý thì không ⇒ đây là khác biệt **HÌNH DẠNG KẾT QUẢ**, không phải bộ lọc.</summary>
public sealed class CampaignMarketingFullVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>`CamMarketingNo` — khoá nối sang chiến dịch (nguồn nối theo SỐ, không theo Id).</summary>
    public string CamNo { get; set; } = "";
    public string VinNo { get; set; } = "";
    /// <summary>`CamMarketingFullVINStatus` — trạng thái lan theo bước duyệt (#392): `P` chờ · `A` đã duyệt.</summary>
    public string? CamMarketingFullVinStatus { get; set; }
    /// <summary>`MyIdxSeq` — nguồn `order by t.MyIdxSeq asc` (thứ tự do bảng lọc quyết định).</summary>
    public int MyIdxSeq { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}


// ===== 🔴 #483 BA BẢNG CON CÒN LẠI CỦA CHIẾN DỊCH MARKETING (trả nốt nợ #392/#393) =====
// Nguồn ghi cả năm bảng con trong CÙNG một hàm `Ser_CampaignMarketing_Create/Update`
//   (`CampaignMarketing/BizCarSv.CampaignMarketing.cs:5205-5310`), mỗi bảng một `#region // SaveDB …`.
// ⚠️ Cả ba đều đặt trạng thái khởi tạo = `TConst.CamMarketingStatus.Pending`; mở hằng
//   (`TERP.Constants/CampaignMarketing/Const.Main.BE.cs:23`) thì **giá trị thật là "P"**, và bảng mã
//   **chỉ có HAI** giá trị: `Pending = "P"` · `Approve = "A"` — **không có mã từ chối**.
// ⚠️ Hai cột nhật ký `LogLUDTime`/`LogLUBy` ở cả ba khối **đều bị COMMENT** ⇒ port dòng ACTIVE:
//   ba bảng con này **không lưu vết người sửa**. Đây là hành vi thật của nguồn, không phải thiếu sót port.
// 🔴 Nguồn ghi **SONG SONG hai CSDL**: `_dbMain.SaveData(...)` rồi `_dbWH.SaveData(...)` cùng một
//   `DataTable` ⇒ ghi kép Main + Kho. MiniHTC một CSDL ⇒ ghi một lần, nêu cờ `dualWriteInSource`.
/// <summary>`Ser_CampaignMarketingVIN` — danh sách VIN được chỉ định thủ công cho chiến dịch.
/// Khác `CampaignMarketingFullVin` (#482): bảng kia là danh sách ĐẦY ĐỦ do hệ sinh ra.</summary>
public sealed class CampaignMarketingVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    /// <summary>Nguồn đặt tên cột là `VIN` (không phải `VinNo` như bảng FullVIN) — giữ đúng phân biệt.</summary>
    public string VIN { get; set; } = "";
    public string? CamMarketingVinStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>`Ser_CampaignMarketingPlateNo` — chỉ định theo BIỂN SỐ.
/// ⚠️ Nguồn chỉ ghi **`StartPlateNo`**; grep toàn cụm **không có `EndPlateNo`** ⇒ đây là DANH SÁCH biển số,
/// KHÔNG phải KHOẢNG biển số, dù tên cột có chữ "Start" gợi ý ngược lại.</summary>
public sealed class CampaignMarketingPlateNo
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    public string StartPlateNo { get; set; } = "";
    public string? CamMarketingPlateNoStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>`Ser_CampaignMarketingDealer` — phạm vi đại lý áp dụng chiến dịch.</summary>
public sealed class CampaignMarketingDealer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CamNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? CamMarketingDealerStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}


// ===== 🔴 #488 DANH MỤC DỊCH VỤ CHUẨN (`Ser_MST_Service`) — trước nay CHƯA mô hình hoá =====
/// <summary>Báo cáo chênh lệch giá (`Ser_ReportRoVarianceCost`) so **giá bán trên lệnh** với **giá chuẩn**
/// của danh mục. Phía phụ tùng đã có `ServicePart.Price/VAT`; phía **dịch vụ** thì thiếu hẳn bảng chuẩn
/// ⇒ không có bảng này thì báo cáo **không tồn tại được** (đúng loại "cột/bảng THIẾU HẲN" mà §12 không bắt).</summary>
public sealed class ServiceMstService
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    /// <summary>`SerID` — khoá nối từ dòng dịch vụ của lệnh (`Ser_ROServiceItems.SerID`).</summary>
    public string SerID { get; set; } = "";
    public string SerCode { get; set; } = "";
    public string? SerName { get; set; }
    /// <summary>`Price` — **giá CHUẨN** (giá danh mục), đối chiếu với giá bán trên lệnh.</summary>
    public decimal Price { get; set; }
    /// <summary>`Vat` — %VAT chuẩn.</summary>
    public decimal Vat { get; set; }
    public decimal? StdManHour { get; set; }
    public string? DealerCode { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
