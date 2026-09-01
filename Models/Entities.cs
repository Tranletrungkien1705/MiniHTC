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
}

/// <summary>Bản ghi tính giá vốn bình quân phụ tùng (mỗi lần tính = 1 snapshot) — port 1:1 FrmPartCostManagement/FrmCaluCost/FrmReportHistoryCost (Tbl_Ser_PartCost_Calculate, TCMotor).</summary>
public sealed class PartCostSnapshot
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string PartCode { get; set; } = "";
    public string? PartName { get; set; }
    public decimal AverageCost { get; set; }
    public decimal TotalQty { get; set; }
    public decimal TotalValue { get; set; }
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
    public string? HtcNote { get; set; }
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
    public string GuaranteeType { get; set; } = "0";     // 0=NH giam sat, 1=NH phat hanh
    public int Term { get; set; }                         // ky han (thang)
    public DateTime? DateOpen { get; set; }
    public DateTime? DateExpired { get; set; }
    public DateTime? DateEnd { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Draft";        // Draft -> Approved / Rejected
    public string FlagSettled { get; set; } = "0";        // 1 = da tat toan
    public string Remark { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
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
}

/// <summary>Lệnh xuất xe phía ngân hàng xác nhận (DO) — port 1:1 FrmBankDO. Header.</summary>
public sealed class BankDeliveryOrder
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string DONo { get; set; } = "";
    public string DealerCode { get; set; } = "";
    public string SOCode { get; set; } = "";
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
    public string OS_HDDT_InvoiceCode { get; set; } = "";  // ma HDDT
    public string InvoiceAdjType { get; set; } = "";       // loai dieu chinh (rong=goc)
    public string RootHTCInvoiceNo { get; set; } = "";     // HD goc (khi la HD dieu chinh)
    public string VatHTCStatus { get; set; } = "Draft";    // Draft -> Issued -> Deleted
    public string DeleteReason { get; set; } = "";
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
    public string Status { get; set; } = "Draft";        // Draft -> Approved / Cancelled
    public DateTime? MortageDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
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
    public string ConfirmStatus { get; set; } = "Pending";   // Pending -> Confirmed
    public string Remark { get; set; } = "";
    public DateTime? ConfirmDate { get; set; }
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

/// <summary>Đơn giá thiết bị GPS theo hợp đồng — port 1:1 FrmMst_UnitPriceGPS (Mst_UnitPriceGPS).</summary>
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
    public string Status { get; set; } = "Sent";   // Sent | Invalid
    public DateTime SendDate { get; set; } = DateTime.Now;
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
    public string Status { get; set; } = "Sent";   // Sent | Invalid
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
}

/// <summary>Phiếu nhập kho phụ tùng dịch vụ (header) — port 1:1 FrmSerInventoryAccStockIn (TblSerInvStockIn, TCMotor).</summary>
public sealed class ServiceStockIn
{
    public long Id { get; set; }
    public Guid OrgId { get; set; }
    public string StockInNo { get; set; } = "";
    public string? SupplierCode { get; set; }
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
    public decimal QtyNeeded { get; set; }
    public decimal QtyFulfilled { get; set; }
    public string? Note { get; set; }
    public string Status { get; set; } = "Open"; // Open -> Fulfilled
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public string? Remark { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? SerCode { get; set; }
    public string? SerName { get; set; }
    public DateTime? DateExpired { get; set; }
    public string? FileNameAttachment { get; set; }
    public string FlagActive { get; set; } = "1";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
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
    public decimal TotalAmount { get; set; }
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
    public decimal Vat { get; set; }
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
