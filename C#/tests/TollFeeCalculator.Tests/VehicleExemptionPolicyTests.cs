using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class VehicleExemptionPolicyTests
{
    private readonly VehicleExemptionPolicy _policy = new();

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void IsExempt_ReturnsTrue_ForExemptVehicleTypes(VehicleType type)
    {
        var vehicle = new GenericVehicle(type);

        Assert.True(_policy.IsExempt(vehicle));
    }

    [Fact]
    public void IsExempt_ReturnsFalse_ForCar()
    {
        var vehicle = new Car();

        Assert.False(_policy.IsExempt(vehicle));
    }

    [Fact]
    public void IsExempt_Throws_WhenVehicleIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => _policy.IsExempt(null!));
    }
}
