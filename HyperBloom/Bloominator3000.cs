using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IPA.Utilities;

namespace HyperBloom
{
    class Bloominator3000 : MonoBehaviour
    {
        private ScoreController _scoreController;
        private MainEffect _mainEffect;
        private MainEffectParams _mainEffectParams;

        internal static Bloominator3000 Instance { get; private set; }

        internal static void Init()
        {
            if (!Instance) Instance = new GameObject().AddComponent<Bloominator3000>();
            Plugin.bloomIntensity = Plugin.initialBloomIntensity;
            Instance._mainEffectParams.SetPrivateField("_bloomIntensity", Plugin.bloomIntensity);
        }

        internal void Cleanup()
        {
            _scoreController.noteWasMissedEvent -= OnNoteMiss;
        }

        private void Awake()
        {
            if (Plugin.bloomOnMissEnabled)
            {
                StartCoroutine(GetScoreController());
                _mainEffect = Resources.FindObjectsOfTypeAll<MainEffect>().FirstOrDefault();
                _mainEffectParams = _mainEffect.GetPrivateField<MainEffectParams>("_mainEffectParams");
            }
        }

        private void OnNoteMiss(NoteData data, int c)
        {
            Plugin.bloomIntensity += Plugin.bloomStep;
            Logger.Log($"Bloom Intensity = {Plugin.bloomIntensity}");
            _mainEffectParams.SetPrivateField("_bloomIntensity", Plugin.bloomIntensity);
        }

        IEnumerator GetScoreController()
        {
            yield return new WaitUntil(() => _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault());
            _scoreController.noteWasMissedEvent += OnNoteMiss;
        }
    }
}
