using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenLayer
{
    Screens,
    Main,
    Popup
}

public struct ScreenStack
{
    public bool Toggled;
    public ScreenType ScreenType;
    public UIScreenBase Screen;
}

public partial class ScreenManager : Manager
{
    private static readonly string GO_NAME = "ScreenBase";
    public override string LoadingInfo => "Preparing the screens";

    private GameObject m_screenParentGO;
    private Dictionary<ScreenLayer, Transform> m_screenLayers = new Dictionary<ScreenLayer, Transform>();
    private Dictionary<ScreenType, ScreenInformations> m_screensInformations = new Dictionary<ScreenType, ScreenInformations>();
    private Stack<ScreenStack> m_screenStack = new Stack<ScreenStack>();

    private ScreenConfig m_config;

    public override void Initialize()
    {
        base.Initialize();

        m_config = Main.Instance.GlobalConfig.ScreenConfig;
        foreach (ScreenInformations info in m_config.ScreensInfo) {
            if (m_screensInformations.ContainsKey(info.ScreenType)) {
                debugger.LogError("Screen of type : " + info.ScreenType + ", has already a screen defined");
                continue;
            }
            m_screensInformations.Add(info.ScreenType, info);
        }

        m_screenParentGO = new GameObject(GO_NAME);
        foreach (ScreenLayer layer in Enum.GetValues(typeof(ScreenLayer))) {
            Transform newLayer = new GameObject(layer.ToString()).transform;
            newLayer.parent = m_screenParentGO.transform;
            m_screenLayers.Add(layer, newLayer);
        }
        m_screenParentGO.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        
        OpenScreen(ScreenType.Loading, new OpenInfo()
        {
            OnCloseScreen = () => OpenScreen(ScreenType.Main)
        });
        IsInitialize = true;
    }

    public UIScreenBase ToggleScreen(ScreenType screenType, OpenInfo openInfo = null)
    {
        if (IsOpen(screenType, out ScreenStack screen)) {
            CloseScreen(screenType);
        } else {
            return OpenScreen(screenType, openInfo);
        }
        return null;
    }

    public UIScreenBase OpenScreen(ScreenType screenType, OpenInfo openInfo = null)
    {
        if (IsOpen(screenType, out ScreenStack screenStack)) {
            DisableAllScreen();
            screenStack.Screen.Enable(true);
            return screenStack.Screen;
        } else {
            if (m_screensInformations.TryGetValue(screenType, out ScreenInformations screenInfo)) {
                if (screenInfo == null) {
                    debugger.LogWarning("No screen associatated with this screen type.");
                    return null;
                }

                UIScreenBase screen = GameObject.Instantiate(screenInfo.Screen, m_screenParentGO.transform);

                if (m_screenStack.Count > 0) {
                    ScreenStack lastScreen = m_screenStack.Peek();
                    if (lastScreen.Screen != null) {
                        lastScreen.Screen.Enable(false);
                    }
                }

                if (m_screenLayers.TryGetValue(screenInfo.ScreenLayer, out Transform parentLayer)) {
                    screen.transform.SetParent(parentLayer);
                    
                }

                if (openInfo == null)
                    openInfo = new OpenInfo();
                openInfo.Informations = screenInfo;

                screen.Open(openInfo);
                m_screenStack.Push(new ScreenStack
                {
                    ScreenType = screenType,
                    Screen = screen
                });

                Events.Screen.FireScreenOpened(screenType);
                debugger.Log("Opening screen : " + screen.name);
                return screen;
            } else {
                debugger.LogWarning("Trying to open a screen type not yet added to the config");
                return null;
            }
        }
    }

    //Todo change close to close all opened screen, use back 
    public void CloseScreen(ScreenType screenType)
    {
        if (m_screenStack.Count > 0 && m_screenStack.Peek().Screen != null) {
            ScreenStack screen = m_screenStack.Peek();
            if (screen.ScreenType == screenType) {
                var screenStack = m_screenStack.Pop();
                screenStack.Screen.OnClose();
                Events.Screen.FireScreenClosed(screenType);
                debugger.Log("Closing screen : " + screenStack.Screen.name);
            }

            if (m_screenStack.Count > 0 && m_screenStack.Peek().Screen != null) {
                m_screenStack.Peek().Screen.Enable(true);
            }

            return;
        }

        debugger.LogWarning("No previous screen, stack is empty.");
    }

    public void DisableAllScreen()
    {
        foreach (ScreenStack screenStack in m_screenStack) {
            screenStack.Screen.Enable(false);
        }
    }

    public void Back()
    {
        if (m_screenStack.Count > 0 && m_screenStack.Peek().Screen == null) {
            debugger.LogWarning(" No previous screen, stack is empty.");
            return;
        }

        var screenStack = m_screenStack.Pop();
        screenStack.Screen.OnClose();

        if (m_screenStack.Count > 0 && m_screenStack.Peek().Screen != null) {
            m_screenStack.Peek().Screen.Enable(true);
            return;
        }

        debugger.LogWarning(" No previous screen, stack is empty.");
    }

    private bool IsOpen(ScreenType screenType, out ScreenStack screen)
    {
        screen = new ScreenStack { };
        foreach (ScreenStack screenStack in m_screenStack) {
            if (screenStack.ScreenType == screenType) {
                screen = screenStack;
                return true;
            }
        }
        return false;
    }
}
