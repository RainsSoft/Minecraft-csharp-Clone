using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class GLAllocation
	{
		/// <summary>
		/// An ArrayList that stores the first index and the length of each display list.
		/// </summary>
        private static List<int> DisplayLists = new List<int>();

		/// <summary>
		/// An ArrayList that stores all the generated texture names. </summary>
        private static List<int> TextureNames = new List<int>();

		public GLAllocation()
		{
		}

		/// <summary>
		/// Generates the specified number of display lists and returns the first index.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static int GenerateDisplayLists(int par0)
		{
            int i = 0;//GL.GenLists(par0);
			DisplayLists.Add(i);
			DisplayLists.Add(par0);
			return i;
		}

		/// <summary>
		/// Generates texture names and stores them in the specified buffer.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void GenerateTextureNames(Buffer<int> par0IntBuffer)
		{
			//GL.GenTextures(0, par0IntBuffer);

			for (int i = par0IntBuffer.Position; i < par0IntBuffer.Length; i++)
			{
				TextureNames.Add(par0IntBuffer.Get(i));
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void DeleteDisplayLists(int par0)
		{
			int i = DisplayLists.IndexOf(par0);
			//GL.DeleteLists(DisplayLists[i], DisplayLists[i + 1]);
			DisplayLists.Remove(i);
			DisplayLists.Remove(i);
		}

		/// <summary>
		/// Deletes all textures and display lists. Called when Minecraft is shutdown to free up resources.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void DeleteTexturesAndDisplayLists()
		{
			for (int i = 0; i < DisplayLists.Count; i += 2)
			{
				//GL.DeleteLists(DisplayLists[i], DisplayLists[i + 1]);
			}

            Buffer<int> intbuffer = new Buffer<int>(TextureNames.Count);// CreateDirectIntBuffer(TextureNames.Count);
			intbuffer.Flip();
			//GL.DeleteTextures(0, intbuffer);

			for (int j = 0; j < TextureNames.Count; j++)
			{
				intbuffer.Put(TextureNames[j]);
			}

			intbuffer.Flip();
			//GL.DeleteTextures(0, intbuffer);
			DisplayLists.Clear();
			TextureNames.Clear();
		}
        /*
		/// <summary>
		/// Creates and returns a direct byte buffer with the specified capacity. Applies native ordering to speed up access.
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static ByteBuffer CreateDirectByteBuffer(int par0)
		{
			ByteBuffer bytebuffer = ByteBuffer.AllocateDirect(par0).Order(ByteOrder.NativeOrder());
			return bytebuffer;
		}
        
		/// <summary>
		/// Creates and returns a direct int buffer with the specified capacity. Applies native ordering to speed up access.
		/// </summary>
		public static IntBuffer CreateDirectIntBuffer(int par0)
		{
			return CreateDirectByteBuffer(par0 << 2).asIntBuffer();
		}
        
		/// <summary>
		/// Creates and returns a direct float buffer with the specified capacity. Applies native ordering to speed up
		/// access.
		/// </summary>
		public static FloatBuffer CreateDirectFloatBuffer(int par0)
		{
			return CreateDirectByteBuffer(par0 << 2).asFloatBuffer();
		}*/
	}
}