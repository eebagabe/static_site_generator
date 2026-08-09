import unittest
from generate_page import extract_title

class TestGeneratePage(unittest.TestCase):
    def test_finds_header(self):
        self.assertEqual("Hello", extract_title("# Hello"))

if __name__ == "__main__":
    unittest.main()