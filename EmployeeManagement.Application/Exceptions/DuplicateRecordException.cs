namespace EmployeeManagement.Application.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException(string message)
            : base(message)
        {
        }
    }
}