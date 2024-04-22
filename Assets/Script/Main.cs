using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main
{
    private Main() 
    {
        m_data = new SaveData();
    }

    private static Main m_instance;

    private bool m_initialized = false;
    public bool IsInitialized => m_initialized;
    public Action OnInitialized;
    private SaveData m_data;

    public static Main Instance { 
        get {
            if (m_instance == null) {
                m_instance = new Main();
            }
            return m_instance;
        }

        private set { }
    }
    private Boot m_boot;
    public Boot Boot => m_boot;
    private GlobalConfig m_globalConfig;
    public GlobalConfig GlobalConfig { get { return m_globalConfig; } }

    private SaveManager m_saveManager = null;
    private CollectionManager m_collection;
    private ScreenManager m_screenManager = null;
    private InputManager m_inputManager = null;
    private UpgradeManager m_upgradeManager = null;
    private UIManager m_uiManager = null;
    private GameManager m_gameManager = null;
    private ProfileManager m_profileManager = null;
    private IslandManager m_islandManager = null;
    private BuildingManager m_buildingManager = null;
    private RoadManager m_roadManager = null;

    private List<Manager> m_managers = new List<Manager>();
    public List<Manager> Managers { get { return m_managers; } }

    public void Initialize(Boot boot)
    {
        m_boot = boot;
        m_globalConfig = boot.GlobalConfig;

        if (m_initialized)
            return;

        //Main managers
        m_saveManager = boot.gameObject.AddComponent<SaveManager>();
        //m_managers.Add(m_gameManager = new GameManager());

        //Game related managers
        //m_managers.Add(m_uiManager = new UIManager());
        m_managers.Add(m_collection = boot.gameObject.AddComponent<CollectionManager>());
        m_managers.Add(m_inputManager = boot.gameObject.AddComponent<InputManager>());
        m_managers.Add(m_screenManager = boot.gameObject.AddComponent<ScreenManager>());
        m_managers.Add(m_profileManager = boot.gameObject.AddComponent<ProfileManager>());
        m_managers.Add(m_islandManager = boot.gameObject.AddComponent<IslandManager>());
        m_managers.Add(m_buildingManager = boot.gameObject.AddComponent<BuildingManager>());
        m_managers.Add(m_upgradeManager = boot.gameObject.AddComponent<UpgradeManager>());
        //m_managers.Add(m_roadManager = new RoadManager());

        m_saveManager.Initialize();
        foreach (Manager manager in m_managers) {
            manager.Initialize();
        }

        LoadGame();

        m_initialized = true;
        OnInitialized?.Invoke();

        Events.Loading.FireFinishLoading();
    }

    public T GetManager<T>()
    {
        foreach (var manager in m_managers) {
            if (manager.GetType() == typeof(T)) {
                if (manager is T)
                    return (T)Convert.ChangeType(manager, typeof(T));
            }
        }
        return default;
    }

    public void OnUpdate(float delta)
    {
        foreach (Manager manager in m_managers) {
            if (!manager.IsInitialize) continue;
            manager.OnUpdate(delta);
        }
    }

    private bool LoadGame()
    {
        if (!m_saveManager.Load(ref m_data)) {
            Debug.Log("Failed to load or create save file");
            return false;
        }

        foreach (Manager manager in m_managers) {
            Events.Loading.FireStartLoadingManager(manager);
            manager.Load(ref m_data);
            Events.Loading.FireManagerFinishLoading();
        }

        return true;
    }

    private void SaveGame()
    {
        foreach (Manager manager in m_managers) {
            manager.Save(ref m_data);
        }
        m_saveManager.Save(ref m_data);
    }

    public void OnQuit()
    {
        SaveGame();
    }

    public void OnDestroy()
    {
        foreach (Manager manager in m_managers) {
            manager.OnDestroy();
        }
    }
}