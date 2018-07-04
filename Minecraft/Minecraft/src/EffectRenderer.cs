using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class EffectRenderer
	{
		/// <summary>
		/// Reference to the World object. </summary>
		protected World WorldObj;
		private List<EntityFX>[] FxLayers;
		private RenderEngineOld Renderer;

		/// <summary>
		/// RNG. </summary>
		private Random Rand;

		public EffectRenderer(World par1World, RenderEngineOld par2RenderEngine)
		{
            FxLayers = new List<EntityFX>[4];
			Rand = new Random();

			if (par1World != null)
			{
				WorldObj = par1World;
			}

			Renderer = par2RenderEngine;

			for (int i = 0; i < 4; i++)
			{
                FxLayers[i] = new List<EntityFX>();
			}
		}

		public virtual void AddEffect(EntityFX par1EntityFX)
		{
			int i = par1EntityFX.GetFXLayer();

			if (FxLayers[i].Count >= 4000)
			{
				FxLayers[i].RemoveAt(0);
			}

			FxLayers[i].Add(par1EntityFX);
		}

		public virtual void UpdateEffects()
		{
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < FxLayers[i].Count; j++)
				{
					EntityFX entityfx = FxLayers[i][j];
					entityfx.OnUpdate();

					if (entityfx.IsDead)
					{
						FxLayers[i].RemoveAt(j--);
					}
				}
			}
		}

		/// <summary>
		/// Renders all current particles. Args player, partialTickTime
		/// </summary>
		public virtual void RenderParticles(Entity par1Entity, float par2)
		{
			float f = ActiveRenderInfo.RotationX;
			float f1 = ActiveRenderInfo.RotationZ;
			float f2 = ActiveRenderInfo.RotationYZ;
			float f3 = ActiveRenderInfo.RotationXY;
			float f4 = ActiveRenderInfo.RotationXZ;
			EntityFX.InterpPosX = par1Entity.LastTickPosX + (par1Entity.PosX - par1Entity.LastTickPosX) * (double)par2;
			EntityFX.InterpPosY = par1Entity.LastTickPosY + (par1Entity.PosY - par1Entity.LastTickPosY) * (double)par2;
			EntityFX.InterpPosZ = par1Entity.LastTickPosZ + (par1Entity.PosZ - par1Entity.LastTickPosZ) * (double)par2;

			for (int i = 0; i < 3; i++)
			{
				if (FxLayers[i].Count == 0)
				{
					continue;
				}

				int j = 0;

				if (i == 0)
				{
					j = Renderer.GetTexture("/particles.png");
				}

				if (i == 1)
				{
					j = Renderer.GetTexture("/terrain.png");
				}

				if (i == 2)
				{
					j = Renderer.GetTexture("/gui/items.png");
				}

				//GL.BindTexture(TextureTarget.Texture2D, j);
				Tessellator tessellator = Tessellator.Instance;
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				tessellator.StartDrawingQuads();

				for (int k = 0; k < FxLayers[i].Count; k++)
				{
					EntityFX entityfx = FxLayers[i][k];
					tessellator.SetBrightness(entityfx.GetBrightnessForRender(par2));
					entityfx.RenderParticle(tessellator, par2, f, f4, f1, f2, f3);
				}

				tessellator.Draw();
			}
		}

		public virtual void Func_1187_b(Entity par1Entity, float par2)
		{
			float f = MathHelper2.Cos(par1Entity.RotationYaw * 0.01745329F);
			float f1 = MathHelper2.Sin(par1Entity.RotationYaw * 0.01745329F);
			float f2 = -f1 * MathHelper2.Sin(par1Entity.RotationPitch * 0.01745329F);
			float f3 = f * MathHelper2.Sin(par1Entity.RotationPitch * 0.01745329F);
			float f4 = MathHelper2.Cos(par1Entity.RotationPitch * 0.01745329F);
			sbyte byte0 = 3;

			if (FxLayers[byte0].Count == 0)
			{
				return;
			}

			Tessellator tessellator = Tessellator.Instance;

			for (int i = 0; i < FxLayers[byte0].Count; i++)
			{
				EntityFX entityfx = FxLayers[byte0][i];
				tessellator.SetBrightness(entityfx.GetBrightnessForRender(par2));
				entityfx.RenderParticle(tessellator, par2, f, f4, f1, f2, f3);
			}
		}

		public virtual void ClearEffects(World par1World)
		{
			WorldObj = par1World;

			for (int i = 0; i < 4; i++)
			{
				FxLayers[i].Clear();
			}
		}

		public virtual void AddBlockDestroyEffects(int par1, int par2, int par3, int par4, int par5)
		{
			if (par4 == 0)
			{
				return;
			}

			Block block = Block.BlocksList[par4];
			int i = 4;

			for (int j = 0; j < i; j++)
			{
				for (int k = 0; k < i; k++)
				{
					for (int l = 0; l < i; l++)
					{
                        float d = par1 + (j + 0.5F) / i;
                        float d1 = par2 + (k + 0.5F) / i;
                        float d2 = par3 + (l + 0.5F) / i;
						int i1 = Rand.Next(6);
						AddEffect((new EntityDiggingFX(WorldObj, d, d1, d2, d - par1 - 0.5F, d1 - par2 - 0.5F, d2 - par3 - 0.5F, block, i1, par5)).Func_4041_a(par1, par2, par3));
					}
				}
			}
		}

		/// <summary>
		/// Adds block hit particles for the specified block. Args: x, y, z, sideHit
		/// </summary>
		public virtual void AddBlockHitEffects(int par1, int par2, int par3, int par4)
		{
			int i = WorldObj.GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return;
			}

			Block block = Block.BlocksList[i];
			float f = 0.1F;
            float d = par1 + Rand.NextFloat() * (block.MaxX - block.MinX - (f * 2.0F)) + f + block.MinX;
            float d1 = par2 + Rand.NextFloat() * (block.MaxY - block.MinY - (f * 2.0F)) + f + block.MinY;
            float d2 = par3 + Rand.NextFloat() * (block.MaxZ - block.MinZ - (f * 2.0F)) + f + block.MinZ;

			if (par4 == 0)
			{
				d1 = (par2 + block.MinY) - f;
			}

			if (par4 == 1)
			{
				d1 = par2 + block.MaxY + f;
			}

			if (par4 == 2)
			{
				d2 = (par3 + block.MinZ) - f;
			}

			if (par4 == 3)
			{
				d2 = par3 + block.MaxZ + f;
			}

			if (par4 == 4)
			{
				d = (par1 + block.MinX) - f;
			}

			if (par4 == 5)
			{
				d = par1 + block.MaxX + f;
			}

			AddEffect((new EntityDiggingFX(WorldObj, d, d1, d2, 0.0F, 0.0F, 0.0F, block, par4, WorldObj.GetBlockMetadata(par1, par2, par3))).Func_4041_a(par1, par2, par3).MultiplyVelocity(0.2F).Func_405_d(0.6F));
		}

		public virtual string GetStatistics()
		{
			return (new StringBuilder()).Append("").Append(FxLayers[0].Count + FxLayers[1].Count + FxLayers[2].Count).ToString();
		}
	}
}