using System.Collections;
using UnityEngine;

namespace Alim
{
    public class FinalCamera : MonoBehaviour
    {
        private Camera finalCamera;
        public GameObject finalCameraHolder;

        private Camera mainCamera;
        public GameObject mainCameraHolder;

        const float RENDER_FPS = 60f;

        private int renderingFront;

        private float dt;

        private const int DefaultFps = 60;

        [SerializeField]
        private RenderTexture renderTexture;

        public Material finalMaterial;

        private bool initialized = false;

        private void OnEnable()
        {
            Debug.Log("OnEnable");

            StartCoroutine(Initialize());    
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");

            if (mainCamera != null)
            {
                mainCamera.targetTexture = null;
                mainCamera.enabled = true;
            }

            if (finalCamera != null)
            {
                finalCamera.enabled = false;
            }
        }


        private IEnumerator Initialize()
        {
            if (finalCameraHolder == null)
                finalCameraHolder = this.gameObject;

            finalCamera = finalCameraHolder.GetComponent<Camera>();
            finalCamera.enabled = false;

            Application.targetFrameRate = (int)RENDER_FPS;

            //DontDestroyOnLoad(gameObject);

            renderingFront = 0;

            SetFps(DefaultFps);

            if (mainCameraHolder == null)
            {
                mainCamera = Camera.main;
            }
            else
            {
                mainCamera = mainCameraHolder.GetComponent<Camera>();
            }

            int rw = 1920;
            int rh = 1080;

            if (renderTexture == null)
            {
                renderTexture = new RenderTexture(rw, rh, 24, RenderTextureFormat.ARGB32);
                renderTexture.Create();
            }

            mainCamera.targetTexture = renderTexture;
            finalMaterial.mainTexture = renderTexture;

            yield return new WaitForEndOfFrame();

            finalCamera.enabled = true;

            mainCamera.enabled = false;

            initialized = true;
        }

        public void SetFps(int fps)
        {
            dt = 1.0f / (float)fps;
        }
    }
}