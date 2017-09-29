// ================================================
// DDD Exercise: Model a card game
//
// ================================================

(*

=== NOUNS ===

A card is a combination of a Suit (Heart, Spade) and a Rank (Two, Three, ... King, Ace)

A hand is a list of cards

A deck  is a list of cards

A player has a name and a hand

A game consists of a deck and list of players

=== VERBS ===

To shuffle a deck, start with any deck, afterwards, you have a shuffled deck. 

To deal, start with a shuffled deck, afterwards, you have a new shuffled deck and a card on the table

To pick up a card, start with your hand and a card on the table, , afterwards, you have a new hand

*)

module CardGame =

    type Suit = ??  // you take it from here!
    type Rank = ??
    type Card = ??

    type Hand = ??
    type Deck = ??

    type Player = ??
    type Game = ??

    type Shuffle = ??   // Do you need to create a new noun?
    type Deal = ??
    type PickupCard = ??

