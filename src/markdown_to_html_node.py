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
            text_without_double_whitespace = text_without_break_line.split()
            text_reuslt = " ".join(text_without_double_whitespace)
            
            content = text_to_children(text_reuslt)

            html_nodes.append(ParentNode(tag="p", children=content))

        elif block_type == BlockType.HEADING:
            for i in range(1, 7):
                tmp = "#"
                tmp *= i

                if block.startswith(tmp + " "):
                    heading_text = clear_prefix_heading(block, i)
                    heading_result = text_to_children(heading_text)
                    heading_content = ParentNode(tag=f"h{i}", children=heading_result)

                    html_nodes.append(heading_content)
                    break
        
        elif block_type == BlockType.QUOTE:
            blocks_quote = block.split("\n")

            quote_array = []

            for quote in blocks_quote:
                quote_text_cleaned = clear_quote_prefix(quote)
                quote_array.append(quote_text_cleaned)

            quote_phrase_result = text_to_children(" ".join(quote_array))
            html_nodes.append(ParentNode(tag="blockquote", children=quote_phrase_result))



    return ParentNode(tag='div', children=html_nodes)

def clear_quote_prefix(text):
    return text[2:]

def clear_prefix_and_sufix(block, block_type):
    if block_type == BlockType.CODE:
        text = block[3:-3]
        return text.strip("\n")

def clear_prefix_heading(block, times):
    return block[times + 1:]

def text_to_children(content):
    text_nodes = text_to_textnodes(content)

    html_nodes = []

    for node in text_nodes:
        html_nodes.append(text_node_to_html_node(node))

    return html_nodes
