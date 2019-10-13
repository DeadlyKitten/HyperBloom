using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;
using System.Linq;
using IPA.Utilities;

namespace HyperBloom
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static BS_Utils.Utilities.Config config;
        static readonly string configName = "HyperBloom";
        static readonly string sectionName = "Settings";

        private bool postProcessEnabled = true;
        private float baseColorBoost = 0.5f;
        private float baseColorBoostThreshold = 0.1f;
        private int bloomIterations = 4;
        private float bloomIntensity = 1f;
        private int textureWidth = 512;


        public void Init(IPALogger logger)
        {
            Logger.logger = logger;
            config = new BS_Utils.Utilities.Config(configName);
        }

        public void OnApplicationStart() { }

        public void OnApplicationQuit() { }

        public void OnFixedUpdate() { }

        public void OnUpdate() { }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            var preset = GeneratePreset();
            var camera = Resources.FindObjectsOfTypeAll<MainEffect>().FirstOrDefault();
            camera?.GetPrivateField<MainEffectParams>("_mainEffectParams")?.InitFromPreset(preset);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }

        public void OnSceneUnloaded(Scene scene) { }

        private MainEffectGraphicsSettingsPresets.Preset GeneratePreset()
        {
            LoadSettings();

            var preset = new MainEffectGraphicsSettingsPresets.Preset
            {
                postProcessEnabled = postProcessEnabled,
                baseColorBoost = baseColorBoost,
                baseColorBoostThreshold = baseColorBoostThreshold,
                bloomIterations = bloomIterations,
                bloomIntensity = bloomIntensity,
                textureWidth = textureWidth
            };

            return preset;
        }

        private void LoadSettings()
        {
            postProcessEnabled = config.GetBool(sectionName, "Post Process Enabled", true, true);
            baseColorBoost = config.GetFloat(sectionName, "Base Color Boost", 0.5f, true);
            baseColorBoostThreshold = config.GetFloat(sectionName, "Base Color Boost Threshold", 0.1f, true);
            bloomIterations = config.GetInt(sectionName, "Bloom Iterations", 4, true);
            bloomIntensity = config.GetFloat(sectionName, "Bloom Intensity", 1, true);
            textureWidth = config.GetInt(sectionName, "Texture Width", 512, true);
        }
    }
}
