namespace talabatsApi.Error
{
    public class ApiValidationError : ApiResponse
    {
       public List<string> errors;
        public ApiValidationError ():base(400)
        {
            this.errors = new List<string>();
        }

    }
}
