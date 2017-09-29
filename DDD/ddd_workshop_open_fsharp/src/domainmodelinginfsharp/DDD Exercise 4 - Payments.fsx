// ================================================
// DDD Exercise: Model a payment taking system
//
// ================================================

(*
The payment taking system should accept the followinhg payment methods
* Cash
* Credit cards
* Cheques
* Paypal
* Bitcoin

For cash, no extra info is required
For cheques, a check number (int) is required
For cards, a card number and card type is required
  * A card type is one of Visa, Mastercard, Amex.
For paypal, an email address is required
for bitcoin, a bitcoin address is required

A payment consists of:
* a payment method
* a payment amount

A payment amount is a non-negative decimal

After designing the types, define the types of functions that will:

* print a payment method
* print a payment, including the amount
* create a new payment from an decimal amount and a payment method


*)

open System

module SimpleTypes = 
    type CardType = Visa | Mastercard | Discover | AmericanExpress 
    type CardNumber = CardNumber of string

    type ChequeNumber = int 
    type ChequeNumber = Cheque of ChequeNumber 

    type EmailAddress = EmailAddress of string 

    type BitcoinAddress = BitcoinAddress of string 
    type PaymentAmount = ??

// ----------------------------------------
// Main domain code
// ----------------------------------------

open SimpleTypes 

type PaymentMethod = 
    | Cash
    | Cheque of ChequeNumber 
    | Card of ??
    | PayPal of ??
    | Bitcoin of ??


type Payment = ??


// ----------------------------------------
// Functions
// ----------------------------------------


type PrintPaymentMethod = PaymentMethod -> unit
type PrintPayment = ??
type MakePayment = ??

// ----------------------------------------
// Implementation
// ----------------------------------------
    
let printPaymentMethod : PrintPaymentMethod = 
    fun paymentMethod -> 
    ??

let printPayment : PrintPayment = 
    fun payment -> 
    ??

let makePayment : MakePayment =
    fun amount paymentMethod -> 
    ??

// ----------------------------------------
// Examples - make these work!
// ----------------------------------------

let paymentMethod1 = Cash
let paymentMethod2 = Cheque (ChequeNumber 42)
let paymentMethod3 = Card (Visa, CardNumber "1234")
let emailAddress = EmailAddress.create "me@example.com" |> Option.get // never do this in production!
let paymentMethod4 = PayPal emailAddress 
let paymentMethod5 = Bitcoin (BitcoinAddress "1234")

// highlight and run
printPaymentMethod paymentMethod1
printPaymentMethod paymentMethod2
printPaymentMethod paymentMethod3
printPaymentMethod paymentMethod4
printPaymentMethod paymentMethod5


let payment1Opt = makePayment 42M paymentMethod1 
let payment2Opt = makePayment 42M paymentMethod2 
let payment3Opt = makePayment 42M paymentMethod3 

payment1Opt |> Option.map printPayment 
payment2Opt |> Option.map printPayment 
payment3Opt |> Option.map printPayment 
