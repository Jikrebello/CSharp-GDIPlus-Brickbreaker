namespace Pong
{
    public enum PaddleStates
    {
        Neutral,
        Thin,
        Wide,
        Fast,
        Slow
    }

    public enum BallStates
    {
        Neutral,
        Big,
        Small,
        Fast,
        Slow,
        Death
    }

    static class Globals
    {
        // --- PADDLE RELEVANT BOOLS ---
        public static bool IS_PADDLE_WIDE;
        public static bool IS_PADDLE_THIN;
        public static bool IS_PADDLE_FAST;
        public static bool IS_PADDLE_SLOW;
        public static bool CAN_PADDLE_SHOOT;

        // --- BALL RELEVANT BOOLS ---
        public static bool IS_BALL_BIG;
        public static bool IS_BALL_SMALL;
        public static bool IS_BALL_FAST;
        public static bool IS_BALL_SLOW;
        public static bool IS_BALL_START;
        public static bool IS_BALL_DEATH;

        // --- PADDLE GROWING AND SHRINKING STATE ---
        public static bool PADDLE_GROWING;
        public static bool PADDLE_SHRINKING;

        // --- BALL GROWING AND SHRINKING STATE ---
        public static bool BALL_GROWING;
        public static bool BALL_SHRINKING;

        // --- PADDLE DIMENSIONS ---
        public const int PADDLE_NORMAL_WIDTH = 50;
        public const int PADDLE_THIN_WIDTH = 30;
        public const int PADDLE_WIDE_WIDTH = 80;

        // --- BALL DIMENSIONS ---
        public const int BALL_NORMAL_DIAMETER = 10;
        public const int BIG_BALL_DIAMETER = 15;
        public const int SMALL_BALL_DIAMETER = 5;

        // --- PADDLE SPEEDS ---
        public const int PADDLE_NORMAL_SPEED = 5;
        public const int PADDLE_SLOW = 3;
        public const int PADDLE_FAST = 7;

        // --- BALL SPEED ---
        public const float BALL_NORMAL_SPEED = 3.5f;
        public const float BALL_SLOW = 0.5f;
        public const float BALL_FAST = 5;
    }
}
