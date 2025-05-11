
namespace talabatsApi.Error
{
    public class ApiResponse 
    {
        public int  status {  get; set; }
      
        public string ? message { get; set; }
        public ApiResponse(int status, string? message = null)
        {
            this.status = status;
            this.message = message??getdefautmassage(status);
        }

        private string? getdefautmassage(int? status)
        {
            return status switch
            {
                400=>"bad request",
                401 => "you are not authorized ",
                404=> "Not found",
                500 =>"Internal server error",
                _=>null
            };
        }
    }
}
