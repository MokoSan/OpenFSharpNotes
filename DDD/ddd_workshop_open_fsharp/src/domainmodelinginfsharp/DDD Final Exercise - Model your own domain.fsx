// 1) Start with the domain types that are independent of state

type CartId = int
type Product = string     // placeholder for now
type CartContents = Product list  // placeholder for now 
type Payment = float     // placeholder for now 

// 2) Create a type to represent the data stored for each type

// type EmptyCartData = not needed
type ActiveCartData = CartContents 
type PaidCartData = CartContents * Payment

// 3) Create a type that represent the choice of all the states

type ShoppingCart = 
    | EmptyCartState of CartId 
    | ActiveCartState of CartId * ActiveCartData 
    | PaidCartState of CartId * PaidCartData

type Command =
    | Add of Product
    | Remove of Product
    | Pay of Payment

type CommandWithId = 
    {
    CartId: CartId
    Command: Command
    }

//type Command =
//    | Add of CartId * Product
//    | Remove of CartId * Product
//    | Pay of CartId * Payment

type CommandFailureReason = 
    | CartIsEmpty
    | NotAllowedToBuy

type CartContentsChangedEvent = CartId * Product
type PaidItemEvent = CartId * Payment

type Event =
    | Added of CartContentsChangedEvent 
    | Removed of CartContentsChangedEvent 
    | Paid of PaidItemEvent

type CommandResponse = 
    | Event of Event list
    | Failure of CommandFailureReason

type CommandHandler = Command -> CommandResponse

let commandHandler commandWithId =
    let {CartId=cartId;Command=command} = commandWithId
    match command with
    | Add product -> Event [Added (cartId,product)]
    | Remove product -> 
        if product = "bad" then
            Failure NotAllowedToBuy
        else
            Event [Removed (cartId,product)]
    | Pay cartId -> Failure NotAllowedToBuy

let (|IsEven|IsOdd|IsZero|NotANumber|) x=
    match System.Int32.TryParse(x) with
    | true,i -> 
        if i = 0 then 
            IsZero 
        else if i%2 = 0 then 
            IsEven 
        else 
            IsOdd
    | false, _ -> NotANumber

match "0" with
| IsEven -> "is even"
| IsOdd -> "is odd"
| IsZero -> "zero"
| NotANumber -> "NaN"

System.Environment.CurrentDirectory

x $ y . z >>= q