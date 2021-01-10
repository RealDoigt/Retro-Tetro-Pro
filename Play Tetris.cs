using System;
using ConsolePaint;
using System.Threading;

namespace TetrisCS
{
	/// <summary>
	/// Description of Play_Tetris.
	/// </summary>
	public static partial class Tetris
	{
		public static void PlayGame(byte fallingTime, byte level, byte tickDuration, byte cycleDuration)
		{	
			if (level > 0)
				fallingTime = InitFallingDelay(level, cycleDuration, fallingTime);
			
			InitGame((Tetromino.Shape)r.Next(7));
			byte tickCounter = 0;
			int linesUntilNextLevel = level < 10 ? level * 10 + 10 : level < 16 ? 100 : level * 10 - 50;
			int lineCounter = 0;
			
			DrawUI();
			
			new Thread(GetInput).Start();
			
			while (!GameOver)
			{
				field.PaintAt(1, 1);
				
				Console.CursorLeft = 1;
				Console.CursorTop = FIELD_HEIGHT + 3;
				Console.Write(score);
				
				Console.CursorLeft = NEXT_POS_X + 1;
				Console.CursorTop = NEXT_POS_Y + 7;
				Console.Write(level); // Levels
				
				Console.CursorLeft = NEXT_POS_X + 1;
				Console.CursorTop = NEXT_POS_Y + 8;
				Console.Write(lineCounter); // lines Cleared
				
				Console.CursorLeft = NEXT_POS_X + 1;
				Console.CursorTop = NEXT_POS_Y + 9;
				Console.Write("{0}", fallingTime > 9 ? string.Format("{0}", fallingTime) : string.Format("{0} ", fallingTime)); // falling speed in Ticks
				
				
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
				
				Thread.Sleep(tickDuration);
				++tickCounter;
				
				if (tickCounter == fallingTime)
				{
					if (DoesPieceFit(piece.x, (sbyte)(piece.y + 1), ref piece.rotation))
						++piece.y;
					
					else
					{
						// Locking piece into place
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
						
						var linesCleared = ClearLines();
						
						if (linesCleared > 0)
							score += GetScore(linesCleared, level);
						
						lineCounter += linesCleared;
						
						if (lineCounter >= linesUntilNextLevel)
						{
							++level;
							linesUntilNextLevel += 10;
							
							if (level % cycleDuration == 0)
								--fallingTime;
						}
						
						InitGame(nextPiece);
						GameOver = !DoesPieceFit(piece.x, piece.y, ref piece.rotation);
					}
					
					tickCounter = 0;
				}
			}
		}
	}
}
