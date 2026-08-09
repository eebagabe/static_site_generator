import os
import shutil

def source_to_public_dir(source, destination):
    content = os.listdir(source)

    for item in content:
        source_path = os.path.join(source, item)

        if os.path.isfile(source_path):
            shutil.copy(source_path, destination)
        else:
            destination_path = os.path.join(destination, item)

            os.mkdir(destination_path)
            source_to_public_dir(source_path, destination_path)


def copy_static_to_public(destination):
    if os.path.exists(destination):
        shutil.rmtree(destination)

    os.mkdir(destination)

    source_to_public_dir("static", destination)