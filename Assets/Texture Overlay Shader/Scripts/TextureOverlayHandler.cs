using UnityEngine;
using Random = UnityEngine.Random;

namespace Chars.Tools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    public class TextureOverlayHandler : MonoBehaviour
    {
        public MeshRenderer Renderer;
        public float MaxOffset = 1f;
        public float MinScale = 0.5f;
        public float MaxScale = 2f;
        public bool RandomizeInAwake = true;

        public void Awake()
        {
            Renderer = GetComponent<MeshRenderer>();

            if (RandomizeInAwake)
            {
                Randomize();
            }
        }

        /// <summary>
        /// if you want to randomize from the context menu in unity (three vertical points at the side to the script)
        /// WARNING: You can get an error from the console, but nothing to be worried
        /// THIS Error you will get -> "Instantiating material due to calling renderer.material during edit mode. 
        /// This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead." 
        /// if you use shared material you will change over all the objects with the same material. 
        /// </summary>
        [ContextMenu("Randomize")]
        public void Randomize()
        {
            // Generate random offsets for each overlay texture
            Vector4 overlay1Offset = GenerateRandomOffset();
            Vector4 overlay2Offset = GenerateRandomOffset();
            Vector4 overlay3Offset = GenerateRandomOffset();

            // Set the offsets in the material
            Renderer.material.SetVector("_Overlay1Offset", overlay1Offset);
            Renderer.material.SetVector("_Overlay2Offset", overlay2Offset);
            Renderer.material.SetVector("_Overlay3Offset", overlay3Offset);

            Vector4 overlay1Scale = GenerateRandomScale();
            Vector4 overlay2Scale = GenerateRandomScale();
            Vector4 overlay3Scale = GenerateRandomScale();

            // Set the scales in the material
            Renderer.material.SetVector("_Overlay1Scale", overlay1Scale);
            Renderer.material.SetVector("_Overlay2Scale", overlay2Scale);
            Renderer.material.SetVector("_Overlay3Scale", overlay3Scale);
        }

        private Vector4 GenerateRandomScale()
        {
            return GenerateXYRandomVector4(MinScale, MaxScale, 1, 1);
        }

        private Vector4 GenerateRandomOffset()
        {
            return GenerateXYRandomVector4(-MaxOffset, MaxOffset, 0, 0);
        }

        private Vector4 GenerateXYRandomVector4(float min, float max, float z, float w)
        {
            float offsetX = Random.Range(min, max);
            float offsetY = Random.Range(min, max);
            return new Vector4(offsetX, offsetY, z, w);
        }
    }
}
