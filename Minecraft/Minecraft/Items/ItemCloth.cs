using System.Text;

namespace net.minecraft.src
{
	public class ItemCloth : ItemBlock
	{
		public ItemCloth(int par1) : base(par1)
		{
			SetMaxDamage(0);
			SetHasSubtypes(true);
		}

		/// <summary>
		/// Gets an icon index based on an item's damage value
		/// </summary>
		public override int GetIconFromDamage(int par1)
		{
			return Block.Cloth.GetBlockTextureFromSideAndMetadata(2, BlockCloth.GetBlockFromDye(par1));
		}

		/// <summary>
		/// Returns the metadata of the block which this Item (ItemBlock) can place
		/// </summary>
		public override int GetMetadata(int par1)
		{
			return par1;
		}

		public override string GetItemNameIS(ItemStack par1ItemStack)
		{
			return (new StringBuilder()).Append(base.GetItemName()).Append(".").Append(ItemDye.DyeColorNames[BlockCloth.GetBlockFromDye(par1ItemStack.GetItemDamage())]).ToString();
		}
	}
}