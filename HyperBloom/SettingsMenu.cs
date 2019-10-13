using CustomUI.Settings;
using UnityEngine;

namespace HyperBloom
{
    class SettingsMenu
    {
        internal static bool initialized;

        internal static void CreateSettingsUI()
        {
            if (initialized) return;
            initialized = true;

            Logger.Log("Creating Settings UI", Logger.LogLevel.Notice);

            string sectionName = "Settings";
            var subMenu = SettingsUI.CreateSubMenu("Hyper Bloom");

            var intensityOption = subMenu.AddList("Bloom Intensity", new float[] { 0, 0.25f, 0.5f, 0.75f, 1, 10, 20, 30, 40, 50, 60, 69, 80, 90, 100, 125, 150, 175, 200, 300, 400, 500, 1000, float.MaxValue });
            intensityOption.GetValue += delegate { return Plugin.bloomIntensity; };
            intensityOption.SetValue += delegate (float value)
            {
                Plugin.bloomIntensity = value;
                Plugin.config.SetFloat(sectionName, "Bloom Intensity", value);
            };
            intensityOption.FormatValue += delegate (float value) { return $"{value}"; };

            var iterationsOption = subMenu.AddList("Bloom Iterations", new float[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20, 25, 30, 35, 40 });
            iterationsOption.GetValue += delegate { return Plugin.bloomIterations; };
            iterationsOption.SetValue += delegate (float value)
            {
                Plugin.bloomIterations = (int) value;
                Plugin.config.SetInt(sectionName, "Bloom Iterations", (int)value);
            };
            iterationsOption.FormatValue += delegate (float value) { return $"{value}"; };

            var textureWidthOption = subMenu.AddList("Texture Width", new float[] { 64, 128, 256, 512, 1024, 2048, 4096 });
            textureWidthOption.GetValue += delegate { return Plugin.textureWidth; };
            textureWidthOption.SetValue += delegate (float value)
            {
                Plugin.textureWidth = (int) value;
                Plugin.config.SetInt(sectionName, "Texture Width", (int)value);
            };
            textureWidthOption.FormatValue += delegate (float value) { return $"{value}"; };
        }
    }
}