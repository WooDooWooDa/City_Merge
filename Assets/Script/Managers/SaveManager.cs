using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public interface ISaveable<T>
{
    public void Save(ref T data);
}

public interface ILoadable<T, X>
{
    public void Load(T data, X info);
}

/// <summary>
/// T is the saving data class (ex: BuildingData)
/// X is the scriptable object information class (ex: BuildingInformations)
/// </summary>
public interface ISavableLoadable<T, X> : ISaveable<T>, ILoadable<T, X> { }

public class SaveManager : Manager
{
    private static readonly string SAVE_EXTENTION = ".save";

    private BinaryFormatter m_formatter = null;
    private SaveData m_internalData = new SaveData();

    public override string LoadingInfo => "Loading from save";

    public override void Initialize()
    {
        base.Initialize();

        m_formatter = GetFormatter();

        IsInitialize = true;
    }

    public override bool Load(ref SaveData data)
    {
        if (LoadFromFile()) {
            data = m_internalData;

            return true;
        } else if (SaveToFile()) {
            data = m_internalData;

            return true;
        }
        return false;
    }

    public override bool Save(ref SaveData data)
    {
        data.TimeStamp = System.DateTime.Now.ToFileTime();

        if (m_internalData.TimeStamp < data.TimeStamp) {
            m_internalData = data;
        }

        return SaveToFile();
    }

    private bool LoadFromFile()
    {
        string path = GetSavePath() + Main.Instance.GlobalConfig.SaveName + SAVE_EXTENTION;

        if (!File.Exists(path)) {
            return false;
        }

        FileStream file = File.Open(path, FileMode.Open);

        try {
            m_internalData = (SaveData)m_formatter.Deserialize(file);
            file.Close();
        } catch {
            return false;
        }

        return true;
    }

    private bool SaveToFile()
    {
        if (!Directory.Exists(GetSavePath())) {
            Directory.CreateDirectory(GetSavePath());
        }

        string path = GetSavePath() + Main.Instance.GlobalConfig.SaveName + SAVE_EXTENTION;

        FileStream file = File.Create(path);

        m_formatter.Serialize(file, m_internalData);

        file.Close();

        return true;
    }

    private BinaryFormatter GetFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }

    private string GetSavePath()
    {
        return Application.persistentDataPath + "/saves/";
    }
}

[System.Serializable]
public class SaveData
{
    public bool FirstTime = true;
    public long TimeStamp = 0;

    public ProfileSaveData ProfileSaveData = new ProfileSaveData();
    public NotificationSaveData NotificationSaveData = new NotificationSaveData();
    public IslandSaveData IslandSaveData = new IslandSaveData();
    public BuildingSaveData BuildingSaveData = new BuildingSaveData();
    public UpgradeSaveData UpgradeSaveData = new UpgradeSaveData();
    public CollectionSaveData CollectionSaveData = new CollectionSaveData();
}
