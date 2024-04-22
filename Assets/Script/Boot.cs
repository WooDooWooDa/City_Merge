using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private GlobalConfig m_globalConfig = null;
    public GlobalConfig GlobalConfig { get { return m_globalConfig; } }
    [SerializeField] public GameObject MainCamera;
    private GameObject m_camera;
    public GameObject Camera => m_camera;

    void Awake()
    {
        m_camera = Instantiate(MainCamera);
    }

    private void Start()
    {
        Main.Instance.Initialize(this);
    }

    private void Update()
    {
        Main.Instance.OnUpdate(Time.deltaTime);
    }

    private void OnApplicationPause(bool pause)
    {
        
    }

    private void OnApplicationQuit()
    {
        Main.Instance.OnQuit();
    }

    private void OnDestroy()
    {
        Main.Instance.OnDestroy();
    }
}
