namespace net.minecraft.src
{
	public class ItemSword : Item
	{
		private int WeaponDamage;
		private readonly ToolMaterial ToolMaterial;

		public ItemSword(int par1, ToolMaterial par2EnumToolMaterial) : base(par1)
		{
			ToolMaterial = par2EnumToolMaterial;
			MaxStackSize = 1;
			SetMaxDamage(par2EnumToolMaterial.MaxUses);
			WeaponDamage = 4 + par2EnumToolMaterial.DamageVsEntity;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public override float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			return par2Block.BlockID != Block.Web.BlockID ? 1.5F : 15F;
		}

		/// <summary>
		/// Current implementations of this method in child classes do not use the entry argument beside ev. They just raise
		/// the damage on the stack.
		/// </summary>
		public override bool HitEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving, EntityLiving par3EntityLiving)
		{
			par1ItemStack.DamageItem(1, par3EntityLiving);
			return true;
		}

		public override bool OnBlockDestroyed(ItemStack par1ItemStack, int par2, int par3, int par4, int par5, EntityLiving par6EntityLiving)
		{
			par1ItemStack.DamageItem(2, par6EntityLiving);
			return true;
		}

		/// <summary>
		/// Returns the damage against a given entity.
		/// </summary>
		public override int GetDamageVsEntity(Entity par1Entity)
		{
			return WeaponDamage;
		}

		/// <summary>
		/// Returns True is the item is renderer in full 3D when hold.
		/// </summary>
		public override bool IsFull3D()
		{
			return true;
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public override EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.Block;
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public override int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 0x11940;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			par3EntityPlayer.SetItemInUse(par1ItemStack, GetMaxItemUseDuration(par1ItemStack));
			return par1ItemStack;
		}

		/// <summary>
		/// Returns if the item (tool) can harvest results from the block type.
		/// </summary>
		public override bool CanHarvestBlock(Block par1Block)
		{
			return par1Block.BlockID == Block.Web.BlockID;
		}

		/// <summary>
		/// Return the enchantability factor of the item, most of the time is based on material.
		/// </summary>
		public override int GetItemEnchantability()
		{
			return ToolMaterial.Enchantability;
		}
	}
}