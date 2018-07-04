using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class ItemRecord : Item
	{
		/// <summary>
		/// The name of the record. </summary>
		public readonly string RecordName;

		public ItemRecord(int par1, string par2Str) : base(par1)
		{
			RecordName = par2Str;
			MaxStackSize = 1;
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par3World.GetBlockId(par4, par5, par6) == Block.Jukebox.BlockID && par3World.GetBlockMetadata(par4, par5, par6) == 0)
			{
				if (par3World.IsRemote)
				{
					return true;
				}
				else
				{
					((BlockJukeBox)Block.Jukebox).InsertRecord(par3World, par4, par5, par6, ShiftedIndex);
					par3World.PlayAuxSFXAtEntity(null, 1005, par4, par5, par6, ShiftedIndex);
					par1ItemStack.StackSize--;
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// allows items to add custom lines of information to the mouseover description
		/// </summary>
		public override void AddInformation(ItemStack par1ItemStack, List<string> par2List)
		{
			par2List.Add(new StringBuilder().Append("C418 - ").Append(RecordName).ToString());
		}

		/// <summary>
		/// Return an item rarity from EnumRarity
		/// </summary>
		public override Rarity GetRarity(ItemStack par1ItemStack)
		{
			return Rarity.Rare;
		}
	}
}