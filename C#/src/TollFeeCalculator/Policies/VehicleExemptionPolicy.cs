using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Policies;

public sealed class VehicleExemptionPolicy : IVehicleExemptionPolicy
{
    private static readonly HashSet<VehicleType> ExemptTypes =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    ];

    public bool IsExempt(IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        return ExemptTypes.Contains(vehicle.Type);
    }
}
