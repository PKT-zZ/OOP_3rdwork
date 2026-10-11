/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Globalization;

namespace DeviceManagement;
public enum DeviceStatus
{
    Active,
    UnderMaintenance,
    Retired
}

//loai may in theo de:laser/phun
public enum PrinterType
{
    Laser,
    Inkjet
}

//nang luc ket noi mang
public interface INetworkable
{
    string IpAddress { get; }
    void Connect(string ipAddress);
    void Disconnect();
    bool IsConnected { get; }
}

//lop truu tuong chua phan chung cua moi thiet bi
public abstract class Device
{
    //nguong sdung >5y
    private const int MaintenanceAgeLimitYears = 5;
 
    protected Device(string deviceId, string deviceName, int yearOfUse, decimal purchasePrice, DeviceStatus status)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            throw new ArgumentException("Mã thiết bị không được rỗng.", nameof(deviceId));
        if (string.IsNullOrWhiteSpace(deviceName))
            throw new ArgumentException("Tên thiết bị không được rỗng.", nameof(deviceName));
        if (purchasePrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(purchasePrice), purchasePrice, "Giá mua phải lớn hơn 0.");
 
        int currentYear = DateTime.Now.Year;
        if (yearOfUse <= 0 || yearOfUse > currentYear)
            throw new ArgumentOutOfRangeException(
                nameof(yearOfUse), yearOfUse, $"Năm đưa vào sử dụng phải từ 1 đến {currentYear}.");
 
        //Ma thiet bi chi dc gan o day nen ko doi dc sau khi tao
        DeviceId = deviceId.Trim();
        DeviceName = deviceName.Trim();
        YearOfUse = yearOfUse;
        PurchasePrice = purchasePrice;
        Status = status;
    }
 
    public string DeviceId { get; }
    public string DeviceName { get; }
    public int YearOfUse { get; }
    public decimal PurchasePrice { get; }
    public DeviceStatus Status { get; set; }

    //So nam da sdung tinh theo nam hien tai
    public int YearsInUse => DateTime.Now.Year - YearOfUse;
 
    public bool IsOlderThanFiveYears => YearsInUse > MaintenanceAgeLimitYears;
 
    //Chi phi bao tri du kien trong 1 nam, moi loai lai tuy cong thuc
    public abstract decimal CalculateAnnualMaintenanceCost();
 
    //dinh dang tien kieu 36.000.000 đ
    public static string FormatMoney(decimal amount) =>
        amount.ToString("N0", CultureInfo.InvariantCulture).Replace(',', '.') + " đ";
 
    public override string ToString() =>
        $"[{DeviceId}] {GetType().Name} {DeviceName} | Năm {YearOfUse} ({YearsInUse} năm) | " +
        $"Giá mua {FormatMoney(PurchasePrice)} | {Status}";
}