using StaticSiteGenerator.Enums;
using StaticSiteGenerator.Models;

namespace StaticSiteGenerator;

public static class MarkdownToHtmlNode
{
    public static ParentNode Convert(string markdown)
    {
        var htmlNodes = new List<HtmlNode>();

        foreach (var block in BlockMarkdown.MarkdownToBlocks(markdown))
        {
            var lines = block.Split("\n");

            switch (BlockMarkdown.BlockToBlockType(block))
            {
                case BlockType.Code:
                    var code = block[3..^3].Trim('\n');
                    htmlNodes.Add(new ParentNode("pre", [new LeafNode("code", code)]));
                    break;

                case BlockType.Paragraph:
                    var paragraph = string.Join(" ", block.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
                    htmlNodes.Add(new ParentNode("p", TextToChildren(paragraph)));
                    break;

                case BlockType.Heading:
                    var level = BlockMarkdown.HeadingLevel(block);
                    htmlNodes.Add(new ParentNode($"h{level}", TextToChildren(block[(level + 1)..])));
                    break;

                case BlockType.Quote:
                    var quote = string.Join(" ", lines.Select(ClearPrefix));
                    htmlNodes.Add(new ParentNode("blockquote", TextToChildren(quote)));
                    break;

                case BlockType.UnorderedList:
                    var unorderedItems = lines.Select(line => (HtmlNode)new ParentNode("li", TextToChildren(ClearPrefix(line))));
                    htmlNodes.Add(new ParentNode("ul", unorderedItems.ToList()));
                    break;

                case BlockType.OrderedList:
                    var orderedItems = lines.Select((line, index) => (HtmlNode)new ParentNode("li", TextToChildren(ClearPrefixNumber(line, index + 1))));
                    htmlNodes.Add(new ParentNode("ol", orderedItems.ToList()));
                    break;
            }
        }

        return new ParentNode("div", htmlNodes);
    }

    private static string ClearPrefixNumber(string text, int index) => text[(index.ToString().Length + 2)..];

    // Lines like ">" (empty quote line) are shorter than the prefix.
    private static string ClearPrefix(string text) => text.Length > 2 ? text[2..] : "";

    private static List<HtmlNode> TextToChildren(string text) =>
        InlineMarkdown.TextToTextNodes(text).Select(node => (HtmlNode)InlineMarkdown.TextNodeToHtmlNode(node)).ToList();
}
