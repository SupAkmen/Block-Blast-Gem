namespace System
{
    public class Event<T>
    {
        private event Action<T> _event;
        
        public void Subcribe(Action<T> subcriber)
        {
            _event += subcriber;
        }
        
        public void Unsubcribe(Action<T> subcriber)
        {
            _event -= subcriber;
        }

        public void Invoke(T arg)
        {
            _event?.Invoke(arg);
        }
    }
}