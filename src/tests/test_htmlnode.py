import unittest
from htmlnode import HTMLNode, LeafNode, ParentNode

class TestHTMLNode(unittest.TestCase):
    def test_to_html(self):
        html_node = HTMLNode("p", "testing p", None, None)
        
        self.assertRaises(NotImplementedError, html_node.to_html)

    def test_props_to_html_with_props_none(self):
        node = HTMLNode("p", "testing p", None, None)

        self.assertEqual("", node.props_to_html())

    def test_props_to_html_with_props(self):
        props = {
            "href": "https://www.google.com",
            "target": "_blank",
        }
        
        node = HTMLNode("a", "Teste de link", None, props=props)

        self.assertEqual(' href="https://www.google.com" target="_blank"', node.props_to_html())

class TestLeafNode(unittest.TestCase):
    def test_leaf_to_html_p(self):
        node = LeafNode("p", "Hello, world!")
        self.assertEqual(node.to_html(), "<p>Hello, world!</p>")

    def test_to_html_without_value(self):
        node = LeafNode("p", None, props=None)

        self.assertRaises(ValueError, node.to_html)

class TestParentNode(unittest.TestCase):
    def test_to_html_with_children(self):
        child_node = LeafNode("span", "child")
        parent_node = ParentNode("div", [child_node])
        
        self.assertEqual(parent_node.to_html(), "<div><span>child</span></div>")

    def test_to_html_with_grandchildren(self):
        grandchild_node = LeafNode("b", "grandchild")
        child_node = ParentNode("span", [grandchild_node])
        parent_node = ParentNode("div", [child_node])
        
        self.assertEqual(
            parent_node.to_html(),
            "<div><span><b>grandchild</b></span></div>",
        )

if __name__ == "__main__":
    unittest.main()