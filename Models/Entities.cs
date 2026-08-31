namespace MiniHTC.Models;
public sealed class Org { public Guid Id { get; set; } = Guid.NewGuid(); public string Name { get; set; } = ""; public string ApiKey { get; set; } = ""; public DateTime CreatedAt { get; set; } = DateTime.Now; }

/// <summary>Khu vực (Mst_Area) — port 1:1 FrmArea (2010.HTC/TERP.HTCClient/Admin/Dealer).</summary>
public sealed class Area
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string AreaCode { get; set; } = "";
    public string AreaName { get; set; } = "";
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
    public decimal Vat { get; set; } = 10;
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Khách hàng (Mst_Customer) — port 1:1 FrmCustomerBase.</summary>
public sealed class Customer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string CustomerCode { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string? Phone { get; set; }
    public string? IdCard { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? ProvinceCode { get; set; }
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Nhân viên bán hàng (Mst_SalesMan) — port 1:1 FrmCreateSalesMan.</summary>
public sealed class SalesMan
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string SalesManCode { get; set; } = "";
    public string SalesManName { get; set; } = "";
    public string? DealerCode { get; set; }
    public string? DepartmentCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Yêu cầu kiểm tra trước giao xe PDI (Pre-Delivery Inspection) — port 1:1 FrmMngDlr_PDIRequest.</summary>
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

/// <summary>Thu hồi xe (FrmMngCarRetrieve) — thu hồi xe từ đại lý (khác triệu hồi an toàn).</summary>
public sealed class CarRetrieve
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? Reason { get; set; }
    public string Status { get; set; } = "Requested";   // Requested → Approved → Retrieved (hoặc Rejected)
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? RetrievedAt { get; set; }
}

/// <summary>Hủy xe (FrmMngCarCancel) — hủy đơn/xe theo lý do, có duyệt.</summary>
public sealed class CarCancel
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Code { get; set; } = "";
    public string Vin { get; set; } = "";
    public string? CancelTypeCode { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "Requested";   // Requested → Approved → Rejected
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

/// <summary>Đề nghị thế chấp xe (RM_ReqMortgage — port 1:1 FrmNewRM_ReqMortgage/FrmMngRM_ReqMortgage):
/// header đề nghị thế chấp lô xe cho ngân hàng. Pending(Mới tạo)→Approved(Đang thế chấp)→Finished(Đã giải chấp).</summary>
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
    public string? TransporterCode { get; set; }
    public DateTime? ExpectedDate { get; set; }
    public string Status { get; set; } = "Pending";  // Pending → Finished
    public DateTime? ApprovedDate { get; set; }
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
    public string ViolateTypeId { get; set; } = "";       // loại vi phạm (Mst_ViolateType)
    public int ViolateNumber { get; set; }                // lần vi phạm thứ n (auto +1 theo NV)
    public DateTime? ViolateDateStart { get; set; }
    public DateTime? ViolateDateEnd { get; set; }
    public string? IdentityCardNo { get; set; }
    public string? PhoneNo { get; set; }
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
    public string? Engineer { get; set; }              // kỹ thuật viên
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
    public string Status { get; set; } = "Draft";      // Draft → Posted
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PostedAt { get; set; }
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
    public string Status { get; set; } = "Draft";      // Draft → Posted
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? PostedAt { get; set; }
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
    public string Status { get; set; } = "Pending";      // Pending → Contacted → Closed
    public string? Result { get; set; }                  // kết quả liên hệ
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ContactedAt { get; set; }
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
    public string DMSStatus { get; set; } = "P";        // P → A (bên DMS đại lý)
    public string TSTStatus { get; set; } = "";         // '' → Processing → Pending → Resolved (bên TST)
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
    public decimal Amount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string Status { get; set; } = "P";          // P → A
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

/// <summary>Yêu cầu báo giá phụ tùng (Req_PartPrice — port 1:1 FrmReq_PartPrice/Mng, TCMotor DMSCarSv/TST):
/// DMS xin TST báo giá PT. DMSStatus P→A→F; TSTStatus Pending(chờ)→Quoted(đã báo giá)→Finished.</summary>
public sealed class ReqPartPrice
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string ReqNo { get; set; } = "";
    public string DMSStatus { get; set; } = "P";       // P → A → F
    public string TSTStatus { get; set; } = "Pending"; // Pending → Quoted → Finished
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

/// <summary>Kỹ thuật viên dịch vụ (Ser_Engineer — port 1:1 FrmEngineerCreate, TCMotor DMSCarSv/Admin):
/// KTV thuộc 1 nhóm sửa chữa. RO service items tham chiếu KTV.</summary>
public sealed class ServiceEngineer
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string EngineerNo { get; set; } = "";
    public string EngineerName { get; set; } = "";
    public string? GroupRCode { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "1";
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
}

/// <summary>Lệnh đặt xe từ nhà máy (POCommand — port 1:1 FrmNewHMCOrder/FrmMngHMCOrder, TCMotor DMSales.Foton):
/// đơn đặt xe tải Foton lên hãng theo tháng. Draft(Nháp)→Sent(Đã gửi hãng).</summary>
public sealed class POCommand
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PoCmdCode { get; set; } = "";
    public string OrderMonth { get; set; } = "";       // YYYYMM tháng đặt hàng
    public string Status { get; set; } = "Draft";      // Draft → Sent
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
    public string Status { get; set; } = "Draft";      // Draft → Approved1 → Approved2 → Delivered / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DeliveredAt { get; set; }
    // Duyệt lệnh giao (FrmApproveDO) — duyệt 2 cấp trước khi giao
    public DateTime? Approved1At { get; set; }
    public DateTime? Approved2At { get; set; }
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
}

/// <summary>Đề nghị làm hồ sơ đăng ký xe (Car_DocReq — port 1:1 FrmNewDocReq/FrmMngDocReq, TCMotor DMSales.Foton):
/// đề nghị làm hồ sơ đăng ký cho lô xe đã giao. Draft→Submitted(đã nộp)→Done(hoàn tất).</summary>
public sealed class DocReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DocReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string Status { get; set; } = "Draft";      // Draft → Submitted → Done
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? DoneAt { get; set; }
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
    public string Status { get; set; } = "Draft";        // Draft → Sent → Approved1 → Approved2 / Rejected
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
    public int? ApprovedQuantity { get; set; }   // SL duyệt (cấp 1)
    public DateTime? ApprovedDate { get; set; }
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
    public decimal PriceAFVAT { get; set; }              // giá sau VAT
}

/// <summary>Ngân hàng đại lý (Mst_DealerBank) — port 1:1 FrmDealerBank (2010.HTC/Admin/Product). Tài khoản/hạn mức NH của đại lý + cờ NH bảo lãnh/thanh toán.</summary>
public sealed class DealerBank
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string BankCode { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string? BankBranchCode { get; set; }
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
}

/// <summary>Biên bản giao xe (Sto_DlvMinutes) — port 1:1 FrmDealerNewDlvMinutes/FrmHTCNewDlvMinutes (2010.HTC/Sales/DlvMinutes). BB giao/vận chuyển xe: VIN, tuyến đi-đến, ĐVVT + lái xe, ngày giao + checklist tình trạng xe (JSON ~25 mục OS/IS/SP/DA).</summary>
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
    public string Status { get; set; } = "Draft"; // Draft → Done
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

/// <summary>Đề nghị giao hồ sơ (RD_ReqInvoice + Dtl) — port 1:1 FrmNewRDInvoice (2010.HTC/Sales/Redeem). Đề nghị giao hồ sơ/hóa đơn cho lô VIN.</summary>
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
    public string Status { get; set; } = "Draft"; // Draft → Approved / Rejected
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
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
    public string DlrSignStatus { get; set; } = "P";  // ký bên B (đại lý): P chờ / S đã ký
    public string HTCSignStatus { get; set; } = "P";  // ký bên A (HTC): P / S
    public string DlrCtrStatus { get; set; } = "Draft"; // Draft → Signed / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DlrApprDTime { get; set; }
    public DateTime? HTCAppr2DTime { get; set; }
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
    public string Status { get; set; } = "Draft"; // Draft → Issued (phát hành) / Cancelled
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
    public string Status { get; set; } = "Draft"; // Draft → Confirmed / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
}
public sealed class StorageRearrangeDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long StorageRearrangeId { get; set; }
    public string VIN { get; set; } = "";
    public string? StorageCodeFrom { get; set; }
    public string StorageCodeTo { get; set; } = "";
    public string? Remark { get; set; }
}

/// <summary>Đề nghị bảo hiểm (Ins_InsuranceReq + Dtl) — port 1:1 FrmNewInsuranceReq (2010.HTC/Sales/Purchase). Đề nghị mua bảo hiểm cho lô VIN theo hãng + loại hình.</summary>
public sealed class InsuranceReq
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string InsReqNo { get; set; } = "";
    public string InsCompanyCode { get; set; } = "";  // hãng BH
    public string InsTypeCode { get; set; } = "";      // loại hình BH
    public string Status { get; set; } = "Draft";      // Draft → Confirmed / Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ConfirmedAt { get; set; }
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

/// <summary>Đề nghị giải chấp (RD_ReqRedeem + Dtl) — port 1:1 FrmNewRedeem (2010.HTC/Sales/Redeem). Đại lý đề nghị giải chấp xe (release thế chấp) theo VIN.</summary>
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

/// <summary>Đăng ký xe trưng bày/test (Car_TestCar + Dtl) — port 1:1 FrmMngRegister_TestCar (2010.HTC/Sales). Đại lý đăng ký lô xe làm xe trưng bày, có duyệt.</summary>
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
    public string Status { get; set; } = "Active"; // Active → Cancelled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

/// <summary>Yêu cầu PDI của đại lý (Dlr_PDIRequest) — port 1:1 FrmNewDlr_PDIRequest (DMSales.Foton/SalesDealer). Đại lý gửi yêu cầu PDI cho danh sách xe/RO.</summary>
public sealed class DlrPdiRequest
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DlrPdiReqNo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string Status { get; set; } = "Draft";   // Draft → Done
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? DoneAt { get; set; }
}
public sealed class DlrPdiRequestDetail
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public long DlrPdiReqId { get; set; }
    public string RONo { get; set; } = "";
    public DateTime? ROCreatedDate { get; set; }
    public string? ROStatus { get; set; }
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

/// <summary>Master code/name/status generic — port 1:1 loạt Frm masters (Bank/Color/DealerType/CarCancelType/...).</summary>
public sealed class MasterItem
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string Category { get; set; } = "";   // 1 category = 1 màn Frm gốc
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Status { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
