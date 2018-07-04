using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderGlobal : IWorldAccess
	{
		public List<TileEntity> TileEntities;

		/// <summary>
		/// A reference to the World object. </summary>
		private World WorldObj;

		/// <summary>
		/// The RenderEngine instance used by RenderGlobal </summary>
		private RenderEngine RenderEngine;
		private List<WorldRenderer> WorldRenderersToUpdate;
		private WorldRenderer[] SortedWorldRenderers;
		private WorldRenderer[] WorldRenderers;
		private int RenderChunksWide;
		private int RenderChunksTall;
		private int RenderChunksDeep;

		/// <summary>
		/// OpenGL render lists base </summary>
		private int GlRenderListBase;

		/// <summary>
		/// A reference to the Minecraft object. </summary>
		private Minecraft Mc;

		/// <summary>
		/// Global render blocks </summary>
		private RenderBlocks GlobalRenderBlocks;

		/// <summary>
		/// OpenGL occlusion query base </summary>
		private Buffer<int> GlOcclusionQueryBase;

		/// <summary>
		/// Is occlusion testing enabled </summary>
		private bool OcclusionEnabled;
		private int CloudOffsetX;

		/// <summary>
		/// The star GL Call list </summary>
		private int StarGLCallList;

		/// <summary>
		/// OpenGL sky list </summary>
		private int GlSkyList;

		/// <summary>
		/// OpenGL sky list 2 </summary>
		private int GlSkyList2;

		/// <summary>
		/// Minimum block X </summary>
		private int MinBlockX;

		/// <summary>
		/// Minimum block Y </summary>
		private int MinBlockY;

		/// <summary>
		/// Minimum block Z </summary>
		private int MinBlockZ;

		/// <summary>
		/// Maximum block X </summary>
		private int MaxBlockX;

		/// <summary>
		/// Maximum block Y </summary>
		private int MaxBlockY;

		/// <summary>
		/// Maximum block Z </summary>
		private int MaxBlockZ;
		private int RenderDistance;

		/// <summary>
		/// Render entities startup counter (init value=2) </summary>
		private int RenderEntitiesStartupCounter;

		/// <summary>
		/// Count entities total </summary>
		private int CountEntitiesTotal;

		/// <summary>
		/// Count entities rendered </summary>
		private int CountEntitiesRendered;

		/// <summary>
		/// Count entities hidden </summary>
		private int CountEntitiesHidden;
		int[] DummyBuf50k;

		/// <summary>
		/// Occlusion query result </summary>
		Buffer<int> OcclusionResult;

		/// <summary>
		/// How many renderers are loaded this frame that try to be rendered </summary>
		private int RenderersLoaded;

		/// <summary>
		/// How many renderers are being clipped by the frustrum this frame </summary>
		private int RenderersBeingClipped;

		/// <summary>
		/// How many renderers are being occluded this frame </summary>
		private int RenderersBeingOccluded;

		/// <summary>
		/// How many renderers are actually being rendered this frame </summary>
		private int RenderersBeingRendered;

		/// <summary>
		/// How many renderers are skipping rendering due to not having a render pass this frame
		/// </summary>
		private int RenderersSkippingRenderPass;

		/// <summary>
		/// Dummy render int </summary>
		private int DummyRenderInt;

		/// <summary>
		/// World renderers check index </summary>
		private int WorldRenderersCheckIndex;

		/// <summary>
		/// List of OpenGL lists for the current render pass </summary>
		private List<WorldRenderer> GlRenderLists;
		private RenderList[] AllRenderLists = { new RenderList(), new RenderList(), new RenderList(), new RenderList() };

		/// <summary>
		/// Previous x position when the renderers were sorted. (Once the distance moves more than 4 units they will be
		/// resorted)
		/// </summary>
		double PrevSortX;

		/// <summary>
		/// Previous y position when the renderers were sorted. (Once the distance moves more than 4 units they will be
		/// resorted)
		/// </summary>
		double PrevSortY;

		/// <summary>
		/// Previous Z position when the renderers were sorted. (Once the distance moves more than 4 units they will be
		/// resorted)
		/// </summary>
		double PrevSortZ;

		/// <summary>
		/// Damage partial time </summary>
		public float DamagePartialTime;

		/// <summary>
		/// The offset used to determine if a renderer is one of the sixteenth that are being updated this frame
		/// </summary>
		int FrustumCheckOffset;

		public RenderGlobal(Minecraft par1Minecraft, RenderEngine par2RenderEngine)
		{
            TileEntities = new List<TileEntity>();
            WorldRenderersToUpdate = new List<WorldRenderer>();
			OcclusionEnabled = false;
			CloudOffsetX = 0;
			RenderDistance = -1;
			RenderEntitiesStartupCounter = 2;
			DummyBuf50k = new int[50000];
            OcclusionResult = new Buffer<int>(64);// GLAllocation.CreateDirectIntBuffer(64);
            GlRenderLists = new List<WorldRenderer>();
			PrevSortX = -9999D;
			PrevSortY = -9999D;
			PrevSortZ = -9999D;
			FrustumCheckOffset = 0;
			Mc = par1Minecraft;
			RenderEngine = par2RenderEngine;
			sbyte byte0 = 34;
			sbyte byte1 = 32;
			GlRenderListBase = GLAllocation.GenerateDisplayLists(byte0 * byte0 * byte1 * 3);
			OcclusionEnabled = OpenGlCapsChecker.CheckARBOcclusion();

			if (OcclusionEnabled)
			{
				OcclusionResult.Clear();
                GlOcclusionQueryBase = new Buffer<int>(byte0 * byte0 * byte1);// GLAllocation.CreateDirectIntBuffer(byte0 * byte0 * byte1);
				GlOcclusionQueryBase.Clear();
				GlOcclusionQueryBase.Position = 0;
				GlOcclusionQueryBase.Limit(byte0 * byte0 * byte1);
                //GL.GenQueries(1, GlOcclusionQueryBase.Data);
			}

			StarGLCallList = GLAllocation.GenerateDisplayLists(3);
			//GL.PushMatrix();
			//GL.NewList(StarGLCallList, ListMode.Compile);
			RenderStars();
			//GL.EndList();
			//GL.PopMatrix();
			Tessellator tessellator = Tessellator.Instance;
			GlSkyList = StarGLCallList + 1;
			//GL.NewList(GlSkyList, ListMode.Compile);
			sbyte byte2 = 64;
			int i = 256 / byte2 + 2;
			float f = 16F;

			for (int j = -byte2 * i; j <= byte2 * i; j += byte2)
			{
				for (int l = -byte2 * i; l <= byte2 * i; l += byte2)
				{
					tessellator.StartDrawingQuads();
					tessellator.AddVertex(j + 0, f, l + 0);
					tessellator.AddVertex(j + byte2, f, l + 0);
					tessellator.AddVertex(j + byte2, f, l + byte2);
					tessellator.AddVertex(j + 0, f, l + byte2);
					tessellator.Draw();
				}
			}

			//GL.EndList();
			GlSkyList2 = StarGLCallList + 2;
			//GL.NewList(GlSkyList2, ListMode.Compile);
			f = -16F;
			tessellator.StartDrawingQuads();

			for (int k = -byte2 * i; k <= byte2 * i; k += byte2)
			{
				for (int i1 = -byte2 * i; i1 <= byte2 * i; i1 += byte2)
				{
					tessellator.AddVertex(k + byte2, f, i1 + 0);
					tessellator.AddVertex(k + 0, f, i1 + 0);
					tessellator.AddVertex(k + 0, f, i1 + byte2);
					tessellator.AddVertex(k + byte2, f, i1 + byte2);
				}
			}

			tessellator.Draw();
			//GL.EndList();
		}

		private void RenderStars()
		{
			Random random = new Random(10842);
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawingQuads();

			for (int i = 0; i < 1500; i++)
			{
				double d = random.NextFloat() * 2.0F - 1.0F;
				double d1 = random.NextFloat() * 2.0F - 1.0F;
				double d2 = random.NextFloat() * 2.0F - 1.0F;
				double d3 = 0.25F + random.NextFloat() * 0.25F;
				double d4 = d * d + d1 * d1 + d2 * d2;

				if (d4 >= 1.0D || d4 <= 0.01D)
				{
					continue;
				}

				d4 = 1.0D / Math.Sqrt(d4);
				d *= d4;
				d1 *= d4;
				d2 *= d4;
				double d5 = d * 100D;
				double d6 = d1 * 100D;
				double d7 = d2 * 100D;
				double d8 = Math.Atan2(d, d2);
				double d9 = Math.Sin(d8);
				double d10 = Math.Cos(d8);
				double d11 = Math.Atan2(Math.Sqrt(d * d + d2 * d2), d1);
				double d12 = Math.Sin(d11);
				double d13 = Math.Cos(d11);
				double d14 = random.NextDouble() * Math.PI * 2D;
				double d15 = Math.Sin(d14);
				double d16 = Math.Cos(d14);

				for (int j = 0; j < 4; j++)
				{
					double d17 = 0.0F;
					double d18 = (double)((j & 2) - 1) * d3;
					double d19 = (double)((j + 1 & 2) - 1) * d3;
					double d20 = d17;
					double d21 = d18 * d16 - d19 * d15;
					double d22 = d19 * d16 + d18 * d15;
					double d23 = d22;
					double d24 = d21 * d12 + d20 * d13;
					double d25 = d20 * d12 - d21 * d13;
					double d26 = d25 * d9 - d23 * d10;
					double d27 = d24;
					double d28 = d23 * d9 + d25 * d10;
					tessellator.AddVertex(d5 + d26, d6 + d27, d7 + d28);
				}
			}

			tessellator.Draw();
		}

		/// <summary>
		/// Changes the world reference in RenderGlobal
		/// </summary>
		public virtual void ChangeWorld(World par1World)
		{
			if (WorldObj != null)
			{
				WorldObj.RemoveWorldAccess(this);
			}

			PrevSortX = -9999D;
			PrevSortY = -9999D;
			PrevSortZ = -9999D;
			RenderManager.Instance.Set(par1World);
			WorldObj = par1World;
			GlobalRenderBlocks = new RenderBlocks(par1World);

			if (par1World != null)
			{
				par1World.AddWorldAccess(this);
				LoadRenderers();
			}
		}

		/// <summary>
		/// Loads all the renderers and sets up the basic settings usage
		/// </summary>
		public virtual void LoadRenderers()
		{
			if (WorldObj == null)
			{
				return;
			}

			Block.Leaves.SetGraphicsLevel(Mc.GameSettings.FancyGraphics);
			RenderDistance = Mc.GameSettings.RenderDistance;

			if (WorldRenderers != null)
			{
				for (int i = 0; i < WorldRenderers.Length; i++)
				{
					WorldRenderers[i].StopRendering();
				}
			}

			int j = 64 << 3 - RenderDistance;

			if (j > 400)
			{
				j = 400;
			}

			RenderChunksWide = j / 16 + 1;
			RenderChunksTall = 16;
			RenderChunksDeep = j / 16 + 1;
			WorldRenderers = new WorldRenderer[RenderChunksWide * RenderChunksTall * RenderChunksDeep];
			SortedWorldRenderers = new WorldRenderer[RenderChunksWide * RenderChunksTall * RenderChunksDeep];
			int k = 0;
			int l = 0;
			MinBlockX = 0;
			MinBlockY = 0;
			MinBlockZ = 0;
			MaxBlockX = RenderChunksWide;
			MaxBlockY = RenderChunksTall;
			MaxBlockZ = RenderChunksDeep;

			for (int i1 = 0; i1 < WorldRenderersToUpdate.Count; i1++)
			{
				WorldRenderersToUpdate[i1].NeedsUpdate = false;
			}

			WorldRenderersToUpdate.Clear();
			TileEntities.Clear();

			for (int j1 = 0; j1 < RenderChunksWide; j1++)
			{
				for (int k1 = 0; k1 < RenderChunksTall; k1++)
				{
					for (int l1 = 0; l1 < RenderChunksDeep; l1++)
					{
                        int index = (l1 * RenderChunksTall + k1) * RenderChunksWide + j1;

                        WorldRenderers[index] = new WorldRenderer(WorldObj, TileEntities, j1 * 16, k1 * 16, l1 * 16, GlRenderListBase + k);

                        WorldRenderer worldRenderer = WorldRenderers[index];

						if (OcclusionEnabled)
						{
                            worldRenderer.GlOcclusionQuery = GlOcclusionQueryBase.Get(l);
						}

                        worldRenderer.IsWaitingOnOcclusionQuery = false;
                        worldRenderer.IsVisible = true;
                        worldRenderer.IsInFrustum = true;
                        worldRenderer.ChunkIndex = l++;
                        worldRenderer.MarkDirty();
                        SortedWorldRenderers[index] = worldRenderer;
                        WorldRenderersToUpdate.Add(worldRenderer);
						k += 3;
					}
				}
			}

			if (WorldObj != null)
			{
				EntityLiving entityliving = Mc.RenderViewEntity;

				if (entityliving != null)
				{
					MarkRenderersForNewPosition(MathHelper2.Floor_double(((Entity)(entityliving)).PosX), MathHelper2.Floor_double(((Entity)(entityliving)).PosY), MathHelper2.Floor_double(((Entity)(entityliving)).PosZ));
					Array.Sort(SortedWorldRenderers, new EntitySorter(entityliving));
				}
			}

			RenderEntitiesStartupCounter = 2;
		}

		/// <summary>
		/// Renders all entities within range and within the frustrum. Args: pos, frustrum, partialTickTime
		/// </summary>
		public virtual void RenderEntities(Vec3D par1Vec3D, ICamera par2ICamera, float par3)
		{
			if (RenderEntitiesStartupCounter > 0)
			{
				RenderEntitiesStartupCounter--;
				return;
			}

			Profiler.StartSection("prepare");
			TileEntityRenderer.Instance.CacheActiveRenderInfo(WorldObj, RenderEngine, Mc.FontRenderer, Mc.RenderViewEntity, par3);
			RenderManager.Instance.CacheActiveRenderInfo(WorldObj, RenderEngine, Mc.FontRenderer, Mc.RenderViewEntity, Mc.GameSettings, par3);
			TileEntityRenderer.Instance.Func_40742_a();
			CountEntitiesTotal = 0;
			CountEntitiesRendered = 0;
			CountEntitiesHidden = 0;
			EntityLiving entityliving = Mc.RenderViewEntity;
			RenderManager.RenderPosX = ((Entity)(entityliving)).LastTickPosX + (((Entity)(entityliving)).PosX - ((Entity)(entityliving)).LastTickPosX) * par3;
			RenderManager.RenderPosY = ((Entity)(entityliving)).LastTickPosY + (((Entity)(entityliving)).PosY - ((Entity)(entityliving)).LastTickPosY) * par3;
			RenderManager.RenderPosZ = ((Entity)(entityliving)).LastTickPosZ + (((Entity)(entityliving)).PosZ - ((Entity)(entityliving)).LastTickPosZ) * par3;
			TileEntityRenderer.StaticPlayerX = ((Entity)(entityliving)).LastTickPosX + (((Entity)(entityliving)).PosX - ((Entity)(entityliving)).LastTickPosX) * par3;
			TileEntityRenderer.StaticPlayerY = ((Entity)(entityliving)).LastTickPosY + (((Entity)(entityliving)).PosY - ((Entity)(entityliving)).LastTickPosY) * par3;
			TileEntityRenderer.StaticPlayerZ = ((Entity)(entityliving)).LastTickPosZ + (((Entity)(entityliving)).PosZ - ((Entity)(entityliving)).LastTickPosZ) * par3;
			Mc.EntityRenderer.EnableLightmap(par3);
			Profiler.EndStartSection("global");
			List<Entity> list = WorldObj.GetLoadedEntityList();
			CountEntitiesTotal = list.Count;

			for (int i = 0; i < WorldObj.WeatherEffects.Count; i++)
			{
				Entity entity = WorldObj.WeatherEffects[i];
				CountEntitiesRendered++;

				if (entity.IsInRangeToRenderVec3D(par1Vec3D))
				{
					RenderManager.Instance.RenderEntity(entity, par3);
				}
			}

			Profiler.EndStartSection("entities");

			for (int j = 0; j < list.Count; j++)
			{
				Entity entity1 = list[j];

				if (entity1.IsInRangeToRenderVec3D(par1Vec3D) && (entity1.IgnoreFrustumCheck || par2ICamera.IsBoundingBoxInFrustum(entity1.BoundingBox)) && (entity1 != Mc.RenderViewEntity || Mc.GameSettings.ThirdPersonView != 0 || Mc.RenderViewEntity.IsPlayerSleeping()) && WorldObj.BlockExists(MathHelper2.Floor_double(entity1.PosX), 0, MathHelper2.Floor_double(entity1.PosZ)))
				{
					CountEntitiesRendered++;
					RenderManager.Instance.RenderEntity(entity1, par3);
				}
			}

			Profiler.EndStartSection("tileentities");
			RenderHelper.EnableStandardItemLighting();

			for (int k = 0; k < TileEntities.Count; k++)
			{
				TileEntityRenderer.Instance.RenderTileEntity(TileEntities[k], par3);
			}

			Mc.EntityRenderer.DisableLightmap(par3);
			Profiler.EndSection();
		}

		/// <summary>
		/// Gets the render info for use on the Debug screen
		/// </summary>
		public virtual string GetDebugInfoRenders()
		{
			return new StringBuilder().Append("C: ").Append(RenderersBeingRendered).Append("/").Append(RenderersLoaded).Append(". F: ").Append(RenderersBeingClipped).Append(", O: ").Append(RenderersBeingOccluded).Append(", E: ").Append(RenderersSkippingRenderPass).ToString();
		}

		/// <summary>
		/// Gets the entities info for use on the Debug screen
		/// </summary>
		public virtual string GetDebugInfoEntities()
		{
			return new StringBuilder().Append("E: ").Append(CountEntitiesRendered).Append("/").Append(CountEntitiesTotal).Append(". B: ").Append(CountEntitiesHidden).Append(", I: ").Append(CountEntitiesTotal - CountEntitiesHidden - CountEntitiesRendered).ToString();
		}

		/// <summary>
		/// Goes through all the renderers setting new positions on them and those that have their position changed are
		/// adding to be updated
		/// </summary>
		private void MarkRenderersForNewPosition(int par1, int par2, int par3)
		{
			par1 -= 8;
			par2 -= 8;
			par3 -= 8;
			MinBlockX = 0x7fffffff;
			MinBlockY = 0x7fffffff;
			MinBlockZ = 0x7fffffff;
			MaxBlockX = 0x8000000;
			MaxBlockY = 0x8000000;
			MaxBlockZ = 0x8000000;
			int i = RenderChunksWide * 16;
			int j = i / 2;

			for (int k = 0; k < RenderChunksWide; k++)
			{
				int l = k * 16;
				int i1 = (l + j) - par1;

				if (i1 < 0)
				{
					i1 -= i - 1;
				}

				i1 /= i;
				l -= i1 * i;

				if (l < MinBlockX)
				{
					MinBlockX = l;
				}

				if (l > MaxBlockX)
				{
					MaxBlockX = l;
				}

				for (int j1 = 0; j1 < RenderChunksDeep; j1++)
				{
					int k1 = j1 * 16;
					int l1 = (k1 + j) - par3;

					if (l1 < 0)
					{
						l1 -= i - 1;
					}

					l1 /= i;
					k1 -= l1 * i;

					if (k1 < MinBlockZ)
					{
						MinBlockZ = k1;
					}

					if (k1 > MaxBlockZ)
					{
						MaxBlockZ = k1;
					}

					for (int i2 = 0; i2 < RenderChunksTall; i2++)
					{
						int j2 = i2 * 16;

						if (j2 < MinBlockY)
						{
							MinBlockY = j2;
						}

						if (j2 > MaxBlockY)
						{
							MaxBlockY = j2;
						}

						WorldRenderer worldrenderer = WorldRenderers[(j1 * RenderChunksTall + i2) * RenderChunksWide + k];
						bool flag = worldrenderer.NeedsUpdate;
						worldrenderer.SetPosition(l, j2, k1);

						if (!flag && worldrenderer.NeedsUpdate)
						{
							WorldRenderersToUpdate.Add(worldrenderer);
						}
					}
				}
			}
		}

		/// <summary>
		/// Sorts all renderers based on the passed in entity. Args: entityLiving, renderPass, partialTickTime
		/// </summary>
		public virtual int SortAndRender(EntityLiving par1EntityLiving, int par2, double par3)
		{
			Profiler.StartSection("sortchunks");

			for (int i = 0; i < 10; i++)
			{
				WorldRenderersCheckIndex = (WorldRenderersCheckIndex + 1) % WorldRenderers.Length;
				WorldRenderer worldrenderer = WorldRenderers[WorldRenderersCheckIndex];

				if (worldrenderer.NeedsUpdate && !WorldRenderersToUpdate.Contains(worldrenderer))
				{
					WorldRenderersToUpdate.Add(worldrenderer);
				}
			}

			if (Mc.GameSettings.RenderDistance != RenderDistance)
			{
				LoadRenderers();
			}

			if (par2 == 0)
			{
				RenderersLoaded = 0;
				DummyRenderInt = 0;
				RenderersBeingClipped = 0;
				RenderersBeingOccluded = 0;
				RenderersBeingRendered = 0;
				RenderersSkippingRenderPass = 0;
			}

			double d = par1EntityLiving.LastTickPosX + (par1EntityLiving.PosX - par1EntityLiving.LastTickPosX) * par3;
			double d1 = par1EntityLiving.LastTickPosY + (par1EntityLiving.PosY - par1EntityLiving.LastTickPosY) * par3;
			double d2 = par1EntityLiving.LastTickPosZ + (par1EntityLiving.PosZ - par1EntityLiving.LastTickPosZ) * par3;
			double d3 = par1EntityLiving.PosX - PrevSortX;
			double d4 = par1EntityLiving.PosY - PrevSortY;
			double d5 = par1EntityLiving.PosZ - PrevSortZ;

			if (d3 * d3 + d4 * d4 + d5 * d5 > 16D)
			{
				PrevSortX = par1EntityLiving.PosX;
				PrevSortY = par1EntityLiving.PosY;
				PrevSortZ = par1EntityLiving.PosZ;
				MarkRenderersForNewPosition(MathHelper2.Floor_double(par1EntityLiving.PosX), MathHelper2.Floor_double(par1EntityLiving.PosY), MathHelper2.Floor_double(par1EntityLiving.PosZ));
				Array.Sort(SortedWorldRenderers, new EntitySorter(par1EntityLiving));
			}

			RenderHelper.DisableStandardItemLighting();
			int j = 0;

			if (OcclusionEnabled && Mc.GameSettings.AdvancedOpengl && !Mc.GameSettings.Anaglyph && par2 == 0)
			{
				int k = 0;
				int l = 16;
				CheckOcclusionQueryResult(k, l);

				for (int i1 = k; i1 < l; i1++)
				{
					SortedWorldRenderers[i1].IsVisible = true;
				}

				Profiler.EndStartSection("render");
				j += RenderSortedRenderers(k, l, par2, par3);

				do
				{
					Profiler.EndStartSection("occ");
					int byte0 = l;
					l *= 2;

					if (l > SortedWorldRenderers.Length)
					{
						l = SortedWorldRenderers.Length;
					}

					//GL.Disable(EnableCap.Texture2D);
					//GL.Disable(EnableCap.Lighting);
					//GL.Disable(EnableCap.AlphaTest);
					//GL.Disable(EnableCap.Fog);
					//GL.ColorMask(false, false, false, false);
					//GL.DepthMask(false);
					Profiler.StartSection("check");
					CheckOcclusionQueryResult(byte0, l);
					Profiler.EndSection();
					//GL.PushMatrix();
					float f = 0.0F;
					float f1 = 0.0F;
					float f2 = 0.0F;

					for (int j1 = byte0; j1 < l; j1++)
					{
						if (SortedWorldRenderers[j1].SkipAllRenderPasses())
						{
							SortedWorldRenderers[j1].IsInFrustum = false;
							continue;
						}

						if (!SortedWorldRenderers[j1].IsInFrustum)
						{
							SortedWorldRenderers[j1].IsVisible = true;
						}

						if (!SortedWorldRenderers[j1].IsInFrustum || SortedWorldRenderers[j1].IsWaitingOnOcclusionQuery)
						{
							continue;
						}

						float f3 = MathHelper2.Sqrt_float(SortedWorldRenderers[j1].DistanceToEntitySquared(par1EntityLiving));
						int k1 = (int)(1.0F + f3 / 128F);

						if (CloudOffsetX % k1 != j1 % k1)
						{
							continue;
						}

						WorldRenderer worldrenderer1 = SortedWorldRenderers[j1];
						float f4 = (float)(worldrenderer1.PosXMinus - d);
						float f5 = (float)(worldrenderer1.PosYMinus - d1);
						float f6 = (float)(worldrenderer1.PosZMinus - d2);
						float f7 = f4 - f;
						float f8 = f5 - f1;
						float f9 = f6 - f2;

						if (f7 != 0.0F || f8 != 0.0F || f9 != 0.0F)
						{
							//GL.Translate(f7, f8, f9);
							f += f7;
							f1 += f8;
							f2 += f9;
						}

						Profiler.StartSection("bb");
						//GL.BeginQuery(QueryTarget.SamplesPassed, SortedWorldRenderers[j1].GlOcclusionQuery);
						SortedWorldRenderers[j1].CallOcclusionQueryList();
						//GL.EndQuery(QueryTarget.SamplesPassed);
						Profiler.EndSection();
						SortedWorldRenderers[j1].IsWaitingOnOcclusionQuery = true;
					}

					//GL.PopMatrix();

					if (Mc.GameSettings.Anaglyph)
					{
						if (EntityRenderer.AnaglyphField == 0)
						{
							//GL.ColorMask(false, true, true, true);
						}
						else
						{
							//GL.ColorMask(true, false, false, true);
						}
					}
					else
					{
						//GL.ColorMask(true, true, true, true);
					}

					//GL.DepthMask(true);
					//GL.Enable(EnableCap.Texture2D);
					//GL.Enable(EnableCap.AlphaTest);
					//GL.Enable(EnableCap.Fog);
					Profiler.EndStartSection("render");
					j += RenderSortedRenderers(byte0, l, par2, par3);
				}
				while (l < SortedWorldRenderers.Length);
			}
			else
			{
				Profiler.EndStartSection("render");
				j += RenderSortedRenderers(0, SortedWorldRenderers.Length, par2, par3);
			}

			Profiler.EndSection();
			return j;
		}

		private void CheckOcclusionQueryResult(int par1, int par2)
		{
			for (int i = par1; i < par2; i++)
			{
				if (!SortedWorldRenderers[i].IsWaitingOnOcclusionQuery)
				{
					continue;
				}

				OcclusionResult.Clear();
                //GL.GetQueryObject(SortedWorldRenderers[i].GlOcclusionQuery, GetQueryObjectParam.QueryResultAvailable, OcclusionResult);

				if (OcclusionResult.Get(0) != 0)
				{
					SortedWorldRenderers[i].IsWaitingOnOcclusionQuery = false;
					OcclusionResult.Clear();
					//GL.GetQueryObject(SortedWorldRenderers[i].GlOcclusionQuery, GetQueryObjectParam.QueryResult, OcclusionResult);
					SortedWorldRenderers[i].IsVisible = OcclusionResult.Get(0) != 0;
				}
			}
		}

		/// <summary>
		/// Renders the sorted renders for the specified render pass. Args: startRenderer, numRenderers, renderPass,
		/// partialTickTime
		/// </summary>
		private int RenderSortedRenderers(int par1, int par2, int par3, double par4)
		{
			GlRenderLists.Clear();
			int i = 0;

			for (int j = par1; j < par2; j++)
			{
				if (par3 == 0)
				{
					RenderersLoaded++;

					if (SortedWorldRenderers[j].SkipRenderPass[par3])
					{
						RenderersSkippingRenderPass++;
					}
					else if (!SortedWorldRenderers[j].IsInFrustum)
					{
						RenderersBeingClipped++;
					}
					else if (OcclusionEnabled && !SortedWorldRenderers[j].IsVisible)
					{
						RenderersBeingOccluded++;
					}
					else
					{
						RenderersBeingRendered++;
					}
				}

				if (SortedWorldRenderers[j].SkipRenderPass[par3] || !SortedWorldRenderers[j].IsInFrustum || OcclusionEnabled && !SortedWorldRenderers[j].IsVisible)
				{
					continue;
				}

				int k = SortedWorldRenderers[j].GetGLCallListForPass(par3);

				if (k >= 0)
				{
					GlRenderLists.Add(SortedWorldRenderers[j]);
					i++;
				}
			}

			EntityLiving entityliving = Mc.RenderViewEntity;
			double d = entityliving.LastTickPosX + (entityliving.PosX - entityliving.LastTickPosX) * par4;
			double d1 = entityliving.LastTickPosY + (entityliving.PosY - entityliving.LastTickPosY) * par4;
			double d2 = entityliving.LastTickPosZ + (entityliving.PosZ - entityliving.LastTickPosZ) * par4;
			int l = 0;

			for (int i1 = 0; i1 < AllRenderLists.Length; i1++)
			{
				AllRenderLists[i1].Func_859_b();
			}

			for (int j1 = 0; j1 < GlRenderLists.Count; j1++)
			{
				WorldRenderer worldrenderer = GlRenderLists[j1];
				int k1 = -1;

				for (int l1 = 0; l1 < l; l1++)
				{
					if (AllRenderLists[l1].Func_862_a(worldrenderer.PosXMinus, worldrenderer.PosYMinus, worldrenderer.PosZMinus))
					{
						k1 = l1;
					}
				}

				if (k1 < 0)
				{
					k1 = l++;
					AllRenderLists[k1].Func_861_a(worldrenderer.PosXMinus, worldrenderer.PosYMinus, worldrenderer.PosZMinus, d, d1, d2);
				}

				AllRenderLists[k1].Func_858_a(worldrenderer.GetGLCallListForPass(par3));
			}

			RenderAllRenderLists(par3, par4);
			return i;
		}

		/// <summary>
		/// Render all render lists
		/// </summary>
		public virtual void RenderAllRenderLists(int par1, double par2)
		{
			Mc.EntityRenderer.EnableLightmap(par2);

			for (int i = 0; i < AllRenderLists.Length; i++)
			{
				AllRenderLists[i].Func_860_a();
			}

			Mc.EntityRenderer.DisableLightmap(par2);
		}

		public virtual void UpdateClouds()
		{
			CloudOffsetX++;
		}

		/// <summary>
		/// Renders the sky with the partial tick time. Args: partialTickTime
		/// </summary>
		public virtual void RenderSky(float par1)
		{
			if (Mc.TheWorld.WorldProvider.TheWorldType == 1)
			{
				//GL.Disable(EnableCap.Fog);
				//GL.Disable(EnableCap.AlphaTest);
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				RenderHelper.DisableStandardItemLighting();
				//GL.DepthMask(false);
				RenderEngine.BindTexture("misc.tunnel.png");
				Tessellator tessellator = Tessellator.Instance;

				for (int i = 0; i < 6; i++)
				{
					//GL.PushMatrix();

					if (i == 1)
					{
						//GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
					}

					if (i == 2)
					{
						//GL.Rotate(-90F, 1.0F, 0.0F, 0.0F);
					}

					if (i == 3)
					{
						//GL.Rotate(180F, 1.0F, 0.0F, 0.0F);
					}

					if (i == 4)
					{
						//GL.Rotate(90F, 0.0F, 0.0F, 1.0F);
					}

					if (i == 5)
					{
						//GL.Rotate(-90F, 0.0F, 0.0F, 1.0F);
					}

					tessellator.StartDrawingQuads();
					tessellator.SetColorOpaque_I(0x181818);
					tessellator.AddVertexWithUV(-100D, -100D, -100D, 0.0F, 0.0F);
					tessellator.AddVertexWithUV(-100D, -100D, 100D, 0.0F, 16D);
					tessellator.AddVertexWithUV(100D, -100D, 100D, 16D, 16D);
					tessellator.AddVertexWithUV(100D, -100D, -100D, 16D, 0.0F);
					tessellator.Draw();
					//GL.PopMatrix();
				}

				//GL.DepthMask(true);
				//GL.Enable(EnableCap.Texture2D);
				//GL.Enable(EnableCap.AlphaTest);
				return;
			}

			if (!Mc.TheWorld.WorldProvider.Func_48217_e())
			{
				return;
			}

			//GL.Disable(EnableCap.Texture2D);
			Vec3D vec3d = WorldObj.GetSkyColor(Mc.RenderViewEntity, par1);
			float f = (float)vec3d.XCoord;
			float f1 = (float)vec3d.YCoord;
			float f2 = (float)vec3d.ZCoord;

			if (Mc.GameSettings.Anaglyph)
			{
				float f3 = (f * 30F + f1 * 59F + f2 * 11F) / 100F;
				float f4 = (f * 30F + f1 * 70F) / 100F;
				float f5 = (f * 30F + f2 * 70F) / 100F;
				f = f3;
				f1 = f4;
				f2 = f5;
			}

			//GL.Color3(f, f1, f2);
			Tessellator tessellator1 = Tessellator.Instance;
			//GL.DepthMask(false);
			//GL.Enable(EnableCap.Fog);
			//GL.Color3(f, f1, f2);
			//GL.CallList(GlSkyList);
			//GL.Disable(EnableCap.Fog);
			//GL.Disable(EnableCap.AlphaTest);
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			RenderHelper.DisableStandardItemLighting();
			float[] af = WorldObj.WorldProvider.CalcSunriseSunsetColors(WorldObj.GetCelestialAngle(par1), par1);

			if (af != null)
			{
				//GL.Disable(EnableCap.Texture2D);
				//GL.ShadeModel(ShadingModel.Smooth);
				//GL.PushMatrix();
				//GL.Rotate(90F, 1.0F, 0.0F, 0.0F);
				//GL.Rotate(MathHelper.Sin(WorldObj.GetCelestialAngleRadians(par1)) >= 0.0F ? 0.0F : 180F, 0.0F, 0.0F, 1.0F);
				//GL.Rotate(90F, 0.0F, 0.0F, 1.0F);
				float f6 = af[0];
				float f8 = af[1];
				float f11 = af[2];

				if (Mc.GameSettings.Anaglyph)
				{
					float f14 = (f6 * 30F + f8 * 59F + f11 * 11F) / 100F;
					float f17 = (f6 * 30F + f8 * 70F) / 100F;
					float f20 = (f6 * 30F + f11 * 70F) / 100F;
					f6 = f14;
					f8 = f17;
					f11 = f20;
				}

				tessellator1.StartDrawing(6);
				tessellator1.SetColorRGBA_F(f6, f8, f11, af[3]);
				tessellator1.AddVertex(0.0F, 100D, 0.0F);
				int j = 16;
				tessellator1.SetColorRGBA_F(af[0], af[1], af[2], 0.0F);

				for (int k = 0; k <= j; k++)
				{
					float f21 = ((float)k * (float)Math.PI * 2.0F) / (float)j;
					float f22 = MathHelper2.Sin(f21);
					float f23 = MathHelper2.Cos(f21);
					tessellator1.AddVertex(f22 * 120F, f23 * 120F, -f23 * 40F * af[3]);
				}

				tessellator1.Draw();
				//GL.PopMatrix();
				//GL.ShadeModel(ShadingModel.Flat);
			}

			//GL.Enable(EnableCap.Texture2D);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			//GL.PushMatrix();
			double d = 1.0F - WorldObj.GetRainStrength(par1);
			float f7 = 0.0F;
			float f9 = 0.0F;
			float f12 = 0.0F;
			//GL.Color4(1.0F, 1.0F, 1.0F, (float) d);
			//GL.Translate(f7, f9, f12);
			//GL.Rotate(-90F, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(WorldObj.GetCelestialAngle(par1) * 360F, 1.0F, 0.0F, 0.0F);
			float f15 = 30F;
			//GL.BindTexture(TextureTarget.Texture2D, RenderEngine.GetTexture("/terrain/sun.png"));
            RenderEngine.BindTexture("terrain.sun.png");
			tessellator1.StartDrawingQuads();
			tessellator1.AddVertexWithUV(-f15, 100D, -f15, 0.0F, 0.0F);
			tessellator1.AddVertexWithUV(f15, 100D, -f15, 1.0D, 0.0F);
			tessellator1.AddVertexWithUV(f15, 100D, f15, 1.0D, 1.0D);
			tessellator1.AddVertexWithUV(-f15, 100D, f15, 0.0F, 1.0D);
			tessellator1.Draw();
			f15 = 20F;
			//GL.BindTexture(TextureTarget.Texture2D, RenderEngine.GetTexture("/terrain/moon_phases.png"));
            RenderEngine.BindTexture("/terrain/moon_phases.png");
			int i18 = WorldObj.GetMoonPhase(par1);
			int l = i18 % 4;
			int i1 = (i18 / 4) % 2;
			float f24 = (float)(l + 0) / 4F;
			float f25 = (float)(i1 + 0) / 2.0F;
			float f26 = (float)(l + 1) / 4F;
			float f27 = (float)(i1 + 1) / 2.0F;
			tessellator1.StartDrawingQuads();
			tessellator1.AddVertexWithUV(-f15, -100D, f15, f26, f27);
			tessellator1.AddVertexWithUV(f15, -100D, f15, f24, f27);
			tessellator1.AddVertexWithUV(f15, -100D, -f15, f24, f25);
			tessellator1.AddVertexWithUV(-f15, -100D, -f15, f26, f25);
			tessellator1.Draw();
			//GL.Disable(EnableCap.Texture2D);
			float f18 = (float)(WorldObj.GetStarBrightness(par1) * d);

			if (f18 > 0.0F)
			{
				//GL.Color4(f18, f18, f18, f18);
				//GL.CallList(StarGLCallList);
			}

			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
			//GL.Enable(EnableCap.AlphaTest);
			//GL.Enable(EnableCap.Fog);
			//GL.PopMatrix();
			//GL.Disable(EnableCap.Texture2D);
			//GL.Color3(0.0F, 0.0F, 0.0F);
			d = Mc.ThePlayer.GetPosition(par1).YCoord - WorldObj.GetSeaLevel();

			if (d < 0.0F)
			{
				//GL.PushMatrix();
				//GL.Translate(0.0F, 12F, 0.0F);
				//GL.CallList(GlSkyList2);
				//GL.PopMatrix();
				float f10 = 1.0F;
				float f13 = -(float)(d + 65D);
				float f16 = -f10;
				float f19 = f13;
				tessellator1.StartDrawingQuads();
				tessellator1.SetColorRGBA_I(0, 255);
				tessellator1.AddVertex(-f10, f19, f10);
				tessellator1.AddVertex(f10, f19, f10);
				tessellator1.AddVertex(f10, f16, f10);
				tessellator1.AddVertex(-f10, f16, f10);
				tessellator1.AddVertex(-f10, f16, -f10);
				tessellator1.AddVertex(f10, f16, -f10);
				tessellator1.AddVertex(f10, f19, -f10);
				tessellator1.AddVertex(-f10, f19, -f10);
				tessellator1.AddVertex(f10, f16, -f10);
				tessellator1.AddVertex(f10, f16, f10);
				tessellator1.AddVertex(f10, f19, f10);
				tessellator1.AddVertex(f10, f19, -f10);
				tessellator1.AddVertex(-f10, f19, -f10);
				tessellator1.AddVertex(-f10, f19, f10);
				tessellator1.AddVertex(-f10, f16, f10);
				tessellator1.AddVertex(-f10, f16, -f10);
				tessellator1.AddVertex(-f10, f16, -f10);
				tessellator1.AddVertex(-f10, f16, f10);
				tessellator1.AddVertex(f10, f16, f10);
				tessellator1.AddVertex(f10, f16, -f10);
				tessellator1.Draw();
			}

			if (WorldObj.WorldProvider.IsSkyColored())
			{
				//GL.Color3(f * 0.2F + 0.04F, f1 * 0.2F + 0.04F, f2 * 0.6F + 0.1F);
			}
			else
			{
				//GL.Color3(f, f1, f2);
			}

			//GL.PushMatrix();
			//GL.Translate(0.0F, -(float)(d - 16D), 0.0F);
			//GL.CallList(GlSkyList2);
			//GL.PopMatrix();
			//GL.Enable(EnableCap.Texture2D);
			//GL.DepthMask(true);
		}

		public virtual void RenderClouds(float par1)
		{
			if (!Mc.TheWorld.WorldProvider.Func_48217_e())
			{
				return;
			}

			if (Mc.GameSettings.FancyGraphics)
			{
				RenderCloudsFancy(par1);
				return;
			}

			//GL.Disable(EnableCap.CullFace);
			float f = (float)(Mc.RenderViewEntity.LastTickPosY + (Mc.RenderViewEntity.PosY - Mc.RenderViewEntity.LastTickPosY) * (double)par1);
			sbyte byte0 = 32;
			int i = 256 / byte0;
			Tessellator tessellator = Tessellator.Instance;
			//GL.BindTexture(TextureTarget.Texture2D, RenderEngine.GetTexture("/environment/clouds.png"));
            RenderEngine.BindTexture("environment.clouds.png");
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			Vec3D vec3d = WorldObj.DrawClouds(par1);
			float f1 = (float)vec3d.XCoord;
			float f2 = (float)vec3d.YCoord;
			float f3 = (float)vec3d.ZCoord;

			if (Mc.GameSettings.Anaglyph)
			{
				float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
				float f6 = (f1 * 30F + f2 * 70F) / 100F;
				float f7 = (f1 * 30F + f3 * 70F) / 100F;
				f1 = f4;
				f2 = f6;
				f3 = f7;
			}

			float f5 = 0.0004882813F;
			double d = (float)CloudOffsetX + par1;
			double d1 = Mc.RenderViewEntity.PrevPosX + (Mc.RenderViewEntity.PosX - Mc.RenderViewEntity.PrevPosX) * (double)par1 + d * 0.029999999329447746D;
			double d2 = Mc.RenderViewEntity.PrevPosZ + (Mc.RenderViewEntity.PosZ - Mc.RenderViewEntity.PrevPosZ) * (double)par1;
			int j = MathHelper2.Floor_double(d1 / 2048D);
			int k = MathHelper2.Floor_double(d2 / 2048D);
			d1 -= j * 2048;
			d2 -= k * 2048;
			float f8 = (WorldObj.WorldProvider.GetCloudHeight() - f) + 0.33F;
			float f9 = (float)(d1 * (double)f5);
			float f10 = (float)(d2 * (double)f5);
			tessellator.StartDrawingQuads();
			tessellator.SetColorRGBA_F(f1, f2, f3, 0.8F);

			for (int l = -byte0 * i; l < byte0 * i; l += byte0)
			{
				for (int i1 = -byte0 * i; i1 < byte0 * i; i1 += byte0)
				{
					tessellator.AddVertexWithUV(l + 0, f8, i1 + byte0, (float)(l + 0) * f5 + f9, (float)(i1 + byte0) * f5 + f10);
					tessellator.AddVertexWithUV(l + byte0, f8, i1 + byte0, (float)(l + byte0) * f5 + f9, (float)(i1 + byte0) * f5 + f10);
					tessellator.AddVertexWithUV(l + byte0, f8, i1 + 0, (float)(l + byte0) * f5 + f9, (float)(i1 + 0) * f5 + f10);
					tessellator.AddVertexWithUV(l + 0, f8, i1 + 0, (float)(l + 0) * f5 + f9, (float)(i1 + 0) * f5 + f10);
				}
			}

			tessellator.Draw();
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
			//GL.Enable(EnableCap.CullFace);
		}

		public virtual bool Func_27307_a(double par1, double par3, double d, float f)
		{
			return false;
		}

		/// <summary>
		/// Renders the 3d fancy clouds
		/// </summary>
		public virtual void RenderCloudsFancy(float par1)
		{
			//GL.Disable(EnableCap.CullFace);
			float f = (float)(Mc.RenderViewEntity.LastTickPosY + (Mc.RenderViewEntity.PosY - Mc.RenderViewEntity.LastTickPosY) * (double)par1);
			Tessellator tessellator = Tessellator.Instance;
			float f1 = 12F;
			float f2 = 4F;
			double d = (float)CloudOffsetX + par1;
			double d1 = (Mc.RenderViewEntity.PrevPosX + (Mc.RenderViewEntity.PosX - Mc.RenderViewEntity.PrevPosX) * (double)par1 + d * 0.029999999329447746D) / (double)f1;
			double d2 = (Mc.RenderViewEntity.PrevPosZ + (Mc.RenderViewEntity.PosZ - Mc.RenderViewEntity.PrevPosZ) * (double)par1) / (double)f1 + 0.33000001311302185D;
			float f3 = (WorldObj.WorldProvider.GetCloudHeight() - f) + 0.33F;
			int i = MathHelper2.Floor_double(d1 / 2048D);
			int j = MathHelper2.Floor_double(d2 / 2048D);
			d1 -= i * 2048;
			d2 -= j * 2048;
			//GL.BindTexture(TextureTarget.Texture2D, RenderEngine.GetTexture("/environment/clouds.png"));
            RenderEngine.BindTexture("environment.clouds.png");
			//GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			Vec3D vec3d = WorldObj.DrawClouds(par1);
			float f4 = (float)vec3d.XCoord;
			float f5 = (float)vec3d.YCoord;
			float f6 = (float)vec3d.ZCoord;

			if (Mc.GameSettings.Anaglyph)
			{
				float f7 = (f4 * 30F + f5 * 59F + f6 * 11F) / 100F;
				float f9 = (f4 * 30F + f5 * 70F) / 100F;
				float f11 = (f4 * 30F + f6 * 70F) / 100F;
				f4 = f7;
				f5 = f9;
				f6 = f11;
			}

			float f8 = (float)(d1 * 0.0F);
			float f10 = (float)(d2 * 0.0F);
			float f12 = 0.00390625F;
			f8 = (float)MathHelper2.Floor_double(d1) * f12;
			f10 = (float)MathHelper2.Floor_double(d2) * f12;
			float f13 = (float)(d1 - (double)MathHelper2.Floor_double(d1));
			float f14 = (float)(d2 - (double)MathHelper2.Floor_double(d2));
			int k = 8;
			sbyte byte0 = 4;
			float f15 = 0.0009765625F;
			//GL.Scale(f1, 1.0F, f1);

			for (int l = 0; l < 2; l++)
			{
				if (l == 0)
				{
					//GL.ColorMask(false, false, false, false);
				}
				else if (Mc.GameSettings.Anaglyph)
				{
					if (EntityRenderer.AnaglyphField == 0)
					{
						//GL.ColorMask(false, true, true, true);
					}
					else
					{
						//GL.ColorMask(true, false, false, true);
					}
				}
				else
				{
					//GL.ColorMask(true, true, true, true);
				}

				for (int i1 = -byte0 + 1; i1 <= byte0; i1++)
				{
					for (int j1 = -byte0 + 1; j1 <= byte0; j1++)
					{
						tessellator.StartDrawingQuads();
						float f16 = i1 * k;
						float f17 = j1 * k;
						float f18 = f16 - f13;
						float f19 = f17 - f14;

						if (f3 > -f2 - 1.0F)
						{
							tessellator.SetColorRGBA_F(f4 * 0.7F, f5 * 0.7F, f6 * 0.7F, 0.8F);
							tessellator.SetNormal(0.0F, -1F, 0.0F);
							tessellator.AddVertexWithUV(f18 + 0.0F, f3 + 0.0F, f19 + (float)k, (f16 + 0.0F) * f12 + f8, (f17 + (float)k) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + (float)k, f3 + 0.0F, f19 + (float)k, (f16 + (float)k) * f12 + f8, (f17 + (float)k) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + (float)k, f3 + 0.0F, f19 + 0.0F, (f16 + (float)k) * f12 + f8, (f17 + 0.0F) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + 0.0F, f3 + 0.0F, f19 + 0.0F, (f16 + 0.0F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
						}

						if (f3 <= f2 + 1.0F)
						{
							tessellator.SetColorRGBA_F(f4, f5, f6, 0.8F);
							tessellator.SetNormal(0.0F, 1.0F, 0.0F);
							tessellator.AddVertexWithUV(f18 + 0.0F, (f3 + f2) - f15, f19 + (float)k, (f16 + 0.0F) * f12 + f8, (f17 + (float)k) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + (float)k, (f3 + f2) - f15, f19 + (float)k, (f16 + (float)k) * f12 + f8, (f17 + (float)k) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + (float)k, (f3 + f2) - f15, f19 + 0.0F, (f16 + (float)k) * f12 + f8, (f17 + 0.0F) * f12 + f10);
							tessellator.AddVertexWithUV(f18 + 0.0F, (f3 + f2) - f15, f19 + 0.0F, (f16 + 0.0F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
						}

						tessellator.SetColorRGBA_F(f4 * 0.9F, f5 * 0.9F, f6 * 0.9F, 0.8F);

						if (i1 > -1)
						{
							tessellator.SetNormal(-1F, 0.0F, 0.0F);

							for (int k1 = 0; k1 < k; k1++)
							{
								tessellator.AddVertexWithUV(f18 + (float)k1 + 0.0F, f3 + 0.0F, f19 + (float)k, (f16 + (float)k1 + 0.5F) * f12 + f8, (f17 + (float)k) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k1 + 0.0F, f3 + f2, f19 + (float)k, (f16 + (float)k1 + 0.5F) * f12 + f8, (f17 + (float)k) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k1 + 0.0F, f3 + f2, f19 + 0.0F, (f16 + (float)k1 + 0.5F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k1 + 0.0F, f3 + 0.0F, f19 + 0.0F, (f16 + (float)k1 + 0.5F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
							}
						}

						if (i1 <= 1)
						{
							tessellator.SetNormal(1.0F, 0.0F, 0.0F);

							for (int l1 = 0; l1 < k; l1++)
							{
								tessellator.AddVertexWithUV((f18 + (float)l1 + 1.0F) - f15, f3 + 0.0F, f19 + (float)k, (f16 + (float)l1 + 0.5F) * f12 + f8, (f17 + (float)k) * f12 + f10);
								tessellator.AddVertexWithUV((f18 + (float)l1 + 1.0F) - f15, f3 + f2, f19 + (float)k, (f16 + (float)l1 + 0.5F) * f12 + f8, (f17 + (float)k) * f12 + f10);
								tessellator.AddVertexWithUV((f18 + (float)l1 + 1.0F) - f15, f3 + f2, f19 + 0.0F, (f16 + (float)l1 + 0.5F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
								tessellator.AddVertexWithUV((f18 + (float)l1 + 1.0F) - f15, f3 + 0.0F, f19 + 0.0F, (f16 + (float)l1 + 0.5F) * f12 + f8, (f17 + 0.0F) * f12 + f10);
							}
						}

						tessellator.SetColorRGBA_F(f4 * 0.8F, f5 * 0.8F, f6 * 0.8F, 0.8F);

						if (j1 > -1)
						{
							tessellator.SetNormal(0.0F, 0.0F, -1F);

							for (int i2 = 0; i2 < k; i2++)
							{
								tessellator.AddVertexWithUV(f18 + 0.0F, f3 + f2, f19 + (float)i2 + 0.0F, (f16 + 0.0F) * f12 + f8, (f17 + (float)i2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k, f3 + f2, f19 + (float)i2 + 0.0F, (f16 + (float)k) * f12 + f8, (f17 + (float)i2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k, f3 + 0.0F, f19 + (float)i2 + 0.0F, (f16 + (float)k) * f12 + f8, (f17 + (float)i2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + 0.0F, f3 + 0.0F, f19 + (float)i2 + 0.0F, (f16 + 0.0F) * f12 + f8, (f17 + (float)i2 + 0.5F) * f12 + f10);
							}
						}

						if (j1 <= 1)
						{
							tessellator.SetNormal(0.0F, 0.0F, 1.0F);

							for (int j2 = 0; j2 < k; j2++)
							{
								tessellator.AddVertexWithUV(f18 + 0.0F, f3 + f2, (f19 + (float)j2 + 1.0F) - f15, (f16 + 0.0F) * f12 + f8, (f17 + (float)j2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k, f3 + f2, (f19 + (float)j2 + 1.0F) - f15, (f16 + (float)k) * f12 + f8, (f17 + (float)j2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + (float)k, f3 + 0.0F, (f19 + (float)j2 + 1.0F) - f15, (f16 + (float)k) * f12 + f8, (f17 + (float)j2 + 0.5F) * f12 + f10);
								tessellator.AddVertexWithUV(f18 + 0.0F, f3 + 0.0F, (f19 + (float)j2 + 1.0F) - f15, (f16 + 0.0F) * f12 + f8, (f17 + (float)j2 + 0.5F) * f12 + f10);
							}
						}

						tessellator.Draw();
					}
				}
			}

			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			//GL.Disable(EnableCap.Blend);
			//GL.Enable(EnableCap.CullFace);
		}

		/// <summary>
		/// Updates some of the renderers sorted by distance from the player
		/// </summary>
		public virtual bool UpdateRenderers(EntityLiving par1EntityLiving, bool par2)
		{
			bool flag = false;

			if (flag)
			{
				WorldRenderersToUpdate.Sort(new RenderSorter(par1EntityLiving));
				int i = WorldRenderersToUpdate.Count - 1;
				int j = WorldRenderersToUpdate.Count;

				for (int k = 0; k < j; k++)
				{
					WorldRenderer worldrenderer = WorldRenderersToUpdate[i - k];

					if (!par2)
					{
						if (worldrenderer.DistanceToEntitySquared(par1EntityLiving) > 256F)
						{
							if (worldrenderer.IsInFrustum)
							{
								if (k >= 30)
								{
									return false;
								}
							}
							else if (k >= 1)
							{
								return false;
							}
						}
					}
					else if (!worldrenderer.IsInFrustum)
					{
						continue;
					}

					worldrenderer.UpdateRenderer();
					WorldRenderersToUpdate.Remove(worldrenderer);
					worldrenderer.NeedsUpdate = false;
				}

				return WorldRenderersToUpdate.Count == 0;
			}

			byte byte0 = 2;
			RenderSorter rendersorter = new RenderSorter(par1EntityLiving);
			WorldRenderer[] aworldrenderer = new WorldRenderer[byte0];
			List<WorldRenderer> arraylist = null;
			int l = WorldRenderersToUpdate.Count;
			int i1 = 0;

			for (int j1 = 0; j1 < l; j1++)
			{
				WorldRenderer worldrenderer1 = WorldRenderersToUpdate[j1];

				if (!par2)
				{
					if (worldrenderer1.DistanceToEntitySquared(par1EntityLiving) > 256F)
					{
						int k2;

						for (k2 = 0; k2 < byte0 && (aworldrenderer[k2] == null || rendersorter.DoCompare(aworldrenderer[k2], worldrenderer1) <= 0); k2++)
						{
						}

						if (--k2 <= 0)
						{
							continue;
						}

						for (int i3 = k2; --i3 != 0;)
						{
							aworldrenderer[i3 - 1] = aworldrenderer[i3];
						}

						aworldrenderer[k2] = worldrenderer1;
						continue;
					}
				}
				else if (!worldrenderer1.IsInFrustum)
				{
					continue;
				}

				if (arraylist == null)
				{
                    arraylist = new List<WorldRenderer>();
				}

				i1++;
				arraylist.Add(worldrenderer1);
				WorldRenderersToUpdate[j1] = null;
			}

			if (arraylist != null)
			{
				if (arraylist.Count > 1)
				{
					arraylist.Sort(rendersorter);
				}

				for (int k1 = arraylist.Count - 1; k1 >= 0; k1--)
				{
					WorldRenderer worldrenderer2 = arraylist[k1];
					worldrenderer2.UpdateRenderer();
					worldrenderer2.NeedsUpdate = false;
				}
			}

			int l1 = 0;

			for (int i2 = byte0 - 1; i2 >= 0; i2--)
			{
				WorldRenderer worldrenderer3 = aworldrenderer[i2];

				if (worldrenderer3 == null)
				{
					continue;
				}

				if (!worldrenderer3.IsInFrustum && i2 != byte0 - 1)
				{
					aworldrenderer[i2] = null;
					aworldrenderer[0] = null;
					break;
				}

				aworldrenderer[i2].UpdateRenderer();
				aworldrenderer[i2].NeedsUpdate = false;
				l1++;
			}

			int j2 = 0;
			int l2 = 0;

			for (int j3 = WorldRenderersToUpdate.Count; j2 != j3; j2++)
			{
				WorldRenderer worldrenderer4 = WorldRenderersToUpdate[j2];

				if (worldrenderer4 == null)
				{
					continue;
				}

				bool flag1 = false;

				for (int k3 = 0; k3 < byte0 && !flag1; k3++)
				{
					if (worldrenderer4 == aworldrenderer[k3])
					{
						flag1 = true;
					}
				}

				if (flag1)
				{
					continue;
				}

				if (l2 != j2)
				{
					WorldRenderersToUpdate[l2] = worldrenderer4;
				}

				l2++;
			}

			while (--j2 >= l2)
			{
				WorldRenderersToUpdate.RemoveAt(j2);
			}

			return l == i1 + l1;
		}

		public virtual void DrawBlockBreaking(EntityPlayer par1EntityPlayer, MovingObjectPosition par2MovingObjectPosition, int par3, ItemStack par4ItemStack, float par5)
		{
			Tessellator tessellator = Tessellator.Instance;
			//GL.Enable(EnableCap.Blend);
			//GL.Enable(EnableCap.AlphaTest);
			//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
			//GL.Color4(1.0F, 1.0F, 1.0F, (MathHelper.Sin((float)JavaHelper.CurrentTimeMillis() / 100F) * 0.2F + 0.4F) * 0.5F);

			if (par3 == 0)
			{
				if (DamagePartialTime > 0.0F)
				{
					//GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.One);
					//int i = RenderEngine.GetTexture("terrain.png");
					//GL.BindTexture(TextureTarget.Texture2D, i);
                    RenderEngine.BindTexture("terrain.png");
					//GL.Color4(1.0F, 1.0F, 1.0F, 0.5F);
					//GL.PushMatrix();
					int j = WorldObj.GetBlockId(par2MovingObjectPosition.BlockX, par2MovingObjectPosition.BlockY, par2MovingObjectPosition.BlockZ);
					Block block = j <= 0 ? null : Block.BlocksList[j];
					//GL.Disable(EnableCap.AlphaTest);
					//GL.PolygonOffset(-3F, -3F);
					//GL.Enable(EnableCap.PolygonOffsetFill);
					double d = par1EntityPlayer.LastTickPosX + (par1EntityPlayer.PosX - par1EntityPlayer.LastTickPosX) * (double)par5;
					double d1 = par1EntityPlayer.LastTickPosY + (par1EntityPlayer.PosY - par1EntityPlayer.LastTickPosY) * (double)par5;
					double d2 = par1EntityPlayer.LastTickPosZ + (par1EntityPlayer.PosZ - par1EntityPlayer.LastTickPosZ) * (double)par5;

					if (block == null)
					{
						block = Block.Stone;
					}

					//GL.Enable(EnableCap.AlphaTest);
					tessellator.StartDrawingQuads();
					tessellator.SetTranslation(-d, -d1, -d2);
					tessellator.DisableColor();
					GlobalRenderBlocks.RenderBlockUsingTexture(block, par2MovingObjectPosition.BlockX, par2MovingObjectPosition.BlockY, par2MovingObjectPosition.BlockZ, 240 + (int)(DamagePartialTime * 10F));
					tessellator.Draw();
					tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
					//GL.Disable(EnableCap.AlphaTest);
					//GL.PolygonOffset(0.0F, 0.0F);
					//GL.Disable(EnableCap.PolygonOffsetFill);
					//GL.Enable(EnableCap.AlphaTest);
					//GL.DepthMask(true);
					//GL.PopMatrix();
				}
			}
			else if (par4ItemStack != null)
			{
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				float f = MathHelper2.Sin((float)JavaHelper.CurrentTimeMillis() / 100F) * 0.2F + 0.8F;
				//GL.Color4(f, f, f, MathHelper.Sin((float)JavaHelper.CurrentTimeMillis() / 200F) * 0.2F + 0.5F);
				//int k = RenderEngine.GetTexture("terrain.png");
				//GL.BindTexture(TextureTarget.Texture2D, k);
                RenderEngine.BindTexture("terrain.png");
				int l = par2MovingObjectPosition.BlockX;
				int i1 = par2MovingObjectPosition.BlockY;
				int j1 = par2MovingObjectPosition.BlockZ;

				if (par2MovingObjectPosition.SideHit == 0)
				{
					i1--;
				}

				if (par2MovingObjectPosition.SideHit == 1)
				{
					i1++;
				}

				if (par2MovingObjectPosition.SideHit == 2)
				{
					j1--;
				}

				if (par2MovingObjectPosition.SideHit == 3)
				{
					j1++;
				}

				if (par2MovingObjectPosition.SideHit == 4)
				{
					l--;
				}

				if (par2MovingObjectPosition.SideHit == 5)
				{
					l++;
				}
			}

			//GL.Disable(EnableCap.Blend);
			//GL.Disable(EnableCap.AlphaTest);
		}

		/// <summary>
		/// Draws the selection box for the player. Args: entityPlayer, rayTraceHit, i, itemStack, partialTickTime
		/// </summary>
		public virtual void DrawSelectionBox(EntityPlayer par1EntityPlayer, MovingObjectPosition par2MovingObjectPosition, int par3, ItemStack par4ItemStack, float par5)
		{
			if (par3 == 0 && par2MovingObjectPosition.TypeOfHit == EnumMovingObjectType.TILE)
			{
				//GL.Enable(EnableCap.Blend);
				//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
				//GL.Color4(0.0F, 0.0F, 0.0F, 0.4F);
				//GL.LineWidth(2.0F);
				//GL.Disable(EnableCap.Texture2D);
				//GL.DepthMask(false);
				float f = 0.002F;
				int i = WorldObj.GetBlockId(par2MovingObjectPosition.BlockX, par2MovingObjectPosition.BlockY, par2MovingObjectPosition.BlockZ);

				if (i > 0)
				{
					Block.BlocksList[i].SetBlockBoundsBasedOnState(WorldObj, par2MovingObjectPosition.BlockX, par2MovingObjectPosition.BlockY, par2MovingObjectPosition.BlockZ);
                    float d = par1EntityPlayer.LastTickPosX + (par1EntityPlayer.PosX - par1EntityPlayer.LastTickPosX) * par5;
                    float d1 = par1EntityPlayer.LastTickPosY + (par1EntityPlayer.PosY - par1EntityPlayer.LastTickPosY) * par5;
                    float d2 = par1EntityPlayer.LastTickPosZ + (par1EntityPlayer.PosZ - par1EntityPlayer.LastTickPosZ) * par5;
					DrawOutlinedBoundingBox(Block.BlocksList[i].GetSelectedBoundingBoxFromPool(WorldObj, par2MovingObjectPosition.BlockX, par2MovingObjectPosition.BlockY, par2MovingObjectPosition.BlockZ).Expand(f, f, f).GetOffsetBoundingBox(-d, -d1, -d2));
				}

				//GL.DepthMask(true);
				//GL.Enable(EnableCap.Texture2D);
				//GL.Disable(EnableCap.Blend);
			}
		}

		/// <summary>
		/// Draws lines for the edges of the bounding box.
		/// </summary>
		private void DrawOutlinedBoundingBox(AxisAlignedBB par1AxisAlignedBB)
		{
			Tessellator tessellator = Tessellator.Instance;
			tessellator.StartDrawing(3);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ);
			tessellator.Draw();
			tessellator.StartDrawing(3);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MinZ);
			tessellator.Draw();
			tessellator.StartDrawing(1);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MinZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MaxX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MinY, par1AxisAlignedBB.MaxZ);
			tessellator.AddVertex(par1AxisAlignedBB.MinX, par1AxisAlignedBB.MaxY, par1AxisAlignedBB.MaxZ);
			tessellator.Draw();
		}

		/// <summary>
		/// Marks the blocks in the given range for update
		/// </summary>
		public virtual void MarkBlocksForUpdate(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			int i = MathHelper2.BucketInt(par1, 16);
			int j = MathHelper2.BucketInt(par2, 16);
			int k = MathHelper2.BucketInt(par3, 16);
			int l = MathHelper2.BucketInt(par4, 16);
			int i1 = MathHelper2.BucketInt(par5, 16);
			int j1 = MathHelper2.BucketInt(par6, 16);

			for (int k1 = i; k1 <= l; k1++)
			{
				int l1 = k1 % RenderChunksWide;

				if (l1 < 0)
				{
					l1 += RenderChunksWide;
				}

				for (int i2 = j; i2 <= i1; i2++)
				{
					int j2 = i2 % RenderChunksTall;

					if (j2 < 0)
					{
						j2 += RenderChunksTall;
					}

					for (int k2 = k; k2 <= j1; k2++)
					{
						int l2 = k2 % RenderChunksDeep;

						if (l2 < 0)
						{
							l2 += RenderChunksDeep;
						}

						int i3 = (l2 * RenderChunksTall + j2) * RenderChunksWide + l1;
						WorldRenderer worldrenderer = WorldRenderers[i3];

						if (!worldrenderer.NeedsUpdate)
						{
							WorldRenderersToUpdate.Add(worldrenderer);
							worldrenderer.MarkDirty();
						}
					}
				}
			}
		}

		/// <summary>
		/// Will mark the block and neighbors that their renderers need an update (could be all the same renderer
		/// potentially) Args: x, y, z
		/// </summary>
		public virtual void MarkBlockNeedsUpdate(int par1, int par2, int par3)
		{
			MarkBlocksForUpdate(par1 - 1, par2 - 1, par3 - 1, par1 + 1, par2 + 1, par3 + 1);
		}

		/// <summary>
		/// As of mc 1.2.3 this method has exactly the same signature and does exactly the same as markBlockNeedsUpdate
		/// </summary>
		public virtual void MarkBlockNeedsUpdate2(int par1, int par2, int par3)
		{
			MarkBlocksForUpdate(par1 - 1, par2 - 1, par3 - 1, par1 + 1, par2 + 1, par3 + 1);
		}

		/// <summary>
		/// Called across all registered IWorldAccess instances when a block range is invalidated. Args: minX, minY, minZ,
		/// maxX, MaxY, maxZ
		/// </summary>
		public virtual void MarkBlockRangeNeedsUpdate(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			MarkBlocksForUpdate(par1 - 1, par2 - 1, par3 - 1, par4 + 1, par5 + 1, par6 + 1);
		}

		/// <summary>
		/// Checks all renderers that previously weren't in the frustum and 1/16th of those that previously were in the
		/// frustum for frustum clipping Args: frustum, partialTickTime
		/// </summary>
		public virtual void ClipRenderersByFrustum(ICamera par1ICamera, float par2)
		{
			for (int i = 0; i < WorldRenderers.Length; i++)
			{
				if (!WorldRenderers[i].SkipAllRenderPasses() && (!WorldRenderers[i].IsInFrustum || (i + FrustumCheckOffset & 0xf) == 0))
				{
					WorldRenderers[i].UpdateInFrustum(par1ICamera);
				}
			}

			FrustumCheckOffset++;
		}

		/// <summary>
		/// Plays the specified record. Arg: recordName, x, y, z
		/// </summary>
		public virtual void PlayRecord(string par1Str, int par2, int par3, int par4)
		{
			if (par1Str != null)
			{
				Mc.IngameGUI.SetRecordPlayingMessage((new StringBuilder()).Append("C418 - ").Append(par1Str).ToString());
			}

			Mc.SndManager.PlayStreaming(par1Str, par2, par3, par4, 1.0F, 1.0F);
		}

		/// <summary>
		/// Plays the specified sound. Arg: x, y, z, soundName, unknown1, unknown2
		/// </summary>
        public virtual void PlaySound(string par1Str, float par2, float par4, float par6, float par8, float par9)
		{
			float f = 16F;

			if (par8 > 1.0F)
			{
				f *= par8;
			}

			if (Mc.RenderViewEntity.GetDistanceSq(par2, par4, par6) < (f * f))
			{
				Mc.SndManager.PlaySound(par1Str, par2, par4, par6, par8, par9);
			}
		}

		/// <summary>
		/// Spawns a particle. Arg: particleType, x, y, z, velX, velY, velZ
		/// </summary>
        public virtual void SpawnParticle(string par1Str, float par2, float par4, float par6, float par8, float par10, float par12)
		{
			Func_40193_b(par1Str, par2, par4, par6, par8, par10, par12);
		}

        public virtual EntityFX Func_40193_b(string par1Str, float par2, float par4, float par6, float par8, float par10, float par12)
		{
			if (Mc == null || Mc.RenderViewEntity == null || Mc.EffectRenderer == null)
			{
				return null;
			}

			int i = Mc.GameSettings.ParticleSetting;

			if (i == 1 && WorldObj.Rand.Next(3) == 0)
			{
				i = 2;
			}

			double d = Mc.RenderViewEntity.PosX - par2;
			double d1 = Mc.RenderViewEntity.PosY - par4;
			double d2 = Mc.RenderViewEntity.PosZ - par6;
			EntityFX obj = null;

			if (par1Str.Equals("hugeexplosion"))
			{
				Mc.EffectRenderer.AddEffect(obj = new EntityHugeExplodeFX(WorldObj, par2, par4, par6, par8, par10, par12));
			}
			else if (par1Str.Equals("largeexplode"))
			{
				Mc.EffectRenderer.AddEffect(obj = new EntityLargeExplodeFX(RenderEngine, WorldObj, par2, par4, par6, par8, par10, par12));
			}

			if (obj != null)
			{
				return obj;
			}

			double d3 = 16D;

			if (d * d + d1 * d1 + d2 * d2 > d3 * d3)
			{
				return null;
			}

			if (i > 1)
			{
				return null;
			}

			if (par1Str.Equals("bubble"))
			{
				obj = new EntityBubbleFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("suspended"))
			{
				obj = new EntitySuspendFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("depthsuspend"))
			{
				obj = new EntityAuraFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("townaura"))
			{
				obj = new EntityAuraFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("crit"))
			{
				obj = new EntityCritFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("magicCrit"))
			{
				obj = new EntityCritFX(WorldObj, par2, par4, par6, par8, par10, par12);
				obj.Func_40097_b(obj.Func_40098_n() * 0.3F, obj.Func_40101_o() * 0.8F, obj.Func_40102_p());
				obj.SetParticleTextureIndex(obj.GetParticleTextureIndex() + 1);
			}
			else if (par1Str.Equals("smoke"))
			{
				obj = new EntitySmokeFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("mobSpell"))
			{
				obj = new EntitySpellParticleFX(WorldObj, par2, par4, par6, 0.0F, 0.0F, 0.0F);
				obj.Func_40097_b((float)par8, (float)par10, (float)par12);
			}
			else if (par1Str.Equals("spell"))
			{
				obj = new EntitySpellParticleFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("instantSpell"))
			{
				obj = new EntitySpellParticleFX(WorldObj, par2, par4, par6, par8, par10, par12);
				((EntitySpellParticleFX)obj).Func_40110_b(144);
			}
			else if (par1Str.Equals("note"))
			{
				obj = new EntityNoteFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("portal"))
			{
				obj = new EntityPortalFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("enchantmenttable"))
			{
				obj = new EntityEnchantmentTableParticleFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("explode"))
			{
				obj = new EntityExplodeFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("flame"))
			{
				obj = new EntityFlameFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("lava"))
			{
				obj = new EntityLavaFX(WorldObj, par2, par4, par6);
			}
			else if (par1Str.Equals("footstep"))
			{
				obj = new EntityFootStepFX(RenderEngine, WorldObj, par2, par4, par6);
			}
			else if (par1Str.Equals("splash"))
			{
				obj = new EntitySplashFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("largesmoke"))
			{
				obj = new EntitySmokeFX(WorldObj, par2, par4, par6, par8, par10, par12, 2.5F);
			}
			else if (par1Str.Equals("cloud"))
			{
				obj = new EntityCloudFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("reddust"))
			{
				obj = new EntityReddustFX(WorldObj, par2, par4, par6, (float)par8, (float)par10, (float)par12);
			}
			else if (par1Str.Equals("snowballpoof"))
			{
				obj = new EntityBreakingFX(WorldObj, par2, par4, par6, Item.Snowball);
			}
			else if (par1Str.Equals("dripWater"))
			{
				obj = new EntityDropParticleFX(WorldObj, par2, par4, par6, Material.Water);
			}
			else if (par1Str.Equals("dripLava"))
			{
				obj = new EntityDropParticleFX(WorldObj, par2, par4, par6, Material.Lava);
			}
			else if (par1Str.Equals("snowshovel"))
			{
				obj = new EntitySnowShovelFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.Equals("slime"))
			{
				obj = new EntityBreakingFX(WorldObj, par2, par4, par6, Item.SlimeBall);
			}
			else if (par1Str.Equals("heart"))
			{
				obj = new EntityHeartFX(WorldObj, par2, par4, par6, par8, par10, par12);
			}
			else if (par1Str.StartsWith("iconcrack_"))
			{
				int j = Convert.ToInt32(par1Str.Substring(par1Str.IndexOf("_") + 1));
				obj = new EntityBreakingFX(WorldObj, par2, par4, par6, par8, par10, par12, Item.ItemsList[j]);
			}
			else if (par1Str.StartsWith("tilecrack_"))
			{
				int k = Convert.ToInt32(par1Str.Substring(par1Str.IndexOf("_") + 1));
				obj = new EntityDiggingFX(WorldObj, par2, par4, par6, par8, par10, par12, Block.BlocksList[k], 0, 0);
			}

			if (obj != null)
			{
				Mc.EffectRenderer.AddEffect(obj);
			}

			return obj;
		}

		/// <summary>
		/// Start the skin for this entity downloading, if necessary, and increment its reference counter
		/// </summary>
		public virtual void ObtainEntitySkin(Entity par1Entity)
		{
			par1Entity.UpdateCloak();

			if (par1Entity.SkinUrl != null)
			{
				RenderEngine.ObtainImageData(par1Entity.SkinUrl, new ImageBufferDownload());
			}

			if (par1Entity.CloakUrl != null)
			{
				RenderEngine.ObtainImageData(par1Entity.CloakUrl, new ImageBufferDownload());
			}
		}

		/// <summary>
		/// Decrement the reference counter for this entity's skin image data
		/// </summary>
		public virtual void ReleaseEntitySkin(Entity par1Entity)
		{
			if (par1Entity.SkinUrl != null)
			{
				RenderEngine.ReleaseImageData(par1Entity.SkinUrl);
			}

			if (par1Entity.CloakUrl != null)
			{
				RenderEngine.ReleaseImageData(par1Entity.CloakUrl);
			}
		}

		/// <summary>
		/// In all implementations, this method does nothing.
		/// </summary>
		public virtual void DoNothingWithTileEntity(int i, int j, int k, TileEntity tileentity)
		{
		}

		public virtual void Func_28137_f()
		{
			GLAllocation.DeleteDisplayLists(GlRenderListBase);
		}

		/// <summary>
		/// Plays a pre-canned sound effect along with potentially auxiliary data-driven one-shot behaviour (particles, etc).
		/// </summary>
		public virtual void PlayAuxSFX(EntityPlayer par1EntityPlayer, int par2, int par3, int par4, int par5, int par6)
		{
			Random random = WorldObj.Rand;

			switch (par2)
			{
				default:
					break;

				case 1001:
					WorldObj.PlaySoundEffect(par3, par4, par5, "random.click", 1.0F, 1.2F);
					break;

				case 1000:
					WorldObj.PlaySoundEffect(par3, par4, par5, "random.click", 1.0F, 1.0F);
					break;

				case 1002:
					WorldObj.PlaySoundEffect(par3, par4, par5, "random.bow", 1.0F, 1.2F);
					break;

				case 2000:
					int i = par6 % 3 - 1;
					int l = (par6 / 3) % 3 - 1;
                    float d3 = par3 + i * 0.59999999999999998F + 0.5F;
                    float d7 = par4 + 0.5F;
                    float d11 = par5 + l * 0.59999999999999998F + 0.5F;

					for (int l1 = 0; l1 < 10; l1++)
					{
                        float d13 = random.NextFloat() * 0.20000000000000001F + 0.01F;
                        float d14 = d3 + i * 0.01F + (random.NextFloat() - 0.5F) * l * 0.5F;
                        float d15 = d7 + (random.NextFloat() - 0.5F) * 0.5F;
                        float d17 = d11 + l * 0.01F + (random.NextFloat() - 0.5F) * i * 0.5F;
                        float d19 = i * d13 + random.NextGaussian() * 0.01F;
                        float d21 = -0.029999999999999999F + random.NextGaussian() * 0.01F;
                        float d23 = l * d13 + random.NextGaussian() * 0.01F;
						SpawnParticle("smoke", d14, d15, d17, d19, d21, d23);
					}

					break;

				case 2003:
                    float d = par3 + 0.5F;
                    float d4 = par4;
                    float d8 = par5 + 0.5F;
					string s = (new StringBuilder()).Append("iconcrack_").Append(Item.EyeOfEnder.ShiftedIndex).ToString();

					for (int i1 = 0; i1 < 8; i1++)
					{
						SpawnParticle(s, d, d4, d8, random.NextGaussian() * 0.14999999999999999F, random.NextFloat() * 0.20000000000000001F, random.NextGaussian() * 0.14999999999999999F);
					}

					for (double d12 = 0.0F; d12 < (Math.PI * 2D); d12 += 0.15707963267948966D)
					{
                        SpawnParticle("portal", d + (float)Math.Cos(d12) * 5, d4 - 0.40000000000000002F, d8 + (float)Math.Sin(d12) * 5, (float)Math.Cos(d12) * -5F, 0.0F, (float)Math.Sin(d12) * -5F);
                        SpawnParticle("portal", d + (float)Math.Cos(d12) * 5, d4 - 0.40000000000000002F, d8 + (float)Math.Sin(d12) * 5, (float)Math.Cos(d12) * -7F, 0.0F, (float)Math.Sin(d12) * -7F);
					}

					break;

				case 2002:
                    float d1 = par3;
                    float d5 = par4;
                    float d9 = par5;
					string s1 = (new StringBuilder()).Append("iconcrack_").Append(Item.Potion.ShiftedIndex).ToString();

					for (int j1 = 0; j1 < 8; j1++)
					{
						SpawnParticle(s1, d1, d5, d9, random.NextGaussian() * 0.14999999999999999F, random.NextFloat() * 0.20000000000000001F, random.NextGaussian() * 0.14999999999999999F);
					}

					int k1 = Item.Potion.GetColorFromDamage(par6, 0);
					float f = (float)(k1 >> 16 & 0xff) / 255F;
					float f1 = (float)(k1 >> 8 & 0xff) / 255F;
					float f2 = (float)(k1 >> 0 & 0xff) / 255F;
					string s2 = "spell";

					if (Item.Potion.IsEffectInstant(par6))
					{
						s2 = "instantSpell";
					}

					for (int i2 = 0; i2 < 100; i2++)
					{
                        float d16 = random.NextFloat() * 4F;
                        float d18 = random.NextFloat() * (float)Math.PI * 2F;
                        float d20 = (float)Math.Cos(d18) * d16;
                        float d22 = 0.01F + random.NextFloat() * 0.5F;
                        float d24 = (float)Math.Sin(d18) * d16;
						EntityFX entityfx = Func_40193_b(s2, d1 + d20 * 0.10000000000000001F, d5 + 0.29999999999999999F, d9 + d24 * 0.10000000000000001F, d20, d22, d24);

						if (entityfx != null)
						{
							float f3 = 0.75F + random.NextFloat() * 0.25F;
							entityfx.Func_40097_b(f * f3, f1 * f3, f2 * f3);
							entityfx.MultiplyVelocity((float)d16);
						}
					}

					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "random.glass", 1.0F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
					break;

				case 2001:
					int j = par6 & 0xfff;

					if (j > 0)
					{
						Block block = Block.BlocksList[j];
						Mc.SndManager.PlaySound(block.StepSound.GetBreakSound(), (float)par3 + 0.5F, (float)par4 + 0.5F, (float)par5 + 0.5F, (block.StepSound.GetVolume() + 1.0F) / 2.0F, block.StepSound.GetPitch() * 0.8F);
					}

					Mc.EffectRenderer.AddBlockDestroyEffects(par3, par4, par5, par6 & 0xfff, par6 >> 12 & 0xff);
					break;

				case 2004:
					for (int k = 0; k < 20; k++)
					{
                        float d2 = par3 + 0.5F + (WorldObj.Rand.NextFloat() - 0.5F) * 2F;
                        float d6 = par4 + 0.5F + (WorldObj.Rand.NextFloat() - 0.5F) * 2F;
                        float d10 = par5 + 0.5F + (WorldObj.Rand.NextFloat() - 0.5F) * 2F;
						WorldObj.SpawnParticle("smoke", d2, d6, d10, 0.0F, 0.0F, 0.0F);
						WorldObj.SpawnParticle("flame", d2, d6, d10, 0.0F, 0.0F, 0.0F);
					}

					break;

				case 1003:
					if ((new Random(1)).NextDouble() < 0.5D)
					{
						WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "random.door_open", 1.0F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
					}
					else
					{
						WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "random.door_close", 1.0F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
					}

					break;

				case 1004:
					WorldObj.PlaySoundEffect((float)par3 + 0.5F, (float)par4 + 0.5F, (float)par5 + 0.5F, "random.fizz", 0.5F, 2.6F + (random.NextFloat() - random.NextFloat()) * 0.8F);
					break;

				case 1005:
					if (Item.ItemsList[par6] is ItemRecord)
					{
						WorldObj.PlayRecord(((ItemRecord)Item.ItemsList[par6]).RecordName, par3, par4, par5);
					}
					else
					{
						WorldObj.PlayRecord(null, par3, par4, par5);
					}

					break;

				case 1007:
					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "mob.ghast.charge", 10F, (random.NextFloat() - random.NextFloat()) * 0.2F + 1.0F);
					break;

				case 1008:
					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "mob.ghast.fireball", 10F, (random.NextFloat() - random.NextFloat()) * 0.2F + 1.0F);
					break;

				case 1010:
					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "mob.zombie.wood", 2.0F, (random.NextFloat() - random.NextFloat()) * 0.2F + 1.0F);
					break;

				case 1012:
					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "mob.zombie.woodbreak", 2.0F, (random.NextFloat() - random.NextFloat()) * 0.2F + 1.0F);
					break;

				case 1011:
					WorldObj.PlaySoundEffect(par3 + 0.5F, par4 + 0.5F, par5 + 0.5F, "mob.zombie.metal", 2.0F, (random.NextFloat() - random.NextFloat()) * 0.2F + 1.0F);
					break;
			}
		}
	}
}