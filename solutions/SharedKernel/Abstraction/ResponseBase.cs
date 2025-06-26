using System.Text.Json.Serialization;
using FluentValidation.Results;
 
namespace SharedKernel;

public class Response<T> where T : class 
{
    public string Status { get; private set; } 
    public T? Data { get; private set; }
    public List<Error> Errors {get;set;} = default!;

    private Response()
    {
        Errors =  new List<Error>();
        Status = ResponseStatus.ERROR;
    }

    [JsonConstructor]
    public Response(T? Data)
    {
        Status = ResponseStatus.OK;
        this.Data = Data;
    }

    private Response(T? Data, string test="")
    {
        Status = ResponseStatus.OK;
        this.Data = Data;
    }

    #region  OK RESPONSE

    public static Response<T> OK(T data)
    {
        return new(data);
    }

    #endregion

    #region  ERROR RESPONSE

    public static Response<T> ValidationError(ValidationResult validationResult)
    {
        Response<T> response = new();
        foreach(var error in validationResult.Errors)
            response.Errors.Add(new Error("", error.ErrorMessage));
                
        return response;
    }
    public static Response<T> ValidationError(Error[] errors)
    {
        Response<T> response = new();
        response.Errors.AddRange(errors);                
        return response;
    }



    public static Response<T> Error(Exception ex)
    {
        Response<T> response = new();
        response.Errors.Add(new Error("", ex.GetAllExceptions()));
        
        return response;
    }


    public static Response<T> Error(string Message, string Code = null, object ReferenceObject = null)
    {
        Response<T> response = new();
        response.Errors.Add(new Error(Code, Message));        
        return response;
    }

    #endregion


    public static implicit operator Response<T>(Error error) => Error(error.Message, error.Code);
    public static implicit operator Response<T>(T Value) => new(Value);
    public static implicit operator Response<T>(string error) => Error(error);

}