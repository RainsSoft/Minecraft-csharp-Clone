using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class WorldRenderer
	{
		/// <summary>
		/// Reference to the World object. </summary>
		public World WorldObj;
		private int GlRenderList;
		private static Tessellator Tessellator;
		public static int ChunksUpdated = 0;
		public int PosX;
		public int PosY;
		public int PosZ;

		/// <summary>
		/// Pos X minus </summary>
		public int PosXMinus;

		/// <summary>
		/// Pos Y minus </summary>
		public int PosYMinus;

		/// <summary>
		/// Pos Z minus </summary>
		public int PosZMinus;

		/// <summary>
		/// Pos X clipped </summary>
		public int PosXClip;

		/// <summary>
		/// Pos Y clipped </summary>
		public int PosYClip;

		/// <summary>
		/// Pos Z clipped </summary>
		public int PosZClip;
		public bool IsInFrustum;
		public bool[] SkipRenderPass;

		/// <summary>
		/// Pos X plus </summary>
		public int PosXPlus;

		/// <summary>
		/// Pos Y plus </summary>
		public int PosYPlus;

		/// <summary>
		/// Pos Z plus </summary>
		public int PosZPlus;

		/// <summary>
		/// bool for whether this renderer needs to be updated or not </summary>
		public bool NeedsUpdate;

		/// <summary>
		/// Axis aligned bounding box </summary>
		public AxisAlignedBB RendererBoundingBox;

		/// <summary>
		/// Chunk index </summary>
		public int ChunkIndex;

		/// <summary>
		/// Is this renderer visible according to the occlusion query </summary>
		public bool IsVisible;

		/// <summary>
		/// Is this renderer waiting on the result of the occlusion query </summary>
		public bool IsWaitingOnOcclusionQuery;

		/// <summary>
		/// OpenGL occlusion query </summary>
		public int GlOcclusionQuery;

		/// <summary>
		/// Is the chunk lit </summary>
		public bool IsChunkLit;
		private bool IsInitialized;

		/// <summary>
		/// All the tile entities that have special rendering code for this chunk </summary>
		public List<TileEntity> TileEntityRenderers;
        private List<TileEntity> TileEntities;

		/// <summary>
		/// Bytes sent to the GPU </summary>
		private int BytesDrawn;

		public WorldRenderer(World par1World, List<TileEntity> par2List, int par3, int par4, int par5, int par6)
		{
			GlRenderList = -1;
			IsInFrustum = false;
			SkipRenderPass = new bool[2];
			IsVisible = true;
			IsInitialized = false;
            TileEntityRenderers = new List<TileEntity>();
			WorldObj = par1World;
			TileEntities = par2List;
			GlRenderList = par6;
			PosX = -999;
			SetPosition(par3, par4, par5);
			NeedsUpdate = false;
		}

		/// <summary>
		/// Sets a new position for the renderer and setting it up so it can be reloaded with the new data for that position
		/// </summary>
		public void SetPosition(int par1, int par2, int par3)
		{
			if (par1 == PosX && par2 == PosY && par3 == PosZ)
			{
				return;
			}
			else
			{
				SetDontDraw();
				PosX = par1;
				PosY = par2;
				PosZ = par3;
				PosXPlus = par1 + 8;
				PosYPlus = par2 + 8;
				PosZPlus = par3 + 8;
				PosXClip = par1 & 0x3ff;
				PosYClip = par2;
				PosZClip = par3 & 0x3ff;
				PosXMinus = par1 - PosXClip;
				PosYMinus = par2 - PosYClip;
				PosZMinus = par3 - PosZClip;
				float f = 6F;
				RendererBoundingBox = AxisAlignedBB.GetBoundingBox(par1 - f, par2 - f, par3 - f, (par1 + 16) + f, (par2 + 16) + f, (par3 + 16) + f);
				//GL.NewList(GlRenderList + 2, ListMode.Compile);
				RenderItem.RenderAABB(AxisAlignedBB.GetBoundingBoxFromPool(PosXClip - f, PosYClip - f, PosZClip - f, (PosXClip + 16) + f, (PosYClip + 16) + f, (PosZClip + 16) + f));
				//GL.EndList();
				MarkDirty();
				return;
			}
		}

		private void SetupGLTranslation()
		{
			//GL.Translate(PosXClip, PosYClip, PosZClip);
		}

		/// <summary>
		/// Will update this chunk renderer
		/// </summary>
		public void UpdateRenderer()
		{
			if (!NeedsUpdate)
			{
				return;
			}

			NeedsUpdate = false;
			int i = PosX;
			int j = PosY;
			int k = PosZ;
			int l = PosX + 16;
			int i1 = PosY + 16;
			int j1 = PosZ + 16;

			for (int k1 = 0; k1 < 2; k1++)
			{
				SkipRenderPass[k1] = true;
			}

			Chunk.IsLit = false;
            List<TileEntity> hashset = new List<TileEntity>();
			hashset.AddRange(TileEntityRenderers);
			TileEntityRenderers.Clear();
			int l1 = 1;
			ChunkCache chunkcache = new ChunkCache(WorldObj, i - l1, j - l1, k - l1, l + l1, i1 + l1, j1 + l1);

			if (!chunkcache.Func_48452_a())
			{
				ChunksUpdated++;
				RenderBlocks renderblocks = new RenderBlocks(chunkcache);
				BytesDrawn = 0;
				int i2 = 0;

				do
				{
					if (i2 >= 2)
					{
						break;
					}

					bool flag = false;
					bool flag1 = false;
					bool flag2 = false;

					for (int j2 = j; j2 < i1; j2++)
					{
						for (int k2 = k; k2 < j1; k2++)
						{
							for (int l2 = i; l2 < l; l2++)
							{
								int i3 = chunkcache.GetBlockId(l2, j2, k2);

								if (i3 <= 0)
								{
									continue;
								}

								if (!flag2)
								{
									flag2 = true;
									//GL.NewList(GlRenderList + i2, ListMode.Compile);
									//GL.PushMatrix();
									SetupGLTranslation();
									float f = 1.000001F;
									//GL.Translate(-8F, -8F, -8F);
									//GL.Scale(f, f, f);
									//GL.Translate(8F, 8F, 8F);
									Tessellator.StartDrawingQuads();
									Tessellator.SetTranslation(-PosX, -PosY, -PosZ);
								}

								if (i2 == 0 && Block.BlocksList[i3].HasTileEntity())
								{
									TileEntity tileentity = chunkcache.GetBlockTileEntity(l2, j2, k2);

									if (TileEntityRenderer.Instance.HasSpecialRenderer(tileentity))
									{
										TileEntityRenderers.Add(tileentity);
									}
								}

								Block block = Block.BlocksList[i3];
								int j3 = block.GetRenderBlockPass();

								if (j3 != i2)
								{
									flag = true;
									continue;
								}

								if (j3 == i2)
								{
									flag1 |= renderblocks.RenderBlockByRenderType(block, l2, j2, k2);
								}
							}
						}
					}

					if (flag2)
					{
						BytesDrawn += Tessellator.Draw();
						//GL.PopMatrix();
						//GL.EndList();
						Tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
					}
					else
					{
						flag1 = false;
					}

					if (flag1)
					{
						SkipRenderPass[i2] = false;
					}

					if (!flag)
					{
						break;
					}

					i2++;
				}
				while (true);
			}

            List<TileEntity> hashset1 = new List<TileEntity>();
			hashset1.AddRange(TileEntityRenderers);

            foreach (TileEntity te in hashset)
			    hashset1.Remove(te);

			TileEntities.AddRange(hashset1);

            foreach(TileEntity te in TileEntityRenderers)
			    hashset.Remove(te);

            foreach (TileEntity te in hashset)
			    TileEntities.Remove(te);

			IsChunkLit = Chunk.IsLit;
			IsInitialized = true;
		}

		/// <summary>
		/// Returns the distance of this chunk renderer to the entity without performing the final normalizing square root,
		/// for performance reasons.
		/// </summary>
		public float DistanceToEntitySquared(Entity par1Entity)
		{
			float f = (float)par1Entity.PosX - PosXPlus;
			float f1 = (float)par1Entity.PosY - PosYPlus;
			float f2 = (float)par1Entity.PosZ - PosZPlus;
			return f * f + f1 * f1 + f2 * f2;
		}

		/// <summary>
		/// When called this renderer won't draw anymore until its gets initialized again
		/// </summary>
		public void SetDontDraw()
		{
			for (int i = 0; i < 2; i++)
			{
				SkipRenderPass[i] = true;
			}

			IsInFrustum = false;
			IsInitialized = false;
		}

		public void StopRendering()
		{
			SetDontDraw();
			WorldObj = null;
		}

		/// <summary>
		/// Takes in the pass the call list is being requested for. Args: renderPass
		/// </summary>
		public int GetGLCallListForPass(int par1)
		{
			if (!IsInFrustum)
			{
				return -1;
			}

			if (!SkipRenderPass[par1])
			{
				return GlRenderList + par1;
			}
			else
			{
				return -1;
			}
		}

		public void UpdateInFrustum(ICamera par1ICamera)
		{
			IsInFrustum = par1ICamera.IsBoundingBoxInFrustum(RendererBoundingBox);
		}

		/// <summary>
		/// Renders the occlusion query GL List
		/// </summary>
		public void CallOcclusionQueryList()
		{
			//GL.CallList(GlRenderList + 2);
		}

		/// <summary>
		/// Checks if all render passes are to be skipped. Returns false if the renderer is not initialized
		/// </summary>
		public bool SkipAllRenderPasses()
		{
			if (!IsInitialized)
			{
				return false;
			}
			else
			{
				return SkipRenderPass[0] && SkipRenderPass[1];
			}
		}

		/// <summary>
		/// Marks the current renderer data as dirty and needing to be updated.
		/// </summary>
		public void MarkDirty()
		{
			NeedsUpdate = true;
		}

		static WorldRenderer()
		{
			Tessellator = Tessellator.Instance;
		}
	}
}