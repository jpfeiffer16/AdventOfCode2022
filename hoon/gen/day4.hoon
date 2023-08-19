:: /*  input %txt  /input/day4/txt
/*  input  %txt  /input/day4_example/txt
:: (slag 123 |=(v v))


:-  %say
|=  *
:-  %noun
=<
  %+  turn  input
  |=  i=@t
  =/  sections  (split (trip i) ',')
  ?~  sections  ~&  >>>  "Error getting sections"  !!
  %+  turn  (need sections)
  |=  s=tape
  (split s '-')
|%
  ++  split
    |=  [i=tape l=@t]
    ^-  [~ ?(~ [l=tape r=tape])]
    =/  idx  (find ~[l] i)
    ?~  idx  [~ ~]
    :-  ~
    :-  l=(scag (need idx) i)
        r=(scag (need idx) i)
--
