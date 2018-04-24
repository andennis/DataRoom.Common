namespace Common.BL
{
    public interface ISequenceGenerator<T>
    {
        T GetNextValue(string counterName);
    }
}