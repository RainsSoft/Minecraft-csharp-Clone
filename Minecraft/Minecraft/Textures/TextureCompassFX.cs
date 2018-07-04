using System;
using System.Drawing;
using System.IO;

namespace net.minecraft.src
{
    public class TextureCompassFX : TextureFX
    {
        /// <summary>
        /// A reference to the Minecraft object. </summary>
        private Minecraft Mc;
        private int[] CompassIconImageData;
        private double Field_4229_i;
        private double Field_4228_j;

        public TextureCompassFX(Minecraft par1Minecraft)
            : base(Item.Compass.GetIconFromDamage(0))
        {
            CompassIconImageData = new int[256];
            Mc = par1Minecraft;
            TileImage = 1;

            try
            {
                //Bitmap bufferedimage = ImageIO.read((typeof(Minecraft)).GetResource("/gui/items.png"));
                int i = (IconIndex % 16) * 16;
                int j = (IconIndex / 16) * 16;
                //bufferedimage.getRGB(i, j, 16, 16, CompassIconImageData, 0, 16);
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.ToString());
                Console.Write(ioexception.StackTrace);
            }
        }

        public override void OnTick()
        {
            for (int i = 0; i < 256; i++)
            {
                int j = CompassIconImageData[i] >> 24 & 0xff;
                int k = CompassIconImageData[i] >> 16 & 0xff;
                int l = CompassIconImageData[i] >> 8 & 0xff;
                int i1 = CompassIconImageData[i] >> 0 & 0xff;

                if (AnaglyphEnabled)
                {
                    int j1 = (k * 30 + l * 59 + i1 * 11) / 100;
                    int k1 = (k * 30 + l * 70) / 100;
                    int l1 = (k * 30 + i1 * 70) / 100;
                    k = j1;
                    l = k1;
                    i1 = l1;
                }

                ImageData[i * 4 + 0] = (byte)k;
                ImageData[i * 4 + 1] = (byte)l;
                ImageData[i * 4 + 2] = (byte)i1;
                ImageData[i * 4 + 3] = (byte)j;
            }

            double d = 0.0F;

            if (Mc.TheWorld != null && Mc.ThePlayer != null)
            {
                ChunkCoordinates chunkcoordinates = Mc.TheWorld.GetSpawnPoint();
                double d2 = (double)chunkcoordinates.PosX - Mc.ThePlayer.PosX;
                double d4 = (double)chunkcoordinates.PosZ - Mc.ThePlayer.PosZ;
                d = ((double)(Mc.ThePlayer.RotationYaw - 90F) * Math.PI) / 180D - Math.Atan2(d4, d2);

                if (!Mc.TheWorld.WorldProvider.Func_48217_e())
                {
                    d = (new Random(1)).NextDouble() * Math.PI * 2D;
                }
            }

            double d1;

            for (d1 = d - Field_4229_i; d1 < -Math.PI; d1 += (Math.PI * 2D))
            {
            }

            for (; d1 >= Math.PI; d1 -= (Math.PI * 2D))
            {
            }

            if (d1 < -1D)
            {
                d1 = -1D;
            }

            if (d1 > 1.0D)
            {
                d1 = 1.0D;
            }

            Field_4228_j += d1 * 0.10000000000000001D;
            Field_4228_j *= 0.80000000000000004D;
            Field_4229_i += Field_4228_j;
            double d3 = Math.Sin(Field_4229_i);
            double d5 = Math.Cos(Field_4229_i);

            for (int i2 = -4; i2 <= 4; i2++)
            {
                int k2 = (int)(8.5D + d5 * (double)i2 * 0.29999999999999999D);
                int i3 = (int)(7.5D - d3 * (double)i2 * 0.29999999999999999D * 0.5D);
                int k3 = i3 * 16 + k2;
                int i4 = 100;
                int k4 = 100;
                int i5 = 100;
                int c = 377;

                if (AnaglyphEnabled)
                {
                    int k5 = (i4 * 30 + k4 * 59 + i5 * 11) / 100;
                    int i6 = (i4 * 30 + k4 * 70) / 100;
                    int k6 = (i4 * 30 + i5 * 70) / 100;
                    i4 = k5;
                    k4 = i6;
                    i5 = k6;
                }

                ImageData[k3 * 4 + 0] = (byte)i4;
                ImageData[k3 * 4 + 1] = (byte)k4;
                ImageData[k3 * 4 + 2] = (byte)i5;
                ImageData[k3 * 4 + 3] = (byte)c;
            }

            for (int j2 = -8; j2 <= 16; j2++)
            {
                int l2 = (int)(8.5D + d3 * (double)j2 * 0.29999999999999999D);
                int j3 = (int)(7.5D + d5 * (double)j2 * 0.29999999999999999D * 0.5D);
                int l3 = j3 * 16 + l2;
                int j4 = j2 < 0 ? 100 : 255;
                int l4 = j2 < 0 ? 100 : 20;
                int j5 = j2 < 0 ? 100 : 20;
                int c1 = 377;

                if (AnaglyphEnabled)
                {
                    int l5 = (j4 * 30 + l4 * 59 + j5 * 11) / 100;
                    int j6 = (j4 * 30 + l4 * 70) / 100;
                    int l6 = (j4 * 30 + j5 * 70) / 100;
                    j4 = l5;
                    l4 = j6;
                    j5 = l6;
                }

                ImageData[l3 * 4 + 0] = (byte)j4;
                ImageData[l3 * 4 + 1] = (byte)l4;
                ImageData[l3 * 4 + 2] = (byte)j5;
                ImageData[l3 * 4 + 3] = (byte)c1;
            }
        }
    }
}