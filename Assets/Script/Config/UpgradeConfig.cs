using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Config/Upgrade", order = 5)]
public class UpgradeConfig: ScriptableObject
{
    //TODO move lock icon to screens config
    [SerializeField] public Sprite LockIcon;

    [SerializeField] private UpgradeInformations[] m_upgradeInformations;
    public UpgradeInformations[] UpgradeInformations { get { return m_upgradeInformations; } }
}
