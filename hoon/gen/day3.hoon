/*  input  %txt  /input/day3_example/txt
:: /*  input  %txt  /input/day3/txt

:-  %say
|=  *
:-  %noun
=<
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
%+  turn
  packs
|=  pack=pack
%-  head
%+  skim  l.pack
|=  left-item=item
%+  contains  r.pack
|=  right-item=item
:: TODO: Could save a lot of time here and use a (map @t @ud)
=(n.left-item n.right-item)


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


:: :-  %say
:: |=  *
:: :-  %noun
:: =<
::   =/  test=(list [l=(list [id=@ud])])  ~[[l=~[[id=123]]] [l=~[[id=321]]]]
::   ^-  (list [l=(list [id=@ud])])
::   %+  skim  test
::   |=  i=[l=(list @ud)]
::   :: %+  contains  `(list @ud)`[123 321 ~]
::   :: |=  i=@ud  =(i 123)
::   %+  contains  `(list [id=@ud])`l.i
::   |=(item=[id=@ud] =(id.item 123))
:: |%
::   ++  contains
::     |*  [l=(list) gate=$-(* ?)]
::     |-  ?:  =(l ~)      %|
::         ?:  (gate -:l)  %&
::       $(l +:l)
:: --


:: =/  a [b=123]
:: (wing b a)
:: !,(*hoon b.a)
