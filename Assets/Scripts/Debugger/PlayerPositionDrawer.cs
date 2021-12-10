
using Society.Player.Controllers;

using UnityEngine;

namespace Society.Debugger
{
    /// <summary>
    /// �������� ������������ ������� ������� ������ 
    /// � ������� ��������� � ���� �������
    /// </summary>
    internal sealed class PlayerPositionDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI text;
        private Transform player;
        private void Start() => player = FindObjectOfType<FirstPersonController>().transform;
        private void Update() => text.text = $"XYZ: {player.position}";
    }
}