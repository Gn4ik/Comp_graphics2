using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace computer_graphics_2.Code
{
	public class Bin
	{
		public static int X, Y, Z;
		public static short[] array;

		public Bin() { }

		public void ReadBIN(string path)
		{
			if(File.Exists(path))
			{
				BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open));

				X = binaryReader.ReadInt32();
				Y = binaryReader.ReadInt32();
				Z = binaryReader.ReadInt32();

				int arraySize = X*Y*Z;

				array = new short[arraySize];

				for (int i = 0; i < arraySize; i++)
				{
					array[i] = binaryReader.ReadInt16();
				}
			}
		}

	}
}
