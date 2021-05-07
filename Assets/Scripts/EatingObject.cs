using UnityEngine;

public sealed class EatingObject : InteractiveObject
{
    [SerializeField] private int thirstNutrition;
    [SerializeField] private int foodNutrition;

    private int defaultThirstNutrition;
    private int defaultFoodNutrition;

    public EatingObjectsPool.type type;
    protected override void Awake()
    {
        defaultThirstNutrition = thirstNutrition;
        defaultFoodNutrition = foodNutrition;
        SetType("Milk");
    }
    public override void Interact(PlayerClasses.PlayerStatements player)
    {
      //  player.SendMessage(PlayerClasses.PlayerStatements.Message.meal, this);
        //TODO pool of objects
        ReturnToPool();
    }
    public int GetThirst()
    {
        return thirstNutrition;
    }
    public int GetFood()
    {
        return foodNutrition;
    }
    private void ReturnToPool()
    {
        thirstNutrition = defaultThirstNutrition;
        foodNutrition = defaultFoodNutrition;
        EatingObjectsPool.ToPool(this);

        //for dev
        MealSpawner.CountOnScene--;
    }
}
