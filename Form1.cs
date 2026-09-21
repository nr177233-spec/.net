using System;
using System.Drawing;
using System.Windows.Forms;

namespace tictactoe
{
    public partial class Form1 : Form
    {
        private readonly Button[,] board = new Button[3, 3];
        private readonly Label statusLabel;
        private readonly Button startButton;
        private readonly System.Windows.Forms.Timer drawTimer;

        private bool xTurn = true;
        private bool gameRunning = false;
        private int moves = 0;

        public Form1()
        {
            InitializeComponent();

            statusLabel = new Label();
            startButton = new Button();
            drawTimer = new System.Windows.Forms.Timer();

            CreateGame();
        }

        private void CreateGame()
        {
            Text = "Tic Tac Toe";
            Size = new Size(450, 570);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.Black;

            // TITLE
            Label title = new Label
            {
                Text = "TIC TAC TOE",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Black,
                AutoSize = true,
                Location = new Point(105, 20)
            };

            Controls.Add(title);

            // STATUS
            statusLabel.Text = "Press START GAME";
            statusLabel.Font =
                new Font("Arial", 15, FontStyle.Bold);

            statusLabel.ForeColor = Color.White;
            statusLabel.BackColor = Color.Black;
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(125, 70);

            Controls.Add(statusLabel);

            // BOARD
            int startX = 50;
            int startY = 120;
            int cellSize = 110;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Button cell = new Button
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Left = startX + col * cellSize,
                        Top = startY + row * cellSize,

                        Font = new Font(
                            "Arial",
                            40,
                            FontStyle.Bold
                        ),

                        BackColor =
                            Color.FromArgb(25, 25, 25),

                        ForeColor = Color.White,

                        FlatStyle = FlatStyle.Flat,

                        Enabled = false,

                        Text = "",

                        Cursor = Cursors.Hand
                    };

                    cell.FlatAppearance.BorderColor =
                        Color.FromArgb(80, 80, 80);

                    cell.FlatAppearance.BorderSize = 2;

                    cell.Click += Cell_Click;

                    board[row, col] = cell;

                    Controls.Add(cell);
                }
            }

            // START BUTTON
            startButton.Text = "START GAME";

            startButton.Font =
                new Font("Arial", 14, FontStyle.Bold);

            startButton.Width = 170;
            startButton.Height = 50;

            startButton.Left = 130;
            startButton.Top = 455;

            startButton.BackColor =
                Color.FromArgb(35, 35, 35);

            startButton.ForeColor = Color.White;

            startButton.FlatStyle = FlatStyle.Flat;

            startButton.FlatAppearance.BorderColor =
                Color.White;

            startButton.FlatAppearance.BorderSize = 1;

            startButton.Cursor = Cursors.Hand;

            startButton.Click += StartButton_Click;

            Controls.Add(startButton);

            // DRAW TIMER
            drawTimer.Interval = 1500;

            drawTimer.Tick += DrawTimer_Tick;
        }

        // START / RESTART GAME
        private void StartButton_Click(
            object? sender,
            EventArgs e)
        {
            drawTimer.Stop();

            StartNewGame();
        }

        // START NEW GAME
        private void StartNewGame()
        {
            xTurn = true;
            moves = 0;
            gameRunning = true;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col].Text = "";

                    board[row, col].Enabled = true;

                    board[row, col].BackColor =
                        Color.FromArgb(25, 25, 25);

                    board[row, col].ForeColor =
                        Color.White;
                }
            }

            statusLabel.Text =
                "Player X's Turn";

            statusLabel.ForeColor =
                Color.Red;

            startButton.Text =
                "RESTART GAME";
        }

        // CELL CLICK
        private void Cell_Click(
            object? sender,
            EventArgs e)
        {
            if (!gameRunning)
                return;

            if (sender is not Button cell)
                return;

            if (cell.Text != "")
                return;

            // X TURN
            if (xTurn)
            {
                cell.Text = "X";
                cell.ForeColor = Color.Red;
            }
            // O TURN
            else
            {
                cell.Text = "O";
                cell.ForeColor = Color.Blue;
            }

            moves++;

            // CHECK WINNER
            if (CheckWinner())
            {
                gameRunning = false;

                DisableBoard();

                string winner =
                    xTurn ? "X" : "O";

                statusLabel.Text =
                    "Player " + winner + " Wins!";

                statusLabel.ForeColor =
                    xTurn ? Color.Red : Color.Blue;

                MessageBox.Show(
                    "Player " + winner + " Wins!",
                    "Game Finished",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                return;
            }

            // CHECK DRAW
            if (moves == 9)
            {
                DrawGame();

                return;
            }

            // CHANGE TURN
            xTurn = !xTurn;

            if (xTurn)
            {
                statusLabel.Text =
                    "Player X's Turn";

                statusLabel.ForeColor =
                    Color.Red;
            }
            else
            {
                statusLabel.Text =
                    "Player O's Turn";

                statusLabel.ForeColor =
                    Color.Blue;
            }
        }

        // CHECK WINNER
        private bool CheckWinner()
        {
            // ROWS
            for (int row = 0; row < 3; row++)
            {
                if (
                    board[row, 0].Text != "" &&
                    board[row, 0].Text ==
                    board[row, 1].Text &&
                    board[row, 1].Text ==
                    board[row, 2].Text
                )
                {
                    HighlightWinner(
                        board[row, 0],
                        board[row, 1],
                        board[row, 2]
                    );

                    return true;
                }
            }

            // COLUMNS
            for (int col = 0; col < 3; col++)
            {
                if (
                    board[0, col].Text != "" &&
                    board[0, col].Text ==
                    board[1, col].Text &&
                    board[1, col].Text ==
                    board[2, col].Text
                )
                {
                    HighlightWinner(
                        board[0, col],
                        board[1, col],
                        board[2, col]
                    );

                    return true;
                }
            }

            // DIAGONAL 1
            if (
                board[0, 0].Text != "" &&
                board[0, 0].Text ==
                board[1, 1].Text &&
                board[1, 1].Text ==
                board[2, 2].Text
            )
            {
                HighlightWinner(
                    board[0, 0],
                    board[1, 1],
                    board[2, 2]
                );

                return true;
            }

            // DIAGONAL 2
            if (
                board[0, 2].Text != "" &&
                board[0, 2].Text ==
                board[1, 1].Text &&
                board[1, 1].Text ==
                board[2, 0].Text
            )
            {
                HighlightWinner(
                    board[0, 2],
                    board[1, 1],
                    board[2, 0]
                );

                return true;
            }

            return false;
        }

        // HIGHLIGHT WINNING CELLS
        private void HighlightWinner(
            Button a,
            Button b,
            Button c)
        {
            Color green =
                Color.FromArgb(30, 100, 30);

            a.BackColor = green;
            b.BackColor = green;
            c.BackColor = green;
        }

        // DRAW
        private void DrawGame()
        {
            gameRunning = false;

            DisableBoard();

            statusLabel.Text = "DRAW!";

            statusLabel.ForeColor = Color.White;

            drawTimer.Start();
        }

        // AUTOMATICALLY RESTART AFTER DRAW
        private void DrawTimer_Tick(
            object? sender,
            EventArgs e)
        {
            drawTimer.Stop();

            StartNewGame();
        }

        // DISABLE BOARD
        private void DisableBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col].Enabled = false;
                }
            }
        }

        // FORM LOAD
        private void Form1_Load(
            object? sender,
            EventArgs e)
        {
        }
    }
}