namespace BRSS.ManageStudent.Domain.Exception;

public class EmailNotConfirmedException : System.Exception
{
    public int ErrorCode { get; set; }

    public EmailNotConfirmedException(){}

    public EmailNotConfirmedException(int errorCode)
    {
        ErrorCode = errorCode;
    }
    public EmailNotConfirmedException(string message) : base(message) { }

    public EmailNotConfirmedException(string message, int errorCode) 
        : base(message) 
    { 
        ErrorCode = errorCode; 
    }
}