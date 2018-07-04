namespace net.minecraft.src
{
	public class ExtendedBlockStorage
	{
		/// <summary>
		/// Contains the bottom-most Y block represented by this ExtendedBlockStorage. Typically a multiple of 16.
		/// </summary>
		private int YBase;

		/// <summary>
		/// A total count of the number of non-air blocks in this block storage's Chunk.
		/// </summary>
		private int BlockRefCount;

		/// <summary>
		/// Contains the number of blocks in this block storage's parent chunk that require random ticking. Used to cull the
		/// Chunk from random tick updates for performance reasons.
		/// </summary>
		private int TickRefCount;
		private byte[] BlockLSBArray;

		/// <summary>
		/// Contains the most significant 4 bits of each block ID belonging to this block storage's parent Chunk.
		/// </summary>
		private NibbleArray BlockMSBArray;

		/// <summary>
		/// Stores the metadata associated with blocks in this ExtendedBlockStorage.
		/// </summary>
		private NibbleArray BlockMetadataArray;

		/// <summary>
		/// The NibbleArray containing a block of Block-light data. </summary>
		private NibbleArray BlocklightArray;

		/// <summary>
		/// The NibbleArray containing a block of Sky-light data. </summary>
		private NibbleArray SkylightArray;

		public ExtendedBlockStorage(int par1)
		{
			YBase = par1;
			BlockLSBArray = new byte[4096];
			BlockMetadataArray = new NibbleArray(BlockLSBArray.Length, 4);
			SkylightArray = new NibbleArray(BlockLSBArray.Length, 4);
			BlocklightArray = new NibbleArray(BlockLSBArray.Length, 4);
		}

		/// <summary>
		/// Returns the extended block ID for a location in a chunk, merged from a byte array and a NibbleArray to form a
		/// full 12-bit block ID.
		/// </summary>
		public virtual int GetExtBlockID(int par1, int par2, int par3)
		{
			int i = BlockLSBArray[par2 << 8 | par3 << 4 | par1] & 0xff;

			if (BlockMSBArray != null)
			{
				return BlockMSBArray.Get(par1, par2, par3) << 8 | i;
			}
			else
			{
				return i;
			}
		}

		/// <summary>
		/// Sets the extended block ID for a location in a chunk, splitting bits 11..8 into a NibbleArray and bits 7..0 into
		/// a byte array. Also performs reference counting to determine whether or not to broadly cull this Chunk from the
		/// random-update tick list.
		/// </summary>
		public virtual void SetExtBlockID(int par1, int par2, int par3, int par4)
		{
			int i = BlockLSBArray[par2 << 8 | par3 << 4 | par1] & 0xff;

			if (BlockMSBArray != null)
			{
				i = BlockMSBArray.Get(par1, par2, par3) << 8 | i;
			}

			if (i == 0 && par4 != 0)
			{
				BlockRefCount++;

				if (Block.BlocksList[par4] != null && Block.BlocksList[par4].GetTickRandomly())
				{
					TickRefCount++;
				}
			}
			else if (i != 0 && par4 == 0)
			{
				BlockRefCount--;

				if (Block.BlocksList[i] != null && Block.BlocksList[i].GetTickRandomly())
				{
					TickRefCount--;
				}
			}
			else if (Block.BlocksList[i] != null && Block.BlocksList[i].GetTickRandomly() && (Block.BlocksList[par4] == null || !Block.BlocksList[par4].GetTickRandomly()))
			{
				TickRefCount--;
			}
			else if ((Block.BlocksList[i] == null || !Block.BlocksList[i].GetTickRandomly()) && Block.BlocksList[par4] != null && Block.BlocksList[par4].GetTickRandomly())
			{
				TickRefCount++;
			}

			BlockLSBArray[par2 << 8 | par3 << 4 | par1] = (byte)(par4 & 0xff);

			if (par4 > 255)
			{
				if (BlockMSBArray == null)
				{
					BlockMSBArray = new NibbleArray(BlockLSBArray.Length, 4);
				}

				BlockMSBArray.Set(par1, par2, par3, (par4 & 0xf00) >> 8);
			}
			else if (BlockMSBArray != null)
			{
				BlockMSBArray.Set(par1, par2, par3, 0);
			}
		}

		/// <summary>
		/// Returns the metadata associated with the block at the given coordinates in this ExtendedBlockStorage.
		/// </summary>
		public virtual int GetExtBlockMetadata(int par1, int par2, int par3)
		{
			return BlockMetadataArray.Get(par1, par2, par3);
		}

		/// <summary>
		/// Sets the metadata of the Block at the given coordinates in this ExtendedBlockStorage to the given metadata.
		/// </summary>
		public virtual void SetExtBlockMetadata(int par1, int par2, int par3, int par4)
		{
			BlockMetadataArray.Set(par1, par2, par3, par4);
		}

		/// <summary>
		/// Returns whether or not this block storage's Chunk is fully empty, based on its reference count.
		/// </summary>
		public virtual bool GetIsEmpty()
		{
			return BlockRefCount == 0;
		}

		/// <summary>
		/// Returns whether or not this block storage's Chunk will require random ticking, used to avoid looping through
		/// random block ticks when there are no blocks that would randomly tick.
		/// </summary>
		public virtual bool GetNeedsRandomTick()
		{
			return TickRefCount > 0;
		}

		/// <summary>
		/// Returns the Y location of this ExtendedBlockStorage.
		/// </summary>
		public virtual int GetYLocation()
		{
			return YBase;
		}

		/// <summary>
		/// Sets the saved Sky-light value in the extended block storage structure.
		/// </summary>
		public virtual void SetExtSkylightValue(int par1, int par2, int par3, int par4)
		{
			SkylightArray.Set(par1, par2, par3, par4);
		}

		/// <summary>
		/// Gets the saved Sky-light value in the extended block storage structure.
		/// </summary>
		public virtual int GetExtSkylightValue(int par1, int par2, int par3)
		{
			return SkylightArray.Get(par1, par2, par3);
		}

		/// <summary>
		/// Sets the saved Block-light value in the extended block storage structure.
		/// </summary>
		public virtual void SetExtBlocklightValue(int par1, int par2, int par3, int par4)
		{
			BlocklightArray.Set(par1, par2, par3, par4);
		}

		/// <summary>
		/// Gets the saved Block-light value in the extended block storage structure.
		/// </summary>
		public virtual int GetExtBlocklightValue(int par1, int par2, int par3)
		{
			return BlocklightArray.Get(par1, par2, par3);
		}

		public virtual void Func_48708_d()
		{
			BlockRefCount = 0;
			TickRefCount = 0;

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 16; k++)
					{
						int l = GetExtBlockID(i, j, k);

						if (l <= 0)
						{
							continue;
						}

						if (Block.BlocksList[l] == null)
						{
							BlockLSBArray[j << 8 | k << 4 | i] = 0;

							if (BlockMSBArray != null)
							{
								BlockMSBArray.Set(i, j, k, 0);
							}

							continue;
						}

						BlockRefCount++;

						if (Block.BlocksList[l].GetTickRandomly())
						{
							TickRefCount++;
						}
					}
				}
			}
		}

		public virtual void Func_48711_e()
		{
		}

		public virtual int Func_48700_f()
		{
			return BlockRefCount;
		}

		public virtual byte[] Func_48692_g()
		{
			return BlockLSBArray;
		}

		public virtual void Func_48715_h()
		{
			BlockMSBArray = null;
		}

		/// <summary>
		/// Returns the block ID MSB (bits 11..8) array for this storage array's Chunk.
		/// </summary>
		public virtual NibbleArray GetBlockMSBArray()
		{
			return BlockMSBArray;
		}

		public virtual NibbleArray Func_48697_j()
		{
			return BlockMetadataArray;
		}

		/// <summary>
		/// Returns the NibbleArray instance containing Block-light data.
		/// </summary>
		public virtual NibbleArray GetBlocklightArray()
		{
			return BlocklightArray;
		}

		/// <summary>
		/// Returns the NibbleArray instance containing Sky-light data.
		/// </summary>
		public virtual NibbleArray GetSkylightArray()
		{
			return SkylightArray;
		}

		/// <summary>
		/// Sets the array of block ID least significant bits for this ExtendedBlockStorage.
		/// </summary>
		public virtual void SetBlockLSBArray(byte[] par1ArrayOfByte)
		{
			BlockLSBArray = par1ArrayOfByte;
		}

		/// <summary>
		/// Sets the array of BlockID most significant bits (blockMSBArray) for this ExtendedBlockStorage.
		/// </summary>
		public virtual void SetBlockMSBArray(NibbleArray par1NibbleArray)
		{
			BlockMSBArray = par1NibbleArray;
		}

		/// <summary>
		/// Sets the NibbleArray of block metadata (blockMetadataArray) for this ExtendedBlockStorage.
		/// </summary>
		public virtual void SetBlockMetadataArray(NibbleArray par1NibbleArray)
		{
			BlockMetadataArray = par1NibbleArray;
		}

		/// <summary>
		/// Sets the NibbleArray instance used for Block-light values in this particular storage block.
		/// </summary>
		public virtual void SetBlocklightArray(NibbleArray par1NibbleArray)
		{
			BlocklightArray = par1NibbleArray;
		}

		/// <summary>
		/// Sets the NibbleArray instance used for Sky-light values in this particular storage block.
		/// </summary>
		public virtual void SetSkylightArray(NibbleArray par1NibbleArray)
		{
			SkylightArray = par1NibbleArray;
		}

		/// <summary>
		/// Called by a Chunk to initialize the MSB array if getBlockMSBArray returns null. Returns the newly-created
		/// NibbleArray instance.
		/// </summary>
		public virtual NibbleArray CreateBlockMSBArray()
		{
			BlockMSBArray = new NibbleArray(BlockLSBArray.Length, 4);
			return BlockMSBArray;
		}
	}
}