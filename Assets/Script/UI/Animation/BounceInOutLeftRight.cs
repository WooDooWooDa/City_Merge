using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceInOutLeftRight", menuName = "Config/Animation/Screen/BounceInOutLeftRight")]
public class BounceInOutLeftRight : ScreenAnimationBase
{
    [SerializeField] public bool m_leftToRight = true;

    private float m_translationValue = 1000f;

    public override void OpenAnimation(GameObject obj)
    {
        this.GameObject = obj;
        var m_startPos = obj.transform.localPosition;
        obj.transform.localPosition = Vector3.right * (m_translationValue * (m_leftToRight ? -1 : 1));
        LeanTween.moveLocalX(obj, m_startPos.x + (100f * (m_leftToRight ? -1 : 1)), 0.1f * m_speedModifier)
                    .setOnComplete(() => {
                        LeanTween.moveLocalX(obj, m_startPos.x, 0.1f * m_speedModifier);
                    });
    }

    public override void CloseAnimation(GameObject obj)
    {
        this.GameObject = obj;
        LeanTween.moveLocalX(obj, obj.transform.position.x + (m_translationValue * (m_leftToRight ? -1 : 1)), 0.1f * m_speedModifier)
                    .setOnComplete(DestroyScreen);
    }

    protected void DestroyScreen()
    {
        Destroy(GameObject);
    }
}
