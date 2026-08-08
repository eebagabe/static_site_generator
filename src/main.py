from textnode import TextNode, TextType
import os
import shutil

def source_to_public_dir():
    if os.path.exists("public"):
        shutil.rmtree("public")

    os.mkdir("public")

def main():
    teste = TextNode("This is some anchor text", TextType.TEXT, "https://www.boot.dev")
    print(teste)

main()