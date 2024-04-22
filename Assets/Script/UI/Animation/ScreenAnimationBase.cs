using UnityEngine;

public abstract class ScreenAnimationBase : ScriptableObject
{
    [SerializeField] protected float m_speedModifier = 1;

    protected GameObject GameObject;

    public abstract void OpenAnimation(GameObject obj);

    public abstract void CloseAnimation(GameObject obj);
}