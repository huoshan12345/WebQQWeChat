namespace WebTest.Models
{
    public class DataResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
    }

    public class DataResult<T> : DataResult
    {
        public T Data { get; set; }
    }
}
