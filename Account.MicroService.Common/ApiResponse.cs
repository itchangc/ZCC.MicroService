using System;

namespace Account.MicroService.Common
{
    /// <summary>
    /// 统一响应类
    /// </summary>
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }

        public static ApiResponse OK()
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = true,
                Message = "Success",
                Code = 200
            };
            return apiResponse;
        }

        public static ApiResponse OK(object data)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = true,
                Message = "Success",
                Code = 200,
                Data = data
            };
            return apiResponse;
        }

        public static ApiResponse OK(string message)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = true,
                Message = message,
                Code = 200
            };
            return apiResponse;
        }

        public static ApiResponse OK(object data, string message)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = true,
                Message = message,
                Code = 200,
                Data = data
            };
            return apiResponse;
        }

        public static ApiResponse Fail()
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = false,
                Message = "Fail",
                Code = -1
            };
            return apiResponse;
        }

        public static ApiResponse Fail(string message)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                Success = false,
                Message = message,
                Code = -1
            };

            return apiResponse;
        }
    }
}
