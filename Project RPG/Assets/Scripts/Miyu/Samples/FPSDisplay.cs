using UnityEngine;
using UnityEngine.UI;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField] private Text m_FrameRateText;
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