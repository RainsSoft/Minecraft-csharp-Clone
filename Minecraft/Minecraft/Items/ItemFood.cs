namespace net.minecraft.src
{
	public class ItemFood : Item
	{
		public readonly int Field_35430_a = 32;

		/// <summary>
		/// The amount this food item heals the player. </summary>
		private readonly int HealAmount;
		private readonly float SaturationModifier;

		/// <summary>
		/// Whether wolves like this food (true for raw and cooked porkchop). </summary>
		private readonly bool isWolfsFavoriteMeat_Renamed;

		/// <summary>
		/// If this field is true, the food can be consumed even if the player don't need to eat.
		/// </summary>
		private bool AlwaysEdible;

		/// <summary>
		/// represents the potion effect that will occurr upon eating this food. Set by setPotionEffect
		/// </summary>
		private int PotionId;

		/// <summary>
		/// set by setPotionEffect </summary>
		private int PotionDuration;

		/// <summary>
		/// set by setPotionEffect </summary>
		private int PotionAmplifier;

		/// <summary>
		/// probably of the set potion effect occurring </summary>
		private float PotionEffectProbability;

		public ItemFood(int par1, int par2, float par3, bool par4) : base(par1)
		{
			HealAmount = par2;
			isWolfsFavoriteMeat_Renamed = par4;
			SaturationModifier = par3;
		}

		public ItemFood(int par1, int par2, bool par3) : this(par1, par2, 0.6F, par3)
		{
		}

		public override ItemStack OnFoodEaten(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			par1ItemStack.StackSize--;
			par3EntityPlayer.GetFoodStats().AddStats(this);
			par2World.PlaySoundAtEntity(par3EntityPlayer, "random.burp", 0.5F, par2World.Rand.NextFloat() * 0.1F + 0.9F);

			if (!par2World.IsRemote && PotionId > 0 && par2World.Rand.NextFloat() < PotionEffectProbability)
			{
				par3EntityPlayer.AddPotionEffect(new PotionEffect(PotionId, PotionDuration * 20, PotionAmplifier));
			}

			return par1ItemStack;
		}

		/// <summary>
		/// How long it takes to use or consume an item
		/// </summary>
		public override int GetMaxItemUseDuration(ItemStack par1ItemStack)
		{
			return 32;
		}

		/// <summary>
		/// returns the action that specifies what animation to play when the items is being used
		/// </summary>
		public override EnumAction GetItemUseAction(ItemStack par1ItemStack)
		{
			return EnumAction.Eat;
		}

		/// <summary>
		/// Called whenever this item is equipped and the right mouse button is pressed. Args: itemStack, world, entityPlayer
		/// </summary>
		public override ItemStack OnItemRightClick(ItemStack par1ItemStack, World par2World, EntityPlayer par3EntityPlayer)
		{
			if (par3EntityPlayer.CanEat(AlwaysEdible))
			{
				par3EntityPlayer.SetItemInUse(par1ItemStack, GetMaxItemUseDuration(par1ItemStack));
			}

			return par1ItemStack;
		}

		public virtual int GetHealAmount()
		{
			return HealAmount;
		}

		/// <summary>
		/// gets the saturationModifier of the ItemFood
		/// </summary>
		public virtual float GetSaturationModifier()
		{
			return SaturationModifier;
		}

		/// <summary>
		/// Whether wolves like this food (true for raw and cooked porkchop).
		/// </summary>
		public virtual bool IsWolfsFavoriteMeat()
		{
			return isWolfsFavoriteMeat_Renamed;
		}

		/// <summary>
		/// sets a potion effect on the item. Args: int potionId, int duration (will be multiplied by 20), int amplifier,
		/// float probability of effect happening
		/// </summary>
		public virtual ItemFood SetPotionEffect(int par1, int par2, int par3, float par4)
		{
			PotionId = par1;
			PotionDuration = par2;
			PotionAmplifier = par3;
			PotionEffectProbability = par4;
			return this;
		}

		/// <summary>
		/// Set the field 'alwaysEdible' to true, and make the food edible even if the player don't need to eat.
		/// </summary>
		public virtual ItemFood SetAlwaysEdible()
		{
			AlwaysEdible = true;
			return this;
		}

		/// <summary>
		/// set name of item from language file
		/// </summary>
		public override Item SetItemName(string par1Str)
		{
			return base.SetItemName(par1Str);
		}
	}

}