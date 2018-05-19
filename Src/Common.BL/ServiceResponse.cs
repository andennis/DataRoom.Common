namespace Common.BL
{
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            Status = ServiceResponseStatus.Success;
        }
        public ServiceResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResponse<TData> : ServiceResponse
    {
        public ServiceResponse(TData data)
        {
            Data = data;
        }
        public TData Data { get; }
    }
}
