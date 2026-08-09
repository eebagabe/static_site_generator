def extract_title(markdown):
    for line in markdown.split("\n"):
        if line.startswith("# "):
            return line[len("# "):].strip()

    raise Exception("Don't have a title in the markdown")

def generate_page(from_path, template_path, dest_path):
    print(f"Generating page from {from_path} to {dest_path} using {template_path}")