using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
    public class RenderBlocks
    {
        /// <summary>
        /// The IBlockAccess used by this Instance of RenderBlocks </summary>
        public IBlockAccess BlockAccess;

        /// <summary>
        /// If set to >=0, all block faces will be rendered using this texture index
        /// </summary>
        private int overrideBlockTexture;

        /// <summary>
        /// Set to true if the texture should be flipped horizontally during render*Face
        /// </summary>
        private bool flipTexture;

        /// <summary>
        /// If true, renders all faces on all blocks rather than using the logic in Block.ShouldSideBeRendered.  Unused.
        /// </summary>
        private bool renderAllFaces;

        /// <summary>
        /// Fancy grass side matching biome </summary>
        public static bool FancyGrass = true;
        public bool UseInventoryTint;
        private int uvRotateEast;
        private int uvRotateWest;
        private int uvRotateSouth;
        private int uvRotateNorth;
        private int uvRotateTop;
        private int uvRotateBottom;

        /// <summary>
        /// Whether ambient occlusion is enabled or not </summary>
        private bool enableAo;

        /// <summary>
        /// Light value of the block itself </summary>
        private float lightValueOwn;

        /// <summary>
        /// Light value one block less in x axis </summary>
        private float aoLightValueXNeg;

        /// <summary>
        /// Light value one block more in y axis </summary>
        private float aoLightValueYNeg;

        /// <summary>
        /// Light value one block more in z axis </summary>
        private float aoLightValueZNeg;

        /// <summary>
        /// Light value one block more in x axis </summary>
        private float aoLightValueXPos;

        /// <summary>
        /// Light value one block more in y axis </summary>
        private float aoLightValueYPos;

        /// <summary>
        /// Light value one block more in z axis </summary>
        private float aoLightValueZPos;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the north/bottom/east corner.
        /// </summary>
        private float aoLightValueScratchXYZNNN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the bottom face and the north face.
        /// </summary>
        private float aoLightValueScratchXYNN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the north/bottom/west corner.
        /// </summary>
        private float aoLightValueScratchXYZNNP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the bottom face and the east face.
        /// </summary>
        private float aoLightValueScratchYZNN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the bottom face and the west face.
        /// </summary>
        private float aoLightValueScratchYZNP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the south/bottom/east corner.
        /// </summary>
        private float aoLightValueScratchXYZPNN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the bottom face and the south face.
        /// </summary>
        private float aoLightValueScratchXYPN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the south/bottom/west corner.
        /// </summary>
        private float aoLightValueScratchXYZPNP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the north/top/east corner.
        /// </summary>
        private float aoLightValueScratchXYZNPN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the top face and the north face.
        /// </summary>
        private float AoLightValueScratchXYNP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the north/top/west corner.
        /// </summary>
        private float AoLightValueScratchXYZNPP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the top face and the east face.
        /// </summary>
        private float AoLightValueScratchYZPN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the south/top/east corner.
        /// </summary>
        private float AoLightValueScratchXYZPPN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the top face and the south face.
        /// </summary>
        private float AoLightValueScratchXYPP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the top face and the west face.
        /// </summary>
        private float AoLightValueScratchYZPP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion on the south/top/west corner.
        /// </summary>
        private float AoLightValueScratchXYZPPP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the north face and the east face.
        /// </summary>
        private float AoLightValueScratchXZNN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the south face and the east face.
        /// </summary>
        private float AoLightValueScratchXZPN;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the north face and the west face.
        /// </summary>
        private float AoLightValueScratchXZNP;

        /// <summary>
        /// Used as a scratch variable for ambient occlusion between the south face and the west face.
        /// </summary>
        private float AoLightValueScratchXZPP;

        /// <summary>
        /// Ambient occlusion Brightness XYZNNN </summary>
        private int AoBrightnessXYZNNN;

        /// <summary>
        /// Ambient occlusion Brightness XYNN </summary>
        private int AoBrightnessXYNN;

        /// <summary>
        /// Ambient occlusion Brightness XYZNNP </summary>
        private int AoBrightnessXYZNNP;

        /// <summary>
        /// Ambient occlusion Brightness YZNN </summary>
        private int AoBrightnessYZNN;

        /// <summary>
        /// Ambient occlusion Brightness YZNP </summary>
        private int AoBrightnessYZNP;

        /// <summary>
        /// Ambient occlusion Brightness XYZPNN </summary>
        private int AoBrightnessXYZPNN;

        /// <summary>
        /// Ambient occlusion Brightness XYPN </summary>
        private int AoBrightnessXYPN;

        /// <summary>
        /// Ambient occlusion Brightness XYZPNP </summary>
        private int AoBrightnessXYZPNP;

        /// <summary>
        /// Ambient occlusion Brightness XYZNPN </summary>
        private int AoBrightnessXYZNPN;

        /// <summary>
        /// Ambient occlusion Brightness XYNP </summary>
        private int AoBrightnessXYNP;

        /// <summary>
        /// Ambient occlusion Brightness XYZNPP </summary>
        private int AoBrightnessXYZNPP;

        /// <summary>
        /// Ambient occlusion Brightness YZPN </summary>
        private int AoBrightnessYZPN;

        /// <summary>
        /// Ambient occlusion Brightness XYZPPN </summary>
        private int AoBrightnessXYZPPN;

        /// <summary>
        /// Ambient occlusion Brightness XYPP </summary>
        private int AoBrightnessXYPP;

        /// <summary>
        /// Ambient occlusion Brightness YZPP </summary>
        private int AoBrightnessYZPP;

        /// <summary>
        /// Ambient occlusion Brightness XYZPPP </summary>
        private int AoBrightnessXYZPPP;

        /// <summary>
        /// Ambient occlusion Brightness XZNN </summary>
        private int AoBrightnessXZNN;

        /// <summary>
        /// Ambient occlusion Brightness XZPN </summary>
        private int AoBrightnessXZPN;

        /// <summary>
        /// Ambient occlusion Brightness XZNP </summary>
        private int AoBrightnessXZNP;

        /// <summary>
        /// Ambient occlusion Brightness XZPP </summary>
        private int AoBrightnessXZPP;

        /// <summary>
        /// Ambient occlusion type (0=simple, 1=complex) </summary>
        private int AoType;

        /// <summary>
        /// Brightness top left </summary>
        private int brightnessTopLeft;

        /// <summary>
        /// Brightness bottom left </summary>
        private int brightnessBottomLeft;

        /// <summary>
        /// Brightness bottom right </summary>
        private int brightnessBottomRight;

        /// <summary>
        /// Brightness top right </summary>
        private int brightnessTopRight;

        /// <summary>
        /// Red Color value for the top left corner </summary>
        private float ColorRedTopLeft;

        /// <summary>
        /// Red Color value for the bottom left corner </summary>
        private float ColorRedBottomLeft;

        /// <summary>
        /// Red Color value for the bottom right corner </summary>
        private float ColorRedBottomRight;

        /// <summary>
        /// Red Color value for the top right corner </summary>
        private float ColorRedTopRight;

        /// <summary>
        /// Green Color value for the top left corner </summary>
        private float ColorGreenTopLeft;

        /// <summary>
        /// Green Color value for the bottom left corner </summary>
        private float ColorGreenBottomLeft;

        /// <summary>
        /// Green Color value for the bottom right corner </summary>
        private float ColorGreenBottomRight;

        /// <summary>
        /// Green Color value for the top right corner </summary>
        private float ColorGreenTopRight;

        /// <summary>
        /// Blue Color value for the top left corner </summary>
        private float ColorBlueTopLeft;

        /// <summary>
        /// Blue Color value for the bottom left corner </summary>
        private float ColorBlueBottomLeft;

        /// <summary>
        /// Blue Color value for the bottom right corner </summary>
        private float ColorBlueBottomRight;

        /// <summary>
        /// Blue Color value for the top right corner </summary>
        private float ColorBlueTopRight;

        /// <summary>
        /// Grass flag for ambient occlusion on Center X, Positive Y, and Negative Z
        /// </summary>
        private bool AoGrassXYZCPN;

        /// <summary>
        /// Grass flag for ambient occlusion on Positive X, Positive Y, and Center Z
        /// </summary>
        private bool AoGrassXYZPPC;

        /// <summary>
        /// Grass flag for ambient occlusion on Negative X, Positive Y, and Center Z
        /// </summary>
        private bool AoGrassXYZNPC;

        /// <summary>
        /// Grass flag for ambient occlusion on Center X, Positive Y, and Positive Z
        /// </summary>
        private bool AoGrassXYZCPP;

        /// <summary>
        /// Grass flag for ambient occlusion on Negative X, Center Y, and Negative Z
        /// </summary>
        private bool AoGrassXYZNCN;

        /// <summary>
        /// Grass flag for ambient occlusion on Positive X, Center Y, and Positive Z
        /// </summary>
        private bool AoGrassXYZPCP;

        /// <summary>
        /// Grass flag for ambient occlusion on Negative X, Center Y, and Positive Z
        /// </summary>
        private bool AoGrassXYZNCP;

        /// <summary>
        /// Grass flag for ambient occlusion on Positive X, Center Y, and Negative Z
        /// </summary>
        private bool AoGrassXYZPCN;

        /// <summary>
        /// Grass flag for ambient occlusion on Center X, Negative Y, and Negative Z
        /// </summary>
        private bool AoGrassXYZCNN;

        /// <summary>
        /// Grass flag for ambient occlusion on Positive X, Negative Y, and Center Z
        /// </summary>
        private bool AoGrassXYZPNC;

        /// <summary>
        /// Grass flag for ambient occlusion on Negative X, Negative Y, and center Z
        /// </summary>
        private bool AoGrassXYZNNC;

        /// <summary>
        /// Grass flag for ambient occlusion on Center X, Negative Y, and Positive Z
        /// </summary>
        private bool AoGrassXYZCNP;

        public RenderBlocks(IBlockAccess par1IBlockAccess)
        {
            overrideBlockTexture = -1;
            flipTexture = false;
            renderAllFaces = false;
            UseInventoryTint = true;
            uvRotateEast = 0;
            uvRotateWest = 0;
            uvRotateSouth = 0;
            uvRotateNorth = 0;
            uvRotateTop = 0;
            uvRotateBottom = 0;
            AoType = 1;
            BlockAccess = par1IBlockAccess;
        }

        public RenderBlocks()
        {
            overrideBlockTexture = -1;
            flipTexture = false;
            renderAllFaces = false;
            UseInventoryTint = true;
            uvRotateEast = 0;
            uvRotateWest = 0;
            uvRotateSouth = 0;
            uvRotateNorth = 0;
            uvRotateTop = 0;
            uvRotateBottom = 0;
            AoType = 1;
        }

        /// <summary>
        /// Clear override block texture
        /// </summary>
        public virtual void ClearOverrideBlockTexture()
        {
            overrideBlockTexture = -1;
        }

        /// <summary>
        /// Renders a block using the given texture instead of the block's own default texture
        /// </summary>
        public virtual void RenderBlockUsingTexture(Block par1Block, int par2, int par3, int par4, int par5)
        {
            overrideBlockTexture = par5;
            RenderBlockByRenderType(par1Block, par2, par3, par4);
            overrideBlockTexture = -1;
        }

        /// <summary>
        /// Render all faces of a block
        /// </summary>
        public virtual void RenderBlockAllFaces(Block par1Block, int par2, int par3, int par4)
        {
            renderAllFaces = true;
            RenderBlockByRenderType(par1Block, par2, par3, par4);
            renderAllFaces = false;
        }

        /// <summary>
        /// Renders the block at the given coordinates using the block's rendering type
        /// </summary>
        public virtual bool RenderBlockByRenderType(Block par1Block, int par2, int par3, int par4)
        {
            int i = par1Block.GetRenderType();
            par1Block.SetBlockBoundsBasedOnState(BlockAccess, par2, par3, par4);

            if (i == 0)
            {
                return RenderStandardBlock(par1Block, par2, par3, par4);
            }

            if (i == 4)
            {
                return RenderBlockFluids(par1Block, par2, par3, par4);
            }

            if (i == 13)
            {
                return RenderBlockCactus(par1Block, par2, par3, par4);
            }

            if (i == 1)
            {
                return RenderCrossedSquares(par1Block, par2, par3, par4);
            }

            if (i == 19)
            {
                return RenderBlockStem(par1Block, par2, par3, par4);
            }

            if (i == 23)
            {
                return RenderBlockLilyPad(par1Block, par2, par3, par4);
            }

            if (i == 6)
            {
                return RenderBlockCrops(par1Block, par2, par3, par4);
            }

            if (i == 2)
            {
                return RenderBlockTorch(par1Block, par2, par3, par4);
            }

            if (i == 3)
            {
                return RenderBlockFire(par1Block, par2, par3, par4);
            }

            if (i == 5)
            {
                return RenderBlockRedstoneWire(par1Block, par2, par3, par4);
            }

            if (i == 8)
            {
                return RenderBlockLadder(par1Block, par2, par3, par4);
            }

            if (i == 7)
            {
                return RenderBlockDoor(par1Block, par2, par3, par4);
            }

            if (i == 9)
            {
                return RenderBlockMinecartTrack((BlockRail)par1Block, par2, par3, par4);
            }

            if (i == 10)
            {
                return RenderBlockStairs(par1Block, par2, par3, par4);
            }

            if (i == 27)
            {
                return RenderBlockDragonEgg((BlockDragonEgg)par1Block, par2, par3, par4);
            }

            if (i == 11)
            {
                return RenderBlockFence((BlockFence)par1Block, par2, par3, par4);
            }

            if (i == 12)
            {
                return RenderBlockLever(par1Block, par2, par3, par4);
            }

            if (i == 14)
            {
                return RenderBlockBed(par1Block, par2, par3, par4);
            }

            if (i == 15)
            {
                return RenderBlockRepeater(par1Block, par2, par3, par4);
            }

            if (i == 16)
            {
                return RenderPistonBase(par1Block, par2, par3, par4, false);
            }

            if (i == 17)
            {
                return RenderPistonExtension(par1Block, par2, par3, par4, true);
            }

            if (i == 18)
            {
                return RenderBlockPane((BlockPane)par1Block, par2, par3, par4);
            }

            if (i == 20)
            {
                return RenderBlockVine(par1Block, par2, par3, par4);
            }

            if (i == 21)
            {
                return RenderBlockFenceGate((BlockFenceGate)par1Block, par2, par3, par4);
            }

            if (i == 24)
            {
                return RenderBlockCauldron((BlockCauldron)par1Block, par2, par3, par4);
            }

            if (i == 25)
            {
                return RenderBlockBrewingStand((BlockBrewingStand)par1Block, par2, par3, par4);
            }

            if (i == 26)
            {
                return RenderBlockEndPortalFrame(par1Block, par2, par3, par4);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Render BlockEndPortalFrame
        /// </summary>
        private bool RenderBlockEndPortalFrame(Block par1Block, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = i & 3;

            if (j == 0)
            {
                uvRotateTop = 3;
            }
            else if (j == 3)
            {
                uvRotateTop = 1;
            }
            else if (j == 1)
            {
                uvRotateTop = 2;
            }

            if (!BlockEndPortalFrame.IsEnderEyeInserted(i))
            {
                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.8125F, 1.0F);
                RenderStandardBlock(par1Block, par2, par3, par4);
                par1Block.SetBlockBoundsForItemRender();
                uvRotateTop = 0;
                return true;
            }
            else
            {
                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.8125F, 1.0F);
                RenderStandardBlock(par1Block, par2, par3, par4);
                overrideBlockTexture = 174;
                par1Block.SetBlockBounds(0.25F, 0.8125F, 0.25F, 0.75F, 1.0F, 0.75F);
                RenderStandardBlock(par1Block, par2, par3, par4);
                ClearOverrideBlockTexture();
                par1Block.SetBlockBoundsForItemRender();
                uvRotateTop = 0;
                return true;
            }
        }

        /// <summary>
        /// render a bed at the given coordinates
        /// </summary>
        private bool RenderBlockBed(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = BlockBed.GetDirection(i);
            bool flag = BlockBed.IsBlockFootOfBed(i);
            float f = 0.5F;
            float f1 = 1.0F;
            float f2 = 0.8F;
            float f3 = 0.6F;
            float f4 = f1;
            float f5 = f1;
            float f6 = f1;
            float f7 = f;
            float f8 = f2;
            float f9 = f3;
            float f10 = f;
            float f11 = f2;
            float f12 = f3;
            float f13 = f;
            float f14 = f2;
            float f15 = f3;
            int k = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4);
            tessellator.SetBrightness(k);
            tessellator.SetColorOpaque_F(f7, f10, f13);
            int l = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 0);
            int i1 = (l & 0xf) << 4;
            int j1 = l & 0xf0;
            double d = (float)i1 / 256F;
            double d1 = ((double)(i1 + 16) - 0.01D) / 256D;
            double d2 = (float)j1 / 256F;
            double d3 = ((double)(j1 + 16) - 0.01D) / 256D;
            double d4 = (double)par2 + par1Block.MinX;
            double d5 = (double)par2 + par1Block.MaxX;
            double d6 = (double)par3 + par1Block.MinY + 0.1875D;
            double d7 = (double)par4 + par1Block.MinZ;
            double d8 = (double)par4 + par1Block.MaxZ;
            tessellator.AddVertexWithUV(d4, d6, d8, d, d3);
            tessellator.AddVertexWithUV(d4, d6, d7, d, d2);
            tessellator.AddVertexWithUV(d5, d6, d7, d1, d2);
            tessellator.AddVertexWithUV(d5, d6, d8, d1, d3);
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4));
            tessellator.SetColorOpaque_F(f4, f5, f6);
            l = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 1);
            i1 = (l & 0xf) << 4;
            j1 = l & 0xf0;
            d = (float)i1 / 256F;
            d1 = ((double)(i1 + 16) - 0.01D) / 256D;
            d2 = (float)j1 / 256F;
            d3 = ((double)(j1 + 16) - 0.01D) / 256D;
            d4 = d;
            d5 = d1;
            d6 = d2;
            d7 = d2;
            d8 = d;
            double d9 = d1;
            double d10 = d3;
            double d11 = d3;

            if (j == 0)
            {
                d5 = d;
                d6 = d3;
                d8 = d1;
                d11 = d2;
            }
            else if (j == 2)
            {
                d4 = d1;
                d7 = d3;
                d9 = d;
                d10 = d2;
            }
            else if (j == 3)
            {
                d4 = d1;
                d7 = d3;
                d9 = d;
                d10 = d2;
                d5 = d;
                d6 = d3;
                d8 = d1;
                d11 = d2;
            }

            double d12 = (double)par2 + par1Block.MinX;
            double d13 = (double)par2 + par1Block.MaxX;
            double d14 = (double)par3 + par1Block.MaxY;
            double d15 = (double)par4 + par1Block.MinZ;
            double d16 = (double)par4 + par1Block.MaxZ;
            tessellator.AddVertexWithUV(d13, d14, d16, d8, d10);
            tessellator.AddVertexWithUV(d13, d14, d15, d4, d6);
            tessellator.AddVertexWithUV(d12, d14, d15, d5, d7);
            tessellator.AddVertexWithUV(d12, d14, d16, d9, d11);
            l = Direction.HeadInvisibleFace[j];

            if (flag)
            {
                l = Direction.HeadInvisibleFace[Direction.FootInvisibleFaceRemap[j]];
            }

            i1 = 4;

            switch (j)
            {
                case 0:
                    i1 = 5;
                    break;

                case 3:
                    i1 = 2;
                    break;

                case 1:
                    i1 = 3;
                    break;
            }

            if (l != 2 && (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 - 1, 2)))
            {
                tessellator.SetBrightness(par1Block.MinZ <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1) : k);
                tessellator.SetColorOpaque_F(f8, f11, f14);
                flipTexture = i1 == 2;
                RenderEastFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 2));
            }

            if (l != 3 && (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 + 1, 3)))
            {
                tessellator.SetBrightness(par1Block.MaxZ >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1) : k);
                tessellator.SetColorOpaque_F(f8, f11, f14);
                flipTexture = i1 == 3;
                RenderWestFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3));
            }

            if (l != 4 && (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 - 1, par3, par4, 4)))
            {
                tessellator.SetBrightness(par1Block.MinZ <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4) : k);
                tessellator.SetColorOpaque_F(f9, f12, f15);
                flipTexture = i1 == 4;
                RenderNorthFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 4));
            }

            if (l != 5 && (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 + 1, par3, par4, 5)))
            {
                tessellator.SetBrightness(par1Block.MaxZ >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4) : k);
                tessellator.SetColorOpaque_F(f9, f12, f15);
                flipTexture = i1 == 5;
                RenderSouthFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 5));
            }

            flipTexture = false;
            return true;
        }

        /// <summary>
        /// Render BlockBrewingStand
        /// </summary>
        private bool RenderBlockBrewingStand(BlockBrewingStand par1BlockBrewingStand, int par2, int par3, int par4)
        {
            par1BlockBrewingStand.SetBlockBounds(0.4375F, 0.0F, 0.4375F, 0.5625F, 0.875F, 0.5625F);
            RenderStandardBlock(par1BlockBrewingStand, par2, par3, par4);
            overrideBlockTexture = 156;
            par1BlockBrewingStand.SetBlockBounds(0.5625F, 0.0F, 0.3125F, 0.9375F, 0.125F, 0.6875F);
            RenderStandardBlock(par1BlockBrewingStand, par2, par3, par4);
            par1BlockBrewingStand.SetBlockBounds(0.125F, 0.0F, 0.0625F, 0.5F, 0.125F, 0.4375F);
            RenderStandardBlock(par1BlockBrewingStand, par2, par3, par4);
            par1BlockBrewingStand.SetBlockBounds(0.125F, 0.0F, 0.5625F, 0.5F, 0.125F, 0.9375F);
            RenderStandardBlock(par1BlockBrewingStand, par2, par3, par4);
            ClearOverrideBlockTexture();
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1BlockBrewingStand.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            int i = par1BlockBrewingStand.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(i >> 16 & 0xff) / 255F;
            float f2 = (float)(i >> 8 & 0xff) / 255F;
            float f3 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
                float f5 = (f1 * 30F + f2 * 70F) / 100F;
                float f6 = (f1 * 30F + f3 * 70F) / 100F;
                f1 = f4;
                f2 = f5;
                f3 = f6;
            }

            tessellator.SetColorOpaque_F(f * f1, f * f2, f * f3);
            int j = par1BlockBrewingStand.GetBlockTextureFromSideAndMetadata(0, 0);

            if (overrideBlockTexture >= 0)
            {
                j = overrideBlockTexture;
            }

            int k = (j & 0xf) << 4;
            int l = j & 0xf0;
            double d = (float)l / 256F;
            double d1 = ((float)l + 15.99F) / 256F;
            int i1 = BlockAccess.GetBlockMetadata(par2, par3, par4);

            for (int j1 = 0; j1 < 3; j1++)
            {
                double d2 = ((double)j1 * Math.PI * 2D) / 3D + (Math.PI / 2D);
                double d3 = ((float)k + 8F) / 256F;
                double d4 = ((float)k + 15.99F) / 256F;

                if ((i1 & 1 << j1) != 0)
                {
                    d3 = ((float)k + 7.99F) / 256F;
                    d4 = ((float)k + 0.0F) / 256F;
                }

                double d5 = (double)par2 + 0.5D;
                double d6 = (double)par2 + 0.5D + (Math.Sin(d2) * 8D) / 16D;
                double d7 = (double)par4 + 0.5D;
                double d8 = (double)par4 + 0.5D + (Math.Cos(d2) * 8D) / 16D;
                tessellator.AddVertexWithUV(d5, par3 + 1, d7, d3, d);
                tessellator.AddVertexWithUV(d5, par3 + 0, d7, d3, d1);
                tessellator.AddVertexWithUV(d6, par3 + 0, d8, d4, d1);
                tessellator.AddVertexWithUV(d6, par3 + 1, d8, d4, d);
                tessellator.AddVertexWithUV(d6, par3 + 1, d8, d4, d);
                tessellator.AddVertexWithUV(d6, par3 + 0, d8, d4, d1);
                tessellator.AddVertexWithUV(d5, par3 + 0, d7, d3, d1);
                tessellator.AddVertexWithUV(d5, par3 + 1, d7, d3, d);
            }

            par1BlockBrewingStand.SetBlockBoundsForItemRender();
            return true;
        }

        /// <summary>
        /// Render block cauldron
        /// </summary>
        private bool RenderBlockCauldron(BlockCauldron par1BlockCauldron, int par2, int par3, int par4)
        {
            RenderStandardBlock(par1BlockCauldron, par2, par3, par4);
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1BlockCauldron.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            int i = par1BlockCauldron.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(i >> 16 & 0xff) / 255F;
            float f2 = (float)(i >> 8 & 0xff) / 255F;
            float f3 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
                float f5 = (f1 * 30F + f2 * 70F) / 100F;
                float f7 = (f1 * 30F + f3 * 70F) / 100F;
                f1 = f4;
                f2 = f5;
                f3 = f7;
            }

            tessellator.SetColorOpaque_F(f * f1, f * f2, f * f3);
            int c = 232;
            float f6 = 0.125F;
            RenderSouthFace(par1BlockCauldron, ((float)par2 - 1.0F) + f6, par3, par4, c);
            RenderNorthFace(par1BlockCauldron, ((float)par2 + 1.0F) - f6, par3, par4, c);
            RenderWestFace(par1BlockCauldron, par2, par3, ((float)par4 - 1.0F) + f6, c);
            RenderEastFace(par1BlockCauldron, par2, par3, ((float)par4 + 1.0F) - f6, c);
            int c1 = 213;
            RenderTopFace(par1BlockCauldron, par2, ((float)par3 - 1.0F) + 0.25F, par4, c1);
            RenderBottomFace(par1BlockCauldron, par2, ((float)par3 + 1.0F) - 0.75F, par4, c1);
            int j = BlockAccess.GetBlockMetadata(par2, par3, par4);

            if (j > 0)
            {
                int c2 = 315;

                if (j > 3)
                {
                    j = 3;
                }

                RenderTopFace(par1BlockCauldron, par2, ((float)par3 - 1.0F) + (6F + (float)j * 3F) / 16F, par4, c2);
            }

            return true;
        }

        ///<summary>
        /// Renders a torch block at the given coordinates
        ///</summary>
        public bool RenderBlockTorch(Block par1Block, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            tessellator.SetColorOpaque_F(1.0F, 1.0F, 1.0F);
            double d = 0.40000000596046448D;
            double d1 = 0.5D - d;
            double d2 = 0.20000000298023224D;

            if (i == 1)
            {
                RenderTorchAtAngle(par1Block, (double)par2 - d1, (double)par3 + d2, par4, -d, 0.0F);
            }
            else if (i == 2)
            {
                RenderTorchAtAngle(par1Block, (double)par2 + d1, (double)par3 + d2, par4, d, 0.0F);
            }
            else if (i == 3)
            {
                RenderTorchAtAngle(par1Block, par2, (double)par3 + d2, (double)par4 - d1, 0.0F, -d);
            }
            else if (i == 4)
            {
                RenderTorchAtAngle(par1Block, par2, (double)par3 + d2, (double)par4 + d1, 0.0F, d);
            }
            else
            {
                RenderTorchAtAngle(par1Block, par2, par3, par4, 0.0F, 0.0F);
            }

            return true;
        }

        ///<summary>
        /// render a redstone repeater at the given coordinates
        ///</summary>
        private bool RenderBlockRepeater(Block par1Block, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = i & 3;
            int k = (i & 0xc) >> 2;
            RenderStandardBlock(par1Block, par2, par3, par4);
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            tessellator.SetColorOpaque_F(1.0F, 1.0F, 1.0F);
            double d = -0.1875D;
            double d1 = 0.0F;
            double d2 = 0.0F;
            double d3 = 0.0F;
            double d4 = 0.0F;

            switch (j)
            {
                case 0:
                    d4 = -0.3125D;
                    d2 = BlockRedstoneRepeater.RepeaterTorchOffset[k];
                    break;

                case 2:
                    d4 = 0.3125D;
                    d2 = -BlockRedstoneRepeater.RepeaterTorchOffset[k];
                    break;

                case 3:
                    d3 = -0.3125D;
                    d1 = BlockRedstoneRepeater.RepeaterTorchOffset[k];
                    break;

                case 1:
                    d3 = 0.3125D;
                    d1 = -BlockRedstoneRepeater.RepeaterTorchOffset[k];
                    break;
            }

            RenderTorchAtAngle(par1Block, (double)par2 + d1, (double)par3 + d, (double)par4 + d2, 0.0F, 0.0F);
            RenderTorchAtAngle(par1Block, (double)par2 + d3, (double)par3 + d, (double)par4 + d4, 0.0F, 0.0F);
            int l = par1Block.GetBlockTextureFromSide(1);
            int i1 = (l & 0xf) << 4;
            int j1 = l & 0xf0;
            double d5 = (float)i1 / 256F;
            double d6 = ((float)i1 + 15.99F) / 256F;
            double d7 = (float)j1 / 256F;
            double d8 = ((float)j1 + 15.99F) / 256F;
            double d9 = 0.125D;
            double d10 = par2 + 1;
            double d11 = par2 + 1;
            double d12 = par2 + 0;
            double d13 = par2 + 0;
            double d14 = par4 + 0;
            double d15 = par4 + 1;
            double d16 = par4 + 1;
            double d17 = par4 + 0;
            double d18 = (double)par3 + d9;

            if (j == 2)
            {
                d10 = d11 = par2 + 0;
                d12 = d13 = par2 + 1;
                d14 = d17 = par4 + 1;
                d15 = d16 = par4 + 0;
            }
            else if (j == 3)
            {
                d10 = d13 = par2 + 0;
                d11 = d12 = par2 + 1;
                d14 = d15 = par4 + 0;
                d16 = d17 = par4 + 1;
            }
            else if (j == 1)
            {
                d10 = d13 = par2 + 1;
                d11 = d12 = par2 + 0;
                d14 = d15 = par4 + 1;
                d16 = d17 = par4 + 0;
            }

            tessellator.AddVertexWithUV(d13, d18, d17, d5, d7);
            tessellator.AddVertexWithUV(d12, d18, d16, d5, d8);
            tessellator.AddVertexWithUV(d11, d18, d15, d6, d8);
            tessellator.AddVertexWithUV(d10, d18, d14, d6, d7);
            return true;
        }

        ///<summary>
        /// Render all faces of the piston base
        ///</summary>
        public void RenderPistonBaseAllFaces(Block par1Block, int par2, int par3, int par4)
        {
            renderAllFaces = true;
            RenderPistonBase(par1Block, par2, par3, par4, true);
            renderAllFaces = false;
        }

        ///<summary>
        /// renders a block as a piston base
        ///</summary>
        private bool RenderPistonBase(Block par1Block, int par2, int par3, int par4, bool par5)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            bool flag = par5 || (i & 8) != 0;
            int j = BlockPistonBase.GetOrientation(i);

            if (flag)
            {
                switch (j)
                {
                    case 0:
                        uvRotateEast = 3;
                        uvRotateWest = 3;
                        uvRotateSouth = 3;
                        uvRotateNorth = 3;
                        par1Block.SetBlockBounds(0.0F, 0.25F, 0.0F, 1.0F, 1.0F, 1.0F);
                        break;

                    case 1:
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.75F, 1.0F);
                        break;

                    case 2:
                        uvRotateSouth = 1;
                        uvRotateNorth = 2;
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.25F, 1.0F, 1.0F, 1.0F);
                        break;

                    case 3:
                        uvRotateSouth = 2;
                        uvRotateNorth = 1;
                        uvRotateTop = 3;
                        uvRotateBottom = 3;
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.75F);
                        break;

                    case 4:
                        uvRotateEast = 1;
                        uvRotateWest = 2;
                        uvRotateTop = 2;
                        uvRotateBottom = 1;
                        par1Block.SetBlockBounds(0.25F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
                        break;

                    case 5:
                        uvRotateEast = 2;
                        uvRotateWest = 1;
                        uvRotateTop = 1;
                        uvRotateBottom = 2;
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 0.75F, 1.0F, 1.0F);
                        break;
                }

                RenderStandardBlock(par1Block, par2, par3, par4);
                uvRotateEast = 0;
                uvRotateWest = 0;
                uvRotateSouth = 0;
                uvRotateNorth = 0;
                uvRotateTop = 0;
                uvRotateBottom = 0;
                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            }
            else
            {
                switch (j)
                {
                    case 0:
                        uvRotateEast = 3;
                        uvRotateWest = 3;
                        uvRotateSouth = 3;
                        uvRotateNorth = 3;
                        break;

                    case 2:
                        uvRotateSouth = 1;
                        uvRotateNorth = 2;
                        break;

                    case 3:
                        uvRotateSouth = 2;
                        uvRotateNorth = 1;
                        uvRotateTop = 3;
                        uvRotateBottom = 3;
                        break;

                    case 4:
                        uvRotateEast = 1;
                        uvRotateWest = 2;
                        uvRotateTop = 2;
                        uvRotateBottom = 1;
                        break;

                    case 5:
                        uvRotateEast = 2;
                        uvRotateWest = 1;
                        uvRotateTop = 1;
                        uvRotateBottom = 2;
                        break;
                }

                RenderStandardBlock(par1Block, par2, par3, par4);
                uvRotateEast = 0;
                uvRotateWest = 0;
                uvRotateSouth = 0;
                uvRotateNorth = 0;
                uvRotateTop = 0;
                uvRotateBottom = 0;
            }

            return true;
        }

        ///<summary>
        /// Render piston rod up/down
        ///</summary>
        private void RenderPistonRodUD(double par1, double par3, double par5, double par7, double par9, double par11, float par13, double par14)
        {
            int i = 108;

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            Tessellator tessellator = Tessellator.Instance;
            double d = (float)(j + 0) / 256F;
            double d1 = (float)(k + 0) / 256F;
            double d2 = (((double)j + par14) - 0.01D) / 256D;
            double d3 = ((double)((float)k + 4F) - 0.01D) / 256D;
            tessellator.SetColorOpaque_F(par13, par13, par13);
            tessellator.AddVertexWithUV(par1, par7, par9, d2, d1);
            tessellator.AddVertexWithUV(par1, par5, par9, d, d1);
            tessellator.AddVertexWithUV(par3, par5, par11, d, d3);
            tessellator.AddVertexWithUV(par3, par7, par11, d2, d3);
        }

        ///<summary>
        /// Render piston rod south/north
        ///</summary>
        private void RenderPistonRodSN(double par1, double par3, double par5, double par7, double par9, double par11, float par13, double par14)
        {
            int i = 108;

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            Tessellator tessellator = Tessellator.Instance;
            double d = (float)(j + 0) / 256F;
            double d1 = (float)(k + 0) / 256F;
            double d2 = (((double)j + par14) - 0.01D) / 256D;
            double d3 = ((double)((float)k + 4F) - 0.01D) / 256D;
            tessellator.SetColorOpaque_F(par13, par13, par13);
            tessellator.AddVertexWithUV(par1, par5, par11, d2, d1);
            tessellator.AddVertexWithUV(par1, par5, par9, d, d1);
            tessellator.AddVertexWithUV(par3, par7, par9, d, d3);
            tessellator.AddVertexWithUV(par3, par7, par11, d2, d3);
        }

        ///<summary>
        /// Render piston rod east/west
        ///</summary>
        private void RenderPistonRodEW(double par1, double par3, double par5, double par7, double par9, double par11, float par13, double par14)
        {
            int i = 108;

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            Tessellator tessellator = Tessellator.Instance;
            double d = (float)(j + 0) / 256F;
            double d1 = (float)(k + 0) / 256F;
            double d2 = (((double)j + par14) - 0.01D) / 256D;
            double d3 = ((double)((float)k + 4F) - 0.01D) / 256D;
            tessellator.SetColorOpaque_F(par13, par13, par13);
            tessellator.AddVertexWithUV(par3, par5, par9, d2, d1);
            tessellator.AddVertexWithUV(par1, par5, par9, d, d1);
            tessellator.AddVertexWithUV(par1, par7, par11, d, d3);
            tessellator.AddVertexWithUV(par3, par7, par11, d2, d3);
        }

        ///<summary>
        /// Render all faces of the piston extension
        ///</summary>
        public void RenderPistonExtensionAllFaces(Block par1Block, int par2, int par3, int par4, bool par5)
        {
            renderAllFaces = true;
            RenderPistonExtension(par1Block, par2, par3, par4, par5);
            renderAllFaces = false;
        }

        ///<summary>
        /// renders the pushing part of a piston
        ///</summary>
        private bool RenderPistonExtension(Block par1Block, int par2, int par3, int par4, bool par5)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = BlockPistonExtension.GetDirectionMeta(i);
            float f = par1Block.GetBlockBrightness(BlockAccess, par2, par3, par4);
            float f1 = par5 ? 1.0F : 0.5F;
            double d = par5 ? 16D : 8D;

            switch (j)
            {
                case 0:
                    uvRotateEast = 3;
                    uvRotateWest = 3;
                    uvRotateSouth = 3;
                    uvRotateNorth = 3;
                    par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.25F, 1.0F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodUD((float)par2 + 0.375F, (float)par2 + 0.625F, (float)par3 + 0.25F, (float)par3 + 0.25F + f1, (float)par4 + 0.625F, (float)par4 + 0.625F, f * 0.8F, d);
                    RenderPistonRodUD((float)par2 + 0.625F, (float)par2 + 0.375F, (float)par3 + 0.25F, (float)par3 + 0.25F + f1, (float)par4 + 0.375F, (float)par4 + 0.375F, f * 0.8F, d);
                    RenderPistonRodUD((float)par2 + 0.375F, (float)par2 + 0.375F, (float)par3 + 0.25F, (float)par3 + 0.25F + f1, (float)par4 + 0.375F, (float)par4 + 0.625F, f * 0.6F, d);
                    RenderPistonRodUD((float)par2 + 0.625F, (float)par2 + 0.625F, (float)par3 + 0.25F, (float)par3 + 0.25F + f1, (float)par4 + 0.625F, (float)par4 + 0.375F, f * 0.6F, d);
                    break;

                case 1:
                    par1Block.SetBlockBounds(0.0F, 0.75F, 0.0F, 1.0F, 1.0F, 1.0F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodUD((float)par2 + 0.375F, (float)par2 + 0.625F, (((float)par3 - 0.25F) + 1.0F) - f1, ((float)par3 - 0.25F) + 1.0F, (float)par4 + 0.625F, (float)par4 + 0.625F, f * 0.8F, d);
                    RenderPistonRodUD((float)par2 + 0.625F, (float)par2 + 0.375F, (((float)par3 - 0.25F) + 1.0F) - f1, ((float)par3 - 0.25F) + 1.0F, (float)par4 + 0.375F, (float)par4 + 0.375F, f * 0.8F, d);
                    RenderPistonRodUD((float)par2 + 0.375F, (float)par2 + 0.375F, (((float)par3 - 0.25F) + 1.0F) - f1, ((float)par3 - 0.25F) + 1.0F, (float)par4 + 0.375F, (float)par4 + 0.625F, f * 0.6F, d);
                    RenderPistonRodUD((float)par2 + 0.625F, (float)par2 + 0.625F, (((float)par3 - 0.25F) + 1.0F) - f1, ((float)par3 - 0.25F) + 1.0F, (float)par4 + 0.625F, (float)par4 + 0.375F, f * 0.6F, d);
                    break;

                case 2:
                    uvRotateSouth = 1;
                    uvRotateNorth = 2;
                    par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.25F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodSN((float)par2 + 0.375F, (float)par2 + 0.375F, (float)par3 + 0.625F, (float)par3 + 0.375F, (float)par4 + 0.25F, (float)par4 + 0.25F + f1, f * 0.6F, d);
                    RenderPistonRodSN((float)par2 + 0.625F, (float)par2 + 0.625F, (float)par3 + 0.375F, (float)par3 + 0.625F, (float)par4 + 0.25F, (float)par4 + 0.25F + f1, f * 0.6F, d);
                    RenderPistonRodSN((float)par2 + 0.375F, (float)par2 + 0.625F, (float)par3 + 0.375F, (float)par3 + 0.375F, (float)par4 + 0.25F, (float)par4 + 0.25F + f1, f * 0.5F, d);
                    RenderPistonRodSN((float)par2 + 0.625F, (float)par2 + 0.375F, (float)par3 + 0.625F, (float)par3 + 0.625F, (float)par4 + 0.25F, (float)par4 + 0.25F + f1, f, d);
                    break;

                case 3:
                    uvRotateSouth = 2;
                    uvRotateNorth = 1;
                    uvRotateTop = 3;
                    uvRotateBottom = 3;
                    par1Block.SetBlockBounds(0.0F, 0.0F, 0.75F, 1.0F, 1.0F, 1.0F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodSN((float)par2 + 0.375F, (float)par2 + 0.375F, (float)par3 + 0.625F, (float)par3 + 0.375F, (((float)par4 - 0.25F) + 1.0F) - f1, ((float)par4 - 0.25F) + 1.0F, f * 0.6F, d);
                    RenderPistonRodSN((float)par2 + 0.625F, (float)par2 + 0.625F, (float)par3 + 0.375F, (float)par3 + 0.625F, (((float)par4 - 0.25F) + 1.0F) - f1, ((float)par4 - 0.25F) + 1.0F, f * 0.6F, d);
                    RenderPistonRodSN((float)par2 + 0.375F, (float)par2 + 0.625F, (float)par3 + 0.375F, (float)par3 + 0.375F, (((float)par4 - 0.25F) + 1.0F) - f1, ((float)par4 - 0.25F) + 1.0F, f * 0.5F, d);
                    RenderPistonRodSN((float)par2 + 0.625F, (float)par2 + 0.375F, (float)par3 + 0.625F, (float)par3 + 0.625F, (((float)par4 - 0.25F) + 1.0F) - f1, ((float)par4 - 0.25F) + 1.0F, f, d);
                    break;

                case 4:
                    uvRotateEast = 1;
                    uvRotateWest = 2;
                    uvRotateTop = 2;
                    uvRotateBottom = 1;
                    par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 0.25F, 1.0F, 1.0F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodEW((float)par2 + 0.25F, (float)par2 + 0.25F + f1, (float)par3 + 0.375F, (float)par3 + 0.375F, (float)par4 + 0.625F, (float)par4 + 0.375F, f * 0.5F, d);
                    RenderPistonRodEW((float)par2 + 0.25F, (float)par2 + 0.25F + f1, (float)par3 + 0.625F, (float)par3 + 0.625F, (float)par4 + 0.375F, (float)par4 + 0.625F, f, d);
                    RenderPistonRodEW((float)par2 + 0.25F, (float)par2 + 0.25F + f1, (float)par3 + 0.375F, (float)par3 + 0.625F, (float)par4 + 0.375F, (float)par4 + 0.375F, f * 0.6F, d);
                    RenderPistonRodEW((float)par2 + 0.25F, (float)par2 + 0.25F + f1, (float)par3 + 0.625F, (float)par3 + 0.375F, (float)par4 + 0.625F, (float)par4 + 0.625F, f * 0.6F, d);
                    break;

                case 5:
                    uvRotateEast = 2;
                    uvRotateWest = 1;
                    uvRotateTop = 1;
                    uvRotateBottom = 2;
                    par1Block.SetBlockBounds(0.75F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
                    RenderStandardBlock(par1Block, par2, par3, par4);
                    RenderPistonRodEW((((float)par2 - 0.25F) + 1.0F) - f1, ((float)par2 - 0.25F) + 1.0F, (float)par3 + 0.375F, (float)par3 + 0.375F, (float)par4 + 0.625F, (float)par4 + 0.375F, f * 0.5F, d);
                    RenderPistonRodEW((((float)par2 - 0.25F) + 1.0F) - f1, ((float)par2 - 0.25F) + 1.0F, (float)par3 + 0.625F, (float)par3 + 0.625F, (float)par4 + 0.375F, (float)par4 + 0.625F, f, d);
                    RenderPistonRodEW((((float)par2 - 0.25F) + 1.0F) - f1, ((float)par2 - 0.25F) + 1.0F, (float)par3 + 0.375F, (float)par3 + 0.625F, (float)par4 + 0.375F, (float)par4 + 0.375F, f * 0.6F, d);
                    RenderPistonRodEW((((float)par2 - 0.25F) + 1.0F) - f1, ((float)par2 - 0.25F) + 1.0F, (float)par3 + 0.625F, (float)par3 + 0.375F, (float)par4 + 0.625F, (float)par4 + 0.625F, f * 0.6F, d);
                    break;
            }

            uvRotateEast = 0;
            uvRotateWest = 0;
            uvRotateSouth = 0;
            uvRotateNorth = 0;
            uvRotateTop = 0;
            uvRotateBottom = 0;
            par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            return true;
        }

        ///<summary>
        /// Renders a lever block at the given coordinates
        ///</summary>
        public bool RenderBlockLever(Block par1Block, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = i & 7;
            bool flag = (i & 8) > 0;
            Tessellator tessellator = Tessellator.Instance;
            bool flag1 = overrideBlockTexture >= 0;

            if (!flag1)
            {
                overrideBlockTexture = Block.Cobblestone.BlockIndexInTexture;
            }

            float f = 0.25F;
            float f1 = 0.1875F;
            float f2 = 0.1875F;

            if (j == 5)
            {
                par1Block.SetBlockBounds(0.5F - f1, 0.0F, 0.5F - f, 0.5F + f1, f2, 0.5F + f);
            }
            else if (j == 6)
            {
                par1Block.SetBlockBounds(0.5F - f, 0.0F, 0.5F - f1, 0.5F + f, f2, 0.5F + f1);
            }
            else if (j == 4)
            {
                par1Block.SetBlockBounds(0.5F - f1, 0.5F - f, 1.0F - f2, 0.5F + f1, 0.5F + f, 1.0F);
            }
            else if (j == 3)
            {
                par1Block.SetBlockBounds(0.5F - f1, 0.5F - f, 0.0F, 0.5F + f1, 0.5F + f, f2);
            }
            else if (j == 2)
            {
                par1Block.SetBlockBounds(1.0F - f2, 0.5F - f, 0.5F - f1, 1.0F, 0.5F + f, 0.5F + f1);
            }
            else if (j == 1)
            {
                par1Block.SetBlockBounds(0.0F, 0.5F - f, 0.5F - f1, f2, 0.5F + f, 0.5F + f1);
            }

            RenderStandardBlock(par1Block, par2, par3, par4);

            if (!flag1)
            {
                overrideBlockTexture = -1;
            }

            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f3 = 1.0F;

            if (Block.LightValue[par1Block.BlockID] > 0)
            {
                f3 = 1.0F;
            }

            tessellator.SetColorOpaque_F(f3, f3, f3);
            int k = par1Block.GetBlockTextureFromSide(0);

            if (overrideBlockTexture >= 0)
            {
                k = overrideBlockTexture;
            }

            int l = (k & 0xf) << 4;
            int i1 = k & 0xf0;
            float f4 = (float)l / 256F;
            float f5 = ((float)l + 15.99F) / 256F;
            float f6 = (float)i1 / 256F;
            float f7 = ((float)i1 + 15.99F) / 256F;
            Vec3D[] avec3d = new Vec3D[8];
            float f8 = 0.0625F;
            float f9 = 0.0625F;
            float f10 = 0.625F;
            avec3d[0] = Vec3D.CreateVector(-f8, 0.0F, -f9);
            avec3d[1] = Vec3D.CreateVector(f8, 0.0F, -f9);
            avec3d[2] = Vec3D.CreateVector(f8, 0.0F, f9);
            avec3d[3] = Vec3D.CreateVector(-f8, 0.0F, f9);
            avec3d[4] = Vec3D.CreateVector(-f8, f10, -f9);
            avec3d[5] = Vec3D.CreateVector(f8, f10, -f9);
            avec3d[6] = Vec3D.CreateVector(f8, f10, f9);
            avec3d[7] = Vec3D.CreateVector(-f8, f10, f9);

            for (int j1 = 0; j1 < 8; j1++)
            {
                if (flag)
                {
                    avec3d[j1].ZCoord -= 0.0625D;
                    avec3d[j1].RotateAroundX(((float)Math.PI * 2F / 9F));
                }
                else
                {
                    avec3d[j1].ZCoord += 0.0625D;
                    avec3d[j1].RotateAroundX(-((float)Math.PI * 2F / 9F));
                }

                if (j == 6)
                {
                    avec3d[j1].RotateAroundY(((float)Math.PI / 2F));
                }

                if (j < 5)
                {
                    avec3d[j1].YCoord -= 0.375D;
                    avec3d[j1].RotateAroundX(((float)Math.PI / 2F));

                    if (j == 4)
                    {
                        avec3d[j1].RotateAroundY(0.0F);
                    }

                    if (j == 3)
                    {
                        avec3d[j1].RotateAroundY((float)Math.PI);
                    }

                    if (j == 2)
                    {
                        avec3d[j1].RotateAroundY(((float)Math.PI / 2F));
                    }

                    if (j == 1)
                    {
                        avec3d[j1].RotateAroundY(-((float)Math.PI / 2F));
                    }

                    avec3d[j1].XCoord += (double)par2 + 0.5D;
                    avec3d[j1].YCoord += (float)par3 + 0.5F;
                    avec3d[j1].ZCoord += (double)par4 + 0.5D;
                }
                else
                {
                    avec3d[j1].XCoord += (double)par2 + 0.5D;
                    avec3d[j1].YCoord += (float)par3 + 0.125F;
                    avec3d[j1].ZCoord += (double)par4 + 0.5D;
                }
            }

            Vec3D vec3d = null;
            Vec3D vec3d1 = null;
            Vec3D vec3d2 = null;
            Vec3D vec3d3 = null;

            for (int k1 = 0; k1 < 6; k1++)
            {
                if (k1 == 0)
                {
                    f4 = (float)(l + 7) / 256F;
                    f5 = ((float)(l + 9) - 0.01F) / 256F;
                    f6 = (float)(i1 + 6) / 256F;
                    f7 = ((float)(i1 + 8) - 0.01F) / 256F;
                }
                else if (k1 == 2)
                {
                    f4 = (float)(l + 7) / 256F;
                    f5 = ((float)(l + 9) - 0.01F) / 256F;
                    f6 = (float)(i1 + 6) / 256F;
                    f7 = ((float)(i1 + 16) - 0.01F) / 256F;
                }

                if (k1 == 0)
                {
                    vec3d = avec3d[0];
                    vec3d1 = avec3d[1];
                    vec3d2 = avec3d[2];
                    vec3d3 = avec3d[3];
                }
                else if (k1 == 1)
                {
                    vec3d = avec3d[7];
                    vec3d1 = avec3d[6];
                    vec3d2 = avec3d[5];
                    vec3d3 = avec3d[4];
                }
                else if (k1 == 2)
                {
                    vec3d = avec3d[1];
                    vec3d1 = avec3d[0];
                    vec3d2 = avec3d[4];
                    vec3d3 = avec3d[5];
                }
                else if (k1 == 3)
                {
                    vec3d = avec3d[2];
                    vec3d1 = avec3d[1];
                    vec3d2 = avec3d[5];
                    vec3d3 = avec3d[6];
                }
                else if (k1 == 4)
                {
                    vec3d = avec3d[3];
                    vec3d1 = avec3d[2];
                    vec3d2 = avec3d[6];
                    vec3d3 = avec3d[7];
                }
                else if (k1 == 5)
                {
                    vec3d = avec3d[0];
                    vec3d1 = avec3d[3];
                    vec3d2 = avec3d[7];
                    vec3d3 = avec3d[4];
                }

                tessellator.AddVertexWithUV(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, f4, f7);
                tessellator.AddVertexWithUV(vec3d1.XCoord, vec3d1.YCoord, vec3d1.ZCoord, f5, f7);
                tessellator.AddVertexWithUV(vec3d2.XCoord, vec3d2.YCoord, vec3d2.ZCoord, f5, f6);
                tessellator.AddVertexWithUV(vec3d3.XCoord, vec3d3.YCoord, vec3d3.ZCoord, f4, f6);
            }

            return true;
        }

        ///<summary>
        /// Renders a fire block at the given coordinates
        ///</summary>
        public bool RenderBlockFire(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSide(0);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            tessellator.SetColorOpaque_F(1.0F, 1.0F, 1.0F);
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            double d = (float)j / 256F;
            double d2 = ((float)j + 15.99F) / 256F;
            double d4 = (float)k / 256F;
            double d6 = ((float)k + 15.99F) / 256F;
            float f = 1.4F;

            if (BlockAccess.IsBlockNormalCube(par2, par3 - 1, par4) || Block.Fire.CanBlockCatchFire(BlockAccess, par2, par3 - 1, par4))
            {
                double d8 = (double)par2 + 0.5D + 0.20000000000000001D;
                double d9 = ((double)par2 + 0.5D) - 0.20000000000000001D;
                double d12 = (double)par4 + 0.5D + 0.20000000000000001D;
                double d14 = ((double)par4 + 0.5D) - 0.20000000000000001D;
                double d16 = ((double)par2 + 0.5D) - 0.29999999999999999D;
                double d18 = (double)par2 + 0.5D + 0.29999999999999999D;
                double d20 = ((double)par4 + 0.5D) - 0.29999999999999999D;
                double d22 = (double)par4 + 0.5D + 0.29999999999999999D;
                tessellator.AddVertexWithUV(d16, (float)par3 + f, par4 + 1, d2, d4);
                tessellator.AddVertexWithUV(d8, par3 + 0, par4 + 1, d2, d6);
                tessellator.AddVertexWithUV(d8, par3 + 0, par4 + 0, d, d6);
                tessellator.AddVertexWithUV(d16, (float)par3 + f, par4 + 0, d, d4);
                tessellator.AddVertexWithUV(d18, (float)par3 + f, par4 + 0, d2, d4);
                tessellator.AddVertexWithUV(d9, par3 + 0, par4 + 0, d2, d6);
                tessellator.AddVertexWithUV(d9, par3 + 0, par4 + 1, d, d6);
                tessellator.AddVertexWithUV(d18, (float)par3 + f, par4 + 1, d, d4);
                d = (float)j / 256F;
                d2 = ((float)j + 15.99F) / 256F;
                d4 = (float)(k + 16) / 256F;
                d6 = ((float)k + 15.99F + 16F) / 256F;
                tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f, d22, d2, d4);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d14, d2, d6);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d14, d, d6);
                tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f, d22, d, d4);
                tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f, d20, d2, d4);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d12, d2, d6);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d12, d, d6);
                tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f, d20, d, d4);
                d8 = ((double)par2 + 0.5D) - 0.5D;
                d9 = (double)par2 + 0.5D + 0.5D;
                d12 = ((double)par4 + 0.5D) - 0.5D;
                d14 = (double)par4 + 0.5D + 0.5D;
                d16 = ((double)par2 + 0.5D) - 0.40000000000000002D;
                d18 = (double)par2 + 0.5D + 0.40000000000000002D;
                d20 = ((double)par4 + 0.5D) - 0.40000000000000002D;
                d22 = (double)par4 + 0.5D + 0.40000000000000002D;
                tessellator.AddVertexWithUV(d16, (float)par3 + f, par4 + 0, d, d4);
                tessellator.AddVertexWithUV(d8, par3 + 0, par4 + 0, d, d6);
                tessellator.AddVertexWithUV(d8, par3 + 0, par4 + 1, d2, d6);
                tessellator.AddVertexWithUV(d16, (float)par3 + f, par4 + 1, d2, d4);
                tessellator.AddVertexWithUV(d18, (float)par3 + f, par4 + 1, d, d4);
                tessellator.AddVertexWithUV(d9, par3 + 0, par4 + 1, d, d6);
                tessellator.AddVertexWithUV(d9, par3 + 0, par4 + 0, d2, d6);
                tessellator.AddVertexWithUV(d18, (float)par3 + f, par4 + 0, d2, d4);
                d = (float)j / 256F;
                d2 = ((float)j + 15.99F) / 256F;
                d4 = (float)k / 256F;
                d6 = ((float)k + 15.99F) / 256F;
                tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f, d22, d, d4);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d14, d, d6);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d14, d2, d6);
                tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f, d22, d2, d4);
                tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f, d20, d, d4);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d12, d, d6);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d12, d2, d6);
                tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f, d20, d2, d4);
            }
            else
            {
                float f2 = 0.2F;
                float f3 = 0.0625F;

                if ((par2 + par3 + par4 & 1) == 1)
                {
                    d = (float)j / 256F;
                    d2 = ((float)j + 15.99F) / 256F;
                    d4 = (float)(k + 16) / 256F;
                    d6 = ((float)k + 15.99F + 16F) / 256F;
                }

                if ((par2 / 2 + par3 / 2 + par4 / 2 & 1) == 1)
                {
                    double d10 = d2;
                    d2 = d;
                    d = d10;
                }

                if (Block.Fire.CanBlockCatchFire(BlockAccess, par2 - 1, par3, par4))
                {
                    tessellator.AddVertexWithUV((float)par2 + f2, (float)par3 + f + f3, par4 + 1, d2, d4);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 1, d2, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV((float)par2 + f2, (float)par3 + f + f3, par4 + 0, d, d4);
                    tessellator.AddVertexWithUV((float)par2 + f2, (float)par3 + f + f3, par4 + 0, d, d4);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 1, d2, d6);
                    tessellator.AddVertexWithUV((float)par2 + f2, (float)par3 + f + f3, par4 + 1, d2, d4);
                }

                if (Block.Fire.CanBlockCatchFire(BlockAccess, par2 + 1, par3, par4))
                {
                    tessellator.AddVertexWithUV((float)(par2 + 1) - f2, (float)par3 + f + f3, par4 + 0, d, d4);
                    tessellator.AddVertexWithUV((par2 + 1) - 0, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV((par2 + 1) - 0, (float)(par3 + 0) + f3, par4 + 1, d2, d6);
                    tessellator.AddVertexWithUV((float)(par2 + 1) - f2, (float)par3 + f + f3, par4 + 1, d2, d4);
                    tessellator.AddVertexWithUV((float)(par2 + 1) - f2, (float)par3 + f + f3, par4 + 1, d2, d4);
                    tessellator.AddVertexWithUV((par2 + 1) - 0, (float)(par3 + 0) + f3, par4 + 1, d2, d6);
                    tessellator.AddVertexWithUV((par2 + 1) - 0, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV((float)(par2 + 1) - f2, (float)par3 + f + f3, par4 + 0, d, d4);
                }

                if (Block.Fire.CanBlockCatchFire(BlockAccess, par2, par3, par4 - 1))
                {
                    tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f + f3, (float)par4 + f2, d2, d4);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 0, d2, d6);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f + f3, (float)par4 + f2, d, d4);
                    tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f + f3, (float)par4 + f2, d, d4);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 0) + f3, par4 + 0, d, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, par4 + 0, d2, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f + f3, (float)par4 + f2, d2, d4);
                }

                if (Block.Fire.CanBlockCatchFire(BlockAccess, par2, par3, par4 + 1))
                {
                    tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f + f3, (float)(par4 + 1) - f2, d, d4);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 0) + f3, (par4 + 1) - 0, d, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, (par4 + 1) - 0, d2, d6);
                    tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f + f3, (float)(par4 + 1) - f2, d2, d4);
                    tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f + f3, (float)(par4 + 1) - f2, d2, d4);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 0) + f3, (par4 + 1) - 0, d2, d6);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 0) + f3, (par4 + 1) - 0, d, d6);
                    tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f + f3, (float)(par4 + 1) - f2, d, d4);
                }

                if (Block.Fire.CanBlockCatchFire(BlockAccess, par2, par3 + 1, par4))
                {
                    double d11 = (double)par2 + 0.5D + 0.5D;
                    double d13 = ((double)par2 + 0.5D) - 0.5D;
                    double d15 = (double)par4 + 0.5D + 0.5D;
                    double d17 = ((double)par4 + 0.5D) - 0.5D;
                    double d19 = ((double)par2 + 0.5D) - 0.5D;
                    double d21 = (double)par2 + 0.5D + 0.5D;
                    double d23 = ((double)par4 + 0.5D) - 0.5D;
                    double d24 = (double)par4 + 0.5D + 0.5D;
                    double d1 = (float)j / 256F;
                    double d3 = ((float)j + 15.99F) / 256F;
                    double d5 = (float)k / 256F;
                    double d7 = ((float)k + 15.99F) / 256F;
                    par3++;
                    float f1 = -0.2F;

                    if ((par2 + par3 + par4 & 1) == 0)
                    {
                        tessellator.AddVertexWithUV(d19, (float)par3 + f1, par4 + 0, d3, d5);
                        tessellator.AddVertexWithUV(d11, par3 + 0, par4 + 0, d3, d7);
                        tessellator.AddVertexWithUV(d11, par3 + 0, par4 + 1, d1, d7);
                        tessellator.AddVertexWithUV(d19, (float)par3 + f1, par4 + 1, d1, d5);
                        d1 = (float)j / 256F;
                        d3 = ((float)j + 15.99F) / 256F;
                        d5 = (float)(k + 16) / 256F;
                        d7 = ((float)k + 15.99F + 16F) / 256F;
                        tessellator.AddVertexWithUV(d21, (float)par3 + f1, par4 + 1, d3, d5);
                        tessellator.AddVertexWithUV(d13, par3 + 0, par4 + 1, d3, d7);
                        tessellator.AddVertexWithUV(d13, par3 + 0, par4 + 0, d1, d7);
                        tessellator.AddVertexWithUV(d21, (float)par3 + f1, par4 + 0, d1, d5);
                    }
                    else
                    {
                        tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f1, d24, d3, d5);
                        tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d17, d3, d7);
                        tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d17, d1, d7);
                        tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f1, d24, d1, d5);
                        d1 = (float)j / 256F;
                        d3 = ((float)j + 15.99F) / 256F;
                        d5 = (float)(k + 16) / 256F;
                        d7 = ((float)k + 15.99F + 16F) / 256F;
                        tessellator.AddVertexWithUV(par2 + 1, (float)par3 + f1, d23, d3, d5);
                        tessellator.AddVertexWithUV(par2 + 1, par3 + 0, d15, d3, d7);
                        tessellator.AddVertexWithUV(par2 + 0, par3 + 0, d15, d1, d7);
                        tessellator.AddVertexWithUV(par2 + 0, (float)par3 + f1, d23, d1, d5);
                    }
                }
            }

            return true;
        }

        ///<summary>
        /// Renders a redstone wire block at the given coordinates
        ///</summary>
        public bool RenderBlockRedstoneWire(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = par1Block.GetBlockTextureFromSideAndMetadata(1, i);

            if (overrideBlockTexture >= 0)
            {
                j = overrideBlockTexture;
            }

            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            float f1 = (float)i / 15F;
            float f2 = f1 * 0.6F + 0.4F;

            if (i == 0)
            {
                f2 = 0.3F;
            }

            float f3 = f1 * f1 * 0.7F - 0.5F;
            float f4 = f1 * f1 * 0.6F - 0.7F;

            if (f3 < 0.0F)
            {
                f3 = 0.0F;
            }

            if (f4 < 0.0F)
            {
                f4 = 0.0F;
            }

            tessellator.SetColorOpaque_F(f2, f3, f4);
            int k = (j & 0xf) << 4;
            int l = j & 0xf0;
            double d = (float)k / 256F;
            double d2 = ((float)k + 15.99F) / 256F;
            double d4 = (float)l / 256F;
            double d6 = ((float)l + 15.99F) / 256F;
            bool flag = BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 - 1, par3, par4, 1) || !BlockAccess.IsBlockNormalCube(par2 - 1, par3, par4) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 - 1, par3 - 1, par4, -1);
            bool flag1 = BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 + 1, par3, par4, 3) || !BlockAccess.IsBlockNormalCube(par2 + 1, par3, par4) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 + 1, par3 - 1, par4, -1);
            bool flag2 = BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3, par4 - 1, 2) || !BlockAccess.IsBlockNormalCube(par2, par3, par4 - 1) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3 - 1, par4 - 1, -1);
            bool flag3 = BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3, par4 + 1, 0) || !BlockAccess.IsBlockNormalCube(par2, par3, par4 + 1) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3 - 1, par4 + 1, -1);

            if (!BlockAccess.IsBlockNormalCube(par2, par3 + 1, par4))
            {
                if (BlockAccess.IsBlockNormalCube(par2 - 1, par3, par4) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 - 1, par3 + 1, par4, -1))
                {
                    flag = true;
                }

                if (BlockAccess.IsBlockNormalCube(par2 + 1, par3, par4) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2 + 1, par3 + 1, par4, -1))
                {
                    flag1 = true;
                }

                if (BlockAccess.IsBlockNormalCube(par2, par3, par4 - 1) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3 + 1, par4 - 1, -1))
                {
                    flag2 = true;
                }

                if (BlockAccess.IsBlockNormalCube(par2, par3, par4 + 1) && BlockRedstoneWire.IsPowerProviderOrWire(BlockAccess, par2, par3 + 1, par4 + 1, -1))
                {
                    flag3 = true;
                }
            }

            float f5 = par2 + 0;
            float f6 = par2 + 1;
            float f7 = par4 + 0;
            float f8 = par4 + 1;
            byte byte0 = 0;

            if ((flag || flag1) && !flag2 && !flag3)
            {
                byte0 = 1;
            }

            if ((flag2 || flag3) && !flag1 && !flag)
            {
                byte0 = 2;
            }

            if (byte0 != 0)
            {
                d = (float)(k + 16) / 256F;
                d2 = ((float)(k + 16) + 15.99F) / 256F;
                d4 = (float)l / 256F;
                d6 = ((float)l + 15.99F) / 256F;
            }

            if (byte0 == 0)
            {
                if (!flag)
                {
                    f5 += 0.3125F;
                }

                if (!flag)
                {
                    d += 0.01953125D;
                }

                if (!flag1)
                {
                    f6 -= 0.3125F;
                }

                if (!flag1)
                {
                    d2 -= 0.01953125D;
                }

                if (!flag2)
                {
                    f7 += 0.3125F;
                }

                if (!flag2)
                {
                    d4 += 0.01953125D;
                }

                if (!flag3)
                {
                    f8 -= 0.3125F;
                }

                if (!flag3)
                {
                    d6 -= 0.01953125D;
                }

                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d2, d4);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d, d6);
                tessellator.SetColorOpaque_F(f, f, f);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6 + 0.0625D);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d2, d4 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d, d6 + 0.0625D);
            }
            else if (byte0 == 1)
            {
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d2, d4);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d, d6);
                tessellator.SetColorOpaque_F(f, f, f);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6 + 0.0625D);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d2, d4 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d, d6 + 0.0625D);
            }
            else if (byte0 == 2)
            {
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d, d6);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d2, d4);
                tessellator.SetColorOpaque_F(f, f, f);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f8, d2, d6 + 0.0625D);
                tessellator.AddVertexWithUV(f6, (double)par3 + 0.015625D, f7, d, d6 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f7, d, d4 + 0.0625D);
                tessellator.AddVertexWithUV(f5, (double)par3 + 0.015625D, f8, d2, d4 + 0.0625D);
            }

            if (!BlockAccess.IsBlockNormalCube(par2, par3 + 1, par4))
            {
                double d1 = (float)(k + 16) / 256F;
                double d3 = ((float)(k + 16) + 15.99F) / 256F;
                double d5 = (float)l / 256F;
                double d7 = ((float)l + 15.99F) / 256F;

                if (BlockAccess.IsBlockNormalCube(par2 - 1, par3, par4) && BlockAccess.GetBlockId(par2 - 1, par3 + 1, par4) == Block.RedstoneWire.BlockID)
                {
                    tessellator.SetColorOpaque_F(f * f2, f * f3, f * f4);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 1, d3, d5);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, par3 + 0, par4 + 1, d1, d5);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, par3 + 0, par4 + 0, d1, d7);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 0, d3, d7);
                    tessellator.SetColorOpaque_F(f, f, f);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 1, d3, d5 + 0.0625D);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, par3 + 0, par4 + 1, d1, d5 + 0.0625D);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, par3 + 0, par4 + 0, d1, d7 + 0.0625D);
                    tessellator.AddVertexWithUV((double)par2 + 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 0, d3, d7 + 0.0625D);
                }

                if (BlockAccess.IsBlockNormalCube(par2 + 1, par3, par4) && BlockAccess.GetBlockId(par2 + 1, par3 + 1, par4) == Block.RedstoneWire.BlockID)
                {
                    tessellator.SetColorOpaque_F(f * f2, f * f3, f * f4);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, par3 + 0, par4 + 1, d1, d7);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 1, d3, d7);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 0, d3, d5);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, par3 + 0, par4 + 0, d1, d5);
                    tessellator.SetColorOpaque_F(f, f, f);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, par3 + 0, par4 + 1, d1, d7 + 0.0625D);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 1, d3, d7 + 0.0625D);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, (float)(par3 + 1) + 0.021875F, par4 + 0, d3, d5 + 0.0625D);
                    tessellator.AddVertexWithUV((double)(par2 + 1) - 0.015625D, par3 + 0, par4 + 0, d1, d5 + 0.0625D);
                }

                if (BlockAccess.IsBlockNormalCube(par2, par3, par4 - 1) && BlockAccess.GetBlockId(par2, par3 + 1, par4 - 1) == Block.RedstoneWire.BlockID)
                {
                    tessellator.SetColorOpaque_F(f * f2, f * f3, f * f4);
                    tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)par4 + 0.015625D, d1, d7);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 1) + 0.021875F, (double)par4 + 0.015625D, d3, d7);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 1) + 0.021875F, (double)par4 + 0.015625D, d3, d5);
                    tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)par4 + 0.015625D, d1, d5);
                    tessellator.SetColorOpaque_F(f, f, f);
                    tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)par4 + 0.015625D, d1, d7 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 1) + 0.021875F, (double)par4 + 0.015625D, d3, d7 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 1) + 0.021875F, (double)par4 + 0.015625D, d3, d5 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)par4 + 0.015625D, d1, d5 + 0.0625D);
                }

                if (BlockAccess.IsBlockNormalCube(par2, par3, par4 + 1) && BlockAccess.GetBlockId(par2, par3 + 1, par4 + 1) == Block.RedstoneWire.BlockID)
                {
                    tessellator.SetColorOpaque_F(f * f2, f * f3, f * f4);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 1) + 0.021875F, (double)(par4 + 1) - 0.015625D, d3, d5);
                    tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)(par4 + 1) - 0.015625D, d1, d5);
                    tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)(par4 + 1) - 0.015625D, d1, d7);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 1) + 0.021875F, (double)(par4 + 1) - 0.015625D, d3, d7);
                    tessellator.SetColorOpaque_F(f, f, f);
                    tessellator.AddVertexWithUV(par2 + 1, (float)(par3 + 1) + 0.021875F, (double)(par4 + 1) - 0.015625D, d3, d5 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)(par4 + 1) - 0.015625D, d1, d5 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)(par4 + 1) - 0.015625D, d1, d7 + 0.0625D);
                    tessellator.AddVertexWithUV(par2 + 0, (float)(par3 + 1) + 0.021875F, (double)(par4 + 1) - 0.015625D, d3, d7 + 0.0625D);
                }
            }

            return true;
        }

        ///<summary>
        /// Renders a minecart track block at the given coordinates
        ///</summary>
        public bool RenderBlockMinecartTrack(BlockRail par1BlockRail, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = par1BlockRail.GetBlockTextureFromSideAndMetadata(0, i);

            if (overrideBlockTexture >= 0)
            {
                j = overrideBlockTexture;
            }

            if (par1BlockRail.IsPowered())
            {
                i &= 7;
            }

            tessellator.SetBrightness(par1BlockRail.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            tessellator.SetColorOpaque_F(1.0F, 1.0F, 1.0F);
            int k = (j & 0xf) << 4;
            int l = j & 0xf0;
            double d = (float)k / 256F;
            double d1 = ((float)k + 15.99F) / 256F;
            double d2 = (float)l / 256F;
            double d3 = ((float)l + 15.99F) / 256F;
            double d4 = 0.0625D;
            double d5 = par2 + 1;
            double d6 = par2 + 1;
            double d7 = par2 + 0;
            double d8 = par2 + 0;
            double d9 = par4 + 0;
            double d10 = par4 + 1;
            double d11 = par4 + 1;
            double d12 = par4 + 0;
            double d13 = (double)par3 + d4;
            double d14 = (double)par3 + d4;
            double d15 = (double)par3 + d4;
            double d16 = (double)par3 + d4;

            if (i == 1 || i == 2 || i == 3 || i == 7)
            {
                d5 = d8 = par2 + 1;
                d6 = d7 = par2 + 0;
                d9 = d10 = par4 + 1;
                d11 = d12 = par4 + 0;
            }
            else if (i == 8)
            {
                d5 = d6 = par2 + 0;
                d7 = d8 = par2 + 1;
                d9 = d12 = par4 + 1;
                d10 = d11 = par4 + 0;
            }
            else if (i == 9)
            {
                d5 = d8 = par2 + 0;
                d6 = d7 = par2 + 1;
                d9 = d10 = par4 + 0;
                d11 = d12 = par4 + 1;
            }

            if (i == 2 || i == 4)
            {
                d13++;
                d16++;
            }
            else if (i == 3 || i == 5)
            {
                d14++;
                d15++;
            }

            tessellator.AddVertexWithUV(d5, d13, d9, d1, d2);
            tessellator.AddVertexWithUV(d6, d14, d10, d1, d3);
            tessellator.AddVertexWithUV(d7, d15, d11, d, d3);
            tessellator.AddVertexWithUV(d8, d16, d12, d, d2);
            tessellator.AddVertexWithUV(d8, d16, d12, d, d2);
            tessellator.AddVertexWithUV(d7, d15, d11, d, d3);
            tessellator.AddVertexWithUV(d6, d14, d10, d1, d3);
            tessellator.AddVertexWithUV(d5, d13, d9, d1, d2);
            return true;
        }

        ///<summary>
        /// Renders a ladder block at the given coordinates
        ///</summary>
        public bool RenderBlockLadder(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSide(0);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            tessellator.SetColorOpaque_F(f, f, f);
            f = (i & 0xf) << 4;
            int j = i & 0xf0;
            double d = (float)f / 256F;
            double d1 = ((float)f + 15.99F) / 256F;
            double d2 = (float)j / 256F;
            double d3 = ((float)j + 15.99F) / 256F;
            int k = BlockAccess.GetBlockMetadata(par2, par3, par4);
            double d4 = 0.0F;
            double d5 = 0.05000000074505806D;

            if (k == 5)
            {
                tessellator.AddVertexWithUV((double)par2 + d5, (double)(par3 + 1) + d4, (double)(par4 + 1) + d4, d, d2);
                tessellator.AddVertexWithUV((double)par2 + d5, (double)(par3 + 0) - d4, (double)(par4 + 1) + d4, d, d3);
                tessellator.AddVertexWithUV((double)par2 + d5, (double)(par3 + 0) - d4, (double)(par4 + 0) - d4, d1, d3);
                tessellator.AddVertexWithUV((double)par2 + d5, (double)(par3 + 1) + d4, (double)(par4 + 0) - d4, d1, d2);
            }

            if (k == 4)
            {
                tessellator.AddVertexWithUV((double)(par2 + 1) - d5, (double)(par3 + 0) - d4, (double)(par4 + 1) + d4, d1, d3);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d5, (double)(par3 + 1) + d4, (double)(par4 + 1) + d4, d1, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d5, (double)(par3 + 1) + d4, (double)(par4 + 0) - d4, d, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d5, (double)(par3 + 0) - d4, (double)(par4 + 0) - d4, d, d3);
            }

            if (k == 3)
            {
                tessellator.AddVertexWithUV((double)(par2 + 1) + d4, (double)(par3 + 0) - d4, (double)par4 + d5, d1, d3);
                tessellator.AddVertexWithUV((double)(par2 + 1) + d4, (double)(par3 + 1) + d4, (double)par4 + d5, d1, d2);
                tessellator.AddVertexWithUV((double)(par2 + 0) - d4, (double)(par3 + 1) + d4, (double)par4 + d5, d, d2);
                tessellator.AddVertexWithUV((double)(par2 + 0) - d4, (double)(par3 + 0) - d4, (double)par4 + d5, d, d3);
            }

            if (k == 2)
            {
                tessellator.AddVertexWithUV((double)(par2 + 1) + d4, (double)(par3 + 1) + d4, (double)(par4 + 1) - d5, d, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) + d4, (double)(par3 + 0) - d4, (double)(par4 + 1) - d5, d, d3);
                tessellator.AddVertexWithUV((double)(par2 + 0) - d4, (double)(par3 + 0) - d4, (double)(par4 + 1) - d5, d1, d3);
                tessellator.AddVertexWithUV((double)(par2 + 0) - d4, (double)(par3 + 1) + d4, (double)(par4 + 1) - d5, d1, d2);
            }

            return true;
        }

        ///<summary>
        /// Render block vine
        ///</summary>
        public bool RenderBlockVine(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSide(0);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            float f = 1.0F;
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            int j = par1Block.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(j >> 16 & 0xff) / 255F;
            float d = (float)(j >> 8 & 0xff) / 255F;
            float f2 = (float)(j & 0xff) / 255F;
            tessellator.SetColorOpaque_F(f * f1, f * d, f * f2);
            j = (i & 0xf) << 4;
            f1 = i & 0xf0;
            d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)f1 / 256F;
            double d3 = ((float)f1 + 15.99F) / 256F;
            double d4 = 0.05000000074505806D;
            int k = BlockAccess.GetBlockMetadata(par2, par3, par4);

            if ((k & 2) != 0)
            {
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 1, par4 + 1, d, d2);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 0, par4 + 1, d, d3);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 0, par4 + 0, d1, d3);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 1, par4 + 0, d1, d2);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 1, par4 + 0, d1, d2);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 0, par4 + 0, d1, d3);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 0, par4 + 1, d, d3);
                tessellator.AddVertexWithUV((double)par2 + d4, par3 + 1, par4 + 1, d, d2);
            }

            if ((k & 8) != 0)
            {
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 0, par4 + 1, d1, d3);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 1, par4 + 1, d1, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 1, par4 + 0, d, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 0, par4 + 0, d, d3);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 0, par4 + 0, d, d3);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 1, par4 + 0, d, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 1, par4 + 1, d1, d2);
                tessellator.AddVertexWithUV((double)(par2 + 1) - d4, par3 + 0, par4 + 1, d1, d3);
            }

            if ((k & 4) != 0)
            {
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)par4 + d4, d1, d3);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 1, (double)par4 + d4, d1, d2);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 1, (double)par4 + d4, d, d2);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)par4 + d4, d, d3);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)par4 + d4, d, d3);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 1, (double)par4 + d4, d, d2);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 1, (double)par4 + d4, d1, d2);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)par4 + d4, d1, d3);
            }

            if ((k & 1) != 0)
            {
                tessellator.AddVertexWithUV(par2 + 1, par3 + 1, (double)(par4 + 1) - d4, d, d2);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)(par4 + 1) - d4, d, d3);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)(par4 + 1) - d4, d1, d3);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 1, (double)(par4 + 1) - d4, d1, d2);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 1, (double)(par4 + 1) - d4, d1, d2);
                tessellator.AddVertexWithUV(par2 + 0, par3 + 0, (double)(par4 + 1) - d4, d1, d3);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 0, (double)(par4 + 1) - d4, d, d3);
                tessellator.AddVertexWithUV(par2 + 1, par3 + 1, (double)(par4 + 1) - d4, d, d2);
            }

            if (BlockAccess.IsBlockNormalCube(par2, par3 + 1, par4))
            {
                tessellator.AddVertexWithUV(par2 + 1, (double)(par3 + 1) - d4, par4 + 0, d, d2);
                tessellator.AddVertexWithUV(par2 + 1, (double)(par3 + 1) - d4, par4 + 1, d, d3);
                tessellator.AddVertexWithUV(par2 + 0, (double)(par3 + 1) - d4, par4 + 1, d1, d3);
                tessellator.AddVertexWithUV(par2 + 0, (double)(par3 + 1) - d4, par4 + 0, d1, d2);
            }

            return true;
        }

        public bool RenderBlockPane(BlockPane par1BlockPane, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetHeight();
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1BlockPane.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            int j = par1BlockPane.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(j >> 16 & 0xff) / 255F;
            float f2 = (float)(j >> 8 & 0xff) / 255F;
            float f3 = (float)(j & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
                float f5 = (f1 * 30F + f2 * 70F) / 100F;
                float f6 = (f1 * 30F + f3 * 70F) / 100F;
                f1 = f4;
                f2 = f5;
                f3 = f6;
            }

            tessellator.SetColorOpaque_F(f * f1, f * f2, f * f3);
            int k = 0;
            int l = 0;

            if (overrideBlockTexture >= 0)
            {
                k = overrideBlockTexture;
                l = overrideBlockTexture;
            }
            else
            {
                int i1 = BlockAccess.GetBlockMetadata(par2, par3, par4);
                k = par1BlockPane.GetBlockTextureFromSideAndMetadata(0, i1);
                l = par1BlockPane.GetSideTextureIndex();
            }

            int j1 = (k & 0xf) << 4;
            int k1 = k & 0xf0;
            double d = (float)j1 / 256F;
            double d1 = ((float)j1 + 7.99F) / 256F;
            double d2 = ((float)j1 + 15.99F) / 256F;
            double d3 = (float)k1 / 256F;
            double d4 = ((float)k1 + 15.99F) / 256F;
            int l1 = (l & 0xf) << 4;
            int i2 = l & 0xf0;
            double d5 = (float)(l1 + 7) / 256F;
            double d6 = ((float)l1 + 8.99F) / 256F;
            double d7 = (float)i2 / 256F;
            double d8 = (float)(i2 + 8) / 256F;
            double d9 = ((float)i2 + 15.99F) / 256F;
            double d10 = par2;
            double d11 = (double)par2 + 0.5D;
            double d12 = par2 + 1;
            double d13 = par4;
            double d14 = (double)par4 + 0.5D;
            double d15 = par4 + 1;
            double d16 = ((double)par2 + 0.5D) - 0.0625D;
            double d17 = (double)par2 + 0.5D + 0.0625D;
            double d18 = ((double)par4 + 0.5D) - 0.0625D;
            double d19 = (double)par4 + 0.5D + 0.0625D;
            bool flag = par1BlockPane.CanThisPaneConnectToThisBlockID(BlockAccess.GetBlockId(par2, par3, par4 - 1));
            bool flag1 = par1BlockPane.CanThisPaneConnectToThisBlockID(BlockAccess.GetBlockId(par2, par3, par4 + 1));
            bool flag2 = par1BlockPane.CanThisPaneConnectToThisBlockID(BlockAccess.GetBlockId(par2 - 1, par3, par4));
            bool flag3 = par1BlockPane.CanThisPaneConnectToThisBlockID(BlockAccess.GetBlockId(par2 + 1, par3, par4));
            bool flag4 = par1BlockPane.ShouldSideBeRendered(BlockAccess, par2, par3 + 1, par4, 1);
            bool flag5 = par1BlockPane.ShouldSideBeRendered(BlockAccess, par2, par3 - 1, par4, 0);

            if (flag2 && flag3 || !flag2 && !flag3 && !flag && !flag1)
            {
                tessellator.AddVertexWithUV(d10, par3 + 1, d14, d, d3);
                tessellator.AddVertexWithUV(d10, par3 + 0, d14, d, d4);
                tessellator.AddVertexWithUV(d12, par3 + 0, d14, d2, d4);
                tessellator.AddVertexWithUV(d12, par3 + 1, d14, d2, d3);
                tessellator.AddVertexWithUV(d12, par3 + 1, d14, d, d3);
                tessellator.AddVertexWithUV(d12, par3 + 0, d14, d, d4);
                tessellator.AddVertexWithUV(d10, par3 + 0, d14, d2, d4);
                tessellator.AddVertexWithUV(d10, par3 + 1, d14, d2, d3);

                if (flag4)
                {
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                }
                else
                {
                    if (par3 < i - 1 && BlockAccess.IsAirBlock(par2 - 1, par3 + 1, par4))
                    {
                        tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                        tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                        tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                    }

                    if (par3 < i - 1 && BlockAccess.IsAirBlock(par2 + 1, par3 + 1, par4))
                    {
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                        tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                        tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                    }
                }

                if (flag5)
                {
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d9);
                }
                else
                {
                    if (par3 > 1 && BlockAccess.IsAirBlock(par2 - 1, par3 - 1, par4))
                    {
                        tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d9);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d9);
                        tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d9);
                        tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d9);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d8);
                    }

                    if (par3 > 1 && BlockAccess.IsAirBlock(par2 + 1, par3 - 1, par4))
                    {
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d7);
                        tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d7);
                        tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d7);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d8);
                        tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d8);
                        tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d7);
                    }
                }
            }
            else if (flag2 && !flag3)
            {
                tessellator.AddVertexWithUV(d10, par3 + 1, d14, d, d3);
                tessellator.AddVertexWithUV(d10, par3 + 0, d14, d, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d1, d3);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d, d4);
                tessellator.AddVertexWithUV(d10, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d10, par3 + 1, d14, d1, d3);

                if (!flag1 && !flag)
                {
                    tessellator.AddVertexWithUV(d11, par3 + 1, d19, d5, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d19, d5, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d18, d6, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d18, d6, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d18, d5, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d18, d5, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d19, d6, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d19, d6, d7);
                }

                if (flag4 || par3 < i - 1 && BlockAccess.IsAirBlock(par2 - 1, par3 + 1, par4))
                {
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d10, (double)(par3 + 1) + 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                }

                if (flag5 || par3 > 1 && BlockAccess.IsAirBlock(par2 - 1, par3 - 1, par4))
                {
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d19, d6, d9);
                    tessellator.AddVertexWithUV(d10, (double)par3 - 0.01D, d18, d5, d9);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d8);
                }
            }
            else if (!flag2 && flag3)
            {
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d1, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d12, par3 + 0, d14, d2, d4);
                tessellator.AddVertexWithUV(d12, par3 + 1, d14, d2, d3);
                tessellator.AddVertexWithUV(d12, par3 + 1, d14, d1, d3);
                tessellator.AddVertexWithUV(d12, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d2, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d2, d3);

                if (!flag1 && !flag)
                {
                    tessellator.AddVertexWithUV(d11, par3 + 1, d18, d5, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d18, d5, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d19, d6, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d19, d6, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d19, d5, d7);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d19, d5, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 0, d18, d6, d9);
                    tessellator.AddVertexWithUV(d11, par3 + 1, d18, d6, d7);
                }

                if (flag4 || par3 < i - 1 && BlockAccess.IsAirBlock(par2 + 1, par3 + 1, par4))
                {
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d11, (double)(par3 + 1) + 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d12, (double)(par3 + 1) + 0.01D, d18, d5, d7);
                }

                if (flag5 || par3 > 1 && BlockAccess.IsAirBlock(par2 + 1, par3 - 1, par4))
                {
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d7);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d19, d6, d7);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d19, d6, d8);
                    tessellator.AddVertexWithUV(d11, (double)par3 - 0.01D, d18, d5, d8);
                    tessellator.AddVertexWithUV(d12, (double)par3 - 0.01D, d18, d5, d7);
                }
            }

            if (flag && flag1 || !flag2 && !flag3 && !flag && !flag1)
            {
                tessellator.AddVertexWithUV(d11, par3 + 1, d15, d, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d15, d, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d13, d2, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d13, d2, d3);
                tessellator.AddVertexWithUV(d11, par3 + 1, d13, d, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d13, d, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d15, d2, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d15, d2, d3);

                if (flag4)
                {
                    tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d13, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d13, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d13, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d13, d5, d9);
                }
                else
                {
                    if (par3 < i - 1 && BlockAccess.IsAirBlock(par2, par3 + 1, par4 - 1))
                    {
                        tessellator.AddVertexWithUV(d16, par3 + 1, d13, d6, d7);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d8);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d8);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d13, d5, d7);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d7);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d13, d6, d8);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d13, d5, d8);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d7);
                    }

                    if (par3 < i - 1 && BlockAccess.IsAirBlock(par2, par3 + 1, par4 + 1))
                    {
                        tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d8);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d9);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d9);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d8);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d8);
                        tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d9);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d9);
                        tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d8);
                    }
                }

                if (flag5)
                {
                    tessellator.AddVertexWithUV(d17, par3, d15, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3, d13, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3, d13, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3, d15, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3, d13, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3, d15, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3, d15, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3, d13, d5, d9);
                }
                else
                {
                    if (par3 > 1 && BlockAccess.IsAirBlock(par2, par3 - 1, par4 - 1))
                    {
                        tessellator.AddVertexWithUV(d16, par3, d13, d6, d7);
                        tessellator.AddVertexWithUV(d16, par3, d14, d6, d8);
                        tessellator.AddVertexWithUV(d17, par3, d14, d5, d8);
                        tessellator.AddVertexWithUV(d17, par3, d13, d5, d7);
                        tessellator.AddVertexWithUV(d16, par3, d14, d6, d7);
                        tessellator.AddVertexWithUV(d16, par3, d13, d6, d8);
                        tessellator.AddVertexWithUV(d17, par3, d13, d5, d8);
                        tessellator.AddVertexWithUV(d17, par3, d14, d5, d7);
                    }

                    if (par3 > 1 && BlockAccess.IsAirBlock(par2, par3 - 1, par4 + 1))
                    {
                        tessellator.AddVertexWithUV(d16, par3, d14, d5, d8);
                        tessellator.AddVertexWithUV(d16, par3, d15, d5, d9);
                        tessellator.AddVertexWithUV(d17, par3, d15, d6, d9);
                        tessellator.AddVertexWithUV(d17, par3, d14, d6, d8);
                        tessellator.AddVertexWithUV(d16, par3, d15, d5, d8);
                        tessellator.AddVertexWithUV(d16, par3, d14, d5, d9);
                        tessellator.AddVertexWithUV(d17, par3, d14, d6, d9);
                        tessellator.AddVertexWithUV(d17, par3, d15, d6, d8);
                    }
                }
            }
            else if (flag && !flag1)
            {
                tessellator.AddVertexWithUV(d11, par3 + 1, d13, d, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d13, d, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d1, d3);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d13, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d13, d1, d3);

                if (!flag3 && !flag2)
                {
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 0, d14, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 0, d14, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d7);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d7);
                    tessellator.AddVertexWithUV(d17, par3 + 0, d14, d5, d9);
                    tessellator.AddVertexWithUV(d16, par3 + 0, d14, d6, d9);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d7);
                }

                if (flag4 || par3 < i - 1 && BlockAccess.IsAirBlock(par2, par3 + 1, par4 - 1))
                {
                    tessellator.AddVertexWithUV(d16, par3 + 1, d13, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d8);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d8);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d13, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d13, d6, d8);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d13, d5, d8);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d7);
                }

                if (flag5 || par3 > 1 && BlockAccess.IsAirBlock(par2, par3 - 1, par4 - 1))
                {
                    tessellator.AddVertexWithUV(d16, par3, d13, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3, d14, d6, d8);
                    tessellator.AddVertexWithUV(d17, par3, d14, d5, d8);
                    tessellator.AddVertexWithUV(d17, par3, d13, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3, d14, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3, d13, d6, d8);
                    tessellator.AddVertexWithUV(d17, par3, d13, d5, d8);
                    tessellator.AddVertexWithUV(d17, par3, d14, d5, d7);
                }
            }
            else if (!flag && flag1)
            {
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d1, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d15, d2, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d15, d2, d3);
                tessellator.AddVertexWithUV(d11, par3 + 1, d15, d1, d3);
                tessellator.AddVertexWithUV(d11, par3 + 0, d15, d1, d4);
                tessellator.AddVertexWithUV(d11, par3 + 0, d14, d2, d4);
                tessellator.AddVertexWithUV(d11, par3 + 1, d14, d2, d3);

                if (!flag3 && !flag2)
                {
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d5, d7);
                    tessellator.AddVertexWithUV(d17, par3 + 0, d14, d5, d9);
                    tessellator.AddVertexWithUV(d16, par3 + 0, d14, d6, d9);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d6, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d7);
                    tessellator.AddVertexWithUV(d16, par3 + 0, d14, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 0, d14, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d7);
                }

                if (flag4 || par3 < i - 1 && BlockAccess.IsAirBlock(par2, par3 + 1, par4 + 1))
                {
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d8);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d8);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d15, d5, d8);
                    tessellator.AddVertexWithUV(d16, par3 + 1, d14, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d14, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3 + 1, d15, d6, d8);
                }

                if (flag5 || par3 > 1 && BlockAccess.IsAirBlock(par2, par3 - 1, par4 + 1))
                {
                    tessellator.AddVertexWithUV(d16, par3, d14, d5, d8);
                    tessellator.AddVertexWithUV(d16, par3, d15, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3, d15, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3, d14, d6, d8);
                    tessellator.AddVertexWithUV(d16, par3, d15, d5, d8);
                    tessellator.AddVertexWithUV(d16, par3, d14, d5, d9);
                    tessellator.AddVertexWithUV(d17, par3, d14, d6, d9);
                    tessellator.AddVertexWithUV(d17, par3, d15, d6, d8);
                }
            }

            return true;
        }

        ///<summary>
        /// Renders any block requiring croseed squares such as reeds, flowers, and mushrooms
        ///</summary>
        public bool RenderCrossedSquares(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            int i = par1Block.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(i >> 16 & 0xff) / 255F;
            float f2 = (float)(i >> 8 & 0xff) / 255F;
            float f3 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
                float f5 = (f1 * 30F + f2 * 70F) / 100F;
                float f6 = (f1 * 30F + f3 * 70F) / 100F;
                f1 = f4;
                f2 = f5;
                f3 = f6;
            }

            tessellator.SetColorOpaque_F(f * f1, f * f2, f * f3);
            double d = par2;
            double d1 = par3;
            double d2 = par4;

            if (par1Block == Block.TallGrass)
            {
                long l = (long)(par2 * 0x2fc20f) ^ (long)par4 * 0x6ebfff5L ^ (long)par3;
                l = l * l * 0x285b825L + l * 11L;
                d += ((double)((float)(l >> 16 & 15L) / 15F) - 0.5D) * 0.5D;
                d1 += ((double)((float)(l >> 20 & 15L) / 15F) - 1.0D) * 0.20000000000000001D;
                d2 += ((double)((float)(l >> 24 & 15L) / 15F) - 0.5D) * 0.5D;
            }

            DrawCrossedSquares(par1Block, BlockAccess.GetBlockMetadata(par2, par3, par4), d, d1, d2);
            return true;
        }

        ///<summary>
        /// Render block stem
        ///</summary>
        public bool RenderBlockStem(Block par1Block, int par2, int par3, int par4)
        {
            BlockStem blockstem = (BlockStem)par1Block;
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(blockstem.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f = 1.0F;
            int i = blockstem.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f1 = (float)(i >> 16 & 0xff) / 255F;
            float f2 = (float)(i >> 8 & 0xff) / 255F;
            float f3 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f4 = (f1 * 30F + f2 * 59F + f3 * 11F) / 100F;
                float f5 = (f1 * 30F + f2 * 70F) / 100F;
                float f6 = (f1 * 30F + f3 * 70F) / 100F;
                f1 = f4;
                f2 = f5;
                f3 = f6;
            }

            tessellator.SetColorOpaque_F(f * f1, f * f2, f * f3);
            blockstem.SetBlockBoundsBasedOnState(BlockAccess, par2, par3, par4);
            int j = blockstem.Func_35296_f(BlockAccess, par2, par3, par4);

            if (j < 0)
            {
                RenderBlockStemSmall(blockstem, BlockAccess.GetBlockMetadata(par2, par3, par4), blockstem.MaxY, par2, par3, par4);
            }
            else
            {
                RenderBlockStemSmall(blockstem, BlockAccess.GetBlockMetadata(par2, par3, par4), 0.5D, par2, par3, par4);
                RenderBlockStemBig(blockstem, BlockAccess.GetBlockMetadata(par2, par3, par4), j, blockstem.MaxY, par2, par3, par4);
            }

            return true;
        }

        ///<summary>
        /// Render block crops
        ///</summary>
        public bool RenderBlockCrops(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            tessellator.SetColorOpaque_F(1.0F, 1.0F, 1.0F);
            RenderBlockCropsImpl(par1Block, BlockAccess.GetBlockMetadata(par2, par3, par4), par2, (float)par3 - 0.0625F, par4);
            return true;
        }

        ///<summary>
        /// Renders a torch at the given coordinates, with the base slanting at the given delta
        ///</summary>
        public void RenderTorchAtAngle(Block par1Block, double par2, double par4, double par6, double par8, double par10)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSide(0);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            float f = (float)j / 256F;
            float f1 = ((float)j + 15.99F) / 256F;
            float f2 = (float)k / 256F;
            float f3 = ((float)k + 15.99F) / 256F;
            double d = (double)f + 0.02734375D;
            double d1 = (double)f2 + 0.0234375D;
            double d2 = (double)f + 0.03515625D;
            double d3 = (double)f2 + 0.03125D;
            par2 += 0.5D;
            par6 += 0.5D;
            double d4 = par2 - 0.5D;
            double d5 = par2 + 0.5D;
            double d6 = par6 - 0.5D;
            double d7 = par6 + 0.5D;
            double d8 = 0.0625D;
            double d9 = 0.625D;
            tessellator.AddVertexWithUV((par2 + par8 * (1.0D - d9)) - d8, par4 + d9, (par6 + par10 * (1.0D - d9)) - d8, d, d1);
            tessellator.AddVertexWithUV((par2 + par8 * (1.0D - d9)) - d8, par4 + d9, par6 + par10 * (1.0D - d9) + d8, d, d3);
            tessellator.AddVertexWithUV(par2 + par8 * (1.0D - d9) + d8, par4 + d9, par6 + par10 * (1.0D - d9) + d8, d2, d3);
            tessellator.AddVertexWithUV(par2 + par8 * (1.0D - d9) + d8, par4 + d9, (par6 + par10 * (1.0D - d9)) - d8, d2, d1);
            tessellator.AddVertexWithUV(par2 - d8, par4 + 1.0D, d6, f, f2);
            tessellator.AddVertexWithUV((par2 - d8) + par8, par4 + 0.0F, d6 + par10, f, f3);
            tessellator.AddVertexWithUV((par2 - d8) + par8, par4 + 0.0F, d7 + par10, f1, f3);
            tessellator.AddVertexWithUV(par2 - d8, par4 + 1.0D, d7, f1, f2);
            tessellator.AddVertexWithUV(par2 + d8, par4 + 1.0D, d7, f, f2);
            tessellator.AddVertexWithUV(par2 + par8 + d8, par4 + 0.0F, d7 + par10, f, f3);
            tessellator.AddVertexWithUV(par2 + par8 + d8, par4 + 0.0F, d6 + par10, f1, f3);
            tessellator.AddVertexWithUV(par2 + d8, par4 + 1.0D, d6, f1, f2);
            tessellator.AddVertexWithUV(d4, par4 + 1.0D, par6 + d8, f, f2);
            tessellator.AddVertexWithUV(d4 + par8, par4 + 0.0F, par6 + d8 + par10, f, f3);
            tessellator.AddVertexWithUV(d5 + par8, par4 + 0.0F, par6 + d8 + par10, f1, f3);
            tessellator.AddVertexWithUV(d5, par4 + 1.0D, par6 + d8, f1, f2);
            tessellator.AddVertexWithUV(d5, par4 + 1.0D, par6 - d8, f, f2);
            tessellator.AddVertexWithUV(d5 + par8, par4 + 0.0F, (par6 - d8) + par10, f, f3);
            tessellator.AddVertexWithUV(d4 + par8, par4 + 0.0F, (par6 - d8) + par10, f1, f3);
            tessellator.AddVertexWithUV(d4, par4 + 1.0D, par6 - d8, f1, f2);
        }

        ///<summary>
        /// Utility function to Draw crossed swuares
        ///</summary>
        public void DrawCrossedSquares(Block par1Block, int par2, double par3, double par5, double par7)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSideAndMetadata(0, par2);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            double d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)k / 256F;
            double d3 = ((float)k + 15.99F) / 256F;
            double d4 = (par3 + 0.5D) - 0.45000000000000001D;
            double d5 = par3 + 0.5D + 0.45000000000000001D;
            double d6 = (par7 + 0.5D) - 0.45000000000000001D;
            double d7 = par7 + 0.5D + 0.45000000000000001D;
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d1, d2);
        }

        ///<summary>
        /// Render block stem small
        ///</summary>
        public void RenderBlockStemSmall(Block par1Block, int par2, double par3, double par5, double par7, double par9)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSideAndMetadata(0, par2);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            double d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)k / 256F;
            double d3 = ((double)k + 15.989999771118164D * par3) / 256D;
            double d4 = (par5 + 0.5D) - 0.44999998807907104D;
            double d5 = par5 + 0.5D + 0.44999998807907104D;
            double d6 = (par9 + 0.5D) - 0.44999998807907104D;
            double d7 = par9 + 0.5D + 0.44999998807907104D;
            tessellator.AddVertexWithUV(d4, par7 + par3, d6, d, d2);
            tessellator.AddVertexWithUV(d4, par7 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d5, par7 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d5, par7 + par3, d7, d1, d2);
            tessellator.AddVertexWithUV(d5, par7 + par3, d7, d, d2);
            tessellator.AddVertexWithUV(d5, par7 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d4, par7 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d4, par7 + par3, d6, d1, d2);
            tessellator.AddVertexWithUV(d4, par7 + par3, d7, d, d2);
            tessellator.AddVertexWithUV(d4, par7 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d5, par7 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d5, par7 + par3, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par7 + par3, d6, d, d2);
            tessellator.AddVertexWithUV(d5, par7 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d4, par7 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d4, par7 + par3, d7, d1, d2);
        }

        ///<summary>
        /// Render BlockLilyPad
        ///</summary>
        public bool RenderBlockLilyPad(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.BlockIndexInTexture;

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            float f = 0.015625F;
            double d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)k / 256F;
            double d3 = ((float)k + 15.99F) / 256F;
            long l = (long)(par2 * 0x2fc20f) ^ (long)par4 * 0x6ebfff5L ^ (long)par3;
            l = l * l * 0x285b825L + l * 11L;
            int i1 = (int)(l >> 16 & 3L);
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
            float f1 = (float)par2 + 0.5F;
            float f2 = (float)par4 + 0.5F;
            float f3 = (float)(i1 & 1) * 0.5F * (float)(1 - ((i1 / 2) % 2) * 2);
            float f4 = (float)(i1 + 1 & 1) * 0.5F * (float)(1 - (((i1 + 1) / 2) % 2) * 2);
            tessellator.SetColorOpaque_I(par1Block.GetBlockColor());
            tessellator.AddVertexWithUV((f1 + f3) - f4, (float)par3 + f, f2 + f3 + f4, d, d2);
            tessellator.AddVertexWithUV(f1 + f3 + f4, (float)par3 + f, (f2 - f3) + f4, d1, d2);
            tessellator.AddVertexWithUV((f1 - f3) + f4, (float)par3 + f, f2 - f3 - f4, d1, d3);
            tessellator.AddVertexWithUV(f1 - f3 - f4, (float)par3 + f, (f2 + f3) - f4, d, d3);
            tessellator.SetColorOpaque_I((par1Block.GetBlockColor() & 0xfefefe) >> 1);
            tessellator.AddVertexWithUV(f1 - f3 - f4, (float)par3 + f, (f2 + f3) - f4, d, d3);
            tessellator.AddVertexWithUV((f1 - f3) + f4, (float)par3 + f, f2 - f3 - f4, d1, d3);
            tessellator.AddVertexWithUV(f1 + f3 + f4, (float)par3 + f, (f2 - f3) + f4, d1, d2);
            tessellator.AddVertexWithUV((f1 + f3) - f4, (float)par3 + f, f2 + f3 + f4, d, d2);
            return true;
        }

        ///<summary>
        /// Render block stem big
        ///</summary>
        public void RenderBlockStemBig(Block par1Block, int par2, int par3, double par4, double par6, double par8, double par10)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSideAndMetadata(0, par2) + 16;

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            double d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)k / 256F;
            double d3 = ((double)k + 15.989999771118164D * par4) / 256D;
            double d4 = (par6 + 0.5D) - 0.5D;
            double d5 = par6 + 0.5D + 0.5D;
            double d6 = (par10 + 0.5D) - 0.5D;
            double d7 = par10 + 0.5D + 0.5D;
            double d8 = par6 + 0.5D;
            double d9 = par10 + 0.5D;

            if (((par3 + 1) / 2) % 2 == 1)
            {
                double d10 = d1;
                d1 = d;
                d = d10;
            }

            if (par3 < 2)
            {
                tessellator.AddVertexWithUV(d4, par8 + par4, d9, d, d2);
                tessellator.AddVertexWithUV(d4, par8 + 0.0F, d9, d, d3);
                tessellator.AddVertexWithUV(d5, par8 + 0.0F, d9, d1, d3);
                tessellator.AddVertexWithUV(d5, par8 + par4, d9, d1, d2);
                tessellator.AddVertexWithUV(d5, par8 + par4, d9, d1, d2);
                tessellator.AddVertexWithUV(d5, par8 + 0.0F, d9, d1, d3);
                tessellator.AddVertexWithUV(d4, par8 + 0.0F, d9, d, d3);
                tessellator.AddVertexWithUV(d4, par8 + par4, d9, d, d2);
            }
            else
            {
                tessellator.AddVertexWithUV(d8, par8 + par4, d7, d, d2);
                tessellator.AddVertexWithUV(d8, par8 + 0.0F, d7, d, d3);
                tessellator.AddVertexWithUV(d8, par8 + 0.0F, d6, d1, d3);
                tessellator.AddVertexWithUV(d8, par8 + par4, d6, d1, d2);
                tessellator.AddVertexWithUV(d8, par8 + par4, d6, d1, d2);
                tessellator.AddVertexWithUV(d8, par8 + 0.0F, d6, d1, d3);
                tessellator.AddVertexWithUV(d8, par8 + 0.0F, d7, d, d3);
                tessellator.AddVertexWithUV(d8, par8 + par4, d7, d, d2);
            }
        }

        ///<summary>
        /// Render block crops implementation
        ///</summary>
        public void RenderBlockCropsImpl(Block par1Block, int par2, double par3, double par5, double par7)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.GetBlockTextureFromSideAndMetadata(0, par2);

            if (overrideBlockTexture >= 0)
            {
                i = overrideBlockTexture;
            }

            int j = (i & 0xf) << 4;
            int k = i & 0xf0;
            double d = (float)j / 256F;
            double d1 = ((float)j + 15.99F) / 256F;
            double d2 = (float)k / 256F;
            double d3 = ((float)k + 15.99F) / 256F;
            double d4 = (par3 + 0.5D) - 0.25D;
            double d5 = par3 + 0.5D + 0.25D;
            double d6 = (par7 + 0.5D) - 0.5D;
            double d7 = par7 + 0.5D + 0.5D;
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d1, d2);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d1, d2);
            d4 = (par3 + 0.5D) - 0.5D;
            d5 = par3 + 0.5D + 0.5D;
            d6 = (par7 + 0.5D) - 0.25D;
            d7 = par7 + 0.5D + 0.25D;
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d6, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d6, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d6, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d6, d1, d2);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d1, d2);
            tessellator.AddVertexWithUV(d4, par5 + 1.0D, d7, d, d2);
            tessellator.AddVertexWithUV(d4, par5 + 0.0F, d7, d, d3);
            tessellator.AddVertexWithUV(d5, par5 + 0.0F, d7, d1, d3);
            tessellator.AddVertexWithUV(d5, par5 + 1.0D, d7, d1, d2);
        }

        ///<summary>
        /// Renders a block based on the BlockFluids class at the given coordinates
        ///</summary>
        public bool RenderBlockFluids(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            int i = par1Block.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f = (float)(i >> 16 & 0xff) / 255F;
            float f1 = (float)(i >> 8 & 0xff) / 255F;
            float f2 = (float)(i & 0xff) / 255F;
            bool flag = par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 + 1, par4, 1);
            bool flag1 = par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 - 1, par4, 0);
            bool[] aflag = new bool[4];
            aflag[0] = par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 - 1, 2);
            aflag[1] = par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 + 1, 3);
            aflag[2] = par1Block.ShouldSideBeRendered(BlockAccess, par2 - 1, par3, par4, 4);
            aflag[3] = par1Block.ShouldSideBeRendered(BlockAccess, par2 + 1, par3, par4, 5);

            if (!flag && !flag1 && !aflag[0] && !aflag[1] && !aflag[2] && !aflag[3])
            {
                return false;
            }

            bool flag2 = false;
            float f3 = 0.5F;
            float f4 = 1.0F;
            float f5 = 0.8F;
            float f6 = 0.6F;
            float d = 0.0F;
            float d1 = 1.0F;
            Material material = par1Block.BlockMaterial;
            int j = BlockAccess.GetBlockMetadata(par2, par3, par4);
            double d2 = GetFluidHeight(par2, par3, par4, material);
            double d3 = GetFluidHeight(par2, par3, par4 + 1, material);
            double d4 = GetFluidHeight(par2 + 1, par3, par4 + 1, material);
            double d5 = GetFluidHeight(par2 + 1, par3, par4, material);
            double d6 = 0.0010000000474974513D;

            if (renderAllFaces || flag)
            {
                flag2 = true;
                int k = par1Block.GetBlockTextureFromSideAndMetadata(1, j);
                float f8 = (float)BlockFluid.Func_293_a(BlockAccess, par2, par3, par4, material);

                if (f8 > -999F)
                {
                    k = par1Block.GetBlockTextureFromSideAndMetadata(2, j);
                }

                d2 -= d6;
                d3 -= d6;
                d4 -= d6;
                d5 -= d6;
                int j1 = (k & 0xf) << 4;
                int l1 = k & 0xf0;
                double d7 = ((double)j1 + 8D) / 256D;
                double d8 = ((double)l1 + 8D) / 256D;

                if (f8 < -999F)
                {
                    f8 = 0.0F;
                }
                else
                {
                    d7 = (float)(j1 + 16) / 256F;
                    d8 = (float)(l1 + 16) / 256F;
                }

                double d10 = (double)(MathHelper2.Sin(f8) * 8F) / 256D;
                double d12 = (double)(MathHelper2.Cos(f8) * 8F) / 256D;
                tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4));
                float f9 = 1.0F;
                tessellator.SetColorOpaque_F(f4 * f9 * f, f4 * f9 * f1, f4 * f9 * f2);
                tessellator.AddVertexWithUV(par2 + 0, (double)par3 + d2, par4 + 0, d7 - d12 - d10, (d8 - d12) + d10);
                tessellator.AddVertexWithUV(par2 + 0, (double)par3 + d3, par4 + 1, (d7 - d12) + d10, d8 + d12 + d10);
                tessellator.AddVertexWithUV(par2 + 1, (double)par3 + d4, par4 + 1, d7 + d12 + d10, (d8 + d12) - d10);
                tessellator.AddVertexWithUV(par2 + 1, (double)par3 + d5, par4 + 0, (d7 + d12) - d10, d8 - d12 - d10);
            }

            if (renderAllFaces || flag1)
            {
                tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4));
                float f7 = 1.0F;
                tessellator.SetColorOpaque_F(f3 * f7, f3 * f7, f3 * f7);
                RenderBottomFace(par1Block, par2, (double)par3 + d6, par4, par1Block.GetBlockTextureFromSide(0));
                flag2 = true;
            }

            for (int l = 0; l < 4; l++)
            {
                int i1 = par2;
                int k1 = par3;
                int i2 = par4;

                if (l == 0)
                {
                    i2--;
                }

                if (l == 1)
                {
                    i2++;
                }

                if (l == 2)
                {
                    i1--;
                }

                if (l == 3)
                {
                    i1++;
                }

                int j2 = par1Block.GetBlockTextureFromSideAndMetadata(l + 2, j);
                int k2 = (j2 & 0xf) << 4;
                int l2 = j2 & 0xf0;

                if (!renderAllFaces && !aflag[l])
                {
                    continue;
                }

                double d9;
                double d11;
                double d13;
                double d14;
                double d15;
                double d16;

                if (l == 0)
                {
                    d9 = d2;
                    d11 = d5;
                    d13 = par2;
                    d15 = par2 + 1;
                    d14 = (double)par4 + d6;
                    d16 = (double)par4 + d6;
                }
                else if (l == 1)
                {
                    d9 = d4;
                    d11 = d3;
                    d13 = par2 + 1;
                    d15 = par2;
                    d14 = (double)(par4 + 1) - d6;
                    d16 = (double)(par4 + 1) - d6;
                }
                else if (l == 2)
                {
                    d9 = d3;
                    d11 = d2;
                    d13 = (double)par2 + d6;
                    d15 = (double)par2 + d6;
                    d14 = par4 + 1;
                    d16 = par4;
                }
                else
                {
                    d9 = d5;
                    d11 = d4;
                    d13 = (double)(par2 + 1) - d6;
                    d15 = (double)(par2 + 1) - d6;
                    d14 = par4;
                    d16 = par4 + 1;
                }

                flag2 = true;
                double d17 = (float)(k2 + 0) / 256F;
                double d18 = ((double)(k2 + 16) - 0.01D) / 256D;
                double d19 = ((double)l2 + (1.0D - d9) * 16D) / 256D;
                double d20 = ((double)l2 + (1.0D - d11) * 16D) / 256D;
                double d21 = ((double)(l2 + 16) - 0.01D) / 256D;
                tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(BlockAccess, i1, k1, i2));
                float f10 = 1.0F;

                if (l < 2)
                {
                    f10 *= f5;
                }
                else
                {
                    f10 *= f6;
                }

                tessellator.SetColorOpaque_F(f4 * f10 * f, f4 * f10 * f1, f4 * f10 * f2);
                tessellator.AddVertexWithUV(d13, (double)par3 + d9, d14, d17, d19);
                tessellator.AddVertexWithUV(d15, (double)par3 + d11, d16, d18, d20);
                tessellator.AddVertexWithUV(d15, par3 + 0, d16, d18, d21);
                tessellator.AddVertexWithUV(d13, par3 + 0, d14, d17, d21);
            }

            par1Block.MinY = d;
            par1Block.MaxY = d1;
            return flag2;
        }

        ///<summary>
        /// Get fluid height
        ///</summary>
        private float GetFluidHeight(int par1, int par2, int par3, Material par4Material)
        {
            int i = 0;
            float f = 0.0F;

            for (int j = 0; j < 4; j++)
            {
                int k = par1 - (j & 1);
                int l = par2;
                int i1 = par3 - (j >> 1 & 1);

                if (BlockAccess.GetBlockMaterial(k, l + 1, i1) == par4Material)
                {
                    return 1.0F;
                }

                Material material = BlockAccess.GetBlockMaterial(k, l, i1);

                if (material == par4Material)
                {
                    int j1 = BlockAccess.GetBlockMetadata(k, l, i1);

                    if (j1 >= 8 || j1 == 0)
                    {
                        f += BlockFluid.GetFluidHeightPercent(j1) * 10F;
                        i += 10;
                    }

                    f += BlockFluid.GetFluidHeightPercent(j1);
                    i++;
                    continue;
                }

                if (!material.IsSolid())
                {
                    f++;
                    i++;
                }
            }

            return 1.0F - f / (float)i;
        }

        public void RenderBlockFallingSand(Block par1Block, World par2World, int par3, int par4, int par5)
        {
            float f = 0.5F;
            float f1 = 1.0F;
            float f2 = 0.8F;
            float f3 = 0.6F;
            Tessellator tessellator = Tessellator.Instance;
            tessellator.StartDrawingQuads();
            tessellator.SetBrightness(par1Block.GetMixedBrightnessForBlock(par2World, par3, par4, par5));
            float f4 = 1.0F;
            float f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f * f5, f * f5, f * f5);
            RenderBottomFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(0));
            f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f1 * f5, f1 * f5, f1 * f5);
            RenderTopFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(1));
            f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f2 * f5, f2 * f5, f2 * f5);
            RenderEastFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(2));
            f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f2 * f5, f2 * f5, f2 * f5);
            RenderWestFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(3));
            f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f3 * f5, f3 * f5, f3 * f5);
            RenderNorthFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(4));
            f5 = 1.0F;

            if (f5 < f4)
            {
                f5 = f4;
            }

            tessellator.SetColorOpaque_F(f3 * f5, f3 * f5, f3 * f5);
            RenderSouthFace(par1Block, -0.5D, -0.5D, -0.5D, par1Block.GetBlockTextureFromSide(5));
            tessellator.Draw();
        }

        ///<summary>
        /// Renders a standard cube block at the given coordinates
        ///</summary>
        public bool RenderStandardBlock(Block par1Block, int par2, int par3, int par4)
        {
            int i = par1Block.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f = (float)(i >> 16 & 0xff) / 255F;
            float f1 = (float)(i >> 8 & 0xff) / 255F;
            float f2 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f3 = (f * 30F + f1 * 59F + f2 * 11F) / 100F;
                float f4 = (f * 30F + f1 * 70F) / 100F;
                float f5 = (f * 30F + f2 * 70F) / 100F;
                f = f3;
                f1 = f4;
                f2 = f5;
            }

            if (Minecraft.IsAmbientOcclusionEnabled() && Block.LightValue[par1Block.BlockID] == 0)
            {
                return RenderStandardBlockWithAmbientOcclusion(par1Block, par2, par3, par4, f, f1, f2);
            }
            else
            {
                return RenderStandardBlockWithColorMultiplier(par1Block, par2, par3, par4, f, f1, f2);
            }
        }

        public bool RenderStandardBlockWithAmbientOcclusion(Block par1Block, int par2, int par3, int par4, float par5, float par6, float par7)
        {
            enableAo = true;
            bool flag = false;
            float f = lightValueOwn;
            float f7 = lightValueOwn;
            float f14 = lightValueOwn;
            float f21 = lightValueOwn;
            bool flag1 = true;
            bool flag2 = true;
            bool flag3 = true;
            bool flag4 = true;
            bool flag5 = true;
            bool flag6 = true;
            lightValueOwn = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4);
            aoLightValueXNeg = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4);
            aoLightValueYNeg = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4);
            aoLightValueZNeg = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 - 1);
            aoLightValueXPos = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4);
            aoLightValueYPos = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4);
            aoLightValueZPos = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 + 1);
            int i = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4);
            int j = i;
            int k = i;
            int l = i;
            int i1 = i;
            int j1 = i;
            int k1 = i;

            if (par1Block.MinY <= 0.0F)
            {
                k = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4);
            }

            if (par1Block.MaxY >= 1.0D)
            {
                j1 = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4);
            }

            if (par1Block.MinX <= 0.0F)
            {
                j = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4);
            }

            if (par1Block.MaxX >= 1.0D)
            {
                i1 = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4);
            }

            if (par1Block.MinZ <= 0.0F)
            {
                l = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1);
            }

            if (par1Block.MaxZ >= 1.0D)
            {
                k1 = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1);
            }

            Tessellator tessellator = Tessellator.Instance;
            tessellator.SetBrightness(0xf000f);
            AoGrassXYZPPC = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 + 1, par3 + 1, par4)];
            AoGrassXYZPNC = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 + 1, par3 - 1, par4)];
            AoGrassXYZPCP = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 + 1, par3, par4 + 1)];
            AoGrassXYZPCN = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 + 1, par3, par4 - 1)];
            AoGrassXYZNPC = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 - 1, par3 + 1, par4)];
            AoGrassXYZNNC = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 - 1, par3 - 1, par4)];
            AoGrassXYZNCN = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 - 1, par3, par4 - 1)];
            AoGrassXYZNCP = Block.CanBlockGrass[BlockAccess.GetBlockId(par2 - 1, par3, par4 + 1)];
            AoGrassXYZCPP = Block.CanBlockGrass[BlockAccess.GetBlockId(par2, par3 + 1, par4 + 1)];
            AoGrassXYZCPN = Block.CanBlockGrass[BlockAccess.GetBlockId(par2, par3 + 1, par4 - 1)];
            AoGrassXYZCNP = Block.CanBlockGrass[BlockAccess.GetBlockId(par2, par3 - 1, par4 + 1)];
            AoGrassXYZCNN = Block.CanBlockGrass[BlockAccess.GetBlockId(par2, par3 - 1, par4 - 1)];

            if (par1Block.BlockIndexInTexture == 3)
            {
                flag1 = flag3 = flag4 = flag5 = flag6 = false;
            }

            if (overrideBlockTexture >= 0)
            {
                flag1 = flag3 = flag4 = flag5 = flag6 = false;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 - 1, par4, 0))
            {
                float f1;
                float f8;
                float f15;
                float f22;

                if (AoType > 0)
                {
                    if (par1Block.MinY <= 0.0F)
                    {
                        par3--;
                    }

                    AoBrightnessXYNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4);
                    AoBrightnessYZNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1);
                    AoBrightnessYZNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1);
                    AoBrightnessXYPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4);
                    aoLightValueScratchXYNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4);
                    aoLightValueScratchYZNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 - 1);
                    aoLightValueScratchYZNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 + 1);
                    aoLightValueScratchXYPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4);

                    if (AoGrassXYZCNN || AoGrassXYZNNC)
                    {
                        aoLightValueScratchXYZNNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4 - 1);
                        AoBrightnessXYZNNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNN = aoLightValueScratchXYNN;
                        AoBrightnessXYZNNN = AoBrightnessXYNN;
                    }

                    if (AoGrassXYZCNP || AoGrassXYZNNC)
                    {
                        aoLightValueScratchXYZNNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4 + 1);
                        AoBrightnessXYZNNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4 + 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNP = aoLightValueScratchXYNN;
                        AoBrightnessXYZNNP = AoBrightnessXYNN;
                    }

                    if (AoGrassXYZCNN || AoGrassXYZPNC)
                    {
                        aoLightValueScratchXYZPNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4 - 1);
                        AoBrightnessXYZPNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNN = aoLightValueScratchXYPN;
                        AoBrightnessXYZPNN = AoBrightnessXYPN;
                    }

                    if (AoGrassXYZCNP || AoGrassXYZPNC)
                    {
                        aoLightValueScratchXYZPNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4 + 1);
                        AoBrightnessXYZPNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4 + 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNP = aoLightValueScratchXYPN;
                        AoBrightnessXYZPNP = AoBrightnessXYPN;
                    }

                    if (par1Block.MinY <= 0.0F)
                    {
                        par3++;
                    }

                    f1 = (aoLightValueScratchXYZNNP + aoLightValueScratchXYNN + aoLightValueScratchYZNP + aoLightValueYNeg) / 4F;
                    f22 = (aoLightValueScratchYZNP + aoLightValueYNeg + aoLightValueScratchXYZPNP + aoLightValueScratchXYPN) / 4F;
                    f15 = (aoLightValueYNeg + aoLightValueScratchYZNN + aoLightValueScratchXYPN + aoLightValueScratchXYZPNN) / 4F;
                    f8 = (aoLightValueScratchXYNN + aoLightValueScratchXYZNNN + aoLightValueYNeg + aoLightValueScratchYZNN) / 4F;
                    brightnessTopLeft = GetAoBrightness(AoBrightnessXYZNNP, AoBrightnessXYNN, AoBrightnessYZNP, k);
                    brightnessTopRight = GetAoBrightness(AoBrightnessYZNP, AoBrightnessXYZPNP, AoBrightnessXYPN, k);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessYZNN, AoBrightnessXYPN, AoBrightnessXYZPNN, k);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessXYNN, AoBrightnessXYZNNN, AoBrightnessYZNN, k);
                }
                else
                {
                    f1 = f8 = f15 = f22 = aoLightValueYNeg;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = AoBrightnessXYNN;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = (flag1 ? par5 : 1.0F) * 0.5F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = (flag1 ? par6 : 1.0F) * 0.5F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = (flag1 ? par7 : 1.0F) * 0.5F;
                ColorRedTopLeft *= f1;
                ColorGreenTopLeft *= f1;
                ColorBlueTopLeft *= f1;
                ColorRedBottomLeft *= f8;
                ColorGreenBottomLeft *= f8;
                ColorBlueBottomLeft *= f8;
                ColorRedBottomRight *= f15;
                ColorGreenBottomRight *= f15;
                ColorBlueBottomRight *= f15;
                ColorRedTopRight *= f22;
                ColorGreenTopRight *= f22;
                ColorBlueTopRight *= f22;
                RenderBottomFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 0));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 + 1, par4, 1))
            {
                float f2;
                float f9;
                float f16;
                float f23;

                if (AoType > 0)
                {
                    if (par1Block.MaxY >= 1.0D)
                    {
                        par3++;
                    }

                    AoBrightnessXYNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4);
                    AoBrightnessXYPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4);
                    AoBrightnessYZPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1);
                    AoBrightnessYZPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1);
                    AoLightValueScratchXYNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4);
                    AoLightValueScratchXYPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4);
                    AoLightValueScratchYZPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 - 1);
                    AoLightValueScratchYZPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 + 1);

                    if (AoGrassXYZCPN || AoGrassXYZNPC)
                    {
                        aoLightValueScratchXYZNPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4 - 1);
                        AoBrightnessXYZNPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNPN = AoLightValueScratchXYNP;
                        AoBrightnessXYZNPN = AoBrightnessXYNP;
                    }

                    if (AoGrassXYZCPN || AoGrassXYZPPC)
                    {
                        AoLightValueScratchXYZPPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4 - 1);
                        AoBrightnessXYZPPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4 - 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPN = AoLightValueScratchXYPP;
                        AoBrightnessXYZPPN = AoBrightnessXYPP;
                    }

                    if (AoGrassXYZCPP || AoGrassXYZNPC)
                    {
                        AoLightValueScratchXYZNPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4 + 1);
                        AoBrightnessXYZNPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4 + 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZNPP = AoLightValueScratchXYNP;
                        AoBrightnessXYZNPP = AoBrightnessXYNP;
                    }

                    if (AoGrassXYZCPP || AoGrassXYZPPC)
                    {
                        AoLightValueScratchXYZPPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4 + 1);
                        AoBrightnessXYZPPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4 + 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPP = AoLightValueScratchXYPP;
                        AoBrightnessXYZPPP = AoBrightnessXYPP;
                    }

                    if (par1Block.MaxY >= 1.0D)
                    {
                        par3--;
                    }

                    f23 = (AoLightValueScratchXYZNPP + AoLightValueScratchXYNP + AoLightValueScratchYZPP + aoLightValueYPos) / 4F;
                    f2 = (AoLightValueScratchYZPP + aoLightValueYPos + AoLightValueScratchXYZPPP + AoLightValueScratchXYPP) / 4F;
                    f9 = (aoLightValueYPos + AoLightValueScratchYZPN + AoLightValueScratchXYPP + AoLightValueScratchXYZPPN) / 4F;
                    f16 = (AoLightValueScratchXYNP + aoLightValueScratchXYZNPN + aoLightValueYPos + AoLightValueScratchYZPN) / 4F;
                    brightnessTopRight = GetAoBrightness(AoBrightnessXYZNPP, AoBrightnessXYNP, AoBrightnessYZPP, j1);
                    brightnessTopLeft = GetAoBrightness(AoBrightnessYZPP, AoBrightnessXYZPPP, AoBrightnessXYPP, j1);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessYZPN, AoBrightnessXYPP, AoBrightnessXYZPPN, j1);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessXYNP, AoBrightnessXYZNPN, AoBrightnessYZPN, j1);
                }
                else
                {
                    f2 = f9 = f16 = f23 = aoLightValueYPos;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = j1;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = flag2 ? par5 : 1.0F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = flag2 ? par6 : 1.0F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = flag2 ? par7 : 1.0F;
                ColorRedTopLeft *= f2;
                ColorGreenTopLeft *= f2;
                ColorBlueTopLeft *= f2;
                ColorRedBottomLeft *= f9;
                ColorGreenBottomLeft *= f9;
                ColorBlueBottomLeft *= f9;
                ColorRedBottomRight *= f16;
                ColorGreenBottomRight *= f16;
                ColorBlueBottomRight *= f16;
                ColorRedTopRight *= f23;
                ColorGreenTopRight *= f23;
                ColorBlueTopRight *= f23;
                RenderTopFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 1));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 - 1, 2))
            {
                float f3;
                float f10;
                float f17;
                float f24;

                if (AoType > 0)
                {
                    if (par1Block.MinZ <= 0.0F)
                    {
                        par4--;
                    }

                    AoLightValueScratchXZNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4);
                    aoLightValueScratchYZNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4);
                    AoLightValueScratchYZPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4);
                    AoLightValueScratchXZPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4);
                    AoBrightnessXZNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4);
                    AoBrightnessYZNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4);
                    AoBrightnessYZPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4);
                    AoBrightnessXZPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4);

                    if (AoGrassXYZNCN || AoGrassXYZCNN)
                    {
                        aoLightValueScratchXYZNNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3 - 1, par4);
                        AoBrightnessXYZNNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3 - 1, par4);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNN = AoLightValueScratchXZNN;
                        AoBrightnessXYZNNN = AoBrightnessXZNN;
                    }

                    if (AoGrassXYZNCN || AoGrassXYZCPN)
                    {
                        aoLightValueScratchXYZNPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3 + 1, par4);
                        AoBrightnessXYZNPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3 + 1, par4);
                    }
                    else
                    {
                        aoLightValueScratchXYZNPN = AoLightValueScratchXZNN;
                        AoBrightnessXYZNPN = AoBrightnessXZNN;
                    }

                    if (AoGrassXYZPCN || AoGrassXYZCNN)
                    {
                        aoLightValueScratchXYZPNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3 - 1, par4);
                        AoBrightnessXYZPNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3 - 1, par4);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNN = AoLightValueScratchXZPN;
                        AoBrightnessXYZPNN = AoBrightnessXZPN;
                    }

                    if (AoGrassXYZPCN || AoGrassXYZCPN)
                    {
                        AoLightValueScratchXYZPPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3 + 1, par4);
                        AoBrightnessXYZPPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3 + 1, par4);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPN = AoLightValueScratchXZPN;
                        AoBrightnessXYZPPN = AoBrightnessXZPN;
                    }

                    if (par1Block.MinZ <= 0.0F)
                    {
                        par4++;
                    }

                    f3 = (AoLightValueScratchXZNN + aoLightValueScratchXYZNPN + aoLightValueZNeg + AoLightValueScratchYZPN) / 4F;
                    f10 = (aoLightValueZNeg + AoLightValueScratchYZPN + AoLightValueScratchXZPN + AoLightValueScratchXYZPPN) / 4F;
                    f17 = (aoLightValueScratchYZNN + aoLightValueZNeg + aoLightValueScratchXYZPNN + AoLightValueScratchXZPN) / 4F;
                    f24 = (aoLightValueScratchXYZNNN + AoLightValueScratchXZNN + aoLightValueScratchYZNN + aoLightValueZNeg) / 4F;
                    brightnessTopLeft = GetAoBrightness(AoBrightnessXZNN, AoBrightnessXYZNPN, AoBrightnessYZPN, l);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessYZPN, AoBrightnessXZPN, AoBrightnessXYZPPN, l);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessYZNN, AoBrightnessXYZPNN, AoBrightnessXZPN, l);
                    brightnessTopRight = GetAoBrightness(AoBrightnessXYZNNN, AoBrightnessXZNN, AoBrightnessYZNN, l);
                }
                else
                {
                    f3 = f10 = f17 = f24 = aoLightValueZNeg;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = l;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = (flag3 ? par5 : 1.0F) * 0.8F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = (flag3 ? par6 : 1.0F) * 0.8F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = (flag3 ? par7 : 1.0F) * 0.8F;
                ColorRedTopLeft *= f3;
                ColorGreenTopLeft *= f3;
                ColorBlueTopLeft *= f3;
                ColorRedBottomLeft *= f10;
                ColorGreenBottomLeft *= f10;
                ColorBlueBottomLeft *= f10;
                ColorRedBottomRight *= f17;
                ColorGreenBottomRight *= f17;
                ColorBlueBottomRight *= f17;
                ColorRedTopRight *= f24;
                ColorGreenTopRight *= f24;
                ColorBlueTopRight *= f24;
                int l1 = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 2);
                RenderEastFace(par1Block, par2, par3, par4, l1);

                if (FancyGrass && l1 == 3 && overrideBlockTexture < 0)
                {
                    ColorRedTopLeft *= par5;
                    ColorRedBottomLeft *= par5;
                    ColorRedBottomRight *= par5;
                    ColorRedTopRight *= par5;
                    ColorGreenTopLeft *= par6;
                    ColorGreenBottomLeft *= par6;
                    ColorGreenBottomRight *= par6;
                    ColorGreenTopRight *= par6;
                    ColorBlueTopLeft *= par7;
                    ColorBlueBottomLeft *= par7;
                    ColorBlueBottomRight *= par7;
                    ColorBlueTopRight *= par7;
                    RenderEastFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 + 1, 3))
            {
                float f4;
                float f11;
                float f18;
                float f25;

                if (AoType > 0)
                {
                    if (par1Block.MaxZ >= 1.0D)
                    {
                        par4++;
                    }

                    AoLightValueScratchXZNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3, par4);
                    AoLightValueScratchXZPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3, par4);
                    aoLightValueScratchYZNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4);
                    AoLightValueScratchYZPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4);
                    AoBrightnessXZNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4);
                    AoBrightnessXZPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4);
                    AoBrightnessYZNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4);
                    AoBrightnessYZPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4);

                    if (AoGrassXYZNCP || AoGrassXYZCNP)
                    {
                        aoLightValueScratchXYZNNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3 - 1, par4);
                        AoBrightnessXYZNNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3 - 1, par4);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNP = AoLightValueScratchXZNP;
                        AoBrightnessXYZNNP = AoBrightnessXZNP;
                    }

                    if (AoGrassXYZNCP || AoGrassXYZCPP)
                    {
                        AoLightValueScratchXYZNPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 - 1, par3 + 1, par4);
                        AoBrightnessXYZNPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3 + 1, par4);
                    }
                    else
                    {
                        AoLightValueScratchXYZNPP = AoLightValueScratchXZNP;
                        AoBrightnessXYZNPP = AoBrightnessXZNP;
                    }

                    if (AoGrassXYZPCP || AoGrassXYZCNP)
                    {
                        aoLightValueScratchXYZPNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3 - 1, par4);
                        AoBrightnessXYZPNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3 - 1, par4);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNP = AoLightValueScratchXZPP;
                        AoBrightnessXYZPNP = AoBrightnessXZPP;
                    }

                    if (AoGrassXYZPCP || AoGrassXYZCPP)
                    {
                        AoLightValueScratchXYZPPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2 + 1, par3 + 1, par4);
                        AoBrightnessXYZPPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3 + 1, par4);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPP = AoLightValueScratchXZPP;
                        AoBrightnessXYZPPP = AoBrightnessXZPP;
                    }

                    if (par1Block.MaxZ >= 1.0D)
                    {
                        par4--;
                    }

                    f4 = (AoLightValueScratchXZNP + AoLightValueScratchXYZNPP + aoLightValueZPos + AoLightValueScratchYZPP) / 4F;
                    f25 = (aoLightValueZPos + AoLightValueScratchYZPP + AoLightValueScratchXZPP + AoLightValueScratchXYZPPP) / 4F;
                    f18 = (aoLightValueScratchYZNP + aoLightValueZPos + aoLightValueScratchXYZPNP + AoLightValueScratchXZPP) / 4F;
                    f11 = (aoLightValueScratchXYZNNP + AoLightValueScratchXZNP + aoLightValueScratchYZNP + aoLightValueZPos) / 4F;
                    brightnessTopLeft = GetAoBrightness(AoBrightnessXZNP, AoBrightnessXYZNPP, AoBrightnessYZPP, k1);
                    brightnessTopRight = GetAoBrightness(AoBrightnessYZPP, AoBrightnessXZPP, AoBrightnessXYZPPP, k1);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessYZNP, AoBrightnessXYZPNP, AoBrightnessXZPP, k1);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessXYZNNP, AoBrightnessXZNP, AoBrightnessYZNP, k1);
                }
                else
                {
                    f4 = f11 = f18 = f25 = aoLightValueZPos;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = k1;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = (flag4 ? par5 : 1.0F) * 0.8F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = (flag4 ? par6 : 1.0F) * 0.8F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = (flag4 ? par7 : 1.0F) * 0.8F;
                ColorRedTopLeft *= f4;
                ColorGreenTopLeft *= f4;
                ColorBlueTopLeft *= f4;
                ColorRedBottomLeft *= f11;
                ColorGreenBottomLeft *= f11;
                ColorBlueBottomLeft *= f11;
                ColorRedBottomRight *= f18;
                ColorGreenBottomRight *= f18;
                ColorBlueBottomRight *= f18;
                ColorRedTopRight *= f25;
                ColorGreenTopRight *= f25;
                ColorBlueTopRight *= f25;
                int i2 = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3);
                RenderWestFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3));

                if (FancyGrass && i2 == 3 && overrideBlockTexture < 0)
                {
                    ColorRedTopLeft *= par5;
                    ColorRedBottomLeft *= par5;
                    ColorRedBottomRight *= par5;
                    ColorRedTopRight *= par5;
                    ColorGreenTopLeft *= par6;
                    ColorGreenBottomLeft *= par6;
                    ColorGreenBottomRight *= par6;
                    ColorGreenTopRight *= par6;
                    ColorBlueTopLeft *= par7;
                    ColorBlueBottomLeft *= par7;
                    ColorBlueBottomRight *= par7;
                    ColorBlueTopRight *= par7;
                    RenderWestFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 - 1, par3, par4, 4))
            {
                float f5;
                float f12;
                float f19;
                float f26;

                if (AoType > 0)
                {
                    if (par1Block.MinX <= 0.0F)
                    {
                        par2--;
                    }

                    aoLightValueScratchXYNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4);
                    AoLightValueScratchXZNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 - 1);
                    AoLightValueScratchXZNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 + 1);
                    AoLightValueScratchXYNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4);
                    AoBrightnessXYNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4);
                    AoBrightnessXZNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1);
                    AoBrightnessXZNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1);
                    AoBrightnessXYNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4);

                    if (AoGrassXYZNCN || AoGrassXYZNNC)
                    {
                        aoLightValueScratchXYZNNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4 - 1);
                        AoBrightnessXYZNNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNN = AoLightValueScratchXZNN;
                        AoBrightnessXYZNNN = AoBrightnessXZNN;
                    }

                    if (AoGrassXYZNCP || AoGrassXYZNNC)
                    {
                        aoLightValueScratchXYZNNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4 + 1);
                        AoBrightnessXYZNNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4 + 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNNP = AoLightValueScratchXZNP;
                        AoBrightnessXYZNNP = AoBrightnessXZNP;
                    }

                    if (AoGrassXYZNCN || AoGrassXYZNPC)
                    {
                        aoLightValueScratchXYZNPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4 - 1);
                        AoBrightnessXYZNPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZNPN = AoLightValueScratchXZNN;
                        AoBrightnessXYZNPN = AoBrightnessXZNN;
                    }

                    if (AoGrassXYZNCP || AoGrassXYZNPC)
                    {
                        AoLightValueScratchXYZNPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4 + 1);
                        AoBrightnessXYZNPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4 + 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZNPP = AoLightValueScratchXZNP;
                        AoBrightnessXYZNPP = AoBrightnessXZNP;
                    }

                    if (par1Block.MinX <= 0.0F)
                    {
                        par2++;
                    }

                    f26 = (aoLightValueScratchXYNN + aoLightValueScratchXYZNNP + aoLightValueXNeg + AoLightValueScratchXZNP) / 4F;
                    f5 = (aoLightValueXNeg + AoLightValueScratchXZNP + AoLightValueScratchXYNP + AoLightValueScratchXYZNPP) / 4F;
                    f12 = (AoLightValueScratchXZNN + aoLightValueXNeg + aoLightValueScratchXYZNPN + AoLightValueScratchXYNP) / 4F;
                    f19 = (aoLightValueScratchXYZNNN + aoLightValueScratchXYNN + AoLightValueScratchXZNN + aoLightValueXNeg) / 4F;
                    brightnessTopRight = GetAoBrightness(AoBrightnessXYNN, AoBrightnessXYZNNP, AoBrightnessXZNP, j);
                    brightnessTopLeft = GetAoBrightness(AoBrightnessXZNP, AoBrightnessXYNP, AoBrightnessXYZNPP, j);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessXZNN, AoBrightnessXYZNPN, AoBrightnessXYNP, j);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessXYZNNN, AoBrightnessXYNN, AoBrightnessXZNN, j);
                }
                else
                {
                    f5 = f12 = f19 = f26 = aoLightValueXNeg;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = j;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = (flag5 ? par5 : 1.0F) * 0.6F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = (flag5 ? par6 : 1.0F) * 0.6F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = (flag5 ? par7 : 1.0F) * 0.6F;
                ColorRedTopLeft *= f5;
                ColorGreenTopLeft *= f5;
                ColorBlueTopLeft *= f5;
                ColorRedBottomLeft *= f12;
                ColorGreenBottomLeft *= f12;
                ColorBlueBottomLeft *= f12;
                ColorRedBottomRight *= f19;
                ColorGreenBottomRight *= f19;
                ColorBlueBottomRight *= f19;
                ColorRedTopRight *= f26;
                ColorGreenTopRight *= f26;
                ColorBlueTopRight *= f26;
                int j2 = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 4);
                RenderNorthFace(par1Block, par2, par3, par4, j2);

                if (FancyGrass && j2 == 3 && overrideBlockTexture < 0)
                {
                    ColorRedTopLeft *= par5;
                    ColorRedBottomLeft *= par5;
                    ColorRedBottomRight *= par5;
                    ColorRedTopRight *= par5;
                    ColorGreenTopLeft *= par6;
                    ColorGreenBottomLeft *= par6;
                    ColorGreenBottomRight *= par6;
                    ColorGreenTopRight *= par6;
                    ColorBlueTopLeft *= par7;
                    ColorBlueBottomLeft *= par7;
                    ColorBlueBottomRight *= par7;
                    ColorBlueTopRight *= par7;
                    RenderNorthFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 + 1, par3, par4, 5))
            {
                float f6;
                float f13;
                float f20;
                float f27;

                if (AoType > 0)
                {
                    if (par1Block.MaxX >= 1.0D)
                    {
                        par2++;
                    }

                    aoLightValueScratchXYPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4);
                    AoLightValueScratchXZPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 - 1);
                    AoLightValueScratchXZPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3, par4 + 1);
                    AoLightValueScratchXYPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4);
                    AoBrightnessXYPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4);
                    AoBrightnessXZPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1);
                    AoBrightnessXZPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1);
                    AoBrightnessXYPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4);

                    if (AoGrassXYZPNC || AoGrassXYZPCN)
                    {
                        aoLightValueScratchXYZPNN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4 - 1);
                        AoBrightnessXYZPNN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4 - 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNN = AoLightValueScratchXZPN;
                        AoBrightnessXYZPNN = AoBrightnessXZPN;
                    }

                    if (AoGrassXYZPNC || AoGrassXYZPCP)
                    {
                        aoLightValueScratchXYZPNP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 - 1, par4 + 1);
                        AoBrightnessXYZPNP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4 + 1);
                    }
                    else
                    {
                        aoLightValueScratchXYZPNP = AoLightValueScratchXZPP;
                        AoBrightnessXYZPNP = AoBrightnessXZPP;
                    }

                    if (AoGrassXYZPPC || AoGrassXYZPCN)
                    {
                        AoLightValueScratchXYZPPN = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4 - 1);
                        AoBrightnessXYZPPN = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4 - 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPN = AoLightValueScratchXZPN;
                        AoBrightnessXYZPPN = AoBrightnessXZPN;
                    }

                    if (AoGrassXYZPPC || AoGrassXYZPCP)
                    {
                        AoLightValueScratchXYZPPP = par1Block.GetAmbientOcclusionLightValue(BlockAccess, par2, par3 + 1, par4 + 1);
                        AoBrightnessXYZPPP = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4 + 1);
                    }
                    else
                    {
                        AoLightValueScratchXYZPPP = AoLightValueScratchXZPP;
                        AoBrightnessXYZPPP = AoBrightnessXZPP;
                    }

                    if (par1Block.MaxX >= 1.0D)
                    {
                        par2--;
                    }

                    f6 = (aoLightValueScratchXYPN + aoLightValueScratchXYZPNP + aoLightValueXPos + AoLightValueScratchXZPP) / 4F;
                    f27 = (aoLightValueXPos + AoLightValueScratchXZPP + AoLightValueScratchXYPP + AoLightValueScratchXYZPPP) / 4F;
                    f20 = (AoLightValueScratchXZPN + aoLightValueXPos + AoLightValueScratchXYZPPN + AoLightValueScratchXYPP) / 4F;
                    f13 = (aoLightValueScratchXYZPNN + aoLightValueScratchXYPN + AoLightValueScratchXZPN + aoLightValueXPos) / 4F;
                    brightnessTopLeft = GetAoBrightness(AoBrightnessXYPN, AoBrightnessXYZPNP, AoBrightnessXZPP, i1);
                    brightnessTopRight = GetAoBrightness(AoBrightnessXZPP, AoBrightnessXYPP, AoBrightnessXYZPPP, i1);
                    brightnessBottomRight = GetAoBrightness(AoBrightnessXZPN, AoBrightnessXYZPPN, AoBrightnessXYPP, i1);
                    brightnessBottomLeft = GetAoBrightness(AoBrightnessXYZPNN, AoBrightnessXYPN, AoBrightnessXZPN, i1);
                }
                else
                {
                    f6 = f13 = f20 = f27 = aoLightValueXPos;
                    brightnessTopLeft = brightnessBottomLeft = brightnessBottomRight = brightnessTopRight = i1;
                }

                ColorRedTopLeft = ColorRedBottomLeft = ColorRedBottomRight = ColorRedTopRight = (flag6 ? par5 : 1.0F) * 0.6F;
                ColorGreenTopLeft = ColorGreenBottomLeft = ColorGreenBottomRight = ColorGreenTopRight = (flag6 ? par6 : 1.0F) * 0.6F;
                ColorBlueTopLeft = ColorBlueBottomLeft = ColorBlueBottomRight = ColorBlueTopRight = (flag6 ? par7 : 1.0F) * 0.6F;
                ColorRedTopLeft *= f6;
                ColorGreenTopLeft *= f6;
                ColorBlueTopLeft *= f6;
                ColorRedBottomLeft *= f13;
                ColorGreenBottomLeft *= f13;
                ColorBlueBottomLeft *= f13;
                ColorRedBottomRight *= f20;
                ColorGreenBottomRight *= f20;
                ColorBlueBottomRight *= f20;
                ColorRedTopRight *= f27;
                ColorGreenTopRight *= f27;
                ColorBlueTopRight *= f27;
                int k2 = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 5);
                RenderSouthFace(par1Block, par2, par3, par4, k2);

                if (FancyGrass && k2 == 3 && overrideBlockTexture < 0)
                {
                    ColorRedTopLeft *= par5;
                    ColorRedBottomLeft *= par5;
                    ColorRedBottomRight *= par5;
                    ColorRedTopRight *= par5;
                    ColorGreenTopLeft *= par6;
                    ColorGreenBottomLeft *= par6;
                    ColorGreenBottomRight *= par6;
                    ColorGreenTopRight *= par6;
                    ColorBlueTopLeft *= par7;
                    ColorBlueBottomLeft *= par7;
                    ColorBlueBottomRight *= par7;
                    ColorBlueTopRight *= par7;
                    RenderSouthFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            enableAo = false;
            return flag;
        }

        ///<summary>
        /// Get ambient occlusion Brightness
        ///</summary>
        private int GetAoBrightness(int par1, int par2, int par3, int par4)
        {
            if (par1 == 0)
            {
                par1 = par4;
            }

            if (par2 == 0)
            {
                par2 = par4;
            }

            if (par3 == 0)
            {
                par3 = par4;
            }

            return par1 + par2 + par3 + par4 >> 2 & 0xff00ff;
        }

        ///<summary>
        /// Renders a standard cube block at the given coordinates, with a given Color ratio.  Args: block, x, y, z, r, g, b
        ///</summary>
        public bool RenderStandardBlockWithColorMultiplier(Block par1Block, int par2, int par3, int par4, float par5, float par6, float par7)
        {
            enableAo = false;
            Tessellator tessellator = Tessellator.Instance;
            bool flag = false;
            float f = 0.5F;
            float f1 = 1.0F;
            float f2 = 0.8F;
            float f3 = 0.6F;
            float f4 = f1 * par5;
            float f5 = f1 * par6;
            float f6 = f1 * par7;
            float f7 = f;
            float f8 = f2;
            float f9 = f3;
            float f10 = f;
            float f11 = f2;
            float f12 = f3;
            float f13 = f;
            float f14 = f2;
            float f15 = f3;

            if (par1Block != Block.Grass)
            {
                f7 *= par5;
                f8 *= par5;
                f9 *= par5;
                f10 *= par6;
                f11 *= par6;
                f12 *= par6;
                f13 *= par7;
                f14 *= par7;
                f15 *= par7;
            }

            int i = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4);

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 - 1, par4, 0))
            {
                tessellator.SetBrightness(par1Block.MinY <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4) : i);
                tessellator.SetColorOpaque_F(f7, f10, f13);
                RenderBottomFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 0));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 + 1, par4, 1))
            {
                tessellator.SetBrightness(par1Block.MaxY >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4) : i);
                tessellator.SetColorOpaque_F(f4, f5, f6);
                RenderTopFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 1));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 - 1, 2))
            {
                tessellator.SetBrightness(par1Block.MinZ <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1) : i);
                tessellator.SetColorOpaque_F(f8, f11, f14);
                int j = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 2);
                RenderEastFace(par1Block, par2, par3, par4, j);

                if (FancyGrass && j == 3 && overrideBlockTexture < 0)
                {
                    tessellator.SetColorOpaque_F(f8 * par5, f11 * par6, f14 * par7);
                    RenderEastFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 + 1, 3))
            {
                tessellator.SetBrightness(par1Block.MaxZ >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1) : i);
                tessellator.SetColorOpaque_F(f8, f11, f14);
                int k = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3);
                RenderWestFace(par1Block, par2, par3, par4, k);

                if (FancyGrass && k == 3 && overrideBlockTexture < 0)
                {
                    tessellator.SetColorOpaque_F(f8 * par5, f11 * par6, f14 * par7);
                    RenderWestFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 - 1, par3, par4, 4))
            {
                tessellator.SetBrightness(par1Block.MinX <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4) : i);
                tessellator.SetColorOpaque_F(f9, f12, f15);
                int l = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 4);
                RenderNorthFace(par1Block, par2, par3, par4, l);

                if (FancyGrass && l == 3 && overrideBlockTexture < 0)
                {
                    tessellator.SetColorOpaque_F(f9 * par5, f12 * par6, f15 * par7);
                    RenderNorthFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 + 1, par3, par4, 5))
            {
                tessellator.SetBrightness(par1Block.MaxX >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4) : i);
                tessellator.SetColorOpaque_F(f9, f12, f15);
                int i1 = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 5);
                RenderSouthFace(par1Block, par2, par3, par4, i1);

                if (FancyGrass && i1 == 3 && overrideBlockTexture < 0)
                {
                    tessellator.SetColorOpaque_F(f9 * par5, f12 * par6, f15 * par7);
                    RenderSouthFace(par1Block, par2, par3, par4, 38);
                }

                flag = true;
            }

            return flag;
        }

        ///<summary>
        /// Renders a cactus block at the given coordinates
        ///</summary>
        public bool RenderBlockCactus(Block par1Block, int par2, int par3, int par4)
        {
            int i = par1Block.ColorMultiplier(BlockAccess, par2, par3, par4);
            float f = (float)(i >> 16 & 0xff) / 255F;
            float f1 = (float)(i >> 8 & 0xff) / 255F;
            float f2 = (float)(i & 0xff) / 255F;

            if (EntityRenderer.AnaglyphEnable)
            {
                float f3 = (f * 30F + f1 * 59F + f2 * 11F) / 100F;
                float f4 = (f * 30F + f1 * 70F) / 100F;
                float f5 = (f * 30F + f2 * 70F) / 100F;
                f = f3;
                f1 = f4;
                f2 = f5;
            }

            return RenderBlockCactusImpl(par1Block, par2, par3, par4, f, f1, f2);
        }

        ///<summary>
        /// Render block cactus implementation
        ///</summary>
        public bool RenderBlockCactusImpl(Block par1Block, int par2, int par3, int par4, float par5, float par6, float par7)
        {
            Tessellator tessellator = Tessellator.Instance;
            bool flag = false;
            float f = 0.5F;
            float f1 = 1.0F;
            float f2 = 0.8F;
            float f3 = 0.6F;
            float f4 = f * par5;
            float f5 = f1 * par5;
            float f6 = f2 * par5;
            float f7 = f3 * par5;
            float f8 = f * par6;
            float f9 = f1 * par6;
            float f10 = f2 * par6;
            float f11 = f3 * par6;
            float f12 = f * par7;
            float f13 = f1 * par7;
            float f14 = f2 * par7;
            float f15 = f3 * par7;
            float f16 = 0.0625F;
            int i = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4);

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 - 1, par4, 0))
            {
                tessellator.SetBrightness(par1Block.MinY <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4) : i);
                tessellator.SetColorOpaque_F(f4, f8, f12);
                RenderBottomFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 0));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3 + 1, par4, 1))
            {
                tessellator.SetBrightness(par1Block.MaxY >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4) : i);
                tessellator.SetColorOpaque_F(f5, f9, f13);
                RenderTopFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 1));
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 - 1, 2))
            {
                tessellator.SetBrightness(par1Block.MinZ <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1) : i);
                tessellator.SetColorOpaque_F(f6, f10, f14);
                tessellator.AddTranslation(0.0F, 0.0F, f16);
                RenderEastFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 2));
                tessellator.AddTranslation(0.0F, 0.0F, -f16);
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2, par3, par4 + 1, 3))
            {
                tessellator.SetBrightness(par1Block.MaxZ >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1) : i);
                tessellator.SetColorOpaque_F(f6, f10, f14);
                tessellator.AddTranslation(0.0F, 0.0F, -f16);
                RenderWestFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3));
                tessellator.AddTranslation(0.0F, 0.0F, f16);
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 - 1, par3, par4, 4))
            {
                tessellator.SetBrightness(par1Block.MinX <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4) : i);
                tessellator.SetColorOpaque_F(f7, f11, f15);
                tessellator.AddTranslation(f16, 0.0F, 0.0F);
                RenderNorthFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 4));
                tessellator.AddTranslation(-f16, 0.0F, 0.0F);
                flag = true;
            }

            if (renderAllFaces || par1Block.ShouldSideBeRendered(BlockAccess, par2 + 1, par3, par4, 5))
            {
                tessellator.SetBrightness(par1Block.MaxX >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4) : i);
                tessellator.SetColorOpaque_F(f7, f11, f15);
                tessellator.AddTranslation(-f16, 0.0F, 0.0F);
                RenderSouthFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 5));
                tessellator.AddTranslation(f16, 0.0F, 0.0F);
                flag = true;
            }

            return flag;
        }

        public bool RenderBlockFence(BlockFence par1BlockFence, int par2, int par3, int par4)
        {
            bool flag = false;
            float f = 0.375F;
            float f1 = 0.625F;
            par1BlockFence.SetBlockBounds(f, 0.0F, f, f1, 1.0F, f1);
            RenderStandardBlock(par1BlockFence, par2, par3, par4);
            flag = true;
            bool flag1 = false;
            bool flag2 = false;

            if (par1BlockFence.CanConnectFenceTo(BlockAccess, par2 - 1, par3, par4) || par1BlockFence.CanConnectFenceTo(BlockAccess, par2 + 1, par3, par4))
            {
                flag1 = true;
            }

            if (par1BlockFence.CanConnectFenceTo(BlockAccess, par2, par3, par4 - 1) || par1BlockFence.CanConnectFenceTo(BlockAccess, par2, par3, par4 + 1))
            {
                flag2 = true;
            }

            bool flag3 = par1BlockFence.CanConnectFenceTo(BlockAccess, par2 - 1, par3, par4);
            bool flag4 = par1BlockFence.CanConnectFenceTo(BlockAccess, par2 + 1, par3, par4);
            bool flag5 = par1BlockFence.CanConnectFenceTo(BlockAccess, par2, par3, par4 - 1);
            bool flag6 = par1BlockFence.CanConnectFenceTo(BlockAccess, par2, par3, par4 + 1);

            if (!flag1 && !flag2)
            {
                flag1 = true;
            }

            f = 0.4375F;
            f1 = 0.5625F;
            float f2 = 0.75F;
            float f3 = 0.9375F;
            float f4 = flag3 ? 0.0F : f;
            float f5 = flag4 ? 1.0F : f1;
            float f6 = flag5 ? 0.0F : f;
            float f7 = flag6 ? 1.0F : f1;

            if (flag1)
            {
                par1BlockFence.SetBlockBounds(f4, f2, f, f5, f3, f1);
                RenderStandardBlock(par1BlockFence, par2, par3, par4);
                flag = true;
            }

            if (flag2)
            {
                par1BlockFence.SetBlockBounds(f, f2, f6, f1, f3, f7);
                RenderStandardBlock(par1BlockFence, par2, par3, par4);
                flag = true;
            }

            f2 = 0.375F;
            f3 = 0.5625F;

            if (flag1)
            {
                par1BlockFence.SetBlockBounds(f4, f2, f, f5, f3, f1);
                RenderStandardBlock(par1BlockFence, par2, par3, par4);
                flag = true;
            }

            if (flag2)
            {
                par1BlockFence.SetBlockBounds(f, f2, f6, f1, f3, f7);
                RenderStandardBlock(par1BlockFence, par2, par3, par4);
                flag = true;
            }

            par1BlockFence.SetBlockBoundsBasedOnState(BlockAccess, par2, par3, par4);
            return flag;
        }

        public bool RenderBlockDragonEgg(BlockDragonEgg par1BlockDragonEgg, int par2, int par3, int par4)
        {
            bool flag = false;
            int i = 0;

            for (int j = 0; j < 8; j++)
            {
                int k = 0;
                byte byte0 = 1;

                if (j == 0)
                {
                    k = 2;
                }

                if (j == 1)
                {
                    k = 3;
                }

                if (j == 2)
                {
                    k = 4;
                }

                if (j == 3)
                {
                    k = 5;
                    byte0 = 2;
                }

                if (j == 4)
                {
                    k = 6;
                    byte0 = 3;
                }

                if (j == 5)
                {
                    k = 7;
                    byte0 = 5;
                }

                if (j == 6)
                {
                    k = 6;
                    byte0 = 2;
                }

                if (j == 7)
                {
                    k = 3;
                }

                float f = (float)k / 16F;
                float f1 = 1.0F - (float)i / 16F;
                float f2 = 1.0F - (float)(i + byte0) / 16F;
                i += byte0;
                par1BlockDragonEgg.SetBlockBounds(0.5F - f, f2, 0.5F - f, 0.5F + f, f1, 0.5F + f);
                RenderStandardBlock(par1BlockDragonEgg, par2, par3, par4);
            }

            flag = true;
            par1BlockDragonEgg.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            return flag;
        }

        ///<summary>
        /// Render block fence gate
        ///</summary>
        public bool RenderBlockFenceGate(BlockFenceGate par1BlockFenceGate, int par2, int par3, int par4)
        {
            bool flag = true;
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            bool flag1 = BlockFenceGate.IsFenceGateOpen(i);
            int j = BlockDirectional.GetDirection(i);

            if (j == 3 || j == 1)
            {
                float f = 0.4375F;
                float f4 = 0.5625F;
                float f8 = 0.0F;
                float f12 = 0.125F;
                par1BlockFenceGate.SetBlockBounds(f, 0.3125F, f8, f4, 1.0F, f12);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                f8 = 0.875F;
                f12 = 1.0F;
                par1BlockFenceGate.SetBlockBounds(f, 0.3125F, f8, f4, 1.0F, f12);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }
            else
            {
                float f1 = 0.0F;
                float f5 = 0.125F;
                float f9 = 0.4375F;
                float f13 = 0.5625F;
                par1BlockFenceGate.SetBlockBounds(f1, 0.3125F, f9, f5, 1.0F, f13);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                f1 = 0.875F;
                f5 = 1.0F;
                par1BlockFenceGate.SetBlockBounds(f1, 0.3125F, f9, f5, 1.0F, f13);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }

            if (!flag1)
            {
                if (j == 3 || j == 1)
                {
                    float f2 = 0.4375F;
                    float f6 = 0.5625F;
                    float f10 = 0.375F;
                    float f14 = 0.5F;
                    par1BlockFenceGate.SetBlockBounds(f2, 0.375F, f10, f6, 0.9375F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f10 = 0.5F;
                    f14 = 0.625F;
                    par1BlockFenceGate.SetBlockBounds(f2, 0.375F, f10, f6, 0.9375F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f10 = 0.625F;
                    f14 = 0.875F;
                    par1BlockFenceGate.SetBlockBounds(f2, 0.375F, f10, f6, 0.5625F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    par1BlockFenceGate.SetBlockBounds(f2, 0.75F, f10, f6, 0.9375F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f10 = 0.125F;
                    f14 = 0.375F;
                    par1BlockFenceGate.SetBlockBounds(f2, 0.375F, f10, f6, 0.5625F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    par1BlockFenceGate.SetBlockBounds(f2, 0.75F, f10, f6, 0.9375F, f14);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                }
                else
                {
                    float f3 = 0.375F;
                    float f7 = 0.5F;
                    float f11 = 0.4375F;
                    float f15 = 0.5625F;
                    par1BlockFenceGate.SetBlockBounds(f3, 0.375F, f11, f7, 0.9375F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f3 = 0.5F;
                    f7 = 0.625F;
                    par1BlockFenceGate.SetBlockBounds(f3, 0.375F, f11, f7, 0.9375F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f3 = 0.625F;
                    f7 = 0.875F;
                    par1BlockFenceGate.SetBlockBounds(f3, 0.375F, f11, f7, 0.5625F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    par1BlockFenceGate.SetBlockBounds(f3, 0.75F, f11, f7, 0.9375F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    f3 = 0.125F;
                    f7 = 0.375F;
                    par1BlockFenceGate.SetBlockBounds(f3, 0.375F, f11, f7, 0.5625F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                    par1BlockFenceGate.SetBlockBounds(f3, 0.75F, f11, f7, 0.9375F, f15);
                    RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                }
            }
            else if (j == 3)
            {
                par1BlockFenceGate.SetBlockBounds(0.8125F, 0.375F, 0.0F, 0.9375F, 0.9375F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.8125F, 0.375F, 0.875F, 0.9375F, 0.9375F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.5625F, 0.375F, 0.0F, 0.8125F, 0.5625F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.5625F, 0.375F, 0.875F, 0.8125F, 0.5625F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.5625F, 0.75F, 0.0F, 0.8125F, 0.9375F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.5625F, 0.75F, 0.875F, 0.8125F, 0.9375F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }
            else if (j == 1)
            {
                par1BlockFenceGate.SetBlockBounds(0.0625F, 0.375F, 0.0F, 0.1875F, 0.9375F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.0625F, 0.375F, 0.875F, 0.1875F, 0.9375F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.1875F, 0.375F, 0.0F, 0.4375F, 0.5625F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.1875F, 0.375F, 0.875F, 0.4375F, 0.5625F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.1875F, 0.75F, 0.0F, 0.4375F, 0.9375F, 0.125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.1875F, 0.75F, 0.875F, 0.4375F, 0.9375F, 1.0F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }
            else if (j == 0)
            {
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.375F, 0.8125F, 0.125F, 0.9375F, 0.9375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.375F, 0.8125F, 1.0F, 0.9375F, 0.9375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.375F, 0.5625F, 0.125F, 0.5625F, 0.8125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.375F, 0.5625F, 1.0F, 0.5625F, 0.8125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.75F, 0.5625F, 0.125F, 0.9375F, 0.8125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.75F, 0.5625F, 1.0F, 0.9375F, 0.8125F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }
            else if (j == 2)
            {
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.375F, 0.0625F, 0.125F, 0.9375F, 0.1875F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.375F, 0.0625F, 1.0F, 0.9375F, 0.1875F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.375F, 0.1875F, 0.125F, 0.5625F, 0.4375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.375F, 0.1875F, 1.0F, 0.5625F, 0.4375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.0F, 0.75F, 0.1875F, 0.125F, 0.9375F, 0.4375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
                par1BlockFenceGate.SetBlockBounds(0.875F, 0.75F, 0.1875F, 1.0F, 0.9375F, 0.4375F);
                RenderStandardBlock(par1BlockFenceGate, par2, par3, par4);
            }

            par1BlockFenceGate.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            return flag;
        }

        ///<summary>
        /// Renders a stair block at the given coordinates
        ///</summary>
        public bool RenderBlockStairs(Block par1Block, int par2, int par3, int par4)
        {
            int i = BlockAccess.GetBlockMetadata(par2, par3, par4);
            int j = i & 3;
            float f = 0.0F;
            float f1 = 0.5F;
            float f2 = 0.5F;
            float f3 = 1.0F;

            if ((i & 4) != 0)
            {
                f = 0.5F;
                f1 = 1.0F;
                f2 = 0.0F;
                f3 = 0.5F;
            }

            par1Block.SetBlockBounds(0.0F, f, 0.0F, 1.0F, f1, 1.0F);
            RenderStandardBlock(par1Block, par2, par3, par4);

            if (j == 0)
            {
                par1Block.SetBlockBounds(0.5F, f2, 0.0F, 1.0F, f3, 1.0F);
                RenderStandardBlock(par1Block, par2, par3, par4);
            }
            else if (j == 1)
            {
                par1Block.SetBlockBounds(0.0F, f2, 0.0F, 0.5F, f3, 1.0F);
                RenderStandardBlock(par1Block, par2, par3, par4);
            }
            else if (j == 2)
            {
                par1Block.SetBlockBounds(0.0F, f2, 0.5F, 1.0F, f3, 1.0F);
                RenderStandardBlock(par1Block, par2, par3, par4);
            }
            else if (j == 3)
            {
                par1Block.SetBlockBounds(0.0F, f2, 0.0F, 1.0F, f3, 0.5F);
                RenderStandardBlock(par1Block, par2, par3, par4);
            }

            par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            return true;
        }

        ///<summary>
        /// Renders a door block at the given coordinates
        ///</summary>
        public bool RenderBlockDoor(Block par1Block, int par2, int par3, int par4)
        {
            Tessellator tessellator = Tessellator.Instance;
            BlockDoor blockdoor = (BlockDoor)par1Block;
            bool flag = false;
            float f = 0.5F;
            float f1 = 1.0F;
            float f2 = 0.8F;
            float f3 = 0.6F;
            int i = par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4);
            tessellator.SetBrightness(par1Block.MinY <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 - 1, par4) : i);
            tessellator.SetColorOpaque_F(f, f, f);
            RenderBottomFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 0));
            flag = true;
            tessellator.SetBrightness(par1Block.MaxY >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3 + 1, par4) : i);
            tessellator.SetColorOpaque_F(f1, f1, f1);
            RenderTopFace(par1Block, par2, par3, par4, par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 1));
            flag = true;
            tessellator.SetBrightness(par1Block.MinZ <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 - 1) : i);
            tessellator.SetColorOpaque_F(f2, f2, f2);
            int j = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 2);

            if (j < 0)
            {
                flipTexture = true;
                j = -j;
            }

            RenderEastFace(par1Block, par2, par3, par4, j);
            flag = true;
            flipTexture = false;
            tessellator.SetBrightness(par1Block.MaxZ >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2, par3, par4 + 1) : i);
            tessellator.SetColorOpaque_F(f2, f2, f2);
            j = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 3);

            if (j < 0)
            {
                flipTexture = true;
                j = -j;
            }

            RenderWestFace(par1Block, par2, par3, par4, j);
            flag = true;
            flipTexture = false;
            tessellator.SetBrightness(par1Block.MinX <= 0.0F ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 - 1, par3, par4) : i);
            tessellator.SetColorOpaque_F(f3, f3, f3);
            j = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 4);

            if (j < 0)
            {
                flipTexture = true;
                j = -j;
            }

            RenderNorthFace(par1Block, par2, par3, par4, j);
            flag = true;
            flipTexture = false;
            tessellator.SetBrightness(par1Block.MaxX >= 1.0D ? par1Block.GetMixedBrightnessForBlock(BlockAccess, par2 + 1, par3, par4) : i);
            tessellator.SetColorOpaque_F(f3, f3, f3);
            j = par1Block.GetBlockTexture(BlockAccess, par2, par3, par4, 5);

            if (j < 0)
            {
                flipTexture = true;
                j = -j;
            }

            RenderSouthFace(par1Block, par2, par3, par4, j);
            flag = true;
            flipTexture = false;
            return flag;
        }

        ///<summary>
        /// Renders the given texture to the bottom face of the block. Args: block, x, y, z, texture
        ///</summary>
        public void RenderBottomFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinX * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxX * 16D) - 0.01D) / 256D;
            double d2 = ((double)j + par1Block.MinZ * 16D) / 256D;
            double d3 = (((double)j + par1Block.MaxZ * 16D) - 0.01D) / 256D;

            if (par1Block.MinX < 0.0F || par1Block.MaxX > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinZ < 0.0F || par1Block.MaxZ > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d4 = d1;
            double d5 = d;
            double d6 = d2;
            double d7 = d3;

            if (uvRotateBottom == 2)
            {
                d = ((double)i + par1Block.MinZ * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MaxX * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxZ * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MinX * 16D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
                d4 = d;
                d5 = d1;
                d2 = d3;
                d3 = d6;
            }
            else if (uvRotateBottom == 1)
            {
                d = ((double)(i + 16) - par1Block.MaxZ * 16D) / 256D;
                d2 = ((double)j + par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinZ * 16D) / 256D;
                d3 = ((double)j + par1Block.MaxX * 16D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
                d = d4;
                d1 = d5;
                d6 = d3;
                d7 = d2;
            }
            else if (uvRotateBottom == 3)
            {
                d = ((double)(i + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxX * 16D - 0.01D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MinZ * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MaxZ * 16D - 0.01D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
            }

            double d8 = par2 + par1Block.MinX;
            double d9 = par2 + par1Block.MaxX;
            double d10 = par4 + par1Block.MinY;
            double d11 = par6 + par1Block.MinZ;
            double d12 = par6 + par1Block.MaxZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d8, d10, d12, d5, d7);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d8, d10, d11, d, d2);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d9, d10, d11, d4, d6);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
            }
            else
            {
                tessellator.AddVertexWithUV(d8, d10, d12, d5, d7);
                tessellator.AddVertexWithUV(d8, d10, d11, d, d2);
                tessellator.AddVertexWithUV(d9, d10, d11, d4, d6);
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
            }
        }

        ///<summary>
        /// Renders the given texture to the top face of the block. Args: block, x, y, z, texture
        ///</summary>
        public void RenderTopFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinX * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxX * 16D) - 0.01D) / 256D;
            double d2 = ((double)j + par1Block.MinZ * 16D) / 256D;
            double d3 = (((double)j + par1Block.MaxZ * 16D) - 0.01D) / 256D;

            if (par1Block.MinX < 0.0F || par1Block.MaxX > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinZ < 0.0F || par1Block.MaxZ > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d4 = d1;
            double d5 = d;
            double d6 = d2;
            double d7 = d3;

            if (uvRotateTop == 1)
            {
                d = ((double)i + par1Block.MinZ * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MaxX * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxZ * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MinX * 16D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
                d4 = d;
                d5 = d1;
                d2 = d3;
                d3 = d6;
            }
            else if (uvRotateTop == 2)
            {
                d = ((double)(i + 16) - par1Block.MaxZ * 16D) / 256D;
                d2 = ((double)j + par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinZ * 16D) / 256D;
                d3 = ((double)j + par1Block.MaxX * 16D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
                d = d4;
                d1 = d5;
                d6 = d3;
                d7 = d2;
            }
            else if (uvRotateTop == 3)
            {
                d = ((double)(i + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxX * 16D - 0.01D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MinZ * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MaxZ * 16D - 0.01D) / 256D;
                d4 = d1;
                d5 = d;
                d6 = d2;
                d7 = d3;
            }

            double d8 = par2 + par1Block.MinX;
            double d9 = par2 + par1Block.MaxX;
            double d10 = par4 + par1Block.MaxY;
            double d11 = par6 + par1Block.MinZ;
            double d12 = par6 + par1Block.MaxZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d9, d10, d11, d4, d6);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d8, d10, d11, d, d2);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d8, d10, d12, d5, d7);
            }
            else
            {
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
                tessellator.AddVertexWithUV(d9, d10, d11, d4, d6);
                tessellator.AddVertexWithUV(d8, d10, d11, d, d2);
                tessellator.AddVertexWithUV(d8, d10, d12, d5, d7);
            }
        }

        ///<summary>
        /// Renders the given texture to the east (z-negative) face of the block.  Args: block, x, y, z, texture
        ///</summary>
        public void RenderEastFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinX * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxX * 16D) - 0.01D) / 256D;
            double d2 = ((double)(j + 16) - par1Block.MaxY * 16D) / 256D;
            double d3 = ((double)(j + 16) - par1Block.MinY * 16D - 0.01D) / 256D;

            if (flipTexture)
            {
                double d4 = d;
                d = d1;
                d1 = d4;
            }

            if (par1Block.MinX < 0.0F || par1Block.MaxX > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinY < 0.0F || par1Block.MaxY > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d5 = d1;
            double d6 = d;
            double d7 = d2;
            double d8 = d3;

            if (uvRotateEast == 2)
            {
                d = ((double)i + par1Block.MinY * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxY * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MaxX * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d5 = d;
                d6 = d1;
                d2 = d3;
                d3 = d7;
            }
            else if (uvRotateEast == 1)
            {
                d = ((double)(i + 16) - par1Block.MaxY * 16D) / 256D;
                d2 = ((double)j + par1Block.MaxX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinY * 16D) / 256D;
                d3 = ((double)j + par1Block.MinX * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d = d5;
                d1 = d6;
                d7 = d3;
                d8 = d2;
            }
            else if (uvRotateEast == 3)
            {
                d = ((double)(i + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxX * 16D - 0.01D) / 256D;
                d2 = ((double)j + par1Block.MaxY * 16D) / 256D;
                d3 = (((double)j + par1Block.MinY * 16D) - 0.01D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
            }

            double d9 = par2 + par1Block.MinX;
            double d10 = par2 + par1Block.MaxX;
            double d11 = par4 + par1Block.MinY;
            double d12 = par4 + par1Block.MaxY;
            double d13 = par6 + par1Block.MinZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d9, d12, d13, d5, d7);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d10, d12, d13, d, d2);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d10, d11, d13, d6, d8);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d9, d11, d13, d1, d3);
            }
            else
            {
                tessellator.AddVertexWithUV(d9, d12, d13, d5, d7);
                tessellator.AddVertexWithUV(d10, d12, d13, d, d2);
                tessellator.AddVertexWithUV(d10, d11, d13, d6, d8);
                tessellator.AddVertexWithUV(d9, d11, d13, d1, d3);
            }
        }

        ///<summary>
        /// Renders the given texture to the west (z-positive) face of the block.  Args: block, x, y, z, texture
        ///</summary>
        public void RenderWestFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinX * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxX * 16D) - 0.01D) / 256D;
            double d2 = ((double)(j + 16) - par1Block.MaxY * 16D) / 256D;
            double d3 = ((double)(j + 16) - par1Block.MinY * 16D - 0.01D) / 256D;

            if (flipTexture)
            {
                double d4 = d;
                d = d1;
                d1 = d4;
            }

            if (par1Block.MinX < 0.0F || par1Block.MaxX > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinY < 0.0F || par1Block.MaxY > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d5 = d1;
            double d6 = d;
            double d7 = d2;
            double d8 = d3;

            if (uvRotateWest == 1)
            {
                d = ((double)i + par1Block.MinY * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxY * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MaxX * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d5 = d;
                d6 = d1;
                d2 = d3;
                d3 = d7;
            }
            else if (uvRotateWest == 2)
            {
                d = ((double)(i + 16) - par1Block.MaxY * 16D) / 256D;
                d2 = ((double)j + par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinY * 16D) / 256D;
                d3 = ((double)j + par1Block.MaxX * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d = d5;
                d1 = d6;
                d7 = d3;
                d8 = d2;
            }
            else if (uvRotateWest == 3)
            {
                d = ((double)(i + 16) - par1Block.MinX * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxX * 16D - 0.01D) / 256D;
                d2 = ((double)j + par1Block.MaxY * 16D) / 256D;
                d3 = (((double)j + par1Block.MinY * 16D) - 0.01D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
            }

            double d9 = par2 + par1Block.MinX;
            double d10 = par2 + par1Block.MaxX;
            double d11 = par4 + par1Block.MinY;
            double d12 = par4 + par1Block.MaxY;
            double d13 = par6 + par1Block.MaxZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d9, d12, d13, d, d2);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d9, d11, d13, d6, d8);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d10, d11, d13, d1, d3);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d10, d12, d13, d5, d7);
            }
            else
            {
                tessellator.AddVertexWithUV(d9, d12, d13, d, d2);
                tessellator.AddVertexWithUV(d9, d11, d13, d6, d8);
                tessellator.AddVertexWithUV(d10, d11, d13, d1, d3);
                tessellator.AddVertexWithUV(d10, d12, d13, d5, d7);
            }
        }

        ///<summary>
        /// Renders the given texture to the north (x-negative) face of the block.  Args: block, x, y, z, texture
        ///</summary>
        public void RenderNorthFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinZ * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxZ * 16D) - 0.01D) / 256D;
            double d2 = ((double)(j + 16) - par1Block.MaxY * 16D) / 256D;
            double d3 = ((double)(j + 16) - par1Block.MinY * 16D - 0.01D) / 256D;

            if (flipTexture)
            {
                double d4 = d;
                d = d1;
                d1 = d4;
            }

            if (par1Block.MinZ < 0.0F || par1Block.MaxZ > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinY < 0.0F || par1Block.MaxY > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d5 = d1;
            double d6 = d;
            double d7 = d2;
            double d8 = d3;

            if (uvRotateNorth == 1)
            {
                d = ((double)i + par1Block.MinY * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MaxZ * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxY * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MinZ * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d5 = d;
                d6 = d1;
                d2 = d3;
                d3 = d7;
            }
            else if (uvRotateNorth == 2)
            {
                d = ((double)(i + 16) - par1Block.MaxY * 16D) / 256D;
                d2 = ((double)j + par1Block.MinZ * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinY * 16D) / 256D;
                d3 = ((double)j + par1Block.MaxZ * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d = d5;
                d1 = d6;
                d7 = d3;
                d8 = d2;
            }
            else if (uvRotateNorth == 3)
            {
                d = ((double)(i + 16) - par1Block.MinZ * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxZ * 16D - 0.01D) / 256D;
                d2 = ((double)j + par1Block.MaxY * 16D) / 256D;
                d3 = (((double)j + par1Block.MinY * 16D) - 0.01D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
            }

            double d9 = par2 + par1Block.MinX;
            double d10 = par4 + par1Block.MinY;
            double d11 = par4 + par1Block.MaxY;
            double d12 = par6 + par1Block.MinZ;
            double d13 = par6 + par1Block.MaxZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d9, d11, d13, d5, d7);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d9, d11, d12, d, d2);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d9, d10, d12, d6, d8);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d9, d10, d13, d1, d3);
            }
            else
            {
                tessellator.AddVertexWithUV(d9, d11, d13, d5, d7);
                tessellator.AddVertexWithUV(d9, d11, d12, d, d2);
                tessellator.AddVertexWithUV(d9, d10, d12, d6, d8);
                tessellator.AddVertexWithUV(d9, d10, d13, d1, d3);
            }
        }

        ///<summary>
        /// Renders the given texture to the south (x-positive) face of the block.  Args: block, x, y, z, texture
        ///</summary>
        public void RenderSouthFace(Block par1Block, double par2, double par4, double par6, int par8)
        {
            Tessellator tessellator = Tessellator.Instance;

            if (overrideBlockTexture >= 0)
            {
                par8 = overrideBlockTexture;
            }

            int i = (par8 & 0xf) << 4;
            int j = par8 & 0xf0;
            double d = ((double)i + par1Block.MinZ * 16D) / 256D;
            double d1 = (((double)i + par1Block.MaxZ * 16D) - 0.01D) / 256D;
            double d2 = ((double)(j + 16) - par1Block.MaxY * 16D) / 256D;
            double d3 = ((double)(j + 16) - par1Block.MinY * 16D - 0.01D) / 256D;

            if (flipTexture)
            {
                double d4 = d;
                d = d1;
                d1 = d4;
            }

            if (par1Block.MinZ < 0.0F || par1Block.MaxZ > 1.0D)
            {
                d = ((float)i + 0.0F) / 256F;
                d1 = ((float)i + 15.99F) / 256F;
            }

            if (par1Block.MinY < 0.0F || par1Block.MaxY > 1.0D)
            {
                d2 = ((float)j + 0.0F) / 256F;
                d3 = ((float)j + 15.99F) / 256F;
            }

            double d5 = d1;
            double d6 = d;
            double d7 = d2;
            double d8 = d3;

            if (uvRotateSouth == 2)
            {
                d = ((double)i + par1Block.MinY * 16D) / 256D;
                d2 = ((double)(j + 16) - par1Block.MinZ * 16D) / 256D;
                d1 = ((double)i + par1Block.MaxY * 16D) / 256D;
                d3 = ((double)(j + 16) - par1Block.MaxZ * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d5 = d;
                d6 = d1;
                d2 = d3;
                d3 = d7;
            }
            else if (uvRotateSouth == 1)
            {
                d = ((double)(i + 16) - par1Block.MaxY * 16D) / 256D;
                d2 = ((double)j + par1Block.MaxZ * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MinY * 16D) / 256D;
                d3 = ((double)j + par1Block.MinZ * 16D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
                d = d5;
                d1 = d6;
                d7 = d3;
                d8 = d2;
            }
            else if (uvRotateSouth == 3)
            {
                d = ((double)(i + 16) - par1Block.MinZ * 16D) / 256D;
                d1 = ((double)(i + 16) - par1Block.MaxZ * 16D - 0.01D) / 256D;
                d2 = ((double)j + par1Block.MaxY * 16D) / 256D;
                d3 = (((double)j + par1Block.MinY * 16D) - 0.01D) / 256D;
                d5 = d1;
                d6 = d;
                d7 = d2;
                d8 = d3;
            }

            double d9 = par2 + par1Block.MaxX;
            double d10 = par4 + par1Block.MinY;
            double d11 = par4 + par1Block.MaxY;
            double d12 = par6 + par1Block.MinZ;
            double d13 = par6 + par1Block.MaxZ;

            if (enableAo)
            {
                tessellator.SetColorOpaque_F(ColorRedTopLeft, ColorGreenTopLeft, ColorBlueTopLeft);
                tessellator.SetBrightness(brightnessTopLeft);
                tessellator.AddVertexWithUV(d9, d10, d13, d6, d8);
                tessellator.SetColorOpaque_F(ColorRedBottomLeft, ColorGreenBottomLeft, ColorBlueBottomLeft);
                tessellator.SetBrightness(brightnessBottomLeft);
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
                tessellator.SetColorOpaque_F(ColorRedBottomRight, ColorGreenBottomRight, ColorBlueBottomRight);
                tessellator.SetBrightness(brightnessBottomRight);
                tessellator.AddVertexWithUV(d9, d11, d12, d5, d7);
                tessellator.SetColorOpaque_F(ColorRedTopRight, ColorGreenTopRight, ColorBlueTopRight);
                tessellator.SetBrightness(brightnessTopRight);
                tessellator.AddVertexWithUV(d9, d11, d13, d, d2);
            }
            else
            {
                tessellator.AddVertexWithUV(d9, d10, d13, d6, d8);
                tessellator.AddVertexWithUV(d9, d10, d12, d1, d3);
                tessellator.AddVertexWithUV(d9, d11, d12, d5, d7);
                tessellator.AddVertexWithUV(d9, d11, d13, d, d2);
            }
        }

        ///<summary>
        /// Is called to render the image of a block on an inventory, as a held item, or as a an item on the ground
        ///</summary>
        public void RenderBlockAsItem(Block par1Block, int par2, float par3)
        {
            Tessellator tessellator = Tessellator.Instance;
            bool flag = par1Block.BlockID == Block.Grass.BlockID;

            if (UseInventoryTint)
            {
                int i = par1Block.GetRenderColor(par2);

                if (flag)
                {
                    i = 0xffffff;
                }

                float f = (float)(i >> 16 & 0xff) / 255F;
                float f2 = (float)(i >> 8 & 0xff) / 255F;
                float f6 = (float)(i & 0xff) / 255F;
                //GL.Color4(f * par3, f2 * par3, f6 * par3, 1.0F);
            }

            int j = par1Block.GetRenderType();

            if (j == 0 || j == 16)
            {
                if (j == 16)
                {
                    par2 = 1;
                }

                par1Block.SetBlockBoundsForItemRender();
                //GL.Translate(-0.5F, -0.5F, -0.5F);
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(0, par2));
                tessellator.Draw();

                if (flag && UseInventoryTint)
                {
                    int k = par1Block.GetRenderColor(par2);
                    float f3 = (float)(k >> 16 & 0xff) / 255F;
                    float f7 = (float)(k >> 8 & 0xff) / 255F;
                    float f8 = (float)(k & 0xff) / 255F;
                    //GL.Color4(f3 * par3, f7 * par3, f8 * par3, 1.0F);
                }

                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(1, par2));
                tessellator.Draw();

                if (flag && UseInventoryTint)
                {
                    //GL.Color4(par3, par3, par3, 1.0F);
                }

                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 0.0F, -1F);
                RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(2, par2));
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(3, par2));
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(-1F, 0.0F, 0.0F);
                RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(4, par2));
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSideAndMetadata(5, par2));
                tessellator.Draw();
                //GL.Translate(0.5F, 0.5F, 0.5F);
            }
            else if (j == 1)
            {
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                DrawCrossedSquares(par1Block, par2, -0.5D, -0.5D, -0.5D);
                tessellator.Draw();
            }
            else if (j == 19)
            {
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                par1Block.SetBlockBoundsForItemRender();
                RenderBlockStemSmall(par1Block, par2, par1Block.MaxY, -0.5D, -0.5D, -0.5D);
                tessellator.Draw();
            }
            else if (j == 23)
            {
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                par1Block.SetBlockBoundsForItemRender();
                tessellator.Draw();
            }
            else if (j == 13)
            {
                par1Block.SetBlockBoundsForItemRender();
                //GL.Translate(-0.5F, -0.5F, -0.5F);
                float f1 = 0.0625F;
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(0));
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(1));
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 0.0F, -1F);
                tessellator.AddTranslation(0.0F, 0.0F, f1);
                RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(2));
                tessellator.AddTranslation(0.0F, 0.0F, -f1);
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                tessellator.AddTranslation(0.0F, 0.0F, -f1);
                RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(3));
                tessellator.AddTranslation(0.0F, 0.0F, f1);
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(-1F, 0.0F, 0.0F);
                tessellator.AddTranslation(f1, 0.0F, 0.0F);
                RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(4));
                tessellator.AddTranslation(-f1, 0.0F, 0.0F);
                tessellator.Draw();
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                tessellator.AddTranslation(-f1, 0.0F, 0.0F);
                RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(5));
                tessellator.AddTranslation(f1, 0.0F, 0.0F);
                tessellator.Draw();
                //GL.Translate(0.5F, 0.5F, 0.5F);
            }
            else if (j == 22)
            {
                ChestItemRenderHelper.Instance.Func_35609_a(par1Block, par2, par3);
                //GL.Enable(EnableCap.RescaleNormal);
            }
            else if (j == 6)
            {
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                RenderBlockCropsImpl(par1Block, par2, -0.5D, -0.5D, -0.5D);
                tessellator.Draw();
            }
            else if (j == 2)
            {
                tessellator.StartDrawingQuads();
                tessellator.SetNormal(0.0F, -1F, 0.0F);
                RenderTorchAtAngle(par1Block, -0.5D, -0.5D, -0.5D, 0.0F, 0.0F);
                tessellator.Draw();
            }
            else if (j == 10)
            {
                for (int l = 0; l < 2; l++)
                {
                    if (l == 0)
                    {
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.5F);
                    }

                    if (l == 1)
                    {
                        par1Block.SetBlockBounds(0.0F, 0.0F, 0.5F, 1.0F, 0.5F, 1.0F);
                    }

                    //GL.Translate(-0.5F, -0.5F, -0.5F);
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, -1F, 0.0F);
                    RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(0));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                    RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(1));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, -1F);
                    RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(2));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                    RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(3));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(-1F, 0.0F, 0.0F);
                    RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(4));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                    RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(5));
                    tessellator.Draw();
                    //GL.Translate(0.5F, 0.5F, 0.5F);
                }
            }
            else if (j == 27)
            {
                int i1 = 0;
                //GL.Translate(-0.5F, -0.5F, -0.5F);
                tessellator.StartDrawingQuads();

                for (int l1 = 0; l1 < 8; l1++)
                {
                    int i2 = 0;
                    byte byte0 = 1;

                    if (l1 == 0)
                    {
                        i2 = 2;
                    }

                    if (l1 == 1)
                    {
                        i2 = 3;
                    }

                    if (l1 == 2)
                    {
                        i2 = 4;
                    }

                    if (l1 == 3)
                    {
                        i2 = 5;
                        byte0 = 2;
                    }

                    if (l1 == 4)
                    {
                        i2 = 6;
                        byte0 = 3;
                    }

                    if (l1 == 5)
                    {
                        i2 = 7;
                        byte0 = 5;
                    }

                    if (l1 == 6)
                    {
                        i2 = 6;
                        byte0 = 2;
                    }

                    if (l1 == 7)
                    {
                        i2 = 3;
                    }

                    float f9 = (float)i2 / 16F;
                    float f10 = 1.0F - (float)i1 / 16F;
                    float f11 = 1.0F - (float)(i1 + byte0) / 16F;
                    i1 += byte0;
                    par1Block.SetBlockBounds(0.5F - f9, f11, 0.5F - f9, 0.5F + f9, f10, 0.5F + f9);
                    tessellator.SetNormal(0.0F, -1F, 0.0F);
                    RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(0));
                    tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                    RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(1));
                    tessellator.SetNormal(0.0F, 0.0F, -1F);
                    RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(2));
                    tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                    RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(3));
                    tessellator.SetNormal(-1F, 0.0F, 0.0F);
                    RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(4));
                    tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                    RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(5));
                }

                tessellator.Draw();
                //GL.Translate(0.5F, 0.5F, 0.5F);
                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            }
            else if (j == 11)
            {
                for (int j1 = 0; j1 < 4; j1++)
                {
                    float f4 = 0.125F;

                    if (j1 == 0)
                    {
                        par1Block.SetBlockBounds(0.5F - f4, 0.0F, 0.0F, 0.5F + f4, 1.0F, f4 * 2.0F);
                    }

                    if (j1 == 1)
                    {
                        par1Block.SetBlockBounds(0.5F - f4, 0.0F, 1.0F - f4 * 2.0F, 0.5F + f4, 1.0F, 1.0F);
                    }

                    f4 = 0.0625F;

                    if (j1 == 2)
                    {
                        par1Block.SetBlockBounds(0.5F - f4, 1.0F - f4 * 3F, -f4 * 2.0F, 0.5F + f4, 1.0F - f4, 1.0F + f4 * 2.0F);
                    }

                    if (j1 == 3)
                    {
                        par1Block.SetBlockBounds(0.5F - f4, 0.5F - f4 * 3F, -f4 * 2.0F, 0.5F + f4, 0.5F - f4, 1.0F + f4 * 2.0F);
                    }

                    //GL.Translate(-0.5F, -0.5F, -0.5F);
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, -1F, 0.0F);
                    RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(0));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                    RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(1));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, -1F);
                    RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(2));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                    RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(3));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(-1F, 0.0F, 0.0F);
                    RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(4));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                    RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(5));
                    tessellator.Draw();
                    //GL.Translate(0.5F, 0.5F, 0.5F);
                }

                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            }
            else if (j == 21)
            {
                for (int k1 = 0; k1 < 3; k1++)
                {
                    float f5 = 0.0625F;

                    if (k1 == 0)
                    {
                        par1Block.SetBlockBounds(0.5F - f5, 0.3F, 0.0F, 0.5F + f5, 1.0F, f5 * 2.0F);
                    }

                    if (k1 == 1)
                    {
                        par1Block.SetBlockBounds(0.5F - f5, 0.3F, 1.0F - f5 * 2.0F, 0.5F + f5, 1.0F, 1.0F);
                    }

                    f5 = 0.0625F;

                    if (k1 == 2)
                    {
                        par1Block.SetBlockBounds(0.5F - f5, 0.5F, 0.0F, 0.5F + f5, 1.0F - f5, 1.0F);
                    }

                    //GL.Translate(-0.5F, -0.5F, -0.5F);
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, -1F, 0.0F);
                    RenderBottomFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(0));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 1.0F, 0.0F);
                    RenderTopFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(1));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, -1F);
                    RenderEastFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(2));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(0.0F, 0.0F, 1.0F);
                    RenderWestFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(3));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(-1F, 0.0F, 0.0F);
                    RenderNorthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(4));
                    tessellator.Draw();
                    tessellator.StartDrawingQuads();
                    tessellator.SetNormal(1.0F, 0.0F, 0.0F);
                    RenderSouthFace(par1Block, 0.0F, 0.0F, 0.0F, par1Block.GetBlockTextureFromSide(5));
                    tessellator.Draw();
                    //GL.Translate(0.5F, 0.5F, 0.5F);
                }

                par1Block.SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
            }
        }

        ///<summary>
        /// Checks to see if the item's render type indicates that it should be rendered as a regular block or not.
        ///</summary>
        public static bool RenderItemIn3d(int par0)
        {
            if (par0 == 0)
            {
                return true;
            }

            if (par0 == 13)
            {
                return true;
            }

            if (par0 == 10)
            {
                return true;
            }

            if (par0 == 11)
            {
                return true;
            }

            if (par0 == 27)
            {
                return true;
            }

            if (par0 == 22)
            {
                return true;
            }

            if (par0 == 21)
            {
                return true;
            }

            return par0 == 16;
        }
    }
}