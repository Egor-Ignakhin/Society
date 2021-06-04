using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBarController : MonoBehaviour
{
    [SerializeField] private Transform movableObject;
    public void OnMoveSlider(float v)
    {
        movableObject.localPosition = new Vector2( 37.5f* -v * movableObject.childCount, 0);
    }
}
