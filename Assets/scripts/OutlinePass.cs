using UnityEngine;
using UnityEngine.Experimental.Rendering.LightweightPipeline;

namespace GamingSense.Flint.Prot
{
	public class OutlinePass : MonoBehaviour, IAfterOpaquePass
	{
		public Shader OutlineShader;
		public Color OutlineColor;

		private OutlinePassImpl _outlinePass;

		public ScriptableRenderPass GetPassToEnqueue(RenderTextureDescriptor baseDescriptor, RenderTargetHandle colorAttachmentHandle, RenderTargetHandle depthAttachmentHandle)
		{
			return _outlinePass ?? (_outlinePass = new OutlinePassImpl(OutlineShader, OutlineColor));
		}
	}
}