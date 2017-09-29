// if using in Visual Studio, this helps to set the current directory correctly
System.IO.Directory.SetCurrentDirectory __SOURCE_DIRECTORY__

// load the Railway Oriented Programming utility library
// See http://fsharpforfunandprofit.com/rop
// See https://github.com/swlaschin/Railway-Oriented-Programming-Example

#load "ResultLibrary.fsx"
open ResultLibrary

// ==============================================
// Set up the simple types: String10, EmailAddress, etc
// ==============================================

open System

module SimpleTypes =

    // ------------------------------
    // String10
    type String10 = private String10 of string
    
    module String10 =
        let create fieldName (s:string) = 
            if String.IsNullOrEmpty(s) then 
                Validation.fail (fieldName + " must not be null or empty")
            else if (s.Length <= 10) then
                Validation.succeed (String10 s)
            else 
                Validation.fail (fieldName + " is more than 10 chars")

        let value (String10 s) = s


    // ------------------------------
    // EmailAddress

    type EmailAddress = private EmailAddress of string
    
    module EmailAddress =
        let create (s:string) = 
            if String.IsNullOrEmpty(s) then 
                Validation.fail "Email must not be null or empty"
            else if s.Contains("@") then
                Validation.succeed (EmailAddress s)
            else 
                Validation.fail "Email does not contain an @"

        let value (EmailAddress s) = s


    // ------------------------------
    // ContactId

    // NOTE: this type would normally be opaque
    type ContactId = private ContactId of int

    module ContactId =
        let create (i:int) = 
            if i < 1 then
                Validation.fail "ContactId must be positive integer"
            else 
                Validation.succeed (ContactId i)

        let value (ContactId i) = i

// ==============================================
// Set up the domain level types: PersonalName, Contact
// ==============================================

module ContactDomain = 
    open SimpleTypes

    // NOTE: these types do NOT have to be opaque
    type PersonalName = {
        FirstName: String10
        LastName: String10 
    }

    type Contact = {
        Id: ContactId
        Name: PersonalName
        Email: EmailAddress
    }

    let createFirstName firstName = 
        let fieldName = "First name"
        String10.create fieldName firstName
    
    let createLastName lastName = 
        let fieldName = "Last name"
        String10.create fieldName lastName 

    let createPersonalName firstName lastName = 
        {FirstName = firstName; LastName = lastName}

    let createContact custId name email = 
        {Id = custId; Name = name; Email = email}


// ==============================================
// Set up the DTO types: PersonalName, Contact
// ==============================================

module ContactDTO =
    open SimpleTypes
    open ContactDomain

    /// Represents a DTO that is exposed on the wire.
    /// This is a regular POCO class which can be null. 
    /// To emulate the C# class, all the properties are initialized to null by default
    ///
    /// Note that in F# you have to make quite an effort to create nullable classes with nullable fields
    [<AllowNullLiteralAttribute>]
    type ContactDto() = 
        member val Id = 0 with get, set
        member val FirstName : string = null with get, set
        member val LastName : string = null with get, set
        member val Email : string  = null with get, set

    /// Convert a domain Contact into a DTO.
    /// There is no possibility of an error 
    /// because the Contact type has stricter constraints than DTO.
    let contactToDto(cust:Contact) =
        // extract the raw int id from the ContactId wrapper
        let custIdInt = cust.Id |> ContactId.value

        // create the object and set the properties
        let contactDto = ContactDto()
        contactDto.Id <- custIdInt 
        contactDto.FirstName <- cust.Name.FirstName |> String10.value
        contactDto.LastName <- cust.Name.LastName |> String10.value
        contactDto.Email <- cust.Email |> EmailAddress.value
        contactDto

    /// Convert a DTO into a domain contact.
    ///
    /// We MUST handle the possibility of one or more errors
    /// because the Contact type has stricter constraints than ContactDto
    /// and the conversion might fail.
    let dtoToContact (dto: ContactDto) = 
        if dto = null then 
            Validation.fail "Contact is required"
        else
            // This is an example of the power of composition!
            // Each step returns a value OR an error.
            // These are then gradually combined to make bigger things, all the while preserving any errors 
            // that happen.

            // if the id is not valid, the createContactId function will return a Failure
            // hover over idOrError and you can see it has type RopResult<ContactId,DomainEvent> rather than just ContactId
            let idOrError = ContactId.create dto.Id

            // similarly for first and last name
            let firstNameOrError = createFirstName dto.FirstName
            let lastNameOrError = createLastName dto.LastName


            // The <!> and <*> operators make it look complicated, but in fact it is always the same pattern.
            //  <!> is used for the first param
            //  <*> is used for the subsequent params
            let (<*>) = Validation.apply
            let (<!>) = Validation.map

            // the "createPersonalName" functions takes normal inputs, not inputs with errors,
            // but it can be "lifted" to the Validation type
            let personalNameOrError = createPersonalName <!> firstNameOrError <*> lastNameOrError 

            // similarly try to make an email
            let emailOrError = EmailAddress.create dto.Email

            // finally add them all together to make a contacts
            // the "createContact" takes three params, so use lift3 to convert it
            let contactOrError = createContact <!> idOrError <*> personalNameOrError <*> emailOrError
            contactOrError 


// ==============================================
// examples
// ==============================================
open ContactDomain
open ContactDTO

let goodDto = ContactDto(Id=1,FirstName="Alice",LastName="Adams",Email="me@example.com")
goodDto |> ContactDTO.dtoToContact

let badDto = ContactDto(Id=0,FirstName=null,LastName="Adams",Email="xample.com")
badDto |> ContactDTO.dtoToContact
