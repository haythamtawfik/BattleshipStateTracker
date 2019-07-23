# Battleship Game State Tracker API 

## Background
This exercise is based on the classic game “Battleship”.
? Two players ? Each have a 10x10 board ? During setup, players can place an arbitrary number of
“battleships” on their board. The ships are 1-by-n sized, must fit entirely on the board, and must be
aligned either vertically or horizontally. ? During play, players take turn “attacking” a single position
on the opponent’s board, and the opponent must respond by either reporting a “hit” on one of their
battleships (if one occupies that position) or a “miss” ? A battleship is sunk if it has been hit on all
the squares it occupies ? A player wins if all of their opponent’s battleships have been sunk.

## The task
The task is to implement a Battleship state-tracker for a single player that must support the following logic: ? Create a board ? Add a battleship to the board ? Take an “attack” at a given position, and report back whether the attack resulted in a hit or a miss ? Return whether the player has lost the game yet (i.e. all battleships are sunk)

## Implementation Assumptions

- The Upper-left of the battle board is represented by the coordinates (0,0)
- The user isn't allowed to add ships after the game start
- The board is a symmetric square board of size n*n
- The ships can be adjacent to each other and doesn't need to have empty cells between them
- All the ships have the same length configured in the game setup step

## Notes

The app is written in .net core 2.2 with VS2019 community edition and it uses the following:
	Swagger
	SeriLog
	Newtonsoft
	Moq
	
The app is hosted on Azure App Service https://battleshiptrackerapi20190724014204.azurewebsites.net/api

Postman collection is included in the repo

Clean architecture is used in the program.

Logging is done for selected methods only as a demonstration

Unittests is done as a demonstraion to few methods 

Integration tests aren't included in this demo
