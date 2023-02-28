using System;
using BepInEx;
using I2.Loc;
using KSP.Game;
using KSP.Modules;
using KSP.OAB;
using KSP.Sim;
using UnityEngine;
using HarmonyLib;
using KSP.Messages;
using System.Reflection;

namespace KSPTestMod
{
    [BepInPlugin("io.bepis.wasd_for_vab", "WASD for VAB", "1.0")]
    public class WASDMod : BaseUnityPlugin
    {
        public GameInstance Game => GameManager.Instance.Game;

        private static Harmony harmony;

        private SubscriptionHandle loadSubscription;

        public void Awake()
        {
            var assembly = Assembly.GetExecutingAssembly();
            harmony = Harmony.CreateAndPatchAll(assembly);
            WASDPatches.patchState.Logger = Logger;
            loadSubscription = Game.Messages.Subscribe<OABLoadFinalizedMessage>(OnOABLoadFinalized);
        }

        private void OnOABLoadFinalized(MessageCenterMessage msg)
        {
            if ((msg as OABLoadFinalizedMessage).isSnapshotLoaded)
            {
                ObjectAssemblyBuilder current = Game.OAB.Current;
                WASDPatches.patchState.OnLoad(current.CameraManager);
            }
        }

        public void Update()
        {
            if (Game != null)
            {
                if (Game.OAB != null && Game.OAB.IsLoaded)
                {
                    if (Input.GetMouseButton(1))
                    {
                        Vector3d inputVector = new Vector3d();
                        if (Input.GetKey(KeyCode.W))
                        {
                            inputVector.z = 1;
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            inputVector.z = -1;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            inputVector.x = 1;
                        }
                        if (Input.GetKey(KeyCode.A))
                        {
                            inputVector.x = -1;
                        }
                        if (Input.GetKey(KeyCode.E))
                        {
                            inputVector.y = 1;
                        }
                        if (Input.GetKey(KeyCode.Q))
                        {
                            inputVector.y = -1;
                        }

                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            inputVector *= 2;
                        }

                        if (!inputVector.IsZero())
                        {
                            WASDPatches.patchState.OnMove(inputVector, Time.deltaTime);
                        }
                    }
                }
            }
        }

        public void OnDestroy()
        {
            Game.Messages.Unsubscribe(ref loadSubscription);
            harmony.UnpatchSelf();
        }

    }
}
