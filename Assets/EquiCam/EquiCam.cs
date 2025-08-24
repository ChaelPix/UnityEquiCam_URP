// Filename: EquiCam.cs
using UnityEngine;
using UnityEngine.Rendering;

namespace BodhiDonselaar
{
    [RequireComponent(typeof(Camera))]
    public class EquiCam : MonoBehaviour
    {
        public Material equiMaterial; 
        [SerializeField]
        private Size RenderResolution = Size.Default;

        [HideInInspector]
        public RenderTexture cubemap;

        private Camera cam;
        private GameObject child;

        public enum Size
        {
            High = 2048,
            Default = 1024,
            Low = 512,
            Minimum = 256
        }

        void OnEnable()
        {
            if (child == null)
            {
                child = new GameObject("CubemapCamera");
                child.hideFlags = HideFlags.HideAndDontSave;
                child.transform.SetParent(transform);
                child.transform.localPosition = Vector3.zero;
                child.transform.localRotation = Quaternion.identity;
                cam = child.AddComponent<Camera>();
                cam.CopyFrom(GetComponent<Camera>());
                cam.cullingMask = GetComponent<Camera>().cullingMask;
                child.SetActive(false);
            }
            
            NewCubemap();
        }

        void OnDisable()
        {
            if (child != null) DestroyImmediate(child);
            if (cubemap != null)
            {
                cubemap.Release();
                DestroyImmediate(cubemap);
            }
        }
        
        void LateUpdate()
        {
            if (cam == null || equiMaterial == null) return;
            
            if (cubemap.width != (int)RenderResolution)
            {
                NewCubemap();
            }

            cam.transform.position = transform.position;
            cam.RenderToCubemap(cubemap);

            equiMaterial.SetTexture("_MainTex", cubemap);
            equiMaterial.SetFloat("FORWARD", transform.eulerAngles.y * Mathf.Deg2Rad);
        }

        private void NewCubemap()
        {
            if (cubemap != null)
            {
                cubemap.Release();
                DestroyImmediate(cubemap);
            }

            cubemap = new RenderTexture((int)RenderResolution, (int)RenderResolution, 16, RenderTextureFormat.Default);
            cubemap.dimension = TextureDimension.Cube;
            cubemap.hideFlags = HideFlags.HideAndDontSave;
            cubemap.Create();
        }
    }
}