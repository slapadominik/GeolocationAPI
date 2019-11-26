namespace GeolocationAPI.DTO.Resources
{
    public abstract class Resource<T> where T : struct
    {
        public T Id { get; set; }
    }
}