namespace TollFeeCalculator.Policies;

public interface ITollFeePolicy
{
    int GetFee(DateTime date);
}
