namespace talabatsApi.Helper
{
    public class pagination<T>
    {
        public int pageindex { get; set; } = 1;
        public int Pagesize { get; set; } = 10;
        public int count { get; set; }
        public IReadOnlyList<T> data { get; set; } 

    }
}
