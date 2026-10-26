using System.Text.RegularExpressions;
using StaticSiteGenerator.Enums;
using StaticSiteGenerator.Models;

namespace StaticSiteGenerator;

public static class InlineMarkdown
{
    public static LeafNode TextNodeToHtmlNode(TextNode textNode) => textNode.TextType switch
    {
        TextType.Text => new LeafNode(null, textNode.Text),
        TextType.Bold => new LeafNode("b", textNode.Text),
        TextType.Italic => new LeafNode("i", textNode.Text),
        TextType.Code => new LeafNode("code", textNode.Text),
        TextType.Link => new LeafNode("a", textNode.Text, new() { ["href"] = textNode.Url! }),
        TextType.Image => new LeafNode("img", "", new() { ["src"] = textNode.Url!, ["alt"] = textNode.Text }),
        _ => throw new ArgumentException("Invalid type"),
    };

    public static List<TextNode> SplitNodesDelimiter(List<TextNode> oldNodes, string delimiter, TextType textType)
    {
        var result = new List<TextNode>();

        foreach (var node in oldNodes)
        {
            if (node.TextType != TextType.Text)
            {
                result.Add(node);
                continue;
            }

            var parts = node.Text.Split(delimiter);

            for (var index = 0; index < parts.Length; index++)
                result.Add(new TextNode(parts[index], index % 2 == 0 ? TextType.Text : textType));
        }

        return result;
    }

    public static List<(string Text, string Url)> ExtractMarkdownImages(string text) =>
        Extract(text, @"!\[([^\[\]]*)\]\(([^\(\)]*)\)");

    public static List<(string Text, string Url)> ExtractMarkdownLinks(string text) =>
        Extract(text, @"(?<!!)\[([^\[\]]*)\]\(([^\(\)]*)\)");

    private static List<(string, string)> Extract(string text, string pattern) =>
        Regex.Matches(text, pattern).Select(m => (m.Groups[1].Value, m.Groups[2].Value)).ToList();

    public static List<TextNode> SplitNodesImage(List<TextNode> oldNodes) =>
        SplitNodes(oldNodes, ExtractMarkdownImages, (text, url) => $"![{text}]({url})", TextType.Image);

    public static List<TextNode> SplitNodesLink(List<TextNode> oldNodes) =>
        SplitNodes(oldNodes, ExtractMarkdownLinks, (text, url) => $"[{text}]({url})", TextType.Link);

    private static List<TextNode> SplitNodes(
        List<TextNode> oldNodes,
        Func<string, List<(string Text, string Url)>> extract,
        Func<string, string, string> format,
        TextType textType)
    {
        var newList = new List<TextNode>();

        foreach (var node in oldNodes)
        {
            if (node.TextType != TextType.Text)
            {
                newList.Add(node);
                continue;
            }

            var remainingText = node.Text;

            foreach (var (text, url) in extract(node.Text))
            {
                var sections = remainingText.Split(format(text, url), 2);

                if (sections[0] != "")
                    newList.Add(new TextNode(sections[0], TextType.Text));

                newList.Add(new TextNode(text, textType, url));
                remainingText = sections[1];
            }

            if (remainingText != "")
                newList.Add(new TextNode(remainingText, TextType.Text));
        }

        return newList;
    }

    public static List<TextNode> TextToTextNodes(string text)
    {
        List<TextNode> result = [new TextNode(text, TextType.Text)];

        result = SplitNodesDelimiter(result, "**", TextType.Bold);
        result = SplitNodesDelimiter(result, "_", TextType.Italic);
        result = SplitNodesDelimiter(result, "`", TextType.Code);
        result = SplitNodesImage(result);
        result = SplitNodesLink(result);

        return result;
    }
}
