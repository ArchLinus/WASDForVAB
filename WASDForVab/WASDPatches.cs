using KSP.OAB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace WASDForVAB
{
    public class WASDPatches
    {
        public class WASDPatchesState
        {
            public float yaw = 0;
            public float pitch = 0;
            public Vector3d lookDirection;
            public float movementSpeed = 20.5f;
            public ObjectAssemblyCameraManager cameraManager;
            public Vector3d cameraPos = new Vector3d();
            public bool isCtrlPressed = false;
            public bool isEnabled = true;
            public WASDConfig config;

            public void OnLoad(ObjectAssemblyCameraManager cameraManager)
            {
                this.cameraManager = cameraManager;
                pitch = cameraManager.CameraGimbal.transform.rotation.eulerAngles.x;
                yaw = cameraManager.CameraGimbal.transform.rotation.eulerAngles.z;
            }

            public void OnMove(Vector3d inputVector, float deltaTime)
            {
                if (cameraManager != null && isEnabled)
                {
                    Vector3d forward = cameraManager.gimbalTransform.transform.forward;
                    Vector3d right = -Vector3d.Cross(forward, Vector3d.up);
                    Vector3d up = Vector3d.up;
                    Vector3d movement = forward * inputVector.z + right * inputVector.x + up * inputVector.y;
                    if (config != null)
                    {
                        movement *= deltaTime * config.BaseSpeed.Value;
                    }
                    else
                    {
                        movement *= deltaTime * movementSpeed;
                    }
                    cameraManager.gimbalTransform.position = cameraManager.gimbalTransform.position + movement;
                }
            }
        }

        public static WASDPatchesState patchState = new WASDPatchesState();

        [HarmonyPatch(typeof(ObjectAssemblyInputHandler), "OnSelectAllPrimaryAssembly")]
        class OnSelectAllPrimaryAssemblyPatch
        {
            static bool Prefix()
            {
                // Prevent ctrl+A binding from firing while we're in control
                return !patchState.isCtrlPressed;
            }
        };


        [HarmonyPatch(typeof(ObjectAssemblyPlacementTool), "ProcessInputCameraRotation")]
        class ProcessInputCameraRotationPatch
        {
            static bool Prefix(Vector3 orbitTargetPos, float prevYaw, float prevPitch, float deltaYaw, float deltaPitch, float distance, ref Quaternion lookRotation, ref Vector3 lookDirection, ref Vector3 lookPosition)
            {
                if (!patchState.isEnabled)
                {
                    return true;
                }

                var cameraSensitivity = 1.0f;
                if (patchState.config != null)
                {
                    cameraSensitivity = patchState.config.CameraSensitivity.Value;
                }

                var currentPitch = patchState.cameraManager.gimbalTransform.transform.eulerAngles.x;
                var currentYaw = patchState.cameraManager.gimbalTransform.transform.eulerAngles.y;
                var newPitch = currentPitch + (deltaPitch * cameraSensitivity);
                var newYaw = currentYaw + (deltaYaw * cameraSensitivity);

                if (currentPitch > 180.0f && currentPitch + deltaPitch < 270.0f)
                {
                    newPitch = 270.0f;
                }
                else if (currentPitch < 90.0f && currentPitch + deltaPitch > 89.0f)
                {
                    newPitch = 89.0f;
                }

                Vector3d lookDir = new Vector3d(0, 0, 1);
                lookRotation = QuaternionD.AngleAxis(newYaw, Vector3d.up) * QuaternionD.AngleAxis(newPitch, Vector3d.right);
                lookDir = lookRotation * lookDir;
                lookDirection = lookDir;
                lookPosition = patchState.cameraManager.gimbalTransform.position;
                return false;
            }
        }
    }
}