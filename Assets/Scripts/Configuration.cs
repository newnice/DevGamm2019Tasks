using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Configuration object. Binds toggle menu button and objects that change their behaviour in debug and default mode 
/// </summary>
public class Configuration : MonoBehaviour
{
    [SerializeField] private Toggle modeToggle = null;

    [SerializeField] private List<Launcher> gameLaunchers = null;
    [SerializeField] private Launcher debugLauncher = null;
    [SerializeField] private VortexDebug vortexDebug = null;

    private void OnEnable()
    {
        modeToggle.onValueChanged.AddListener((e) =>
        {
            debugLauncher.EnableLaunch(e);
            vortexDebug.IsDebugEnabled = e;
        });
        foreach (var launcher in gameLaunchers)
        {
            modeToggle.onValueChanged.AddListener((e) => launcher.EnableLaunch(!e));
        }
    }

    private void OnDisable()
    {
        modeToggle.onValueChanged.RemoveAllListeners();
    }
}