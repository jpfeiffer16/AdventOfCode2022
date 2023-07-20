/*  input  %txt  /input/day1_example/txt
:-  %say
|=  *
:-  %noun
=<
  =/  groups=(list pack)  ~[*pack]
  =/  input=wain   input
  |-
  ^-  (list [t=@ud n=(list @ud)])
  ?~  input  groups
  =/  wrd  (trip -:input)
  ?:  =(wrd ~)
    %=  $
      input   +:input
      groups  [*pack groups]
    ==
  =/  cur     `pack`-:groups
  =/  parsed  (scan wrd dim:ag)
  %=  $
    input   +:input
    groups  [[t=(add t.cur parsed) n=[parsed n.cur]] +:groups]
  ==
|%
  +$  pack  [t=@ud n=(list @ud)]
--
