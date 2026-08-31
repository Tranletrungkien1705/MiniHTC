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
