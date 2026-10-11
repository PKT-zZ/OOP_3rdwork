/****************/ 
// MSSV: 202418996
// Họ tên: Nguyễn Khánh Toàn
/****************/

namespace DeviceManagement;

//phong giu tham chieu den Device nen day la quan he ket tap
public class LabRoom
{
    private readonly List<Device> _devices = new();

    public LabRoom(string roomId, string roomName, int capacity)
    {
        if (string.IsNullOrWhiteSpace(roomId))
            throw new ArgumentException("Mã phòng không được rỗng.", nameof(roomId));
        if (string.IsNullOrWhiteSpace(roomName))
            throw new ArgumentException("Tên phòng không được rỗng.", nameof(roomName));
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Sức chứa phải lớn hơn 0.");

        RoomId = roomId.Trim();
        RoomName = roomName.Trim();
        Capacity = capacity;
    }

    public string RoomId { get; }
    public string RoomName { get; }
    public int Capacity { get; }
    public IReadOnlyList<Device> Devices => _devices;

    public void AddDevice(Device device)
    {
        if (device == null)
            throw new ArgumentNullException(nameof(device));

        if (_devices.Any(d => d.DeviceId == device.DeviceId))
            throw new InvalidOperationException($"Phòng {RoomId} đã có thiết bị mã '{device.DeviceId}'.");

        _devices.Add(device);
    }

    public bool RemoveDevice(string deviceId)
    {
        Device? device = FindDevice(deviceId);
        if (device == null)
            return false;

        _devices.Remove(device);
        return true;
    }

    public Device? FindDevice(string deviceId) =>
        _devices.FirstOrDefault(d => d.DeviceId == deviceId);

    //goi da hinh tren Device, ko can kiem tra tung loai
    public decimal CalculateAnnualMaintenanceCost() =>
        _devices.Sum(d => d.CalculateAnnualMaintenanceCost());

    //thiet bi dang bao tri hoac da dung > 5 nam
    public List<Device> GetDevicesRequiringMaintenance() =>
        _devices
            .Where(d => d.Status == DeviceStatus.UnderMaintenance || d.IsOlderThanFiveYears)
            .ToList();

    public override string ToString() =>
        $"[{RoomId}] {RoomName} | Sức chứa: {Capacity} | Số thiết bị: {_devices.Count}";
}