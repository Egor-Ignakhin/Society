using System;
using UnityEngine;
using UnityEngine.UI;

class ScrollBarController : MonoBehaviour
{
    [SerializeField] private Scrollbar mscrollbar;
    
    public void ResetScroll()
    {
        mscrollbar.value = 0;        
    }    
}
