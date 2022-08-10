namespace zsh_ultra
{
    [Serializable]
    public struct Section
    {
        public readonly string leftLimit;
        public readonly string content;
        public readonly string rightLimit;
        internal readonly ConsoleColor leftColor;
        internal readonly ConsoleColor contentColor;
        internal readonly ConsoleColor rightColor;

        public Section(string leftLimit, string content, string rightLimit, params ConsoleColor[] colors)
        {
            this.leftLimit = leftLimit;
            this.content = content;
            this.rightLimit = rightLimit;
            leftColor = colors[0];
            contentColor = colors[1];
            rightColor = colors[2];
        }

        public override string? ToString() => leftLimit + content + rightLimit;
    }
}