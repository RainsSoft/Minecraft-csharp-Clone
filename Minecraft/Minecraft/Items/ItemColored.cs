using System.Text;

namespace net.minecraft.src
{

	public class ItemColored : ItemBlock
	{
		private readonly Block BlockRef;
		private string[] BlockNames;

		public ItemColored(int par1, bool par2) : base(par1)
		{
			BlockRef = Block.BlocksList[GetBlockID()];

			if (par2)
			{
				SetMaxDamage(0);
				SetHasSubtypes(true);
			}
		}

		public override int GetColorFromDamage(int par1, int par2)
		{
			return BlockRef.GetRenderColor(par1);
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return BlockRef.GetBlockTextureFromSideAndMetadata(0, par1);
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1;
		}

		/// <summary>
		/// sets the array of strings to be used for name lookups from item damage to metadata
		/// </summary>
		public virtual ItemColored SetBlockNames(string[] par1ArrayOfStr)
		{
			BlockNames = par1ArrayOfStr;
			return this;
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			if (BlockNames == null)
			{
				return base.GetItemNameIS(par1ItemStack);
			}

			int i = par1ItemStack.GetItemDamage();

			if (i >= 0 && i < BlockNames.Length)
			{
				return (new StringBuilder()).Append(base.GetItemNameIS(par1ItemStack)).Append(".").Append(BlockNames[i]).ToString();
			}
			else
			{
				return base.GetItemNameIS(par1ItemStack);
			}
		}
	}

}