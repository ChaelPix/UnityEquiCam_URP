// Filename: EquiCamRenderFeature.cs
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EquiCamRenderFeature : ScriptableRendererFeature
{
    class EquiCamPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryTexture;

        public EquiCamPass(Material material)
        {
            this.material = material;
            temporaryTexture.Init("_TemporaryColorTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            source = renderingData.cameraData.renderer.cameraColorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("EquiCam Pass");

            Blit(cmd, source, source, material, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    private EquiCamPass equiCamPass;
    private Material equiCamMaterial;

    public override void Create()
    {
        var equiCamComponent = FindObjectOfType<BodhiDonselaar.EquiCam>();
        if (equiCamComponent != null && equiCamComponent.equiMaterial != null)
        {
            equiCamMaterial = equiCamComponent.equiMaterial;
            equiCamPass = new EquiCamPass(equiCamMaterial);
            
            equiCamPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (equiCamPass != null)
        {
            renderer.EnqueuePass(equiCamPass);
        }
    }
}