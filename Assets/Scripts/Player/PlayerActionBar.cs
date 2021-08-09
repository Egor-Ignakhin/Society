using PlayerClasses;
using System;
using UnityEngine;

/// <summary>
/// аниматор полоски здоровья(утрата)
/// </summary>
class PlayerActionBar : MonoBehaviour
{
    [SerializeField] private RectTransform healthAnimatedBar;
    private BasicNeeds playerBn;

    private float currentHP;
    private Vector2 nextSize;
    private Vector3 nextPos;
    private readonly float animateSpeed = 3.5f;// скорость анимации
    private float timeFromLastChangeHP;// время перед анимацией
    private float lastHp = 80;
    private void Awake()
    {
        playerBn = FindObjectOfType<BasicNeeds>();
        playerBn.HealthChangeValue += OnChangeHealth;
    }
    private void Update() => HealthAnimate();
    private void HealthAnimate()
    {
        return;
        if ((timeFromLastChangeHP -= Time.deltaTime) > 0)
            return;

        healthAnimatedBar.localPosition = Vector3.MoveTowards(healthAnimatedBar.localPosition, nextPos, 1 / animateSpeed);
        healthAnimatedBar.sizeDelta = Vector2.MoveTowards(healthAnimatedBar.sizeDelta, nextSize, 1 / animateSpeed);
    }

    internal void SetVisible(bool v)
    {
        gameObject.SetActive(v);
    }

    private void OnChangeHealth(float v)
    {
        lastHp = currentHP;
        currentHP = v;
        if (v < lastHp)
            timeFromLastChangeHP = 2;
        float max = 100 / playerBn.MaximumHealth;
        nextSize = new Vector2(currentHP * max, 100);
        nextPos = new Vector3(max * currentHP - 50, 0);
    }
    private void OnDestroy()
    {
        if (playerBn)
            playerBn.HealthChangeValue -= OnChangeHealth;
    }
}
