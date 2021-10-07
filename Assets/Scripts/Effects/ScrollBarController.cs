using System.Collections;

using UnityEngine;
using UnityEngine.UI;
namespace Society.Effects
{
    sealed class ScrollBarController : MonoBehaviour
    {
        [SerializeField] private ScrollRect sr;

        public void ResetScroll()
        {
            StartCoroutine(nameof(DelayBeforeNormalize));
        }
        /// <summary>
        /// тупо костыль - описание - скроллбар должен возвращатся в начало при смене оружия или нажатии на мод, но при смене оружия он не работает!
        /// Поэтому ожидание следующего кадра, и уже там нормализация.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayBeforeNormalize()
        {
            yield return null;
            sr.horizontalNormalizedPosition = 0;
            yield break;
        }
    }
}