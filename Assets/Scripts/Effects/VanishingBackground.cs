using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingBackground : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float delayBeforeAnimate;
    [SerializeField] private UnityEngine.UI.Image img;
    [SerializeField] private GameObject disablableObject;
    [SerializeField] private List<GameObject> enabledComponents = new List<GameObject>();
    private void Update()
    {
        if ((delayBeforeAnimate -= Time.deltaTime) > 0)
            return;
        LerpColor();
    }
    private void LerpColor()
    {
        img.color -= new Color(0, 0, 0, Time.deltaTime * speed);
        if (img.color.a < 0)
        {            
            gameObject.SetActive(false);
            disablableObject.SetActive(false);
            foreach (var c in enabledComponents)
                c.SetActive(true);
        }
    }
}
