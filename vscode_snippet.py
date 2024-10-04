import json

import pyperclip


def main():
    name = "INPUT_HERE"
    code = get_original_code()
    data = make_data(name, code)
    pyperclip.copy(data)


def get_original_code():
    code = pyperclip.paste()
    return code


def make_data(name: str, code: str) -> str:
    lines = convert_code(code)

    content = {
        "prefix": name,
        "body": lines,
    }

    return f'"{name}": {json.dumps(content, indent=4, ensure_ascii=False)},'


def convert_code(raw_code: str) -> list[str]:
    escaped_code = raw_code.replace("$", "$$")
    lines = remove_empty_lines(escaped_code.splitlines())

    common_indent = detect_common_indent(lines)
    if not common_indent:
        return lines

    dedented_lines = []
    for line in lines:
        if line.startswith(common_indent):
            dedented_lines.append(line[len(common_indent) :])
        else:
            dedented_lines.append(line)
    return dedented_lines


def remove_empty_lines(lines: list[str]) -> list[str]:
    while lines and not lines[0].strip():
        lines.pop(0)
    while lines and not lines[-1].strip():
        lines.pop()
    return lines


def detect_common_indent(lines: list[str]) -> str:
    indents = []
    for line in lines:
        if line.strip():
            indent = get_line_indent(line)
            indents.append(indent)

    common_indent = indents[0]
    for indent in indents[1:]:
        common_length = min(len(common_indent), len(indent))
        i = 0
        while i < common_length and common_indent[i] == indent[i]:
            i += 1
        common_indent = common_indent[:i]
        if not common_indent:
            break

    return common_indent


def get_line_indent(line: str) -> str:
    indent = ""
    for char in line:
        if char == " " or char == "\t":
            indent += char
        else:
            break
    return indent


if __name__ == "__main__":
    main()
