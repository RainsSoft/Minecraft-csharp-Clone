using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class ModelBase
	{
		public float OnGround;
		public bool IsRiding;

		/// <summary>
		/// This is a list of all the boxes (ModelRenderer.class) in the current model.
		/// </summary>
		public List<ModelRenderer> BoxList;
		public bool IsChild;

		/// <summary>
		/// A mapping for all texture offsets </summary>
		private Dictionary<string, TextureOffset> ModelTextureMap;
		public int TextureWidth;
		public int TextureHeight;

		public ModelBase()
		{
			IsRiding = false;
            BoxList = new List<ModelRenderer>();
			IsChild = true;
            ModelTextureMap = new Dictionary<string, TextureOffset>();
			TextureWidth = 64;
			TextureHeight = 32;
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public virtual void Render(Entity entity, float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public virtual void SetRotationAngles(float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public virtual void SetLivingAnimations(EntityLiving entityliving, float f, float f1, float f2)
		{
		}

		protected virtual void SetTextureOffset(string par1Str, int par2, int par3)
		{
			ModelTextureMap[par1Str] = new TextureOffset(par2, par3);
		}

		public virtual TextureOffset GetTextureOffset(string par1Str)
		{
			return ModelTextureMap[par1Str];
		}
	}
}