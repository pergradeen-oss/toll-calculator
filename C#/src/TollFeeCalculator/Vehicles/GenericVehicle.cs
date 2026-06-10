namespace TollFeeCalculator.Vehicles;

public sealed class GenericVehicle(VehicleType type) : IVehicle
{
    public VehicleType Type { get; } = type;
}
