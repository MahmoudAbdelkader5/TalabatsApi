namespace talabatsApi.Error
{
    public class apiExpectionResponse : ApiResponse
    {
        public string ? details { get; set; }
        public apiExpectionResponse(int status, string? message = null, string? details = null) : base(status, message)
        {
            this.details = details;
        }
    }
}
