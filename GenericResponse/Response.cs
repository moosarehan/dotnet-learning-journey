namespace myfirstrestapi.GenericResponse
{
    public class Response<T>
    {
        public T? data { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }  
        public static Response<T> success(T? data,string msg)
        {
            return new Response<T>
            {
                data = data,
                Status = true,
                Message = msg
            };
        }

        public static Response<T> failure(T?data, string msg)
        {
            return new Response<T>
            {
                data = data,
                Status = false,
                Message = msg
            };
        }
    }
}
