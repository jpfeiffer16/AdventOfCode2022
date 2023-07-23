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
  :-  ply=(get-shape -:l)
  :-  res=(get-shape +>-:l)
  stg=(get-strategy +>-:l)

  :-
    %+  roll
    %+  turn  plays
    |=  play=round
    (get-score play)
    add
  %+  roll
  %+  turn  plays
  |=  play=round
  (apply-strategy play)
  add
|%
  +$  shape     ?(%rock %paper %scissors)
  +$  strategy  ?(%los %win %drw)
  +$  round     [ply=shape res=shape stg=strategy]

  ++  get-shape
    |=  shp=@t
    ^-  shape
    ?:  |(=(shp 'A') =(shp 'X'))  %rock
    ?:  |(=(shp 'B') =(shp 'Y'))  %paper
    ?:  |(=(shp 'C') =(shp 'Z'))  %scissors
    ~&  >>>  "Error: unable to parse shape {<shp>}"
    !!
  ++  get-strategy
    :: X - Loose
    :: Y - Draw
    :: Z - Win
    |=  stg=@t
    ^-  strategy
    ?:  =(stg 'X')  %los
    ?:  =(stg 'Y')  %drw
    ?:  =(stg 'Z')  %win
    ~&  >>>  "Error: unable to parse strategy {<stg>}"
    !!
  ++  apply-strategy
    |=  play=round
    ^-  @ud
    =/  result
    ?:  =(stg.play %drw)
      ply.play
    ?:  =(stg.play %win)
      ?-  ply.play
        %rock     %paper
        %paper    %scissors
        %scissors  %rock
      ==
    ?:  =(stg.play %los)
      ?-  ply.play
        %rock     %scissors
        %paper    %rock
        %scissors  %paper
      ==
    ~&  >>>  "Unable to play for strategy {<stg.play>}"
    !!
    %-  get-score
    :-  ply=ply.play
    :-  res=result
        stg=stg.play
  ++  get-score
    |=  round=round
    ^-  @ud
    %+  add
      ?-  res.round
        %rock      1
        %paper     2
        %scissors  3
      ==
    :: Could do a check for =(ply.round res.round) here and return
    :: 3 for a tie, but I wanted to use union switches.
    :: I naively believe this to be more performant. TODO: is this
    :: actually the case?
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
