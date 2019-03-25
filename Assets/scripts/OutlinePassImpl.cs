using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.LightweightPipeline;

namespace GamingSense.Flint.Prot
{
	public class OutlinePassImpl : ScriptableRenderPass
	{
		private readonly Material _outlineMaterial;
		private readonly FilterRenderersSettings _outlineFilterSettings;

		public OutlinePassImpl(Color outlineColor)
		{
			// Должно совпадать с тегом прохода шейдера, висящем на объекте, как в шейдере SimpleColor
			RegisterShaderPassName("LightweightForward");
			// Соответствует имени outline shader, указанному выше
			_outlineMaterial = CoreUtils.CreateEngineMaterial("GS/SimpleOutline");

			var outlineColorId = Shader.PropertyToID("_OutlineColor");
			_outlineMaterial.SetColor(outlineColorId, outlineColor);
			//_outlineMaterial.renderQueue = 1;

			_outlineFilterSettings = new FilterRenderersSettings(true)
			{
				renderQueueRange = RenderQueueRange.opaque,
				renderingLayerMask = 1 << 1
			};
		}

		public override void Execute(ScriptableRenderer renderer, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			var camera = renderingData.cameraData.camera;
			var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			var drawSettings = CreateDrawRendererSettings(camera, sortFlags, RendererConfiguration.None, renderingData.supportsDynamicBatching);
			drawSettings.SetOverrideMaterial(_outlineMaterial, 0);
			context.DrawRenderers(renderingData.cullResults.visibleRenderers, ref drawSettings, _outlineFilterSettings);
		}
	}
}