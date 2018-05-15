namespace Common.WebService
{
    public class WebServiceResponse
    {
        public WebServiceResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class WebServiceResponse<TData> : WebServiceResponse
    {
        public WebServiceResponse(TData data)
        {
            Status = WebServiceResponseStatus.Success;
            Data = data;
        }
        public TData Data { get; }
    }
}
