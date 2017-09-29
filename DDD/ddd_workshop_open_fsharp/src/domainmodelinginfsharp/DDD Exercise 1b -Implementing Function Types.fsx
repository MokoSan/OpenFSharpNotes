(*
You can define a function as a type -- but how do you then implement that function to match the type?
*)


open System

// =====================
// example
// =====================

// definition
type AddOne = int -> int

// implementation
let addOne : AddOne =
    fun input -> input + 1

// definition
type ToLower = string -> string 

// implementation
let toLower : ToLower =
    fun input -> input.ToLower()



// =====================
// Card examples
// =====================

// definitions in domain model
type Suit = Club | Diamond | Spade | Heart
type Rank = Two | Three | Four | Five | Six | Seven | Eight 
                    | Nine | Ten | Jack | Queen | King | Ace
type Card = Suit * Rank
type Hand = Card list
type Deck = Card list
type ShuffledDeck = ShuffledDeck of Deck
type Player = {Name : string; Hand : Hand}
type Game = {Deck : Deck; Players : Player list}

type Shuffle = Deck -> ShuffledDeck 
type Deal = ShuffledDeck -> ShuffledDeck * Card
type PickupCard = Hand * Card -> Hand

// implementation
let shuffle : Shuffle =
    fun cards -> 
        let shuffledCards = cards |> List.sort // worst shuffle!
        ShuffledDeck shuffledCards 

// implementation
let deal : Deal =
    fun (ShuffledDeck cards) -> 
        match cards with
        | dealtCard::remaining -> (ShuffledDeck remaining,dealtCard)
        // can you find the bug?

// implementation
let pickupCard : PickupCard =
    fun (hand,card) -> 
        let newHand = card::hand
        newHand 
