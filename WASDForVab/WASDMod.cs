using KSP.OAB;
using UnityEngine;
using HarmonyLib;
using KSP.Messages;
using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using BepInEx.Configuration;
using System.Reflection;

namespace WASDForVAB;
[BepInPlugin("com.archlinus.wasd_for_vab", "WASD For VAB", "0.3.0")]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class WASDMod : BaseSpaceWarpPlugin
{
    private SubscriptionHandle loadSubscription;
    private WASDConfig config = new WASDConfig();
    private bool slowToggled = false;
    private Harmony harmony;

    public override void OnInitialized()
    {
        loadSubscription = Game.Messages.PersistentSubscribe<OABLoadedMessage>(OnOABLoadFinalized);
        config.Initialize(Config);
        harmony = new Harmony("com.archlinus.wasd_for_vab");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void OnOABLoadFinalized(MessageCenterMessage msg)
    {
        slowToggled = false;
        WASDPatches.patchState.isEnabled = true;
        if (Game != null && Game.OAB != null && Game.OAB.Current != null)
        {
            ObjectAssemblyBuilder current = Game.OAB.Current;
            WASDPatches.patchState.OnLoad(current.CameraManager);
        }
    }

    public void Update()
    {
        WASDPatches.patchState.isCtrlPressed = false;
        if (Game != null)
        {
            if (Game.OAB != null && Game.OAB.IsLoaded)
            {
                // Toggle WASD cam on ALT+w
                if (config.KeyToggleEnabled.Value.IsDown())
                {
                    WASDPatches.patchState.isEnabled = !WASDPatches.patchState.isEnabled;
                }

                if (config.RequireRightClickForControl.Value && !Input.GetMouseButton(1))
                    return;

                if (Input.GetKeyDown(config.KeySlowToggle.Value))
                {
                    slowToggled = !slowToggled;
                }

                Vector3d inputVector = new Vector3d();

                if (Input.GetKey(config.KeyForward.Value))
                {
                    inputVector.z = 1;
                }
                if (Input.GetKey(config.KeyBack.Value))
                {
                    inputVector.z = -1;
                }
                if (Input.GetKey(config.KeyRight.Value))
                {
                    inputVector.x = 1;
                }
                if (Input.GetKey(config.KeyLeft.Value))
                {
                    inputVector.x = -1;
                }
                if (Input.GetKey(config.KeyUp.Value))
                {
                    inputVector.y = 1;
                }
                if (Input.GetKey(config.KeyDown.Value))
                {
                    inputVector.y = -1;
                }
                if (Input.GetKey(config.KeyFast.Value))
                {
                    inputVector *= config.FastSpeedMultiplier.Value;
                }
                if (slowToggled || Input.GetKey(config.KeySlow.Value))
                {
                    inputVector *= config.SlowSpeedMultiplier.Value;
                }

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    WASDPatches.patchState.isCtrlPressed = true;
                }

                if (!inputVector.IsZero())
                {
                    WASDPatches.patchState.OnMove(inputVector, Time.deltaTime);
                }
            }
        }
    }

    public void OnDestroy()
    {
        if (Game != null)
        {
            Game.Messages.Unsubscribe(ref loadSubscription);
        }
        harmony.UnpatchSelf();
    }
}
