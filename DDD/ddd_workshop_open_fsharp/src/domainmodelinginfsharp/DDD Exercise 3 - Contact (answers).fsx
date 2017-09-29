// ================================================
// DDD Exercise: Model a Contact management system
//
// ================================================

(*
REQUIREMENTS

The Contact management system stores Contacts

A Contact has 
* a personal name
* an optional email address
* an optional postal address
* Rule: a contact must have an email or a postal address

A Personal Name consists of a first name, middle initial, last name
* Rule: the first name and last name are required
* Rule: the middle initial is optional
* Rule: the first name and last name must not be more than 50 chars
* Rule: the middle initial is exactly 1 char, if present

A postal address consists of a four address fields plus a country

Rule: An Email Address can be verified or unverified

*)

open System

// ----------------------------------------
// Helper module
// ----------------------------------------
module SimpleTypes =

    type String1 = private String1 of string
    
    module String1 =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                None               
            else if (s.Length <= 1) then
                Some (String1 s)
            else None

        let value (String1 s) = s

    type String50 = private String50 of string
    
    module String50 =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                None               
            else if (s.Length <= 50) then
                Some (String50 s)
            else None

        let value (String50 s) = s

    type EmailAddress = private EmailAddress of string
    
    module EmailAddress =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                None               
            else if s.Contains("@") then
                Some (EmailAddress s)
            else None

        let value (EmailAddress s) = s

    
// ----------------------------------------
// Main domain code
// ----------------------------------------

open SimpleTypes 

type VerifiedEmail = 
    VerifiedEmail of EmailAddress

type EmailContactInfo = 
    | Unverified of EmailAddress
    | Verified of VerifiedEmail

type PostalContactInfo = {
    Address1: String50 
    Address2: String50 
    Address3: String50 
    Address4: String50 
    Country: String50 
    }

type ContactInfo = 
    | EmailOnly of EmailContactInfo
    | AddrOnly of PostalContactInfo
    | EmailAndAddr of EmailContactInfo * PostalContactInfo

type PersonalName = {
    FirstName: String50
    MiddleInitial: String1 option
    LastName: String50 }

type Contact = {
    Name: PersonalName 
    ContactInfo : ContactInfo  }

