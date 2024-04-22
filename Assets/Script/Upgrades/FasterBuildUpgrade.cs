using System;
using UnityEngine;

public class FasterBuildUpgrade : Upgrade
{
    public int BaseGoal = 10;
    public int CurrentGoalWithUpgrade { get { return -CurrentLevel; } } 

    public override void Initialize(UpgradeManager upgradeManager)
    {
        base.Initialize(upgradeManager);

        currentAdv = $"{BaseGoal + CurrentGoalWithUpgrade}";
        nextAdv = $"{BaseGoal + CurrentGoalWithUpgrade - 1} clicks";
    }

    public override void LevelUp()
    {
        base.LevelUp();
        currentAdv = $"{BaseGoal + CurrentGoalWithUpgrade}";
        nextAdv = $"{BaseGoal + CurrentGoalWithUpgrade - 1} clicks";
    }

    public override void OnUpdate() { }

    protected override bool CheckIntegrity()
    {
        return true;
    }

    protected override void OnToggled() { }
}
