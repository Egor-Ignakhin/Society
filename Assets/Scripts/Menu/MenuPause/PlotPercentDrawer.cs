
using Society.Missions;

using UnityEngine;

namespace Society.Menu.MenuPause
{
    /// <summary>
    /// �������� �������� ����������� ������
    /// </summary>
    internal sealed class PlotPercentDrawer : MonoBehaviour
    {

        [SerializeField] private TMPro.TextMeshProUGUI drawableText;

        private void OnEnable() => ReDraw();

        private void ReDraw()
        {
            int percent = Plot.CalculateCompletedPercent();
            drawableText.SetText($"�������� {percent}%");
        }
    }
}