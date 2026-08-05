from composable import markdown_to_blocks, block_to_block_type, text_to_textnodes, text_node_to_html_node
from htmlnode import ParentNode, LeafNode
from enums import BlockType

def markdown_to_html_node(markdown):
    blocks = markdown_to_blocks(markdown)
    html_nodes = []

    for block in blocks:
        block_type = block_to_block_type(block)

        if block_type == BlockType.CODE:
            content = clear_prefix_and_sufix(block, block_type)
            leaf_node = LeafNode(tag='code', value=content)
            parent_node = ParentNode(tag='pre', children=[leaf_node])

            html_nodes.append(parent_node)

        elif block_type == BlockType.PARAGRAPH:
            text_without_break_line = block.replace("\n", " ")
            content = text_to_children(text_without_break_line)

            html_nodes.append(ParentNode(tag="p", children=content))


    return ParentNode(tag='div', children=html_nodes)

def clear_prefix_and_sufix(block, block_type):
    if block_type == BlockType.CODE:
        text = block[3:-3]
        return text.strip("\n")

def text_to_children(content):
    text_nodes = text_to_textnodes(content)

    html_nodes = []

    for node in text_nodes:
        html_nodes.append(text_node_to_html_node(node))

    return html_nodes