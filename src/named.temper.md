From https://drafts.csswg.org/css-color-4/#named-colors

## 6.1. Named Colors

CSS defines a large set of named colors, so that common colors can be written
and read more easily.

A `<named-color>` is written as an `<ident>`, accepted anywhere a `<color>` is.
As usual for CSS-defined `<ident>`s, all of these keywords are ASCII
case-insensitive. The names resolve to colors in sRGB.

16 of CSS's named colors come from the VGA palette originally, and were then
adopted into HTML: aqua, black, blue, fuchsia, gray, green, lime, maroon, navy,
olive, purple, red, silver, teal, white, and yellow. Most of the rest come from
one version of the X11 color system, used in Unix-derived systems to specify
colors for the console, and were then adopted into SVG.

> **Note:** these color names are standardized here, not because they are good,
> but because their use and implementation has been widespread for decades and
> the standard needs to reflect reality.
>
> Indeed, it is often hard to imagine what each name will look like (hence the
> list below); the names are not evenly distributed throughout the sRGB color
> volume, the names are not even internally consistent (darkgray is lighter than
> gray, while lightpink is darker than pink), and some names (such as indianred,
> which was originally named after a red pigment from India), have been found to
> be offensive.
>
> Thus, their use is not encouraged.

(Two special color values, transparent and currentcolor, are specially defined
in their own sections.)

The following table defines all of the opaque named colors, by giving equivalent
numeric specifications in the other color syntaxes.

    export let namedColors = new Map([
      new Pair("aliceblue", [240, 248, 255]), // #f0f8ff
      new Pair("antiquewhite", [250, 235, 215]), // #faebd7
      new Pair("aqua", [0, 255, 255]), // #00ffff
      new Pair("aquamarine", [127, 255, 212]), // #7fffd4
      new Pair("azure", [240, 255, 255]), // #f0ffff
      new Pair("beige", [245, 245, 220]), // #f5f5dc
      new Pair("bisque", [255, 228, 196]), // #ffe4c4
      new Pair("black", [0, 0, 0]), // #000000
      new Pair("blanchedalmond", [255, 235, 205]), // #ffebcd
      new Pair("blue", [0, 0, 255]), // #0000ff
      new Pair("blueviolet", [138, 43, 226]), // #8a2be2
      new Pair("brown", [165, 42, 42]), // #a52a2a
      new Pair("burlywood", [222, 184, 135]), // #deb887
      new Pair("cadetblue", [95, 158, 160]), // #5f9ea0
      new Pair("chartreuse", [127, 255, 0]), // #7fff00
      new Pair("chocolate", [210, 105, 30]), // #d2691e
      new Pair("coral", [255, 127, 80]), // #ff7f50
      new Pair("cornflowerblue", [100, 149, 237]), // #6495ed
      new Pair("cornsilk", [255, 248, 220]), // #fff8dc
      new Pair("crimson", [220, 20, 60]), // #dc143c
      new Pair("cyan", [0, 255, 255]), // #00ffff
      new Pair("darkblue", [0, 0, 139]), // #00008b
      new Pair("darkcyan", [0, 139, 139]), // #008b8b
      new Pair("darkgoldenrod", [184, 134, 11]), // #b8860b
      new Pair("darkgray", [169, 169, 169]), // #a9a9a9
      new Pair("darkgreen", [0, 100, 0]), // #006400
      new Pair("darkgrey", [169, 169, 169]), // #a9a9a9
      new Pair("darkkhaki", [189, 183, 107]), // #bdb76b
      new Pair("darkmagenta", [139, 0, 139]), // #8b008b
      new Pair("darkolivegreen", [85, 107, 47]), // #556b2f
      new Pair("darkorange", [255, 140, 0]), // #ff8c00
      new Pair("darkorchid", [153, 50, 204]), // #9932cc
      new Pair("darkred", [139, 0, 0]), // #8b0000
      new Pair("darksalmon", [233, 150, 122]), // #e9967a
      new Pair("darkseagreen", [143, 188, 143]), // #8fbc8f
      new Pair("darkslateblue", [72, 61, 139]), // #483d8b
      new Pair("darkslategray", [47, 79, 79]), // #2f4f4f
      new Pair("darkslategrey", [47, 79, 79]), // #2f4f4f
      new Pair("darkturquoise", [0, 206, 209]), // #00ced1
      new Pair("darkviolet", [148, 0, 211]), // #9400d3
      new Pair("deeppink", [255, 20, 147]), // #ff1493
      new Pair("deepskyblue", [0, 191, 255]), // #00bfff
      new Pair("dimgray", [105, 105, 105]), // #696969
      new Pair("dimgrey", [105, 105, 105]), // #696969
      new Pair("dodgerblue", [30, 144, 255]), // #1e90ff
      new Pair("firebrick", [178, 34, 34]), // #b22222
      new Pair("floralwhite", [255, 250, 240]), // #fffaf0
      new Pair("forestgreen", [34, 139, 34]), // #228b22
      new Pair("fuchsia", [255, 0, 255]), // #ff00ff
      new Pair("gainsboro", [220, 220, 220]), // #dcdcdc
      new Pair("ghostwhite", [248, 248, 255]), // #f8f8ff
      new Pair("gold", [255, 215, 0]), // #ffd700
      new Pair("goldenrod", [218, 165, 32]), // #daa520
      new Pair("gray", [128, 128, 128]), // #808080
      new Pair("green", [0, 128, 0]), // #008000
      new Pair("greenyellow", [173, 255, 47]), // #adff2f
      new Pair("grey", [128, 128, 128]), // #808080
      new Pair("honeydew", [240, 255, 240]), // #f0fff0
      new Pair("hotpink", [255, 105, 180]), // #ff69b4
      new Pair("indianred", [205, 92, 92]), // #cd5c5c
      new Pair("indigo", [75, 0, 130]), // #4b0082
      new Pair("ivory", [255, 255, 240]), // #fffff0
      new Pair("khaki", [240, 230, 140]), // #f0e68c
      new Pair("lavender", [230, 230, 250]), // #e6e6fa
      new Pair("lavenderblush", [255, 240, 245]), // #fff0f5
      new Pair("lawngreen", [124, 252, 0]), // #7cfc00
      new Pair("lemonchiffon", [255, 250, 205]), // #fffacd
      new Pair("lightblue", [173, 216, 230]), // #add8e6
      new Pair("lightcoral", [240, 128, 128]), // #f08080
      new Pair("lightcyan", [224, 255, 255]), // #e0ffff
      new Pair("lightgoldenrodyellow", [250, 250, 210]), // #fafad2
      new Pair("lightgray", [211, 211, 211]), // #d3d3d3
      new Pair("lightgreen", [144, 238, 144]), // #90ee90
      new Pair("lightgrey", [211, 211, 211]), // #d3d3d3
      new Pair("lightpink", [255, 182, 193]), // #ffb6c1
      new Pair("lightsalmon", [255, 160, 122]), // #ffa07a
      new Pair("lightseagreen", [32, 178, 170]), // #20b2aa
      new Pair("lightskyblue", [135, 206, 250]), // #87cefa
      new Pair("lightslategray", [119, 136, 153]), // #778899
      new Pair("lightslategrey", [119, 136, 153]), // #778899
      new Pair("lightsteelblue", [176, 196, 222]), // #b0c4de
      new Pair("lightyellow", [255, 255, 224]), // #ffffe0
      new Pair("lime", [0, 255, 0]), // #00ff00
      new Pair("limegreen", [50, 205, 50]), // #32cd32
      new Pair("linen", [250, 240, 230]), // #faf0e6
      new Pair("magenta", [255, 0, 255]), // #ff00ff
      new Pair("maroon", [128, 0, 0]), // #800000
      new Pair("mediumaquamarine", [102, 205, 170]), // #66cdaa
      new Pair("mediumblue", [0, 0, 205]), // #0000cd
      new Pair("mediumorchid", [186, 85, 211]), // #ba55d3
      new Pair("mediumpurple", [147, 112, 219]), // #9370db
      new Pair("mediumseagreen", [60, 179, 113]), // #3cb371
      new Pair("mediumslateblue", [123, 104, 238]), // #7b68ee
      new Pair("mediumspringgreen", [0, 250, 154]), // #00fa9a
      new Pair("mediumturquoise", [72, 209, 204]), // #48d1cc
      new Pair("mediumvioletred", [199, 21, 133]), // #c71585
      new Pair("midnightblue", [25, 25, 112]), // #191970
      new Pair("mintcream", [245, 255, 250]), // #f5fffa
      new Pair("mistyrose", [255, 228, 225]), // #ffe4e1
      new Pair("moccasin", [255, 228, 181]), // #ffe4b5
      new Pair("navajowhite", [255, 222, 173]), // #ffdead
      new Pair("navy", [0, 0, 128]), // #000080
      new Pair("oldlace", [253, 245, 230]), // #fdf5e6
      new Pair("olive", [128, 128, 0]), // #808000
      new Pair("olivedrab", [107, 142, 35]), // #6b8e23
      new Pair("orange", [255, 165, 0]), // #ffa500
      new Pair("orangered", [255, 69, 0]), // #ff4500
      new Pair("orchid", [218, 112, 214]), // #da70d6
      new Pair("palegoldenrod", [238, 232, 170]), // #eee8aa
      new Pair("palegreen", [152, 251, 152]), // #98fb98
      new Pair("paleturquoise", [175, 238, 238]), // #afeeee
      new Pair("palevioletred", [219, 112, 147]), // #db7093
      new Pair("papayawhip", [255, 239, 213]), // #ffefd5
      new Pair("peachpuff", [255, 218, 185]), // #ffdab9
      new Pair("peru", [205, 133, 63]), // #cd853f
      new Pair("pink", [255, 192, 203]), // #ffc0cb
      new Pair("plum", [221, 160, 221]), // #dda0dd
      new Pair("powderblue", [176, 224, 230]), // #b0e0e6
      new Pair("purple", [128, 0, 128]), // #800080
      new Pair("rebeccapurple", [102, 51, 153]), // #663399
      new Pair("red", [255, 0, 0]), // #ff0000
      new Pair("rosybrown", [188, 143, 143]), // #bc8f8f
      new Pair("royalblue", [65, 105, 225]), // #4169e1
      new Pair("saddlebrown", [139, 69, 19]), // #8b4513
      new Pair("salmon", [250, 128, 114]), // #fa8072
      new Pair("sandybrown", [244, 164, 96]), // #f4a460
      new Pair("seagreen", [46, 139, 87]), // #2e8b57
      new Pair("seashell", [255, 245, 238]), // #fff5ee
      new Pair("sienna", [160, 82, 45]), // #a0522d
      new Pair("silver", [192, 192, 192]), // #c0c0c0
      new Pair("skyblue", [135, 206, 235]), // #87ceeb
      new Pair("slateblue", [106, 90, 205]), // #6a5acd
      new Pair("slategray", [112, 128, 144]), // #708090
      new Pair("slategrey", [112, 128, 144]), // #708090
      new Pair("snow", [255, 250, 250]), // #fffafa
      new Pair("springgreen", [0, 255, 127]), // #00ff7f
      new Pair("steelblue", [70, 130, 180]), // #4682b4
      new Pair("tan", [210, 180, 140]), // #d2b48c
      new Pair("teal", [0, 128, 128]), // #008080
      new Pair("thistle", [216, 191, 216]), // #d8bfd8
      new Pair("tomato", [255, 99, 71]), // #ff6347
      new Pair("turquoise", [64, 224, 208]), // #40e0d0
      new Pair("violet", [238, 130, 238]), // #ee82ee
      new Pair("wheat", [245, 222, 179]), // #f5deb3
      new Pair("white", [255, 255, 255]), // #ffffff
      new Pair("whitesmoke", [245, 245, 245]), // #f5f5f5
      new Pair("yellow", [255, 255, 0]), // #ffff00
      new Pair("yellowgreen", [154, 205, 50]), // #9acd32
    ]);
