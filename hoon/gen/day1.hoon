/*  input  %txt  /input/day1_example/txt
:-  %say
|=  *
:-  %noun
=/  groups=(list [t=@ud n=(list @ud)])  ~[[t=0 n=*(list @ud)]]
=/  input=wain                          input
|-
^-  (list [t=@ud n=(list @ud)])
?~  input  groups
=/  wrd  (trip -:input)
?:  =(wrd ~)
  :: ~&  [[t=0 n=*(list @ud)] groups]
  %=  $
    input   +:input
    groups  [[t=0 n=*(list @ud)] groups]
  ==
=/  cur     `[t=@ud n=(list @ud)]`-:groups
=/  parsed  (scan wrd dim:ag)
%=  $
  input   +:input
  groups  [[t=(add t.cur parsed) n=[parsed n.cur]] +:groups]
==
