namespace net.minecraft.src
{

	public class ModelSign : ModelBase
	{
		/// <summary>
		/// The board on a sign that has the writing on it. </summary>
		public ModelRenderer SignBoard;

		/// <summary>
		/// The stick a sign stands on. </summary>
		public ModelRenderer SignStick;

		public ModelSign()
		{
			SignBoard = new ModelRenderer(this, 0, 0);
			SignBoard.AddBox(-12F, -14F, -1F, 24, 12, 2, 0.0F);
			SignStick = new ModelRenderer(this, 0, 14);
			SignStick.AddBox(-1F, -2F, -1F, 2, 14, 2, 0.0F);
		}

		/// <summary>
		/// Renders the sign model through TileEntitySignRenderer
		/// </summary>
		public virtual void RenderSign()
		{
			SignBoard.Render(0.0625F);
			SignStick.Render(0.0625F);
		}
	}

}