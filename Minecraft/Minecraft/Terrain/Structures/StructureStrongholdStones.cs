using System;

namespace net.minecraft.src
{
	public class StructureStrongholdStones : StructurePieceBlockSelector
	{
		public StructureStrongholdStones()
		{
		}

		public StructureStrongholdStones(StructureStrongholdPieceWeight2 par1StructureStrongholdPieceWeight2) : this()
		{
		}

		/// <summary>
		/// 'picks Block Ids and Metadata (Silverfish)'
		/// </summary>
		public override void SelectBlocks(Random par1Random, int par2, int par3, int par4, bool par5)
		{
			if (!par5)
			{
				SelectedBlockId = 0;
				SelectedBlockMetaData = 0;
			}
			else
			{
				SelectedBlockId = Block.StoneBrick.BlockID;
				float f = par1Random.NextFloat();

				if (f < 0.2F)
				{
					SelectedBlockMetaData = 2;
				}
				else if (f < 0.5F)
				{
					SelectedBlockMetaData = 1;
				}
				else if (f < 0.55F)
				{
					SelectedBlockId = Block.Silverfish.BlockID;
					SelectedBlockMetaData = 2;
				}
				else
				{
					SelectedBlockMetaData = 0;
				}
			}
		}
	}
}