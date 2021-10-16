using System.IO;

#if UNITY_EDITOR
using UnityEditor;

#endif
using UnityEngine;
namespace Society.Effects
{
    internal sealed class ImageFromCameraRender : MonoBehaviour
    {
#if UNITY_EDITOR
        private Camera cam;
        [SerializeField] private bool isItem;
        private void Start()
        {
            cam = GetComponent<Camera>();
            RenderTexture currentRT = RenderTexture.active;

            RenderTexture rt = isItem ? new RenderTexture(256, 256, 64) : new RenderTexture(3840, 2160, 4096);

            // Устанавливаем созданную текстуру как целевую
            RenderTexture.active = rt;
            cam.targetTexture = rt;

            // Принудительно вызываем рендер камеры
            cam.Render();

            // Получаем обычную текстуру из RenderTexture'ы
            // Ее можно будет использовать в игре, или же
            // Сохранить, что мы и сделаем
            Texture2D image = new Texture2D(rt.width, rt.height);
            image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            image.Apply();

            // Пишем текстуру в .png файл
            byte[] bytes = image.EncodeToPNG();
            File.WriteAllBytes(Directory.GetCurrentDirectory() + "/Assets/NormalImages/ImageFromCameraRender/" + "render" + ".png", bytes);

            // Восстанавливаем рендер таргет
            RenderTexture.active = currentRT;

            // Чистим все (при необходимости)
            Destroy(image);
        }

        [MenuItem("Tools/Create Render Camera")]
        private static void CreateCamera()
        {
            var cgm = Instantiate(Resources.Load<ImageFromCameraRender>("Editor\\ImageFromCameraRender"));
            cgm.transform.rotation = SceneView.lastActiveSceneView.rotation;

            Selection.activeObject = cgm;
        }
#endif
    }
}