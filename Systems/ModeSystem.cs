﻿using Game.UI.InGame;
using Game.UI;
using System;
using System.Reflection;
using System.Reflection.Emit;
using Game.Rendering;
using Colossal.UI.Binding;
using Game.Tools;
using Game.Simulation;
using Unity.Entities;
using Game.UI.Widgets;
using System.Collections.Generic;
using Game;
using Game.Rendering.CinematicCamera;
using UnityEngine;
using Game.Rendering.Utilities;

namespace PerformanceSqueezer.Systems
{
    public partial class ModeSystem : SystemBase
    {
        bool hasTriggered = false;
        bool ctrlPressed = false;
        bool altPressed = false;
        RenderingSystem RenderingSystem;


        public bool PotatoMode { get; set; }
        private bool graphicsSettingsApplied = false;
        protected override void OnUpdate()
        {

            bool currentCtrlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool currentAltPressed = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

            if (currentCtrlPressed && currentAltPressed && !ctrlPressed && !altPressed)
            {
                // Toggle PotatoMode if Ctrl + Alt are pressed for the first time
                PotatoMode = !PotatoMode;

                // Set flags to indicate the keys were pressed
                ctrlPressed = true;
                altPressed = true;
            }

            if ((!currentCtrlPressed || !currentAltPressed) && (ctrlPressed || altPressed))
            {
                // Reset the flags if either Ctrl or Alt is released
                ctrlPressed = false;
                altPressed = false;
                graphicsSettingsApplied = false;
            }
            // Check if PotatoMode is true and ApplyLowestGraphicsSettings() hasn't been applied yet
            if (PotatoMode && !graphicsSettingsApplied)
            {
                ApplyLowestGraphicsSettings();
                graphicsSettingsApplied = true; // Mark settings as applied
            }




        }
        void ApplyLowestGraphicsSettings()
        {


            QualitySettings.SetQualityLevel(0, true); // Set to the lowest quality level

            QualitySettings.masterTextureLimit = 3; // Reduces texture quality to minimum

            QualitySettings.shadows = ShadowQuality.Disable; // Disable shadows

            QualitySettings.shadowResolution = ShadowResolution.Low; // Set shadow resolution to low

            QualitySettings.shadowDistance = 0; // Set shadow distance to 0

            QualitySettings.shadowCascades = 0; // Disable shadow cascades

            QualitySettings.shadowProjection = ShadowProjection.CloseFit; // Use close-fit shadow projection

            QualitySettings.shadowNearPlaneOffset = 0.2f; // Set shadow near plane offset

            QualitySettings.realtimeReflectionProbes = false; // Disable realtime reflection probes

            QualitySettings.billboardsFaceCameraPosition = false; // Billboards don't face camera position

            QualitySettings.antiAliasing = 0; // Disable anti-aliasing

            QualitySettings.asyncUploadTimeSlice = 2; // Set async upload time slice

            QualitySettings.asyncUploadBufferSize = 2; // Set async upload buffer size
            SetPrivateFieldValue();
            SetIsEnabledToTrue();
            UnityEngine.Debug.Log("Settings set to lowest possible.");
        }



        public void SetPrivateFieldValue()
        {
            Type type = typeof(Game.Rendering.Utilities.AdaptiveDynamicResolutionScale);
            FieldInfo fieldInfo = type.GetField("s_CurrentScaleFraction", BindingFlags.NonPublic | BindingFlags.Static);

            if (fieldInfo != null)
            {
                // Setting the value to 0.01f
                fieldInfo.SetValue(null, 0.5f);
                UnityEngine.Debug.Log("s_CurrentScaleFraction value set to 0.01f");
            }
            else
            {
                Console.WriteLine("[Critical] CurrentScaleFraction not found.");
            }
        }

        public void SetIsEnabledToTrue()
        {
            Type type = typeof(Game.Rendering.Utilities.AdaptiveDynamicResolutionScale); // Replace with your class name
            PropertyInfo propertyInfo = type.GetProperty("isEnabled");

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                // Setting the value to true
                propertyInfo.SetValue(null, true, null);
                UnityEngine.Debug.Log("isEnabled set to true (Enabled)");
            }
            else
            {
                Console.WriteLine("[Critical] isEnabled property not found or cannot be written.");
            }
        }
        public void SetMinScaleToValue()
        {
            Type type = typeof(Game.Rendering.Utilities.AdaptiveDynamicResolutionScale); // Replace with your class name
            PropertyInfo propertyInfo = type.GetProperty("minScale");

            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                // Setting the value to 0.01f
                propertyInfo.SetValue(null, 0.01f, null);
                UnityEngine.Debug.Log("minScale value set to 0.01f");
            }
            else
            {
                Console.WriteLine("[Critical] minScale property not found or cannot be written.");
            }
        }

    }
}
