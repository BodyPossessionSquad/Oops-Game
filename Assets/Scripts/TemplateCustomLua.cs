using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CustomLuaFunctions : MonoBehaviour
{
    public UIManager uiManager; // Assign this in the Inspector

    public bool unregisterOnDisable = false;

    void OnEnable()
    {
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();

        Lua.RegisterFunction("ShowCutsceneEndScreen", uiManager, SymbolExtensions.GetMethodInfo(() => uiManager.ShowCutsceneEndScreen()));
    }

    void OnDisable()
    {
        if (unregisterOnDisable)
        {
            Lua.UnregisterFunction("ShowCutsceneEndScreen");
        }
    }
}
