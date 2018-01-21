using System;
using System.Collections;
using IPlayable;
using System.Threading;
namespace FriendlyBoardSwitch {
    class FabulousWrestlingBoard : IPlayable.IPlayable {
        int[,] board;
        int boardSize = 8;
        private String name = "Dan";
        public FabulousWrestlingBoard() {
            Initialize();
        }
        //initialize the board
        private void Initialize() {
            //init 2d array
            board = new int[boardSize, boardSize];
            //fill the array with starting pos
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if ((y == 3 || y == 4) && (x == 3 || x == 4))
                        board[x, y] = x == y ? 0 : 1;
                    else
                        board[x, y] = -1;
                }
            }
        }

        //test if a move is outside the board
        private bool Out(int x, int y) {
            return (x < 0 || y < 0 || x >= boardSize || y >= boardSize) ? true : false;
        }

        //test if a tile is empty
        private bool Empty(int x, int y) {
            return (board[x, y] == -1) ? true : false;
        }
        private bool Empty(int x, int y, int[,] b) {
            return (b[x, y] == -1) ? true : false;
        }

        //get the score of the player
        private int GetScore(int player) {
            int s = 0;
            foreach (int e in board) {
                if (e == player)
                    s++;
            }
            return s;
        }
        private bool CheckLine(int x, int y, int direction, int color, bool stockCurrentLocation, ArrayList ary = null, int[,] game = null, ArrayList ops = null) {
            if (game == null) {
                game = this.board;
            }
            switch (direction) {
                case 1: {
                        return CheckLine(x, y, x - 1, y + 1, -1, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 2: {
                        return CheckLine(x, y, x, y + 1, 0, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 3: {
                        return CheckLine(x, y, x + 1, y + 1, 1, 1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 4: {
                        return CheckLine(x, y, x - 1, y, -1, 0, color, stockCurrentLocation, ary, game, ops);
                    }
                case 6: {
                        return CheckLine(x, y, x + 1, y, 1, 0, color, stockCurrentLocation, ary, game, ops);
                    }
                case 7: {
                        return CheckLine(x, y, x - 1, y - 1, -1, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 8: {
                        return CheckLine(x, y, x, y - 1, 0, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                case 9: {
                        return CheckLine(x, y, x + 1, y - 1, 1, -1, color, stockCurrentLocation, ary, game, ops);
                    }
                default: {
                        return false;
                    }
            }
        }
        private bool CheckLine(int startingX, int startingY, int x, int y, int xInc, int yInc, int color, bool stockCurrentLocations, ArrayList ary = null, int[,] game = null, ArrayList ops = null) {
            if (game == null) {
                game = this.board;
            }
            int foeColor = color == 0 ? 1 : 0;
            bool firstPass = false;
            while (!Out(x, y)) {
                if (game[x, y] == -1)
                    return false;
                if (game[x, y] == color && firstPass == false) {
                    return false;
                }
                if (game[x, y] == foeColor) {
                    if (stockCurrentLocations) {
                        ary.Add(new Tuple<int, int>(x, y));
                    }
                    firstPass = true;
                }
                if (firstPass == true && game[x, y] == color) {
                    if (ops != null){
                        bool isPresent = false;
                        for (int i = 0; i < ops.Count; i++) {
                            int[] toTest = (int[])ops[i];
                            if (startingX == toTest[0] && startingY == toTest[1]) {
                                isPresent = true;
                                break;
                            }
                        }
                        if (isPresent == false) {
                            int[] op = new int[2];
                            op[0] = startingX;
                            op[1] = startingY;
                            ops.Add(op);
                        }
                    }
                    return true;
                }
                x += xInc;
                y += yInc;
            }
            return false;
        }

        //give all the possible moves for a player
        private ArrayList Ops(int[,] game, int player) {
            ArrayList ops = new ArrayList();
            for (int y = 0; y < this.boardSize; y++) {
                for (int x = 0; x < this.boardSize; x++) {
                    for (int i = 0; i < 10; i++) {
                        if (i != 5) {
                            if (game[x,y] == -1) {
                                CheckLine(x, y, i, player, false, null, game, ops);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ops.Count; i++) {
                int[] temp = (int[])ops[i];
            }
            return ops;
        }

        //get the score of the black pawns
        public int GetBlackScore() {
            return GetScore(1);
        }

        //get the score of the white pawns
        public int GetWhiteScore() {
            return GetScore(0);
        }

        //get the name of the IA
        public string GetName() {
            return name;
        }

        //test if a move is legal or not
        public bool IsPlayable(int column, int line, bool isWhite) {
            if (Out(column, line) || !Empty(column, line))
                return false;
            else {
                int c = isWhite ? 0 : 1;
                for (int i = 0; i < 10; i++) {
                    if (i != 5) {
                        if (CheckLine(column, line, i, c, false, null, null, null)) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        //play a move on the board
        public bool PlayMove(int column, int line, bool isWhite) {
            ArrayList ary = new ArrayList();
            int c = isWhite ? 0 : 1;
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    if (CheckLine(column, line, i, c, false, null, null, null)) {
                        CheckLine(column, line, i, c, true, ary, null, null);
                    }
                }
            }
            if (ary.Count == 0) {
                return false;
            } else {
                foreach (Tuple<int, int> t in ary) {
                    board[t.Item1, t.Item2] = board[t.Item1, t.Item2] == 1 ? 0 : 1;
                    board[column, line] = c;
                }
                ary = null;
                return true;
            }
        }

        //apply an operation (move) on a given board
        public int[,] Apply(int column, int line, bool isWhite, int[,] game) {
            int[,] newBoard;
            newBoard = (int[,])game.Clone();
            ArrayList ary = new ArrayList();
            int c = isWhite ? 0 : 1;
            for (int i = 0; i < 10; i++) {
                if (i != 5) {
                    if (CheckLine(column, line, i, c, false, null, newBoard, null)) {
                        CheckLine(column, line, i, c, true, ary, newBoard, null);
                    }
                }
            }
            foreach (Tuple<int, int> t in ary) {
                newBoard[t.Item1, t.Item2] = newBoard[t.Item1, t.Item2] == 1 ? 0 : 1;
                newBoard[column, line] = c;
            }
            ary = null;
            return newBoard;
        }

        //get the next move to play
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn) {
            Tuple<Double, int, int> move = AlphaBeta(board, level, 1, 0, whiteTurn ? 0 : 1);
            return new Tuple<int, int>(move.Item2, move.Item3);
        }

        //our implementation of alphabeta
        public Tuple<Double, int, int> AlphaBeta(int[,] root, int depth, int minOrMax, Double parentValue, int player) {
            if (depth == 0 || Final(player, root)) {
                return new Tuple<Double, int, int>(Score(root, player == 0 ? 1 : 0), -1, -1);
            }
            Double optVal = minOrMax*Double.MinValue;
            int[] optOp = { -1, -1 };
            foreach (int[] op in Ops(root, player)) {             
                int[,] newNode = Apply(op[0], op[1], player == 0, root);
                Double val = AlphaBeta(newNode, depth - 1, -minOrMax, optVal, player == 0 ? 1 : 0).Item1;
                if (val * minOrMax > optVal * minOrMax) {
                    optVal = val;
                    optOp = op;
                    if (optVal * minOrMax > parentValue * minOrMax) {
                        break;
                    }
                }
            }
            return new Tuple<Double, int, int>(optVal, optOp[0], optOp[1]);
        }

        //the evaluation function
        public Double Score(int[,] board, int player) {
            //Evaluation fonction comes from https://github.com/kartikkukreja/blog-codes/blob/master/src/Heuristic%20Function%20for%20Reversi%20(Othello).cpp
            int my_color = player;
            int opp_color = player == 0 ? 1 : 0;
            double p = 0, c = 0, l = 0, m = 0, f = 0, d = 0;
            double m_weight = 100.0, l_weight = -12.5, c_weight = 25.0, pfd_weight = 100.0;
            int[] X1 = { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] Y1 = { 0, 1, 1, 1, 0, -1, -1, -1 };

            //a matrice wich give the score for each tile
            int[,] weightedMatrix = {{20, -3, 11, 8, 8, 11, -3, 20},
                       {-3, -7, -4, 1, 1, -4, -7, -3},
                       { 11, -4, 2, 2, 2, 2, -4, 11},
                       { 8, 1, 2, -3, -3, 2, 1, 8},
                       { 8, 1, 2, -3, -3, 2, 1, 8},
                       { 11, -4, 2, 2, 2, 2, -4, 11},
                       { -3, -7, -4, 1, 1, -4, -7, -3},
                       { 20, -3, 11, 8, 8, 11, -3, 20} };

            EvaluateDiffDiskSquares(board, my_color, opp_color, weightedMatrix, X1, Y1, pfd_weight, out p, out f, out d);
            c = EvaluateCornerOccupancy(board, my_color, opp_color, c_weight);
            l = EvaluateCornerCloseness(board, my_color ,opp_color, l_weight);
            m = EvaluateMobility(board, my_color, opp_color, m_weight);
            //return final weighted score
            return (10 * p) + (801.724 * c) + (382.026 * l) + (78.922 * m) + (74.396 * f) + (10 * d);
        }


        private void EvaluateDiffDiskSquares(int[,] board, int color, int foe_color,int[,] weightedMatrix, int[] X1, int[] Y1, double pfd_weight, out double p, out double f, out double d) {
            d = 0;
            f = 0;
            p = 0;
            int x, y, player_tiles_count = 0, foe_tiles_count = 0, player_front_tiles = 0, foe_front_tiles = 0;
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (board[i, j] == color) {
                        d += weightedMatrix[i, j];
                        player_tiles_count++;
                    } else if (board[i, j] == foe_color) {
                        d -= weightedMatrix[i, j];
                        foe_tiles_count++;
                    }
                    if (!Empty(i, j, board)) {
                        for (int k = 0; k < 8; k++) {
                            x = i + X1[k]; y = j + Y1[k];
                            if (!Out(x, y) && Empty(x, y)) {
                                if (board[i, j] == color) player_front_tiles++;
                                else foe_front_tiles++;
                                break;
                            }
                        }
                    }
                }
            }
            int total_tiles_count = player_tiles_count + foe_tiles_count;
            int total_front_tiles_count = player_front_tiles + foe_front_tiles;

            if (player_tiles_count > foe_tiles_count) {
                p = (pfd_weight * player_tiles_count) / total_tiles_count;
            }
            else if (player_tiles_count < foe_tiles_count) {
                p = -(pfd_weight * foe_tiles_count) / total_tiles_count;
            }
            else p = 0;
            if (player_tiles_count > foe_tiles_count) {
                f = -(pfd_weight * player_front_tiles) / (total_front_tiles_count);
            }
            else if (player_front_tiles < foe_front_tiles) {
                f = (pfd_weight * foe_front_tiles) / (total_front_tiles_count);
            }
            else f = 0;
        }

        //evaluate if the corner are occupate or not
        private double EvaluateCornerOccupancy(int[,] board, int color, int foe_color, double weight) {
            int player_tiles_count = 0;
            int foe_tiles_count = 0;
            Tuple<int, int>[] corners = GetCorners();
            foreach(Tuple<int, int> t in corners){
                if (board[t.Item1, t.Item2] == color) {
                    player_tiles_count++;
                }
                else if (board[t.Item1, t.Item2] == foe_color) {
                    foe_tiles_count++;
                }
            }
            return weight * (player_tiles_count - foe_tiles_count);
        }
        
        //evaluate the square around the corners
        private double EvaluateCornerCloseness(int[,] board, int color, int foe_color, double weight) {
            int player_tiles_count = 0;
            int foe_tiles_count = 0;
            if (Empty(0, 0, board)) {
                if (board[0, 1] == color) player_tiles_count++;
                else if (board[0, 1] == foe_color) foe_tiles_count++;
                if (board[1, 1] == color) player_tiles_count++;
                else if (board[1, 1] == foe_color) foe_tiles_count++;
                if (board[1, 0] == color) player_tiles_count++;
                else if (board[1, 0] == foe_color) foe_tiles_count++;
            }
            if (Empty(0, 7, board)) {
                if (board[0, 6] == color) player_tiles_count++;
                else if (board[0, 6] == foe_color) foe_tiles_count++;
                if (board[1, 6] == color) player_tiles_count++;
                else if (board[1, 6] == foe_color) foe_tiles_count++;
                if (board[1, 7] == color) player_tiles_count++;
                else if (board[1, 7] == foe_color) foe_tiles_count++;
            }
            if (Empty(7, 0, board)) {
                if (board[7, 1] == color) player_tiles_count++;
                else if (board[7, 1] == foe_color) foe_tiles_count++;
                if (board[6, 1] == color) player_tiles_count++;
                else if (board[6, 1] == foe_color) foe_tiles_count++;
                if (board[6, 0] == color) player_tiles_count++;
                else if (board[6, 0] == foe_color) foe_tiles_count++;
            }
            if (Empty(7,7, board)) {
                if (board[6, 7] == color) player_tiles_count++;
                else if (board[6, 7] == foe_color) foe_tiles_count++;
                if (board[6, 6] == color) player_tiles_count++;
                else if (board[6, 6] == foe_color) foe_tiles_count++;
                if (board[7, 6] == color) player_tiles_count++;
                else if (board[7, 6] == foe_color) foe_tiles_count++;
            }
            return weight * (player_tiles_count - foe_tiles_count);
        }

        private Tuple<int, int>[] GetCorners() {
            Tuple<int, int>[] corners = new Tuple<int, int>[4];
            corners[0] = new Tuple<int, int>(0, 0); //Top left corner
            corners[1] = new Tuple<int, int>(0, 7); //Top right corner
            corners[2] = new Tuple<int, int>(7, 0); //Bottom left corner
            corners[3] = new Tuple<int, int>(7, 7); //Bottom right corner
            return corners;
        }

        //evaluate the number of moves possibility for each player
        private double EvaluateMobility(int[,] board, int color, int foe_color, double weight) {
            int player_tiles_count = Ops(board, color).Count;
            int foe_tiles_count = Ops(board, foe_color).Count;
            int total_tiles_count = player_tiles_count + foe_tiles_count;
            if (player_tiles_count > foe_tiles_count) {
                return (weight * player_tiles_count) / (total_tiles_count);
            }
            else if (player_tiles_count < foe_tiles_count) {
                return -(weight * foe_tiles_count) / (total_tiles_count);
            } else {
                return 0;
            }
        }

        // get the board state
        public int[,] GetBoard() {
            return board;
        }

        //test if a board state is a final state 
        public bool Final(int color, int[,] game) {
            if (isGameFinished(game)) {
                return true;
            } else {
                for (int y = 0; y < boardSize; y++) {
                    for (int x = 0; x < boardSize; x++) {
                        for (int i = 0; i < 10; i++) {
                            if (i != 5 && game[x, y] == -1) {
                                if (CheckLine(x, y, i, color, false, null, game, null)) {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        //test if a game is finished
        public bool isGameFinished(int[,] game) {
            for (int y = 0; y < boardSize; y++) {
                for (int x = 0; x < boardSize; x++) {
                    if (game[x, y] == -1) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
    