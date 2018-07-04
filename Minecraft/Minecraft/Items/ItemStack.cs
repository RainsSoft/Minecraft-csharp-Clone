using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public sealed class ItemStack
	{
		/// <summary>
		/// Size of the stack. </summary>
		public int StackSize;

		/// <summary>
		/// Number of animation frames to go when receiving an item (by walking into it, for example).
		/// </summary>
		public int AnimationsToGo;

		/// <summary>
		/// ID of the item. </summary>
		public int ItemID;

		/// <summary>
		/// A NBTTagMap containing data about an ItemStack. Can only be used for non stackable items
		/// </summary>
		public NBTTagCompound StackTagCompound;

		/// <summary>
		/// Damage dealt to the item or number of use. Raise when using items. </summary>
		private int ItemDamage;

		public ItemStack(Block par1Block) : this(par1Block, 1)
		{
		}

		public ItemStack(Block par1Block, int par2) : this(par1Block.BlockID, par2, 0)
		{
		}

		public ItemStack(Block par1Block, int par2, int par3) : this(par1Block.BlockID, par2, par3)
		{
		}

		public ItemStack(Item par1Item) : this(par1Item.ShiftedIndex, 1, 0)
		{
		}

		public ItemStack(Item par1Item, int par2) : this(par1Item.ShiftedIndex, par2, 0)
		{
		}

		public ItemStack(Item par1Item, int par2, int par3) : this(par1Item.ShiftedIndex, par2, par3)
		{
		}

		public ItemStack(int par1, int par2, int par3)
		{
			StackSize = 0;
			ItemID = par1;
			StackSize = par2;
			ItemDamage = par3;
		}

		public static ItemStack LoadItemStackFromNBT(NBTTagCompound par0NBTTagCompound)
		{
			ItemStack itemstack = new ItemStack();
			itemstack.ReadFromNBT(par0NBTTagCompound);
			return itemstack.GetItem() == null ? null : itemstack;
		}

		private ItemStack()
		{
			StackSize = 0;
		}

		/// <summary>
		/// Remove the argument from the stack size. Return a new stack object with argument size.
		/// </summary>
		public ItemStack SplitStack(int par1)
		{
			ItemStack itemstack = new ItemStack(ItemID, par1, ItemDamage);

			if (StackTagCompound != null)
			{
				itemstack.StackTagCompound = (NBTTagCompound)StackTagCompound.Copy();
			}

			StackSize -= par1;
			return itemstack;
		}

		/// <summary>
		/// Returns the object corresponding to the stack.
		/// </summary>
		public Item GetItem()
		{
			return Item.ItemsList[ItemID];
		}

		/// <summary>
		/// Returns the icon index of the current stack.
		/// </summary>
		public int GetIconIndex()
		{
			return GetItem().GetIconIndex(this);
		}

		/// <summary>
		/// Uses the item stack by the player. Gives the coordinates of the block its being used against and the side. Args:
		/// player, world, x, y, z, side
		/// </summary>
		public bool UseItem(EntityPlayer par1EntityPlayer, World par2World, int par3, int par4, int par5, int par6)
		{
			bool flag = GetItem().OnItemUse(this, par1EntityPlayer, par2World, par3, par4, par5, par6);

			if (flag)
			{
				par1EntityPlayer.AddStat(StatList.ObjectUseStats[ItemID], 1);
			}

			return flag;
		}

		/// <summary>
		/// Returns the strength of the stack against a given block.
		/// </summary>
		public float GetStrVsBlock(Block par1Block)
		{
			return GetItem().GetStrVsBlock(this, par1Block);
		}

		/// <summary>
		/// Called whenever this item stack is equipped and right clicked. Returns the new item stack to put in the position
		/// where this item is. Args: world, player
		/// </summary>
		public ItemStack UseItemRightClick(World par1World, EntityPlayer par2EntityPlayer)
		{
			return GetItem().OnItemRightClick(this, par1World, par2EntityPlayer);
		}

		public ItemStack OnFoodEaten(World par1World, EntityPlayer par2EntityPlayer)
		{
			return GetItem().OnFoodEaten(this, par1World, par2EntityPlayer);
		}

		/// <summary>
		/// Write the stack fields to a NBT object. Return the new NBT object.
		/// </summary>
		public NBTTagCompound WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetShort("id", (short)ItemID);
			par1NBTTagCompound.SetByte("Count", (byte)StackSize);
			par1NBTTagCompound.SetShort("Damage", (short)ItemDamage);

			if (StackTagCompound != null)
			{
				par1NBTTagCompound.SetTag("tag", StackTagCompound);
			}

			return par1NBTTagCompound;
		}

		/// <summary>
		/// Read the stack fields from a NBT object.
		/// </summary>
		public void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			ItemID = par1NBTTagCompound.GetShort("id");
			StackSize = par1NBTTagCompound.GetByte("Count");
			ItemDamage = par1NBTTagCompound.GetShort("Damage");

			if (par1NBTTagCompound.HasKey("tag"))
			{
				StackTagCompound = par1NBTTagCompound.GetCompoundTag("tag");
			}
		}

		/// <summary>
		/// Returns maximum size of the stack.
		/// </summary>
		public int GetMaxStackSize()
		{
			return GetItem().GetItemStackLimit();
		}

		/// <summary>
		/// Returns true if the ItemStack can hold 2 or more units of the item.
		/// </summary>
		public bool IsStackable()
		{
			return GetMaxStackSize() > 1 && (!IsItemStackDamageable() || !IsItemDamaged());
		}

		/// <summary>
		/// true if this itemStack is damageable
		/// </summary>
		public bool IsItemStackDamageable()
		{
			return Item.ItemsList[ItemID].GetMaxDamage() > 0;
		}

		public bool GetHasSubtypes()
		{
			return Item.ItemsList[ItemID].GetHasSubtypes();
		}

		/// <summary>
		/// returns true when a damageable item is damaged
		/// </summary>
		public bool IsItemDamaged()
		{
			return IsItemStackDamageable() && ItemDamage > 0;
		}

		/// <summary>
		/// gets the damage of an itemstack, for displaying purposes
		/// </summary>
		public int GetItemDamageForDisplay()
		{
			return ItemDamage;
		}

		/// <summary>
		/// gets the damage of an itemstack
		/// </summary>
		public int GetItemDamage()
		{
			return ItemDamage;
		}

		/// <summary>
		/// Sets the item damage of the ItemStack.
		/// </summary>
		public void SetItemDamage(int par1)
		{
			ItemDamage = par1;
		}

		/// <summary>
		/// Returns the max damage an item in the stack can take.
		/// </summary>
		public int GetMaxDamage()
		{
			return Item.ItemsList[ItemID].GetMaxDamage();
		}

		/// <summary>
		/// Damages the item in the ItemStack
		/// </summary>
		public void DamageItem(int par1, EntityLiving par2EntityLiving)
		{
			if (!IsItemStackDamageable())
			{
				return;
			}

			if (par1 > 0 && (par2EntityLiving is EntityPlayer))
			{
				int i = EnchantmentHelper.GetUnbreakingModifier(((EntityPlayer)par2EntityLiving).Inventory);

				if (i > 0 && par2EntityLiving.WorldObj.Rand.Next(i + 1) > 0)
				{
					return;
				}
			}

			ItemDamage += par1;

			if (ItemDamage > GetMaxDamage())
			{
				par2EntityLiving.RenderBrokenItemStack(this);

				if (par2EntityLiving is EntityPlayer)
				{
					((EntityPlayer)par2EntityLiving).AddStat(StatList.ObjectBreakStats[ItemID], 1);
				}

				StackSize--;

				if (StackSize < 0)
				{
					StackSize = 0;
				}

				ItemDamage = 0;
			}
		}

		/// <summary>
		/// Calls the corresponding fct in di
		/// </summary>
		public void HitEntity(EntityLiving par1EntityLiving, EntityPlayer par2EntityPlayer)
		{
			bool flag = Item.ItemsList[ItemID].HitEntity(this, par1EntityLiving, par2EntityPlayer);

			if (flag)
			{
				par2EntityPlayer.AddStat(StatList.ObjectUseStats[ItemID], 1);
			}
		}

		public void OnDestroyBlock(int par1, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			bool flag = Item.ItemsList[ItemID].OnBlockDestroyed(this, par1, par2, par3, par4, par5EntityPlayer);

			if (flag)
			{
				par5EntityPlayer.AddStat(StatList.ObjectUseStats[ItemID], 1);
			}
		}

		/// <summary>
		/// Returns the damage against a given entity.
		/// </summary>
		public int GetDamageVsEntity(Entity par1Entity)
		{
			return Item.ItemsList[ItemID].GetDamageVsEntity(par1Entity);
		}

		/// <summary>
		/// Checks if the itemStack object can harvest a specified block
		/// </summary>
		public bool CanHarvestBlock(Block par1Block)
		{
			return Item.ItemsList[ItemID].CanHarvestBlock(par1Block);
		}

		/// <summary>
		/// Called when a given item stack is about to be destroyed due to its damage level expiring when used on a block or
		/// entity. Typically used by tools.
		/// </summary>
		public void OnItemDestroyedByUse(EntityPlayer entityplayer)
		{
		}

		/// <summary>
		/// Uses the stack on the entity.
		/// </summary>
		public void UseItemOnEntity(EntityLiving par1EntityLiving)
		{
			Item.ItemsList[ItemID].UseItemOnEntity(this, par1EntityLiving);
		}

		/// <summary>
		/// Returns a new stack with the same properties.
		/// </summary>
		public ItemStack Copy()
		{
			ItemStack itemstack = new ItemStack(ItemID, StackSize, ItemDamage);

			if (StackTagCompound != null)
			{
				itemstack.StackTagCompound = (NBTTagCompound)StackTagCompound.Copy();

				if (!itemstack.StackTagCompound.Equals(StackTagCompound))
				{
					return itemstack;
				}
			}

			return itemstack;
		}

		public static bool Func_46154_a(ItemStack par0ItemStack, ItemStack par1ItemStack)
		{
			if (par0ItemStack == null && par1ItemStack == null)
			{
				return true;
			}

			if (par0ItemStack == null || par1ItemStack == null)
			{
				return false;
			}

			if (par0ItemStack.StackTagCompound == null && par1ItemStack.StackTagCompound != null)
			{
				return false;
			}

			return par0ItemStack.StackTagCompound == null || par0ItemStack.StackTagCompound.Equals(par1ItemStack.StackTagCompound);
		}

		/// <summary>
		/// compares ItemStack argument1 with ItemStack argument2; returns true if both ItemStacks are equal
		/// </summary>
		public static bool AreItemStacksEqual(ItemStack par0ItemStack, ItemStack par1ItemStack)
		{
			if (par0ItemStack == null && par1ItemStack == null)
			{
				return true;
			}

			if (par0ItemStack == null || par1ItemStack == null)
			{
				return false;
			}
			else
			{
				return par0ItemStack.IsItemStackEqual(par1ItemStack);
			}
		}

		/// <summary>
		/// compares ItemStack argument to the instance ItemStack; returns true if both ItemStacks are equal
		/// </summary>
		private bool IsItemStackEqual(ItemStack par1ItemStack)
		{
			if (StackSize != par1ItemStack.StackSize)
			{
				return false;
			}

			if (ItemID != par1ItemStack.ItemID)
			{
				return false;
			}

			if (ItemDamage != par1ItemStack.ItemDamage)
			{
				return false;
			}

			if (StackTagCompound == null && par1ItemStack.StackTagCompound != null)
			{
				return false;
			}

			return StackTagCompound == null || StackTagCompound.Equals(par1ItemStack.StackTagCompound);
		}

		/// <summary>
		/// compares ItemStack argument to the instance ItemStack; returns true if the Items contained in both ItemStacks are
		/// equal
		/// </summary>
		public bool IsItemEqual(ItemStack par1ItemStack)
		{
			return ItemID == par1ItemStack.ItemID && ItemDamage == par1ItemStack.ItemDamage;
		}

		/// <summary>
		/// Creates a copy of a ItemStack, a null parameters will return a null.
		/// </summary>
		public static ItemStack CopyItemStack(ItemStack par0ItemStack)
		{
			return par0ItemStack != null ? par0ItemStack.Copy() : null;
		}

		public string ToString()
		{
			return (new StringBuilder()).Append(StackSize).Append("x").Append(Item.ItemsList[ItemID].GetItemName()).Append("@").Append(ItemDamage).ToString();
		}

		/// <summary>
		/// Called each tick as long the ItemStack in on player inventory. Used to progress the pickup animation and update
		/// maps.
		/// </summary>
		public void UpdateAnimation(World par1World, Entity par2Entity, int par3, bool par4)
		{
			if (AnimationsToGo > 0)
			{
				AnimationsToGo--;
			}

			Item.ItemsList[ItemID].OnUpdate(this, par1World, par2Entity, par3, par4);
		}

		public void OnCrafting(World par1World, EntityPlayer par2EntityPlayer, int par3)
		{
			par2EntityPlayer.AddStat(StatList.ObjectCraftStats[ItemID], par3);
			Item.ItemsList[ItemID].OnCreated(this, par1World, par2EntityPlayer);
		}

		public bool IsStackEqual(ItemStack par1ItemStack)
		{
			return ItemID == par1ItemStack.ItemID && StackSize == par1ItemStack.StackSize && ItemDamage == par1ItemStack.ItemDamage;
		}

		public int GetMaxItemUseDuration()
		{
			return GetItem().GetMaxItemUseDuration(this);
		}

		public EnumAction GetItemUseAction()
		{
			return GetItem().GetItemUseAction(this);
		}

		/// <summary>
		/// called when the player releases the use item button. Args: world, entityplayer, itemInUseCount
		/// </summary>
		public void OnPlayerStoppedUsing(World par1World, EntityPlayer par2EntityPlayer, int par3)
		{
			GetItem().OnPlayerStoppedUsing(this, par1World, par2EntityPlayer, par3);
		}

		/// <summary>
		/// Returns true if the ItemStack have a NBTTagCompound. Used currently to store enchantments.
		/// </summary>
		public bool HasTagCompound()
		{
			return StackTagCompound != null;
		}

		/// <summary>
		/// Returns the NBTTagCompound of the ItemStack.
		/// </summary>
		public NBTTagCompound GetTagCompound()
		{
			return StackTagCompound;
		}

		public NBTTagList GetEnchantmentTagList()
		{
			if (StackTagCompound == null)
			{
				return null;
			}
			else
			{
				return (NBTTagList)StackTagCompound.GetTag("ench");
			}
		}

		/// <summary>
		/// Assigns a NBTTagCompound to the ItemStack, minecraft validates that only non-stackable items can have it.
		/// </summary>
		public void SetTagCompound(NBTTagCompound par1NBTTagCompound)
		{
			StackTagCompound = par1NBTTagCompound;
		}

		/// <summary>
		/// gets a list of strings representing the item name and successive extra data, eg Enchantments and potion effects
		/// </summary>
        public List<string> GetItemNameandInformation()
		{
            List<string> arraylist = new List<string>();
			Item item = Item.ItemsList[ItemID];
			arraylist.Add(item.GetItemDisplayName(this));
			item.AddInformation(this, arraylist);

			if (HasTagCompound())
			{
				NBTTagList nbttaglist = GetEnchantmentTagList();

				if (nbttaglist != null)
				{
					for (int i = 0; i < nbttaglist.TagCount(); i++)
					{
						short word0 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("id");
						short word1 = ((NBTTagCompound)nbttaglist.TagAt(i)).GetShort("lvl");

						if (Enchantment.EnchantmentsList[word0] != null)
						{
							arraylist.Add(Enchantment.EnchantmentsList[word0].GetTranslatedName(word1));
						}
					}
				}
			}

			return arraylist;
		}

		public bool HasEffect()
		{
			return GetItem().HasEffect(this);
		}

		public Rarity GetRarity()
		{
			return GetItem().GetRarity(this);
		}

		/// <summary>
		/// True if it is a tool and has no enchantments to begin with
		/// </summary>
		public bool IsItemEnchantable()
		{
			if (!GetItem().IsItemTool(this))
			{
				return false;
			}

			return !IsItemEnchanted();
		}

		/// <summary>
		/// Adds a enchantments with a desired level on the ItemStack.
		/// </summary>
		public void AddEnchantment(Enchantment par1Enchantment, int par2)
		{
			if (StackTagCompound == null)
			{
				SetTagCompound(new NBTTagCompound());
			}

			if (!StackTagCompound.HasKey("ench"))
			{
				StackTagCompound.SetTag("ench", new NBTTagList("ench"));
			}

			NBTTagList nbttaglist = (NBTTagList)StackTagCompound.GetTag("ench");
			NBTTagCompound nbttagcompound = new NBTTagCompound();
			nbttagcompound.SetShort("id", (short)par1Enchantment.EffectId);
			nbttagcompound.SetShort("lvl", (sbyte)par2);
			nbttaglist.AppendTag(nbttagcompound);
		}

		/// <summary>
		/// True if the item has enchantment data
		/// </summary>
		public bool IsItemEnchanted()
		{
			return StackTagCompound != null && StackTagCompound.HasKey("ench");
		}
	}
}