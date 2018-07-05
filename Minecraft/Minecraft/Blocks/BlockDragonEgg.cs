using System;

namespace net.minecraft.src
{
    public class BlockDragonEgg : Block
    {
        public BlockDragonEgg(int par1, int par2)
            : base(par1, par2, Material.DragonEgg)
        {
        }

        /// <summary>
        /// Called whenever the block is added into the world. Args: world, x, y, z
        /// </summary>
        public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
        {
            par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
        }

        /// <summary>
        /// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
        /// their own) Args: x, y, z, neighbor BlockID
        /// </summary>
        public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
        {
            par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
        }

        /// <summary>
        /// Ticks the block if it's been scheduled
        /// </summary>
        public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
        {
            FallIfPossible(par1World, par2, par3, par4);
        }

        /// <summary>
        /// Checks if the dragon egg can fall down, and if so, makes it fall.
        /// </summary>
        private void FallIfPossible(World par1World, int par2, int par3, int par4)
        {
            int i = par2;
            int j = par3;
            int k = par4;

            if (BlockSand.CanFallBelow(par1World, i, j - 1, k) && j >= 0)
            {
                sbyte byte0 = 32;

                if (BlockSand.FallInstantly || !par1World.CheckChunksExist(par2 - byte0, par3 - byte0, par4 - byte0, par2 + byte0, par3 + byte0, par4 + byte0))
                {
                    par1World.SetBlockWithNotify(par2, par3, par4, 0);

                    for (; BlockSand.CanFallBelow(par1World, par2, par3 - 1, par4) && par3 > 0; par3--)
                    {
                    }

                    if (par3 > 0)
                    {
                        par1World.SetBlockWithNotify(par2, par3, par4, BlockID);
                    }
                }
                else
                {
                    EntityFallingSand entityfallingsand = new EntityFallingSand(par1World, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, BlockID);
                    par1World.SpawnEntityInWorld(entityfallingsand);
                }
            }
        }

        /// <summary>
        /// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
        /// block.
        /// </summary>
        public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
        {
            TeleportNearby(par1World, par2, par3, par4);
            return true;
        }

        /// <summary>
        /// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
        /// </summary>
        public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
        {
            TeleportNearby(par1World, par2, par3, par4);
        }

        /// <summary>
        /// Teleports the dragon egg somewhere else in a 31x19x31 area centered on the egg.
        /// </summary>
        private void TeleportNearby(World par1World, int par2, int par3, int par4)
        {
            if (par1World.GetBlockId(par2, par3, par4) != BlockID)
            {
                return;
            }

            if (par1World.IsRemote)
            {
                return;
            }

            for (int i = 0; i < 1000; i++)
            {
                int j = (par2 + par1World.Rand.Next(16)) - par1World.Rand.Next(16);
                int k = (par3 + par1World.Rand.Next(8)) - par1World.Rand.Next(8);
                int l = (par4 + par1World.Rand.Next(16)) - par1World.Rand.Next(16);

                if (par1World.GetBlockId(j, k, l) == 0)
                {
                    par1World.SetBlockAndMetadataWithNotify(j, k, l, BlockID, par1World.GetBlockMetadata(par2, par3, par4));
                    par1World.SetBlockWithNotify(par2, par3, par4, 0);
                    int c = 200;

                    for (int i1 = 0; i1 < c; i1++)
                    {
                        double d = par1World.Rand.NextDouble();
                        float f = (par1World.Rand.NextFloat() - 0.5F) * 0.2F;
                        float f1 = (par1World.Rand.NextFloat() - 0.5F) * 0.2F;
                        float f2 = (par1World.Rand.NextFloat() - 0.5F) * 0.2F;
                        double d1 = (double)j + (double)(par2 - j) * d + (par1World.Rand.NextDouble() - 0.5D) * 1.0D + 0.5D;
                        double d2 = ((double)k + (double)(par3 - k) * d + par1World.Rand.NextDouble() * 1.0D) - 0.5D;
                        double d3 = (double)l + (double)(par4 - l) * d + (par1World.Rand.NextDouble() - 0.5D) * 1.0D + 0.5D;
                        par1World.SpawnParticle("portal", d1, d2, d3, f, f1, f2);
                    }

                    return;
                }
            }
        }

        ///<summary>
        /// How many world ticks before ticking
        ///</summary>
        public new static int TickRate()
        {
            return 3;
        }

        ///<summary>
        /// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
        ///</summary>
        public new bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
        {
            return base.CanPlaceBlockAt(par1World, par2, par3, par4);
        }

        ///<summary>
        /// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
        /// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
        ///</summary>
        public new static bool IsOpaqueCube()
        {
            return false;
        }

        ///<summary>
        /// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
        ///</summary>
        public new static bool  RenderAsNormalBlock()
        {
            return false;
        }

        ///<summary>
        /// The type of render function that is called for this block
        ///</summary>
        public new static int GetRenderType()
        {
            return 27;
        }
    }
}