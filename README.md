reloc
=====

A simple DOS/CLI utility that resizes and relocates the current command prompt window to various locations on the screen based on the specified arguments (or whatever is saved in its config).

    USAGE:

        reloc.exe [--config][--clear] direction

      Supported Directions:

        7|tl            8|up|top        9|tr
        _____________   _____________   _____________
        ¦¦¦¦¦¦      ¦   ¦¦¦¦¦¦¦¦¦¦¦¦¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦¦¦¦¦¦¦¦¦¦¦¦¦   ¦      ¦¦¦¦¦¦
        ¦           ¦   ¦           ¦   ¦           ¦
        ¦           ¦   ¦           ¦   ¦           ¦
        ¦           ¦   ¦           ¦   ¦           ¦
        ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯

        4|left|west     5|center        6|right|east
        _____________   _____________   _____________
        ¦¦¦¦¦¦      ¦   ¦           ¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦   _____   ¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦   ¦¦¦¦¦   ¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦   ¯¯¯¯¯   ¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦           ¦   ¦      ¦¦¦¦¦¦
        ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯

        1|bl            2|down|bottom   3|br
        _____________   _____________   _____________
        ¦           ¦   ¦           ¦   ¦           ¦
        ¦           ¦   ¦           ¦   ¦           ¦
        ¦           ¦   ¦           ¦   ¦           ¦
        ¦¦¦¦¦¦      ¦   ¦¦¦¦¦¦¦¦¦¦¦¦¦   ¦      ¦¦¦¦¦¦
        ¦¦¦¦¦¦      ¦   ¦¦¦¦¦¦¦¦¦¦¦¦¦   ¦      ¦¦¦¦¦¦
        ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯   ¯¯¯¯¯¯¯¯¯¯¯¯¯

        10|max|x
        _____________
        ¦¦¦¦¦¦¦¦¦¦¦¦¦
        ¦¦¦¦¦¦¦¦¦¦¦¦¦
        ¦¦¦¦¦¦¦¦¦¦¦¦¦
        ¦¦¦¦¦¦¦¦¦¦¦¦¦
        ¦¦¦¦¦¦¦¦¦¦¦¦¦
        ¯¯¯¯¯¯¯¯¯¯¯¯¯

      --clear            » Clears the values in config.
      --config           » When used with direction, the direction saved to config. When used by itself, the config values are displayed.
                         » The position of the `--config` option does not matter in relation to the direction.

    EXAMPLES:

      > reloc max        » Maximizes the console window.
      > reloc 10         » Maximizes the console window.
      > reloc up         » Resizes the console and positions at the top of the screen.
      > reloc br         » Resizes the console and positions at the bottom right corner of the screen.

      > reloc --config   » Displays current config values.
      > reloc l          » Relocates to the left edge and saves to config.
      