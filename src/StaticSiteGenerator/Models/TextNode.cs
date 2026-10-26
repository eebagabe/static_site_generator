using StaticSiteGenerator.Enums;

namespace StaticSiteGenerator.Models;

public record TextNode(string Text, TextType TextType, string? Url = null);
