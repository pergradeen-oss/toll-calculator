using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Policies;

public interface IVehicleExemptionPolicy
{
    bool IsExempt(IVehicle vehicle);
}
