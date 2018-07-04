using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class TileEntity
	{
		/// <summary>
		/// A HashMap storing string names of classes mapping to the actual java.lang.Class type.
		/// </summary>
        private static Dictionary<string, Type> NameToClassMap = new Dictionary<string, Type>();

		/// <summary>
		/// A HashMap storing the classes and mapping to the string names (reverse of nameToClassMap).
		/// </summary>
        private static Dictionary<Type, string> ClassToNameMap = new Dictionary<Type, string>();

		/// <summary>
		/// The reference to the world. </summary>
		public World WorldObj;

		/// <summary>
		/// The x coordinate of the tile entity. </summary>
		public int XCoord;

		/// <summary>
		/// The y coordinate of the tile entity. </summary>
		public int YCoord;

		/// <summary>
		/// The z coordinate of the tile entity. </summary>
		public int ZCoord;
		protected bool TileEntityInvalid;
		public int BlockMetadata;

		/// <summary>
		/// the Block type that this TileEntity is contained within </summary>
		public Block BlockType;

		public TileEntity()
		{
			BlockMetadata = -1;
		}

		/// <summary>
		/// Adds a new two-way mapping between the class and its string name in both hashmaps.
		/// </summary>
		private static void AddMapping(Type par0Class, string par1Str)
		{
			if (ClassToNameMap.ContainsValue(par1Str))
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Duplicate id: ").Append(par1Str).ToString());
			}
			else
			{
				NameToClassMap[par1Str] = par0Class;
				ClassToNameMap[par0Class] = par1Str;
				return;
			}
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public virtual void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			XCoord = par1NBTTagCompound.GetInteger("x");
			YCoord = par1NBTTagCompound.GetInteger("y");
			ZCoord = par1NBTTagCompound.GetInteger("z");
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public virtual void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			string s = ClassToNameMap[this.GetType()];

			if (s == null)
			{
				throw new Exception((new StringBuilder()).Append(this.GetType()).Append(" is missing a mapping! This is a bug!").ToString());
			}
			else
			{
				par1NBTTagCompound.SetString("id", s);
				par1NBTTagCompound.SetInteger("x", XCoord);
				par1NBTTagCompound.SetInteger("y", YCoord);
				par1NBTTagCompound.SetInteger("z", ZCoord);
				return;
			}
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public virtual void UpdateEntity()
		{
		}

		/// <summary>
		/// Creates a new entity and loads its data from the specified NBT.
		/// </summary>
		public static TileEntity CreateAndLoadEntity(NBTTagCompound par0NBTTagCompound)
		{
			TileEntity tileentity = null;

			try
			{
				Type class1 = NameToClassMap[par0NBTTagCompound.GetString("id")];

				if (class1 != null)
				{
					tileentity = (TileEntity)Activator.CreateInstance(class1);
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			if (tileentity != null)
			{
				tileentity.ReadFromNBT(par0NBTTagCompound);
			}
			else
			{
				Console.WriteLine((new StringBuilder()).Append("Skipping TileEntity with id ").Append(par0NBTTagCompound.GetString("id")).ToString());
			}

			return tileentity;
		}

		/// <summary>
		/// Returns block data at the location of this entity (client-only).
		/// </summary>
		public virtual int GetBlockMetadata()
		{
			if (BlockMetadata == -1)
			{
				BlockMetadata = WorldObj.GetBlockMetadata(XCoord, YCoord, ZCoord);
			}

			return BlockMetadata;
		}

		/// <summary>
		/// Called when an the contents of an Inventory change, usually
		/// </summary>
		public virtual void OnInventoryChanged()
		{
			if (WorldObj != null)
			{
				BlockMetadata = WorldObj.GetBlockMetadata(XCoord, YCoord, ZCoord);
				WorldObj.UpdateTileEntityChunkAndDoNothing(XCoord, YCoord, ZCoord, this);
			}
		}

		/// <summary>
		/// Returns the square of the distance between this entity and the passed in coordinates.
		/// </summary>
		public virtual double GetDistanceFrom(double par1, double par3, double par5)
		{
			double d = ((double)XCoord + 0.5D) - par1;
			double d1 = ((double)YCoord + 0.5D) - par3;
			double d2 = ((double)ZCoord + 0.5D) - par5;
			return d * d + d1 * d1 + d2 * d2;
		}

		/// <summary>
		/// Gets the block type at the location of this entity (client-only).
		/// </summary>
		public virtual Block GetBlockType()
		{
			if (BlockType == null)
			{
				BlockType = Block.BlocksList[WorldObj.GetBlockId(XCoord, YCoord, ZCoord)];
			}

			return BlockType;
		}

		/// <summary>
		/// returns true if tile entity is invalid, false otherwise
		/// </summary>
		public virtual bool IsInvalid()
		{
			return TileEntityInvalid;
		}

		/// <summary>
		/// invalidates a tile entity
		/// </summary>
		public virtual void Invalidate()
		{
			TileEntityInvalid = true;
		}

		/// <summary>
		/// validates a tile entity
		/// </summary>
		public virtual void Validate()
		{
			TileEntityInvalid = false;
		}

		public virtual void OnTileEntityPowered(int i, int j)
		{
		}

		/// <summary>
		/// causes the TileEntity to reset all it's cached values for it's container block, BlockID, metaData and in the case
		/// of chests, the adjcacent chest check
		/// </summary>
		public virtual void UpdateContainingBlockInfo()
		{
			BlockType = null;
			BlockMetadata = -1;
		}

		static TileEntity()
		{
			AddMapping(typeof(net.minecraft.src.TileEntityFurnace), "Furnace");
			AddMapping(typeof(net.minecraft.src.TileEntityChest), "Chest");
			AddMapping(typeof(net.minecraft.src.TileEntityRecordPlayer), "RecordPlayer");
			AddMapping(typeof(net.minecraft.src.TileEntityDispenser), "Trap");
			AddMapping(typeof(net.minecraft.src.TileEntitySign), "Sign");
			AddMapping(typeof(net.minecraft.src.TileEntityMobSpawner), "MobSpawner");
			AddMapping(typeof(net.minecraft.src.TileEntityNote), "Music");
			AddMapping(typeof(net.minecraft.src.TileEntityPiston), "Piston");
			AddMapping(typeof(net.minecraft.src.TileEntityBrewingStand), "Cauldron");
			AddMapping(typeof(net.minecraft.src.TileEntityEnchantmentTable), "EnchantTable");
			AddMapping(typeof(net.minecraft.src.TileEntityEndPortal), "Airportal");
		}
	}
}