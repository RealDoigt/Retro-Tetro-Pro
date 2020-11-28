# Retro Tetro Pro
Retro Tetro Pro is a game which adheres to the core of tetromino gameplay as seen in early installements of the venerable franchise while also allowing the user as much creativity as possible to play the game they want. Everything in the game OUGHT to be customizable. It is of utmost importance than every variable which affects the flow of the game be available to the user to modify as they wish. This game is also a challenge to myself - and other programmers willing to join my efforts - to make a game in the .Net console by only using native libraries that can run on both .Net and Mono so that it may run on any machine and make the challenge much more interesting. It also gives the game a distinct retro look, which may remind some of a day when graphics were done with characters.
# How to compile
1. Create a new console project in Visual Studio/MonoDevelop/SharpDevelop. All three work should work.
2. Copy over the files, then add "existing" files from your IDE.
3. Make sure to add the necessary references to ConsolePaint in your project. 
4. System.Drawing tends to be absent from the default references, so make sure it is there too.
5. Change the copy action of both "image.png" and "Boo.Lang.dll" to something along the lines of "copy newest version".
