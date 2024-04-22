using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHole", menuName = "Config/Animation/Screen/BlackHole")]
public class BlackHole : ScreenAnimationBase
{
    public override void OpenAnimation(GameObject obj)
    {
        
    }

    public override void CloseAnimation(GameObject obj)
    {
        this.GameObject = obj;
        LeanTween.scale(obj, Vector3.zero, 0.1f * m_speedModifier)
                    .setOnComplete(DestroyScreen);
    }

    protected void DestroyScreen()
    {
        Destroy(GameObject);
    }
}