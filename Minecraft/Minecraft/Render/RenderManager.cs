using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderManager
	{
		/// <summary>
		/// A map of entity classes and the associated renderer. </summary>
		private Dictionary<Type, Render> EntityRenderMap;

		/// <summary>
		/// The static instance of RenderManager. </summary>
		public static RenderManager Instance = new RenderManager();

		/// <summary>
		/// Renders fonts </summary>
		private FontRenderer FontRenderer;
		public static double RenderPosX;
		public static double RenderPosY;
		public static double RenderPosZ;
		public RenderEngine RenderEngine;
		public ItemRenderer ItemRenderer;

		/// <summary>
		/// Reference to the World object. </summary>
		public World WorldObj;

		/// <summary>
		/// Rendermanager's variable for the player </summary>
		public EntityLiving LivingPlayer;
		public float PlayerViewY;
		public float PlayerViewX;

		/// <summary>
		/// Reference to the GameSettings object. </summary>
		public GameSettings Options;
		public double Field_1222_l;
		public double Field_1221_m;
		public double Field_1220_n;

		private RenderManager()
		{
            EntityRenderMap = new Dictionary<Type, Render>();
			EntityRenderMap[typeof(EntitySpider)] = new RenderSpider();
			EntityRenderMap[typeof(EntityCaveSpider)] = new RenderSpider();
			EntityRenderMap[typeof(EntityPig)] = new RenderPig(new ModelPig(), new ModelPig(0.5F), 0.7F);
			EntityRenderMap[typeof(EntitySheep)] = new RenderSheep(new ModelSheep2(), new ModelSheep1(), 0.7F);
			EntityRenderMap[typeof(EntityCow)] = new RenderCow(new ModelCow(), 0.7F);
			EntityRenderMap[typeof(EntityMooshroom)] = new RenderMooshroom(new ModelCow(), 0.7F);
			EntityRenderMap[typeof(EntityWolf)] = new RenderWolf(new ModelWolf(), 0.5F);
			EntityRenderMap[typeof(EntityChicken)] = new RenderChicken(new ModelChicken(), 0.3F);
			EntityRenderMap[typeof(EntityOcelot)] = new RenderOcelot(new ModelOcelot(), 0.4F);
			EntityRenderMap[typeof(EntitySilverfish)] = new RenderSilverfish();
			EntityRenderMap[typeof(EntityCreeper)] = new RenderCreeper();
			EntityRenderMap[typeof(EntityEnderman)] = new RenderEnderman();
			EntityRenderMap[typeof(EntitySnowman)] = new RenderSnowMan();
			EntityRenderMap[typeof(EntitySkeleton)] = new RenderBiped(new ModelSkeleton(), 0.5F);
			EntityRenderMap[typeof(EntityBlaze)] = new RenderBlaze();
			EntityRenderMap[typeof(EntityZombie)] = new RenderBiped(new ModelZombie(), 0.5F);
			EntityRenderMap[typeof(EntitySlime)] = new RenderSlime(new ModelSlime(16), new ModelSlime(0), 0.25F);
			EntityRenderMap[typeof(EntityMagmaCube)] = new RenderMagmaCube();
			EntityRenderMap[typeof(EntityPlayer)] = new RenderPlayer();
			EntityRenderMap[typeof(EntityGiantZombie)] = new RenderGiantZombie(new ModelZombie(), 0.5F, 6F);
			EntityRenderMap[typeof(EntityGhast)] = new RenderGhast();
			EntityRenderMap[typeof(EntitySquid)] = new RenderSquid(new ModelSquid(), 0.7F);
			EntityRenderMap[typeof(EntityVillager)] = new RenderVillager();
			EntityRenderMap[typeof(EntityIronGolem)] = new RenderIronGolem();
			EntityRenderMap[typeof(EntityLiving)] = new RenderLiving(new ModelBiped(), 0.5F);
			EntityRenderMap[typeof(EntityDragon)] = new RenderDragon();
			EntityRenderMap[typeof(EntityEnderCrystal)] = new RenderEnderCrystal();
			EntityRenderMap[typeof(Entity)] = new RenderEntity();
			EntityRenderMap[typeof(EntityPainting)] = new RenderPainting();
			EntityRenderMap[typeof(EntityArrow)] = new RenderArrow();
			EntityRenderMap[typeof(EntitySnowball)] = new RenderSnowball(Item.Snowball.GetIconFromDamage(0));
			EntityRenderMap[typeof(EntityEnderPearl)] = new RenderSnowball(Item.EnderPearl.GetIconFromDamage(0));
			EntityRenderMap[typeof(EntityEnderEye)] = new RenderSnowball(Item.EyeOfEnder.GetIconFromDamage(0));
			EntityRenderMap[typeof(EntityEgg)] = new RenderSnowball(Item.Egg.GetIconFromDamage(0));
			EntityRenderMap[typeof(EntityPotion)] = new RenderSnowball(154);
			EntityRenderMap[typeof(EntityExpBottle)] = new RenderSnowball(Item.ExpBottle.GetIconFromDamage(0));
			EntityRenderMap[typeof(EntityFireball)] = new RenderFireball(2.0F);
			EntityRenderMap[typeof(EntitySmallFireball)] = new RenderFireball(0.5F);
			EntityRenderMap[typeof(EntityItem)] = new RenderItem();
			EntityRenderMap[typeof(EntityXPOrb)] = new RenderXPOrb();
			EntityRenderMap[typeof(EntityTNTPrimed)] = new RenderTNTPrimed();
			EntityRenderMap[typeof(EntityFallingSand)] = new RenderFallingSand();
			EntityRenderMap[typeof(EntityMinecart)] = new RenderMinecart();
			EntityRenderMap[typeof(EntityBoat)] = new RenderBoat();
			EntityRenderMap[typeof(EntityFishHook)] = new RenderFish();
			EntityRenderMap[typeof(EntityLightningBolt)] = new RenderLightningBolt();
			Render render;

			for (IEnumerator<Render> iterator = EntityRenderMap.Values.GetEnumerator(); iterator.MoveNext(); render.SetRenderManager(this))
			{
				render = iterator.Current;
			}
		}

		public virtual Render GetEntityClassRenderObject(Type par1Class)
		{
			Render render = EntityRenderMap[par1Class];

			if (render == null && par1Class != (typeof(Entity)))
			{
				render = GetEntityClassRenderObject(par1Class.BaseType);
				EntityRenderMap[par1Class] = render;
			}

			return render;
		}

		public virtual Render GetEntityRenderObject(Entity par1Entity)
		{
			return GetEntityClassRenderObject(par1Entity.GetType());
		}

		/// <summary>
		/// Caches the current frame's active render info, including the current World, RenderEngine, GameSettings and
		/// FontRenderer settings, as well as interpolated player position, pitch and yaw.
		/// </summary>
		public virtual void CacheActiveRenderInfo(World par1World, RenderEngine par2RenderEngine, FontRenderer par3FontRenderer, EntityLiving par4EntityLiving, GameSettings par5GameSettings, float par6)
		{
			WorldObj = par1World;
			RenderEngine = par2RenderEngine;
			Options = par5GameSettings;
			LivingPlayer = par4EntityLiving;
			FontRenderer = par3FontRenderer;

			if (par4EntityLiving.IsPlayerSleeping())
			{
				int i = par1World.GetBlockId(MathHelper2.Floor_double(par4EntityLiving.PosX), MathHelper2.Floor_double(par4EntityLiving.PosY), MathHelper2.Floor_double(par4EntityLiving.PosZ));

				if (i == Block.Bed.BlockID)
				{
					int j = par1World.GetBlockMetadata(MathHelper2.Floor_double(par4EntityLiving.PosX), MathHelper2.Floor_double(par4EntityLiving.PosY), MathHelper2.Floor_double(par4EntityLiving.PosZ));
					int k = j & 3;
					PlayerViewY = k * 90 + 180;
					PlayerViewX = 0.0F;
				}
			}
			else
			{
				PlayerViewY = par4EntityLiving.PrevRotationYaw + (par4EntityLiving.RotationYaw - par4EntityLiving.PrevRotationYaw) * par6;
				PlayerViewX = par4EntityLiving.PrevRotationPitch + (par4EntityLiving.RotationPitch - par4EntityLiving.PrevRotationPitch) * par6;
			}

			if (par5GameSettings.ThirdPersonView == 2)
			{
				PlayerViewY += 180F;
			}

			Field_1222_l = par4EntityLiving.LastTickPosX + (par4EntityLiving.PosX - par4EntityLiving.LastTickPosX) * (double)par6;
			Field_1221_m = par4EntityLiving.LastTickPosY + (par4EntityLiving.PosY - par4EntityLiving.LastTickPosY) * (double)par6;
			Field_1220_n = par4EntityLiving.LastTickPosZ + (par4EntityLiving.PosZ - par4EntityLiving.LastTickPosZ) * (double)par6;
		}

		/// <summary>
		/// Will render the specified entity at the specified partial tick time. Args: entity, partialTickTime
		/// </summary>
		public virtual void RenderEntity(Entity par1Entity, float par2)
		{
			double d = par1Entity.LastTickPosX + (par1Entity.PosX - par1Entity.LastTickPosX) * (double)par2;
			double d1 = par1Entity.LastTickPosY + (par1Entity.PosY - par1Entity.LastTickPosY) * (double)par2;
			double d2 = par1Entity.LastTickPosZ + (par1Entity.PosZ - par1Entity.LastTickPosZ) * (double)par2;
			float f = par1Entity.PrevRotationYaw + (par1Entity.RotationYaw - par1Entity.PrevRotationYaw) * par2;
			int i = par1Entity.GetBrightnessForRender(par2);

			if (par1Entity.IsBurning())
			{
				i = 0xf000f0;
			}

			int j = i % 0x10000;
			int k = i / 0x10000;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)j / 1.0F, (float)k / 1.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			RenderEntityWithPosYaw(par1Entity, d - RenderPosX, d1 - RenderPosY, d2 - RenderPosZ, f, par2);
		}

		/// <summary>
		/// Renders the specified entity with the passed in position, yaw, and partialTickTime. Args: entity, x, y, z, yaw,
		/// partialTickTime
		/// </summary>
		public virtual void RenderEntityWithPosYaw(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Render render = GetEntityRenderObject(par1Entity);

			if (render != null)
			{
				render.DoRender(par1Entity, par2, par4, par6, par8, par9);
				render.DoRenderShadowAndFire(par1Entity, par2, par4, par6, par8, par9);
			}
		}

		/// <summary>
		/// World sets this RenderManager's worldObj to the world provided
		/// </summary>
		public virtual void Set(World par1World)
		{
			WorldObj = par1World;
		}

		public virtual double GetDistanceToCamera(double par1, double par3, double par5)
		{
			double d = par1 - Field_1222_l;
			double d1 = par3 - Field_1221_m;
			double d2 = par5 - Field_1220_n;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// Returns the font renderer
		/// </summary>
		public virtual FontRenderer GetFontRenderer()
		{
			return FontRenderer;
		}
	}
}