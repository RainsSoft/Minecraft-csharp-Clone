using System;

namespace net.minecraft.src
{
	public class WorldGenBigTree : WorldGenerator
	{
		static readonly sbyte[] OtherCoordPairs = {2, 0, 0, 1, 2, 1};

		/// <summary>
		/// random seed for GenBigTree </summary>
		Random Rand;

		/// <summary>
		/// Reference to the World object. </summary>
		World WorldObj;
		int[] BasePos = {0, 0, 0};
		int HeightLimit;
		int Height;
		double HeightAttenuation;
		double BranchDensity;
		double BranchSlope;
		double ScaleWidth;
		double LeafDensity;

		/// <summary>
		/// Currently always 1, can be set to 2 in the class constructor to generate a double-sized tree trunk for big trees.
		/// </summary>
		int TrunkSize;

		/// <summary>
		/// Sets the limit of the random value used to initialize the height limit.
		/// </summary>
		int HeightLimitLimit;

		/// <summary>
		/// Sets the distance limit for how far away the generator will populate leaves from the base leaf node.
		/// </summary>
		int LeafDistanceLimit;
		int[][] LeafNodes;

		public WorldGenBigTree(bool par1) : base(par1)
		{
			Rand = new Random();
			HeightLimit = 0;
			HeightAttenuation = 0.61799999999999999D;
			BranchDensity = 1.0D;
			BranchSlope = 0.38100000000000001D;
			ScaleWidth = 1.0D;
			LeafDensity = 1.0D;
			TrunkSize = 1;
			HeightLimitLimit = 12;
			LeafDistanceLimit = 4;
		}

		/// <summary>
		/// Generates a list of leaf nodes for the tree, to be populated by generateLeaves.
		/// </summary>
		public virtual void GenerateLeafNodeList()
		{
			Height = (int)((double)HeightLimit * HeightAttenuation);

			if (Height >= HeightLimit)
			{
				Height = HeightLimit - 1;
			}

			int i = (int)(1.3819999999999999D + Math.Pow((LeafDensity * (double)HeightLimit) / 13D, 2D));

			if (i < 1)
			{
				i = 1;
			}

			int[][] ai = JavaHelper.ReturnRectangularArray<int>(i * HeightLimit, 4);
			int j = (BasePos[1] + HeightLimit) - LeafDistanceLimit;
			int k = 1;
			int l = BasePos[1] + Height;
			int i1 = j - BasePos[1];
			ai[0][0] = BasePos[0];
			ai[0][1] = j;
			ai[0][2] = BasePos[2];
			ai[0][3] = l;
			j--;

			while (i1 >= 0)
			{
				int j1 = 0;
				float f = LayerSize(i1);

				if (f < 0.0F)
				{
					j--;
					i1--;
				}
				else
				{
					double d = 0.5D;

					for (; j1 < i; j1++)
					{
						double d1 = ScaleWidth * ((double)f * ((double)Rand.NextFloat() + 0.32800000000000001D));
						double d2 = (double)Rand.NextFloat() * 2D * Math.PI;
						int k1 = MathHelper2.Floor_double(d1 * Math.Sin(d2) + (double)BasePos[0] + d);
						int l1 = MathHelper2.Floor_double(d1 * Math.Cos(d2) + (double)BasePos[2] + d);
						int[] ai1 = {k1, j, l1};
						int[] ai2 = {k1, j + LeafDistanceLimit, l1};

						if (CheckBlockLine(ai1, ai2) != -1)
						{
							continue;
						}

						int[] ai3 = {BasePos[0], BasePos[1], BasePos[2]};
						double d3 = Math.Sqrt(Math.Pow(Math.Abs(BasePos[0] - ai1[0]), 2D) + Math.Pow(Math.Abs(BasePos[2] - ai1[2]), 2D));
						double d4 = d3 * BranchSlope;

						if ((double)ai1[1] - d4 > (double)l)
						{
							ai3[1] = l;
						}
						else
						{
							ai3[1] = (int)((double)ai1[1] - d4);
						}

						if (CheckBlockLine(ai3, ai1) == -1)
						{
							ai[k][0] = k1;
							ai[k][1] = j;
							ai[k][2] = l1;
							ai[k][3] = ai3[1];
							k++;
						}
					}

					j--;
					i1--;
				}
			}

			LeafNodes = JavaHelper.ReturnRectangularArray<int>(k, 4);
			Array.Copy(ai, 0, LeafNodes, 0, k);
		}

		public virtual void GenTreeLayer(int par1, int par2, int par3, float par4, sbyte par5, int par6)
		{
			int i = (int)((double)par4 + 0.61799999999999999D);
			sbyte byte0 = OtherCoordPairs[par5];
			sbyte byte1 = OtherCoordPairs[par5 + 3];
			int[] ai = {par1, par2, par3};
			int[] ai1 = {0, 0, 0};
			int j = -i;
			int k = -i;
			ai1[par5] = ai[par5];

			for (; j <= i; j++)
			{
				ai1[byte0] = ai[byte0] + j;

				for (int l = -i; l <= i;)
				{
					double d = Math.Sqrt(Math.Pow((double)Math.Abs(j) + 0.5D, 2D) + Math.Pow((double)Math.Abs(l) + 0.5D, 2D));

					if (d > (double)par4)
					{
						l++;
					}
					else
					{
						ai1[byte1] = ai[byte1] + l;
						int i1 = WorldObj.GetBlockId(ai1[0], ai1[1], ai1[2]);

						if (i1 != 0 && i1 != 18)
						{
							l++;
						}
						else
						{
							SetBlockAndMetadata(WorldObj, ai1[0], ai1[1], ai1[2], par6, 0);
							l++;
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the rough size of a layer of the tree.
		/// </summary>
		public virtual float LayerSize(int par1)
		{
			if ((double)par1 < (double)(float)HeightLimit * 0.29999999999999999D)
			{
				return -1.618F;
			}

			float f = (float)HeightLimit / 2.0F;
			float f1 = (float)HeightLimit / 2.0F - (float)par1;
			float f2;

			if (f1 == 0.0F)
			{
				f2 = f;
			}
			else if (Math.Abs(f1) >= f)
			{
				f2 = 0.0F;
			}
			else
			{
				f2 = (float)Math.Sqrt(Math.Pow(Math.Abs(f), 2D) - Math.Pow(Math.Abs(f1), 2D));
			}

			f2 *= 0.5F;
			return f2;
		}

		public virtual float LeafSize(int par1)
		{
			if (par1 < 0 || par1 >= LeafDistanceLimit)
			{
				return -1F;
			}

			return par1 != 0 && par1 != LeafDistanceLimit - 1 ? 3F : 2.0F;
		}

		/// <summary>
		/// Generates the leaves surrounding an individual entry in the leafNodes list.
		/// </summary>
		public virtual void GenerateLeafNode(int par1, int par2, int par3)
		{
			int i = par2;

			for (int j = par2 + LeafDistanceLimit; i < j; i++)
			{
				float f = LeafSize(i - par2);
				GenTreeLayer(par1, i, par3, f, (sbyte)1, 18);
			}
		}

		/// <summary>
		/// Places a line of the specified block ID into the world from the first coordinate triplet to the second.
		/// </summary>
		public virtual void PlaceBlockLine(int[] par1ArrayOfInteger, int[] par2ArrayOfInteger, int par3)
		{
			int[] ai = {0, 0, 0};
			sbyte byte0 = 0;
			int i = 0;

			for (; byte0 < 3; byte0++)
			{
				ai[byte0] = par2ArrayOfInteger[byte0] - par1ArrayOfInteger[byte0];

				if (Math.Abs(ai[byte0]) > Math.Abs(ai[i]))
				{
					i = byte0;
				}
			}

			if (ai[i] == 0)
			{
				return;
			}

			sbyte byte1 = OtherCoordPairs[i];
			sbyte byte2 = OtherCoordPairs[i + 3];
			sbyte byte3;

			if (ai[i] > 0)
			{
				byte3 = 1;
			}
			else
			{
				byte3 = -1;
			}

			double d = (double)ai[byte1] / (double)ai[i];
			double d1 = (double)ai[byte2] / (double)ai[i];
			int[] ai1 = {0, 0, 0};
			int j = 0;

			for (int k = ai[i] + byte3; j != k; j += byte3)
			{
				ai1[i] = MathHelper2.Floor_double((double)(par1ArrayOfInteger[i] + j) + 0.5D);
				ai1[byte1] = MathHelper2.Floor_double((double)par1ArrayOfInteger[byte1] + (double)j * d + 0.5D);
				ai1[byte2] = MathHelper2.Floor_double((double)par1ArrayOfInteger[byte2] + (double)j * d1 + 0.5D);
				SetBlockAndMetadata(WorldObj, ai1[0], ai1[1], ai1[2], par3, 0);
			}
		}

		/// <summary>
		/// Generates the leaf portion of the tree as specified by the leafNodes list.
		/// </summary>
		public virtual void GenerateLeaves()
		{
			int i = 0;

			for (int j = LeafNodes.Length; i < j; i++)
			{
				int k = LeafNodes[i][0];
				int l = LeafNodes[i][1];
				int i1 = LeafNodes[i][2];
				GenerateLeafNode(k, l, i1);
			}
		}

		/// <summary>
		/// Indicates whether or not a leaf node requires additional wood to be added to preserve integrity.
		/// </summary>
		public virtual bool LeafNodeNeedsBase(int par1)
		{
			return (double)par1 >= (double)HeightLimit * 0.20000000000000001D;
		}

		/// <summary>
		/// Places the trunk for the big tree that is being generated. Able to generate double-sized trunks by changing a
		/// field that is always 1 to 2.
		/// </summary>
		public virtual void GenerateTrunk()
		{
			int i = BasePos[0];
			int j = BasePos[1];
			int k = BasePos[1] + Height;
			int l = BasePos[2];
			int[] ai = {i, j, l};
			int[] ai1 = {i, k, l};
			PlaceBlockLine(ai, ai1, 17);

			if (TrunkSize == 2)
			{
				ai[0]++;
				ai1[0]++;
				PlaceBlockLine(ai, ai1, 17);
				ai[2]++;
				ai1[2]++;
				PlaceBlockLine(ai, ai1, 17);
				ai[0]--;
				ai1[0]--;
				PlaceBlockLine(ai, ai1, 17);
			}
		}

		/// <summary>
		/// Generates additional wood blocks to fill out the bases of different leaf nodes that would otherwise degrade.
		/// </summary>
		public virtual void GenerateLeafNodeBases()
		{
			int i = 0;
			int j = LeafNodes.Length;
			int[] ai = {BasePos[0], BasePos[1], BasePos[2]};

			for (; i < j; i++)
			{
				int[] ai1 = LeafNodes[i];
				int[] ai2 = {ai1[0], ai1[1], ai1[2]};
				ai[1] = ai1[3];
				int k = ai[1] - BasePos[1];

				if (LeafNodeNeedsBase(k))
				{
					PlaceBlockLine(ai, ai2, 17);
				}
			}
		}

		/// <summary>
		/// Checks a line of blocks in the world from the first coordinate to triplet to the second, returning the distance
		/// (in blocks) before a non-air, non-leaf block is encountered and/or the end is encountered.
		/// </summary>
		public virtual int CheckBlockLine(int[] par1ArrayOfInteger, int[] par2ArrayOfInteger)
		{
			int[] ai = {0, 0, 0};
			sbyte byte0 = 0;
			int i = 0;

			for (; byte0 < 3; byte0++)
			{
				ai[byte0] = par2ArrayOfInteger[byte0] - par1ArrayOfInteger[byte0];

				if (Math.Abs(ai[byte0]) > Math.Abs(ai[i]))
				{
					i = byte0;
				}
			}

			if (ai[i] == 0)
			{
				return -1;
			}

			sbyte byte1 = OtherCoordPairs[i];
			sbyte byte2 = OtherCoordPairs[i + 3];
			sbyte byte3;

			if (ai[i] > 0)
			{
				byte3 = 1;
			}
			else
			{
				byte3 = -1;
			}

			double d = (double)ai[byte1] / (double)ai[i];
			double d1 = (double)ai[byte2] / (double)ai[i];
			int[] ai1 = {0, 0, 0};
			int j = 0;
			int k = ai[i] + byte3;

			do
			{
				if (j == k)
				{
					break;
				}

				ai1[i] = par1ArrayOfInteger[i] + j;
				ai1[byte1] = MathHelper2.Floor_double((double)par1ArrayOfInteger[byte1] + (double)j * d);
				ai1[byte2] = MathHelper2.Floor_double((double)par1ArrayOfInteger[byte2] + (double)j * d1);
				int l = WorldObj.GetBlockId(ai1[0], ai1[1], ai1[2]);

				if (l != 0 && l != 18)
				{
					break;
				}

				j += byte3;
			}
			while (true);

			if (j == k)
			{
				return -1;
			}
			else
			{
				return Math.Abs(j);
			}
		}

		/// <summary>
		/// Returns a bool indicating whether or not the current location for the tree, spanning basePos to to the height
		/// limit, is valid.
		/// </summary>
		public virtual bool ValidTreeLocation()
		{
			int[] ai = {BasePos[0], BasePos[1], BasePos[2]};
			int[] ai1 = {BasePos[0], (BasePos[1] + HeightLimit) - 1, BasePos[2]};
			int i = WorldObj.GetBlockId(BasePos[0], BasePos[1] - 1, BasePos[2]);

			if (i != 2 && i != 3)
			{
				return false;
			}

			int j = CheckBlockLine(ai, ai1);

			if (j == -1)
			{
				return true;
			}

			if (j < 6)
			{
				return false;
			}
			else
			{
				HeightLimit = j;
				return true;
			}
		}

		/// <summary>
		/// Rescales the generator settings, only used in WorldGenBigTree
		/// </summary>
		public override void SetScale(double par1, double par3, double par5)
		{
			HeightLimitLimit = (int)(par1 * 12D);

			if (par1 > 0.5D)
			{
				LeafDistanceLimit = 5;
			}

			ScaleWidth = par3;
			LeafDensity = par5;
		}

		public override bool Generate(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			WorldObj = par1World;
			long l = par2Random.Next();
			Rand.SetSeed((int)l);
			BasePos[0] = par3;
			BasePos[1] = par4;
			BasePos[2] = par5;

			if (HeightLimit == 0)
			{
				HeightLimit = 5 + Rand.Next(HeightLimitLimit);
			}

			if (!ValidTreeLocation())
			{
				return false;
			}
			else
			{
				GenerateLeafNodeList();
				GenerateLeaves();
				GenerateTrunk();
				GenerateLeafNodeBases();
				return true;
			}
		}
	}
}