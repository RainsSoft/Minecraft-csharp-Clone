namespace net.minecraft.src
{
	public class ItemLeaves : ItemBlock
	{
		public ItemLeaves(int par1) : base(par1)
		{
			SetMaxDamage(0);
			SetHasSubtypes(true);
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1 | 4;
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return Block.Leaves.GetBlockTextureFromSideAndMetadata(0, par1);
		}

		public override int GetColorFromDamage(int par1, int par2)
		{
			if ((par1 & 1) == 1)
			{
				return ColorizerFoliage.GetFoliageColorPine();
			}

			if ((par1 & 2) == 2)
			{
				return ColorizerFoliage.GetFoliageColorBirch();
			}
			else
			{
				return ColorizerFoliage.GetFoliageColorBasic();
			}
		}
	}
}