using System;

namespace TetrisCS
{
	public struct Tetromino
	{
		private string shape;
		private ConsoleColor colour;
		
		public sbyte x;
		public sbyte y;
		public short rotation;
		
		public string Type { get { return shape; } }
		public ConsoleColor Colour { get { return colour; } }
		
		public Tetromino(Shape s, char glyph = DEFAULT_GLYPH, ConsoleColor c = ConsoleColor.Gray)
		{
			x = 0;
			y = 0;
			rotation = 0;
			shape = Get(s, glyph);
			colour = c;
		}
		
		public enum Shape { I, J, L, O, S, T, Z };
		
		const char DEFAULT_GLYPH = 'X';
		const short MAX_ROTATION = 270;
		public const byte LENGTH = 4;
		
		private static string Get(Shape s, char glyph)
		{
			if (glyph == ' ')
				glyph = DEFAULT_GLYPH;
			
			switch (s) 
			{
				default:
					return "  X   X   X   X ".Replace(DEFAULT_GLYPH, glyph);
					
				case Tetromino.Shape.J:
					return "  X  XX  X      ".Replace(DEFAULT_GLYPH, glyph);
						
				case Tetromino.Shape.L:
					return " X   XX   X     ".Replace(DEFAULT_GLYPH, glyph);
						
				case Tetromino.Shape.O:
					return "     XX  XX     ".Replace(DEFAULT_GLYPH, glyph);
					
				case Tetromino.Shape.S:
					return "  X  XX   X     ".Replace(DEFAULT_GLYPH, glyph);
					
				case Tetromino.Shape.T:
					return "     XX   X   X ".Replace(DEFAULT_GLYPH, glyph);
					
				case Tetromino.Shape.Z:
					return "     XX  X   X  ".Replace(DEFAULT_GLYPH, glyph);
			}
		}
		
		public static byte GetIndexByRotation(byte x, byte y, ref short rotation)
		{
			if (rotation > MAX_ROTATION)
				rotation = 0;
			
			switch (rotation) 
			{
				case 90:
					return (byte)(12 + y - (x << 2));
					
				case 180:
					return (byte)(15 - (y << 2) - x);
					
				case MAX_ROTATION:
					return (byte)(3 - y + (x << 2));
					
				default:
					return (byte)((y << 2) + x);
			}
		}
	}
}
