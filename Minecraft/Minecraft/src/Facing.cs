namespace net.minecraft.src
{

	public class Facing
	{
		public static readonly int[] FaceToSide = { 1, 0, 3, 2, 5, 4 };
		public static readonly int[] OffsetsXForSide = { 0, 0, 0, 0, -1, 1 };
		public static readonly int[] OffsetsYForSide = { -1, 1, 0, 0, 0, 0 };
		public static readonly int[] OffsetsZForSide = { 0, 0, -1, 1, 0, 0 };

		public Facing()
		{
		}
	}

}