using IPA.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

namespace HyperBloom
{
    class Bloominator3000 : MonoBehaviour
    {
        private ScoreController _scoreController;
        private MainEffect _mainEffect;
        private MainEffectParams _mainEffectParams;

        private IEnumerator _flashbangCoroutine;
        private bool _flashbangActive = false;
        private float _flashbangTimestep = 0.02f;

        internal static Bloominator3000 Instance { get; private set; }

        internal static void Init()
        {
            if (!Instance) Instance = new GameObject().AddComponent<Bloominator3000>();
            Plugin.bloomIntensity = Plugin.initialBloomIntensity;
            Instance?._mainEffectParams?.SetPrivateField("_bloomIntensity", Plugin.bloomIntensity);
        }

        internal void Cleanup()
        {
            try
            {
                _scoreController.noteWasMissedEvent -= OnNoteMiss;
            }
            catch (Exception e)
            {
                Logger.Log("Error unsubscribing OnNoteMiss event.", Logger.LogLevel.Error);
                Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
            }
        }

        private void Awake()
        {
            if (Plugin.bloomOnMissEnabled)
            {
                Logger.Log("Bloom On Miss™ active");
                StartCoroutine(GetScoreController());
                _mainEffect = Resources.FindObjectsOfTypeAll<MainEffect>().FirstOrDefault();
                _mainEffectParams = _mainEffect?.GetPrivateField<MainEffectParams>("_mainEffectParams");
                _flashbangCoroutine = Flashbang();
                Logger.Log("BLOOMINATOR 3000 ACTIVATED");
                Logger.Log($"Status: {((_mainEffectParams != null) ? "OK" : "ERROR")}");
            }
        }

        private void OnNoteMiss(NoteData data, int c)
        {
            AmplifyBloom();
        }

        private void AmplifyBloom()
        {
            Plugin.bloomIntensity += Plugin.bloomStep;
            if (!Plugin.flashbangEnabled)
                _mainEffectParams?.SetPrivateField("_bloomIntensity", Plugin.bloomIntensity);
            else
            {
                Logger.Log("Flashbang?");
                if (_flashbangActive) StopCoroutine(_flashbangCoroutine);
                _flashbangCoroutine = Flashbang();
                StartCoroutine(_flashbangCoroutine);
            }
        }

        private void OnNoteCute(NoteData data, NoteCutInfo info, int c)
        {
            if (data.noteType == NoteType.Bomb || !info.allIsOK)
                AmplifyBloom();
        }

        IEnumerator GetScoreController()
        {
            yield return new WaitUntil(() => _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault());
            _scoreController.noteWasCutEvent += OnNoteCute;
            _scoreController.noteWasMissedEvent += OnNoteMiss;
        }

        IEnumerator Flashbang()
        {
            Logger.Log("FLASHBANG");
            var decreaseAmount = (Plugin.flashbangIntensity / Plugin.flashbangTimeToReturn) * _flashbangTimestep;
            Logger.Log($"Decrease Amount: {decreaseAmount}");
            var currentIntensity = Plugin.flashbangIntensity;
            _flashbangActive = true;
            _mainEffectParams?.SetPrivateField("_bloomIntensity", currentIntensity);
            
            for (int i = 0; i < (1 / _flashbangTimestep) * Plugin.flashbangTimeToReturn; i++)
            {
                yield return new WaitForSeconds(_flashbangTimestep);
                currentIntensity -= decreaseAmount;
                if (currentIntensity < Plugin.bloomIntensity) currentIntensity = Plugin.bloomIntensity;
                Logger.Log($"Current Intensity: {currentIntensity}");
                _mainEffectParams?.SetPrivateField("_bloomIntensity", currentIntensity);
            }

            _mainEffectParams?.SetPrivateField("_bloomIntensity", Plugin.bloomIntensity);
            _flashbangActive = false;
        }
    }
}
