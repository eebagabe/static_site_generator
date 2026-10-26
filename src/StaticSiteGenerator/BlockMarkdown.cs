using StaticSiteGenerator.Enums;

namespace StaticSiteGenerator;

public static class BlockMarkdown
{
    public static List<string> MarkdownToBlocks(string markdown) =>
        markdown.Split("\n\n")
            .Select(block => block.Trim())
            .Where(block => block != "")
            .ToList();

    public static BlockType BlockToBlockType(string block)
    {
        if (HeadingLevel(block) > 0)
            return BlockType.Heading;

        if (block.StartsWith("```\n") && block.EndsWith("\n```"))
            return BlockType.Code;

        var lines = block.Split("\n");

        if (lines.All(line => line.StartsWith('>')))
            return BlockType.Quote;

        if (lines.All(line => line.StartsWith("- ")))
            return BlockType.UnorderedList;

        if (lines.Select((line, index) => line.StartsWith($"{index + 1}. ")).All(ok => ok))
            return BlockType.OrderedList;

        return BlockType.Paragraph;
    }

    // Returns 1-6 for "# " .. "###### ", 0 when the block is not a heading.
    public static int HeadingLevel(string block)
    {
        for (var level = 1; level <= 6; level++)
        {
            if (block.StartsWith(new string('#', level) + " "))
                return level;
        }

        return 0;
    }
}
