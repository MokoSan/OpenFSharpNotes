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



// ----------------------------------------
// Helper module
// ----------------------------------------
module SimpleTypes =

    type String1 = private String1 of string 

    module String1 =
      let create( s : string ) =
        if System.String.IsNullOrEmpty( s ) then None
        else if s.Length > 0 && s.Length <= 1 
          then Some ( String1 s )
        else None

      let get ( String1 s ) = s

    type String50 = private String50 of string 
    module String50 =
      let create ( s : string ) = 
        if System.String.IsNullOrEmpty( s ) then None
        else if s.Length > 0 && s.Length <= 50 
          then Some ( String50 s )
        else None

    type EmailString = private EmailString of string
    module EmailString = 
      let create ( s : string ) = 
        if System.String.IsNullOrEmpty( s ) then None
        else if s.Contains( "@") then Some ( EmailString s )
        else None

      let get ( EmailString s ) = s

    type VerifiedEmail = VerifiedEmail of EmailString 

    type EmailAddress = 
      | VerifiedEmail of VerifiedEmail 
      | UnverifiedEmail of EmailString 

    type Name = {
      FirstName: String50 
      MiddleInitial: String1 option
      LastName: String50 
    }

    type PostalAddress = {
      PostAddress1 : String50
      PostAddress2 : String50
      PostAddress3 : String50
      PostAddress4 : String50
      Country      : String50
    }

    type Contact =
      | EmailAddress of EmailAddress
      | PostalAddress of PostalAddress
      | EmailAndPostalAddress of EmailAddress * PostalAddress

// ----------------------------------------
// Main domain code
// ----------------------------------------

open SimpleTypes 

// this is what we DON'T want to do!
type BadContactDesign = {

  FirstName: string
  MiddleInitial: string
  LastName: string

  EmailAddress: string
  IsEmailVerified: bool 
  }


type Contact = { Name : Name; Contact : Contact } 