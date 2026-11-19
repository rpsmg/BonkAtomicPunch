# Paints bonkcan.png (4x9, same size and shading columns as the game's soda can).
# Yellow can, red top/bottom bands, red "BONK" stripe and radiation dot. Silver rims copied from the vanilla can.
# Run: python paint_can.py  (needs Pillow)
from PIL import Image
PAL = {
    "t": [(161, 161, 171), (160, 169, 172), (170, 168, 158), (168, 157, 162)],  # top rim
    "b": [(164, 155, 152), (157, 165, 159), (152, 154, 160), (167, 157, 158)],  # bottom rim
    "R": [(122, 22, 22), (230, 70, 58), (196, 42, 36), (110, 20, 20)],           # red band / lettering
    "Y": [(184, 150, 16), (255, 236, 96), (242, 204, 28), (166, 134, 12)],        # yellow body
    "X": [(184, 150, 16), (255, 236, 96), (196, 42, 36), (166, 134, 12)],         # yellow with radiation dot
}
ART = "tRYRYXYRb"  # one letter per row, top to bottom
im = Image.new("RGBA", (4, len(ART)))
for y, k in enumerate(ART):
    for x, c in enumerate(PAL[k]):
        im.putpixel((x, y), c + (255,))
im.save("bonkcan.png")

# bonked.png: 20x20 moodle icon (vanilla moodle size, 33.3 px/unit). Same can, bigger, with a radiation trefoil.
MPAL = {
    "t": (200, 200, 208), "u": (150, 150, 158),  # rim, rim shade
    "R": (220, 56, 46), "r": (150, 28, 24),      # red band, shade
    "Y": (250, 214, 40), "y": (190, 150, 16),    # yellow body, shade
    "K": (178, 30, 30),                          # trefoil
}
MART = [
    "ttttttttuu",
    "RRRRRRRRrr",
    "RRRRRRRRrr",
    "YYYYYYYYyy",
    "YKKYYKKYyy",
    "YKKYYKKYyy",
    "YYYKKYYYyy",
    "YYYKKYYYyy",
    "YYKKKKYYyy",
    "YYYYYYYYyy",
    "RRRRRRRRrr",
    "RRRRRRRRrr",
    "ttttttttuu",
]
icon = Image.new("RGBA", (20, 20))
for y, row in enumerate(MART):
    for x, k in enumerate(row):
        icon.putpixel((5 + x, 3 + y), MPAL[k] + (255,))
icon.save("bonked.png")
