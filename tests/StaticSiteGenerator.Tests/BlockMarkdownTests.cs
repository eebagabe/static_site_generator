using StaticSiteGenerator.Enums;

namespace StaticSiteGenerator.Tests;

public class BlockMarkdownTests
{
    [Fact]
    public void MarkdownToBlocks()
    {
        var md = """

            This is **bolded** paragraph

            This is another paragraph with _italic_ text and `code` here
            This is the same paragraph on a new line

            - This is a list
            - with items

            """;

        Assert.Equal(
            [
                "This is **bolded** paragraph",
                "This is another paragraph with _italic_ text and `code` here\nThis is the same paragraph on a new line",
                "- This is a list\n- with items",
            ],
            BlockMarkdown.MarkdownToBlocks(md));
    }

    [Theory]
    [InlineData("isso aqui é só um texto comum, sem formatação especial", BlockType.Paragraph)]
    [InlineData("### um título de nível 3", BlockType.Heading)]
    [InlineData("####### não é título", BlockType.Paragraph)]
    [InlineData("```\ncodigo\n```", BlockType.Code)]
    [InlineData("> citação\n>\n> autor", BlockType.Quote)]
    [InlineData("- primeiro item\n- segundo item\n- terceiro item", BlockType.UnorderedList)]
    [InlineData("1. primeiro passo\n2. segundo passo\n3. terceiro passo", BlockType.OrderedList)]
    [InlineData("1. primeiro passo\n3. terceiro passo", BlockType.Paragraph)]
    public void BlockToBlockType(string block, BlockType expected)
    {
        Assert.Equal(expected, BlockMarkdown.BlockToBlockType(block));
    }
}
