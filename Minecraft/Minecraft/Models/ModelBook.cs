using System;

namespace net.minecraft.src
{

	public class ModelBook : ModelBase
	{
		/// <summary>
		/// Right cover renderer (when facing the book) </summary>
		public ModelRenderer CoverRight;

		/// <summary>
		/// Left cover renderer (when facing the book) </summary>
		public ModelRenderer CoverLeft;

		/// <summary>
		/// The right pages renderer (when facing the book) </summary>
		public ModelRenderer PagesRight;

		/// <summary>
		/// The left pages renderer (when facing the book) </summary>
		public ModelRenderer PagesLeft;

		/// <summary>
		/// Right cover renderer (when facing the book) </summary>
		public ModelRenderer FlippingPageRight;

		/// <summary>
		/// Right cover renderer (when facing the book) </summary>
		public ModelRenderer FlippingPageLeft;

		/// <summary>
		/// The renderer of spine of the book </summary>
		public ModelRenderer BookSpine;

		public ModelBook()
		{
			CoverRight = (new ModelRenderer(this)).SetTextureOffset(0, 0).AddBox(-6F, -5F, 0.0F, 6, 10, 0);
			CoverLeft = (new ModelRenderer(this)).SetTextureOffset(16, 0).AddBox(0.0F, -5F, 0.0F, 6, 10, 0);
			BookSpine = (new ModelRenderer(this)).SetTextureOffset(12, 0).AddBox(-1F, -5F, 0.0F, 2, 10, 0);
			PagesRight = (new ModelRenderer(this)).SetTextureOffset(0, 10).AddBox(0.0F, -4F, -0.99F, 5, 8, 1);
			PagesLeft = (new ModelRenderer(this)).SetTextureOffset(12, 10).AddBox(0.0F, -4F, -0.01F, 5, 8, 1);
			FlippingPageRight = (new ModelRenderer(this)).SetTextureOffset(24, 10).AddBox(0.0F, -4F, 0.0F, 5, 8, 0);
			FlippingPageLeft = (new ModelRenderer(this)).SetTextureOffset(24, 10).AddBox(0.0F, -4F, 0.0F, 5, 8, 0);
			CoverRight.SetRotationPoint(0.0F, 0.0F, -1F);
			CoverLeft.SetRotationPoint(0.0F, 0.0F, 1.0F);
			BookSpine.RotateAngleY = ((float)Math.PI / 2F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			CoverRight.Render(par7);
			CoverLeft.Render(par7);
			BookSpine.Render(par7);
			PagesRight.Render(par7);
			PagesLeft.Render(par7);
			FlippingPageRight.Render(par7);
			FlippingPageLeft.Render(par7);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			float f = (MathHelper2.Sin(par1 * 0.02F) * 0.1F + 1.25F) * par4;
			CoverRight.RotateAngleY = (float)Math.PI + f;
			CoverLeft.RotateAngleY = -f;
			PagesRight.RotateAngleY = f;
			PagesLeft.RotateAngleY = -f;
			FlippingPageRight.RotateAngleY = f - f * 2.0F * par2;
			FlippingPageLeft.RotateAngleY = f - f * 2.0F * par3;
			PagesRight.RotationPointX = MathHelper2.Sin(f);
			PagesLeft.RotationPointX = MathHelper2.Sin(f);
			FlippingPageRight.RotationPointX = MathHelper2.Sin(f);
			FlippingPageLeft.RotationPointX = MathHelper2.Sin(f);
		}
	}

}