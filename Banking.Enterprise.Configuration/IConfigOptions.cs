namespace Banking.Enterprise.Configuration
{
    public interface IConfigOptions<T> where T : class
    {
        T Value { get; }
    }

}
