using UnityEngine;

namespace GameManagers.Pattern
{
    public class Singleton
    {
        private static Singleton _instance = null;

        protected Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Singleton();
                }

                return _instance;
            }
            private set {}
        }
    }
}
