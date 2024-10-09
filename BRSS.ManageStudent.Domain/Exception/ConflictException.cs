namespace BRSS.ManageStudent.Domain.Exception;

public class ConflictException: System.Exception
{
    public int ErrorCode { get; set; }
    public ConflictException() { }
    public ConflictException(int errorCode)
    {
        ErrorCode = errorCode;
    }
    public ConflictException(string message) : base(message) { }
    public ConflictException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}