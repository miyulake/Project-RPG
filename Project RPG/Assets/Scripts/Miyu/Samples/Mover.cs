using UnityEngine;
using Miyu.Tools;

namespace Miyu.Samples
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Vector3 m_TargetPosition;
        [SerializeField] private AnimationCurve m_MoveCurve;
        [SerializeField] private float m_Duration = 1f;
        [SerializeField] private bool m_Loop = true;

        private Tween m_Tween;
        private Vector3 m_OriginalPosition;

        private void Start()
        {
            m_OriginalPosition = transform.position;
            m_Tween = new(m_MoveCurve, m_Duration, m_Loop);
            m_Tween.OnUpdate += value =>
            { transform.position = Vector3.Lerp(m_OriginalPosition, m_TargetPosition, value); };
            m_Tween.Play();
        }

        private void Update() => m_Tween.Tick(Time.deltaTime);
    }
}