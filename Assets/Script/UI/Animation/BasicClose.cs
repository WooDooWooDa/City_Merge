using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicClose", menuName = "Config/Animation/Screen/BasicClose")]
public class BasicClose : ScreenAnimationBase
{
    public override void CloseAnimation(GameObject obj)
    {
        this.GameObject = obj;
        DestroyScreen();
    }

    public override void OpenAnimation(GameObject obj)
    {
        
    }

    protected void DestroyScreen()
    {
        Destroy(GameObject);
    }
}
