using System;
using ConsolePaint;

namespace TetrisCS
{
	public static partial class Tetris
	{
		static void DrawUI()
		{
			var border = RectanglePainting.BorderType.Double;
			var colour = ConsoleColor.DarkGreen;
			
			// Playing field
			RectanglePainting.DrawRectangle(border, 0, 0, FIELD_HEIGHT + 1, FIELD_WIDTH + 1, colour);
			
			// Next piece
			RectanglePainting.DrawSquare(border, NEXT_POS_X, NEXT_POS_Y, Tetromino.LENGTH + 2, colour);
			Console.ForegroundColor = ConsoleColor.White;
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y - 1;
			Console.Write("NEXT");
			
			// Level, Lines, speed
			RectanglePainting.DrawRectangle(border, NEXT_POS_X, NEXT_POS_Y + 6, 5, Tetromino.LENGTH + 2, colour);
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 7;
			Console.Write("   L"); // Levels
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 8;
			Console.Write("   C"); // lines Cleared
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 9;
			Console.Write("   T"); // falling speed in Ticks
			
			// Score
			RectanglePainting.DrawRectangle(border, 0, FIELD_HEIGHT + 2, 3, FIELD_WIDTH + 8, ConsoleColor.Yellow);
			Console.CursorLeft = 7;
			Console.CursorTop = FIELD_HEIGHT + 1;
			//Console.ForegroundColor = ConsoleColor.White;
			Console.Write("SCORE");
			Console.CursorTop = FIELD_HEIGHT + 3;
			Console.CursorLeft = 2;
			Console.Write("                 ");
		}
		
		static void DrawPiece(ref Tetromino piece)
		{
			for (byte x = 0; x < Tetromino.LENGTH; ++x)
				for (byte y = 0; y < Tetromino.LENGTH; ++y)
				{ 
					var c = piece.Type[Tetromino.GetIndexByRotation(x, y, ref piece.rotation)];
						
					if (c != ' ')
					{
						Painting.brush = c;
						Painting.DrawCell((short)(piece.x + x + 1), (short)(piece.y + y + 1), piece.Colour);
					}
				}
		}
		
		static void LockPiece(ref Tetromino piece, ComplexConsoleImage field)
		{
			for (byte x = 0; x < Tetromino.LENGTH; ++x)
				for (byte y = 0; y < Tetromino.LENGTH; ++y)
				{
					var c = piece.Type[Tetromino.GetIndexByRotation(x, y, ref piece.rotation)];
								
					if (c != ' ')
					{
						field[(ushort)(piece.x + x), (ushort)(piece.y + y)] = piece.Colour;
						field.SetGlyph((ushort)(piece.x + x), (ushort)(piece.y + y), c);
					}
				}
		}
		
		static void WriteStats(byte score, byte level, byte lineCounter, byte fallingTime)
		{
			Console.CursorLeft = 1;
			Console.CursorTop = FIELD_HEIGHT + 3;
			Console.Write(score);
				
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 7;
			Console.Write(level); 
				
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 8;
			Console.Write(lineCounter); // lines Cleared
				
			Console.CursorLeft = NEXT_POS_X + 1;
			Console.CursorTop = NEXT_POS_Y + 9;
			Console.Write("{0}", fallingTime > 9 ? string.Format("{0}", fallingTime) : string.Format("{0} ", fallingTime));
			// falling speed in Ticks
		}
	}
}
