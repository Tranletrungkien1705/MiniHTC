import requests, subprocess, time, os, sqlite3, sys

print("1. Start SSO Stub on 8147")
sso = subprocess.Popen(["python", "D:/idocNet/_labs/labs-audit/tools/sso_stub.py", "--port", "8147"])
time.sleep(2)

print("2. Seed SQLite verify.db")
if os.path.exists("verify_r77.db"): os.remove("verify_r77.db")
conn = sqlite3.connect("verify_r77.db")
c = conn.cursor()
c.execute("CREATE TABLE Orgs (Id TEXT PRIMARY KEY, Name TEXT, ApiKey TEXT, CreatedAt TEXT)")
c.execute("CREATE TABLE Areas (Id INTEGER PRIMARY KEY, OrgId TEXT, AreaCode TEXT, AreaName TEXT, AreaRootCode TEXT, Status TEXT, Level INTEGER, CreatedAt TEXT)")
c.execute("CREATE TABLE SalesManViolates (Id INTEGER PRIMARY KEY, OrgId TEXT, SalesManCode TEXT, SalesManName TEXT, DealerCode TEXT, ViolateTypeId TEXT, ViolateNumber INTEGER, CreatedAt TEXT, ViolateDateStart TEXT, ViolateDateEnd TEXT, PhoneNo TEXT, Remark TEXT, IdentityCardNo TEXT, SmDateOfBirth TEXT, SMType TEXT, LogLUDateTime TEXT, LogLUBy TEXT)")
c.execute("CREATE TABLE SalesMen (Id INTEGER PRIMARY KEY, OrgId TEXT, SalesManCode TEXT, SalesManName TEXT, DealerCode TEXT, DepartmentCode TEXT, SalesType TEXT, Status TEXT, SMHyundaiCode TEXT, Phone TEXT, Email TEXT, Gender TEXT, DateOfBirth TEXT, Address TEXT, ProvinceCode TEXT, QualificationCode TEXT, Specialized TEXT, YearExperience TEXT, StartDate TEXT, EndDate TEXT, Position TEXT, PositionCode TEXT, CertificateCode TEXT)")
c.execute("CREATE TABLE SalesManTypes (Id INTEGER PRIMARY KEY, OrgId TEXT, DepartmentCode TEXT, SMType TEXT, SMTypeName TEXT, FlagActive TEXT, UpdatedAt TEXT, FlagEmail TEXT, LogLUDate TEXT, LogLUBy TEXT)")
c.execute("CREATE TABLE MstViolateTypes (Id INTEGER PRIMARY KEY, OrgId TEXT, ViolateTypeId TEXT, ViolateTypeName TEXT, FlagActive TEXT)")
c.execute("CREATE TABLE DealerDeals (Id INTEGER PRIMARY KEY, OrgId TEXT, DealNo TEXT, DealerCode TEXT, FlagInitDeal TEXT)")
c.execute("CREATE TABLE DealerDealDetails (Id INTEGER PRIMARY KEY, OrgId TEXT, DealId INTEGER, CarId TEXT, PlateNo TEXT)")
c.execute("CREATE TABLE CarVinMasters (Id INTEGER PRIMARY KEY, OrgId TEXT, VIN TEXT, CQStartDate TEXT, CQEndDate TEXT, StoreDate TEXT, CODate TEXT, LogLUDateTime TEXT)")
c.execute("CREATE TABLE DlrContractDetails (Id INTEGER PRIMARY KEY, OrgId TEXT)")

uid = "00000000-0000-0000-0000-000000000000"
c.execute("INSERT INTO Orgs (Id, Name, ApiKey, CreatedAt) VALUES (?, 'HTC', 'demo-htc', '2026-01-01')", (uid,))
c.execute("INSERT INTO Areas (Id, OrgId, AreaCode, AreaName, Status) VALUES (1, ?, 'MB', 'Mien Bac', '1')", (uid,))

c.execute("INSERT INTO SalesMen (OrgId, SalesManCode, SalesManName, DealerCode, DepartmentCode, SalesType, Status, SMHyundaiCode) VALUES (?, 'SM01', 'Kien', 'HTC', 'DEP01', 'T1', '1', 'HD01')", (uid,))
c.execute("INSERT INTO SalesManTypes (OrgId, DepartmentCode, SMType, SMTypeName, FlagActive, UpdatedAt, FlagEmail) VALUES (?, 'DEP01', 'T1', 'Sale Chuyen Nghiep', '1', '2026-01-01', '1')", (uid,))
c.execute("INSERT INTO MstViolateTypes (OrgId, ViolateTypeId, ViolateTypeName, FlagActive) VALUES (?, 'VV', 'Vinh Vien', '1')", (uid,))
c.execute("INSERT INTO SalesManViolates (OrgId, SalesManCode, DealerCode, ViolateTypeId, ViolateNumber, CreatedAt) VALUES (?, 'SM01', 'HTC', 'VV', 1, '2026-01-01')", (uid,))

c.execute("INSERT INTO DealerDeals (Id, OrgId, DealNo, DealerCode, FlagInitDeal) VALUES (1, ?, 'D01', 'HTC', '1')", (uid,))
c.execute("INSERT INTO DealerDealDetails (OrgId, DealId, CarId, PlateNo) VALUES (?, 1, 'VIN01', '29A-123.45')", (uid,))
c.execute("INSERT INTO CarVinMasters (OrgId, VIN, CQStartDate, CQEndDate, StoreDate, CODate, LogLUDateTime) VALUES (?, 'VIN01', '2026-01-01', '2026-01-02', '2026-01-03', '2026-01-04', '2026-01-01')", (uid,))

conn.commit()
conn.close()

print("3. Start MiniHTC on 8195")
env = os.environ.copy()
env["PORT"] = "8195"
env["SSO_AUTHORITY"] = "http://127.0.0.1:8147"
env["ConnectionStrings__DefaultConnection"] = "Data Source=verify_r77.db"
app = subprocess.Popen(["C:/dotnet-sdk8/dotnet.exe", "bin/Release/net8.0/MiniHTC.dll"], env=env)
time.sleep(5)

try:
    print("4. Get token")
    r = requests.get("http://127.0.0.1:8147/token?sub=kien&name=Kien&role=admin")
    tok = r.text

    print("5. Verify GET /api/salesmanviolates")
    r = requests.get("http://127.0.0.1:8195/api/salesmanviolates", headers={"Authorization": f"Bearer {tok}"})
    assert r.status_code == 200, f"Failed: {r.text}"
    j = r.json()
    assert j["count"] == 1
    item = j["items"][0]
    print("item /api/salesmanviolates:", item)
    assert item["msmt_SMTypeName"] == "Sale Chuyen Nghiep"
    assert item["mvt_ViolateTypeName"] == "Vinh Vien"

    print("6. Verify GET /api/smviolates")
    r = requests.get("http://127.0.0.1:8195/api/smviolates", headers={"Authorization": f"Bearer {tok}"})
    assert r.status_code == 200, f"Failed: {r.text}"
    j = r.json()
    assert j["count"] == 1
    item = j["items"][0]
    print("item /api/smviolates:", item)
    assert item.get("smTypeName", item.get("SMTypeName")) == "Sale Chuyen Nghiep"
    assert item.get("violateTypeName", item.get("ViolateTypeName")) == "Vinh Vien"

    print("7. Verify GET /api/dealerdeals/search")
    r = requests.get("http://127.0.0.1:8195/api/dealerdeals/search?flagInitDeal=1", headers={"Authorization": f"Bearer {tok}"})
    assert r.status_code == 200, f"Failed: {r.text}"
    j = r.json()
    assert j["count"] == 1
    item = j["items"][0]
    print("item /api/dealerdeals/search:", item)
    assert "2026-01-02" in item["cvCQEndDate"]
    assert "2026-01-01" in item["cvCQStartDate"]
    
    print("ALL PASSED 100%")
finally:
    sso.kill()
    app.kill()
