namespace Common.BL
{
    public class ServiceResponse
    {
        public ServiceResponseStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResponse<TData> : ServiceResponse
    {
        public ServiceResponse(TData data)
        {
            Status = ServiceResponseStatus.Success;
            Data = data;
        }
        public TData Data { get; }
    }
}
