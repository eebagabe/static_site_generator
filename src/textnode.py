from enum import Enum
from htmlnode import LeafNode
import re

class TextType(Enum):
    TEXT = "text"
    BOLD = "bold"
    ITALIC = "italic"
    CODE = "code"
    LINK = "link"
    IMAGE = "image"

class TextNode:
    def __init__(self, text, text_type, url=None):
        self.text = text
        self.text_type = text_type
        self.url = url

    def __eq__(self, other):
        if self.text == other.text and self.text_type == other.text_type and  self.url == other.url:
            return True

        return False

    def __repr__(self):
        return f"TextNode({self.text}, {self.text_type.value}, {self.url})"
    
def text_node_to_html_node(text_node: TextNode) -> LeafNode:
    if text_node.text_type == TextType.TEXT:
        return LeafNode(tag=None, value=text_node.text, props=None)

    if text_node.text_type == TextType.BOLD:
        return LeafNode("b", value=text_node.text, props=None)

    if text_node.text_type == TextType.ITALIC:
        return LeafNode("i", value=text_node.text, props=None)

    if text_node.text_type == TextType.CODE:
        return LeafNode("code", value=text_node.text, props=None)

    if text_node.text_type == TextType.LINK:
        return LeafNode("a", value=text_node.text, props={"href": text_node.url})

    if text_node.text_type == TextType.IMAGE:
        return LeafNode("img", value="", props={"src": text_node.url, "alt": text_node.text})

    raise ValueError("Invalid type")

def split_nodes_delimiter(old_notes: list[TextNode], delimiter: str, text_type: TextType) -> list[TextNode]:
    result = []

    for node in old_notes:
        if node.text_type is not TextType.TEXT:
            result.append(node)
        
        elif node.text_type is TextType.TEXT:
            parts = node.text.split(delimiter)

            for index, part in enumerate(parts):
                if index % 2 == 0:
                    result.append(TextNode(part, TextType.TEXT))
                
                if index % 2 == 1:
                    result.append(TextNode(part, text_type))
    
    return result

def extract_markdown_images(text):
    return re.findall(r"!\[([^\[\]]*)\]\(([^\(\)]*)\)", text)

def extract_markdown_links(text):
    return re.findall(r"(?<!!)\[([^\[\]]*)\]\(([^\(\)]*)\)",text)

def split_nodes_image(old_nodes: list[TextNode]) -> list[TextNode]:
    new_list = []

    for node in old_nodes:
        if node.text_type is not TextType.TEXT:
            new_list.append(node)
            continue

        md_links = extract_markdown_images(node.text)

        if len(md_links) == 0:
            new_list.append(node)
            continue

        remaining_text = node.text

        for alt_text, url in md_links:
            sections = remaining_text.split(f"![{alt_text}]({url})", 1)

            before = sections[0]
            after = sections[1]

            if before != "":
               new_list.append(TextNode(before, TextType.TEXT))
            
            new_list.append(TextNode(alt_text, TextType.IMAGE, url))
            remaining_text = after

        if remaining_text != "":
            new_list.append(TextNode(remaining_text, TextType.TEXT))

    return new_list

def split_nodes_link(old_nodes: list[TextNode]) -> list[TextNode]:
    new_list = []

    for node in old_nodes:
        if node.text_type is not TextType.TEXT:
            new_list.append(node)
            continue

        md_links = extract_markdown_links(node.text)

        if len(md_links) == 0:
            new_list.append(node)
            continue

        remaining_text = node.text

        for link_text, url in md_links:
            sections = remaining_text.split(f"[{link_text}]({url})", 1)

            before = sections[0]
            after = sections[1]

            if before != "":
               new_list.append(TextNode(before, TextType.TEXT))
            
            new_list.append(TextNode(link_text, TextType.LINK, url))
            remaining_text = after

        if remaining_text != "":
            new_list.append(TextNode(remaining_text, TextType.TEXT))

    return new_list

def text_to_textnodes(text):
    node = TextNode(text, TextType.TEXT)
    initial_list = [node]

    result = split_nodes_delimiter(initial_list, '**', TextType.BOLD)
    result = split_nodes_delimiter(result, '_', TextType.ITALIC)
    result = split_nodes_delimiter(result, '`', TextType.CODE)
    result = split_nodes_image(result)
    result = split_nodes_link(result)

    return result
    

