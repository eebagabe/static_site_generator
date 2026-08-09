import unittest
from enums.block_type import BlockType
from block_markdown import block_to_block_type


class TestBlockToBlockType(unittest.TestCase):
    def test_paragraph(self):
        block = "isso aqui é só um texto comum, sem formatação especial"
        block_type = block_to_block_type(block)
        self.assertEqual(block_type, BlockType.PARAGRAPH)

    def test_heading_level_three(self):
        block = "### um título de nível 3"
        block_type = block_to_block_type(block)
        self.assertEqual(block_type, BlockType.HEADING)

    def test_unordered_list(self):
        block = "- primeiro item\n- segundo item\n- terceiro item"
        block_type = block_to_block_type(block)
        self.assertEqual(block_type, BlockType.UNORDERED_LIST)

    def test_ordered_list(self):
        block = "1. primeiro passo\n2. segundo passo\n3. terceiro passo"
        block_type = block_to_block_type(block)
        self.assertEqual(block_type, BlockType.ORDERED_LIST)


if __name__ == "__main__":
    unittest.main()