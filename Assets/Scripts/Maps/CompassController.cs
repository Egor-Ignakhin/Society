using Society.Player.Controllers;

using UnityEngine;
namespace Society.Maps
{
    /// <summary>
    /// класс-контроллёр для компаса
    /// </summary>
    sealed class CompassController : MonoBehaviour
    {
        private Transform player;
        private void Awake()
        {
            player = FindObjectOfType<FirstPersonController>().transform;
        }
        private void FixedUpdate()
        {
            transform.localEulerAngles = new Vector3(0, 0, player.eulerAngles.y);
        }
    }
}