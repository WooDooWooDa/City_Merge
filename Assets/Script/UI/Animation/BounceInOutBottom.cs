using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceInOutBottom", menuName = "Config/Animation/Screen/BounceInOutBottom")]
public class BounceInOutBottom : ScreenAnimationBase
{
    public override void OpenAnimation(GameObject obj)
    {
        this.GameObject = obj;
        var m_startPos = obj.transform.localPosition;
        obj.transform.localPosition = Vector3.up * -500f;
        LeanTween.moveLocalY(obj, m_startPos.y + 100f, 0.1f * m_speedModifier)
                    .setOnComplete(() => {
                        LeanTween.moveLocalY(obj, m_startPos.y, 0.1f * m_speedModifier);
                    });
    }

    public override void CloseAnimation(GameObject obj)
    {
        this.GameObject = obj;
        LeanTween.moveLocalY(obj, obj.transform.localPosition.y + 100f, 0.1f * m_speedModifier)
            .setOnComplete(() => {
                LeanTween.moveLocalY(obj, obj.transform.localPosition.y - 500f, 0.1f * m_speedModifier)
                    .setOnComplete(DestroyScreen);
            });
    }

    protected void DestroyScreen()
    {
        Destroy(GameObject);
    }
}
