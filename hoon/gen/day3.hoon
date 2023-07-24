/*  input  %txt  /input/day3_example/txt
:: /*  input  %txt  /input/day3/txt

:-  %say
|=  *
:-  %noun
=<
:: Parse the packs
%+  turn
input
|=  l=@t
=/  l    (trip l)
=/  len  (lent l)
[l=(parse-items (scag (div len 2) l)) r=(parse-items (slag (div len 2) l)) full=(parse-items l)]

|%
  +$  item  [p=@ud n=@t]
  +$  pack  [l=(list item) r=(list item) full=(list item)]
  ++  parse-items
      |=  input=tape
      %+  turn  input
      |=  l=@t
      =/  pri
      ?:  &((gte l 'a') (lte l 'z'))  `@ud`(sub l 96)
      ?:  &((gte l 'A') (lte l 'Z'))  `@ud`(sub l 38)
        ~&  >>>  "Error, unknown char: {<l>}"
        !!
      :-
        p=pri
      n=l
--
