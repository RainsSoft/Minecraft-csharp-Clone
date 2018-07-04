namespace net.minecraft.src
{

	public class Direction
	{
		public static readonly int[] OffsetX = { 0, -1, 0, 1 };
		public static readonly int[] OffsetZ = { 1, 0, -1, 0 };
		public static readonly int[] HeadInvisibleFace = { 3, 4, 2, 5 };
		public static readonly int[] VineGrowth = { -1, -1, 2, 0, 1, 3 };
		public static readonly int[] FootInvisibleFaceRemap = { 2, 3, 0, 1 };
		public static readonly int[] EnderEyeMetaToDirection = { 1, 2, 3, 0 };
		public static readonly int[] Field_35868_g = { 3, 0, 1, 2 };
		public static readonly int[][] BedDirection = { new int[] { 1, 0, 3, 2, 5, 4 }, new int[] { 1, 0, 5, 4, 2, 3 }, new int[] { 1, 0, 2, 3, 4, 5 }, new int[] { 1, 0, 4, 5, 3, 2 } };

		public Direction()
		{
		}
	}

}