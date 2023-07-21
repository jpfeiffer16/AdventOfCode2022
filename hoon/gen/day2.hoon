/*  input  %txt  /input/day2_example/txt

:: A, X - Rock
:: B, Y - Paper
:: C, Z - Scissors

:-  %say
|=  *
:-  %noun
=<
  %+  turn  input
  |=  l=@t
  =/  l  (trip l)
  [ply=(get-shape -:l) res=(get-shape +>-:l)]
|%
  +$  shape  ?(%rock %paper %scissors)
  +$  round  [ply=shape res=shape]
  ++  get-shape
    |=  shp=@t
    ^-  shape
    ?:  |(=(shp 'A') =(shp 'X'))  %rock
    ?:  |(=(shp 'B') =(shp 'Y'))  %paper
    ?:  |(=(shp 'C') =(shp 'Z'))  %scissors
    ~&  >>>  "Error: unable to parse shape {<shp>}"
    !!
  ++  get-score
    |=  round=round
    ^-  @ud
    !!
--
