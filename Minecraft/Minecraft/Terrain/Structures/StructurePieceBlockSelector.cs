using System;

namespace net.minecraft.src
{
	public abstract class StructurePieceBlockSelector
	{
		protected int SelectedBlockId;
		protected int SelectedBlockMetaData;

		protected StructurePieceBlockSelector()
		{
		}

		/// <summary>
		/// 'picks Block Ids and Metadata (Silverfish)'
		/// </summary>
		public abstract void SelectBlocks(Random random, int i, int j, int k, bool flag);

		public virtual int GetSelectedBlockId()
		{
			return SelectedBlockId;
		}

		public virtual int GetSelectedBlockMetaData()
		{
			return SelectedBlockMetaData;
		}
	}

}