using UnityEngine;
using Miyu.Tools;
using TMPro;

namespace Game.Debug
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_FrameRateText;
        [SerializeField] private float m_UpdateInterval = 0.33f;
        private FPSMonitor m_FPSMonitor;

        private void Awake()
        {
            m_FPSMonitor = new(m_UpdateInterval);
            m_FPSMonitor.OnUpdated += OnFPSUpdated;
        }

        private void Update() => m_FPSMonitor.Tick(Time.unscaledDeltaTime);

        private void OnFPSUpdated(float fps) => m_FrameRateText.text = $"<color=yellow>{Mathf.RoundToInt(fps)}</color> fps";
    }
}