import unittest
from textnode import TextNode
from composable import text_node_to_html_node, split_nodes_image, text_to_textnodes, markdown_to_blocks
from enums import TextType

class TestTextNode(unittest.TestCase):
    def test_eq(self):
        node = TextNode("This is a text node", TextType.BOLD)
        node2 = TextNode("This is a text node", TextType.BOLD)

        self.assertEqual(node, node2)
    
    def test_not_equal(self):
        node = TextNode("This is a text node", TextType.BOLD)
        node2 = TextNode("This is not a text node", TextType.TEXT)

        self.assertNotEqual(node, node2)

    def test_url_is_none(self):
        node = TextNode("This is a text node", TextType.BOLD)

        self.assertEqual(node.url, None)

    def test_text(self):
        node = TextNode(text="This is a text node", text_type=TextType.TEXT)
        html_node = text_node_to_html_node(node)
        self.assertEqual(html_node.tag, None)
        self.assertEqual(html_node.value, "This is a text node")

    def test_bold(self):
        node = TextNode(text="This is a text bold", text_type=TextType.BOLD)
        html_node = text_node_to_html_node(node)

        self.assertEqual(html_node.tag, "b")
        self.assertEqual(html_node.value, "This is a text bold")
        self.assertEqual(html_node.props, None)
    
    def test_italic(self):
        node = TextNode(text="This is a text italic", text_type=TextType.ITALIC)
        html_node = text_node_to_html_node(node)

        self.assertEqual(html_node.tag, "i")
        self.assertEqual(html_node.value, "This is a text italic")
        self.assertEqual(html_node.props, None)
    
    def test_code(self):
        node = TextNode(text="This is a text code", text_type=TextType.CODE)
        html_node = text_node_to_html_node(node)

        self.assertEqual(html_node.tag, "code")
        self.assertEqual(html_node.value, "This is a text code")
        self.assertEqual(html_node.props, None)
    
    def test_link(self):
        node = TextNode(text="This is a link", text_type=TextType.LINK, url="https://google.com")
        html_node = text_node_to_html_node(node)

        self.assertEqual(html_node.tag, "a")
        self.assertEqual(html_node.value, "This is a link")
        self.assertEqual(html_node.props, {"href": "https://google.com"})

    def test_image(self):
        node = TextNode(text="Image of Google", text_type=TextType.IMAGE, url="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSgtOvib9DAid8R8y96JhR4zVTyaFB9N2XIDxe29v7Wng&s")
        html_node = text_node_to_html_node(node)

        self.assertEqual(html_node.tag, "img")
        self.assertEqual(html_node.value, "")
        self.assertEqual(html_node.props, {"src": "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSgtOvib9DAid8R8y96JhR4zVTyaFB9N2XIDxe29v7Wng&s", "alt": "Image of Google"})

    def test_split_images(self):
        node = TextNode(
            "This is text with an ![image](https://i.imgur.com/zjjcJKZ.png) and another ![second image](https://i.imgur.com/3elNhQu.png)",
            TextType.TEXT,
        )

        new_nodes = split_nodes_image([node])
        
        self.assertListEqual(
            [
                TextNode("This is text with an ", TextType.TEXT),
                TextNode("image", TextType.IMAGE, "https://i.imgur.com/zjjcJKZ.png"),
                TextNode(" and another ", TextType.TEXT),
                TextNode(
                    "second image", TextType.IMAGE, "https://i.imgur.com/3elNhQu.png"
                ),
            ],
            new_nodes,
        )
    
    def test_text_to_textnodes(self):
        text = "This is **text** with an _italic_ word and a `code block` and an ![obi wan image](https://i.imgur.com/fJRm4Vk.jpeg) and a [link](https://boot.dev)"

        nodes = text_to_textnodes(text)
        
        self.assertListEqual(
            [
                TextNode("This is ", TextType.TEXT),
                TextNode("text", TextType.BOLD),
                TextNode(" with an ", TextType.TEXT),
                TextNode("italic", TextType.ITALIC),
                TextNode(" word and a ", TextType.TEXT),
                TextNode("code block", TextType.CODE),
                TextNode(" and an ", TextType.TEXT),
                TextNode("obi wan image", TextType.IMAGE, "https://i.imgur.com/fJRm4Vk.jpeg"),
                TextNode(" and a ", TextType.TEXT),
                TextNode("link", TextType.LINK, "https://boot.dev"),
            ],
            nodes,
        )

        def test_markdown_to_blocks(self):
            md = """
This is **bolded** paragraph

This is another paragraph with _italic_ text and `code` here
This is the same paragraph on a new line

- This is a list
- with items
"""
            blocks = markdown_to_blocks(md)
            self.assertEqual(
                blocks,
                [
                    "This is **bolded** paragraph",
                    "This is another paragraph with _italic_ text and `code` here\nThis is the same paragraph on a new line",
                    "- This is a list\n- with items",
                ],
            )

if __name__ == "__main__":
    unittest.main()