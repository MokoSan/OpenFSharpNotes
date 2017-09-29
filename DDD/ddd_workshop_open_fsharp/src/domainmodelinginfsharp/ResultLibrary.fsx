//==============================================
// Helpers for Result type, Validation type and AsyncResult type
//==============================================

// The "Result" type is built-in to F# 4.1 and newer,
// so comment this out if you are using these versions

/// Result represents a choice between success and failure
type Result<'success, 'failure> = 
    | Ok of 'success
    | Error of 'failure


/// Functions for Result type (functor and monad).
/// For applicatives, see Validation.
[<RequireQualifiedAccess>]
module Result =

    /// Use a function for each case
    let bimap onSuccess onError xR = 
        match xR with
        | Ok x -> onSuccess x
        | Error err -> onError err

    // In F# 4.1 and newer, map, mapError and bind are built-in functions,
    // so comment out these implementations.
    let map f result = 
        match result with
        | Ok success -> Ok (f success)
        | Error err -> Error err 

    let mapError f result = 
        match result with
        | Ok success -> Ok success
        | Error err -> Error (f err)

    let bind f result = 
        match result with
        | Ok success -> f success
        | Error err -> Error err

    // In F# 4.1 and newer, uncomment these aliases
    // let map = Result.map
    // let mapError = Result.mapError
    // let bind = Result.bind

    let iter (f : _ -> unit) result = 
        map f result |> ignore    

    /// Apply a Result<fn> to a Result<x> monadically
    let apply fR xR = 
        match fR, xR with
        | Ok f, Ok x -> Ok (f x)
        | Error err1, Ok _ -> Error err1
        | Ok _, Error err2 -> Error err2
        | Error err1, Error _ -> Error err1 


    // combine a list of results, monadically
    let sequence aListOfResults = 
        let (<*>) = apply // monadic
        let (<!>) = map
        let cons head tail = head::tail
        let consR headR tailR = cons <!> headR <*> tailR
        let initialValue = Ok [] // empty list inside Result
 
        // loop through the list, prepending each element
        // to the initial value
        List.foldBack consR aListOfResults initialValue



    //-----------------------------------
    // Lifting

    /// Lift a two parameter function to use Result parameters
    let lift2 f x1 x2 = 
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2

    /// Lift a three parameter function to use Result parameters
    let lift3 f x1 x2 x3 = 
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3

    /// Lift a four parameter function to use Result parameters
    let lift4 f x1 x2 x3 x4 = 
        let (<!>) = map
        let (<*>) = apply
        f <!> x1 <*> x2 <*> x3 <*> x4

    /// Apply a monadic function with two parameters 
    let bind2 f x1 x2 = lift2 f x1 x2 |> bind id

    /// Apply a monadic function with three parameters
    let bind3 f x1 x2 x3 = lift3 f x1 x2 x3 |> bind id

    //-----------------------------------
    // Predicates

    /// Predicate that returns true on success
    let isOk = 
        function 
        | Ok _ -> true
        | Error _ -> false

    /// Predicate that returns true on failure
    let isError xR = 
        xR |> isOk |> not

    /// Lift a given predicate into a predicate that works on Results
    let filter pred = 
        function 
        | Ok x -> pred x
        | Error _ -> true


    //-----------------------------------
    // Mixing simple values and results

    /// Return a value for the failure case
    let ifError defaultVal = 
        function 
        | Ok x -> x
        | Error _ -> defaultVal


    //-----------------------------------
    // Mixing options and results

    /// Apply a monadic function to an Result<x option>
    let bindOption f xR =
        match xR with
        | Some x -> f x |> map Some
        | None -> Ok None

    /// Convert an Option into a Result. If none, use the passed-in errorValue 
    let ofOption errorValue opt = 
        match opt with
        | Some v -> Ok v
        | None -> Error errorValue

    /// Convert an Result into an Option 
    let toOption xR = 
        match xR with
        | Ok v -> Some v
        | Error _ -> None

    /// Convert the Error case into an Option (useful for List.choose)
    let toErrorOption = 
        function 
        | Ok _ -> None
        | Error err -> Some err


//==============================================
// Computation Expression for Result
//==============================================

[<AutoOpen>]
module ResultComputationExpression =

    type ResultBuilder() =
        member __.Return(x) = Ok x
        member __.Bind(x, f) = Result.bind f x
    
        member __.ReturnFrom(x) = x
        member this.Zero() = this.Return ()

        member __.Delay(f) = f
        member __.Run(f) = f()

        member this.While(guard, body) =
            if not (guard()) 
            then this.Zero() 
            else this.Bind( body(), fun () -> 
                this.While(guard, body))  

        member this.TryWith(body, handler) =
            try this.ReturnFrom(body())
            with e -> handler e

        member this.TryFinally(body, compensation) =
            try this.ReturnFrom(body())
            finally compensation() 

        member this.Using(disposable:#System.IDisposable, body) =
            let body' = fun () -> body disposable
            this.TryFinally(body', fun () -> 
                match disposable with 
                    | null -> () 
                    | disp -> disp.Dispose())

        member this.For(sequence:seq<_>, body) =
            this.Using(sequence.GetEnumerator(),fun enum -> 
                this.While(enum.MoveNext, 
                    this.Delay(fun () -> body enum.Current)))

        member this.Combine (a,b) = 
            this.Bind(a, fun () -> b())

    let result = new ResultBuilder()

//==============================================
// Validation is Result with a list for failures
// to allow applicatives to be used
//==============================================

type Validation<'Success,'Failure> = 
    Result<'Success,'Failure list>

/// Functions for Validation type (mostly applicative)
[<RequireQualifiedAccess>]
module Validation =

    let succeed x : Validation<_,_> = Ok x
    let fail err : Validation<_,_> = Error [err]

    let map = Result.map

    /// Apply a Validation<fn> to a Validation<x> applicatively
    let apply (fV:Validation<_,_>) (xV:Validation<_,_>) :Validation<_,_> = 
        match fV, xV with
        | Ok f, Ok x -> Ok (f x)
        | Error errs1, Ok _ -> Error errs1
        | Ok _, Error errs2 -> Error errs2
        | Error errs1, Error errs2 -> Error (errs1 @ errs2)

    // combine a list of Validation, applicatively
    let sequence (aListOfValidations:Validation<_,_> list) = 
        let (<*>) = apply
        let (<!>) = Result.map
        let cons head tail = head::tail
        let consR headR tailR = cons <!> headR <*> tailR
        let initialValue = Ok [] // empty list inside Result
  
        // loop through the list, prepending each element
        // to the initial value
        List.foldBack consR aListOfValidations initialValue

    //-----------------------------------
    // Converting between Validations and other types
    
    let ofResult xR :Validation<_,_> = 
        xR |> Result.mapError List.singleton

    let toResult (xV:Validation<_,_>) :Result<_,_> = 
        xV


