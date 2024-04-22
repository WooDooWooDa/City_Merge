using System;

public enum CollectionType
{
    Building,
    Upgrade,
    Count
}

public partial class CollectionManager : Manager
{
    private CollectionSaveData m_data = new CollectionSaveData();
    public CollectionSaveData Data { get { return m_data; } set { m_data = value; } }

    public override string LoadingInfo => "Arranging all your trophies";

    public override bool Load(ref SaveData data)
    {
        m_data = data.CollectionSaveData;

        return true;
    }

    public override bool Save(ref SaveData data)
    {
        data.CollectionSaveData = m_data;

        return true;
    }
}

[Serializable]
public class CollectionSaveData
{
    public int[] Collections = new int[(int)CollectionType.Count];
} 