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
