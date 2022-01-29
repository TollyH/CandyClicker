# Candy Clicker Save File

## Gameplay Save

All gameplay values saved by the application are stored as a list of 8 bytes (64 bits, unsigned, little endian) each in the following structure within `save_data.dat`:

| Byte Range      | Description                                      |
|-----------------|--------------------------------------------------|
|`0x00 - 0x07`    | Save Header (Currently the string `CdClk1.1`)    |
|`0x08 - 0x0F`    | Total Candy Score                                |
|`0x10 - 0x17`    | Candy Per Click                                  |
|`0x18 - 0x1F`    | Candy Per Second                                 |
|`0x20 - 0x27`    | Reincarnation Multiplier                         |
|`0x28 - 0x2F`    | Reincarnation Counter                            |
|`0x30 - 0x37`    | Overflow Counter                                 |
|`0x38 - 0x3F`    | Shop Multiplier                                  |
|`0x40 - ....`    | Shop Purchase Counters (See below)               |
|`Final 16 Bytes` | MD5 Hash of the save file (excluding the header) |

### Shop Purchase Counters

The number of times that each item in the shop has been purchased is stored from byte `0x38` onwards until, but not including, the final 16 bytes of the save file. Each value is stored in the same way as all the others in the file (8 bytes / 64 bits, unsigned, little endian), and they are stored in the same order that the shop items appear in-game. This means that in order to find the value for any one item, you navigate to byte offset `item_index * 0x08 + 0x38` where `item_index` is the 0-based index of the shop item as shown in-game, and read the next 8 bytes / 64 bits.

### Integrity Measures and Modification

If either the save header or the MD5 hash do not match their expected values, then the application will refuse to launch. If you wish to modify the save file, you will need a hex editor application such as [HxD](https://mh-nexus.de/en/hxd/). Ensure that you do not modify the first 8 bytes of the file, and once you are finished, you generate an MD5 hash of the new save file without the save header included, overwriting the old one. **Make sure that you insert the MD5 hash as raw bytes, not in plain text. It should be 16 bytes long and not appear as readable hex values in a standard text editor.**

## Customisations Save

| Bytes       | Description                       |
|-------------|-----------------------------------|
|`0x00`       | Background Red                    |
|`0x01`       | Background Green                  |
|`0x02`       | Background Blue                   |
|`0x03`       | Foreground Red                    |
|`0x04`       | Foreground Green                  |
|`0x05`       | Foreground Blue                   |
|`0x06 - EOF` | Image Path (Optional - See below) |

### Image Path

The full path to the image to use for the candy is stored as a string of UTF-8 bytes that runs until the end of the file. It is not terminated with any particular character.
