using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class TileEntityRenderer
	{
		/// <summary>
		/// A mapping of TileEntitySpecialRenderers used for each TileEntity that has one
		/// </summary>
		private Dictionary<Type, TileEntitySpecialRenderer> SpecialRendererMap;

		/// <summary>
		/// The static instance of TileEntityRenderer </summary>
		public static TileEntityRenderer Instance = new TileEntityRenderer();

		/// <summary>
		/// The FontRenderer instance used by the TileEntityRenderer </summary>
		private FontRenderer FontRenderer;

		/// <summary>
		/// The player's current X position (same as playerX) </summary>
        public static float StaticPlayerX;

		/// <summary>
		/// The player's current Y position (same as playerY) </summary>
        public static float StaticPlayerY;

		/// <summary>
		/// The player's current Z position (same as playerZ) </summary>
        public static float StaticPlayerZ;

		/// <summary>
		/// The RenderEngine instance used by the TileEntityRenderer </summary>
		public RenderEngine RenderEngine;

		/// <summary>
		/// Reference to the World object. </summary>
		public World WorldObj;
		public EntityLiving EntityLivingPlayer;
		public float PlayerYaw;
		public float PlayerPitch;

		/// <summary>
		/// The player's X position in this rendering context </summary>
		public double PlayerX;

		/// <summary>
		/// The player's Y position in this rendering context </summary>
		public double PlayerY;

		/// <summary>
		/// The player's Z position in this rendering context </summary>
		public double PlayerZ;

		private TileEntityRenderer()
		{
            SpecialRendererMap = new Dictionary<Type, TileEntitySpecialRenderer>();
			SpecialRendererMap[typeof(TileEntitySign)] = new TileEntitySignRenderer();
			SpecialRendererMap[typeof(TileEntityMobSpawner)] = new TileEntityMobSpawnerRenderer();
			SpecialRendererMap[typeof(TileEntityPiston)] = new TileEntityRendererPiston();
			SpecialRendererMap[typeof(TileEntityChest)] = new TileEntityChestRenderer();
			SpecialRendererMap[typeof(TileEntityEnchantmentTable)] = new RenderEnchantmentTable();
			SpecialRendererMap[typeof(TileEntityEndPortal)] = new RenderEndPortal();
			TileEntitySpecialRenderer tileentityspecialrenderer;

			for (IEnumerator<TileEntitySpecialRenderer> iterator = SpecialRendererMap.Values.GetEnumerator(); iterator.MoveNext(); tileentityspecialrenderer.SetTileEntityRenderer(this))
			{
				tileentityspecialrenderer = iterator.Current;
			}
		}

		/// <summary>
		/// Returns the TileEntitySpecialRenderer used to render this TileEntity class, or null if it has no special renderer
		/// </summary>
		public virtual TileEntitySpecialRenderer GetSpecialRendererForClass(Type par1Class)
		{
			TileEntitySpecialRenderer tileentityspecialrenderer = (TileEntitySpecialRenderer)SpecialRendererMap[par1Class];

			if (tileentityspecialrenderer == null && par1Class != (typeof(net.minecraft.src.TileEntity)))
			{
				tileentityspecialrenderer = GetSpecialRendererForClass(par1Class.BaseType);
				SpecialRendererMap[par1Class] = tileentityspecialrenderer;
			}

			return tileentityspecialrenderer;
		}

		/// <summary>
		/// Returns true if this TileEntity instance has a TileEntitySpecialRenderer associated with it, false otherwise.
		/// </summary>
		public virtual bool HasSpecialRenderer(TileEntity par1TileEntity)
		{
			return GetSpecialRendererForEntity(par1TileEntity) != null;
		}

		/// <summary>
		/// Returns the TileEntitySpecialRenderer used to render this TileEntity instance, or null if it has no special
		/// renderer
		/// </summary>
		public virtual TileEntitySpecialRenderer GetSpecialRendererForEntity(TileEntity par1TileEntity)
		{
			if (par1TileEntity == null)
			{
				return null;
			}
			else
			{
				return GetSpecialRendererForClass(par1TileEntity.GetType());
			}
		}

		/// <summary>
		/// Caches several render-related references, including the active World, RenderEngine, FontRenderer, and the camera-
		/// bound EntityLiving's interpolated pitch, yaw and position. Args: world, renderengine, fontrenderer, entityliving,
		/// partialTickTime
		/// </summary>
		public virtual void CacheActiveRenderInfo(World par1World, RenderEngine par2RenderEngine, FontRenderer par3FontRenderer, EntityLiving par4EntityLiving, float par5)
		{
			if (WorldObj != par1World)
			{
				CacheSpecialRenderInfo(par1World);
			}

			RenderEngine = par2RenderEngine;
			EntityLivingPlayer = par4EntityLiving;
			FontRenderer = par3FontRenderer;
			PlayerYaw = par4EntityLiving.PrevRotationYaw + (par4EntityLiving.RotationYaw - par4EntityLiving.PrevRotationYaw) * par5;
			PlayerPitch = par4EntityLiving.PrevRotationPitch + (par4EntityLiving.RotationPitch - par4EntityLiving.PrevRotationPitch) * par5;
			PlayerX = par4EntityLiving.LastTickPosX + (par4EntityLiving.PosX - par4EntityLiving.LastTickPosX) * (double)par5;
			PlayerY = par4EntityLiving.LastTickPosY + (par4EntityLiving.PosY - par4EntityLiving.LastTickPosY) * (double)par5;
			PlayerZ = par4EntityLiving.LastTickPosZ + (par4EntityLiving.PosZ - par4EntityLiving.LastTickPosZ) * (double)par5;
		}

		public virtual void Func_40742_a()
		{
		}

		/// <summary>
		/// Render this TileEntity at its current position from the player
		/// </summary>
		public virtual void RenderTileEntity(TileEntity par1TileEntity, float par2)
		{
			if (par1TileEntity.GetDistanceFrom(PlayerX, PlayerY, PlayerZ) < 4096D)
			{
				int i = WorldObj.GetLightBrightnessForSkyBlocks(par1TileEntity.XCoord, par1TileEntity.YCoord, par1TileEntity.ZCoord, 0);
				int j = i % 0x10000;
				int k = i / 0x10000;
				OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
				//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
				RenderTileEntityAt(par1TileEntity, par1TileEntity.XCoord - StaticPlayerX, par1TileEntity.YCoord - StaticPlayerY, par1TileEntity.ZCoord - StaticPlayerZ, par2);
			}
		}

		/// <summary>
		/// Render this TileEntity at a given set of coordinates
		/// </summary>
        public virtual void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			TileEntitySpecialRenderer tileentityspecialrenderer = GetSpecialRendererForEntity(par1TileEntity);

			if (tileentityspecialrenderer != null)
			{
				tileentityspecialrenderer.RenderTileEntityAt(par1TileEntity, par2, par4, par6, par8);
			}
		}

		/// <summary>
		/// Called from cacheActiveRenderInfo() to cache render-related references for TileEntitySpecialRenderers in
		/// specialRendererMap. Currently only the world reference from cacheActiveRenderInfo() is passed to this method.
		/// </summary>
		public virtual void CacheSpecialRenderInfo(World par1World)
		{
			WorldObj = par1World;
			IEnumerator<TileEntitySpecialRenderer> iterator = SpecialRendererMap.Values.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				TileEntitySpecialRenderer tileentityspecialrenderer = iterator.Current;

				if (tileentityspecialrenderer != null)
				{
					tileentityspecialrenderer.CacheSpecialRenderInfo(par1World);
				}
			}
			while (true);
		}

		public virtual FontRenderer GetFontRenderer()
		{
			return FontRenderer;
		}
	}
}