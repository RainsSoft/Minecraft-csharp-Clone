namespace net.minecraft.src
{

	public class ModelMagmaCube : ModelBase
	{
		ModelRenderer[] Field_40345_a;
		ModelRenderer Field_40344_b;

		public ModelMagmaCube()
		{
			Field_40345_a = new ModelRenderer[8];

			for (int i = 0; i < Field_40345_a.Length; i++)
			{
				sbyte byte0 = 0;
				int j = i;

				if (i == 2)
				{
					byte0 = 24;
					j = 10;
				}
				else if (i == 3)
				{
					byte0 = 24;
					j = 19;
				}

				Field_40345_a[i] = new ModelRenderer(this, byte0, j);
				Field_40345_a[i].AddBox(-4F, 16 + i, -4F, 8, 1, 8);
			}

			Field_40344_b = new ModelRenderer(this, 0, 16);
			Field_40344_b.AddBox(-2F, 18F, -2F, 4, 4, 4);
		}

		public virtual int Func_40343_a()
		{
			return 5;
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			EntityMagmaCube entitymagmacube = (EntityMagmaCube)par1EntityLiving;
			float f = entitymagmacube.Field_767_b + (entitymagmacube.Field_768_a - entitymagmacube.Field_767_b) * par4;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			for (int i = 0; i < Field_40345_a.Length; i++)
			{
				Field_40345_a[i].RotationPointY = (float)(-(4 - i)) * f * 1.7F;
			}
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			Field_40344_b.Render(par7);

			for (int i = 0; i < Field_40345_a.Length; i++)
			{
				Field_40345_a[i].Render(par7);
			}
		}
	}

}