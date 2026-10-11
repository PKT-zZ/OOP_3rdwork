/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;
 
//may tinh luon co kha nang ket noi mang
public class Computer : Device, INetworkable
{
    private string _ipAddress = string.Empty;
 
    public Computer(
        string deviceId, string deviceName, int yearOfUse, decimal purchasePrice,
        int ramGb, string processor, bool hasDedicatedGpu,
        DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUse, purchasePrice, status)
    {
        if (ramGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(ramGb), ramGb, "Dung lượng RAM phải lớn hơn 0.");
        if (string.IsNullOrWhiteSpace(processor))
            throw new ArgumentException("Loại bộ xử lý không được rỗng.", nameof(processor));
 
        RamGb = ramGb;
        Processor = processor.Trim();
        HasDedicatedGpu = hasDedicatedGpu;
    }
 
    public int RamGb { get; }
    public string Processor { get; }
    public bool HasDedicatedGpu { get; }
 
    //5% gia mua, +2% neu co GPU roi, +1% neu dung > 5 nam
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal rate = 0.05m;
        if (HasDedicatedGpu) rate += 0.02m;
        if (IsOlderThanFiveYears) rate += 0.01m;
        return PurchasePrice * rate;
    }
 
    //phan ket noi mang theo INetworkable
    public string IpAddress => _ipAddress;
 
    public bool IsConnected { get; private set; }
 
    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("Địa chỉ IP không được rỗng.", nameof(ipAddress));
        if (IsConnected)
            throw new InvalidOperationException($"Thiết bị {DeviceId} đang kết nối ({_ipAddress}), không thể kết nối lại.");
 
        _ipAddress = ipAddress.Trim();
        IsConnected = true;
    }
 
    public void Disconnect()
    {
        IsConnected = false;
        _ipAddress = string.Empty;
    }
 
    public override string ToString() =>
        $"{base.ToString()} | RAM {RamGb} GB, {Processor}, GPU rời: {(HasDedicatedGpu ? "có" : "không")} | " +
        (IsConnected ? $"Mạng: {_ipAddress}" : "Mạng: chưa kết nối");
}
 
//may in thuong ko co hanh vi mang
public class Printer : Device
{
    private const decimal BaseRate = 0.04m;
    private const int HeavyUsePageLimit = 100_000;
    private const decimal HeavyUseSurcharge = 500_000m;
    private const decimal ColorSurcharge = 300_000m;
 
    public Printer(
        string deviceId, string deviceName, int yearOfUse, decimal purchasePrice,
        PrinterType printerType, int printedPages, bool isColor,
        DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUse, purchasePrice, status)
    {
        if (printedPages < 0)
            throw new ArgumentOutOfRangeException(nameof(printedPages), printedPages, "Số trang đã in không được âm.");
 
        PrinterType = printerType;
        PrintedPages = printedPages;
        IsColor = isColor;
    }
 
    public PrinterType PrinterType { get; }
    public int PrintedPages { get; }
 
    //can thuoc tinh nay de tinh phu phi may in mau theo de
    public bool IsColor { get; }
  
    //4% gia mua, +500.000 neu > 100.000 trang, +300.000 neu in mau
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = PurchasePrice * BaseRate;
        if (PrintedPages > HeavyUsePageLimit) cost += HeavyUseSurcharge;
        if (IsColor) cost += ColorSurcharge;
        return cost;
    }
 
    public override string ToString() =>
        $"{base.ToString()} | {PrinterType}, {PrintedPages:N0} trang, {(IsColor ? "màu" : "đen trắng")}";
}
 
//tach rieng may in co kha nang ket noi mang
public class NetworkPrinter : Printer, INetworkable
{
    private string _ipAddress = string.Empty;
 
    public NetworkPrinter(
        string deviceId, string deviceName, int yearOfUse, decimal purchasePrice,
        PrinterType printerType, int printedPages, bool isColor,
        DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUse, purchasePrice, printerType, printedPages, isColor, status)
    {
    }
  
    //phan ket noi mang theo INetworkable
    public string IpAddress => _ipAddress;
 
    public bool IsConnected { get; private set; }
 
    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            throw new ArgumentException("Địa chỉ IP không được rỗng.", nameof(ipAddress));
        if (IsConnected)
            throw new InvalidOperationException($"Thiết bị {DeviceId} đang kết nối ({_ipAddress}), không thể kết nối lại.");
 
        _ipAddress = ipAddress.Trim();
        IsConnected = true;
    }
 
    public void Disconnect()
    {
        IsConnected = false;
        _ipAddress = string.Empty;
    }
 
    public override string ToString() =>
        $"{base.ToString()} | " + (IsConnected ? $"Mạng: {_ipAddress}" : "Mạng: chưa kết nối");
}
 
//may chieu ko co yeu cau ket noi mang
public class Projector : Device
{
    private const decimal BaseRate = 0.03m;
    private const int LampHoursLimit = 3_000;
    private const decimal LampSurcharge = 1_500_000m;
 
    public Projector(
        string deviceId, string deviceName, int yearOfUse, decimal purchasePrice,
        int brightnessLumens, int lampHours,
        DeviceStatus status = DeviceStatus.Active)
        : base(deviceId, deviceName, yearOfUse, purchasePrice, status)
    {
        if (brightnessLumens <= 0)
            throw new ArgumentOutOfRangeException(nameof(brightnessLumens), brightnessLumens, "Độ sáng phải lớn hơn 0.");
        if (lampHours < 0)
            throw new ArgumentOutOfRangeException(nameof(lampHours), lampHours, "Số giờ dùng bóng đèn không được âm.");
 
        BrightnessLumens = brightnessLumens;
        LampHours = lampHours;
    }
 
    public int BrightnessLumens { get; }
    public int LampHours { get; }
 
    //3% gia mua, +1.500.000 neu bong den > 3.000 gio
    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = PurchasePrice * BaseRate;
        if (LampHours > LampHoursLimit) cost += LampSurcharge;
        return cost;
    }
 
    public override string ToString() =>
        $"{base.ToString()} | {BrightnessLumens:N0} lumen, bóng đèn {LampHours:N0} giờ";
}