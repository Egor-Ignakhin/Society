using System;
using System.Collections.Generic;
using UnityEngine;

public class MapOfWorldCanvas : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> renderers = new List<MonoBehaviour>();
    [SerializeField] private List<MonoBehaviour> binoculeRenderers = new List<MonoBehaviour>();

    internal void EnableAllWithoutBunocule()
    {
        foreach (var r in renderers)
            r.enabled = true;
    }

    internal void DisableAllWithoutBunocule()
    {
        foreach (var r in renderers)
            r.enabled = false;
    }

    internal void SetVisible(bool v)
    {
        foreach (var r in renderers)
            r.enabled = v;
        foreach (var br in binoculeRenderers)
        {
            br.enabled = v;
        }
    }
}
