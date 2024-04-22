

using UnityEngine;

public abstract class Manager: MonoBehaviour
{
    public abstract string LoadingInfo { get; }

    public bool IsInitialize = false;
    protected ScriptableObject ManagerConfig;

    public virtual void FirstInitialization() { }

    public virtual void Initialize() {
        debugger = new Debugger(this.GetType().Name);
        if (IsInitialize) return;
    }

    public virtual void OnUpdate(float delta) { }

    public virtual void OnDestroy() { }

    public abstract bool Save(ref SaveData data);
    public abstract bool Load(ref SaveData data);

    #region Debug

    protected Debugger debugger;

    protected class Debugger
    {
        private string ManagerType;

        public Debugger(string name)
        {
            ManagerType = name;
        }

        public void Log(string msg)
        {
            Debug.Log($"[{ManagerType}] {msg}");
        }

        public void LogWarning(string msg)
        {
            Debug.LogWarning($"[{ManagerType}] {msg}");
        }

        public void LogError(string msg)
        {
            Debug.LogError($"[{ManagerType}] {msg}");
        }
    }

    #endregion
}