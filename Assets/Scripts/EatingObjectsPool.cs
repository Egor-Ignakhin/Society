using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EatingObjectsPool
{
    private static List<EatingObject> eatingObjects = new List<EatingObject>();// свободные объекты
    public enum type { milk};
    public static void ToPool(EatingObject eatingObject)
    {
        eatingObjects.Add(eatingObject);// добавление в список свободных об.
        eatingObject.gameObject.SetActive(false);// выключение
        eatingObject.transform.position = Vector3.zero;// перемещение в начало координат
        //Debug.Log("add in pool");
    }
    public static GameObject FromPool(type t)
    {        
        if (eatingObjects.Count > 0)// если список не пуст
        {            
            for (int i = 0; i< eatingObjects.Count; i++)
            {
                if(eatingObjects[i].type == t)// если нашёлся нужный объект
                {
                    var retG = eatingObjects[eatingObjects.Count - 1];// ссылка на найденный объект
                    eatingObjects.Remove(retG);// удаление об. из списка
                    //Debug.Log("take at pool");
                    retG.gameObject.SetActive(true);
                    return retG.gameObject;// возвращение об.
                }
            }            
        }
         return CreateNewObject(t);//иначе создать новый объект
    }
    private static GameObject CreateNewObject(type t)
    {
        //Debug.Log("create new object");
        switch (t)
        {
            case type.milk:
                return Object.Instantiate(Resources.Load<GameObject>("MilkBottle"));
            default:
                Debug.LogError("Error instantiate!");
                return null;
        }
    }
}
