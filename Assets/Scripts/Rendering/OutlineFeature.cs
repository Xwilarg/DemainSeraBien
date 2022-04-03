using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class OutlineFeature : ScriptableRendererFeature
{
	class RenderObjectsPass : ScriptableRenderPass
	{
		private readonly string tag;

		public Material overrideMaterial { get; set; }
		public int overrideMaterialPassIndex { get; set; }

		private FilteringSettings filteringSettings;
		private readonly List<ShaderTagId> shaderTagIds;
		private RenderStateBlock renderStateBlock;

		private RenderTargetHandle destination;


		public RenderObjectsPass(string tag, int layerMask)
		{
			this.tag = tag;

			filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

			shaderTagIds = new List<ShaderTagId> {
				new ShaderTagId("UniversalForward"),
				new ShaderTagId("LightweightForward"),
				new ShaderTagId("SRPDefaultUnlit")
			};

			renderStateBlock = new RenderStateBlock(RenderStateMask.Depth);
			renderStateBlock.depthState = new DepthState(true, CompareFunction.LessEqual);
		}

		public void Setup(RenderTargetHandle destination)
		{
			this.destination = destination;
		}

		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (destination != RenderTargetHandle.CameraTarget)
			{
				RenderTextureDescriptor descriptor = cameraTextureDescriptor;
				descriptor.colorFormat = RenderTextureFormat.R8;
				descriptor.depthBufferBits = 24;
				descriptor.mipCount = 1;

				cmd.GetTemporaryRT(destination.id, descriptor);
			}

			ConfigureTarget(destination.Identifier(), destination.Identifier());
			ConfigureClear(ClearFlag.All, Color.black);
		}

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;

			DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIds, ref renderingData, sortingCriteria);
			drawingSettings.overrideMaterial = overrideMaterial;
			drawingSettings.overrideMaterialPassIndex = overrideMaterialPassIndex;

			CommandBuffer cmd = CommandBufferPool.Get(tag);
			using (new ProfilingScope(cmd, profilingSampler))
			{
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
			}

			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		public override void FrameCleanup(CommandBuffer cmd)
		{
			if (destination != RenderTargetHandle.CameraTarget)
				cmd.ReleaseTemporaryRT(destination.id);
		}
	}

	class OutlinePass : ScriptableRenderPass
	{
		private readonly string tag;
        private readonly Material material;

        private RenderTargetIdentifier source;
        private RenderTargetHandle destination;
        private RenderTargetHandle objectsToOutline;
        private RenderTargetHandle temporaryColorTexture;


        public OutlinePass(string tag, Material material)
        {
            this.tag = tag;
            this.material = material;

            temporaryColorTexture.Init(tag + "_Temp");
        }

        public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination, RenderTargetHandle objectsToOutline)
        {
            this.source = source;
            this.destination = destination;
            this.objectsToOutline = objectsToOutline;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            ConfigureTarget(destination.Identifier());
            ConfigureClear(ClearFlag.Color, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(tag);

            using (new ProfilingScope(cmd, profilingSampler))
            {
	            cmd.SetGlobalTexture("_ObjectsToOutlineTexture", objectsToOutline.Identifier());

                if (destination == RenderTargetHandle.CameraTarget)
                {
                    RenderTextureDescriptor opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                    opaqueDescriptor.depthBufferBits = 0;

                    cmd.GetTemporaryRT(temporaryColorTexture.id, opaqueDescriptor, FilterMode.Point);
                    Blit(cmd, source, temporaryColorTexture.Identifier(), material);
                    Blit(cmd, temporaryColorTexture.Identifier(), source);
                }
                else
                {
                    Blit(cmd, source, destination.Identifier(), material);
                }
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (destination == RenderTargetHandle.CameraTarget)
                cmd.ReleaseTemporaryRT(temporaryColorTexture.id);
        }
	}

	[Serializable]
	public class Settings
	{
		public RenderPassEvent renderObjectsPassEvent = RenderPassEvent.AfterRenderingOpaques;
		public Material renderObjectsMaterial;
		public LayerMask layerMask = 0;

		public RenderPassEvent outlinesPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
		public Material outlineMaterial;
	}

	public Settings settings = new Settings();

	private RenderObjectsPass renderObjectsPass;
	private RenderTargetHandle renderObjectsTextureHandle;

	private OutlinePass outlinePass;


	public override void Create()
	{
		renderObjectsPass = new RenderObjectsPass(name + ".RenderObjects", settings.layerMask) {
			renderPassEvent = settings.renderObjectsPassEvent
		};

		renderObjectsPass.overrideMaterial = settings.renderObjectsMaterial;
		renderObjectsPass.overrideMaterialPassIndex = 0;

		renderObjectsTextureHandle.Init(name + "_RenderObjects");

		outlinePass = new OutlinePass(name + ".Outline", settings.outlineMaterial) {
			renderPassEvent = settings.outlinesPassEvent
		};
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderObjectsPass.Setup(renderObjectsTextureHandle);
		renderer.EnqueuePass(renderObjectsPass);

		outlinePass.Setup(renderer.cameraColorTarget, RenderTargetHandle.CameraTarget, renderObjectsTextureHandle);
		renderer.EnqueuePass(outlinePass);
	}
}
