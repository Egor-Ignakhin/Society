using System.Collections;

using UnityEngine;

namespace Society.Debugger
{
    /// <summary>
    /// �������� ������������ ������� FPS ������������ � ���� �������
    /// </summary>
    internal sealed class FpsDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;
        private readonly float updateFrequency = 0.5f;
        private void OnEnable() => StartCoroutine(nameof(FpsCoroutine));
        private IEnumerator FpsCoroutine()
        {
            while (true)
            {
                text.text = $"Fps: {(int)(1 / Time.deltaTime)}";

                yield return new WaitForSeconds(updateFrequency);
            }
        }
    }
}