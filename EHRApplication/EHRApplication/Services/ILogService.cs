namespace EHRApplication.Services
{
    public interface ILogService
    {
        void WriteToDatabase(string severity, string message, string context);
    }
}
