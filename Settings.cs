using System.IO;

namespace TetrisCS
{
	public struct Settings
	{
		public enum Difficulties { Easy, Normal, Hard, VeryHard, Impossible, Custom };
		
		public Difficulties difficulty;
		public byte startingLevel;
		public byte waitTime;
		public byte tickDuration;
		public byte cycleDuration;
		
		const byte DEFAULT_CYCLE = 1;
		
		public Settings(byte cycle, byte wait = 20, byte ms = 50, byte level = 0)
		{
			startingLevel = level;
			waitTime = wait;
			tickDuration = ms;
			difficulty = Difficulties.Custom;
			cycleDuration = cycle;
		}
		
		public Settings(string filePath)
		{
			using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
			{
				Tetris.enableChars = reader.ReadBoolean();
				Tetris.enableColour = reader.ReadBoolean();
//				Tetris.enableMusic = reader.ReadBoolean();
				difficulty = (Difficulties)reader.ReadByte();
				startingLevel = reader.ReadByte();
				waitTime = reader.ReadByte();
				tickDuration = reader.ReadByte();
				cycleDuration = reader.ReadByte();
			}
		}
		
		public static Settings FromDifficulty(Difficulties d)
		{
			switch (d) 
			{
				case Settings.Difficulties.Easy:
					return new Settings(3){difficulty = Difficulties.Easy};
					
				case Settings.Difficulties.Hard:
					return new Settings(DEFAULT_CYCLE, 10, 40){difficulty = Difficulties.Hard};
					
				case Settings.Difficulties.VeryHard:
					return new Settings(DEFAULT_CYCLE, 5, 25){difficulty = Difficulties.VeryHard};
					
				case Settings.Difficulties.Impossible:
					return new Settings(DEFAULT_CYCLE, 1, 10){difficulty = Difficulties.Impossible};
					
				default:
					return new Settings(2){difficulty = Difficulties.Normal};
			}
		}
		
		public void Save()
		{
			using (var writer = new BinaryWriter(File.Open("game.stgs", FileMode.Create))) 
			{
				writer.Write(Tetris.enableChars);
				writer.Write(Tetris.enableColour);
//				writer.Write(Tetris.enableMusic);
				writer.Write((byte)difficulty);
				writer.Write(startingLevel);
				writer.Write(waitTime);
				writer.Write(tickDuration);
				writer.Write(cycleDuration);
			}
		}
	}
}
