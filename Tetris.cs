using System;
using ConsolePaint;
using System.Threading;
// using System.Media;

namespace TetrisCS
{
	public static partial class Tetris
	{
		// Was starting to be way too many params, so I put them here. I know, I'll rot in hell for this.
		public static bool enableColour = true;
		public static bool enableChars = false;
//		public static bool enableMusic = false;
		
		static ConsoleKey input = 0;
		static Random r = new Random();
		
		const byte FIELD_WIDTH = 12; // in characters
		const byte FIELD_HEIGHT = 18; // in characters
		const char EMPTY = '\0';
		const byte NEXT_POS_X = FIELD_WIDTH + 2;
		const byte NEXT_POS_Y = 2;
		
		static ComplexConsoleImage field = new ComplexConsoleImage(FIELD_HEIGHT - 1, FIELD_WIDTH - 1);
		
		static Tetromino piece;
		static bool GameOver = false;
		static bool isPaused = false;
		static Tetromino.Shape nextPiece;
		
		static uint score;
		
//		static void PlayMusic()
//		{
//			SoundPlayer sp = new SoundPlayer();
//			
//			for (byte count = (byte)r.Next(1, 11); !GameOver; ++count)
//			{
//				sp.SoundLocation = string.Format("music/{0}.wav", count);
//				sp.PlaySync();
//				
//				if (count == 10)
//					count = 0;
//			}
//		}

		static void GetInput()
		{
			while (!GameOver) 
			{
				input = Console.ReadKey(true).Key;
				
				if (!isPaused)
				{
					if (input == ConsoleKey.DownArrow && DoesPieceFit(piece.x, (sbyte)(piece.y + 1), ref piece.rotation))
						++piece.y;
				
					else if (input == ConsoleKey.LeftArrow && DoesPieceFit((sbyte)(piece.x - 1), piece.y, ref piece.rotation))
						--piece.x;
				
					else if (input == ConsoleKey.RightArrow && DoesPieceFit((sbyte)(piece.x + 1), piece.y, ref piece.rotation))
						++piece.x;
				
					else if (input == ConsoleKey.Z)
					{
						var nextRotation = (short)(piece.rotation + 90);
					
						if (DoesPieceFit(piece.x, piece.y, ref nextRotation))
							piece.rotation = nextRotation;
					}
				}
				
				if (input == ConsoleKey.Spacebar)
					isPaused = !isPaused;
				
				Console.Beep(800, 20);
			}
		}
		
		static bool DoesPieceFit(sbyte posX, sbyte posY, ref short rotation)
		{
			for (byte x = 0; x < Tetromino.LENGTH; ++x)
				for (byte y = 0; y < Tetromino.LENGTH; ++y)
				{
					var pieceIndex = Tetromino.GetIndexByRotation(x, y, ref rotation);
					var fieldIndexX = posX + x;
					var fieldIndexY = posY + y;
					
					if (fieldIndexY >= 0 && fieldIndexY < field.Height && fieldIndexX >= 0 && fieldIndexX < field.Width)
					{
						var fieldValue = field.GetGlyph((ushort)fieldIndexX, (ushort)fieldIndexY);
						
						if (fieldValue != EMPTY && piece.Type[pieceIndex] != ' ')
							return false;
					}
					
					if (piece.Type[pieceIndex] != ' ' && (fieldIndexX >= field.Width || fieldIndexX < 0))
						return false;
					
					if (piece.Type[pieceIndex] != ' ' && fieldIndexY >= field.Height)
						return false;
				}
			
			return true;
		}
		
		static void InitGame(Tetromino.Shape next)
		{
			string temp;
			
			if (enableChars)
				temp = "☻♥♫";
			
			else
				temp = "█";
			
			// colours which are below 7 are all dark colours
			piece = new Tetromino(next, temp[r.Next(temp.Length)], enableColour ? (ConsoleColor)r.Next(9, 16) : ConsoleColor.Gray);
			piece.x = FIELD_WIDTH - 1 >> 1;
			nextPiece = (Tetromino.Shape)r.Next(7);
			DrawNextPiece();
		}
		
		static void DrawNextPiece()
		{
			byte posX = NEXT_POS_X + 1, posY = NEXT_POS_Y + 1;
			var nextTetrad = new Tetromino(nextPiece);
			
			for (byte x = 0; x < Tetromino.LENGTH; ++x)
				for (byte y = 0; y < Tetromino.LENGTH; ++y)
					{
						Painting.brush = nextTetrad.Type[Tetromino.GetIndexByRotation(x, y, ref piece.rotation)];
						Painting.DrawCell((short)(posX + x), (short)(posY + y), nextTetrad.Colour);
					}
		}
		
		public static void Reset()
		{
			field = new ComplexConsoleImage(FIELD_HEIGHT - 1, FIELD_WIDTH - 1);
			GameOver = false;
			score = 0;
		}
		
		static byte ClearLines()
		{
			byte clearCount = 0;
			
			for (byte y = (byte)piece.y; y < FIELD_HEIGHT - 1; ++y)
				for (byte x = 0; x < FIELD_WIDTH - 1; ++x)
				{
					if (field.GetGlyph(x, y) == EMPTY)
						break;
					
					else if (x == FIELD_WIDTH - 2)
					{
						RemoveLine(y);
						++clearCount;
					}
				}
			
			return clearCount;
		}
		
		static void RemoveLine(byte posY)
		{
			for (byte y = posY; y > 0; --y) 
				for (byte x = 0; x < FIELD_WIDTH - 1; ++x)
				{
					if (y == 0)
					{
						field.SetGlyph(x, y, EMPTY);
						field[x, y] = ConsoleColor.Black;
					}
					
					else
					{
						field.SetGlyph(x, y, field.GetGlyph(x, (ushort)(y - 1)));
						field[x, y] = field[x, (ushort)(y - 1)];
					}
				}
		}
		
		static uint GetScore(byte linesCleared, byte level)
		{
			Func<short, uint> getScore = (multiplier) => (uint)((level + 1) * multiplier);
			
			switch (linesCleared)
			{
				case 2:
					return getScore(100);
					
				case 3:
					return getScore(300);
					
				case 4:
					Console.Beep(800, 100);
					Console.Beep(1000, 200);
					Console.Beep(800, 500);
					return getScore(1200);
					
				default:
					return getScore(40);
			}
		}
		
		static byte InitFallingDelay(byte level, byte cycle, byte delay)
		{
			for (byte count = 1; count != level && delay > 1; ++count)
				if (count % cycle == 0)
					--delay;
			
			return delay;
		}
	}
}
