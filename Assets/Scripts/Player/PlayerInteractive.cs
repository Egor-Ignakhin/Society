using Society.Inventory;
using Society.Inventory.Other;
using Society.Patterns;
using Society.Player.Controllers;
using Society.Settings;

using System.Linq;
using System.Text;

using UnityEngine;

namespace Society.Player
{
    /// <summary>
    /// Обработчик пользовательских взаимодействий
    /// </summary>
    public sealed class PlayerInteractive : MonoBehaviour
    {
        /// <summary>
        /// Камера игрока
        /// </summary>
        private Camera playerCamera;

        /// <summary>
        /// Максимальная дальность взаимодействия
        /// </summary>
        private const float maxInterctionDistance = 2;

        /// <summary>
        /// Размер сферы-зрителя коллайдеров
        /// </summary>
        private const float sphereCasterRadius = 0.01f;

        /// <summary>
        /// Comments?
        /// </summary>
        private const float sphSCastRadiusMultiply = 10;

        /// <summary>
        /// Кнопка взаимодействия нажата
        /// </summary>
        private bool interactionButtonPressed = false;

        /// <summary>
        /// Последнее место удара луча о коллайдер
        /// </summary>
        private Vector3 lastHitPoint;

        /// <summary>
        /// Список интерактивных объектов броска луча
        /// </summary>
        private InteractiveObject[] interactiveRayThrowingObjects = new InteractiveObject[0];

        /// <summary>
        /// Помощник пользователя - цветной круг
        /// </summary>
        private SupportItemCircular supportItemCircular;

        /// <summary>
        /// Текущее представление луч
        /// </summary>
        private Ray currentRay;

        /// <summary>
        /// Текущая точка контакта
        /// </summary>
        private Vector3 currentPointContact;

        /// <summary>
        /// Матрица слоёв взаимодействия
        /// </summary>
        [SerializeField] private LayerMask interactionLayers;

        /// <summary>
        /// Количество возможного инвентарного предмеиа
        /// </summary>
        int iICount = 1;

        /// <summary>
        /// Подсобное описание IO
        /// </summary>
        private StringBuilder iISubDescription = new StringBuilder();

        /// <summary>
        /// Главное описание IO
        /// </summary>
        private StringBuilder iIMainDescription = new StringBuilder();


        private void Start()
        {
            playerCamera = GetComponent<FirstPersonController>().GetCamera();
            supportItemCircular = FindObjectOfType<SupportItemCircular>();
        }

        private void Update()
        {
            //Если кнопка взаимодействия нажата
            if (Input.GetKeyDown(GameSettings.GetInteractionKeyCode()))
                interactionButtonPressed = true;

            //Если кнопка взаимодействия отпущена
            else if (Input.GetKeyUp(GameSettings.GetInteractionKeyCode()))
                interactionButtonPressed = false;
        }

        /// <summary>
        /// Интерактивный объект под лучом?
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        internal bool IOUnderTheBeam(InteractiveObject io) =>
             (interactiveRayThrowingObjects != null) && interactiveRayThrowingObjects.Contains(io);


        private void FixedUpdate()
        {
            //Создаём новый луч из центра экрана
            currentRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            //Бросам луч
            RayThrow();

            //Бросам луч для 
            RayThrowForActivateSuppurtCurcular();
        }
        
        private void RayThrow()
        {           
            //Очищаем описания II
            iISubDescription = iISubDescription.Clear();            
            iIMainDescription = iIMainDescription.Clear();

            //Обнуляем IRTO
            interactiveRayThrowingObjects = null;


            if (Physics.SphereCast(currentRay.origin, sphereCasterRadius,
                currentRay.direction, out RaycastHit hit, maxInterctionDistance, 
                interactionLayers, QueryTriggerInteraction.Ignore))
            {
                //Присваиваем IRTO IO компоненты луче-пресекателя
                interactiveRayThrowingObjects = hit.transform.GetComponents<InteractiveObject>();

                //Если размер IRTO больше 0
                if (interactiveRayThrowingObjects.Length > 0)
                {
                    //Указываем последнюю точку как текущую
                    lastHitPoint = hit.point;

                    //Проходимся по IRTO
                    for (int i = 0; i < interactiveRayThrowingObjects.Length; i++)
                    {

                        InteractiveObject currentIO = interactiveRayThrowingObjects[i];
                        string getDesc = currentIO.Description;
                        string getMainDesc = currentIO.MainDescription;

                        int getCount = currentIO is InventoryItem ? (currentIO as InventoryItem).GetCount() : 1;
                        if (!string.IsNullOrEmpty(getDesc))
                        {
                            iISubDescription = iISubDescription.Clear();
                            iIMainDescription = iIMainDescription.Clear();
                            iISubDescription.Append(getDesc);
                            iIMainDescription.Append(getMainDesc);
                            iICount = getCount;
                        }

                        
                        if (!interactionButtonPressed)
                            continue;

                        currentIO.Interact();
                        if (++i == interactiveRayThrowingObjects.Length)
                            interactionButtonPressed = false;
                    }
                }
                currentPointContact = hit.point;
            }
        }

        internal StringBuilder GetiISubDescription() => iISubDescription;

        internal StringBuilder GetiIMainDescription() => iIMainDescription;

        internal int GetIICount() => iICount;

        /// <summary>
        /// Задачётся и регулируется прозрачность 
        /// </summary>
        private void RayThrowForActivateSuppurtCurcular()
        {
            float tempOpacity = 0;
            //float max            
            if (Physics.SphereCast(currentRay.origin, sphereCasterRadius * sphSCastRadiusMultiply,
                currentRay.direction, out RaycastHit hit, maxInterctionDistance, interactionLayers, QueryTriggerInteraction.Ignore))// Бросок сфера удвоенного радиуса
            {
                if (hit.transform.GetComponent<InteractiveObject>())
                {
                    tempOpacity = 1 / Vector3.Distance(hit.point, currentPointContact) / 100;
                }
            }

            if (interactiveRayThrowingObjects != null && (interactiveRayThrowingObjects.Length > 0))
                supportItemCircular.SetColorByTypeOfInteractiveObject(interactiveRayThrowingObjects);
            supportItemCircular.SetSpriteOpacity(tempOpacity);
        }

        internal Vector3 GetLastHitPoint() => lastHitPoint;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            #region drawSphereForDefaultInteraction
            try
            {
                bool isHit = Physics.SphereCast(currentRay.origin, sphereCasterRadius, currentRay.direction, out RaycastHit hit, maxInterctionDistance, interactionLayers);
                if (isHit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(currentRay.origin + currentRay.direction * hit.distance, sphereCasterRadius);
                }
            }
            catch
            {
            }
            #endregion
            #region DrawSphereForSPCircularInteraction
            try
            {
                bool isHit = Physics.SphereCast(currentRay.origin, sphereCasterRadius * sphSCastRadiusMultiply, currentRay.direction, out RaycastHit hit, maxInterctionDistance, interactionLayers);
                if (isHit)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(currentRay.origin + currentRay.direction * hit.distance, sphereCasterRadius * sphSCastRadiusMultiply);
                }
            }
            catch
            {
            }
            #endregion
        }
#endif
    }
}