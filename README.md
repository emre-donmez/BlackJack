# Blackjack Game - ASP.NET MVC

This is the README file for a Blackjack game developed using ASP.NET MVC. This game can be played in a web browser, allowing users to enjoy a game of Blackjack.

## Project Overview

The project has been developed using the ASP.NET MVC framework. The Blackjack game requires an initial entry with a balance of your choice.

## Game Rules

- Players start with a specific balance at the beginning.
- In each hand, players initially receive two cards.
- Cards are valued between 2 and 11 points. They come in four different suits: Spades, Diamonds, Hearts, and Clubs.
- The goal is to reach 21 or get as close to 21 as possible.
- Players make decisions to either take more cards or pass.
- If players exceed 21, they lose the hand.
- Bets are updated when players win or lose.
- Players' balances are displayed at the end of each hand.

## Getting Started

Once you have set your balance, you can choose your bet amount, and then click the "Deal Cards" button to start the game.

## Starting the Game

1. Enter your balance on the main page and click the "Start Game" button.
2. When you start the game, you will begin with your initial balance.
3. In each hand, click the appropriate buttons to take cards or pass.
4. Your balance will be updated at the end of each hand, depending on whether you win or lose.
5. To end the game, click the "Quit" button.

## Development of the Game

The game has been developed using ASP.NET MVC, and player information and game state are stored using sessions. The rules and game mechanics are implemented in the GameController and model classes.

## Requirements

To run the game, you need the following:

- Visual Studio (or another IDE)
- ASP.NET MVC
- .NET Framework

## How to Run

1. Open the project in Visual Studio.
2. Build and run the project.
3. Open your web browser and start the game at [http://localhost:port].
