/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

using System.Text;

namespace DeviceManagement;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int now = DateTime.Now.Year;

        //1. KHỞI TẠO THIẾT BỊ
        PrintHeader("1. KHỞI TẠO THIẾT BỊ");

        var c01 = new Computer("C01", "Dell Precision 3660", now - 6, 20_000_000m,
            ramGb: 32, processor: "Intel Core i7", hasDedicatedGpu: true);
        var c02 = new Computer("C02", "HP ProDesk 400", now - 2, 15_000_000m,
            ramGb: 16, processor: "Intel Core i5", hasDedicatedGpu: false);

        var p01 = new Printer("P01", "HP LaserJet Pro", now - 3, 10_000_000m,
            PrinterType.Laser, printedPages: 20_000, isColor: false, DeviceStatus.UnderMaintenance);
        var np01 = new NetworkPrinter("NP01", "Canon PIXMA G4770", now - 4, 12_000_000m,
            PrinterType.Inkjet, printedPages: 120_001, isColor: true);

        var pj01 = new Projector("PJ01", "Epson EB-X49", now - 1, 25_000_000m,
            brightnessLumens: 3_600, lampHours: 3_500);

        Device[] allDevices = { c01, c02, p01, np01, pj01 };
        foreach (Device device in allDevices)
            Console.WriteLine($"  {device}");

        //2. THÊM THIẾT BỊ VÀO PHÒNG
        PrintHeader("2. THÊM THIẾT BỊ VÀO PHÒNG");

        var labA = new LabRoom("LAB-A", "Phòng thực hành A", 30);
        var labB = new LabRoom("LAB-B", "Phòng thực hành B", 25);

        labA.AddDevice(c01);
        labA.AddDevice(p01);
        labA.AddDevice(pj01);
        labB.AddDevice(c02);
        labB.AddDevice(np01);

        Console.WriteLine($"  Đã thêm thiết bị vào {labA.RoomId} và {labB.RoomId}.");

        //3. KIỂM TRA TRÙNG MÃ
        PrintHeader("3. KIỂM TRA TRÙNG MÃ");

        try
        {
            labA.AddDevice(new Computer("C01", "Máy trùng mã", now - 1,
                9_000_000m, 8, "Intel Core i3", false));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  {ex.Message}");
        }

        //4. DANH SÁCH THIẾT BỊ THEO PHÒNG
        PrintHeader("4. DANH SÁCH THIẾT BỊ THEO PHÒNG");

        LabRoom[] rooms = { labA, labB };
        foreach (LabRoom room in rooms)
        {
            Console.WriteLine($"  {room}");
            foreach (Device device in room.Devices)
                Console.WriteLine($"    {device}");
        }

        //5. CHI PHÍ BẢO TRÌ
        PrintHeader("5. CHI PHÍ BẢO TRÌ");

        foreach (LabRoom room in rooms)
        {
            Console.WriteLine($"  {room.RoomId}:");
            foreach (Device device in room.Devices)
            {
                decimal cost = device.CalculateAnnualMaintenanceCost();
                Console.WriteLine($"    {device.DeviceId}: {Device.FormatMoney(cost)}");
            }

            Console.WriteLine($"  Tổng: {Device.FormatMoney(room.CalculateAnnualMaintenanceCost())}");
        }

        //6. THIẾT BỊ CẦN BẢO TRÌ
        PrintHeader("6. THIẾT BỊ CẦN BẢO TRÌ");

        foreach (LabRoom room in rooms)
        {
            Console.WriteLine($"  {room.RoomId}:");
            List<Device> needMaintenance = room.GetDevicesRequiringMaintenance();

            if (needMaintenance.Count == 0)
            {
                Console.WriteLine("    Không có");
                continue;
            }

            foreach (Device device in needMaintenance)
                Console.WriteLine($"    {device.DeviceId} - {device.DeviceName}");
        }

        //7. KẾT NỐI MẠNG QUA INETWORKABLE
        PrintHeader("7. KẾT NỐI MẠNG QUA INETWORKABLE");

        List<INetworkable> networkDevices = new() { c01, c02, np01 };
        string[] ipAddresses = { "192.168.1.10", "192.168.1.11", "192.168.1.12" };

        for (int i = 0; i < networkDevices.Count; i++)
        {
            INetworkable device = networkDevices[i];
            device.Connect(ipAddresses[i]);
            Console.WriteLine($"  {device.GetType().Name}: {device.IpAddress}, IsConnected = {device.IsConnected}");
        }

        foreach (INetworkable device in networkDevices)
            device.Disconnect();

        Console.WriteLine("  Đã ngắt kết nối các thiết bị mạng.");
    }

    private static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"===== {title} =====");
    }
}