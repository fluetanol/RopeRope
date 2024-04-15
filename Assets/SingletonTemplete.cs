
using UnityEngine;

public abstract class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    protected void Awake() {
        DontDestroyOnLoad(this);
    }
    protected abstract void SetInstance();
}

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}
