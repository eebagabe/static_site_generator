from enums.block_type import BlockType

def markdown_to_blocks(markdown):
    result = []

    array = markdown.split("\n\n")

    for item in array:
        if item.strip() != "":
            result.append(item.strip())

    return result

def block_to_block_type(block):
    for i in range(1, 7):
        tmp = "#"
        tmp *= i

        if block.startswith(tmp + " "):
            return BlockType.HEADING

    if block.startswith("```\n") and block.endswith("\n```"):
        return BlockType.CODE


    lines_block = block.split("\n")

    is_quote = True

    for line in lines_block:
        if not line.startswith(">"):
            is_quote = False
            break

    if is_quote:
        return BlockType.QUOTE

    is_unordered_list = True

    for line in lines_block:
        if not line.startswith("- "):
            is_unordered_list = False
            break

    if is_unordered_list:
        return BlockType.UNORDERED_LIST


    is_ordered_list = True

    for index, line in enumerate(lines_block, start=1):
        prefix = f"{index}. "

        if not line.startswith(prefix):
            is_ordered_list = False
            break

    if is_ordered_list:
        return BlockType.ORDERED_LIST

    return BlockType.PARAGRAPH