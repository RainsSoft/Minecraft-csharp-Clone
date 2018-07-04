using System.Collections.Generic;

namespace net.minecraft.src
{
	class ContainerCreative : Container
	{
		/// <summary>
		/// the list of items in this container </summary>
		public List<ItemStack> ItemList;

		public ContainerCreative(EntityPlayer par1EntityPlayer)
		{
            ItemList = new List<ItemStack>();
			Block[] ablock = { Block.Cobblestone, Block.Stone, Block.OreDiamond, Block.OreGold, Block.OreIron, Block.OreCoal, Block.OreLapis, Block.OreRedstone, Block.StoneBrick, Block.StoneBrick, Block.StoneBrick, Block.StoneBrick, Block.BlockClay, Block.BlockDiamond, Block.BlockGold, Block.BlockSteel, Block.Bedrock, Block.BlockLapis, Block.Brick, Block.CobblestoneMossy, Block.StairSingle, Block.StairSingle, Block.StairSingle, Block.StairSingle, Block.StairSingle, Block.StairSingle, Block.Obsidian, Block.Netherrack, Block.SlowSand, Block.GlowStone, Block.Wood, Block.Wood, Block.Wood, Block.Wood, Block.Leaves, Block.Leaves, Block.Leaves, Block.Leaves, Block.Dirt, Block.Grass, Block.Sand, Block.SandStone, Block.SandStone, Block.SandStone, Block.Gravel, Block.Web, Block.Planks, Block.Planks, Block.Planks, Block.Planks, Block.Sapling, Block.Sapling, Block.Sapling, Block.Sapling, Block.DeadBush, Block.Sponge, Block.Ice, Block.BlockSnow, Block.PlantYellow, Block.PlantRed, Block.MushroomBrown, Block.MushroomRed, Block.Cactus, Block.Melon, Block.Pumpkin, Block.PumpkinLantern, Block.Vine, Block.FenceIron, Block.ThinGlass, Block.NetherBrick, Block.NetherFence, Block.StairsNetherBrick, Block.WhiteStone, Block.Mycelium, Block.Waterlily, Block.TallGrass, Block.TallGrass, Block.Chest, Block.Workbench, Block.Glass, Block.Tnt, Block.BookShelf, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Cloth, Block.Dispenser, Block.StoneOvenIdle, Block.Music, Block.Jukebox, Block.PistonStickyBase, Block.PistonBase, Block.Fence, Block.FenceGate, Block.Ladder, Block.Rail, Block.RailPowered, Block.RailDetector, Block.TorchWood, Block.StairCompactPlanks, Block.StairCompactCobblestone, Block.StairsBrick, Block.StairsStoneBrickSmooth, Block.Lever, Block.PressurePlateStone, Block.PressurePlatePlanks, Block.TorchRedstoneActive, Block.Button, Block.Trapdoor, Block.EnchantmentTable, Block.RedstoneLampIdle };
			int i = 0;
			int j = 0;
			int k = 0;
			int l = 0;
			int i1 = 0;
			int j1 = 0;
			int k1 = 0;
			int l1 = 0;
			int i2 = 1;

			for (int j2 = 0; j2 < ablock.Length; j2++)
			{
				int i3 = 0;

				if (ablock[j2] == Block.Cloth)
				{
					i3 = i++;
				}
				else if (ablock[j2] == Block.StairSingle)
				{
					i3 = j++;
				}
				else if (ablock[j2] == Block.Wood)
				{
					i3 = k++;
				}
				else if (ablock[j2] == Block.Planks)
				{
					i3 = l++;
				}
				else if (ablock[j2] == Block.Sapling)
				{
					i3 = i1++;
				}
				else if (ablock[j2] == Block.StoneBrick)
				{
					i3 = j1++;
				}
				else if (ablock[j2] == Block.SandStone)
				{
					i3 = k1++;
				}
				else if (ablock[j2] == Block.TallGrass)
				{
					i3 = i2++;
				}
				else if (ablock[j2] == Block.Leaves)
				{
					i3 = l1++;
				}

				ItemList.Add(new ItemStack(ablock[j2], 1, i3));
			}

			for (int k2 = 256; k2 < Item.ItemsList.Length; k2++)
			{
				if (Item.ItemsList[k2] != null && Item.ItemsList[k2].ShiftedIndex != Item.Potion.ShiftedIndex && Item.ItemsList[k2].ShiftedIndex != Item.MonsterPlacer.ShiftedIndex)
				{
					ItemList.Add(new ItemStack(Item.ItemsList[k2]));
				}
			}

			for (int l2 = 1; l2 < 16; l2++)
			{
				ItemList.Add(new ItemStack(Item.DyePowder.ShiftedIndex, 1, l2));
			}

			int integer;

			for (IEnumerator<int> iterator = EntityList.EntityEggs.Keys.GetEnumerator(); iterator.MoveNext(); ItemList.Add(new ItemStack(Item.MonsterPlacer.ShiftedIndex, 1, integer)))
			{
				integer = iterator.Current;
			}

			InventoryPlayer inventoryplayer = par1EntityPlayer.Inventory;

			for (int j3 = 0; j3 < 9; j3++)
			{
				for (int l3 = 0; l3 < 8; l3++)
				{
					AddSlot(new Slot(GuiContainerCreative.GetInventory(), l3 + j3 * 8, 8 + l3 * 18, 18 + j3 * 18));
				}
			}

			for (int k3 = 0; k3 < 9; k3++)
			{
				AddSlot(new Slot(inventoryplayer, k3, 8 + k3 * 18, 184));
			}

			ScrollTo(0.0F);
		}

		public override bool CanInteractWith(EntityPlayer par1EntityPlayer)
		{
			return true;
		}

		/// <summary>
		/// Updates the gui slots ItemStack's based on scroll position.
		/// </summary>
		public virtual void ScrollTo(float par1)
		{
			int i = (ItemList.Count / 8 - 8) + 1;
			int j = (int)((double)(par1 * (float)i) + 0.5D);

			if (j < 0)
			{
				j = 0;
			}

			for (int k = 0; k < 9; k++)
			{
				for (int l = 0; l < 8; l++)
				{
					int i1 = l + (k + j) * 8;

					if (i1 >= 0 && i1 < ItemList.Count)
					{
						GuiContainerCreative.GetInventory().SetInventorySlotContents(l + k * 8, (ItemStack)ItemList[i1]);
					}
					else
					{
						GuiContainerCreative.GetInventory().SetInventorySlotContents(l + k * 8, null);
					}
				}
			}
		}

		protected override void RetrySlotClick(int i, int j, bool flag, EntityPlayer entityplayer)
		{
		}
	}
}