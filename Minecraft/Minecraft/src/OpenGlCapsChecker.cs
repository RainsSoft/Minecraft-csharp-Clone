namespace net.minecraft.src
{
	public class OpenGlCapsChecker
	{
		/// <summary>
		/// Whether or not we should try to check occlusion - defaults to false and is never changed in 1.2.2.
		/// </summary>
		private static bool TryCheckOcclusionCapable = true;

		public OpenGlCapsChecker()
		{
		}

		/// <summary>
		/// Checks if we support OpenGL occlusion.
		/// </summary>
		public static bool CheckARBOcclusion()
		{
            return true;// TryCheckOcclusionCapable && GraphicsContext.getCapabilities().GL_ARB_occlusion_query;
		}
	}
}