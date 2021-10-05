using System.Collections;

using UnityEngine;

namespace Society.Debugger
{
    public sealed class FpsDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;
        private void OnEnable()
        {
            StartCoroutine(nameof(FpsCoroutine));
        }
        private IEnumerator FpsCoroutine()
        {
            while (true)
            {
                text.text = $"Fps: {(int)(1 / Time.deltaTime)}";

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}