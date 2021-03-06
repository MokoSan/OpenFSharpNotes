﻿// ================================================
// DDD Exercise: Refactoring designs to use states
// ================================================

(*
Much C# code has implicit states that you can recognize by fields called "IsSomething", or nullable date

This is a sign that states transitions are present but not being modelled properly.
*)

// Redesign this type into two states: RegisteredCustomer (with an id) and GuestCustomer (without an id)
// Anytime you see a boolean.. you are doing it wrong
type Customer_Before = 
    {
    CustomerName: string
    IsGuest: bool
    RegistrationId: int option
    }

type CustomerName = CustomerName of string
type RegistrationId = RegistrationId of int

type Customer_After = 
    | Guest of CustomerName
    | RegisteredCustomer of CustomerName * RegistrationId


// Exercise 3b - redesign this type into two states: Connected and Disconnected
type Connection_Before = 
   {
   IsConnected: bool
   ConnectionStartedUtc: System.DateTime option
   ConnectionHandle: int
   ReasonForDisconnection: string
   }


type ConnectionStartedUtc = System.DateTime
type ConnectionHandle = int
type ReasonForDisconnection = string 

type Connection_After = 
    | Connected of ConnectionStartedUtc * ConnectionHandle
    | Disconnected of ReasonForDisconnection

// Redesign this type into two states -- can you guess what the states
// are from the flags -- how does the refactored version help improve the documentation?
type Order_Before = 
   {
   OrderId: int 
   IsPaid: bool
   PaidAmount: float option
   PaidDate: System.DateTime option
   }

type OrderId = int
type PaidAmount = float option
type PaidDate = System.DateTime option

type Order_After = 
    | PaidOrder of OrderId * PaidAmount * PaidDate 
    | UnpaidOrder of OrderId