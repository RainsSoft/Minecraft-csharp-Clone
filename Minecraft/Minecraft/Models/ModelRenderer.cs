using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class ModelRenderer
	{
		/// <summary>
		/// The size of the texture file's width in pixels. </summary>
		public float TextureWidth;

		/// <summary>
		/// The size of the texture file's height in pixels. </summary>
		public float TextureHeight;

		/// <summary>
		/// The X offset into the texture used for displaying this model </summary>
		private int TextureOffsetX;

		/// <summary>
		/// The Y offset into the texture used for displaying this model </summary>
		private int TextureOffsetY;
		public float RotationPointX;
		public float RotationPointY;
		public float RotationPointZ;
		public float RotateAngleX;
		public float RotateAngleY;
		public float RotateAngleZ;
		private bool Compiled;

		/// <summary>
		/// The GL display list rendered by the Tessellator for this model </summary>
		private int DisplayList;
		public bool Mirror;
		public bool ShowModel;

		/// <summary>
		/// Hides the model. </summary>
		public bool IsHidden;
		public List<ModelBox> CubeList;
		public List<ModelRenderer> ChildModels;
		public readonly string BoxName;
		private ModelBase BaseModel;

		public ModelRenderer(ModelBase par1ModelBase, string par2Str)
		{
			TextureWidth = 64F;
			TextureHeight = 32F;
			Compiled = false;
			DisplayList = 0;
			Mirror = false;
			ShowModel = true;
			IsHidden = false;
            CubeList = new List<ModelBox>();
			BaseModel = par1ModelBase;
			par1ModelBase.BoxList.Add(this);
			BoxName = par2Str;
			SetTextureSize(par1ModelBase.TextureWidth, par1ModelBase.TextureHeight);
		}

		public ModelRenderer(ModelBase par1ModelBase) : this(par1ModelBase, null)
		{
		}

		public ModelRenderer(ModelBase par1ModelBase, int par2, int par3) : this(par1ModelBase)
		{
			SetTextureOffset(par2, par3);
		}

		/// <summary>
		/// Sets the current box's rotation points and rotation angles to another box.
		/// </summary>
		public virtual void AddChild(ModelRenderer par1ModelRenderer)
		{
			if (ChildModels == null)
			{
                ChildModels = new List<ModelRenderer>();
			}

			ChildModels.Add(par1ModelRenderer);
		}

		public virtual ModelRenderer SetTextureOffset(int par1, int par2)
		{
			TextureOffsetX = par1;
			TextureOffsetY = par2;
			return this;
		}

		public virtual ModelRenderer AddBox(string par1Str, float par2, float par3, float par4, int par5, int par6, int par7)
		{
			par1Str = (new StringBuilder()).Append(BoxName).Append(".").Append(par1Str).ToString();
			TextureOffset textureoffset = BaseModel.GetTextureOffset(par1Str);
			SetTextureOffset(textureoffset.Field_40734_a, textureoffset.Field_40733_b);
			CubeList.Add((new ModelBox(this, TextureOffsetX, TextureOffsetY, par2, par3, par4, par5, par6, par7, 0.0F)).Func_40671_a(par1Str));
			return this;
		}

		public virtual ModelRenderer AddBox(float par1, float par2, float par3, int par4, int par5, int par6)
		{
			CubeList.Add(new ModelBox(this, TextureOffsetX, TextureOffsetY, par1, par2, par3, par4, par5, par6, 0.0F));
			return this;
		}

		/// <summary>
		/// Creates a textured box. Args: originX, originY, originZ, width, height, depth, scaleFactor.
		/// </summary>
		public virtual void AddBox(float par1, float par2, float par3, int par4, int par5, int par6, float par7)
		{
			CubeList.Add(new ModelBox(this, TextureOffsetX, TextureOffsetY, par1, par2, par3, par4, par5, par6, par7));
		}

		public virtual void SetRotationPoint(float par1, float par2, float par3)
		{
			RotationPointX = par1;
			RotationPointY = par2;
			RotationPointZ = par3;
		}

		public virtual void Render(float par1)
		{
			if (IsHidden)
			{
				return;
			}

			if (!ShowModel)
			{
				return;
			}

			if (!Compiled)
			{
				CompileDisplayList(par1);
			}

			if (RotateAngleX != 0.0F || RotateAngleY != 0.0F || RotateAngleZ != 0.0F)
			{
				//GL.PushMatrix();
				//GL.Translate(RotationPointX * par1, RotationPointY * par1, RotationPointZ * par1);

				if (RotateAngleZ != 0.0F)
				{
					//GL.Rotate(RotateAngleZ * (180F / (float)Math.PI), 0.0F, 0.0F, 1.0F);
				}

				if (RotateAngleY != 0.0F)
				{
					//GL.Rotate(RotateAngleY * (180F / (float)Math.PI), 0.0F, 1.0F, 0.0F);
				}

				if (RotateAngleX != 0.0F)
				{
					//GL.Rotate(RotateAngleX * (180F / (float)Math.PI), 1.0F, 0.0F, 0.0F);
				}

				//GL.CallList(DisplayList);

				if (ChildModels != null)
				{
					for (int i = 0; i < ChildModels.Count; i++)
					{
						ChildModels[i].Render(par1);
					}
				}

				//GL.PopMatrix();
			}
			else if (RotationPointX != 0.0F || RotationPointY != 0.0F || RotationPointZ != 0.0F)
			{
				//GL.Translate(RotationPointX * par1, RotationPointY * par1, RotationPointZ * par1);
				//GL.CallList(DisplayList);

				if (ChildModels != null)
				{
					for (int j = 0; j < ChildModels.Count; j++)
					{
						ChildModels[j].Render(par1);
					}
				}

				//GL.Translate(-RotationPointX * par1, -RotationPointY * par1, -RotationPointZ * par1);
			}
			else
			{
				//GL.CallList(DisplayList);

				if (ChildModels != null)
				{
					for (int k = 0; k < ChildModels.Count; k++)
					{
						ChildModels[k].Render(par1);
					}
				}
			}
		}

		public virtual void RenderWithRotation(float par1)
		{
			if (IsHidden)
			{
				return;
			}

			if (!ShowModel)
			{
				return;
			}

			if (!Compiled)
			{
				CompileDisplayList(par1);
			}

			//GL.PushMatrix();
			//GL.Translate(RotationPointX * par1, RotationPointY * par1, RotationPointZ * par1);

			if (RotateAngleY != 0.0F)
			{
				//GL.Rotate(RotateAngleY * (180F / (float)Math.PI), 0.0F, 1.0F, 0.0F);
			}

			if (RotateAngleX != 0.0F)
			{
				//GL.Rotate(RotateAngleX * (180F / (float)Math.PI), 1.0F, 0.0F, 0.0F);
			}

			if (RotateAngleZ != 0.0F)
			{
				//GL.Rotate(RotateAngleZ * (180F / (float)Math.PI), 0.0F, 0.0F, 1.0F);
			}

			//GL.CallList(DisplayList);
			//GL.PopMatrix();
		}

		/// <summary>
		/// Allows the changing of Angles after a box has been rendered
		/// </summary>
		public virtual void PostRender(float par1)
		{
			if (IsHidden)
			{
				return;
			}

			if (!ShowModel)
			{
				return;
			}

			if (!Compiled)
			{
				CompileDisplayList(par1);
			}

			if (RotateAngleX != 0.0F || RotateAngleY != 0.0F || RotateAngleZ != 0.0F)
			{
				//GL.Translate(RotationPointX * par1, RotationPointY * par1, RotationPointZ * par1);

				if (RotateAngleZ != 0.0F)
				{
					//GL.Rotate(RotateAngleZ * (180F / (float)Math.PI), 0.0F, 0.0F, 1.0F);
				}

				if (RotateAngleY != 0.0F)
				{
					//GL.Rotate(RotateAngleY * (180F / (float)Math.PI), 0.0F, 1.0F, 0.0F);
				}

				if (RotateAngleX != 0.0F)
				{
					//GL.Rotate(RotateAngleX * (180F / (float)Math.PI), 1.0F, 0.0F, 0.0F);
				}
			}
			else if (RotationPointX != 0.0F || RotationPointY != 0.0F || RotationPointZ != 0.0F)
			{
				//GL.Translate(RotationPointX * par1, RotationPointY * par1, RotationPointZ * par1);
			}
		}

		/// <summary>
		/// Compiles a GL display list for this model
		/// </summary>
		private void CompileDisplayList(float par1)
		{
			DisplayList = GLAllocation.GenerateDisplayLists(1);
			//GL.NewList(DisplayList, ListMode.Compile);
			Tessellator tessellator = Tessellator.Instance;

			for (int i = 0; i < CubeList.Count; i++)
			{
				CubeList[i].Render(tessellator, par1);
			}

			//GL.EndList();
			Compiled = true;
		}

		/// <summary>
		/// Returns the model renderer with the new texture parameters.
		/// </summary>
		public virtual ModelRenderer SetTextureSize(int par1, int par2)
		{
			TextureWidth = par1;
			TextureHeight = par2;
			return this;
		}
	}
}