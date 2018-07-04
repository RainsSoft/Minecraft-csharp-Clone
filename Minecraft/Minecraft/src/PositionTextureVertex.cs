namespace net.minecraft.src
{

	public class PositionTextureVertex
	{
		public Vec3D Vector3D;
		public float TexturePositionX;
		public float TexturePositionY;

		public PositionTextureVertex(float par1, float par2, float par3, float par4, float par5) : this(Vec3D.CreateVectorHelper(par1, par2, par3), par4, par5)
		{
		}

		public virtual PositionTextureVertex SetTexturePosition(float par1, float par2)
		{
			return new PositionTextureVertex(this, par1, par2);
		}

		public PositionTextureVertex(PositionTextureVertex par1PositionTextureVertex, float par2, float par3)
		{
			Vector3D = par1PositionTextureVertex.Vector3D;
			TexturePositionX = par2;
			TexturePositionY = par3;
		}

		public PositionTextureVertex(Vec3D par1Vec3D, float par2, float par3)
		{
			Vector3D = par1Vec3D;
			TexturePositionX = par2;
			TexturePositionY = par3;
		}
	}

}