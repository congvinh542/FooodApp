namespace ClickBuy_Api.Service.Helpers
{
    public class DataResult<T>
    {
        public T? Entity { get; set; } = default(T);
        public List<T>? Items { get; set; } = new List<T>();
        public List<String>? Errors { get; set; } = new List<String>();
        public Boolean? Success => !Errors?.Any();
        public Int32? TotalRecords { get; set; } = 0;
    }
}
