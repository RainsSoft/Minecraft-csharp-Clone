using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class Tessellator
	{
		/// <summary>
		/// bool used to check whether quads should be drawn as four triangles. Initialized to true and never changed.
		/// </summary>
		private static bool ConvertQuadsToTriangles = false;

		/// <summary>
		/// bool used to check if we should use vertex buffers. Initialized to false and never changed.
		/// </summary>
		private static bool TryVBO = false;

		/// <summary>
		/// The byte buffer used for GL allocation. </summary>
		private Buffer<byte> ByteBuffer;

		/// <summary>
		/// The same memory as byteBuffer, but referenced as an integer buffer. </summary>
		private Buffer<int> IntBuffer;

		/// <summary>
		/// The same memory as byteBuffer, but referenced as an float buffer. </summary>
		private Buffer<float> FloatBuffer;

		/// <summary>
		/// Short buffer </summary>
		private Buffer<short> ShortBuffer;
		private int[] RawBuffer;

		/// <summary>
		/// The number of vertices to be drawn in the next draw call. Reset to 0 between draw calls.
		/// </summary>
		private int VertexCount;

		/// <summary>
		/// The first coordinate to be used for the texture. </summary>
		private float TextureU;

		/// <summary>
		/// The second coordinate to be used for the texture. </summary>
		private float TextureV;

		private int Brightness;

		/// <summary>
		/// The color (RGBA) value to be used for the following draw call. </summary>
		private int Color;

		/// <summary>
		/// Whether the current draw object for this tessellator has color values.
		/// </summary>
		private bool HasColor;

		/// <summary>
		/// Whether the current draw object for this tessellator has texture coordinates.
		/// </summary>
		private bool HasTexture;
		private bool HasBrightness;

		/// <summary>
		/// Whether the current draw object for this tessellator has normal values.
		/// </summary>
		private bool HasNormals;

		/// <summary>
		/// The index into the raw buffer to be used for the next data. </summary>
		private int RawBufferIndex;

		/// <summary>
		/// The number of vertices manually added to the given draw call. This differs from vertexCount because it adds extra
		/// vertices when converting quads to triangles.
		/// </summary>
		private int AddedVertices;

		/// <summary>
		/// Disables all color information for the following draw call. </summary>
		private bool IsColorDisabled;

		/// <summary>
		/// The draw mode currently being used by the tessellator. </summary>
		private int DrawMode;

		/// <summary>
		/// An offset to be applied along the x-axis for all vertices in this draw call.
		/// </summary>
		private float XOffset;

		/// <summary>
		/// An offset to be applied along the y-axis for all vertices in this draw call.
		/// </summary>
		private float YOffset;

		/// <summary>
		/// An offset to be applied along the z-axis for all vertices in this draw call.
		/// </summary>
		private float ZOffset;

		/// <summary>
		/// The normal to be applied to the face being drawn. </summary>
		private int Normal;

		/// <summary>
		/// The static instance of the Tessellator. </summary>
		public static readonly Tessellator Instance = new Tessellator(0x200000);

		/// <summary>
		/// Whether this tessellator is currently in draw mode. </summary>
		private bool IsDrawing;

		/// <summary>
		/// Whether we are currently using VBO or not. </summary>
		private bool UseVBO;

		/// <summary>
		/// An IntBuffer used to store the indices of vertex buffer objects. </summary>
		private Buffer<int> VertexBuffers;

		/// <summary>
		/// The index of the last VBO used. This is used in round-robin fashion, sequentially, through the vboCount vertex
		/// buffers.
		/// </summary>
		private int VboIndex;

		/// <summary>
		/// Number of vertex buffer objects allocated for use. </summary>
		private int VboCount;

		/// <summary>
		/// The size of the buffers used (in integers). </summary>
		private int BufferSize;

		private Tessellator(int par1)
		{
			VertexCount = 0;
			HasColor = false;
			HasTexture = false;
			HasBrightness = false;
			HasNormals = false;
			RawBufferIndex = 0;
			AddedVertices = 0;
			IsColorDisabled = false;
			IsDrawing = false;
			UseVBO = false;
			VboIndex = 0;
			VboCount = 10;
			BufferSize = par1;
            ByteBuffer = new Buffer<byte>(par1 * 4);// GLAllocation.CreateDirectByteBuffer(par1 * 4);
            IntBuffer = new Buffer<int>(par1);// ByteBuffer.asIntBuffer();
            FloatBuffer = new Buffer<float>(par1);// ByteBuffer.asFloatBuffer();
            ShortBuffer = new Buffer<short>(par1 * 2);// ByteBuffer.asShortBuffer();
			RawBuffer = new int[par1];
			UseVBO = TryVBO;// && GLContext.getCapabilities().GL_ARB_vertex_buffer_object;

			if (UseVBO)
			{
                VertexBuffers = new Buffer<int>(VboCount);// GLAllocation.CreateDirectIntBuffer(VboCount);
				//GL.Arb.GenBuffers(0, VertexBuffers);
			}
		}

		/// <summary>
		/// Draws the data set up in this tessellator and resets the state to prepare for new drawing.
		/// </summary>
		public virtual int Draw()
		{
			if (!IsDrawing)
			{
				throw new InvalidOperationException("Not tesselating!");
			}

			IsDrawing = false;

			if (VertexCount > 0)
			{
				IntBuffer.Clear();
				IntBuffer.Put(RawBuffer, 0, RawBufferIndex);
				ByteBuffer.Position = 0;
				ByteBuffer.Limit(RawBufferIndex * 4);

				if (UseVBO)
				{
					VboIndex = (VboIndex + 1) % VboCount;
                    //GL.Arb.BindBuffer(BufferTargetArb.ArrayBuffer, VertexBuffers.Get(VboIndex));
                    //GL.Arb.BufferData(BufferTargetArb.ArrayBuffer, (IntPtr)ByteBuffer.Length, ref ByteBuffer, BufferUsageArb.StreamDraw);
				}

				if (HasTexture)
				{
					if (UseVBO)
					{
						//GL.TexCoordPointer(2, TexCoordPointerType.Float, 32, 12);
					}
					else
					{
						FloatBuffer.Position = 3;
						//GL.TexCoordPointer(2, TexCoordPointerType.Float, 32, FloatBuffer.Data);
					}

					//GL.EnableClientState(ArrayCap.TextureCoordArray);
				}

				if (HasBrightness)
				{
					OpenGlHelper.SetClientActiveTexture(OpenGlHelper.LightmapTexUnit);

					if (UseVBO)
					{
						//GL.TexCoordPointer(2, TexCoordPointerType.Short, 32, 28);
					}
					else
					{
						ShortBuffer.Position = 14;
						//GL.TexCoordPointer(2, TexCoordPointerType.Short, 32, ref ShortBuffer);
					}

                    //GL.EnableClientState(ArrayCap.TextureCoordArray);
					OpenGlHelper.SetClientActiveTexture(OpenGlHelper.DefaultTexUnit);
				}

				if (HasColor)
				{
					if (UseVBO)
					{
						//GL.ColorPointer(4, ColorPointerType.UnsignedByte, 32, 20);
					}
					else
					{
						ByteBuffer.Position = 20;
						//GL.ColorPointer(4, ColorPointerType.Byte, 32, ByteBuffer.Data);
					}

					//GL.EnableClientState(ArrayCap.ColorArray);
				}

				if (HasNormals)
				{
					if (UseVBO)
					{
						//GL.NormalPointer(NormalPointerType.Byte, 32, 24);
					}
					else
					{
						ByteBuffer.Position = 24;
						//GL.NormalPointer(NormalPointerType.Byte, 32, ref ByteBuffer);
					}

					//GL.EnableClientState(ArrayCap.NormalArray);
				}

				if (UseVBO)
				{
					//GL.VertexPointer(3, VertexPointerType.Float, 32, 0);
				}
				else
				{
					FloatBuffer.Position = 0;
					//GL.VertexPointer(3, VertexPointerType.Float, 32, FloatBuffer.Data);
				}

				//GL.EnableClientState(ArrayCap.VertexArray);

				if (DrawMode == 7 && ConvertQuadsToTriangles)
				{
					//GL.DrawArrays(BeginMode.Triangles, 0, VertexCount);
				}
				else
				{
					//GL.DrawArrays((BeginMode)DrawMode, 0, VertexCount);
				}

				//GL.DisableClientState(ArrayCap.VertexArray);

				if (HasTexture)
				{
					//GL.DisableClientState(ArrayCap.TextureCoordArray);
				}

				if (HasBrightness)
				{
					OpenGlHelper.SetClientActiveTexture(OpenGlHelper.LightmapTexUnit);
					//GL.DisableClientState(ArrayCap.TextureCoordArray);
					OpenGlHelper.SetClientActiveTexture(OpenGlHelper.DefaultTexUnit);
				}

				if (HasColor)
				{
					//GL.DisableClientState(ArrayCap.ColorArray);
				}

				if (HasNormals)
				{
					//GL.DisableClientState(ArrayCap.NormalArray);
				}
			}

			int i = RawBufferIndex * 4;
			Reset();
			return i;
		}

		/// <summary>
		/// Clears the tessellator state in preparation for new drawing.
		/// </summary>
		private void Reset()
		{
			VertexCount = 0;
			ByteBuffer.Clear();
			RawBufferIndex = 0;
			AddedVertices = 0;
		}

		/// <summary>
		/// Sets draw mode in the tessellator to draw quads.
		/// </summary>
		public virtual void StartDrawingQuads()
		{
			StartDrawing(7);
		}

		/// <summary>
		/// Resets tessellator state and prepares for drawing (with the specified draw mode).
		/// </summary>
		public virtual void StartDrawing(int par1)
		{
			if (IsDrawing)
			{
				throw new InvalidOperationException("Already tesselating!");
			}
			else
			{
				IsDrawing = true;
				Reset();
				DrawMode = par1;
				HasNormals = false;
				HasColor = false;
				HasTexture = false;
				HasBrightness = false;
				IsColorDisabled = false;
				return;
			}
		}

		/// <summary>
		/// Sets the texture coordinates.
		/// </summary>
		public virtual void SetTextureUV(float u, float v)
		{
			HasTexture = true;
			TextureU = u;
			TextureV = v;
		}

		public virtual void SetBrightness(int par1)
		{
			HasBrightness = true;
			Brightness = par1;
		}

		/// <summary>
		/// Sets the RGB values as specified, converting from floats between 0 and 1 to integers from 0-255.
		/// </summary>
		public virtual void SetColorOpaque_F(float par1, float par2, float par3)
		{
			SetColorOpaque((int)(par1 * 255F), (int)(par2 * 255F), (int)(par3 * 255F));
		}

		/// <summary>
		/// Sets the RGBA values for the color, converting from floats between 0 and 1 to integers from 0-255.
		/// </summary>
		public virtual void SetColorRGBA_F(float par1, float par2, float par3, float par4)
		{
			SetColorRGBA((int)(par1 * 255F), (int)(par2 * 255F), (int)(par3 * 255F), (int)(par4 * 255F));
		}

		/// <summary>
		/// Sets the RGB values as specified, and sets alpha to opaque.
		/// </summary>
		public virtual void SetColorOpaque(int par1, int par2, int par3)
		{
			SetColorRGBA(par1, par2, par3, 255);
		}

		/// <summary>
		/// Sets the RGBA values for the color. Also clamps them to 0-255.
		/// </summary>
		public virtual void SetColorRGBA(int par1, int par2, int par3, int par4)
		{
			if (IsColorDisabled)
			{
				return;
			}

			if (par1 > 255)
			{
				par1 = 255;
			}

			if (par2 > 255)
			{
				par2 = 255;
			}

			if (par3 > 255)
			{
				par3 = 255;
			}

			if (par4 > 255)
			{
				par4 = 255;
			}

			if (par1 < 0)
			{
				par1 = 0;
			}

			if (par2 < 0)
			{
				par2 = 0;
			}

			if (par3 < 0)
			{
				par3 = 0;
			}

			if (par4 < 0)
			{
				par4 = 0;
			}

			HasColor = true;

			if (BitConverter.IsLittleEndian)
			{
				Color = par4 << 24 | par3 << 16 | par2 << 8 | par1;
			}
			else
			{
				Color = par1 << 24 | par2 << 16 | par3 << 8 | par4;
			}
		}

		/// <summary>
		/// Adds a vertex specifying both x,y,z and the texture u,v for it.
		/// </summary>
		public void AddVertexWithUV(float x, float y, float z, float u, float v)
		{
			SetTextureUV(u, v);
			AddVertex(x, y, z);
		}

        public void AddVertexWithUV(double x, double y, double z, double u, double v)
		{
            SetTextureUV((float)u, (float)v);
            AddVertex((float)x, (float)y, (float)z);
		}

		/// <summary>
		/// Adds a vertex with the specified x,y,z to the current draw call. It will trigger a draw() if the buffer gets
		/// full.
		/// </summary>
		public void AddVertex(float x, float y, float z)
		{
			AddedVertices++;

			if (DrawMode == 7 && ConvertQuadsToTriangles && AddedVertices % 4 == 0)
			{
				for (int i = 0; i < 2; i++)
				{
					int j = 8 * (3 - i);

					if (HasTexture)
					{
						RawBuffer[RawBufferIndex + 3] = RawBuffer[(RawBufferIndex - j) + 3];
						RawBuffer[RawBufferIndex + 4] = RawBuffer[(RawBufferIndex - j) + 4];
					}

					if (HasBrightness)
					{
						RawBuffer[RawBufferIndex + 7] = RawBuffer[(RawBufferIndex - j) + 7];
					}

					if (HasColor)
					{
						RawBuffer[RawBufferIndex + 5] = RawBuffer[(RawBufferIndex - j) + 5];
					}

					RawBuffer[RawBufferIndex + 0] = RawBuffer[(RawBufferIndex - j) + 0];
					RawBuffer[RawBufferIndex + 1] = RawBuffer[(RawBufferIndex - j) + 1];
					RawBuffer[RawBufferIndex + 2] = RawBuffer[(RawBufferIndex - j) + 2];
					VertexCount++;
					RawBufferIndex += 8;
				}
			}

			if (HasTexture)
			{
				RawBuffer[RawBufferIndex + 3] = BitConverter.ToInt32(BitConverter.GetBytes((float)TextureU), 0);
				RawBuffer[RawBufferIndex + 4] = BitConverter.ToInt32(BitConverter.GetBytes((float)TextureV), 0);
			}

			if (HasBrightness)
			{
				RawBuffer[RawBufferIndex + 7] = Brightness;
			}

			if (HasColor)
			{
				RawBuffer[RawBufferIndex + 5] = Color;
			}

			if (HasNormals)
			{
				RawBuffer[RawBufferIndex + 6] = Normal;
			}

			RawBuffer[RawBufferIndex + 0] = BitConverter.ToInt32(BitConverter.GetBytes((float)(x + XOffset)), 0);
			RawBuffer[RawBufferIndex + 1] = BitConverter.ToInt32(BitConverter.GetBytes((float)(y + YOffset)), 0);
			RawBuffer[RawBufferIndex + 2] = BitConverter.ToInt32(BitConverter.GetBytes((float)(z + ZOffset)), 0);
			RawBufferIndex += 8;
			VertexCount++;

			if (VertexCount % 4 == 0 && RawBufferIndex >= BufferSize - 32)
			{
				Draw();
				IsDrawing = true;
			}
		}

        public void AddVertex(double x, double y, double z)
        {
            AddVertex((float)x, (float)y, (float)z);
        }

		/// <summary>
		/// Sets the color to the given opaque value (stored as byte values packed in an integer).
		/// </summary>
		public void SetColorOpaque_I(int par1)
		{
			int i = par1 >> 16 & 0xff;
			int j = par1 >> 8 & 0xff;
			int k = par1 & 0xff;
			SetColorOpaque(i, j, k);
		}

		/// <summary>
		/// Sets the color to the given color (packed as bytes in integer) and alpha values.
		/// </summary>
		public void SetColorRGBA_I(int par1, int par2)
		{
			int i = par1 >> 16 & 0xff;
			int j = par1 >> 8 & 0xff;
			int k = par1 & 0xff;
			SetColorRGBA(i, j, k, par2);
		}

		/// <summary>
		/// Disables colors for the current draw call.
		/// </summary>
		public void DisableColor()
		{
			IsColorDisabled = true;
		}

		/// <summary>
		/// Sets the normal for the current draw call.
		/// </summary>
		public void SetNormal(float par1, float par2, float par3)
		{
			HasNormals = true;
			byte byte0 = (byte)(par1 * 127F);
			byte byte1 = (byte)(par2 * 127F);
			byte byte2 = (byte)(par3 * 127F);
			Normal = byte0 & 0xff | (byte1 & 0xff) << 8 | (byte2 & 0xff) << 16;
		}

		/// <summary>
		/// Sets the translation for all vertices in the current draw call.
		/// </summary>
		public void SetTranslation(float par1, float par3, float par5)
		{
			XOffset = par1;
			YOffset = par3;
			ZOffset = par5;
		}

		public void SetTranslation(double par1, double par3, double par5)
		{
            XOffset = (float)par1;
            YOffset = (float)par3;
            ZOffset = (float)par5;
		}

		/// <summary>
		/// Offsets the translation for all vertices in the current draw call.
		/// </summary>
		public void AddTranslation(float par1, float par2, float par3)
		{
			XOffset += par1;
			YOffset += par2;
			ZOffset += par3;
		}
	}
}