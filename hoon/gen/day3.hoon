/*  input  %txt  /input/day3_example/txt
:: /*  input  %txt  /input/day3/txt

:-  %say
|=  *
:-  %noun
=>
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
        [p=pri n=l]
    ++  contains
      |*  [l=(list) gate=$-(* ?)]
      |-  ?:  =(l ~)      %|
          ?:  (gate -:l)  %&
        $(l +:l)
  --
:: Parse the packs
=/  packs
^-  (list pack)
%+  turn
input
|=  l=@t
=/  l    (trip l)
=/  len  (lent l)
:-  ^=  l      (parse-items (scag (div len 2) l))
:-  ^=  r      (parse-items (slag (div len 2) l))
    ^=  full   (parse-items l)

:: TODO: Use the proper pretty printing here to make this look good
:: Part 1
:-
^=  part-1
  %+  roll
    %+  turn
      packs
    |=  pack=pack
    =<  p
    %-  head
    %+  skim  l.pack
    |=  left-item=item
    %+  contains  r.pack
    |=  right-item=item
    :: TODO: Could save a lot of time here and use a (map @t @ud)
    =(n.left-item n.right-item)
  add

:: Part 2
^=  part-2
  =-
    %-  roll
    :_  add
    %+  turn  `(list (list pack))`groups
    |=  group=(list pack)
    =<  p
    %-  head
    %+  skim  =<(full ^-(pack (head group)))
    |=  item=item
    ?&
      (contains =<(full `pack`+<:group) |=(i=^item =(n.i n.item)))
      (contains =<(full `pack`+>-:group) |=(i=^item =(n.i n.item)))
    ==
  =/  grouped=(list (list pack))  ~
  :*  ^=  groups
      ^-  (list (list pack))
      |-  ?~  packs  grouped
      %=  $
        grouped  [(scag 3 `(list pack)`packs) grouped]
        packs    +>+:packs
      ==
  ==
