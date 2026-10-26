namespace StaticSiteGenerator.Models;

public class HtmlNode(
    string? tag = null,
    string? value = null,
    List<HtmlNode>? children = null,
    Dictionary<string, string>? props = null)
{
    public string? Tag { get; } = tag;
    public string? Value { get; } = value;
    public List<HtmlNode>? Children { get; } = children;
    public Dictionary<string, string>? Props { get; } = props;

    public virtual string ToHtml() => throw new NotImplementedException();

    public string PropsToHtml()
    {
        if (Props is null || Props.Count == 0)
            return "";

        return " " + string.Join(" ", Props.Select(p => $"{p.Key}=\"{p.Value}\""));
    }

    public override string ToString() => $"HtmlNode({Tag}, {Value}, {Children}, {Props})";
}

public class LeafNode(string? tag, string? value, Dictionary<string, string>? props = null)
    : HtmlNode(tag, value, null, props)
{
    public override string ToHtml()
    {
        if (Value is null)
            throw new InvalidOperationException("Leaf node must have a value");

        if (string.IsNullOrEmpty(Tag))
            return Value;

        return $"<{Tag}{PropsToHtml()}>{Value}</{Tag}>";
    }

    public override string ToString() => $"LeafNode({Tag}, {Value}, {Props})";
}

public class ParentNode(string? tag, List<HtmlNode>? children, Dictionary<string, string>? props = null)
    : HtmlNode(tag, null, children, props)
{
    public override string ToHtml()
    {
        if (Tag is null)
            throw new InvalidOperationException("Parent node must have a tag");

        if (Children is null)
            throw new InvalidOperationException("Children is missing");

        return $"<{Tag}{PropsToHtml()}>{string.Concat(Children.Select(c => c.ToHtml()))}</{Tag}>";
    }
}
