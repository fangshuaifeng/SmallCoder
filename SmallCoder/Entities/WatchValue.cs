namespace SmallCoder.Entities
{
    public class WatchValue<T> where T : struct
    {
        public delegate void Change(T old, T now);
        public event Change Update;

        private T _content = default;

        public T Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (!_content.Equals(value))
                {
                    Update(_content, value);
                }
                _content = value;
            }
        }
    }
}
