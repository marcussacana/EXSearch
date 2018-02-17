# Encoding X Searcher

This tool can search text using an unknown coding, for it to work you need to use a line that contains repetitions of a same letter in a non sequencial form, the more repetitions you have more precise will be the result, the tool even if not supporting cryptography can too be used to search text in "crypted" files by a simple XOR of one byte

# Strategy

The search use the character repetion and serach for a repetion with the same pattern of repetions
for example:

***"This is just a sample text"***
can match with
***"Yjod od kidy s dsqzçr yrcy"***

He matchs because the repetion pattern have the same distance and times in this line

- **T** and **Y**: Shows only one time at the position **0**
- **h** and **j**: Shows only one time at the position **1**
- **i** and **o**: Shows at the positions: **[2, 5]**
- **s** and **d**: Shows  at the positions: **[3, 6, 10, 15]**
- **j** and **k**: Shows only one time at the position **8**
- **u** and **i**: Shows only one time at the position **9**
- **t** and **y**: Shows at the positions: **[11, 22, 25]**
- **a** and **s**: Shows at the positions: **[13, 16]**
- **m** and **q**: Shows only one time at the position **17**
- **p** and **z**: Shows only one time at the position **18**
- **l** and **ç**: Shows only one time at the position **19**
- **e** and **r**: Shows at the positions: **[20, 23]**
- **x** and **c**: Shows only one time at the position **24**

And if all pattern match he will list the file and the match offset, If the file use a simple obfuscation like a static xor this tool will works too.
But keep in mind, strings like this:
"Nothing Here"
Only contains the repetion in the 'e', this will a false positive match's, you need choose a good string to a proper search.

## Why use this?
I created this tool to search the text inside game scripts who uses a personal encoding, since i don't know where is the text i just found a good string and give to this tool search, he will say where the string matchs and I can analyze the game encoding just putting a hardware breakpoint in the text.
