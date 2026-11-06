using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Common
{
    public record Result(bool IsSuccess, string? ErrorMessage = null,ResultKind kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(true);
        public static Result Fail(string errorMessage, ResultKind kind = ResultKind.Conflict) => new Result(false, errorMessage, kind);

        public static Result NotFound(string errorMessage = "Not Found") => new Result(false, errorMessage, ResultKind.NotFound);

        public static Result ValidationFailed(string errorMessage = "Validation Failed") => new Result(false, errorMessage, ResultKind.VailedationFailed);

    }

    public record Result<T>(bool IsSuccess, T? Data = default, string? ErrorMessage = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T data) => new Result<T>(true, data);
        public static Result<T> Fail(string errorMessage , ResultKind kind = ResultKind.Conflict) => new Result<T>(false, default, errorMessage, kind);

        public static Result<T> NotFound(string errorMessage = "Not Found") => new Result<T>(false, default, errorMessage, ResultKind.NotFound);
    }

    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        VailedationFailed,
        Forbidden,
    }
}
