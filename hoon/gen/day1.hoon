:: /*  input  %txt  /input/day1_example/txt
/*  input  %txt  /input/day1/txt
:-  %say
|=  *
:-  %noun
=>
  |%
    +$  pack  [t=@ud n=(list @ud)]
  --
=/  sorted-packs
%+  sort
  %+  turn
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
  |=  [n=@ud *]  n
gth

:-  one=(head sorted-packs)
    three=(roll (scag 3 sorted-packs) add)
