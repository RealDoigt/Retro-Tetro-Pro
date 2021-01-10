using System;
using ConsolePaint;

namespace TetrisCS
{
	class Program
	{
		static Settings settings = Settings.FromDifficulty(Settings.Difficulties.Normal);
		
		public static byte PrintCursor(byte menuX, byte menuY, byte optionQTY, byte choice, Action a)
		{
			Action erasePrevious = () => {
				
				Console.CursorTop = menuY + choice;
				Console.CursorLeft = menuX + 1;
				Console.Write(" ");
			};
			
			Console.ForegroundColor = ConsoleColor.Gray;
			
			Console.Beep(5000, 5);
			
			switch (Console.ReadKey().Key) 
			{
				case ConsoleKey.Enter:
					if (a != null)
						a();
					break;
					
				case ConsoleKey.UpArrow:
					
					if (menuY - 1 >= 0 && choice - 1 >= 0)
					{
						erasePrevious();
						Console.CursorTop = menuY + choice - 1;
						Console.CursorLeft = menuX + 1;
						Console.Write(">");
						
						return (byte)(choice - 1);
					}
					break;
				
				case ConsoleKey.DownArrow:
					
					if (choice + 1 < optionQTY)
					{
						erasePrevious();
						Console.CursorTop = menuY + choice + 1;
						Console.CursorLeft = menuX + 1;
						Console.Write(">");
						
						return (byte)(choice + 1);
					}
					break;
			}
			
			return choice;
		}
		
		public static void MakeMainMenu()
		{
			RectanglePainting.DrawRectangle(RectanglePainting.SpecialBorderType.Wavish, 35, 10, 6, 11, ConsoleColor.Gray);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.CursorLeft = 37;
			Console.CursorTop = 11;
			Console.WriteLine("Play");
			Console.CursorLeft = 37;
			Console.WriteLine("Options");
			Console.CursorLeft = 37;
			Console.WriteLine("Controls");
			Console.CursorLeft = 37;
			Console.Write("Exit");
		}
		
		public static void MakeOptionMenu()
		{
			RectanglePainting.DrawRectangle(RectanglePainting.SpecialBorderType.Wavish, 0, 0, 11, 35, ConsoleColor.Yellow);
			
			Console.CursorTop = 1;
			Console.CursorLeft = 2;
			Console.WriteLine("EXIT OPTIONS");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Coloured Tetrominos");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Special Characters");
			
//			Console.CursorLeft = 2;
//			Console.WriteLine("Play Music");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Game Difficulty");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Starting Level");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Piece Falling Delay");
			
			Console.CursorLeft = 2;
			Console.WriteLine("Tick Duration");
			
			Console.CursorLeft = 2;
			Console.Write("Speed Increase Cycle");
		}
		
		public static void PrintSettings()
		{
			const string lineEraser = "          ";
			const int posX = 23;
			Console.CursorTop = 2;
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(Tetris.enableColour);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(Tetris.enableChars);
			
//			Console.CursorLeft = posX;
//			Console.Write(lineEraser);
//			Console.CursorLeft = posX;
//			Console.WriteLine(Tetris.enableMusic);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(settings.difficulty);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(settings.startingLevel);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(settings.waitTime);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.WriteLine(settings.tickDuration);
			
			Console.CursorLeft = posX;
			Console.Write(lineEraser);
			Console.CursorLeft = posX;
			Console.Write(settings.cycleDuration);
		}
		
		public static void MoveSwitchArrows(byte x, byte y, bool valueIsIncreasing)
		{
			var originalColor = Console.ForegroundColor;
			
			if (valueIsIncreasing)
				Console.ForegroundColor = ConsoleColor.Green;
			
			else
				Console.ForegroundColor = ConsoleColor.Red;
			
			Console.CursorLeft = x;
			Console.CursorTop = y;
			
			if (valueIsIncreasing)
				Console.Write("▲");
				
			else
				Console.Write("▼");
			
			Console.ForegroundColor = originalColor;
		}
		
		public static void EraseSwitchArrows(byte x, byte y)
		{
			Console.CursorLeft = x;
			Console.CursorTop = y;
			Console.Write(" ");
		}
		
		public static void SwitchDifficulty()
		{
			ConsoleKey input = 0;
			
			while (input != ConsoleKey.Enter)
			{
				input = Console.ReadKey(true).Key;
				
				if (input == ConsoleKey.UpArrow && settings.difficulty != Settings.Difficulties.Impossible)
				{
					MoveSwitchArrows(22, 4, true);
					settings = Settings.FromDifficulty(++settings.difficulty);
				}
				
				else if (input == ConsoleKey.DownArrow && settings.difficulty != Settings.Difficulties.Easy)
				{
					MoveSwitchArrows(22, 4, false);
					settings = Settings.FromDifficulty(--settings.difficulty);
				}
				
				PrintSettings();
			}
			
			EraseSwitchArrows(22, 4);
		}
		
		public static void SwitchValue(ref byte setting, byte x, byte y)
		{
			settings.difficulty = Settings.Difficulties.Custom;
			ConsoleKey input = 0;
			
			while (input != ConsoleKey.Enter)
			{
				input = Console.ReadKey(true).Key;
				
				if (input == ConsoleKey.UpArrow && setting != byte.MaxValue)
				{
					MoveSwitchArrows(x, y, true);
					++setting;
				}
				
				else if (input == ConsoleKey.DownArrow && setting != 1)
				{
					MoveSwitchArrows(x, y, false);
					--setting;
				}
				
				PrintSettings();
			}
			
			EraseSwitchArrows(x, y);
		}
		
		public static void SwitchLevel()
		{
			ConsoleKey input = 0;
			
			while (input != ConsoleKey.Enter)
			{
				input = Console.ReadKey(true).Key;
				
				if (input == ConsoleKey.UpArrow && settings.startingLevel != byte.MaxValue)
				{
					MoveSwitchArrows(22, 5, true);
					++settings.startingLevel;
				}
				
				else if (input == ConsoleKey.DownArrow && settings.startingLevel != byte.MinValue)
				{
					MoveSwitchArrows(22, 5, false);
					--settings.startingLevel;
				}
				
				PrintSettings();
			}
			
			EraseSwitchArrows(22, 5);
		}
		
		public static void NavigateOptions()
		{
			Console.Clear();
			MakeOptionMenu();
			var exit = false;
			byte choice = 0;
			
			var actions = new Action[8];
			
			actions[0] = () => exit = true;
			actions[1] = () => Tetris.enableColour = !Tetris.enableColour;
			actions[2] = () => Tetris.enableChars = !Tetris.enableChars;
//			actions[3] = () => Tetris.enableMusic = !Tetris.enableMusic;
			actions[3] = () => SwitchDifficulty();
			actions[4] = () => SwitchLevel();
			actions[5] = () => SwitchValue(ref settings.waitTime, 22, 6);
			actions[6] = () => SwitchValue(ref settings.tickDuration, 22, 7);
			actions[7] = () => SwitchValue(ref settings.cycleDuration, 22, 8);
			
			while (!exit)
			{
				PrintSettings();
				choice = PrintCursor(0, 1, 8, choice, actions[choice]);
			}
			
			settings.Save();
			Console.Clear();
		}
		
		public static void PrintControls()
		{
			const byte LEFT_PADDING = 2;
			Console.Clear();
			RectanglePainting.DrawRectangle(RectanglePainting.SpecialBorderType.Wavish, 0, 0, 5, 25, ConsoleColor.DarkRed);
			Console.SetCursorPosition(LEFT_PADDING, 1);
			Console.WriteLine("Arrow keys to move");
			Console.CursorLeft = LEFT_PADDING;
			Console.WriteLine("Space bar to pause");
			Console.CursorLeft = LEFT_PADDING;
			Console.WriteLine("Z to rotate");
			Console.CursorTop += 2;
		}
		
		public static void NavigateControls()
		{
			PrintControls();
			Console.WriteLine("Press any key to return.");
			Console.ReadKey(true);
			Console.Clear();
		}

		public static void Main(string[] args)
		{
			Console.CursorVisible = false;
			var exit = false;
			var logo = new ConsoleImage(new System.Drawing.Bitmap("image.png"));
			byte choice = 0;
			
			if (System.IO.File.Exists("game.stgs"))
				settings = new Settings("game.stgs");
			
			var actions = new Action[4];
			
			// play the game
			actions[0] = () => {
				
				Console.Clear();
				Tetris.Reset();
				Tetris.PlayGame(settings.waitTime, settings.startingLevel, settings.tickDuration, settings.cycleDuration);
				logo.Paint();
			};
			
			// options
			actions[1] = () => {
				
				NavigateOptions();
				logo.Paint();
			};
			
			// how to play
			actions[2] = () => { 
				
				NavigateControls();
				logo.Paint();
			};
			
			// quit the game
			actions[3] = () => exit = true;
			
			logo.Paint();
			
			while (!exit)
			{
				MakeMainMenu();
				choice = PrintCursor(35, 11, 4, choice, actions[choice]);
			}
		}
	}
}