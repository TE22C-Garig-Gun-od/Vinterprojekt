using System;
using System.Numerics;
using Raylib_cs;

class PongGame
{
    // Variabler för skärmstorlek och objektstorlekar och hastighet
    static int screenWidth = 800;
    static int screenHeight = 600;
    static int paddleWidth = 20;
    static int paddleHeight = 100;
    static int ballSize = 20;

    static float paddleSpeed = 0.3f;
    static float ballSpeed = 0.2f;

    // Här initialiseras positionerna för paddlarna och bollen samt riktningen för bollen.
    static Vector2 paddle1 = new Vector2(20, screenHeight / 2 - paddleHeight / 2);
    static Vector2 paddle2 = new Vector2(screenWidth - 20 - paddleWidth, screenHeight / 2 - paddleHeight / 2);

    static Vector2 paddle3 = new Vector2()
    static Vector2 ballPosition = new Vector2(screenWidth / 2, screenHeight / 2);
    static Random random = new Random();
    static Vector2 ballDirection = new Vector2(random.Next(2) == 0 ? -1 : 1, random.Next(2) == 0 ? -1 : 1);

    // High score
    static int highScore = 0;
    static int currentScore = 0;

    // Main method
    static void Main()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "Pong Game");

        while (!Raylib.WindowShouldClose())
        {
            UpdateGame();
            DrawGame();
        }

        Raylib.CloseWindow();
    }

    // Update game logic
    static void UpdateGame()
    {
        // Move paddles
        MovePaddle1TowardsBall();

        if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) && paddle2.Y > 0)
            paddle2.Y -= paddleSpeed;
        if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) && paddle2.Y + paddleHeight < screenHeight)
            paddle2.Y += paddleSpeed;

        // Move ball
        ballPosition.X += ballDirection.X * ballSpeed;
        ballPosition.Y += ballDirection.Y * ballSpeed;

        // Ball collision with paddles
        if (Raylib.CheckCollisionPointRec(ballPosition, new Rectangle(paddle1.X, paddle1.Y, paddleWidth, paddleHeight))
            || Raylib.CheckCollisionPointRec(ballPosition, new Rectangle(paddle2.X, paddle2.Y, paddleWidth, paddleHeight)))
        {
            ballDirection.X *= -1; // Reflect ball if it hits a paddle
        }

        // Ball collision with walls
        if (ballPosition.Y - ballSize / 2 <= 0 || ballPosition.Y + ballSize / 2 >= screenHeight)
        {
            ballDirection.Y *= -1; // Reflect ball if it hits the top or bottom wall
        }

        // Ball out of bounds
        if (ballPosition.X - ballSize / 1 <= 0)
        {
            currentScore++;
            ResetBall();
        }
        else if (ballPosition.X + ballSize / 1 >= screenWidth) // det kan vara 2 istället för 1
        {
            if (currentScore > highScore)
                highScore = currentScore;

            currentScore = 0;
            ResetBall();
        }
    }

    // Move paddle 1 towards the ball
    static void MovePaddle1TowardsBall()
    {
        if (ballPosition.Y < paddle1.Y + paddleHeight / 2)
        {
            paddle1.Y -= paddleSpeed;
        }
        else if (ballPosition.Y > paddle1.Y + paddleHeight / 2)
        {
            paddle1.Y += paddleSpeed;
        }
    }

    // Reset ball position and direction
    static void ResetBall()
    {
        ballPosition = new Vector2(screenWidth / 2, screenHeight / 2);
        ballDirection = new Vector2(random.Next(2) == 0 ? -1 : 1, random.Next(2) == 0 ? -1 : 1);
    }

    // Draw game objects
    static void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.PURPLE);

        Raylib.DrawRectangleRec(new Rectangle(paddle1.X, paddle1.Y, paddleWidth, paddleHeight), Color.WHITE);
        Raylib.DrawRectangleRec(new Rectangle(paddle2.X, paddle2.Y, paddleWidth, paddleHeight), Color.WHITE);
        Raylib.DrawCircleV(ballPosition, ballSize / 2, Color.WHITE);

        // Draw current and high scores
        Raylib.DrawText($"Score: {currentScore}", 10, 10, 20, Color.WHITE);
        Raylib.DrawText($"High Score: {highScore}", screenWidth - 150, 10, 20, Color.WHITE);

        Raylib.EndDrawing();
    }
}