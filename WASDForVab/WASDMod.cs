using KSP.OAB;
using UnityEngine;
using HarmonyLib;
using KSP.Messages;
using System.Reflection;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Configuration;
using SpaceWarp.API.Managers;

namespace WASDForVAB
{
    [MainMod]
    public class WASDMod : Mod
    {
        private SubscriptionHandle loadSubscription;
        private WASDConfig config = new WASDConfig();

        public override void OnInitialized()
        {
            loadSubscription = Game.Messages.PersistentSubscribe<OABLoadedMessage>(OnOABLoadFinalized);

            // Load the configuration
            if (ManagerLocator.TryGet(out ConfigurationManager configManager))
            {
                if (configManager.TryGet(Info.mod_id, out var config))
                {
                    WASDConfig cfg = (WASDConfig)config.configObject;
                    this.config = cfg;
                    WASDPatches.patchState.config = cfg;
                    Logger.Info($"Camera sensitivity: {cfg.CameraSensitivity}");
                }
                else
                {
                    Logger.Warn($"Failed to find configuration for {Info.mod_id}");
                }
            }
            else
            {
                Logger.Error($"Failed to find configuration manager");
            }
        }

        private void OnOABLoadFinalized(MessageCenterMessage msg)
        {
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
                    if (Input.GetKey(KeyCode.LeftAlt))
                    {
                        if (Input.GetKeyDown(KeyCode.W))
                        {
                            WASDPatches.patchState.isEnabled = !WASDPatches.patchState.isEnabled;
                        }
                    }

                    if (config.RequireRightClickForControl && !Input.GetMouseButton(1))
                        return;

                    Vector3d inputVector = new Vector3d();

                    if (Input.GetKey(config.KeyForward.ToLowerInvariant()))
                    {
                        inputVector.z = 1;
                    }
                    if (Input.GetKey(config.KeyBack.ToLowerInvariant()))
                    {
                        inputVector.z = -1;
                    }
                    if (Input.GetKey(config.KeyRight.ToLowerInvariant()))
                    {
                        inputVector.x = 1;
                    }
                    if (Input.GetKey(config.KeyLeft.ToLowerInvariant()))
                    {
                        inputVector.x = -1;
                    }
                    if (Input.GetKey(config.KeyUp.ToLowerInvariant()))
                    {
                        inputVector.y = 1;
                    }
                    if (Input.GetKey(config.KeyDown.ToLowerInvariant()))
                    {
                        inputVector.y = -1;
                    }
                    if (Input.GetKey(config.KeyFast.ToLowerInvariant()))
                    {
                        inputVector *= config.FastSpeedMultiplier;
                    }
                    if (Input.GetKey(config.KeySlow.ToLowerInvariant()))
                    {
                        inputVector *= config.SlowSpeedMultiplier;
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
        }
    }
}
