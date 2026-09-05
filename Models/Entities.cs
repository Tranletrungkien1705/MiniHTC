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
public sealed class Dealer
{
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

/// <summary>Thu hồi xe (FrmMngCarRetrieve / FrmNewCarRetrieve) — thu hồi xe từ đại lý về kho HTC.</summary>
public sealed class CarRetrieve
{
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
    public string Status { get; set; } = "Pending";      // Pending → Approved / Rejected (Stage.Pending nguồn)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
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
    /// <summary>Số lần lập kế hoạch (`TimesPlan`) — nguồn giữ khi nhân bản phiên bản.</summary>
    public int TimesPlan { get; set; }
    public DateTime? CancelledAt { get; set; }
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

/// <summary>Đơn đặt hàng nhà cung cấp (TCMotor Supplier PO) — OEM mua phụ tùng từ NCC.</summary>
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

/// <summary>Hạn mức phân bổ xe (Mst_Quota — port 1:1 FrmMngQuota): số lượng xe theo đại lý/model/kỳ.</summary>
public sealed class Quota
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public string Period { get; set; } = "";   // YYYYMM
    public int Qty { get; set; }
    public int UsedQty { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
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
}

/// <summary>Danh sách hóa đơn xuất bán (InvoiceList — port 1:1 FrmNewInvoice/FrmMngInvoice):
/// header 1 lô hóa đơn (nhập Excel A2), CreatedDate + số tham chiếu.</summary>
public sealed class InvoiceList
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InvoiceListCode { get; set; } = "";   // số tham chiếu (GetInvoiceListNo)
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

/// <summary>Dòng hóa đơn (InvoiceListDetail): 1 xe/VIN + số hóa đơn + ngày HĐ.</summary>
public sealed class InvoiceLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ListId { get; set; }
    public string? CarId { get; set; }
    public string? DealerCode { get; set; }
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
    public string Status { get; set; } = "Pending";   // Pending → Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }
}

/// <summary>Dòng xe trong YC vận chuyển (TranspReqDtl): VIN + DO + màu + kho.</summary>
public sealed class TransportReqCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string Vin { get; set; } = "";
    public string? DoNo { get; set; }
    public string? ColorCode { get; set; }
    public string? StorageCode { get; set; }
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
}

/// <summary>Biên bản vận chuyển / giao nhận (TransportMinutes — port 1:1 FrmNewTransportMinutes/FrmMngTransportMinutes, Phase2):
/// biên bản giao nhận lô xe do nhà VC chuyển tới đại lý. Pending(Đang xử lý)→Approved(Phê duyệt)/Rejected(Từ chối).</summary>
public sealed class TransportMinutes
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string TransportMinutesNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string TransporterCode { get; set; } = "";
    public string Status { get; set; } = "Pending";   // Pending → Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DecidedAt { get; set; }
}

/// <summary>Dòng xe trong BB vận chuyển (TransportMinutesDetail): VIN + DO + màu + trạng thái dòng.</summary>
public sealed class TransportMinutesCar
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long MinutesId { get; set; }
    public string Vin { get; set; } = "";
    public string? DoNo { get; set; }
    public string? ColorCode { get; set; }
    public string? EngineNo { get; set; }
    public string DtlStatus { get; set; } = "Pending";  // theo header
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
    public string? GpsInType { get; set; }        // loại nhập
    public string StorageCode { get; set; } = ""; // kho GPS
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Dòng thiết bị nhập (StoF_GPSInDtl): số thiết bị + số hộp + trạng thái map.</summary>
public sealed class GpsInDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long InId { get; set; }
    public string GpsDvNo { get; set; } = "";     // số thiết bị GPS
    public string? GpsBoxNo { get; set; }         // số hộp
    public string MapStatus { get; set; } = "0";  // '1' = đã gắn lên xe
    public string? Remark { get; set; }
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
}

/// <summary>Dòng thiết bị xuất (StoF_GPSOutDtl): số thiết bị + số hộp + trạng thái map.</summary>
public sealed class GpsOutDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long OutId { get; set; }
    public string GpsDvNo { get; set; } = "";
    public string? GpsBoxNo { get; set; }
    public string MapStatus { get; set; } = "0";
    public string? Remark { get; set; }
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
    public decimal ValTransport { get; set; }   // tự tính = TFValReal + InsuranceCost - TPValReal
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

/// <summary>NVBH đại lý (Mst_DlSalesMan — port 1:1 FrmMngSalesManHTC/FrmMngSalesManApproved, SalesDealer):
/// NVBH tại đại lý; duyệt = cấp SMHyundaiCode (mã Hyundai). SMStatus: THUVIEC/CHINGTHUC/CTVIEN/NGHIVIEC.</summary>
public sealed class DlSalesMan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SMCode { get; set; } = "";
    public string SMName { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? SMHyundaiCode { get; set; }        // mã Hyundai cấp (có = đã duyệt)
    public string SMStatus { get; set; } = "THUVIEC"; // THUVIEC/CHINGTHUC/CTVIEN/NGHIVIEC
    public string? Sex { get; set; }                  // 0=Nam, 1=Nữ
    public DateTime? DateOfBirth { get; set; }
    public string? PhoneNo { get; set; }
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
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string RONo { get; set; } = "";
    public string LicensePlate { get; set; } = "";     // biển số
    public string? Vin { get; set; }
    public string? CusName { get; set; }               // chủ xe
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
    public string? TrademarkNameModel { get; set; }    // Ser_RO.TrademarkNameModel — hiệu/dòng xe
    public string? ColorCode { get; set; }             // Ser_Car.ColorCode — màu xe
    public string? Assistant { get; set; }             // Ser_RO.Assistant — cố vấn dịch vụ
    public DateTime? ActualDeliveryDate { get; set; }  // Ser_RO.ActualDeliveryDate — "Giờ giao xe thực tế"
    public DateTime? FinishedDate { get; set; }        // Ser_RO.FinishedDate — khoá sắp xếp (order by desc)
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

    public decimal Amount { get; set; }                // tiền công
}

/// <summary>Dòng phụ tùng trong RO (Ser_RO_PartItems): mã PT + ĐVT + SL cần + đơn giá.</summary>
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
    public string? RONO { get; set; }                  // RO liên kết (nếu đã lập lệnh)
    public string Status { get; set; } = "Pending";    // Pending(Tiếp nhận) → Approved(Giao xe)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliveredAt { get; set; }
}

/// <summary>Phiếu nhập kho phụ tùng (Ser_Inv_StockIn — port 1:1 FrmStockInCreate, TCMotor DMSCarSv/Inventory):
/// nhập phụ tùng vào kho. Draft → Posted (ghi sổ, tăng tồn PartStock).</summary>
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
    public string? RejectReason { get; set; }
    public string? RejectedBy { get; set; }
    public DateTime? RejectedAt { get; set; }
}

/// <summary>Dòng phụ tùng xuất (Ser_Inv_StockOutDetail): mã PT + vị trí + SL.</summary>
public sealed class PartStockOutLine
{
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

/// <summary>Master phụ tùng TST (TST_Mst_Part) — port 1:1 FrmTST_Mst_Part (TCMotor DMSCarSv). Mã + tên HTC/Việt/Anh + đơn vị + VAT + giá TST + nhóm/loại.</summary>
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
    public decimal TSTPrice { get; set; }
    public string? PartGroup { get; set; }
    public string? PartType { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; }
}

/// <summary>Hợp đồng bảo hiểm dịch vụ (Ser_InsuranceContract) — port 1:1 FrmInsuranceContractCreate/Search (TCMotor DMSCarSv/Admin). Theo mã HĐ (auto): số HĐ + loại thanh toán + hiệu lực + hãng BH (InsNo→SerInsurance) + hạn mức.</summary>
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
    public string BDHStatus { get; set; } = "Pending";   // Pending → Approved / Rejected (BĐH duyệt)
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
}

/// <summary>Số tiền chiết khấu TT được duyệt theo VIN (PRD_PaymentReqDiscount_VIN) — port 1:1 FrmImportExl_PaymentReqDiscount (2010.HTC). Import số tiền HTC duyệt cho từng VIN trong đề nghị chiết khấu; upsert theo (PRDiscountNo × VIN).
/// Mở rộng thêm các cột chi tiết VIN (CarId/SpecCode/DeliveryDate/DlrContractNo/SMName/UnitPriceActual/AmountDealerRequest/CustomerName) để phục vụ FrmPayReDiscount/FrmMngPaymentReqDiscountDealer (đề nghị + duyệt 2 cấp).</summary>
public sealed class PaymentReqDiscountVin
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PRDiscountNo { get; set; } = "";
    public string VIN { get; set; } = "";
    public decimal AmountHTCAppr { get; set; }
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
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? Approve1At { get; set; }
    public DateTime? Approve2At { get; set; }
    public DateTime? CancelledAt { get; set; }
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
    public string ModelList { get; set; } = "";     // ListModel
    public string? SpecMix { get; set; }             // ListSpecMix
    public string? DealerList { get; set; }          // ListDealer
    public decimal TotalQty { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
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
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
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
    public string FlagActive { get; set; } = "1";
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
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
public sealed class SerAssignmentWork
{
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

/// <summary>Đề nghị bảo hành dịch vụ (đại lý gửi HTC duyệt theo RO) — port 1:1 FrmWarrantyReportDealerSearch/HTCSearch/HTCApproved (Ser_ROWarrantyReport, TCMotor).</summary>
public sealed class ServiceWarrantyClaim
{
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 🔴 DÒNG PHỤ TÙNG của đề nghị bảo hành (`Ser_ROWarrantyReportPartItems` — 1-n theo ROWID).
/// Port cũ CHỈ có 1 cột vô hướng <c>ServiceWarrantyClaim.PartCode</c> ⇒ mỗi đề nghị chỉ khai được
/// ĐÚNG 1 phụ tùng, không có số lượng / đơn giá / VAT / nguồn gốc PT ⇒ mất toàn bộ chiều chi tiết
/// và mọi luật kiểm tra theo dòng của nguồn (BizCarSv.WarrantyReport.cs:960-1290, máy 150 canonical).
/// </summary>
public sealed class WarrantyClaimPartItem
{
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
/// CRM follow-up. CareType: CARE24H/CARE72H/DOB(sinh nhật)/MAINT(nhắc bảo dưỡng). Pending→Contacted→Closed.</summary>
public sealed class CustomerCare
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CareNo { get; set; } = "";
    public string CareType { get; set; } = "CARE24H";   // CARE24H/CARE72H/DOB/MAINT
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ContactedAt { get; set; }
}

/// <summary>
/// Chăm sóc khách hàng nhân dịp SINH NHẬT (Ser_CustomerCareBth —
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
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
}

/// <summary>
/// Phiếu khảo sát chăm sóc khách hàng sau dịch vụ (Ser_CustomerCare24h / Ser_CustomerCare72h —
/// port 1:1 FrmCSCCustomerCare24h/72h, TCMotor DMSCarSv/Customer).
/// Mỗi phiếu CSKH (<see cref="CustomerCare"/>) có TỐI ĐA MỘT bản khảo sát: nguồn đọc
/// "top 1 * where CusCareID = ..." rồi insert-nếu-chưa-có / update-nếu-đã-có (upsert theo CareNo).
/// Bộ 6 câu hỏi dùng CHUNG cho cả 24h và 72h (form 24h nạp hằng số của lớp SerCusCare72hQA).
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

    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Chiến dịch marketing HTC gửi đại lý (Ser_CampaignMarketing — port 1:1 FrmSer_CampaignMarketing/Mng + FrmListDealer(Update),
/// TCMotor DMSCarSv/Ser_CampaignMarketing): header + điều kiện áp dụng (VIN/biển số/đại lý, lưu CSV theo đúng cách nhập tay của WinForm)
/// + danh sách phụ tùng khuyến mãi kèm % giảm.</summary>
public sealed class CampaignMarketing
{
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
    public string? ContEmail { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

/// <summary>Đơn đặt phụ tùng từ NCC (Ser_Order_Part — port 1:1 FrmSer_Order_Part, TCMotor DMSCarSv/TST):
/// đơn mua phụ tùng gửi nhà cung cấp. OrderPartStatus: Pending(Mới tạo)→Approved(Đã gửi NCC)→Finished(Hoàn thành).</summary>
public sealed class OrderPart
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string OrderPartNo { get; set; } = "";
    public string SupplierCode { get; set; } = "";
    public string? WarehouseCode { get; set; }
    public string OrderPartStatus { get; set; } = "Pending"; // Pending → Approved → Finished
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}

/// <summary>Dòng phụ tùng đặt (Ser_Order_Part_Dtl): mã PT + SL đặt + đơn giá.</summary>
public sealed class OrderPartLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long OrderPartId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal OrderQty { get; set; } = 1;
    public decimal Price { get; set; }

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

    public string? Resolution { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }          // ApprDTime
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

    /// <summary>Thành tiền dòng = QtyPay × Price × (1 + VAT%).</summary>
    public decimal Amount { get; set; }
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? QuotedAt { get; set; }
}

/// <summary>Dòng PT xin báo giá (Req_PartPriceDtl): mã PT + SL yêu cầu + giá TST báo (điền sau).</summary>
public sealed class ReqPartPriceLine
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ReqId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal ReqQty { get; set; } = 1;
    public decimal QuotedPrice { get; set; }           // TST điền
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
    public string? EngineerType { get; set; }      // 1=CVDV,2=KTV sửa chữa chung,3=KTV đồng sơn,4=Khác
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
}

/// <summary>Packing List (PL) — port 1:1 FrmNewPL/FrmMngPL (DMSales.Foton). Danh sách đóng gói lô xe lên tàu theo LC, cảng, ngày lên tàu/đến cảng.</summary>
public sealed class PackingList
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PLNo { get; set; } = "";
    public string LcNo { get; set; } = "";
    public string? PortCode { get; set; }
    public string? PLType { get; set; }
    public DateTime ShippingDateStart { get; set; }        // ngày lên tàu
    public DateTime ShippingDateEndExpected { get; set; }  // ngày DK đến cảng
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
}

/// <summary>Ngưỡng tồn kho bán hàng (Mst_MngRateTonKhoBanHang) — port 1:1 FrmMstSalesInventoryThreshold (2010.HTC/Admin/Product). Ngưỡng bán hàng (NguongBH) theo đại lý + model.</summary>
public sealed class SalesInventoryThreshold
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DealerCode { get; set; } = "";
    public string ModelCode { get; set; } = "";
    public int NguongBH { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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

/// <summary>Ngân hàng đại lý (Mst_DealerBank) — port 1:1 FrmDealerBank (2010.HTC/Admin/Product). Tài khoản/hạn mức NH của đại lý + cờ NH bảo lãnh/thanh toán.</summary>
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
    public string SoDeNghi { get; set; } = "";       // số đề nghị (auto)
    public string BankCode { get; set; } = "";       // ngân hàng
    public string TransType { get; set; } = "";       // loại ĐN GD: GNTT/BLLC/PHLC
    public DateTime? DisbursementDate { get; set; }   // ngày giải ngân
    public decimal AmountDisbursed { get; set; }      // số tiền giải ngân
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Draft";     // Draft → Sent → Approved / Rejected
    public string? Remark { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SentAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    /// <summary>
    /// Trạng thái bên NGÂN HÀNG (`BkTransBankStatus`, `TConst.BkTransBankStatus` — hệ `ERP.DMS.HTC.VPBank.WS`,
    /// **chỉ có trên máy 150**): "N" · "P" · "C" · **"A0".."A5"** các mức duyệt · "F" hoàn tất · "R" từ chối.
    /// 📌 Port cũ chỉ ghi nhận P/A1/A2/A3 — nguồn có **A0 và A4, A5** nữa, cùng "F"/"C"/"R".
    /// </summary>
    public string BankStatus { get; set; } = "P";
    public DateTime? PushedToBankAt { get; set; }

    /// <summary>Mã tham chiếu do ngân hàng cấp khi báo kết quả về (`RefBankCode`).</summary>
    public string? RefBankCode { get; set; }
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
public sealed class DealerContractDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DealerContractId { get; set; }
    public string CarId { get; set; } = "";
    public decimal UnitPrice { get; set; }
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
    public string MtnType { get; set; } = "";        // loại bảo trì (Mst_MaintainType)
    public string Status { get; set; } = "Draft";    // Draft → Done
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
}
public sealed class StoFMaintainMain
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StoFMaintainId { get; set; }
    public string VIN { get; set; } = "";
    public string? MtnTp { get; set; }               // loại BT dòng
    public string? ModelCode { get; set; }
    public string? UserCodeMtn { get; set; }         // người bảo trì
    public string? StorageCodeInit { get; set; }     // kho ban đầu
    public string? StorageCodeCurrent { get; set; }  // kho hiện tại
    public string? MtnStatusMain { get; set; }       // trạng thái bảo trì
    public string? Remark { get; set; }
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
}
public sealed class DlrContractDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long ContractId { get; set; }
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

/// <summary>Khách hàng của đại lý (DealerCustomer) — port 1:1 FrmNewCustomer/FrmMngCustomer (DMSales.Foton/SalesDealer). Master KH cấp đại lý: loại KH, CMND, giới tính, DOB...</summary>
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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

/// <summary>Phiếu thanh toán ngân hàng (Pmt_PM) — port 1:1 FrmMngPM. Header.</summary>
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

/// <summary>Chi tiết phiếu thanh toán theo VIN (Pmt_PMDetail) — port 1:1 FrmMngPM detail.</summary>
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
    public decimal VAT { get; set; } = 10;
    public string DealerCode { get; set; } = "";
    public string BankCode { get; set; } = "";
    public string SourceInvoiceName { get; set; } = "";   // nguon HD
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
    public string SignStatusDtl { get; set; } = "P";
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
    /// <summary>Thời điểm giải chấp xong toàn bộ lô.</summary>
    public DateTime? FinishedAt { get; set; }
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
    public string ConfirmStatus { get; set; } = "Pending";   // Pending -> Confirmed (phía nhà vận chuyển)
    public string Remark { get; set; } = "";
    public DateTime? ConfirmDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // --- Biên bản giao nhận có HAI PHÍA duyệt độc lập (Sto_DlvMinutes, 2010.HTC) ---
    // F = phía GIAO (kho/nhà máy HTC), T = phía NHẬN (đại lý). Mã trạng thái theo TConst.Stage: P/A.
    // Duyệt phía F KHÔNG tự chốt phía T — nguồn cố ý comment dòng cập nhật TDlvMnStatus (--20131126).

    /// <summary>Trạng thái duyệt phía giao (FDLVMNSTATUS): P = chờ duyệt, A = đã duyệt.</summary>
    public string FDlvMnStatus { get; set; } = "P";

    /// <summary>Trạng thái duyệt phía nhận (TDLVMNSTATUS): P = chờ duyệt, A = đã duyệt.</summary>
    public string TDlvMnStatus { get; set; } = "P";

    public DateTime? FApprovedDate { get; set; }
    public string? FApprovedBy { get; set; }
    public DateTime? TApprovedDate { get; set; }
    public string? TApprovedBy { get; set; }

    /// <summary>Yêu cầu vận chuyển gắn với biên bản (TRANSPREQNO / TRANSPREQTYPE).</summary>
    public string? TranspReqNo { get; set; }
    public string? TranspReqType { get; set; }

    /// <summary>Kho + địa chỉ hai đầu tuyến (FSTORAGECODE/TSTORAGECODE, FADDRESS/TADDRESS).</summary>
    public string? FStorageCode { get; set; }
    public string? TStorageCode { get; set; }
    public string? FAddress { get; set; }
    public string? TAddress { get; set; }

    /// <summary>Ngày xuất kho / ngày giao đến (DLVSTARTDATE / DLVENDDATE).</summary>
    public DateTime? DlvStartDate { get; set; }
    public DateTime? DlvEndDate { get; set; }

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

/// <summary>Tài khoản SMS trả trước (số dư) — port 1:1 FrmSMSAccountMng (TblSMS_Account, TCMotor).</summary>
public sealed class SmsAccount
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AccountName { get; set; } = "";
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    /// <summary>Tên file đính kèm dùng chung cả lô (AttachmentName).</summary>
    public string? AttachmentName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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

    /// <summary>Thành tiền = <see cref="UnitPrice"/> × <see cref="MsgParts"/>.</summary>
    public decimal Cost { get; set; }

    /// <summary>Số lần đã thử gửi — nguồn giới hạn `SMSTryCountMax = 1` (KHÔNG tự gửi lại).</summary>
    public int TryCount { get; set; }

    public DateTime SendDate { get; set; } = DateTime.Now;
}

/// <summary>Mẫu email theo loại nghiệp vụ (tiêu đề + nội dung + file đính kèm) — port 1:1 FrmEmail_TempEmailCreate (TblEmail_TempEmail, TCMotor).</summary>
public sealed class EmailTemplate
{
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
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SmsType { get; set; } = "";   // loại: MAINT/BIRTHDAY/PROMO/...
    public string? SmsName { get; set; }
    public string SmsBody { get; set; } = "";
    public string FlagActive { get; set; } = "1";
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
    public string FlagActive { get; set; } = "1";
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

    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Danh mục phụ tùng dịch vụ (master lõi) — port 1:1 FrmPart (TblSerMSTPart, TCMotor).</summary>
public sealed class ServicePart
{
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
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public decimal QuantityShare { get; set; }  // SL sẵn sàng chia sẻ
    public string? Remark { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Closed
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Thông báo kỹ thuật (bulletin) — số/nội dung/PT-DV liên quan/hết hạn/file — port 1:1 FrmBulletinHTCCreate (Tbl_Blt_Bulletin, TCMotor).</summary>
public sealed class Bulletin
{
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

    /// <summary>Thành tiền TRƯỚC thuế, đã nhân hệ số (nguồn trả cột AmountBeforeVAT).</summary>
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
    public string FlagActive { get; set; } = "1";
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
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
