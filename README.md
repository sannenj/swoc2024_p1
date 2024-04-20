# SWOC24
Playing snake in N dimensions.
This is the code that I developed for the Sioux Weekend of Code 2024 edition (5th of April 2024, finale at 6th of April).
The challenge was to create a bot that would play a modified version of snake in N dimensions in a multiplayer world (with other challenge competitors).
The server for this can be found at https://github.com/matthijsman/Swoc2024, he also developed the visualization (visual 12 dimensions in a 3d space is a bit of challenge :) ).
Shoutout to him for the amazing challenge!
The gRPC proto file was provide to use to interact with the server.

And all the other organisers and presenters during the weekend for a amazing time!

# Running the bot
This bot is a basic C# application with some unit tests included.
All (nuget) dependencies are defined in the project and should be downloaded when build using regular .NET tooling (ie Visual Studio, or the CLI).
The gRPC package should automatically generate the interface classes from the included proto file.

# Know problems
This bot does have problems.
As long as you only have 1 snake, most things seem fine, but when the snake is suppose to split, it falls apart.
I had a wrong assumption in that the game world delta would include the snakeID of a cell that got occupied, but it only contains the playername.
This is fine for enemy snakes, because I also use the same to actually do the updates of my own snake, the split snake 'disapears'.
It gets added to my original snake (which happens to have my player name incidently).
This snake now has 2 heads and some funny behaviors start to appear.

There are problably more, but this is a major one that I couldn't fix in time.
Yes, it got unit tests, but I tested with the wrong input :).
