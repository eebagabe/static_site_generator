from composable import markdown_to_blocks, block_to_block_type
from htmlnode import HTMLNode, ParentNode, LeafNode
from enums import BlockType

# Converts a full markdown document into a single parent HTMLNode
def markdown_to_html_node(markdown):

    # 01. Split the markdown into blocks
    blocks = markdown_to_blocks(markdown)

    html_nodes = []

    for block in blocks:
        block_type = block_to_block_type(block)

        if block_type == BlockType.CODE:
            content = clear_prefix_and_sufix(block, block_type)
            leaf_node = LeafNode(tag='code', value=content)
            parent_node = ParentNode(tag='pre', children=[leaf_node])

            html_nodes.append(parent_node)

    return ParentNode(tag='div', children=html_nodes)

def clear_prefix_and_sufix(block, block_type):
    if block_type == BlockType.CODE:
        text = block[3:-3]
        return text.strip("\n")