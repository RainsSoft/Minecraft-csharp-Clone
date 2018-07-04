using System;

namespace net.minecraft.src
{
	public class BlockFlowing : BlockFluid
	{
		/// <summary>
		/// Number of horizontally adjacent liquid source blocks. Diagonal doesn't count. Only source blocks of the same
		/// liquid as the block using the field are counted.
		/// </summary>
		int NumAdjacentSources;
		bool[] IsOptimalFlowDirection;
		int[] FlowCost;

        public BlockFlowing(int par1, Material par2Material)
            : base(par1, par2Material)
		{
			NumAdjacentSources = 0;
			IsOptimalFlowDirection = new bool[4];
			FlowCost = new int[4];
		}

		/// <summary>
		/// Updates the flow for the BlockFlowing object.
		/// </summary>
		private void UpdateFlow(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			par1World.SetBlockAndMetadata(par2, par3, par4, BlockID + 1, i);
			par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			par1World.MarkBlockNeedsUpdate(par2, par3, par4);
		}

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return BlockMaterial != Material.Lava;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = GetFlowDecay(par1World, par2, par3, par4);
			sbyte byte0 = 1;

			if (BlockMaterial == Material.Lava && !par1World.WorldProvider.IsHellWorld)
			{
				byte0 = 2;
			}

			bool flag = true;

			if (i > 0)
			{
				int j = -100;
				NumAdjacentSources = 0;
				j = GetSmallestFlowDecay(par1World, par2 - 1, par3, par4, j);
				j = GetSmallestFlowDecay(par1World, par2 + 1, par3, par4, j);
				j = GetSmallestFlowDecay(par1World, par2, par3, par4 - 1, j);
				j = GetSmallestFlowDecay(par1World, par2, par3, par4 + 1, j);
				int k = j + byte0;

				if (k >= 8 || j < 0)
				{
					k = -1;
				}

				if (GetFlowDecay(par1World, par2, par3 + 1, par4) >= 0)
				{
					int i1 = GetFlowDecay(par1World, par2, par3 + 1, par4);

					if (i1 >= 8)
					{
						k = i1;
					}
					else
					{
						k = i1 + 8;
					}
				}

				if (NumAdjacentSources >= 2 && BlockMaterial == Material.Water)
				{
					if (par1World.GetBlockMaterial(par2, par3 - 1, par4).IsSolid())
					{
						k = 0;
					}
					else if (par1World.GetBlockMaterial(par2, par3 - 1, par4) == BlockMaterial && par1World.GetBlockMetadata(par2, par3, par4) == 0)
					{
						k = 0;
					}
				}

				if (BlockMaterial == Material.Lava && i < 8 && k < 8 && k > i && par5Random.Next(4) != 0)
				{
					k = i;
					flag = false;
				}

				if (k != i)
				{
					i = k;

					if (i < 0)
					{
						par1World.SetBlockWithNotify(par2, par3, par4, 0);
					}
					else
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
						par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
						par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
					}
				}
				else if (flag)
				{
					UpdateFlow(par1World, par2, par3, par4);
				}
			}
			else
			{
				UpdateFlow(par1World, par2, par3, par4);
			}

			if (LiquidCanDisplaceBlock(par1World, par2, par3 - 1, par4))
			{
				if (BlockMaterial == Material.Lava && par1World.GetBlockMaterial(par2, par3 - 1, par4) == Material.Water)
				{
					par1World.SetBlockWithNotify(par2, par3 - 1, par4, Block.Stone.BlockID);
					TriggerLavaMixEffects(par1World, par2, par3 - 1, par4);
					return;
				}

				if (i >= 8)
				{
					par1World.SetBlockAndMetadataWithNotify(par2, par3 - 1, par4, BlockID, i);
				}
				else
				{
					par1World.SetBlockAndMetadataWithNotify(par2, par3 - 1, par4, BlockID, i + 8);
				}
			}
			else if (i >= 0 && (i == 0 || BlockBlocksFlow(par1World, par2, par3 - 1, par4)))
			{
				bool[] aflag = GetOptimalFlowDirections(par1World, par2, par3, par4);
				int l = i + byte0;

				if (i >= 8)
				{
					l = 1;
				}

				if (l >= 8)
				{
					return;
				}

				if (aflag[0])
				{
					FlowIntoBlock(par1World, par2 - 1, par3, par4, l);
				}

				if (aflag[1])
				{
					FlowIntoBlock(par1World, par2 + 1, par3, par4, l);
				}

				if (aflag[2])
				{
					FlowIntoBlock(par1World, par2, par3, par4 - 1, l);
				}

				if (aflag[3])
				{
					FlowIntoBlock(par1World, par2, par3, par4 + 1, l);
				}
			}
		}

		/// <summary>
		/// flowIntoBlock(World world, int x, int y, int z, int newFlowDecay) - Flows into the block at the coordinates and
		/// changes the block type to the liquid.
		/// </summary>
		private void FlowIntoBlock(World par1World, int par2, int par3, int par4, int par5)
		{
			if (LiquidCanDisplaceBlock(par1World, par2, par3, par4))
			{
				int i = par1World.GetBlockId(par2, par3, par4);

				if (i > 0)
				{
					if (BlockMaterial == Material.Lava)
					{
						TriggerLavaMixEffects(par1World, par2, par3, par4);
					}
					else
					{
						Block.BlocksList[i].DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
					}
				}

				par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, BlockID, par5);
			}
		}

		/// <summary>
		/// calculateFlowCost(World world, int x, int y, int z, int accumulatedCost, int previousDirectionOfFlow) - Used to
		/// determine the path of least resistance, this method returns the lowest possible flow cost for the direction of
		/// flow indicated. Each necessary horizontal flow adds to the flow cost.
		/// </summary>
		private int CalculateFlowCost(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			int i = 1000;

			for (int j = 0; j < 4; j++)
			{
				if (j == 0 && par6 == 1 || j == 1 && par6 == 0 || j == 2 && par6 == 3 || j == 3 && par6 == 2)
				{
					continue;
				}

				int k = par2;
				int l = par3;
				int i1 = par4;

				if (j == 0)
				{
					k--;
				}

				if (j == 1)
				{
					k++;
				}

				if (j == 2)
				{
					i1--;
				}

				if (j == 3)
				{
					i1++;
				}

				if (BlockBlocksFlow(par1World, k, l, i1) || par1World.GetBlockMaterial(k, l, i1) == BlockMaterial && par1World.GetBlockMetadata(k, l, i1) == 0)
				{
					continue;
				}

				if (!BlockBlocksFlow(par1World, k, l - 1, i1))
				{
					return par5;
				}

				if (par5 >= 4)
				{
					continue;
				}

				int j1 = CalculateFlowCost(par1World, k, l, i1, par5 + 1, j);

				if (j1 < i)
				{
					i = j1;
				}
			}

			return i;
		}

		/// <summary>
		/// Returns a bool array indicating which flow directions are optimal based on each direction's calculated flow
		/// cost. Each array index corresponds to one of the four cardinal directions. A value of true indicates the
		/// direction is optimal.
		/// </summary>
		private bool[] GetOptimalFlowDirections(World par1World, int par2, int par3, int par4)
		{
			for (int i = 0; i < 4; i++)
			{
				FlowCost[i] = 1000;
				int k = par2;
				int j1 = par3;
				int k1 = par4;

				if (i == 0)
				{
					k--;
				}

				if (i == 1)
				{
					k++;
				}

				if (i == 2)
				{
					k1--;
				}

				if (i == 3)
				{
					k1++;
				}

				if (BlockBlocksFlow(par1World, k, j1, k1) || par1World.GetBlockMaterial(k, j1, k1) == BlockMaterial && par1World.GetBlockMetadata(k, j1, k1) == 0)
				{
					continue;
				}

				if (!BlockBlocksFlow(par1World, k, j1 - 1, k1))
				{
					FlowCost[i] = 0;
				}
				else
				{
					FlowCost[i] = CalculateFlowCost(par1World, k, j1, k1, 1, i);
				}
			}

			int j = FlowCost[0];

			for (int l = 1; l < 4; l++)
			{
				if (FlowCost[l] < j)
				{
					j = FlowCost[l];
				}
			}

			for (int i1 = 0; i1 < 4; i1++)
			{
				IsOptimalFlowDirection[i1] = FlowCost[i1] == j;
			}

			return IsOptimalFlowDirection;
		}

		/// <summary>
		/// Returns true if block at coords blocks fluids
		/// </summary>
		private bool BlockBlocksFlow(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3, par4);

			if (i == Block.DoorWood.BlockID || i == Block.DoorSteel.BlockID || i == Block.SignPost.BlockID || i == Block.Ladder.BlockID || i == Block.Reed.BlockID)
			{
				return true;
			}

			if (i == 0)
			{
				return false;
			}

			Material material = Block.BlocksList[i].BlockMaterial;

			if (material == Material.Portal)
			{
				return true;
			}

			return material.BlocksMovement();
		}

		/// <summary>
		/// getSmallestFlowDecay(World world, intx, int y, int z, int currentSmallestFlowDecay) - Looks up the flow decay at
		/// the coordinates given and returns the smaller of this value or the provided currentSmallestFlowDecay. If one
		/// value is valid and the other isn't, the valid value will be returned. Valid values are >= 0. Flow decay is the
		/// amount that a liquid has dissipated. 0 indicates a source block.
		/// </summary>
		protected virtual int GetSmallestFlowDecay(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = GetFlowDecay(par1World, par2, par3, par4);

			if (i < 0)
			{
				return par5;
			}

			if (i == 0)
			{
				NumAdjacentSources++;
			}

			if (i >= 8)
			{
				i = 0;
			}

			return par5 >= 0 && i >= par5 ? par5 : i;
		}

		/// <summary>
		/// Returns true if the block at the coordinates can be displaced by the liquid.
		/// </summary>
		private bool LiquidCanDisplaceBlock(World par1World, int par2, int par3, int par4)
		{
			Material material = par1World.GetBlockMaterial(par2, par3, par4);

			if (material == BlockMaterial)
			{
				return false;
			}

			if (material == Material.Lava)
			{
				return false;
			}
			else
			{
				return !BlockBlocksFlow(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);

			if (par1World.GetBlockId(par2, par3, par4) == BlockID)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
			}
		}
	}
}