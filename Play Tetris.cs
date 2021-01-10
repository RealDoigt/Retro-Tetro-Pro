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
				
				WriteStats(score, level, lineCounter, fallingTime);
				 

				DrawPiece(ref piece);
				
				Thread.Sleep(tickDuration);
				++tickCounter;
				
				if (tickCounter == fallingTime)
				{
					if (DoesPieceFit(piece.x, (sbyte)(piece.y + 1), ref piece.rotation))
						++piece.y;
					
					else
					{
						// Locking piece into place
						LockPiece(ref piece, field);
						
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
