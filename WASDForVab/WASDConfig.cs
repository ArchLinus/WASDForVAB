using BepInEx.Configuration;
using UnityEngine;
namespace WASDForVAB;

public class WASDConfig
{
    public ConfigEntry<float> CameraSensitivity;
    public ConfigEntry<float> BaseSpeed;
    public ConfigEntry<float> FastSpeedMultiplier;
    public ConfigEntry<float> SlowSpeedMultiplier;
    public ConfigEntry<bool> RequireRightClickForControl;
    public ConfigEntry<KeyboardShortcut> KeyToggleEnabled;
    public ConfigEntry<KeyCode> KeyForward;
    public ConfigEntry<KeyCode> KeyLeft;
    public ConfigEntry<KeyCode> KeyBack;
    public ConfigEntry<KeyCode> KeyRight;
    public ConfigEntry<KeyCode> KeyUp;
    public ConfigEntry<KeyCode> KeyDown;
    public ConfigEntry<KeyCode> KeyFast;
    public ConfigEntry<KeyCode> KeySlow;
    public ConfigEntry<KeyCode> KeySlowToggle;

    public void Initialize(ConfigFile Config)
    {
        CameraSensitivity = Config.Bind("Settings", "Camera Sensitivity", 1.0f);
        BaseSpeed = Config.Bind("Settings", "Base Speed", 20.0f);
        FastSpeedMultiplier = Config.Bind("Settings", "Fast Speed Multiplier", 3.0f);
        SlowSpeedMultiplier = Config.Bind("Settings", "Slow Speed Multiplier", 0.4f);
        RequireRightClickForControl = Config.Bind("Settings", "Require Right Click For Control", true);
        KeyToggleEnabled = Config.Bind("Settings", "Key Toggle Enabled", new KeyboardShortcut(KeyCode.W, KeyCode.LeftAlt));
        KeyForward = Config.Bind("Settings", "Key Forward", KeyCode.W);
        KeyLeft = Config.Bind("Settings", "Key Left", KeyCode.A);
        KeyBack = Config.Bind("Settings", "Key Back", KeyCode.S);
        KeyRight = Config.Bind("Settings", "Key Right", KeyCode.D);
        KeyUp = Config.Bind("Settings", "Key Up", KeyCode.E);
        KeyDown = Config.Bind("Settings", "Key Down", KeyCode.Q);
        KeyFast = Config.Bind("Settings", "Key Fast", KeyCode.LeftShift);
        KeySlow = Config.Bind("Settings", "Key Slow", KeyCode.LeftControl);
        KeySlowToggle = Config.Bind("Settings", "Key Slow Toggle", KeyCode.None);
    }
}