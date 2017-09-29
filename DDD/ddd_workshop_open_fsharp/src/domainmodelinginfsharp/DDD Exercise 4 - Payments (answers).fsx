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

After designing the types, create functions that will:

* print a payment method
* print a payment, including the amount
* create a new payment from an amount and method

*)

open System

module SimpleTypes =
    type CardType = Visa | Mastercard
    type CardNumber = CardNumber of string
    type ChequeNumber = ChequeNumber of int
    type BitcoinAddress = BitcoinAddress of string

    type EmailAddress = private EmailAddress of string
    
    module EmailAddress =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                None               
            else if s.Contains("@") then
                Some (EmailAddress s)
            else None

        let value (EmailAddress s) = s

    type PaymentAmount = private PaymentAmount of decimal

    module PaymentAmount =
        let create (d:decimal) = 
            if d <= 0M then 
                None               
            else 
                Some (PaymentAmount d)

        let value (PaymentAmount d) = d


// ----------------------------------------
// Main domain code
// ----------------------------------------

open SimpleTypes 

type PaymentMethod = 
    | Cash
    | Cheque of ChequeNumber 
    | Card of CardType * CardNumber
    | PayPal of EmailAddress 
    | Bitcoin of BitcoinAddress 



type Payment = {
    paymentMethod: PaymentMethod
    paymentAmount: PaymentAmount
    }


// ----------------------------------------
// Functions
// ----------------------------------------


type PrintPaymentMethod = PaymentMethod -> unit
type PrintPayment = Payment -> unit
type MakePayment = decimal -> PaymentMethod -> Payment option


// ----------------------------------------
// Implementation
// ----------------------------------------
    
   
let printPaymentMethod : PrintPaymentMethod = 
    fun paymentMethod -> 
    match paymentMethod with
    | Cash ->  printfn "Paid in cash"
    | Cheque checkNo -> printfn "Paid by cheque: %A" checkNo
    | Card (cardType,cardNo) -> printfn "Paid with %A %A" cardType cardNo
    | PayPal emailAddress -> printfn "Paid with PayPal %A" emailAddress
    | Bitcoin bitcoinAddress -> printfn "Paid with BitCoin %A" bitcoinAddress

let printPayment : PrintPayment = 
    fun payment ->
    let amount = payment.paymentAmount |> PaymentAmount.value
    printf "Amount: %g. " amount  // %g is used for decimals
    printPaymentMethod payment.paymentMethod

let makePayment : MakePayment =
    fun amount paymentMethod -> 
    let paOpt = PaymentAmount.create amount
    paOpt |> Option.map (fun pa -> {paymentMethod=paymentMethod; paymentAmount=pa} )

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

