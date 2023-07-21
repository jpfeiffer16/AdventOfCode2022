:: /*  input  %txt  /input/day2_example/txt
/*  input  %txt  /input/day2/txt

:: A, X - Rock
:: B, Y - Paper
:: C, Z - Scissors

:-  %say
|=  *
:-  %noun
=<
  =/  plays
  %+  turn  input
  |=  l=@t
  =/  l  (trip l)
  [ply=(get-shape -:l) res=(get-shape +>-:l)]

  %+  roll
  %+  turn  plays
  |=  play=round
  (get-score play)
  add
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
    %+  add
      ?-  res.round
        %rock      1
        %paper     2
        %scissors  3
      ==
    ?:  =(ply.round %rock)
      ?-  res.round
        %rock      3
        %paper     6
        %scissors  0
      ==
    ?:  =(ply.round %paper)
      ?-  res.round
        %rock      0
        %paper     3
        %scissors  6
      ==
    ?:  =(ply.round %scissors)
      ?-  res.round
        %rock      6
        %paper     0
        %scissors  3
      ==
    ~&  >>>  "Error: unable to get score {<round>}"
    !!
--
