using UnityEngine;
namespace Society.Player
{
    /// <summary>
    /// аниматор полоски здоровья(утрата)
    /// </summary>
    internal sealed class PlayerActionBar : MonoBehaviour
    {
        internal void SetVisible(bool v) => gameObject.SetActive(v);
    }
}