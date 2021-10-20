using Society.GameScreens;
using Society.Shoot;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Society.Menu.GameOverlay
{
    public sealed class GameOverlayManager : MonoBehaviour, IGameScreen
    {
        [SerializeField] private GameObject menuChangeBulletType;

        private readonly Dictionary<GameOverlayType, Action<bool>> gameOverlayCallableActions = new Dictionary<GameOverlayType, Action<bool>>();
        private Action<bool> activeAction;
        private Action<string> CALLBACK;
        private void Awake()
        {
            #region Инициализация вызываемых методов

            gameOverlayCallableActions.Add(GameOverlayType.ChangeBullet, EnableMenuChangeBulletType);

            #endregion

            #region Выключение всех меню

            menuChangeBulletType.SetActive(false);

            #endregion
        }
        internal void SetEnableMenu(GameOverlayType changeBullet, bool isActive, Action<string> callback = null)
        {
            gameOverlayCallableActions[changeBullet].Invoke(isActive);

            ScreensManager.SetScreen(isActive ? this : null);

            activeAction = isActive ? gameOverlayCallableActions[changeBullet] : null;

            CALLBACK = callback;
        }

        private void EnableMenuChangeBulletType(bool isActive)
        {
            menuChangeBulletType.SetActive(isActive);
        }

        public void SetTypeBulletDefault()
        {            
            CALLBACK?.Invoke(BulletType.Default.ToString());
        }
        public void SetTypeBulletElectric()
        {            
            CALLBACK?.Invoke("Electric");
        }

        public bool Hide()
        {
            if (activeAction != null)
                activeAction.Invoke(false);

            return true;
        }

        public KeyCode HideKey()
        {
            return KeyCode.Escape;
        }
    }
}