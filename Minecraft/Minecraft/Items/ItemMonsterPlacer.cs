using System;
using System.Text;

namespace net.minecraft.src
{
	public class ItemMonsterPlacer : Item
	{
		public ItemMonsterPlacer(int par1) : base(par1)
		{
			SetHasSubtypes(true);
		}

		public override string GetItemDisplayName(ItemStack par1ItemStack)
		{
			string s = (new StringBuilder()).Append("").Append(StatCollector.TranslateToLocal((new StringBuilder()).Append(GetItemName()).Append(".name").ToString())).ToString().Trim();
			string s1 = EntityList.GetStringFromID(par1ItemStack.GetItemDamage());

			if (s1 != null)
			{
				s = (new StringBuilder()).Append(s).Append(" ").Append(StatCollector.TranslateToLocal((new StringBuilder()).Append("entity.").Append(s1).Append(".name").ToString())).ToString();
			}

			return s;
		}

		public override int GetColorFromDamage(int par1, int par2)
		{
			EntityEggInfo entityegginfo = (EntityEggInfo)EntityList.EntityEggs[par1];

			if (entityegginfo != null)
			{
				if (par2 == 0)
				{
					return entityegginfo.PrimaryColor;
				}
				else
				{
					return entityegginfo.SecondaryColor;
				}
			}
			else
			{
				return 0xffffff;
			}
		}

		public override bool Func_46058_c()
		{
			return true;
		}

		public override int Func_46057_a(int par1, int par2)
		{
			if (par2 > 0)
			{
				return base.Func_46057_a(par1, par2) + 16;
			}
			else
			{
				return base.Func_46057_a(par1, par2);
			}
		}

		/// <summary>
		/// Callback for item usage. If the item does something special on right clicking, he will have one of those. Return
		/// True if something happen and false if it don't. This is for ITEMS, not BLOCKS !
		/// </summary>
		public override bool OnItemUse(ItemStack par1ItemStack, EntityPlayer par2EntityPlayer, World par3World, int par4, int par5, int par6, int par7)
		{
			if (par3World.IsRemote)
			{
				return true;
			}

			int i = par3World.GetBlockId(par4, par5, par6);
			par4 += Facing.OffsetsXForSide[par7];
			par5 += Facing.OffsetsYForSide[par7];
			par6 += Facing.OffsetsZForSide[par7];
            float d = 0.0F;

			if (par7 == 1 && i == Block.Fence.BlockID || i == Block.NetherFence.BlockID)
			{
				d = 0.5F;
			}

			if (Func_48440_a(par3World, par1ItemStack.GetItemDamage(), par4 + 0.5F, par5 + d, par6 + 0.5F) && !par2EntityPlayer.Capabilities.IsCreativeMode)
			{
				par1ItemStack.StackSize--;
			}

			return true;
		}

        public static bool Func_48440_a(World par0World, int par1, float par2, float par4, float par6)
		{
			if (!EntityList.EntityEggs.ContainsKey(par1))
			{
				return false;
			}

			Entity entity = EntityList.CreateEntityByID(par1, par0World);

			if (entity != null)
			{
				entity.SetLocationAndAngles(par2, par4, par6, par0World.Rand.NextFloat() * 360F, 0.0F);
				par0World.SpawnEntityInWorld(entity);
				((EntityLiving)entity).PlayLivingSound();
			}

			return entity != null;
		}
	}
}