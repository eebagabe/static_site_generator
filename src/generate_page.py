def extract_title(markdown):
    for line in markdown.split("\n"):
        if line.startswith("# "):
            return line[len("# "):].strip()

    raise Exception("Don't have a title in the markdown")

