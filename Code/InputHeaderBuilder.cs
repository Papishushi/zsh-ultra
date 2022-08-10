using zsh_ultra;

[Serializable]
public struct InputHeaderBuilder
{
    public readonly List<Section> sections;
    public string separator;
    public ConsoleColor separatorColor;
    public string finalizer;
    public ConsoleColor finalizerColor;

    public InputHeaderBuilder()
    {
        sections = new List<Section>();
        separator = string.Empty;
        finalizer = string.Empty;
        separatorColor = ConsoleColor.Gray;
        finalizerColor = ConsoleColor.Gray;
    }

    public InputHeaderBuilder(string separator, string finalizer, params ConsoleColor[] colors)
    {
        sections = new List<Section>();
        this.separator = separator;
        this.finalizer = finalizer;
        separatorColor = colors[0];
        finalizerColor = colors[1];
    }

    public void Print()
    {
        if (sections == null) return;
        foreach (var section in sections)
        {
            Tools.ColorWrite(section.leftLimit, section.leftColor);
            Tools.ColorWrite(section.content, section.contentColor);
            Tools.ColorWrite(section.rightLimit, section.rightColor);
            Tools.ColorWrite(separator, separatorColor);
        }
        Tools.ColorWrite(finalizer, finalizerColor);
    }

    private string Build()
    {
        var append = string.Empty;
        var separator = this.separator;
        sections.ForEach(section => append += section.ToString() + separator);
        append += finalizer;
        return append;
    }

    public override string? ToString() => sections == null ? null : Build();

    public static implicit operator string?(InputHeaderBuilder t) => t.ToString();
}
