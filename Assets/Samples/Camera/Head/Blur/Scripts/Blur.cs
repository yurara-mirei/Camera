using UnityEngine;

namespace Yurara
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class Blur : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 4f)]
        float intensity = 1f;

        Camera camera;

        [SerializeField]
        Material material;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            bool doFinalBlur = intensity > 0;

            if (!doFinalBlur)
            {
                Graphics.Blit(source, destination, material);
                return;
            }

            int size;

            if (intensity < 1f)
            {
                size = (int)Mathf.Lerp(camera.pixelWidth, 512, intensity);
            }
            else
            {
                throw new System.NotImplementedException();
            }

            float aspectRatio = (float)source.height / source.width;

            RenderTextureDescriptor descriptor = source.descriptor;
            descriptor.width = size;
            descriptor.height = Mathf.Max(1, (int)(size * aspectRatio));

            RenderTexture renderTexture = RenderTexture.GetTemporary(descriptor);

            float blurScale = intensity > 1f ? 1f : intensity;


        }

        private void OnEnable()
        {
            camera = GetComponent<Camera>();
        }
    }
}