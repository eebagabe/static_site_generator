import os
import shutil

def extract_title(markdown):
     
     pass

def source_to_public_dir(source, destine_path):
    content = os.listdir(source)

    for item in content:
        relative_path = os.path.join(source, item)

        if os.path.isfile(relative_path):
            shutil.copy(relative_path, destine_path)
            print("Path Inicial: ", relative_path)
            print("Path Final: ", destine_path)
        else:
            origin = os.path.join(source, item)
            destine = os.path.join(destine_path, item)

            print("Diretorio Inicial: ", origin)
            print("Diretorio Final: ", destine)

            os.mkdir(destine)
            source_to_public_dir(origin, destine)

def copy_static_to_public(destine):
    if os.path.exists(destine):
            shutil.rmtree(destine)
    
    os.mkdir(destine)

    source_to_public_dir("static", "public")


def main():
    copy_static_to_public("public")

main()