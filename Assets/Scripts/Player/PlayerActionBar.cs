using UnityEngine;
namespace PlayerClasses
{
    /// <summary>
    /// аниматор полоски здоровья(утрата)
    /// </summary>
    sealed class PlayerActionBar : MonoBehaviour
    {
        internal void SetVisible(bool v) => gameObject.SetActive(v);
    }
}