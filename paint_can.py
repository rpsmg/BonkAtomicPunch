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
