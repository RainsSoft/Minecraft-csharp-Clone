namespace net.minecraft.src
{
	public class ItemTool : Item
	{
		private Block[] BlocksEffectiveAgainst;
		protected float EfficiencyOnProperMaterial;

		/// <summary>
		/// Damage versus entities. </summary>
		private int DamageVsEntity;

		/// <summary>
		/// The material this tool is made from. </summary>
		protected ToolMaterial ToolMaterial;

		protected ItemTool(int par1, int par2, ToolMaterial par3EnumToolMaterial, Block[] par4ArrayOfBlock) : base(par1)
		{
			EfficiencyOnProperMaterial = 4F;
			ToolMaterial = par3EnumToolMaterial;
			BlocksEffectiveAgainst = par4ArrayOfBlock;
			MaxStackSize = 1;
			SetMaxDamage(par3EnumToolMaterial.MaxUses);
			EfficiencyOnProperMaterial = par3EnumToolMaterial.EfficiencyOnProperMaterial;
			DamageVsEntity = par2 + par3EnumToolMaterial.DamageVsEntity;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block. 1.0F base, (Quality+1)*2 if correct blocktype, 1.5F if
		/// sword
		/// </summary>
		public override float GetStrVsBlock(ItemStack par1ItemStack, Block par2Block)
		{
			for (int i = 0; i < BlocksEffectiveAgainst.Length; i++)
			{
				if (BlocksEffectiveAgainst[i] == par2Block)
				{
					return EfficiencyOnProperMaterial;
				}
			}

			return 1.0F;
		}

		/// <summary>
		/// Current implementations of this method in child classes do not use the entry argument beside ev. They just raise
		/// the damage on the stack.
		/// </summary>
		public override bool HitEntity(ItemStack par1ItemStack, EntityLiving par2EntityLiving, EntityLiving par3EntityLiving)
		{
			par1ItemStack.DamageItem(2, par3EntityLiving);
			return true;
		}

		public override bool OnBlockDestroyed(ItemStack par1ItemStack, int par2, int par3, int par4, int par5, EntityLiving par6EntityLiving)
		{
			par1ItemStack.DamageItem(1, par6EntityLiving);
			return true;
		}

		/// <summary>
		/// Returns the damage against a given entity.
		/// </summary>
		public override int GetDamageVsEntity(Entity par1Entity)
		{
			return DamageVsEntity;
		}

		/// <summary>
		/// Returns True is the item is renderer in full 3D when hold.
		/// </summary>
		public override bool IsFull3D()
		{
			return true;
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