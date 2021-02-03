namespace Pattern
{
    public class  SingletonCommon<T> where T :  SingletonCommon<T>, new()
    {
        protected static T m_instance = default;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new T();
                    m_instance.Init();
                }
                return m_instance;
            }
        }

        protected virtual void Init()
        {

        }
    }
}