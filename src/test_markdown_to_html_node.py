import unittest
from markdown_to_html_node import markdown_to_html_node

class TestMarkdownToHtmlNode(unittest.TestCase):
    def test_headings(self):
        md = """
# This is a heading

## This is a heading with **bold** text
"""

        node = markdown_to_html_node(md)
        html = node.to_html()
        self.assertEqual(
            html,
            "<div><h1>This is a heading</h1><h2>This is a heading with <b>bold</b> text</h2></div>",
        )

    def test_quote(self):
        md = """
> This is a quote
> that spans multiple lines
> with **bold** text
"""

        node = markdown_to_html_node(md)
        html = node.to_html()
        self.assertEqual(
            html,
            "<div><blockquote>This is a quote that spans multiple lines with <b>bold</b> text</blockquote></div>",
        )

    def test_unordered_list(self):
        md = """
- Primeiro item
- Segundo item com **negrito**
- Terceiro item com _itálico_
"""

        node = markdown_to_html_node(md)
        html = node.to_html()
        self.assertEqual(
            html,
            "<div><ul><li>Primeiro item</li><li>Segundo item com <b>negrito</b></li><li>Terceiro item com <i>itálico</i></li></ul></div>",
        )

if __name__ == "__main__":
    unittest.main()