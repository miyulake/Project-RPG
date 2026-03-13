using UnityEngine;

namespace Miyu.Samples
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Light m_Light;
        [SerializeField] private float m_Speed = 2f;
        [Header("Intensity")]
        [Min(0), SerializeField] private float m_MinIntensity = 1f;
        [Min(0), SerializeField] private float m_MaxIntensity = 5f;
        [Header("Range")]
        [Min(0), SerializeField] private float m_MinRange = 10f;
        [Min(0), SerializeField] private float m_MaxRange = 15f;
        [Header("Movement")]
        [Range(0, 0.01f), SerializeField] private float m_MovementRange = 0.0025f;

        private float m_NoiseOffset;

        private void Awake()
        {
            if (m_Light == null) m_Light = GetComponent<Light>();
            m_NoiseOffset = Random.Range(0f, 100f);
        }

        private void FixedUpdate()
        {
            var noise = Mathf.PerlinNoise(Time.time * m_Speed + m_NoiseOffset, m_NoiseOffset);
            m_Light.intensity = Mathf.Lerp(m_MinIntensity, m_MaxIntensity, noise);
            m_Light.range = Mathf.Lerp(m_MinRange, m_MaxRange, noise);
            if (m_MovementRange != 0) transform.localPosition += Random.insideUnitSphere * m_MovementRange;
        }
    }
}