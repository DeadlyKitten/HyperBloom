using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace HyperBloom
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static BS_Utils.Utilities.Config config;
        static readonly string configName = "CustomDebrisSettings";
        static readonly string sectionParticles = "Settings";

        private bool postProcessEnabled = true;
        private float baseColorBoost = 0.5f;
        private float baseColorBoostThreshold = 0.1f;
        private int bloomIterations = 4;
        private float bloomIntensity = 1f;
        private int textureWidth = 512;


        public void Init(IPALogger logger)
        {
            Logger.logger = logger;
        }

        public void OnApplicationStart() { }

        public void OnApplicationQuit() { }

        public void OnFixedUpdate() { }

        public void OnUpdate() { }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene) { }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) { }

        public void OnSceneUnloaded(Scene scene) { }

        private MainEffectGraphicsSettingsPresets.Preset GeneratePreset()
        {

        }
    }
}
