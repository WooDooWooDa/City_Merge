
using UnityEngine;

public partial class ProfileManager : Manager
{
    private ProfileSaveData m_data = new ProfileSaveData();
    public ProfileSaveData Data { get { return m_data; } set { m_data = value; } }

    public override bool Load(ref SaveData data)
    {
        m_data = data.ProfileSaveData;

        if (m_data.FirstTime) {
            m_data.ID = RandomString(16);

            m_data.FirstTime = false;

            debugger.Log("Creating a new profile with id : " + m_data.ID);
        }

        return true;
    }

    public override bool Save(ref SaveData data)
    {

        data.ProfileSaveData = m_data;
        return true;
    }
}

[System.Serializable]
public class ProfileSaveData
{
    public bool FirstTime = true;
    public bool OnBoardingCompleted = false;

    public string ID = "";
    public int Level = 1;

    public CurrencyAmount[] Currencies = new CurrencyAmount[(int)CurrencyType.Count];
}